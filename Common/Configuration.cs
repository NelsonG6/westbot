using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Discord.Commands;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using WestBot;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Westbot
{
    //Move this config functionality to the sql database
    public static class BotConfiguration
    {   //The configuration class. Renamed to "CurrentConfiguration" to be easier to follow when used elsewhere in code.
        public static string ConfigurationName { get; set; } = "";
        public static char Prefix { get; set; } = '\0';
        public static string Token { get; set; } = "";

        //public static ulong ServerID { get; set; } = 0;
        
        public static ulong GetChannelID(string target_channel, ulong ServerID)
        {
            //Channels stored:
            //looking for games
            //user join

            try
            {
                using (SqlConnection conn = new SqlConnection(MySQLConnString.Get()))
                {
                    SqlCommand command = new SqlCommand("GetChannelID", conn);
                    SqlParameter returnValue = new SqlParameter("@result", SqlDbType.BigInt);
                    returnValue.Direction = ParameterDirection.Output;
                    command.Parameters.Add(returnValue);
                    command.Parameters.Add(new SqlParameter("@serverID", ServerID));
                    command.Parameters.Add(new SqlParameter("@target_channel", "Stream"));

                    conn.Open();
                    command.CommandType = CommandType.StoredProcedure;
                    command.ExecuteNonQuery();

                    long result2 = (long)returnValue.Value;
                    UInt64 result = (UInt64)result2;

                    return result;
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return 0;
            }
        }

        public static void Load(string[] config_name)
        {
            if (config_name.Length <= 0)
                ConfigurationName = "Testbot";
            else
                ConfigurationName = config_name[0];
            
            // load config from database
            try
            {
                using (SqlConnection conn = new SqlConnection(MySQLConnString.Get()))
                {
                    SqlParameter param_token = new SqlParameter("@return_token", SqlDbType.NVarChar);
                    param_token.Direction = ParameterDirection.Output;
                    param_token.Size = 100;

                    SqlParameter param_prefix = new SqlParameter("@return_prefix", SqlDbType.Char);
                    param_prefix.Direction = ParameterDirection.Output;
                    param_prefix.Size = 1;

                    SqlCommand command = new SqlCommand("GetConfiguration", conn);
                    command.Parameters.Add(param_token);
                    command.Parameters.Add(param_prefix);
                    if(config_name.Length == 0)
                        command.Parameters.Add(new SqlParameter("@input_config_name", "Testbot"));
                    else
                        command.Parameters.Add(new SqlParameter("@input_config_name", config_name[0]));
                    
                    command.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    
                    command.ExecuteNonQuery();

                    Token = (string)param_token.Value;

                    Prefix = ((string)param_prefix.Value).ToCharArray()[0];

                    /*
                    SqlParameter param_serverID = new SqlParameter("@return_server_id", SqlDbType.BigInt);
                    command.Parameters.Add(param_serverID);
                    param_serverID.Direction = ParameterDirection.Output;
                    var result = (long)param_serverID.Value;
                    ServerID = (UInt64)result;
                    */

                    Console.WriteLine("Configuration Loaded.");
                    Console.WriteLine("Config name: " + ConfigurationName);
                    Console.WriteLine("This programs path: " + AppContext.BaseDirectory);
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                return;
            }
            //no match was found
        }    

        async public static Task GenerateReaction(ICommandContext Context, bool error_state)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(MySQLConnString.Get()))
                {
                    string reaction_type;
                    if (!error_state)
                        reaction_type = "Accept";
                    else
                        reaction_type = "Error";

                    SqlCommand command = new SqlCommand("EmoteOrEmoji", conn);
                    SqlParameter newparam = new SqlParameter("@input_reaction_type", reaction_type);
                    command.Parameters.Add(newparam);
                    command.Parameters.Add(new SqlParameter("@input_server_id", (Int64)Context.Guild.Id));
                    SqlParameter returnValue = new SqlParameter("result", SqlDbType.Int);
                    returnValue.Direction = ParameterDirection.Output;
                    command.Parameters.Add(returnValue);

                    conn.Open();
                    command.CommandType = CommandType.StoredProcedure;
                    command.ExecuteNonQuery();

                    if ((int)returnValue.Value == 0)
                    {   //Emojis
                        command = new SqlCommand("GetRandomEmoji", conn);
                        returnValue = new SqlParameter("return_value", SqlDbType.NVarChar);
                        returnValue.Size = 50;
                        returnValue.Direction = ParameterDirection.Output;
                        command.Parameters.Add(returnValue);
                        var returnDescription = new SqlParameter("return_description", SqlDbType.VarChar);
                        returnDescription.Size = 50;
                        returnDescription.Direction = ParameterDirection.Output;
                        command.Parameters.Add(returnDescription);
                        command.Parameters.Add(new SqlParameter("@input_reaction_type", reaction_type));
                        command.CommandType = CommandType.StoredProcedure;
                        command.ExecuteNonQuery();

                        /*string str = (string)returnValue.Value;
                        string emoji = string.Join("", (from Match m in Regex.Matches(str, @"\S{4}")
                                                        select (char)int.Parse(m.Value, NumberStyles.HexNumber)).ToArray());
                        */
                        Console.WriteLine("Emoji from database: " + (string)returnDescription.Value);

                        Emoji emoji_to_add = new Emoji((string)returnValue.Value);
                        await Context.Message.AddReactionAsync(emoji_to_add);

                    }
                    else
                    {   //emotes
                        command = new SqlCommand("GetRandomEmote", conn);
                        returnValue = new SqlParameter("return_emote_id", SqlDbType.BigInt);
                        returnValue.Direction = ParameterDirection.Output;
                        command.Parameters.Add(returnValue);
                        command.Parameters.Add(new SqlParameter("@input_reaction_type", reaction_type));
                        command.Parameters.Add(new SqlParameter("@input_server_id", (Int64)Context.Guild.Id));
                        command.CommandType = CommandType.StoredProcedure;
                        command.ExecuteNonQuery();

                        long result2 = (long)returnValue.Value;
                        UInt64 result = (UInt64)result2;

                        Console.WriteLine("Emote from database: " + result.ToString());

                        IEmote emote_to_add = Context.Guild.Emotes.FirstOrDefault(x => x.Id == result);
                        //var emote_to_add = Context.Guild.GetEmoteAsync(result);
                        
                        await Context.Message.AddReactionAsync((IEmote)emote_to_add);                       
                    }
                }
            }

            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                await Context.Channel.SendMessageAsync("Exception: " + ex.Message);
            }
        }
    }
}