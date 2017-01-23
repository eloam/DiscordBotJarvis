using DiscordBotJarvis.TextRecognitionModule.Controllers;
using DiscordBotJarvis.TextRecognitionModule.Dal;
using DiscordBotJarvis.TextRecognitionModule.Enums;
using DiscordBotJarvis.TextRecognitionModule.Models;
using DSharpPlus;
using DSharpPlus.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace DiscordBotJarvis
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
                Token = File.ReadAllText("token.txt"),
                AutoReconnect = true
            });

            _client.Connect();
            clientStatut = ClientStatutEnum.Connected;

            _client.UseCommands(new CommandConfig()
            {
                Prefix = "!",
                SelfBot = false
            });

            ListSentences = SentencesDal.BuildListSentences();

            CreateCommands(_client);
            Cortana(_client);

            Thread.Sleep(1000);
            Console.WriteLine("Statut du bot : Connecté");
            DisplayMainMenu(_client);
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
                    case 'Q':
                        Console.WriteLine("Arrêt du bot en cours...");
                        if (clientStatut == ClientStatutEnum.Connected)
                        {
                            _client.Disconnect();
                            Thread.Sleep(1000);
                        }
                        Environment.Exit(0);
                        break;
                    default:
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

        private static void Cortana(DiscordClient _client)
        {
            _client.MessageCreated += (sender, e) =>
            {
                DateTime t1 = DateTime.Now;
                if (e.Message.Author.ID != _client.Me.ID)
                {
                    CortanaCore.ExecuteQuery(e, ListSentences);
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
