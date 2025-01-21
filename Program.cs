using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace DiscordBot
{
    public class Program
    {
        private static CommandHandler _commandHandler;
        private static DiscordSocketClient _client;
        private static CommandService _commands;

        public static void Main(string[] args)
        {
            Task.Run(() => MainAsync()).GetAwaiter().GetResult();
        }

        public static async Task MainAsync()
        {
            var config = new DiscordSocketConfig
            {
                AlwaysDownloadUsers = true,
                MessageCacheSize = 100,
                GatewayIntents = GatewayIntents.All | GatewayIntents.MessageContent | GatewayIntents.GuildMessages
            };

            _client = new DiscordSocketClient(config);

            var configurations = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("AppSettings.json")
                .Build();

            var token = configurations["DiscordBot:Token"];

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();



            // Ligação do CommandHandler
            _commands = new CommandService();
            _commandHandler = new CommandHandler(_client, _commands);
            await _commandHandler.RunAsync();

            // Usando a sintaxe correta com async
            _client.Ready += async () =>
            {
                Console.WriteLine("@everyone to pronto pra besteira");

                ulong channelId = 1183593740791717950; // id do canal da msg 
                var channel = _client.GetChannel(channelId) as ITextChannel;

                if (channel != null)
                {
                    await channel.SendMessageAsync("Daniela?");
                }
            };

            await Task.Delay(-1);
        }

        private static Task Log(LogMessage logMessage)
        {
            Console.WriteLine(logMessage.ToString());
            return Task.CompletedTask;
        }
    }
}
