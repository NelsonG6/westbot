using Discord.Commands;
using System.Linq;
using System.Threading.Tasks;
using Westbot.Preconditions;
using Discord;
using Discord.WebSocket;
using System.Data.SqlClient;
using System.Data;
using System;
using WestBot;

namespace Westbot.Modules
{
    [Name("Posting in various channels")]
    public class PostsModule : ModuleBase<SocketCommandContext>
    {
        [Command("post")]
        [Remarks("Command that allows users to post to a certain channel.")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Post(string channel_string = "", [Remainder]string post_string = "")
        {
            var channel = Context.Guild.TextChannels.FirstOrDefault(x => x.Name == channel_string);
            await channel.SendMessageAsync("Posted by " + Context.User.Mention + "\n" + post_string);
            return WestbotCommandResult.AcceptReact("postsuccess");
        }

        [Command("post")]
        [Remarks("testing")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Post2(SocketChannel channel, [Remainder]string post_string = "")
        {
            var post_channel = Context.Guild.GetTextChannel(channel.Id);
            await post_channel.SendMessageAsync("Posted by " + Context.User.Mention + "\n" + post_string);
            return WestbotCommandResult.AcceptReact("postsuccess");
        }

        [Command("stream")]
        [Remarks("Post a stream to the stream channel.")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Post3([Remainder]string post_string = "")
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(MySQLConnString.Get()))
                {
                    SqlCommand command = new SqlCommand("GetChannelID", conn);
                    SqlParameter returnValue = new SqlParameter("@result", SqlDbType.BigInt);
                    returnValue.Direction = ParameterDirection.Output;
                    command.Parameters.Add(returnValue);
                    command.Parameters.Add(new SqlParameter("@serverID", Context.Guild.Id.ToString()));
                    command.Parameters.Add(new SqlParameter("@target_channel", "Stream"));

                    command.CommandType = CommandType.StoredProcedure;
                    //command.Parameters.Add(new SqlParameter("@ID", (Int64)ID));

                    conn.Open();

                    command.ExecuteNonQuery();

                    long result2 = (long)returnValue.Value;
                    UInt64 result = (UInt64)result2;

                    var stream_channel = Context.Guild.GetTextChannel(result);
                    await stream_channel.SendMessageAsync("Posted by " + Context.User.Mention + "\n" + post_string);
                    return WestbotCommandResult.AcceptReact("postsuccess");

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