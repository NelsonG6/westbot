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
    class DatabaseFeatures
    {
        [Name("Random features for the database.")]

        public class PrivateChannel : ModuleBase<SocketCommandContext>
        {
            [Command("GetAliases")]
            [Remarks("Gets aliases of users for the database.")]
            [MinPermissions(AccessLevel.User)]
            public async Task<RuntimeResult> BuildRole([Remainder]String arg = "")
            {
                if (arg.Length == 0)
                {   //Didn't pass a string 

                    return WestbotCommandResult.ErrorReact(null, true);
                }
                else
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(MySQLConnString.Get()))
                        {
                            conn.Open();

                            //Get a list of all users in the database without an alias, and udpate their alias from the server.



                            /*
                            SqlCommand command = new SqlCommand("GetDatabaseUserID", conn);
                            command.Parameters.Add(new SqlParameter("@input_user_id", (Int64)Context.User.Id));
                            command.Parameters.Add(new SqlParameter("@input_server_id", (Int64)Context.Guild.Id));
                            SqlParameter returnValue = new SqlParameter("@result", SqlDbType.BigInt);
                            returnValue.Direction = ParameterDirection.Output;
                            command.Parameters.Add(returnValue);
                            command.CommandType = CommandType.StoredProcedure;

                            long ID = (long)returnValue.Value;

                            command = new SqlCommand("GetPersonalRole", conn);
                            command.Parameters.Add(new SqlParameter("@input_database_id", (Int64)Context.User.Id));
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
                                command.Parameters.Add(new SqlParameter("@input_user_id", (Int64)Context.User.Id));
                                command.Parameters.Add(new SqlParameter("@input_server_id", Context.Guild.Id.ToString()));
                                command.Parameters.Add(new SqlParameter("@input_role_id", (Int64)role.Id));
                                command.CommandType = CommandType.StoredProcedure;
                                command.ExecuteNonQuery();

                                await Context.Channel.SendMessageAsync("Role created, added to the database, and assigned to you.");
                                */
                            return WestbotCommandResult.AcceptReact(null, true);
                            /*
                        }

                        else
                        {
                            await Context.Channel.SendMessageAsync("You already have a personal role.");
                            return WestbotCommandResult.ErrorReact(null, true);
                        }
                        */
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
}
