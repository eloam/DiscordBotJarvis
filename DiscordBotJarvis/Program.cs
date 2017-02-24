using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using DiscordBotJarvis.Core;
using DiscordBotJarvis.Extensions;
using DiscordBotJarvis.Helpers;
using DiscordBotJarvis.Models.Commands;
using DiscordBotJarvis.Models.ResourcePacks;
using DiscordBotJarvis.Models.ResourcePacks.CommandDefinitions;
using DiscordBotJarvis.Models.ResourcePacks.ConfigFile;
using DiscordBotJarvis.Models.Settings;
using WebSocketSharp;

namespace DiscordBotJarvis
{
    internal class Program
    {
        private static IEnumerable<ResourcePack> ResourcePacksList { get; set; } = new List<ResourcePack>();

        private static void Main(string[] args)
        {
            //AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);         

            Console.Title = "DiscordBotJarvis";

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

            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

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

            AppConfig appConfig = XmlSerializationHelper.Decode<AppConfig>("AppConfig.xml");
            ResourcePacksList = ResourcePacksCore.LoadAll(appConfig.ResourcePacksCurrentCulture);

            CreateCommands(client);
            Jarvis(client);

            Console.WriteLine("Jarvis is connected to Discord.");
            Console.WriteLine("[INFO] Jarvis is currently in DEBUG mode.");

            CultureInfo culture = new CultureInfo("fr-FR");
            Console.WriteLine($"[INFO] The current language of the resource packs is {culture.EnglishName}.");
            Console.WriteLine("[INFO] You can change it by editing the AppConfig.xml file.");
            

            Console.WriteLine("Press the 'Q' key to exit...");

            do
            {
                char response = Console.ReadKey(true).KeyChar;
                if (response.ToString().ToUpper() == "Q")
                    Environment.Exit(0);
            } while (true);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine((e.ExceptionObject as Exception).Message, "Unhandled UI Exception");
            // here you can log the exception ...
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
            // Evénement lancé lors de la création d'un message dans le client Discord
            client.MessageCreated += (sender, e) =>
            {
                DateTime t1 = DateTime.Now;
                if (e.Message.Author.ID == client.Me.ID) return;
                TextRecognitionCore.ExecuteQuery(e, ResourcePacksList);
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
