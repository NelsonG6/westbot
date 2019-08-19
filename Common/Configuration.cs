//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Discord.Commands;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Westbot
{
    public class Reaction
    { //Contains emotes and emojis; if description is equal to "emote", then this is an emote; otherwise it indicates this is an emoji.
        public string Name { get; set; }
        public string Description { get; set; } = "Emote";
    }

    public class ChannelData
    {
        //Channel IDs are associated to an easier to understand label using this object.
        public string SearchKey { get; set; } //The common name for the channel
        public string ServerSpecificName { get; set; } //The server-specific name for the channel
        public IMessageChannel Channel;
    }

    public class ConfigurationFromFile
    { //Used for non-static instances of configurations, only helpful at loadtime.
        public string ConfigurationName { get; set; } = "";
        public ulong[] Owners { get; set; }
        public char Prefix { get; set; } = '\0';
        public string Token { get; set; } = "";
        public List<Reaction> ErrorReactions { get; set; }
        public List<Reaction> AcceptReactions { get; set; }
        public List<ChannelData> ChannelDataList{ get; set; }

        public ulong JoinMessageChannel { get; set; }

        public ConfigurationFromFile()
        {
            ErrorReactions = new List<Reaction>();
            AcceptReactions = new List<Reaction>();
            ChannelDataList = new List<ChannelData>();
        }
    }

    public static class CurrentConfiguration
    {   //The configuration class. Renamed to "CurrentConfiguration" to be easier to follow when used elsewhere in code.
        public static string ConfigurationName { get; set; } = "";
        public static ulong[] Owners { get; set; }
        public static char Prefix { get; set; } = '\0';
        public static string Token { get; set; } = "";
        public static List<Reaction> ErrorReactions { get; set; }
        public static List<Reaction> AcceptReactions { get; set; }
        public static List<ChannelData> ChannelDataList { get; set; }
        

        public static void MergeIntoThis(ConfigurationFromFile copy_from)
        { //This takes a configuration as an argument and copies any popualted fields into the current configuration
            if (copy_from.Owners != null)
                Owners = copy_from.Owners;
            if (copy_from.Prefix != '\0')
                Prefix = copy_from.Prefix;
            if (copy_from.Token != null)
                Token = copy_from.Token;
            if (copy_from.ConfigurationName != null)
                ConfigurationName = copy_from.ConfigurationName;

            ErrorReactions.AddRange(copy_from.ErrorReactions);
            AcceptReactions.AddRange(copy_from.AcceptReactions);
            ChannelDataList.AddRange(copy_from.ChannelDataList);
        }

        public static string GetChannelName(string description)
        {
            //Channels stored:
            //looking for games
            //user join

            foreach (ChannelData current in ChannelDataList)
            {
                if (current.SearchKey.ToLower() == description.ToLower())
                    if (current.ServerSpecificName == "none")
                        return null;
                    else
                        return current.ServerSpecificName;
            }
            //no match was found
            return "";
        }

        public static Task LoadChannels(SocketGuild guild)
        {
            foreach (ChannelData current in ChannelDataList)
            {
                current.Channel = guild.TextChannels.FirstOrDefault(x => x.Name == current.ServerSpecificName);
            }

            return Task.CompletedTask;
        }

        public static IMessageChannel GetChannel(string to_get)
        {
            foreach(ChannelData current in ChannelDataList)
            {
                if (current.SearchKey.ToLower() == to_get.ToLower())
                    return current.Channel;
            }
            return null;
        }

        public static void Load(string[] call_string) //Reads from configuration.json and builds CurrentConfiguration
        {
            ErrorReactions = new List<Reaction>();
            AcceptReactions = new List<Reaction>();
            ChannelDataList = new List<ChannelData>();

            List <ConfigurationFromFile> ConfigurationsFromFile; //A list of configs from file for us to build from at loadtime

            string filename = "configuration.json"; //all config data is in here
            string path = Path.Combine(AppContext.BaseDirectory, filename);
            string configurationNameSearch = "default";

            if (!File.Exists(path))
            {
                Console.WriteLine("Failed to load config file.");
                return;// Check if the configuration file exists.
            }

            //Gets a list of configurations 
            ConfigurationsFromFile = JsonConvert.DeserializeObject<List<ConfigurationFromFile>>(File.ReadAllText(path));

            //replace default as the config name if we were passed an argument
            if (call_string.Length > 0)
                if (call_string[0].Length > 0)
                    configurationNameSearch = call_string[0];

            //Find the universal config
            foreach (ConfigurationFromFile current in ConfigurationsFromFile)
            {
                if (current.ConfigurationName.ToLower() == "universal")
                {
                    CurrentConfiguration.MergeIntoThis(current);
                    break;
                }
            }
            //The universal template is applied; the call-argument template will overwrite or merge with this.

            //Find the configuration that matches the call argument and assign it
            foreach (ConfigurationFromFile current in ConfigurationsFromFile)
            {
                if (configurationNameSearch.ToLower() == current.ConfigurationName.ToLower())
                {
                    CurrentConfiguration.MergeIntoThis(current);
                    break;
                }
            }

            Console.WriteLine("Configuration Loaded.");
            Console.WriteLine("Config name: " + CurrentConfiguration.ConfigurationName);
            Console.WriteLine("This programs path: " + AppContext.BaseDirectory);

            //get the new_warriors channel

        }    

        async public static Task GenerateReaction(ICommandContext Context, bool error_state)
        {
            List<Reaction> currentReactionList = new List<Reaction>();

            if (error_state)
                currentReactionList = ErrorReactions;
            else
                currentReactionList = AcceptReactions;

            Random rnd = new Random();

            //ProcessError returns a ReactionCollection, which is where GenerateReaction should be called from.
            //So we don't need this line anymore.
            //ReactionCollection currentReactionCollection = ProcessError(error_react);

            //Get our randomly selected reactoin
            Reaction currentReaction = currentReactionList.ElementAt(rnd.Next(0, currentReactionList.Count - 1));

            if (currentReaction.Description.ToLower() == "emote")
            { //emote
                IEmote emote_to_add = Context.Guild.Emotes.First(e => e.Name == currentReaction.Name);
                await Context.Message.AddReactionAsync(emote_to_add);
            }
            else // emoji
            {
                Emoji emoji_to_add = new Emoji(currentReaction.Name);
                await Context.Message.AddReactionAsync(emoji_to_add);
            }
        }
    }
}