using System;
using System.Threading.Tasks;
using Discord.Commands;
using Westbot;
using Westbot.Preconditions;
using System.Data.SqlClient;
using System.Data;
using Discord;

namespace WestBot
{
    class PersonalRole
    {
        [Name("Commands for creating your personal role, and having it assigned.")]

        public class PrivateChannel : ModuleBase<SocketCommandContext>
        {
            [Command("BuildRole")]
            [Remarks("This command creates your role, if it doesn't exist.")]
            [MinPermissions(AccessLevel.User)]
            public async Task<RuntimeResult> BuildRole([Remainder]String arg = "")
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(MySQLConnString.Get()))
                    {
                        conn.Open();
                        SqlCommand command = new SqlCommand("GetDatabaseUserID", conn);
                        command.Parameters.Add(new SqlParameter("@input_user_id", (Int64)Context.User.Id));
                        command.Parameters.Add(new SqlParameter("@input_server_id", (Int64)Context.Guild.Id));
                        SqlParameter returnValue = new SqlParameter("@result", SqlDbType.BigInt);
                        returnValue.Direction = ParameterDirection.Output;
                        command.Parameters.Add(returnValue);
                        command.CommandType = CommandType.StoredProcedure;
                        command.ExecuteNonQuery();

                        long ID = (long)returnValue.Value;

                        command = new SqlCommand("GetPersonalRole", conn);
                        command.Parameters.Add(new SqlParameter("@input_database_id", ID));
                        returnValue = new SqlParameter("@result", SqlDbType.BigInt);
                        returnValue.Direction = ParameterDirection.Output;
                        command.Parameters.Add(returnValue);
                        command.CommandType = CommandType.StoredProcedure;                    
                        command.ExecuteNonQuery();

                        long result = (long)returnValue.Value;

                        if (result == 0)
                        {   //Build a personal role for this user and add it to the table

                            //Make the role
                            var name = Context.User.Username.ToString();
                            var role = await Context.Guild.CreateRoleAsync(name);

                            await ((IGuildUser)Context.User).AddRoleAsync(role);

                            //Add the role to the database
                            //Now that the private channel has been created, add it to the database.
                            command = new SqlCommand("AddPersonalRole", conn);
                            command.Parameters.Add(new SqlParameter("@input_database_id", ID));
                            command.Parameters.Add(new SqlParameter("@input_role_id", (Int64)role.Id));
                            command.CommandType = CommandType.StoredProcedure;
                            command.ExecuteNonQuery();

                            await Context.Channel.SendMessageAsync("Role created, added to the database, and assigned to you.");
                            return WestbotCommandResult.AcceptReact(null, true);
                        }

                        else
                        {
                            await Context.Channel.SendMessageAsync("You already have a personal role.");
                            return WestbotCommandResult.ErrorReact(null, true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //display error message
                    Console.WriteLine("Exception: " + ex.Message);
                    await Context.Channel.SendMessageAsync("Exception: " + ex.Message);
                    return WestbotCommandResult.ErrorReact(null, true);
                }
            }

            [Command("color")]
            [Remarks("Change the color of your role.")]
            [MinPermissions(AccessLevel.User)]
            public async Task<RuntimeResult> color([Remainder]String arg = "")
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(MySQLConnString.Get()))
                    {
                        SqlCommand command = new SqlCommand("GetPersonalRole", conn);
                        command.Parameters.Add(new SqlParameter("@input_user_id", (Int64)Context.User.Id));
                        command.Parameters.Add(new SqlParameter("@input_server_id", (Int64)Context.Guild.Id));
                        SqlParameter returnValue = new SqlParameter("@result", SqlDbType.BigInt);
                        returnValue.Direction = ParameterDirection.Output;
                        command.Parameters.Add(returnValue);

                        command.CommandType = CommandType.StoredProcedure;

                        conn.Open();

                        command.ExecuteNonQuery();

                        long result2 = (long)returnValue.Value;
                        UInt64 id = (UInt64)result2;

                        var role = Context.Guild.GetRole(id);
                        
                        //from stackoverflow
                        await role.ModifyAsync(x =>
                        {
                            x.Name = "cool boi";
                            x.Color = Color.Gold;
                            x.Hoist = true;
                            x.Mentionable = true;
                        });

                        return WestbotCommandResult.AcceptReact(null, true);
                    }
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
