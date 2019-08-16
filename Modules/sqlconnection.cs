using Discord.Commands;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Westbot;
using Westbot.Preconditions;

namespace WestBot.Modules
{
    [Name("SQL")]

    public class Sqlconnection : ModuleBase<SocketCommandContext>
    {
        [Command("sqltest")]
        [Remarks("sql testing")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> sqltest([Remainder]String arg = "")
        {
            string connString = "server=(local);Data Source=RIGHTPC\\SQLexpress;Initial Catalog=Westbot;Trusted_Connection = True;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    await Context.Channel.SendMessageAsync("Connected");

                    string queryString = "SELECT discord_id FROM dbo.user_table WHERE discord_id =";
                    queryString += Context.User.Id.ToString();
                    queryString += ";";

                    SqlCommand command = new SqlCommand(queryString, conn);
                    conn.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            await Context.Channel.SendMessageAsync(String.Format("{0}", reader[0]));
                            Console.WriteLine(String.Format("{0}", reader[0]));
                        }
                    }





                    //access SQL Server and run your command
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                await Context.Channel.SendMessageAsync("Exception: " + ex.Message);
            }



            return WestbotCommandResult.AcceptNoReact();
        }

        [Command("setup")]
        [Remarks("initiate the ")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Grinch([Remainder]String arg = "") //Leave the remainder in case they type extra stuff for no reason
        {
            string connString = "server=(local);Data Source=RIGHTPC\\SQLexpress;Initial Catalog=Westbot;Trusted_Connection = True;";

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    var ID = Context.User.Id;

                    SqlCommand command = new SqlCommand("InsertNewUser", conn);
                    SqlParameter returnValue = new SqlParameter("returnVal", SqlDbType.Int);
                    returnValue.Direction = ParameterDirection.ReturnValue;
                    command.Parameters.Add(returnValue);
                    command.Parameters.Add(new SqlParameter("@insert_ID", (Int64)Context.User.Id));
                    command.Parameters.Add(new SqlParameter("@insert_display_name", Context.User.Username.ToString()));
                    command.Parameters.Add(new SqlParameter("@insert_current_server", Context.Guild.Id.ToString()));
                    command.CommandType = CommandType.StoredProcedure;
                    //command.Parameters.Add(new SqlParameter("@ID", (Int64)ID));

                    conn.Open();

                    command.ExecuteNonQuery();

                    conn.Close();

                    await Context.Channel.SendMessageAsync("User updated for this server.");

                    

                   


                    /*
                     *                         while (reader.Read())
                     *  {
                            await Context.Channel.SendMessageAsync(String.Format("{0}", reader[0]));
                            Console.WriteLine(String.Format("{0}", reader[0]));
                        }
                    */


                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                await Context.Channel.SendMessageAsync("Exception: " + ex.Message);
            }



            return WestbotCommandResult.AcceptNoReact();
        }

    }
}
