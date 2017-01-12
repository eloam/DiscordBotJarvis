using DiscordBotCaptainObvious.Cortana.Controllers;
using DiscordBotCaptainObvious.Cortana.Enums;
using DiscordBotCaptainObvious.Cortana.Helpers;
using DiscordBotCaptainObvious.Cortana.Models;
using DSharpPlus;
using DSharpPlus.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordBotCaptainObvious
{
    class Program
    {
        private static IEnumerable<Sentence> ListSentences { get; set; } = new List<Sentence>();
        private static List<String> ListObviousSentences { get; set; } = new List<string>();
        private static List<String> ListCsGoSentences { get; set; } = new List<string>();
        private static List<String> ListPunchlineSentences { get; set; } = new List<string>();


        private static readonly object syncLock = new object();
        private static ClientStatutEnum clientStatut;

        static void Main(string[] args)
        {
            Console.WriteLine("Jarvis : a Discord bot");
            Console.WriteLine("======================");
            Console.WriteLine("Démarrage du bot...");

            DiscordClient _client = new DiscordClient(new DiscordConfig()
            {
                Token = File.ReadAllText("../../token.txt"),
                AutoReconnect = true
            });

            _client.Connect();
            clientStatut = ClientStatutEnum.Connected;

            _client.UseCommands(new CommandConfig()
            {
                Prefix = "!",
                SelfBot = false
            });

            BuildListSentence();

            CreateCommands(_client);
            Cortana(_client);

            ListObviousSentences = ReadObviousSentences();
            ListCsGoSentences = ReadCsGoSentences();
            ListPunchlineSentences = ReadPunchlineSentences();

            Thread.Sleep(1000);
            Console.WriteLine("Statut du bot : Connecté");
            DisplayMainMenu(_client);
        }

        private static void BuildListSentence()
        {
            // Créationde la liste temporaire des "Sentences"
            List<Sentence> sentences = new List<Sentence>();

            // Say "Hello"
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Hello }, new string[] { "bonjour" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }, false, ComparisonModeEnum.StartsWith));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Hello }, new string[] { "bjr" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }, false, ComparisonModeEnum.StartsWith));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Hello }, new string[] { "salut" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }, false, ComparisonModeEnum.StartsWith));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Hello }, new string[] { "hi" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }, false, ComparisonModeEnum.StartsWith));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Hello }, new string[] { "hello" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }, false, ComparisonModeEnum.StartsWith));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Hello }, new string[] { "yo" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }, false, ComparisonModeEnum.StartsWith));

            // Say "Goodbye"
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Goodbye }, new string[] { "bye" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }, false, ComparisonModeEnum.StartsWith));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Goodbye }, new string[] { "a+" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }, false, ComparisonModeEnum.StartsWith));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Goodbye }, new string[] { "++" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }, false, ComparisonModeEnum.StartsWith));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Goodbye }, new string[] { "@+" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }, false, ComparisonModeEnum.StartsWith));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Goodbye }, new string[] { "au revoir" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }, false));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Goodbye }, new string[] { "j'y vais" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }, false));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Goodbye }, new string[] { "j'y go" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }, false));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Goodbye }, new string[] { "je go" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }, false));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Goodbye }, new string[] { "bonne nuit" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }, false));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Goodbye }, new string[] { "tchuss" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }, false));

            // Say "De rien"
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.DeRien }, new string[] { "merci" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.DeRien }, new string[] { "remerci" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.DeRien }, new string[] { "nice" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.DeRien }, new string[] { "thank you" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.DeRien }, new string[] { "thanks" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.DeRien }, new string[] { "thx" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.DeRien }, new string[] { "ty" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }));

            // Say punchline sentence
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Punchline }, new string[] { "punchline" }));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Punchline }, new string[] { "connard" }));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Punchline }, new string[] { "putain" }));

            // Say obvious sentence
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Obvious }, new string[] { "obvious" }));

            // Say russian sentence
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Russian }, new string[] { "russie" }));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Russian }, new string[] { "russes" }));
        
            // Say "Tout à fait Thierry"
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.ToutAFait }, new string[] { "n'est ce pas captain ?" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }, true, ComparisonModeEnum.EndsWith));

            // Say "Stats compte de jeu Overwatch"
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.StatsJeuOverwatch }, new string[] { "stats", "overwatch" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.StatsJeuOverwatch }, new string[] { "stats", "ow" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }));

            // Say "Play CS GO song"
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.YesOrder, SentencesEnum.PlayMusiqueCsGo }, new string[] { "musique", "cs" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.YesOrder, SentencesEnum.PlayMusiqueCsGo }, new string[] { "musique", "csgo" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.YesOrder, SentencesEnum.PlayMusiqueCsGo }, new string[] { "musique", "cs", "go" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }));

            // Valorisation de la liste
            ListSentences = sentences;
        }

        private static void DisplayMainMenu(DiscordClient _client)
        {
            Console.WriteLine("Actions possibles : (C) Connexion du bot - (D) Déconnexion du bot - (R) Redémarrer le bot - (Q) Quitter");
            while (true)
            {
                char response = char.ToUpper(Console.ReadKey(true).KeyChar);
                switch (response)
                {
                    case 'C':
                        if (clientStatut == ClientStatutEnum.Disconnected)
                        {
                            _client.Connect();
                            clientStatut = ClientStatutEnum.Connected;
                            Thread.Sleep(1000);
                            Console.WriteLine("Statut du bot : Connecté");
                        }
                        else
                        {
                            Console.WriteLine("Echec de la connection : Le client est déjà connecté !");
                        }
                        break;
                    case 'D':
                        if (clientStatut == ClientStatutEnum.Connected)
                        {
                            _client.Disconnect();
                            clientStatut = ClientStatutEnum.Disconnected;
                            Thread.Sleep(1000);
                            Console.WriteLine("Statut du bot : Déconnecté");
                        }
                        else
                        {
                            Console.WriteLine("Echec de la déconnection : Le client est déjà déconnecté !");
                        }
                        break;
                    case 'R':
                        Console.WriteLine("Redémarrage en cours...");
                        _client.Reconnect();
                        clientStatut = ClientStatutEnum.Connected;
                        Thread.Sleep(1000);
                        Console.WriteLine("Le client a été redémarré avec succès !");
                        break;
                    default:
                        Console.WriteLine("Arrêt du bot en cours...");
                        if (clientStatut == ClientStatutEnum.Connected)
                        {
                            _client.Disconnect();
                            Thread.Sleep(1000);
                        }
                        Environment.Exit(0);
                        break;
                }
            }
        }

        private static void CreateCommands(DiscordClient _client)
        {
            _client.AddCommand("help", async (e) =>
            {
                DiscordDMChannel dm = await _client.CreateDM(e.Message.Author.ID);
                DiscordMessage x = await _client.SendMessage(dm, File.ReadAllText("../../files/help.txt"));
            });

            _client.AddCommand("hello", async (e) =>
            {
                await e.Message.Parent.SendMessage($"Salut, {e.Message.Author.Mention}");
            });
            _client.AddCommand("goodbye", async (e) =>
            {
                await e.Message.Parent.SendMessage($"Au revoir, {e.Message.Author.Mention} !");
            });
            _client.AddCommand("obvious", async (e) =>
            {
                string randomSentence = GetRandomSentence(ListObviousSentences);
                await e.Message.Parent.SendMessage(randomSentence);
            });
            _client.AddCommand("cs", async (e) =>
            {
                string randomSentence = GetRandomSentence(ListCsGoSentences);
                await e.Message.Parent.SendMessage(randomSentence);
            });
            _client.AddCommand("therussian", async (e) =>
            {
                string randomSentence = GetRandomSentence(ListCsGoSentences);
                await e.Message.Parent.SendMessage(randomSentence);
            });
            _client.AddCommand("punchline", async (e) =>
            {
                string randomSentence = GetRandomSentence(ListPunchlineSentences);
                await e.Message.Parent.SendMessage(randomSentence);
            });
            /*_client.AddCommand("ytb", async (e) =>
            {
                await e.Message.Parent.SendMessage("https://www.youtube.com/watch?v=sUzfUYSGuMQ");
            });*/


            _client.AddCommand("statut", async (e) =>
            {
                DiscordPresence p = _client.GetUserPresence(e.Message.Author.ID);

                string info = "Ton statut sur le serveur mon cher camarade :";
                info += "\nID : " + p.UserID;
                info += "\nJeu en cours : ";

                if (p.Game != string.Empty)
                    info += $"Tu est en train de jouer à {p.Game}, un bon gros jeu de merde.";
                else
                    info += "Tu joue à aucun jeu, si ce n'est pas triste !";

                info += "\nStatus : " + p.Status;

                await e.Message.Respond(info);
            });


        }

        private static void Cortana(DiscordClient _client)
        {
            _client.MessageCreated += (sender, e) =>
            {
                if (e.Message.Author.ID != _client.Me.ID)
                {
                    CortanaCore.ExecuteQuery(e, ListSentences);
                }
            };

            _client.PresenceUpdate += (sender, e) =>
            {
                if (e.User != null && e.Game != string.Empty)
                    _client.SendMessage(e.GuildID, $"{e.User.Mention} joue à {e.Game}.");
            };
        }

        private static List<string> ReadObviousSentences()
        {
            ListObviousSentences = File.ReadAllText("../../files/obvious.txt").Split('\n').ToList<String>();
            return ListObviousSentences;
        }
        private static List<string> ReadCsGoSentences()
        {
            ListCsGoSentences = File.ReadAllText("../../files/csgo.txt").Split('\n').ToList<String>();
            return ListCsGoSentences;
        }

        private static List<string> ReadPunchlineSentences()
        {
            ListPunchlineSentences = File.ReadAllText("../../files/punchline.txt").Split('\n').ToList<String>();
            return ListPunchlineSentences;
        }

        private static string GetRandomSentence(List<String> list)
        {
            Random random = new Random();
            int idSentence;
            lock (syncLock)
                idSentence = random.Next(list.Count());
            return list[idSentence];
        }

        /*private static void CortanaWip(MessageCreateEventArgs e)
        {
            string request = FormatStringHelper.NormalizeQuery(e.Message.Content);
            string response = string.Empty;

            string[] callBotContains = new string[] { "bot", "captain", "captainobvious" };


            Sentence item = new Sentence(SentencesEnum.Hello, new string[] { "bonjour" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention });

            if (item.Keywords.All(w => w.Contains(w)))
            {
                if (item.CallBotRequired)
                {
                    if ()
                }
            }
        }*/
    }
}
