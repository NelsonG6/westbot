using Discord.Commands;
using Westbot.Preconditions;
using System.Threading.Tasks;
using System.Linq;
using System;
using Discord;
using WestBot;

namespace Westbot
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
            return WestbotCommandResult.AcceptReact("lfgsuccess");
        }

        [Command("LFGr"), Alias("lfgr", "Lfgr", "LFgr")]
        [Remarks("Bot will post that you are looking for games.")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Lfgr([Remainder]String arg = "")
        {
            await Lfg_function();

            return WestbotCommandResult.AcceptReact("lfgsuccess");
        }

        public async Task Lfg_function()
        {
            ulong channel_ID = DatabaseHandler.GetChannelID("LFG");
            var day = DateTime.Today.DayOfWeek;
            DateTime time = DateTime.Now;
            var timestr = time.ToString("h:mm tt");
            var mention = Context.User.Mention;

            IMessageChannel channel = (IMessageChannel)Context.Guild.Channels.FirstOrDefault(x => x.Id == channel_ID);

            await channel.SendMessageAsync($"{mention} is looking for games. [{day}, {timestr}]");
        }

    }        
}