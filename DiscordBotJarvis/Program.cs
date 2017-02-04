using DiscordBotJarvis.TextRecognitionModule.Controllers;
using DiscordBotJarvis.TextRecognitionModule.Dal;
using DiscordBotJarvis.TextRecognitionModule.Enums;
using DiscordBotJarvis.TextRecognitionModule.Models;
using DSharpPlus;
using DSharpPlus.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Net.Security;

namespace DiscordBotJarvis
{
    class Program
    {
        private static IEnumerable<Sentence> ListSentences { get; set; } = new List<Sentence>();

        static void Main(string[] args)
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

            DiscordClient _client = new DiscordClient(new DiscordConfig()
            {
                Token = File.ReadAllText("token.txt"),
                AutoReconnect = true
            });

            _client.Connect();

            _client.UseCommands(new CommandConfig()
            {
                Prefix = "!",
                SelfBot = false
            });

            ListSentences = SentencesDal.BuildListSentences();

            CreateCommands(_client);
            Jarvis(_client);

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

        private static void CreateCommands(DiscordClient _client)
        {
            _client.AddCommand("help", async (e) =>
            {
                DiscordDMChannel dm = await _client.CreateDM(e.Message.Author.ID);
                DiscordMessage x = await _client.SendMessage(dm, File.ReadAllText("../../files/help.txt"));
            });

            _client.AddCommand("statut", async (e) =>
            {
                DiscordPresence p = _client.GetUserPresence(e.Message.Author.ID);

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

        private static void Jarvis(DiscordClient _client)
        {
            _client.MessageCreated += (sender, e) =>
            {
                DateTime t1 = DateTime.Now;
                if (e.Message.Author.ID != _client.Me.ID)
                {
                    JarvisCoreController.ExecuteQuery(e, ListSentences);
                    Console.WriteLine($"Traitement effectué en {(DateTime.Now - t1).TotalMilliseconds}.");
                }
            };

            _client.PresenceUpdate += (sender, e) =>
            {
                if (e.User != null && e.Game != string.Empty)
                    _client.SendMessage(e.GuildID, $"{e.User.Mention} joue à {e.Game}.");
            };
        }
    }
}
