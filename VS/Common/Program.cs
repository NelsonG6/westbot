﻿using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Discord.Commands;
using WestBot;

namespace Westbot
{
    class Program
    {
        static void Main(string[] call_args)
            => new Program().MainAsync(call_args).GetAwaiter().GetResult();

        public async Task MainAsync(string[] call_args)
        {
            var services = ConfigureServices();

            var client = services.GetRequiredService<DiscordSocketClient>();

            client.Log += LogAsync;
            services.GetRequiredService<CommandService>().Log += LogAsync;

            BotConfiguration.Load(call_args);

            await client.LoginAsync(TokenType.Bot, BotConfiguration.Token);
            await client.StartAsync();
            
            await services.GetRequiredService<Services.CommandHandler>().InstallCommandsAsync();

            client.Ready += () =>
            {
                ulong BotVersionChannel = 0;
                
                try
                {
                    BotVersionChannel = DatabaseHandler.GetChannelID("BotVersion");
                }
                catch (Exception ex)
                {
                    Console.Write("Exception: " + ex.Message);
                }

                var Guild = client.GetGuild(BotConfiguration.ServerID);
                var channel = Guild.GetTextChannel(BotVersionChannel);

                string version = "";
                try
                {
                    version = DatabaseHandler.GetVersion();
                }
                catch (Exception ex)
                {
                    Console.Write("Exception: " + ex.Message);
                }

                if(version != "0")
                    channel.SendMessageAsync("Bot version " + version + " is online.");

                Console.WriteLine("**Bot online.**");
                return Task.CompletedTask;
            };

            await Task.Delay(-1);
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());

            return Task.CompletedTask;
        }

        private IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<Services.CommandHandler>()
                //.AddSingleton<Tournament>()
                .AddSingleton<HttpClient>()
                .BuildServiceProvider();
        }
    }
}