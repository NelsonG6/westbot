using System;
using System.Data.SqlClient;
using System.Data;

namespace WestBot
{
    static class DatabaseHandler
    {
        static public int GetUserID(ulong DiscordID, ulong ServerID)
        {
            using (SqlConnection conn = new SqlConnection(MySQLConnString.Get()))
            {
                SqlCommand command = new SqlCommand("GetUserID", conn);
                command.Parameters.Add(new SqlParameter("@input_discord_id", (Int64)DiscordID));
                command.Parameters.Add(new SqlParameter("@input_server_id", (Int64)ServerID));
                SqlParameter returnValue = new SqlParameter("@result", SqlDbType.BigInt);
                returnValue.Direction = ParameterDirection.Output;
                command.Parameters.Add(returnValue);
                command.CommandType = CommandType.StoredProcedure;
                conn.Open();
                command.ExecuteNonQuery();
                return Convert.ToInt32(returnValue.Value);
            }   
        }

        static public int VerifyUserExists(ulong UserID)
        {
            using (SqlConnection conn = new SqlConnection(MySQLConnString.Get()))
            {
                SqlCommand command = new SqlCommand("VerifyUserExists", conn);
                command.Parameters.Add(new SqlParameter("@input_user_id", (Int64)UserID));
                SqlParameter returnValue = new SqlParameter("@result", SqlDbType.BigInt);
                returnValue.Direction = ParameterDirection.Output;
                command.Parameters.Add(returnValue);
                command.CommandType = CommandType.StoredProcedure;
                conn.Open();
                command.ExecuteNonQuery();
                return Convert.ToInt32(returnValue.Value);
            }
        }

        static public ulong GetPersonalChannel(int userID)
        {
            using (SqlConnection conn = new SqlConnection(MySQLConnString.Get()))
            {
                SqlCommand command = new SqlCommand("GetPersonalChannel", conn);
                SqlParameter returnValue = new SqlParameter("@result", SqlDbType.BigInt);
                returnValue.Direction = ParameterDirection.Output;
                command.Parameters.Add(returnValue);
                command.Parameters.Add(new SqlParameter("@input_database_id", userID));
                command.CommandType = CommandType.StoredProcedure;
                conn.Open();
                command.ExecuteNonQuery();
                var returnval = (long)returnValue.Value;
                return (ulong)returnval;
            }
        }

        static public ulong GetAdminbotRoleID(ulong serverID)
        {

            using (SqlConnection conn = new SqlConnection(MySQLConnString.Get()))
            {
                //create a channel and pass the stored procedure the channel ID
                //Get the role ID for adminbot role on this current server,
                //Which is used to grant adminbots access to the users private channel.
                SqlCommand command = new SqlCommand("GetAdminbotRoleID", conn);
                command.Parameters.Add(new SqlParameter("@current_server", (Int64)serverID));
                SqlParameter returnValue = new SqlParameter
                {
                    SqlDbType = SqlDbType.BigInt,
                    ParameterName = "@result",
                    Direction = ParameterDirection.Output

                };
                command.Parameters.Add(returnValue);
                command.CommandType = CommandType.StoredProcedure;
                conn.Open();
                command.ExecuteNonQuery();

                long returnval = (long)returnValue.Value;
                return (ulong)returnval;
            }
        }

        static public void AddPersonalChannel(int userID, ulong newChannelID)
        {
            using (SqlConnection conn = new SqlConnection(MySQLConnString.Get()))
            {
                //Now that the private channel has been created, add it to the database.
                SqlCommand command = new SqlCommand("AddPersonalChannel", conn);
                command.Parameters.Add(new SqlParameter("@input_user_id", (Int64)userID));
                command.Parameters.Add(new SqlParameter("@input_channel_id", (Int64)newChannelID));
                command.CommandType = CommandType.StoredProcedure;
                conn.Open();
                command.ExecuteNonQuery();
            }
        }

        static public int AddUser(ulong discordID, ulong newChannelID, string alias)
        {
            using (SqlConnection conn = new SqlConnection(MySQLConnString.Get()))
            {
                //Now that the private channel has been created, add it to the database.
                SqlCommand command = new SqlCommand("AddUser", conn);
                command.Parameters.Add(new SqlParameter("@input_discord_id", (Int64)discordID));
                command.Parameters.Add(new SqlParameter("@input_server_id", (Int64)newChannelID));
                command.Parameters.Add(new SqlParameter("@input_alias", alias));
                SqlParameter returnValue = new SqlParameter("@result", SqlDbType.BigInt);
                returnValue.Direction = ParameterDirection.Output;
                command.Parameters.Add(returnValue);

                command.CommandType = CommandType.StoredProcedure;
                conn.Open();
                command.ExecuteNonQuery();
                return Convert.ToInt32(returnValue.Value);
            }
        }

        static public ulong GetPersonalRole(int userID)
        {
            using (SqlConnection conn = new SqlConnection(MySQLConnString.Get()))
            {
                SqlCommand command = new SqlCommand("GetPersonalRole", conn);
                command.Parameters.Add(new SqlParameter("@input_user_id", (Int64)userID));
                SqlParameter returnValue = new SqlParameter("@result", SqlDbType.BigInt);
                returnValue.Direction = ParameterDirection.Output;
                command.Parameters.Add(returnValue);
                command.CommandType = CommandType.StoredProcedure;
                conn.Open();
                command.ExecuteNonQuery();

                var returnval = (long)returnValue.Value;
                return (ulong)returnval;
            }
        }

        static public void AddPersonalRole(int userID, ulong newRoleID)
        {
            using (SqlConnection conn = new SqlConnection(MySQLConnString.Get()))
            {
                //Now that the private channel has been created, add it to the database.
                SqlCommand command = new SqlCommand("AddPersonalRole", conn);
                command.Parameters.Add(new SqlParameter("@input_user_id", (Int64)userID));
                command.Parameters.Add(new SqlParameter("@input_role_id", (Int64)newRoleID));
                command.CommandType = CommandType.StoredProcedure;
                conn.Open();
                command.ExecuteNonQuery();
            }
        }

        static public void RemoveUser(int userID)
        {
            using (SqlConnection conn = new SqlConnection(MySQLConnString.Get()))
            {
                //Now that the private channel has been created, add it to the database.
                SqlCommand command = new SqlCommand("RemoveUser", conn);
                command.Parameters.Add(new SqlParameter("@input_user_id", (Int64)userID));
                command.CommandType = CommandType.StoredProcedure;
                conn.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
