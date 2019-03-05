using Discord.Commands;
using Westbot.Preconditions;
using System.Threading.Tasks;
using System.Linq;
using System;
using Discord;

namespace Westbot.Services
{
    [Name("LFG commands")]
    public class LookingForGamesModule : ModuleBase<SocketCommandContext>
    {
        [Command("LFG"), Alias("lfg", "Lfg", "LFg")]
        [Remarks("Bot will post that you are looking for games.")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Lfg([Remainder]String arg = "")
        {
            await Lfg_function();
            if(arg != "") //an arg was passed
            //add the role
                await Lfg_addrole();
            return WestbotCommandResult.AcceptReact("lfgsuccess");
        }

        [Command("LFGr"), Alias("lfgr", "Lfgr", "LFgr")]
        [Remarks("Bot will post that you are looking for games.")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Lfgr([Remainder]String arg = "")
        {
            await Lfg_function();
            await Lfg_addrole();

            return WestbotCommandResult.AcceptReact("lfgsuccess");
        }

        public async Task Lfg_function()
        {
            var channel = CurrentConfiguration.GetChannel("looking for games");
            var day = DateTime.Today.DayOfWeek;
            DateTime time = DateTime.Now;
            var timestr = time.ToString("h:mm tt");
            var mention = Context.User.Mention;

            await channel.SendMessageAsync($"{mention} is looking for games. [{day}, {timestr}]");
        }

        public async Task Lfg_addrole()
        {
            string role_name = CurrentConfiguration.GetChannelName("LFG");
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == role_name);
            await (Context.User as IGuildUser).AddRoleAsync(role);
        }
    }        
}