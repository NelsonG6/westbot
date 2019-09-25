using System;
using System.Threading.Tasks;
using Discord.Commands;
using Westbot;
using Westbot.Preconditions;
using System.Data.SqlClient;
using Discord;

namespace WestBot
{
    class DatabaseFeatures
    {
        [Name("Random features for the database.")]

        public class PrivateChannel : ModuleBase<SocketCommandContext>
        {
            [Command("Setup"), Alias("Build","BuildChannel","BuildRole")]
            [Remarks("Get a user ID in the database, and Set yourself up with a private channel and a personal role.")]
            [MinPermissions(AccessLevel.User)]
            public async Task<RuntimeResult> Setup([Remainder]String arg = "")
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(MySQLConnString.Get()))
                    {
                        var mention = Context.User.Mention;

                        string resultString = $"Setting up {mention}:\n";
                        
                        //User ID section
                        int userID = DatabaseHandler.GetUserID(Context.User.Id, Context.Guild.Id);

                        //If the user doesn't exist, create and insert him.
                        if (userID == 0)
                        {
                            userID = DatabaseHandler.AddUser(Context.User.Id, Context.Guild.Id, Context.User.Username);
                            resultString += "User ID created.\n";
                        }
                        else
                            resultString += "User ID already exists.\n";
                        
                        //Personal Channel section
                        ulong personalChannel = DatabaseHandler.GetPersonalChannel(userID);

                        if (personalChannel == 0)
                        {
                            ulong adminBotRoleID = DatabaseHandler.GetAdminbotRoleID(Context.Guild.Id);

                            var adminBot = Context.Guild.GetRole(adminBotRoleID);

                            var newChannel = await Context.Guild.CreateTextChannelAsync(Context.User.Username.ToString(), x =>
                            {
                                x.Topic = $"Your personal channel to talk to the bot.";
                                x.Position = 2;
                            });

                            var newChannelID = newChannel.Id;

                            var everyone = Context.Guild.EveryoneRole;

                            await newChannel.AddPermissionOverwriteAsync(everyone, new Discord.OverwritePermissions(viewChannel: Discord.PermValue.Deny));
                            await newChannel.AddPermissionOverwriteAsync(Context.User, new Discord.OverwritePermissions(viewChannel: Discord.PermValue.Allow));
                            await newChannel.AddPermissionOverwriteAsync(adminBot, new Discord.OverwritePermissions(viewChannel: Discord.PermValue.Allow));

                            DatabaseHandler.AddPersonalChannel(userID, newChannelID);
                            resultString += "Channel created.\n";
                        }
                        else
                            resultString += "Channel already exists.\n";

                        //Personal role section
                        ulong result = DatabaseHandler.GetPersonalRole(userID);

                        //Check if we got a result.
                        if (result == 0)
                        {   //No result was found

                            var name = Context.User.Username.ToString();

                            //Create the role in discord.
                            var role = await Context.Guild.CreateRoleAsync(name);

                            //Add the role to the user who called this command.
                            await ((IGuildUser)Context.User).AddRoleAsync(role);

                            //Add the role to the database
                            DatabaseHandler.AddPersonalRole(userID, role.Id);

                            //Command complete.
                            resultString += "Personal role created.\n";
                        }
                        else
                            resultString += "Personal role already exists.";
                        await Context.Channel.SendMessageAsync(resultString);
                        return WestbotCommandResult.AcceptReact(null, true);
                    }
                }
                catch (Exception ex)
                {   //Display exception.
                    Console.WriteLine("Exception: " + ex.Message);
                    await Context.Channel.SendMessageAsync("Exception: " + ex.Message);
                    return WestbotCommandResult.ErrorReact(null, true);
                }
            }

            [Command("color"), Alias("colour")]
            [Remarks("Change the color of your role.")]
            [MinPermissions(AccessLevel.User)]
            public async Task<RuntimeResult> color([Remainder]string arg)
            {
                try
                {
                    var userID = DatabaseHandler.GetUserID(Context.User.Id, Context.Guild.Id);
                    if(userID == 0)
                    {
                        await Setup();
                        userID = DatabaseHandler.GetUserID(Context.User.Id, Context.Guild.Id);

                        await Context.Channel.SendMessageAsync($"I had to set you up before I could change your color.\nUse the {BotConfiguration.Prefix}color command again.");
                        return WestbotCommandResult.AcceptReact(null, true);
                    }

                    ulong roleID = DatabaseHandler.GetPersonalRole(userID);

                    var role = Context.Guild.GetRole(roleID);
                    arg = arg.TrimStart('#');

                    //from stackoverflow
                    await role.ModifyAsync(x =>
                    {                        
                        x.Color = new Color(Convert.ToUInt32(arg, 16));
                    });

                    var mention = Context.User.Mention;

                    await Context.Channel.SendMessageAsync($"I set your color for you, {mention}.");
                    return WestbotCommandResult.AcceptReact(null, true);
                }
                catch (Exception ex)
                {
                    //display error message
                    Console.WriteLine("Exception: " + ex.Message);
                    await Context.Channel.SendMessageAsync("Exception: " + ex.Message);
                    return WestbotCommandResult.ErrorReact(null, true);
                }
            }
        }
    }
}
