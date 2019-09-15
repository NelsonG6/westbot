using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Westbot.Preconditions;
using Discord;
using Discord.WebSocket;

namespace Westbot.Services
{
    [Name("Public role management commands")]
    public class RolesModule : ModuleBase<SocketCommandContext>
    {
        //Commands begin
        [Command("create"), Alias("makerole", "createrole")]
        [Remarks("Bot makes a role")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Createrole([Remainder]string role_argument = "")
        {
            var admin_role = GetRole("Admin");
            if (!DoesUserHaveRole(admin_role, (SocketGuildUser)Context.User))
            {
                await Context.Channel.SendMessageAsync("You need the admin role to use that command.");
                return WestbotCommandResult.ErrorReact("Invalid role level");
            }
            var role = GetRole(role_argument);
            if (role != null)
            {
                await Context.Channel.SendMessageAsync("That role already exists.");
                return WestbotCommandResult.ErrorReact("Created a role that already existed");
            }

            else
            {
                await Context.Guild.CreateRoleAsync(role_argument);
                await Context.Channel.SendMessageAsync("I have created the `" + role_argument + "` role for you, " + Context.User.Mention + ".");
                return WestbotCommandResult.AcceptReact();
            }
            
        }

        [Command("giverole"), Alias("setrole", "assignrole", "give")]
        [Remarks("Admin instructs the bot to add a role to a user.")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Giverole(string user_name = "", [Remainder]string role_name = "")
        {
            var admin_role = GetRole("Admin");
            if (!DoesUserHaveRole(admin_role, (SocketGuildUser)Context.User))
            {
                await Context.Channel.SendMessageAsync("You need the admin role to use that command.");
                return WestbotCommandResult.ErrorReact("Invalid role level");
            }

            SocketGuildUser temp_user = Context.Guild.Users.FirstOrDefault(x => x.Nickname == user_name);
            IRole temp_role = GetRole(role_name);

            if (DoesUserHaveRole(temp_role, temp_user) || temp_role == null)
            {
                await Context.Channel.SendMessageAsync("That user already has the `" + temp_role + "` role, " + Context.User.Mention + ".");
                return WestbotCommandResult.ErrorReact("user had role");
            }
            await (Context.User as IGuildUser).AddRoleAsync(temp_role);

            await Context.Channel.SendMessageAsync("I have given the `" + temp_role + "` role to " + temp_user.Mention + ".");
            return WestbotCommandResult.AcceptReact();
        }

        [Command("giverole"), Alias("assignrole", "give")]
        [Remarks("Admin instructs the bot to add a role to a user.")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Giverole2(SocketUser temp_user, [Remainder]string role_name = "")
        {
            var admin_role = GetRole("Admin");
            if (!DoesUserHaveRole(admin_role, (SocketGuildUser)Context.User))
            {
                await Context.Channel.SendMessageAsync("You need the admin role to use that command.");
                return WestbotCommandResult.ErrorReact("Invalid role level");
            }

            IRole temp_role = GetRole(role_name);
            await (Context.User as IGuildUser).AddRoleAsync(temp_role);

            if (DoesUserHaveRole(temp_role, (SocketGuildUser)temp_user) || temp_role == null)
            {
                await Context.Channel.SendMessageAsync("That user already has the `" + temp_role + "` role, " + Context.User.Mention + ".");
                return WestbotCommandResult.ErrorReact("user had role");
            }

            await Context.Channel.SendMessageAsync("I have given the `" + temp_role + "` role to " + temp_user.Mention + ".");
            return WestbotCommandResult.AcceptReact();
        }

        [Command("unassign"), Alias("removerole", "unassignrole")]
        [Remarks("admin directs the bot to remove a role from someone")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Removerole(string user_string, [Remainder]string role_argument = "")
        {
            var admin_role = GetRole("Admin");
            if (!DoesUserHaveRole(admin_role, (SocketGuildUser)Context.User))
            {
                await Context.Channel.SendMessageAsync("You need the admin role to use that command.");
                return WestbotCommandResult.ErrorReact("Invalid role level");
            }

            var temp_user = Context.Guild.Users.FirstOrDefault(x => x.Nickname == user_string);
            IRole temp_role = GetRole(role_argument);

            await temp_user.RemoveRoleAsync(temp_role);
            await Context.Channel.SendMessageAsync("I have removed the `" + temp_role + "` role for you, " + Context.User.Mention + ".");

            return WestbotCommandResult.AcceptReact();
        }


        [Command("unassign"), Alias("removerole", "unassignrole")]
        [Remarks("admin directs the bot to remove a role from someone")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Removerole2(SocketUser user, [Remainder]string role_argument = "")
        {
            var admin_role = GetRole("Admin");
            if (!DoesUserHaveRole(admin_role, (SocketGuildUser)Context.User))
            {
                await Context.Channel.SendMessageAsync("You need the admin role to use that command.");
                return WestbotCommandResult.ErrorReact("Invalid role level");
            }

            var temp_user = Context.Guild.GetUser(user.Id);
            IRole temp_role = GetRole(role_argument);

            await temp_user.RemoveRoleAsync(temp_role);
            await Context.Channel.SendMessageAsync("I have removed the `" + temp_role + "` role for you, " + Context.User.Mention + ".");
            return WestbotCommandResult.AcceptReact();
        }


        [Command("iam"), Alias("im", "i'm", "giveme", "get")]
        [Remarks("bot gives a role to someone")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Iam([Remainder]string role_argument = "")
        {
            //Remove accdiental double spaces someone might have put int.
            //I control the roles, so I know for sure that no roles have two spaces.


            IRole to_add = GetRole(role_argument); //Checks if role exists, gets role if so. wont generate an error

            if (to_add != null)
            {
                if (!DoesUserHaveRole(to_add, (SocketGuildUser)Context.User) && RoleIsLegal(to_add))
                { //Here we know the role matches an existing role, and is not already added.
                    Console.WriteLine("Role matched in if statement");
                    await (Context.User as IGuildUser).AddRoleAsync(to_add);
                    await ReplyAsync($"I have added `{to_add}` for you, {Context.User.Mention}.");
                    return WestbotCommandResult.AcceptReact("Role matched in if statement");
                }
                else if (DoesUserHaveRole(to_add, (SocketGuildUser)Context.User))
                {
                    Console.WriteLine("User had this role.");
                    await ReplyAsync($"You already have `{to_add}`, {Context.User.Mention}.");
                    return WestbotCommandResult.ErrorReact("Role not found");
                }
                else if (!RoleIsLegal(to_add))
                {
                    Console.Write($"`I can't give you `{to_add}`, {Context.User.Mention}.");
                    return WestbotCommandResult.ErrorReact("RoleNotLegal");
                }
            }
            return WestbotCommandResult.ErrorReact("Should not reach this");
        }

        [Command("remove"), Alias("iamn", "abandon", "stop being", "please remove", "remove role", "iamnot", "imnot", "i dont want")]
        [Remarks("Bot removes a role for a user")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Remove([Remainder]string role_argument = "")
        {
            Console.WriteLine(".iamnot() called. Calling argument: [" + role_argument + "]");

            //Remove accdiental double spaces someone might have put int.
            //I control the roles, so I know for sure that no roles have two spaces.

            IRole to_add = GetRole(role_argument); //Checks if role exists, gets role if so. wont generate an error

            if (to_add != null)
            {
                if (DoesUserHaveRole(to_add, ((SocketGuildUser)Context.User)))
                { //Here we know the role matches an existing role, and is not already added.
                    Console.WriteLine("Role matched in if statement");
                    await (Context.User as IGuildUser).RemoveRoleAsync(to_add);
                    await ReplyAsync($"I have removed `{to_add}` for you, {Context.User.Mention}.");
                    return WestbotCommandResult.AcceptReact("Role matched in if statement");
                }

                else //User doesn't have role to remove
                {
                    Console.WriteLine("to_add was null, did not add a role.");
                    await ReplyAsync($"You dont have `{to_add}` for me to remove, {Context.User.Mention}.");
                    return WestbotCommandResult.ErrorReact("User doesn't have role");
                }
            }
            //to_add was null, role provided was bad
            await ReplyAsync($"I was unable to find `{to_add}`, {Context.User.Mention}.");
            return WestbotCommandResult.ErrorReact("Role not found");
        }

        

        //add remove role from user and create role commands eventually

        public IRole GetRole(string role_argument)
        {
            //Replace multiple spaces (  ) in role_argument with just one space ( ) to help with typos
            string temp_argument = role_argument;
            temp_argument = role_argument.Replace("  ", " ");
            //If temp_argument is different than role_argument, then spaces were removed.
            //If that is the case, more spaces may need to be removed as well.
            //Loop will be entered.
            while (temp_argument != role_argument)
            {
                role_argument = temp_argument;
                temp_argument = role_argument.Replace("  ", " ");
            }

            foreach (IRole temp_role in Context.Guild.Roles.ToArray())
            { //check if the role exists
                if (role_argument.ToLower() == temp_role.ToString().ToLower())
                    return temp_role;
            }
            Console.WriteLine("Returning NULL from GetRole() for role " + role_argument);
            return null; //nothing found
        }

        public Boolean DoesUserHaveRole(IRole to_check, SocketGuildUser user)
        {
            foreach (IRole temp_role in (user.Roles))
            { //check if the user already has the role
                if (temp_role.ToString().ToLower() == to_check.ToString().ToLower())
                    return true;
            }
            return false;
        }

        public Boolean RoleIsLegal(IRole to_check)
        {
            if (to_check.ToString() == "Admin" || to_check.ToString() == "Bot")
                return false;
            return true;
        }
    }
}