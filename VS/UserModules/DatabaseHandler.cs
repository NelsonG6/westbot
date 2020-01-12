using System;
using System.Data.SqlClient;
using System.Data;
using Westbot;

namespace WestBot
{
    static class DatabaseHandler
    {
        static public int GetUserID(ulong DiscordID)
        {
            using (SqlConnection conn = new SqlConnection(MySQLConnString.Get()))
            {
                SqlCommand command = new SqlCommand("GetUserID", conn);
                command.Parameters.Add(new SqlParameter("@input_discord_id", (Int64)DiscordID));
                command.Parameters.Add(new SqlParameter("@input_server_id", (Int64)BotConfiguration.ServerID));
                SqlParameter returnValue = new SqlParameter("@result", SqlDbType.BigInt);
                returnValue.Direction = ParameterDirection.Output;
                command.Parameters.Add(returnValue);
                command.CommandType = CommandType.StoredProcedure;
                conn.Open();
                command.ExecuteNonQuery();
                return Convert.ToInt32(returnValue.Value);
            }   
        }

        static public void GetConfiguration(string config_name)
        {
            using (SqlConnection conn = new SqlConnection(MySQLConnString.Get()))
            {
                SqlParameter param_token = new SqlParameter("@return_token", SqlDbType.NVarChar);
                param_token.Direction = ParameterDirection.Output;
                param_token.Size = 100;

                SqlParameter param_prefix = new SqlParameter("@return_prefix", SqlDbType.Char);
                param_prefix.Direction = ParameterDirection.Output;
                param_prefix.Size = 1;

                SqlParameter param_server_id = new SqlParameter("@return_server_id", SqlDbType.BigInt);
                param_server_id.Direction = ParameterDirection.Output;

                SqlCommand command = new SqlCommand("GetConfiguration", conn);
                command.Parameters.Add(param_token);
                command.Parameters.Add(param_prefix);
                command.Parameters.Add(param_server_id);

                command.Parameters.Add(new SqlParameter("@input_config_name", config_name));

                command.CommandType = CommandType.StoredProcedure;

                conn.Open();
                command.ExecuteNonQuery();

                BotConfiguration.Token = (string)param_token.Value;
                BotConfiguration.Prefix = ((string)param_prefix.Value).ToCharArray()[0];

                //Store the server ID
                long result2 = (long)param_server_id.Value;
                UInt64 ResultID = (UInt64)result2;
                BotConfiguration.ServerID = ResultID;
            }
        }

        static public int EmoteOrEmoji(string reaction_type)
        {
            using (SqlConnection conn = new SqlConnection(MySQLConnString.Get()))
            {
                SqlCommand command = new SqlCommand("EmoteOrEmoji", conn);
                SqlParameter newparam = new SqlParameter("@input_reaction_type", reaction_type);
                command.Parameters.Add(newparam);
                command.Parameters.Add(new SqlParameter("@input_server_id", (Int64)BotConfiguration.ServerID));
                SqlParameter returnValue = new SqlParameter("result", SqlDbType.Int);
                returnValue.Direction = ParameterDirection.Output;
                command.Parameters.Add(returnValue);

                conn.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();

                return (int)returnValue.Value;
            }
        }

        static public string GetRandomEmoji(string reaction_type)
        {
            using (SqlConnection conn = new SqlConnection(MySQLConnString.Get()))
            {
                SqlCommand command = new SqlCommand("GetRandomEmoji", conn);
                SqlParameter returnValue = new SqlParameter("return_value", SqlDbType.NVarChar);
                returnValue.Size = 50;
                returnValue.Direction = ParameterDirection.Output;
                command.Parameters.Add(returnValue);
                var returnDescription = new SqlParameter("return_description", SqlDbType.VarChar);
                returnDescription.Size = 50;
                returnDescription.Direction = ParameterDirection.Output;
                command.Parameters.Add(returnDescription);
                command.Parameters.Add(new SqlParameter("@input_reaction_type", reaction_type));
                command.CommandType = CommandType.StoredProcedure;
                conn.Open();
                command.ExecuteNonQuery();

                /*string str = (string)returnValue.Value;
                string emoji = string.Join("", (from Match m in Regex.Matches(str, @"\S{4}")
                                                select (char)int.Parse(m.Value, NumberStyles.HexNumber)).ToArray());
                */
                Console.WriteLine("Emoji from database: " + (string)returnDescription.Value);

                return (string)returnValue.Value;
            }
        }

        static public long GetRandomEmote(string reaction_type)
        {
            using (SqlConnection conn = new SqlConnection(MySQLConnString.Get()))
            {
                SqlCommand command = new SqlCommand("GetRandomEmote", conn);
                SqlParameter returnValue = new SqlParameter("return_emote_id", SqlDbType.BigInt);
                returnValue.Direction = ParameterDirection.Output;
                command.Parameters.Add(returnValue);
                command.Parameters.Add(new SqlParameter("@input_reaction_type", reaction_type));
                command.Parameters.Add(new SqlParameter("@input_server_id", (Int64)BotConfiguration.ServerID));
                command.CommandType = CommandType.StoredProcedure;
                conn.Open();
                command.ExecuteNonQuery();
                Console.WriteLine("Emote from database: " + returnValue.Value.ToString());

                return (long)returnValue.Value;
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

        static public ulong GetChannelID(string ChannelType)
        {
            using (SqlConnection conn = new SqlConnection(MySQLConnString.Get()))
            {
                SqlCommand command = new SqlCommand("GetChannelID", conn);
                SqlParameter returnValue = new SqlParameter("@result", SqlDbType.BigInt);
                returnValue.Direction = ParameterDirection.Output;
                command.Parameters.Add(returnValue);
                command.Parameters.Add(new SqlParameter("@input_server_ID", (Int64)BotConfiguration.ServerID));
                command.Parameters.Add(new SqlParameter("@input_channel_type", ChannelType));
                command.CommandType = CommandType.StoredProcedure;
                conn.Open();
                command.ExecuteNonQuery();
                var returnval = (long)returnValue.Value;
                return (ulong)returnval;
            }
        }

        static public ulong GetAdminbotRoleID()
        {

            using (SqlConnection conn = new SqlConnection(MySQLConnString.Get()))
            {
                //create a channel and pass the stored procedure the channel ID
                //Get the role ID for adminbot role on this current server,
                //Which is used to grant adminbots access to the users private channel.
                SqlCommand command = new SqlCommand("GetAdminbotRoleID", conn);
                command.Parameters.Add(new SqlParameter("@current_server", (Int64)BotConfiguration.ServerID));
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

        static public string GetVersion()
        {
            using (SqlConnection conn = new SqlConnection(MySQLConnString.Get()))
            {
                //Now that the private channel has been created, add it to the database.
                SqlCommand command = new SqlCommand("GetVersion", conn);
                SqlParameter returnValue = new SqlParameter("@version_string", SqlDbType.VarChar);
                returnValue.Direction = ParameterDirection.Output;
                returnValue.Size = 10;
                command.Parameters.Add(returnValue);
                command.CommandType = CommandType.StoredProcedure;
                conn.Open();
                command.ExecuteNonQuery();
                return (string)returnValue.Value;
            }
        }

        static public void BumpPatch()
        {
            using (SqlConnection conn = new SqlConnection(MySQLConnString.Get()))
            {
                //Now that the private channel has been created, add it to the database.
                SqlCommand command = new SqlCommand("BumpVersion", conn);
                command.Parameters.Add(new SqlParameter("@selector", Convert.ToInt64(0)));
                command.CommandType = CommandType.StoredProcedure;
                conn.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
