using Discord.Commands;
using Discord.WebSocket;
using Westbot.Preconditions;
using System.Threading.Tasks;
using Discord;
using System;
using System.Linq;
using System.Collections.Generic;

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

        [Command("deleteinlast"), Alias("removelast")]
        [Remarks("Kick the specified user.")]
        [MinPermissions(AccessLevel.ServerMod)]
        public async Task deleteinlast([Remainder]int seconds)
        {
            // Search all channels, and delete any posts that were made in the timeframe specified
            var currentTime = DateTime.Now;
            
            var channels = Context.Guild.Channels;
            foreach(ITextChannel channel in channels)
            {
                // Get the most recent message
                IMessage recentMessage = (IMessage)channel.GetMessagesAsync(1);
                var time = recentMessage.Timestamp;
            }
        }

        [Command("hackcleanup")]
        [Remarks("Clean up after the last hack.")]
        [MinPermissions(AccessLevel.ServerMod)]
        public async Task HackCleanUp([Remainder]int Seconds)
        {
            // Get all channels
            // Loop through all channels
            
            
            // Search all channels, and delete any posts that were made in the timeframe specified
            var currentTime = DateTime.Now;

            /*

            foreach (ITextChannel channel in channels)
            {
                // Get the most recent message
                IMessage recentMessage = (IMessage)channel.GetMessagesAsync(1);
                var time = recentMessage.Timestamp;
            }

            */

        }

        [Command("deletetest")]
        [Remarks("Test deleting some bot posts.")]
        [MinPermissions(AccessLevel.ServerMod)]
        public async Task deletetest([Remainder]string remainder = "")
        {
            // Use UTC timezone
            // var currentTime = DateTime.UtcNow;

            int messageCount = 10;

            // Get my private channel
            // Get all channels

            var channels = Context.Guild.TextChannels;

            Console.WriteLine($"{channels.Count} channels found.");

            int count = 1;

            foreach(ITextChannel InterfaceTextChannel in channels)
            {
                //ITextChannel InterfaceTextChannel = Context.Guild.GetTextChannel(625897353597157427);

                // var currentTime = DateTime.Now;

                // Get 10 last messages

                // if(channel.)

                var botID = Context.Client.CurrentUser.Id;
                var recentMessages = await InterfaceTextChannel.GetMessagesAsync(messageCount).FlattenAsync();

                List<string> outputs = new List<string>();

                outputs.Add($"[Channel {count}]Looking at the last {messageCount} messages for channel {InterfaceTextChannel.Name}.");

                foreach (var recentMessage in recentMessages)
                {
                    if (recentMessage.Author.Id == botID)
                    {
                        if (recentMessage.Content.Contains("nigger"))
                        {
                            await recentMessage.DeleteAsync();
                            outputs.Add("Deleted vulgar message.");
                        }
                        else
                        {
                            outputs.Add("Message was posted by the bot, but did not contain vulgarity.");
                        }
                    }
                    else
                    {
                        outputs.Add("Message was not posted by the bot.");
                    }
                }

                string toWrite = String.Join("\n", outputs);

                Console.WriteLine(toWrite);
                count++;
            }
        }
    }

}
