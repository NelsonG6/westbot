using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;
using Discord;
using System.Data.SqlClient;
using WestBot;
using System.Data;
using System.Linq;


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
            //Announce the join in general chat
            try
            {
                ulong GeneralChatChannel = DatabaseHandler.GetChannelID("General");
                IMessageChannel channel = (IMessageChannel)user.Guild.Channels.FirstOrDefault(x => x.Id == GeneralChatChannel);
                await channel.SendMessageAsync($"Welcome, <@{user.Id}>!");
            }
            catch (Exception ex)
            {
                Console.Write("Exception: " + ex.Message);
            }

            //Announce the channel in the user join channel, if one exists
            try
            {
                ulong UserJoinChannel = DatabaseHandler.GetChannelID("UserJoin");
                IMessageChannel channel = (IMessageChannel)user.Guild.Channels.FirstOrDefault(x => x.Id == UserJoinChannel);
                await channel.SendMessageAsync($"<@{user.Id}> has joined.");
            }
            catch (Exception ex)
            {
                Console.Write("Exception: " + ex.Message);
            }
        }

        public async Task InstallCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            _client.UserJoined += AnnounceJoinedUser;
            //_client.GuildAvailable += BotConfiguration.LoadChannels;
            _commands.CommandExecuted += OnCommandExecutedAsync;
            await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(),
                                            services: _services);
        }

        public async Task HandleCommandAsync(SocketMessage msg)
        {
            //Not sure what this does
            if (!(msg is SocketUserMessage message)) return;
            if (msg.Source != MessageSource.User) return;

            char[] messageArray = message.Content.ToCharArray();

            int argPos = 0;
            char prefix = BotConfiguration.Prefix;

            if (!
               (message.Content.ToCharArray()[argPos] == prefix ||
                message.Author.IsBot)
                )
                return;
            
            //Check if the first two characters are a period. If so, don't take the command.
            if(messageArray[0] == BotConfiguration.Prefix && messageArray[1] == BotConfiguration.Prefix)
                return;

            //Skip extra spaces that come after the prefix
            while (messageArray[argPos + 1] == ' ')
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
            bool error_state = false;

            switch (result)
            {
                case WestbotCommandResult customResult:
                    if (!customResult.Get_reaction())
                        break; //no reaction needed

                    if (customResult.IsSuccess)
                        error_state = AcceptState.Accept;
                    else
                        error_state = AcceptState.Error;

                    break;
                default:
                    if (!string.IsNullOrEmpty(result.ErrorReason))
                        error_state = AcceptState.Error;
                    else
                        error_state = AcceptState.Accept;
                    break;
            }

            string reaction_type;
            if (!error_state)
                reaction_type = "Accept";
            else
                reaction_type = "Error";

            int ReturnValue = DatabaseHandler.EmoteOrEmoji(reaction_type);

            if (ReturnValue == 0)
            {   //Emojis
                string ReturnString = DatabaseHandler.GetRandomEmoji(reaction_type);

                Emoji emoji_to_add = new Emoji(ReturnString);
                await context.Message.AddReactionAsync(emoji_to_add);
            }
            else
            {   //emotes

                long ReturnID = DatabaseHandler.GetRandomEmote(reaction_type);
                UInt64 ResultID = (UInt64)ReturnID;

                IEmote emote_to_add = context.Guild.Emotes.FirstOrDefault(x => x.Id == ResultID);
                //var emote_to_add = Context.Guild.GetEmoteAsync(result);

                await context.Message.AddReactionAsync((IEmote)emote_to_add);
            }

        }
    }
}
