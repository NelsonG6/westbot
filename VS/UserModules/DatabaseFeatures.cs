using System;
using System.Threading.Tasks;
using Discord.Commands;
using Westbot;
using Westbot.Preconditions;
using System.Data.SqlClient;
using Discord;
using System.Collections.Generic;
using System.Linq;

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
                        int userID = DatabaseHandler.GetUserID(Context.User.Id);

                        //If the user doesn't exist, create and insert him.
                        if (userID == 0)
                        {
                            userID = DatabaseHandler.AddUser(Context.User.Id, Context.Guild.Id, Context.User.Username);
                            resultString += "User ID created.\n";
                        }
                        else
                            resultString += "User ID already exists.\n";

                        //Personal role section
                        ulong result = DatabaseHandler.GetPersonalRole(userID);

                        Discord.Rest.RestRole role = null;

                        //Check if we got a result.
                        if (result == 0)
                        {   //No result was found

                            var name = Context.User.Username.ToString();

                            //Create the role in discord.
                            role = await Context.Guild.CreateRoleAsync(name);

                            //Add the role to the user who called this command.
                            await ((IGuildUser)Context.User).AddRoleAsync(role);

                            //Add the role to the database
                            DatabaseHandler.AddPersonalRole(userID, role.Id);

                            //Command complete.
                            resultString += "Personal role created.\n";

                            if (arg != "")
                            {
                                arg = arg.TrimStart('#');

                                await role.ModifyAsync(x =>
                                {
                                    x.Color = new Color(Convert.ToUInt32(arg, 16));
                                });
                                resultString += "I set your color for you.\n";
                            }
                        }
                        else
                            resultString += "Personal role already exists.";

                        //Personal Channel section
                        ulong personalChannel = DatabaseHandler.GetPersonalChannel(userID);

                        if (personalChannel == 0)
                        {
                            ulong adminBotRoleID = DatabaseHandler.GetAdminbotRoleID();

                            var adminBot = Context.Guild.GetRole(adminBotRoleID);

                            var newChannel = await Context.Guild.CreateTextChannelAsync(Context.User.Username.ToString(), x =>
                            {
                                x.Topic = $"Your personal channel to talk to the bot.";
                                x.Position = 2;
                            });

                            var newChannelID = newChannel.Id;

                            var everyone = Context.Guild.EveryoneRole;

                            await newChannel.AddPermissionOverwriteAsync(adminBot, new Discord.OverwritePermissions(viewChannel: Discord.PermValue.Allow));
                            await newChannel.AddPermissionOverwriteAsync(role, new Discord.OverwritePermissions(viewChannel: Discord.PermValue.Allow));
                            await newChannel.AddPermissionOverwriteAsync(everyone, new Discord.OverwritePermissions(viewChannel: Discord.PermValue.Deny));

                            DatabaseHandler.AddPersonalChannel(userID, newChannelID);
                            resultString += "Channel created.\n";
                        }
                        else
                            resultString += "Channel already exists.\n";

                        

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
            public async Task<RuntimeResult> color([Remainder]string arg = "")
            {
                if(arg == "")
                {
                    await Context.Channel.SendMessageAsync("Go to https://www.color-hex.com/ and pick a color. Then type: ```.color #123456```");
                    return WestbotCommandResult.AcceptReact(null, true);
                }

                try
                {
                    var userID = DatabaseHandler.GetUserID(Context.User.Id);
                    if(userID == 0)
                    {
                        await Setup(arg);
                        return WestbotCommandResult.AcceptNoReaction(null, true);
                    }

                    ulong roleID = DatabaseHandler.GetPersonalRole(userID);

                    var role = Context.Guild.GetRole(roleID);
                    arg = arg.TrimStart('#');

                    var mention = Context.User.Mention;

                    var color = new Color(Convert.ToUInt32(arg, 16));

                    int r = color.R;
                    int b = color.B;
                    int g = color.G;

                    List<int> colors = new List<int>();
                    colors.Add(r);
                    colors.Add(b);
                    colors.Add(g);

                    int max = colors.Max();
                    int min = colors.Min();

                    bool deny_color = false;
                    int total_difference = 0;

                    if (max > 200)
                    {
                        //Since there is a color above 200, we need to make sure the difference of these colors...
                        //Is great enough to prevent a whitish color from being chosen.
                        //To do this, just ensure that the difference of all rbg values adds up to greater than 30.
                        
                        foreach (var i in colors)
                        {
                            if(i != max)
                            {
                                total_difference += Math.Abs(max - i);
                            }
                        }
                        if (total_difference <= 50)
                            deny_color = true;
                    }

                    else if (max > 175)
                    {
                        //Since there is a color above 200, we need to make sure the difference of these colors...
                        //Is great enough to prevent a whitish color from being chosen.
                        //To do this, just ensure that the difference of all rbg values adds up to greater than 30.

                        foreach (var i in colors)
                        {
                            if (i != max)
                            {
                                total_difference += Math.Abs(max - i);
                            }
                        }
                        if (total_difference <= 20)
                            deny_color = true;
                    }

                    if (deny_color)
                    {
                        await Context.Channel.SendMessageAsync($"That color was too white, {mention}.");
                        return WestbotCommandResult.ErrorReact(null, true);
                    }
                    
                    //from stackoverflow
                    await role.ModifyAsync(x =>
                    {
                        x.Color = new Color(Convert.ToUInt32(arg, 16));
                    });

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

            [Command("deleteuser"), Alias("erase")]
            [Remarks("Change the color of your role.")]
            [MinPermissions(AccessLevel.ServerOwner)]
            public async Task<RuntimeResult> delete([Remainder]ulong ID)
            {
                if (ID == 0)
                {
                    await Context.Channel.SendMessageAsync("Invalid user entered.");
                    return WestbotCommandResult.ErrorReact(null, true);
                }

                try
                {
                    int result = DatabaseHandler.VerifyUserExists(ID);
                    if (result == 0)
                    {
                        await Context.Channel.SendMessageAsync("Invalid user entered.");
                        return WestbotCommandResult.ErrorReact(null, true);
                    }

                    //Valid user found.
                    //Delete the user's role and channel, and then remove from the database.

                    ulong PersonalChannel = DatabaseHandler.GetPersonalChannel((int)ID);
                    var Channel = Context.Guild.GetChannel(PersonalChannel);
                    if(Channel != null)
                    {
                        await Channel.DeleteAsync();
                    }
                    
                    ulong PersonalRole = DatabaseHandler.GetPersonalRole((int)ID);
                    var Role = Context.Guild.GetRole(PersonalRole);
                    if(Role != null)
                    {
                        await Role.DeleteAsync();
                    }

                    DatabaseHandler.RemoveUser((int)ID);

                    await Context.Channel.SendMessageAsync("User removed.");
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
