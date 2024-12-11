using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;
using System;

namespace DiscordBot
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private string _prefix;

        // Construtor do CommandHandler
        public CommandHandler(DiscordSocketClient client, CommandService commands)
        {
            _client = client;
            _commands = commands;
        }
        public static async Task Main(string[] args)
        { 
            var client = new DiscordSocketClient();
            var commands = new CommandService();

            var handler = new CommandHandler(client, commands);

            await handler.RunAsync();
            client.Log += handler.LogAsync;

        }
        public async Task RunAsync()
        {

            _client.Log += LogAsync;

            //conectar o evento MessageReceived ao manipulador de comandos
            _client.MessageReceived += HandleCommandAsync;

            var configurations = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("AppSettings.json")
                .Build();

            _prefix = configurations["DiscordBot:CommandPrefix"]; // Carregar o prefixo do AppSettings.json
            var token = configurations["DiscordBot:Token"]; // Carregar o token do bot

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), services: null);

            await Task.Delay(-1); // Espera indefinidamente
        }

        // Função de log para mensagens de erro ou eventos
        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            //verfica se a mensagem é do tipo SocketUserMessage e não foi enviada por um bot
            if (messageParam is not SocketUserMessage message || message.Author.IsBot) return;

            int argPos = 0;

            // Verifica se a mensagem começa com o prefixo configurado
            if (!message.HasCharPrefix(_prefix[0], ref argPos)) return;

            var context = new SocketCommandContext(_client, message);

            try
            {
                // Executa o comando
                var result = await _commands.ExecuteAsync(context: context, argPos: argPos, services: null);
                if (!result.IsSuccess)
                {
                    await message.Channel.SendMessageAsync($"sabe nem escrever o comando arrombado vai se fuder <@{message.Author.Id}>");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao executar comando: {ex.Message}");
                await message.Channel.SendMessageAsync("Ocorreu um erro ao executar o comando.");
            }
        }
    }
}
