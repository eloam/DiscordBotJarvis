using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using DiscordBotJarvis.Enums;
using DiscordBotJarvis.Extensions;
using DiscordBotJarvis.Helpers;
using DiscordBotJarvis.Models.Queries;
using DiscordBotJarvis.Models.ResourcePacks;
using DiscordBotJarvis.Models.ResourcePacks.CommandDefinitions;
using DiscordBotJarvis.Services;

namespace DiscordBotJarvis.Core
{
    public static class TextRecognition
    {
        private delegate bool ComparisonModeDelegate(string keyword);

        public static string[] ExecuteQuery(UserQuery userQuery, IEnumerable<ResourcePack> resourcePacksList)
        {
            // Vérification si un ou plusieurs paramètres sont 'null'
            if (userQuery == null) throw new ArgumentNullException(nameof(userQuery));
            if (resourcePacksList == null) throw new ArgumentNullException(nameof(resourcePacksList));

            // On parcours tous les packs de ressources afin de lire le dictionnaire CommandsList associé
            foreach (ResourcePack currentResourcePack in resourcePacksList)
            {
                // On parcours tous les fichiers xml situés dans le répertoire "CommandDefinitions" du pack de ressources courant
                foreach (KeyValuePair<string, IEnumerable<CommandSet>> currentCommandDefinitionsKeyValuePair in currentResourcePack.Commands)
                {
                    // On parcours toutes les commandes que contient le fichier xml courant, afin de recherche une correspondance eventuelle avec la requête utilisateur
                    foreach (CommandSet command in currentCommandDefinitionsKeyValuePair.Value)
                    {
                        // On regarde si les arguments (mots-clés et expressions regulières) correspondent à la requête
                        bool resultMatch = ArgumentsMatch(userQuery, command);

                        // On regarde si le bot a besoin d'être appelé et que si c'est le cas, que son nom figure dans la requête de l'utilisateur
                        bool triggerBot = CheckTriggerBot(userQuery, command.BotMentionRequired);

                        // Si le résultat correspond aux mots-clés et expressions régulières définit dans l'objet et si le bot doit être appelé, alors...
                        if (resultMatch && triggerBot)
                            return GetFeedbacks(userQuery, currentResourcePack, command.Feedbacks);
                    }
                }
            }

            // Si aucune commande correspondant à la requête initial à été trouvée, on retourne null
            return null;
        }

        private static bool ArgumentsMatch(UserQuery userQuery, CommandSet command)
        {
            // Vérification si les paramètres en entrées de fonction ne sont pas à 'null'
            if (userQuery == null) throw new ArgumentNullException(nameof(userQuery));
            if (command == null) throw new ArgumentNullException(nameof(command));

            // Resultat définitif
            bool resultMatch = false;
            
            // Indique si l'objet de type Feedback en cours de traitement correspond aux mot-clés et regex definies dans la requête provenant de l'utilisateur
            bool keywordsMatch = false;
            bool regexMatch = false;

            if (!command.IsListKeywordsEmpty)
                keywordsMatch = CheckKeywordsMatch(userQuery, command.KeywordsList, command.KeywordsComparisonMode);

            if (!command.IsListRegexEmpty)
                regexMatch = CheckRegexMatch(userQuery, command.RegexList);

            // Determination du résultat
            if (!command.IsListKeywordsEmpty && !command.IsListRegexEmpty)
                resultMatch = keywordsMatch && regexMatch;
            else if (!command.IsListKeywordsEmpty)
                resultMatch = keywordsMatch;
            else if (!command.IsListRegexEmpty)
                resultMatch = regexMatch;

            return resultMatch;
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

        private static bool CheckKeywordsMatch(UserQuery userQuery, IReadOnlyList<string[]> keywords, KeywordsComparison comparisonMode)
        {
            // Delegate permettant de définir le mode de comparaison de la requête (en début/fin de str ou n'importe ou dans la requete)
            ComparisonModeDelegate comparisonModeDel = keyword =>
            {
                bool result;
                switch (comparisonMode)
                {
                    case KeywordsComparison.StartsWith:
                        result = userQuery.QueryProcessed.StartsWith(keyword);
                        break;
                    case KeywordsComparison.Contains:
                        result = userQuery.QueryProcessed.Contains(keyword);
                        break;
                    case KeywordsComparison.EndsWith:
                        result = userQuery.QueryProcessed.EndsWith(keyword);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(comparisonMode), comparisonMode, "Argument not specified.");
                }
                return result;
            };

            // Déclaration d'un booléen indiquant si la requete correspond aux mots clés définit dans l'objet Sentence'
            bool keywordsMatch = true;

            // Recherche si la requête de l'utilisateur correspond aux mots-clés de l'objet Sentence
            int index = 0;
            do
            {
                // Ligne de mots-clés
                string[] rowKeywordsSentence = keywords[index];

                // Si au moins un des mot-clé est trouvé dans la liste, on continue la vérification pour tableaux de mots-clés suivants,
                // dans le cas contraite on sort de la boucle
                if (!rowKeywordsSentence.Any(keyword => comparisonModeDel(keyword.AddWhiteSpaceAroundString())))
                    keywordsMatch = false;

                index++;
            } while ((index < keywords.Count) && keywordsMatch);

            return keywordsMatch;
        }

        private static bool CheckRegexMatch(UserQuery userQuery, List<string[]> regexList)
        {
            // Déclaration d'un booléen indiquant si la requete correspond aux expressions regulières définit dans l'objet Sentence'
            bool regexMatch = true;

            // Recherche si la requête de l'utilisateur correspond aux expressions régulières de l'objet Sentence
            int index = 0;
            do
            {
                // Obtenir à partir de la liste de regex, la ligne correspond a son indice
                string[] rowRegexSentence = regexList[index];

                // Si au moins un des mot-clé est trouvé dans la liste, on continue la vérification pour tableaux de regex suivants,
                // dans le cas contraite on sort de la boucle
                if (!rowRegexSentence.Any(pattern => Regex.Match(userQuery.Query, pattern).Success))
                    regexMatch = false;

                index++;
            } while ((index < regexList.Count) && regexMatch);

            return regexMatch;
        }

        private static bool CheckTriggerBot(UserQuery userQuery, bool callBotRequired)
        {
            string[] callBotContains = { "bot", "jarvis" };
            bool triggerBot = (callBotContains.Any(botname => userQuery.QueryProcessed.Contains(botname.AddWhiteSpaceAroundString())) && callBotRequired) || !callBotRequired;

            return triggerBot;
        }

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
