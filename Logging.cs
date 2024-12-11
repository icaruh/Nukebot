using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
public class LoggingService
{
    public LoggingService(DiscordSocketClient client, CommandService command)
    {
        client.Log += LogAsync;
        command.Log += LogAsync;
    }

    private Task LogAsync(LogMessage message)
    {
        if (message.Exception is CommandException cmdException)
        {
            Console.WriteLine($"Comando/{message.Severity}] {cmdException.Command.Aliases.First()}"
                + $"Falha ao executar no {cmdException.Context.Channel}.");
            Console.WriteLine(cmdException);
        }
        else
        {
            Console.WriteLine($"[General/{message.Severity}] {message}");
        }
        return Task.CompletedTask;
    }
}
