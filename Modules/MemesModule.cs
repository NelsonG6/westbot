using Discord.Commands;
using Westbot.Preconditions;
using System.Threading.Tasks;
using System;
using System.IO;
using Newtonsoft.Json;

using System.Collections.Generic;

namespace Westbot.Services
{
    [Name("Memes")]

    public class MemesModule : ModuleBase<SocketCommandContext>
    {
        //These commands have emote-substitutes
        [Command("arnold"), Alias("terminator", "t1000", "T1000",
            "juiceman", "terminated", "hasta la vista", "term", "steroid user", 
            "<:terminator:333699069643849729>")]
        [Remarks("T1000")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> T1000([Remainder]String arg = "")
        {
            var filename = "arnold.png";
            string path = Path.Combine(AppContext.BaseDirectory, "Uploads", filename);
            await Context.Channel.SendFileAsync(path);
            return WestbotCommandResult.AcceptNoReact();
        }
        [Command("grinch"), Alias("<:grinch:431949955603628032>" , "<:grinch:431949858077933568>")]
        [Remarks("grinch")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Grinch([Remainder]String arg = "")
        {
            var filename = "grinch.gif";
            string path = Path.Combine(AppContext.BaseDirectory, "Uploads", filename);

            await Context.Channel.SendFileAsync(path);
            return WestbotCommandResult.AcceptNoReact();
        }

        [Command("mjcry"), Alias("<:mjcry:306930362943143937>", "<:mjcry:330212258993012747>")]
        [Remarks("mj")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> MJcry([Remainder]String arg = "")
        {
            var filename = "mjcry.gif";
            string path = Path.Combine(AppContext.BaseDirectory, "Uploads", filename);

            await Context.Channel.SendFileAsync(path);
            return WestbotCommandResult.AcceptNoReact();
        }
        [Command("penguin"), Alias("🐧")]
        [Remarks("penguin")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Penguin([Remainder]String arg = "")
        {
            var filename = "penguin.gif";
            string path = Path.Combine(AppContext.BaseDirectory, "Uploads", filename);

            await Context.Channel.SendFileAsync(path);
            return WestbotCommandResult.AcceptNoReact();
        }

        [Command("noatmosphere"), Alias("no atmosphere", "<:noatmosphere:377372969967550465>", "<:noatmosphere:377395230997217281>")]
        [Remarks("totalrecall2")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Noatmosphere2([Remainder]String arg = "")

        {
            var filename = "noatmosphere.gif";
            string path = Path.Combine(AppContext.BaseDirectory, "Uploads", filename);
            await Context.Channel.SendFileAsync(path);
            return WestbotCommandResult.AcceptNoReact();
        }

        [Command("stare"), Alias("<:stare:485009331436126219>", "staring", "12 monkeys", "<:stare:485217505263222787>")]
        [Remarks("12 monkeys")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Stare([Remainder]String arg = "")
        {
            var filename = "staring.gif";
            string path = Path.Combine(AppContext.BaseDirectory, "Uploads", filename);
            await Context.Channel.SendFileAsync(path);
            return WestbotCommandResult.AcceptNoReact();
        }

        [Command("mindblown"), Alias("Mindblown", "<galaxybrain499025178693468181>", "<:galaxybrain:498737219817963542>")]
        [Remarks("mindblown")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Mindblown([Remainder]String arg = "")
        {
            var filename = "mindblown.gif";
            string path = Path.Combine(AppContext.BaseDirectory, "Uploads", filename);

            await Context.Channel.SendFileAsync(path);
            return WestbotCommandResult.AcceptNoReact();
        }
        [Command("yes"), Alias("<:yes:456337505541685249>")]
        [Remarks("yes")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Yes([Remainder]String arg = "")
        {
            var filename = "yes.gif";
            string path = Path.Combine(AppContext.BaseDirectory, "Uploads", filename);

            await Context.Channel.SendFileAsync(path);
            return WestbotCommandResult.AcceptNoReact();
        }

        [Command("obamamic"), Alias("Obamamic", "<:obama:441684724281376770>", "<:obamawhew:306930386292703233>")]
        [Remarks("obama")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Obama([Remainder]String arg = "")
        {
            var filename = "obamamic.gif";
            string path = Path.Combine(AppContext.BaseDirectory, "Uploads", filename);

            await Context.Channel.SendFileAsync(path);
            return WestbotCommandResult.AcceptNoReact();
        }

        [Command("toast"), Alias("toastman", "<:toastman:465469059236888578>", "<:toastman:464923916599558154>")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Toast([Remainder]String arg = "")
        {
            var filename = "toast.gif";
            string path = Path.Combine(AppContext.BaseDirectory, "Uploads", filename);

            await Context.Channel.SendFileAsync(path);
            return WestbotCommandResult.AcceptNoReact();
        }
        [Command("free"), Alias("<:freerealestate:453347925033222166>", "🆓")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Free([Remainder]String arg = "")
        {
            var filename = "free.gif";
            await Context.Channel.SendFileAsync(Path.Combine(AppContext.BaseDirectory, "Uploads", filename));
            return WestbotCommandResult.AcceptNoReact();
        } 

        //Commands without emote aliases
        [Command("blink"), Alias("blinking")]
        [Remarks("Blinking guy gif")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Blinking([Remainder]String arg = "")
        {
            var filename = "blinking.gif";
            string path = Path.Combine(AppContext.BaseDirectory, "Uploads", filename);
            await Context.Channel.SendFileAsync(path);
            return WestbotCommandResult.AcceptNoReact();
        }

        [Command("yeet"), Alias("Yeet")]
        [Remarks("dancing black guy")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Yeet([Remainder]String arg = "")
        {
            var filename = "yeetguy.gif";
            string path = Path.Combine(AppContext.BaseDirectory, "Uploads", filename);
            await Context.Channel.SendFileAsync(path);
            return WestbotCommandResult.AcceptNoReact();
        }

        [Command("noatmospherelarge"), Alias("no atmosphere large")]
        [Remarks("totalrecall")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Nomtmospherelarge([Remainder]String arg = "")
        {
            await ReplyAsync("https://gfycat.com/SlimyDarkKagu");
            return WestbotCommandResult.AcceptNoReact();
        }

        [Command("picard"), Alias("Picard", "Jean Luc Picard")]
        [Remarks("star trek")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Picard([Remainder]String arg = "")
        {
            var filename = "picard.gif";
            string path = Path.Combine(AppContext.BaseDirectory, "Uploads", filename);
            await Context.Channel.SendFileAsync(path);
            return WestbotCommandResult.AcceptNoReact();
        }

        [Command("rikerzoom"), Alias("Rikerzoom")]
        [Remarks("riker")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Riker([Remainder]String arg = "")
        {
            var filename = "rikerzoom.gif";
            string path = Path.Combine(AppContext.BaseDirectory, "Uploads", filename);

            await Context.Channel.SendFileAsync(path);
            return WestbotCommandResult.AcceptNoReact();
        }

        

        [Command("sweatpeele"), Alias("Sweatpeele")]
        [Remarks("obama")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Sweatpeele([Remainder]String arg = "")
        {
            var filename = "sweatpeele.gif";
            string path = Path.Combine(AppContext.BaseDirectory, "Uploads", filename);

            await Context.Channel.SendFileAsync(path);
            return WestbotCommandResult.AcceptNoReact();
        }

        [Command("blackjet"), Alias("Blackjet")]
        [Remarks("jet")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Blackjet([Remainder]String arg = "")
        {
            var filename = "blackjet.gif";
            string path = Path.Combine(AppContext.BaseDirectory, "Uploads", filename);

            await Context.Channel.SendFileAsync(path);
            return WestbotCommandResult.AcceptNoReact();
        }

        [Command("sailormoon"), Alias("Sailormoon")]
        [Remarks("obama")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Sailormoon([Remainder]String arg = "")
        {
            var filename = "sailormoon.gif";
            string path = Path.Combine(AppContext.BaseDirectory, "Uploads", filename);

            await Context.Channel.SendFileAsync(path);
            return WestbotCommandResult.AcceptNoReact();
        }

        [Command("dylan"), Alias("Dylan")]
        [Remarks("dylan")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Dylan([Remainder]String arg = "")
        {
            var filename = "dylan.gif";
            string path = Path.Combine(AppContext.BaseDirectory, "Uploads", filename);

            await Context.Channel.SendFileAsync(path);
            return WestbotCommandResult.AcceptNoReact();
        }

        [Command("pyle"), Alias("Pyle", "Full metal jacket", "full metal jacket")]
        [Remarks("pyle")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Pyle([Remainder]String arg = "")
        {
            var filename = "pyle.gif";
            string path = Path.Combine(AppContext.BaseDirectory, "Uploads", filename);

            await Context.Channel.SendFileAsync(path);
            return WestbotCommandResult.AcceptNoReact();
        }

        

        

        [Command("peace"), Alias("Peace")]
        [Remarks("peace")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Peace([Remainder]String arg = "")
        {
            var filename = "peace.gif";
            string path = Path.Combine(AppContext.BaseDirectory, "Uploads", filename);

            await Context.Channel.SendFileAsync(path);
            return WestbotCommandResult.AcceptNoReact();
        }

        
        [Command("sweatairplane"), Alias("Sweatairplane")]
        [Remarks("sweatairplane")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Sweatairplane([Remainder]String arg = "")
        {
            var filename = "sweatairplane.gif";
            string path = Path.Combine(AppContext.BaseDirectory, "Uploads", filename);

            await Context.Channel.SendFileAsync(path);
            return WestbotCommandResult.AcceptNoReact();
        }

        [Command("gumbee"), Alias("Gumbee")]
        [Remarks("gumbee")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Gumbee([Remainder]String arg = "")
        {
            var filename = "gumbee.gif";
            string path = Path.Combine(AppContext.BaseDirectory, "Uploads", filename);

            await Context.Channel.SendFileAsync(path);
            return WestbotCommandResult.AcceptNoReact();
        }

        [Command("squirrel"), Alias("Squirrel")]
        [Remarks("squirrel")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Squirrel([Remainder]String arg = "")
        {
            var filename = "squirrel.gif";
            string path = Path.Combine(AppContext.BaseDirectory, "Uploads", filename);

            await Context.Channel.SendFileAsync(path);
            return WestbotCommandResult.AcceptNoReact();
        }

        
        [Command("beard"), Alias("Beard")]
        [Remarks("beard")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Beard([Remainder]String arg = "")
        {
            var filename = "beard.gif";
            string path = Path.Combine(AppContext.BaseDirectory, "Uploads", filename);

            await Context.Channel.SendFileAsync(path);
            return WestbotCommandResult.AcceptNoReact();
        }

        [Command("crack"), Alias("Crack")]
        [Remarks("crack")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Crack([Remainder]String arg = "")
        {
            var filename = "crack.gif";
            string path = Path.Combine(AppContext.BaseDirectory, "Uploads", filename);

            await Context.Channel.SendFileAsync(path);
            return WestbotCommandResult.AcceptNoReact();
        }

        

        [Command("skynet"), Alias("Skynet")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Skynet([Remainder]String arg = "")
        {
            var filename = "skynet.gif";
            string path = Path.Combine(AppContext.BaseDirectory, "Uploads", filename);

            await Context.Channel.SendFileAsync(path);
            return WestbotCommandResult.AcceptNoReact();
        }

        [Command("riker"), Alias("riker1", "Riker", "Riker1")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Riker1([Remainder]String arg = "")
        {
            var filename = "riker1.png";
            string path = Path.Combine(AppContext.BaseDirectory, "Uploads", filename);

            await Context.Channel.SendFileAsync(path);
            return WestbotCommandResult.AcceptNoReact();
        }

        [Command("riker2"), Alias("Riker2")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Riker2([Remainder]String arg = "")
        {
            var filename = "riker2.png";
            string path = Path.Combine(AppContext.BaseDirectory, "Uploads", filename);

            await Context.Channel.SendFileAsync(path);
            return WestbotCommandResult.AcceptNoReact();
        }

        [Command("kirby"), Alias("Kirby")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Kirby([Remainder]String arg = "")
        {
            var filename = "Kirby.gif";
            string path = Path.Combine(AppContext.BaseDirectory, "Uploads", filename);

            await Context.Channel.SendFileAsync(path);
            return WestbotCommandResult.AcceptNoReact();
        }

        [Command("spin"), Alias("Spin")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Spin([Remainder]String arg = "")
        {
            var filename = "spin.gif";
            string path = Path.Combine(AppContext.BaseDirectory, "Uploads", filename);

            await Context.Channel.SendFileAsync(path);
            return WestbotCommandResult.AcceptNoReact();
        }

        [Command("g"), Alias("G")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> G([Remainder]String arg = "")
        {
            var filename = "g.gif";
            string path = Path.Combine(AppContext.BaseDirectory, "Uploads", filename);

            await Context.Channel.SendFileAsync(path);
            return WestbotCommandResult.AcceptNoReact();
        }



        [Command("agree"), Alias("Agree", "iagree", "IAgree", "iagree")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Agree([Remainder]String arg = "")
        {
            var filename = "agree.gif";
            await Context.Channel.SendFileAsync(Path.Combine(AppContext.BaseDirectory, "Uploads", filename));
            return WestbotCommandResult.AcceptNoReact();
        }

        [Command("bloodyclaw"), Alias("Bloodyclaw", "claw", "Claw", "blood", "Blood")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Claw([Remainder]String arg = "")
        {
            var filename = "bloodyclaw.gif";
            await Context.Channel.SendFileAsync(Path.Combine(AppContext.BaseDirectory, "Uploads", filename));
            return WestbotCommandResult.AcceptNoReact();
        }

        [Command("bongo"), Alias("Bongo", "Bongos", "bongos")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Bongo([Remainder]String arg = "")
        {
            var filename = "bongo.gif";
            await Context.Channel.SendFileAsync(Path.Combine(AppContext.BaseDirectory, "Uploads", filename));
            return WestbotCommandResult.AcceptNoReact();
        }

        [Command("carlin"), Alias("Carlin", "jerkoff", "Jerkoff", "Georgecarlin", "georgecarlin")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Carlin([Remainder]String arg = "")
        {
            var filename = "carlin.gif";
            await Context.Channel.SendFileAsync(Path.Combine(AppContext.BaseDirectory, "Uploads", filename));
            return WestbotCommandResult.AcceptNoReact();
        }

        [Command("dab"), Alias("Dab")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Dab([Remainder]String arg = "")
        {
            var filename = "dab.gif";
            await Context.Channel.SendFileAsync(Path.Combine(AppContext.BaseDirectory, "Uploads", filename));
            return WestbotCommandResult.AcceptNoReact();
        }

        [Command("endme"), Alias("Endme", "End me", "end me")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> End([Remainder]String arg = "")
        {
            var filename = "endme.gif";
            await Context.Channel.SendFileAsync(Path.Combine(AppContext.BaseDirectory, "Uploads", filename));
            return WestbotCommandResult.AcceptNoReact();
        }

        

        [Command("fuckyocouch"), Alias("Fuckyocouch", "Fuckyourcouch", "fuckyourcouch")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Couch([Remainder]String arg = "")
        {
            var filename = "fuckyocouch.gif";
            await Context.Channel.SendFileAsync(Path.Combine(AppContext.BaseDirectory, "Uploads", filename));
            return WestbotCommandResult.AcceptNoReact();
        }


        [Command("redshirt"), Alias("Reddshirt")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> Redshirt([Remainder]String arg = "")
        {
            var filename = "redshirt.gif";
            await Context.Channel.SendFileAsync(Path.Combine(AppContext.BaseDirectory, "Uploads", filename));
            return WestbotCommandResult.AcceptNoReact();
        }


        [Command("skelton"), Alias("Skelton", "Skeleton", "skeleton", "Skeltonjohn", "skeltonjohn")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> skelton([Remainder]String arg = "")
        {
            var filename = "skelton.gif";
            await Context.Channel.SendFileAsync(Path.Combine(AppContext.BaseDirectory, "Uploads", filename));
            return WestbotCommandResult.AcceptNoReact();
        }

        [Command("what"), Alias("What")]
        [MinPermissions(AccessLevel.User)]
        public async Task<RuntimeResult> What([Remainder]String arg = "")
        {
            var filename = "what.gif";
            await Context.Channel.SendFileAsync(Path.Combine(AppContext.BaseDirectory, "Uploads", filename));
            return WestbotCommandResult.AcceptNoReact();
        }
    }

    //add worm
}