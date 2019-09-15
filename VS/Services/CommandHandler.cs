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
            //Calling getchannelname with "user join" will get the name of this local channel's user join channel
            IMessageChannel channel = (IMessageChannel)user.Guild.Channels.FirstOrDefault(x => x.Id == BotConfiguration.GetChannelID("General", user.Guild.Id));

            await channel.SendMessageAsync($"Welcome, <@{user.Id}>!");

            try
            {
                using (SqlConnection conn = new SqlConnection(MySQLConnString.Get()))
                {
                    SqlCommand command = new SqlCommand("GetStreamChannelID", conn);
                    SqlParameter returnValue = new SqlParameter("@result", SqlDbType.BigInt);
                    returnValue.Direction = ParameterDirection.Output;
                    command.Parameters.Add(returnValue);
                    command.Parameters.Add(new SqlParameter("@serverID", user.Guild.Id.ToString()));
                    command.Parameters.Add(new SqlParameter("@target_channel", "Stream"));

                    conn.Open();
                    command.CommandType = CommandType.StoredProcedure;
                    command.ExecuteNonQuery();

                    long result2 = (long)returnValue.Value;
                    UInt64 result = (UInt64)result2;

                    IMessageChannel general_channel = (IMessageChannel)user.Guild.Channels.FirstOrDefault(x => x.Id == result);
                    await general_channel.SendMessageAsync(user.Mention + " has joined.");
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return;
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
            if (!(msg is SocketUserMessage message)) return;
            if (msg.Source != MessageSource.User) return;

            int argPos = 0;
            char prefix = BotConfiguration.Prefix;

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
                        await BotConfiguration.GenerateReaction(context, AcceptState.Accept);
                    else
                        await BotConfiguration.GenerateReaction(context, AcceptState.Error);

                    break;
                default:
                    if (!string.IsNullOrEmpty(result.ErrorReason))
                        await BotConfiguration.GenerateReaction(context, AcceptState.Error);
                    else
                        await BotConfiguration.GenerateReaction(context, AcceptState.Accept);
                    break;
            }
        }
    }
}
