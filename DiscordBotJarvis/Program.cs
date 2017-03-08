using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using DiscordBotJarvis.Core;
using DiscordBotJarvis.Extensions;
using DiscordBotJarvis.Helpers;
using DiscordBotJarvis.Models.Commands;
using DiscordBotJarvis.Models.Queries;
using DiscordBotJarvis.Models.ResourcePacks;
using DiscordBotJarvis.Models.Settings;
using LogLevel = DiscordBotJarvis.Enums.LogLevel;

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

            Logger logger = new Logger("Lastest.log", logLevelDisplayConsole: LogLevel.Verbose);

            logger.Verbose($"Démarrage en cours de {Console.Title}");
            logger.Verbose($"Version de l'application {Assembly.GetEntryAssembly().GetName().Version}");

            logger.LogLevelDisplayConsole = LogLevel.Info;
            logger.Info($"Consultez le fichier de log {logger.LogFilePath} pour plus de détails en cas d'erreurs");

            DiscordClient client = new DiscordClient(new DiscordConfig()
            {
                Token = File.ReadAllText("Token.txt"),
                AutoReconnect = true
            });

            Task.Run(async delegate
            {
                logger.Verbose("Etablissement de la connexion du client à Discord...");
                await client.Connect();
                logger.Verbose("Etablissement de la connexion du client à Discord... [Terminé]");
            });
            
            client.UseCommands(new CommandConfig()
            {
                Prefix = "!",
                SelfBot = false
            });

            logger.Verbose("Lecture du fichier de configuration de l'application...");

            AppConfig appConfig = XmlSerializerHelper.Decode<AppConfig>("AppConfig.xml") ?? new AppConfig();

            logger.Verbose("Lecture du fichier de configuration de l'application... [Terminé]");
            CultureInfo culture = new CultureInfo("fr-FR");
            logger.Info($"La langue actuelle des pack de ressources est : {culture.EnglishName}");
            logger.Info("Vous pouvez le changer en éditant le fichier AppConfig.xml.");
            logger.Verbose("Chargement de tous les packs de ressources...");

            ResourcePacksList = ResourcePacks.LoadAll(appConfig.ResourcePacksCurrentCulture);

            logger.Verbose("Chargement de tous les packs de ressources... [Terminé]");

            ProcessingEvents(client);

            logger.Info("En attente de requête...");

            Console.WriteLine("Pressez la touche Q pour quitter...");

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
            client.MessageCreated += async (sender, e) =>
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                DateTime t1 = DateTime.Now;

                if (e.Message.Author.ID == client.Me.ID) return;

                // Creation de l'objet Query
                AuthorQuery author = new AuthorQuery(e.Message.Author.ID.ToString(), e.Message.Author.Username);
                UserQuery query = new UserQuery(e.Message.Content, author);

                Console.WriteLine($"T01 {sw.Elapsed} ms.");

                string[] responses = TextRecognition.ExecuteQuery(query, ResourcePacksList);

                Console.WriteLine($"T02 {sw.Elapsed} ms.");

                if (responses == null) return;

                Console.WriteLine($"T03 {sw.Elapsed} ms.");

                foreach (string response in responses)
                    await e.Message.Respond(response);

                sw.Stop();
                Console.WriteLine($"Processing performed in {sw.Elapsed} ms.");
            };

            client.PresenceUpdate += async (sender, e) =>
            {
                if (e.User != null && e.Game != string.Empty)
                    await client.SendMessage(e.GuildID, $"{e.User.Mention} joue à {e.Game}.");
            };
        }
    }
}
