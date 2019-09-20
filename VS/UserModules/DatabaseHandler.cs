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
    static class DatabaseHandler
    {
        static public long GetUserID(ulong UserID, ulong ServerID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(MySQLConnString.Get()))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand("GetDatabaseUserID", conn);
                    command.Parameters.Add(new SqlParameter("@input_discord_id", (Int64)UserID));
                    command.Parameters.Add(new SqlParameter("@input_server_id", (Int64)ServerID));
                    SqlParameter returnValue = new SqlParameter("@result", SqlDbType.BigInt);
                    returnValue.Direction = ParameterDirection.Output;
                    command.Parameters.Add(returnValue);
                    command.CommandType = CommandType.StoredProcedure;
                    command.ExecuteNonQuery();

                    return (long)returnValue.Value;
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return 0;
            }
        }
    }
}
