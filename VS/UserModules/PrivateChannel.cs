using Discord.Commands;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Westbot;
using Westbot.Preconditions;

namespace WestBot.Modules
{
    [Name("PrivateChannel Commands")]

    public class PrivateChannel : ModuleBase<SocketCommandContext>
    {
        [Command("BuildChannel")]
        [Remarks("This command creates your channel, if it doesn't exist.")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> BuildChannels([Remainder]String arg = "")
        {
            //Contact the database and check the table that relates channel ID to discord ID
            //See if every user has a channel ID already, and for those that don't, create a channel 

            //If he does not have an associated channel ID, create a channel for him, and add that channel ID to the database.
        
            try
            {
                using (SqlConnection conn = new SqlConnection(MySQLConnString.Get()))
                {
                    SqlCommand command = new SqlCommand("CheckPrivateChannel", conn);
                    SqlParameter returnValue = new SqlParameter("returnVal", SqlDbType.Int);
                    returnValue.Direction = ParameterDirection.ReturnValue;
                    command.Parameters.Add(returnValue);
                    command.Parameters.Add(new SqlParameter("@input_user_id", (Int64)Context.User.Id));
                    command.Parameters.Add(new SqlParameter("@input_server_id", Context.Guild.Id.ToString()));

                    command.CommandType = CommandType.StoredProcedure;
                    //command.Parameters.Add(new SqlParameter("@ID", (Int64)ID));

                    conn.Open();

                    command.ExecuteNonQuery();

                    int result = (Int32)returnValue.Value;

                    if(result == 0)
                    {   
                        //create a channel and pass the stored procedure the channel ID
                        //Get the role ID for adminbot role on this current server,
                        //Which is used to grant adminbots access to the users private channel.
                        command = new SqlCommand("GetAdminbotRoleID", conn);
                        command.Parameters.Add(new SqlParameter("@current_server", Context.Guild.Id.ToString()));
                        returnValue = new SqlParameter
                        {
                            SqlDbType = SqlDbType.BigInt,
                            ParameterName = "@result",
                            Direction = ParameterDirection.Output

                        };
                        command.Parameters.Add(returnValue);
                        command.CommandType = CommandType.StoredProcedure;
                        command.ExecuteNonQuery();

                        long id1 = (long)returnValue.Value;
                        UInt64 id2 = (UInt64)id1;

                        var adminBot = Context.Guild.GetRole(id2);

                        var newChannel = await Context.Guild.CreateTextChannelAsync(Context.User.Username.ToString(), x =>
                        {
                            x.Topic = $"Your personal channel to talk to the bot.";
                            x.Position = 2;
                        });

                        var id = newChannel.Id;

                        var everyone = Context.Guild.EveryoneRole;

                        await newChannel.AddPermissionOverwriteAsync(everyone, new Discord.OverwritePermissions(viewChannel: Discord.PermValue.Deny));
                        await newChannel.AddPermissionOverwriteAsync(Context.User, new Discord.OverwritePermissions(viewChannel: Discord.PermValue.Allow));
                        await newChannel.AddPermissionOverwriteAsync(adminBot, new Discord.OverwritePermissions(viewChannel: Discord.PermValue.Allow));

                        
                        //Now that the private channel has been created, add it to the database.
                        command = new SqlCommand("AddPrivateChannel", conn);
                        command.Parameters.Add(new SqlParameter("@input_user_id", (Int64)Context.User.Id));
                        command.Parameters.Add(new SqlParameter("@input_server_id", Context.Guild.Id.ToString()));
                        command.Parameters.Add(new SqlParameter("@input_channel_id", (Int64)id));
                        command.CommandType = CommandType.StoredProcedure;
                        command.ExecuteNonQuery();

                        conn.Close();

                        await Context.Channel.SendMessageAsync("Channel added to database.");
                        return WestbotCommandResult.AcceptReact();
                    }
                    else
                    {
                        await Context.Channel.SendMessageAsync("You already have a private channel registered.");
                        return WestbotCommandResult.ErrorReact(null, true);
                    }

                }
            }

            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                await Context.Channel.SendMessageAsync("Exception: " + ex.Message);
            }


            return WestbotCommandResult.AcceptReact();
        }
    }
}
