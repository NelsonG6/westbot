using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Microsoft.Extensions.Http;
using Discord.Commands;

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

            CurrentConfiguration.Load(call_args);

            await client.LoginAsync(TokenType.Bot, CurrentConfiguration.Token);
            await client.StartAsync();
            
            await services.GetRequiredService<Services.CommandHandler>().InstallCommandsAsync();

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
                .AddSingleton<Tournament>()
                .AddSingleton<HttpClient>()
                .BuildServiceProvider();
        }
    }
}
