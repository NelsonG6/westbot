using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;
using Discord;
using Microsoft.Extensions.DependencyInjection;

namespace Westbot.Services
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;

        public CommandHandler(DiscordSocketClient client, CommandService commands, IServiceProvider services)
        {
            _commands = commands;
            _client = client;
            _services = services;
        }

        public async Task AnnounceJoinedUser(SocketGuildUser user) //Welcomes the new user
        {
            //Calling getchannelname with "user join" will get the name of this local channel's user join channel
            await CurrentConfiguration.GetChannel("General").SendMessageAsync($"Welcome, <@{user.Id}>!");
            await CurrentConfiguration.GetChannel("User Join").SendMessageAsync(user.Mention + " has joined.");
        }

        public async Task InstallCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            _client.UserJoined += AnnounceJoinedUser;
            _client.GuildAvailable += CurrentConfiguration.LoadChannels;
            _commands.CommandExecuted += OnCommandExecutedAsync;
            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(),
                                            services: _services);
        }

        public async Task HandleCommandAsync(SocketMessage msg)
        {
            if (!(msg is SocketUserMessage message)) return;
            if (msg.Source != MessageSource.User) return;

            int argPos = 0;
            char prefix = CurrentConfiguration.Prefix;

            if (!
               (message.Content.ToCharArray()[argPos] == prefix ||
                message.Author.IsBot)
                )
                return;
            
            //Skip extra spaces that come after the prefix
            while (message.Content.ToCharArray()[argPos + 1] == ' ')
            {
                ++argPos;
            }

            //argposition now points to the character that starts the command
            ++argPos;
            
            var context = new SocketCommandContext(_client, message);
            await _commands.ExecuteAsync(context, argPos, _services);
        }

        public async Task OnCommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            switch (result)
            {
                case WestbotCommandResult customResult:
                    if (!customResult.Get_reaction())
                        break; //no reaction needed

                    if (customResult.IsSuccess)
                        await CurrentConfiguration.GenerateReaction(context, AcceptState.Accept);
                    else
                        await CurrentConfiguration.GenerateReaction(context, AcceptState.Error);

                    break;
                default:
                    if (!string.IsNullOrEmpty(result.ErrorReason))
                        await CurrentConfiguration.GenerateReaction(context, AcceptState.Error);
                    else
                        await CurrentConfiguration.GenerateReaction(context, AcceptState.Accept);
                    break;
            }
        }
    }
}
