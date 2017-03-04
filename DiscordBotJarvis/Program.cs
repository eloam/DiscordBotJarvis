using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DiscordBotJarvis.Core;
using DiscordBotJarvis.Data;
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

            AppConfig appConfig = XmlSerializerHelper.Decode<AppConfig>("AppConfig.xml");
            ResourcePacksList = ResourcePacks.LoadAll(appConfig.ResourcePacksCurrentCulture);

            ProcessingEvents(client);

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

        private static void ProcessingEvents(DiscordClient client)
        {
            // Evénement lancé lors de la création d'un message dans le client Discord
            client.MessageCreated += (sender, e) =>
            {
                DateTime t1 = DateTime.Now;
                if (e.Message.Author.ID == client.Me.ID) return;
                TextRecognition.ExecuteQuery(e, ResourcePacksList);
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
