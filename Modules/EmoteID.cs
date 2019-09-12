using System.Threading.Tasks;
using Discord.Commands;
using Westbot.Preconditions;

namespace Westbot
{
    [Name("Debugging")]

    public class EmoteID : ModuleBase<SocketCommandContext>
    {
        [Command("showemote"), Alias("show emote", "getemote", "get emote")]
        [Remarks("Gets an emote and outputs the string")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> ShowEmote([Remainder]string arg = "")
        {
            if (string.IsNullOrEmpty(arg))
                return WestbotCommandResult.ErrorReact("empty in show emote");

            //arg = arg.Replace(":", ""); //remove the :

            await ReplyAsync($"{arg}");
            return WestbotCommandResult.AcceptReact();
        }
    }
}
