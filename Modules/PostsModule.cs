using Discord.Commands;
using System.Linq;
using System.Threading.Tasks;
using Westbot.Preconditions;
using Discord;
using Discord.WebSocket;

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

    }
}