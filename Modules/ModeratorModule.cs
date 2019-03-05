using Discord.Commands;
using Discord.WebSocket;
using Westbot.Preconditions;
using System.Threading.Tasks;

namespace Westbot.Services
{
    [Name("Moderator Commands")]
    [RequireContext(ContextType.Guild)]
    public class ModeratorModule : ModuleBase<SocketCommandContext>
    {
        [Command("kick")]
        [Remarks("Kick the specified user.")]
        [MinPermissions(AccessLevel.ServerMod)]
        public async Task Kick([Remainder]SocketGuildUser user)
        {
            await ReplyAsync($"cya {user.Mention} :wave:");
            await user.KickAsync();
        }
    }

    //createrole and setrole will go here

}
