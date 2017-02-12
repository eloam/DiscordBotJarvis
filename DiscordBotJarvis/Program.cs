using DSharpPlus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DiscordBotJarvis.Controllers.TextRecognitionModule;
using DiscordBotJarvis.Dal;
using DiscordBotJarvis.Models.CommandDefinitions;
using DiscordBotJarvis.Controllers.CommandsModule;

namespace DiscordBotJarvis
{
    internal class Program
    {
        private static IEnumerable<CommandSet> ListSentences { get; set; } = new List<CommandSet>();

        private static void Main(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("                                          ██╗ █████╗ ██████╗ ██╗   ██╗██╗███████╗");
            Console.WriteLine("                                          ██║██╔══██╗██╔══██╗██║   ██║██║██╔════╝");
            Console.WriteLine("                                          ██║███████║██████╔╝██║   ██║██║███████╗");
            Console.WriteLine("                                     ██   ██║██╔══██║██╔══██╗╚██╗ ██╔╝██║╚════██║");
            Console.WriteLine("                                     ╚█████╔╝██║  ██║██║  ██║ ╚████╔╝ ██║███████║");
            Console.WriteLine("                                      ╚════╝ ╚═╝  ╚═╝╚═╝  ╚═╝  ╚═══╝  ╚═╝╚══════╝");
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine($"Version {Assembly.GetEntryAssembly().GetName().Version}");

            DiscordClient client = new DiscordClient(new DiscordConfig()
            {
                Token = File.ReadAllText("token.txt"),
                AutoReconnect = true
            });

            Task.Run(async delegate
            {
                await client.Connect();
            });
            
            client.UseCommands(new CommandConfig()
            {
                Prefix = "!",
                SelfBot = false
            });

            ListSentences = SentencesDal.BuildListSentences();

            CreateCommands(client);
            Jarvis(client);

            Thread.Sleep(1000);
            Console.WriteLine("Jarvis is connected to Discord.");
            Console.WriteLine("Press the 'Q' key to exit...");

            do
            {
                char response = Console.ReadKey(true).KeyChar;
                if (response.ToString().ToUpper() == "Q")
                    Environment.Exit(0);
            } while (true);
        }

        private static void CreateCommands(DiscordClient client)
        {
            client.AddCommand("help", async (e) =>
            {
                DiscordDMChannel dm = await client.CreateDM(e.Message.Author.ID);
                await client.SendMessage(dm, File.ReadAllText("../../files/help.txt"));
            });

            client.AddCommand("statut", async (e) =>
            {
                DiscordPresence p = client.GetUserPresence(e.Message.Author.ID);

                string info = "Ton statut sur le serveur mon cher camarade :";
                info += "\nID : " + p.UserID;
                info += "\nJeu en cours : ";

                if (p.Game != string.Empty)
                    info += $"Tu est en train de jouer à {p.Game}.";
                else
                    info += "Tu joue à aucun jeu, si ce n'est pas triste !";

                info += "\nStatus : " + p.Status;

                await e.Message.Respond(info);
            });
        }

        private static void Jarvis(DiscordClient client)
        {
            client.MessageCreated += (sender, e) =>
            {
                DateTime t1 = DateTime.Now;
                if (e.Message.Author.ID == client.Me.ID) return;
                JarvisCoreController.ExecuteQuery(e, ListSentences);
                Console.WriteLine($"Processing performed in {(DateTime.Now - t1).TotalMilliseconds} ms.");
            };

            client.PresenceUpdate += async (sender, e) =>
            {
                if (e.User != null && e.Game != string.Empty)
                    await client.SendMessage(e.GuildID, $"{e.User.Mention} joue à {e.Game}.");
            };
        }
    }
}
