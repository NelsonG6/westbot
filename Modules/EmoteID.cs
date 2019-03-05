using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Westbot.Preconditions;
using System.IO;
using Newtonsoft.Json;

using Discord;


using Discord.WebSocket;
using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace Westbot.Services
{
    [Name("Debugging")]

    public class EmoteID : ModuleBase<SocketCommandContext>
    {
        [Command("showemote"), Alias("show emote", "getemote", "get emote")]
        [Remarks("Gets an emote and outputs the string")]
        [MinPermissions(AccessLevel.ServerMod)]
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
