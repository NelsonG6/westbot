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
            await Lfg_function("LFG-SFV");
            return WestbotCommandResult.AcceptReact("lfgsuccess");
        }

        [Command("LFGr"), Alias("lfgr", "Lfgr", "LFgr")]
        [Remarks("Bot will post that you are looking for games.")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Lfgr([Remainder]String arg = "")
        {
            await Lfg_function("LFG-SFV");

            return WestbotCommandResult.AcceptReact("lfgsuccess");
        }

        [Command("LFG granblue"), Alias("LFG gb", "LFG-gb", "lfggb")]
        [Remarks("Bot will post that you are looking for games.")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> LfgGranblue([Remainder]String arg = "")
        {
            await Lfg_function("LFG-Granblue");

            return WestbotCommandResult.AcceptReact("lfgsuccess");
        }

        [Command("LFG GG"), Alias("LFG-gg", "lfg guilty gear")]
        [Remarks("Bot will post that you are looking for games.")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> LfgGG([Remainder]String arg = "")
        {
            await Lfg_function("LFG-GG");

            return WestbotCommandResult.AcceptReact("lfgsuccess");
        }

        public async Task Lfg_function(string channelType)
        {
            ulong channelID = DatabaseHandler.GetChannelID(channelType);
            var day = DateTime.Today.DayOfWeek;
            DateTime time = DateTime.Now;
            var timestr = time.ToString("h:mm tt");
            var mention = Context.User.Mention;

            IMessageChannel channel = (IMessageChannel)Context.Guild.Channels.FirstOrDefault(x => x.Id == channelID);

            await channel.SendMessageAsync($"{mention} is looking for games. [{day}, {timestr}]");
        }

    }        
}