using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using DiscordBotJarvis.Enums;
using DiscordBotJarvis.Helpers;
using DiscordBotJarvis.Models.Queries;
using DiscordBotJarvis.Models.ResourcePacks;
using DiscordBotJarvis.Models.ResourcePacks.CommandDefinitions;
using DiscordBotJarvis.Services;
using DiscordBotJarvis.Models.Client;

namespace DiscordBotJarvis.Core
{
    public static class TextRecognition
    {
        private delegate bool ComparisonModeDelegate(string keyword);

        public static string[] ExecuteQuery(BotConfig config, UserQuery userQuery, IEnumerable<ResourcePack> commands)
        {
            // Vérification si un ou plusieurs paramètres sont 'null'
            if (userQuery == null) throw new ArgumentNullException(nameof(userQuery));
            if (commands == null) throw new ArgumentNullException(nameof(commands));

            // On parcours tous les packs de ressources afin de lire le dictionnaire CommandsList associé
            foreach (ResourcePack currentResourcePack in commands)
            {
                // On parcours tous les fichiers xml situés dans le répertoire "CommandDefinitions" du pack de ressources courant
                foreach (KeyValuePair<string, IEnumerable<CommandSet>> currentCommandDefinitionsKeyValuePair in currentResourcePack.Commands)
                {
                    // On parcours toutes les commandes que contient le fichier xml courant, afin de recherche une correspondance eventuelle avec la requête utilisateur
                    foreach (CommandSet command in currentCommandDefinitionsKeyValuePair.Value)
                    {
                        // On regarde si les arguments (mots-clés et expressions regulières) correspondent à la requête
                        bool resultMatch = ArgumentsMatch(userQuery.QueryProcessed, command.TriggersRegex);

                        // On regarde si le bot a besoin d'être appelé et que si c'est le cas, que son nom figure dans la requête de l'utilisateur
                        bool triggerBot = CheckTriggerBot(userQuery.QueryProcessed, config.BotNamesRegex, command.BotMentionRequired);

                        // Si le résultat correspond aux mots-clés et expressions régulières définit dans l'objet et si le bot doit être appelé, alors...
                        if (resultMatch && triggerBot)
                            return GetFeedbacks(userQuery, currentResourcePack, command.Feedbacks);
                    }
                }
            }

            // Si aucune commande correspondant à la requête initial à été trouvée, on retourne null
            return null;
        }

        private static bool ArgumentsMatch(string queryProcessed, Regex[] triggers)
        {
            // Vérification si les paramètres en entrées de fonction ne sont pas à l'état : null/empty
            if (string.IsNullOrWhiteSpace(queryProcessed)) throw new ArgumentException(nameof(queryProcessed));
            if (triggers == null) throw new ArgumentNullException(nameof(triggers));

            foreach (Regex regex in triggers)
                if (!regex.Match(queryProcessed).Success) return false;

            return true;
        }

        private static string[] GetFeedbacks(UserQuery userQuery, ResourcePack resourcePack, Feedback[] feedbacks)
        {
            // Vérification si les paramètres en entrées de fonction ne sont pas à 'null'
            if (userQuery == null) throw new ArgumentNullException(nameof(userQuery));
            if (resourcePack == null) throw new ArgumentNullException(nameof(resourcePack));
            if (feedbacks == null) throw new ArgumentNullException(nameof(feedbacks));

            // Initialisation de la liste de retour
            List<string> reponses = new List<string>();

            // On parcours toutes les objets SentenceConfig afin d'afficher leurs contenus
            foreach (Feedback feedback in feedbacks)
            {
                // Message(s) fournit par le bot à l'utilisateur
                string response = null;

                if (feedback is Sentence)
                {
                    Sentence sentence = (Sentence)feedback;

                    if (sentence.Parameters != null)
                    {
                        object[] parameters = ParametersToValuesConverter(userQuery, sentence.Parameters);
                        response = string.Format(sentence.Phrase, parameters);
                    }
                    else
                    {
                        response = sentence.Phrase;
                    }
                }
                else if (feedback is SentenceFile)
                {
                    SentenceFile sentence = (SentenceFile)feedback;
                    string fileSentencesPath = string.Format(EndPoints.Path.ResourcePacksResources, resourcePack.DirectoryName, sentence.FileName);

                    if (File.Exists(fileSentencesPath))
                    {
                        switch (sentence.FileReadMode)
                        {
                            case ReadFileMode.OneSentenceRandom:
                                response = GetSentenceHelper.ReadLineRandom(fileSentencesPath);
                                break;
                            case ReadFileMode.OneSentenceSpecified:
                                response = GetSentenceHelper.ReadLineSpecified(fileSentencesPath, sentence.ReadLineOfFile);
                                break;
                            case ReadFileMode.File:
                                response = GetSentenceHelper.ReadFile(fileSentencesPath);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException(nameof(sentence.FileReadMode), sentence.FileReadMode, "Argument not specified.");
                        }

                        if (sentence.Parameters != null)
                        {
                            object[] parameters = ParametersToValuesConverter(userQuery, sentence.Parameters);
                            if (response != null) response = string.Format(response, parameters);
                        }
                    }
                }
                else if (feedback is Service)
                {
                    Service service = (Service)feedback;
                    string libraryPath = string.Format(EndPoints.Path.ResourcePacksServices, resourcePack.DirectoryName, service.LibraryName);

                    if (File.Exists(libraryPath))
                    {
                        AssemblyName assemblyName = AssemblyName.GetAssemblyName(libraryPath);
                        Assembly assembly = Assembly.Load(assemblyName);
                        Type pluginType = typeof(IService);

                        if (assembly != null)
                        {
                            Type[] types = assembly.GetTypes();
                            Type serviceType = types.Where(type => !type.IsInterface && !type.IsAbstract && !type.IsEnum)
                                .Where(type => type.GetInterface(pluginType.FullName) != null)
                                .FirstOrDefault(type => type.Name == service.ClassName);

                            if (serviceType != null)
                            {
                                IService plugin = (IService)Activator.CreateInstance(serviceType);
                                response = plugin.Do("request", "requestProcessed", "fr-FR");
                            }
                        }
                    }
                }

                // Si le message fournit par le bot à l'utilisateur est différent de null, vide ou composé uniquement d'espaces blancs.
                if (!string.IsNullOrWhiteSpace(response))
                    reponses.Add(response);
            }

            return reponses.ToArray();
        }

        private static bool CheckTriggerBot(string queryProcessed, Regex botNamesRegex, bool callBotRequired)
            => callBotRequired ? botNamesRegex.Match(queryProcessed).Success : true;

        private static object[] ParametersToValuesConverter(UserQuery userQuery, SentenceParameters[] parameters)
        {
            List<object> valuesParameters = new List<object>();
            foreach (SentenceParameters item in parameters)
            {
                switch (item)
                {
                    case SentenceParameters.MessageAuthorMention:
                        valuesParameters.Add(userQuery.Author.UserName);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(parameters), parameters, "Argument not specified.");
                }
            }

            return valuesParameters.ToArray();
        }
    }
}
