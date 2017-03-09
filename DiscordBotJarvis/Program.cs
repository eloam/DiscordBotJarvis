using DSharpPlus;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using DiscordBotJarvis.Core;
using DiscordBotJarvis.Extensions;
using DiscordBotJarvis.Models.Client;
using DiscordBotJarvis.Models.Commands;
using DiscordBotJarvis.Models.Queries;
using LogLevel = DiscordBotJarvis.Enums.LogLevel;

namespace DiscordBotJarvis
{
    internal class Program
    {
        // Methods
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

            {
                Logger logger = new Logger("Lastest.log", logLevelDisplayConsole: LogLevel.Verbose);
                logger.Verbose($"Démarrage en cours de {Console.Title}");
                logger.Verbose($"Version de l'application {Assembly.GetEntryAssembly().GetName().Version}");
                logger.LogLevelDisplayConsole = LogLevel.Info;
                logger.Info($"Consultez le fichier de log {logger.LogFilePath} pour plus de détails en cas d'erreurs");
            }

            DiscordClient discordClient = new DiscordClient(new DiscordConfig()
            {
                Token = File.ReadAllText("Token.txt"),
                AutoReconnect = true
            });

            Task.Run(async delegate
            {
                await discordClient.Connect();
            });
            
            discordClient.UseCommands(new CommandConfig()
            {
                Prefix = "!",
                SelfBot = false
            });

            BotClient botClient = new BotClient();

            botClient.Logger.Info($"La langue actuelle des pack de ressources est : {botClient.Config.CultureInfo.EnglishName}");
            botClient.Logger.Info("Vous pouvez le changer en éditant le fichier AppConfig.xml.");
            botClient.Logger.Verbose("Chargement de tous les packs de ressources...");


            botClient.Logger.Verbose("Chargement de tous les packs de ressources... [Terminé]");

            ProcessingEvents(discordClient, botClient);

            botClient.Logger.Info("En attente de requête...");

            Console.WriteLine("Pressez la touche Q pour quitter...");

            do
            {
                char response = Console.ReadKey(true).KeyChar;
                if (response.ToString().ToUpper() == "Q")
                    Environment.Exit(0);
            } while (true);
        }

        private static void ProcessingEvents(DiscordClient discordClient, BotClient botClient)
        {
            // Evénement lancé lors de la création d'un message dans le client Discord
            discordClient.MessageCreated += async (sender, e) =>
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                if (e.Message.Author.ID == discordClient.Me.ID) return;

                // Creation de l'objet Query
                AuthorQuery author = new AuthorQuery(e.Message.Author.ID.ToString(), e.Message.Author.Username);
                UserQuery userQuery = new UserQuery(e.Message.Content, author);

                Console.WriteLine($"T01 {sw.Elapsed} : Création de l'objet UserQuery & AuthorQuery");

                string[] responses = botClient.Query(userQuery);

                Console.WriteLine($"T02 {sw.Elapsed} : Execution de la requête");

                if (responses == null) return;

                foreach (string response in responses)
                    await e.Message.Respond(response);

                sw.Stop();
                Console.WriteLine($"T03 Requête traitée en {sw.Elapsed} secondes.");
            };

            discordClient.PresenceUpdate += async (sender, e) =>
            {
                if (e.User != null && e.Game != string.Empty)
                    await discordClient.SendMessage(e.GuildID, $"{e.User.Mention} joue à {e.Game}.");
            };
        }
    }
}
