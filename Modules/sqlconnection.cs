using Discord.Commands;
using System;
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

                    string queryString = "SELECT display_name FROM dbo.channel_data;";

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
                    await Context.Channel.SendMessageAsync("Connected");

                    var ID = Context.User.Id;

                    int result = -1;

                    SqlCommand command = new SqlCommand("user_id", conn);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@ID", (Int64)ID));

                    conn.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        result = (int)reader[0];
                    }

                    if(result == -1)
                    {
                        await Context.Channel.SendMessageAsync("ID not found in the database.");
                    }
                    else
                    {
                        await Context.Channel.SendMessageAsync("ID found in the database.");

                    }
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
