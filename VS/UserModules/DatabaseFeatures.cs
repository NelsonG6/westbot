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
            [Command("InsertAlias")]
            [Remarks("Gets aliases of users for the database.")]
            [MinPermissions(AccessLevel.User)]
            public async Task<RuntimeResult> InsertAlias([Remainder]String arg = "")
            {
                //Just fill in the discord alias for the first result that doesn't have their alias set
                try
                {
                    using (SqlConnection conn = new SqlConnection(MySQLConnString.Get()))
                    {
                        conn.Open();
                        SqlCommand command = new SqlCommand("GetUserWithoutAlias", conn);
                        command.Parameters.Add(new SqlParameter("@input_server_id", Context.Guild.Id.ToString()));
                        SqlParameter returnValue = new SqlParameter("@result", SqlDbType.BigInt);
                        returnValue.Direction = ParameterDirection.Output;
                        command.Parameters.Add(returnValue);
                        command.CommandType = CommandType.StoredProcedure;
                        command.ExecuteNonQuery();

                        long long_ID = (long)returnValue.Value;

                        if(long_ID == 0)
                        {
                            await Context.Channel.SendMessageAsync("Everyone already has an alias on this server.");
                            return WestbotCommandResult.ErrorReact(null, true);
                        }

                        ulong ID = (UInt64)long_ID;

                        string alias = Context.Guild.GetUser(ID).Nickname;

                        long UserID = DatabaseHandler.GetUserID(ID, Context.Guild.Id);

                        command = new SqlCommand("AddAlias", conn);
                        command.Parameters.Add(new SqlParameter("@input_user_id", (Int64)UserID));
                        command.Parameters.Add(new SqlParameter("@input_alias", alias));
                        command.CommandType = CommandType.StoredProcedure;
                        command.ExecuteNonQuery();

                        await Context.Channel.SendMessageAsync("Updated the alias for [" + alias + "].");
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
