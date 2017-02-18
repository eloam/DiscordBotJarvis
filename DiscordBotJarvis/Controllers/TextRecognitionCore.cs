using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DiscordBotJarvis.Enums;
using DiscordBotJarvis.Extensions;
using DiscordBotJarvis.Helpers;
using DiscordBotJarvis.Models.ResourcePacks;
using DiscordBotJarvis.Models.ResourcePacks.CommandDefinitions;
using DSharpPlus;

namespace DiscordBotJarvis.Controllers
{
    public static class UserQueryProcessing
    {
        private delegate bool ComparisonModeDelegate(string keyword);

        public static async void ExecuteQuery(MessageCreateEventArgs e, IEnumerable<ResourcePack> resourcePacksList)
        {
            // Requête de l'utilisateur
            string request = e.Message.Content;
            // Requête de l'utilisateur après traitement
            string requestProcessed = request.ProcessingUserRequest();

            // On parcours tous les packs de ressources afin de lire le dictionnaire CommandsList associé
            foreach (ResourcePack currentResourcePack in resourcePacksList)
            {
                // On parcours tous les fichiers xml situés dans le répertoire "CommandDefinitions" du pack de ressources courant
                foreach (KeyValuePair<string, List<CommandSet>> currentCommandDefinitionsKeyValuePair in currentResourcePack.CommandsDictionary)
                {
                    // On parcours toutes les commandes que contient le fichier xml courant, afin de recherche une correspondance eventuelle avec la requête utilisateur
                    foreach (CommandSet command in currentCommandDefinitionsKeyValuePair.Value)
                    {
                        // On regarde si les arguments (mots-clés et expressions regulières) correspondent à la requête
                        bool resultMatch = ArgumentsMatch(request, requestProcessed, command);

                        // On regarde si le bot a besoin d'être appelé et que si c'est le cas, que son nom figure dans la requête de l'utilisateur
                        bool triggerBot = CheckTriggerBot(request, command.BotMentionRequired);

                        // Si le résultat correspond aux mots-clés et expressions régulières définit dans l'objet et si le bot doit être appelé, alors...
                        if (resultMatch && triggerBot)
                        {
                            await DisplayFeedbacks(e, currentResourcePack, command.Feedbacks);
                            return;
                        }
                    }
                }
            }     
        }

        private static bool ArgumentsMatch(string request, string requestProcessed, CommandSet command)
        {
            // Resultat définitif
            bool resultMatch = false;
            
            // Indique si l'objet de type Feedback en cours de traitement correspond aux mot-clés et regex definies dans la requête provenant de l'utilisateur
            bool keywordsMatch = false;
            bool regexMatch = false;

            if (!command.IsListKeywordsEmpty)
                keywordsMatch = CheckKeywordsMatch(requestProcessed, command.KeywordsList, command.KeywordsComparisonMode);

            if (!command.IsListRegexEmpty)
                regexMatch = CheckRegexMatch(request, command.RegexList);

            // Determination du résultat
            if (!command.IsListKeywordsEmpty && !command.IsListRegexEmpty)
                resultMatch = keywordsMatch && regexMatch;
            else if (!command.IsListKeywordsEmpty)
                resultMatch = keywordsMatch;
            else if (!command.IsListRegexEmpty)
                resultMatch = regexMatch;

            return resultMatch;
        }

        private static async Task DisplayFeedbacks(MessageCreateEventArgs e, ResourcePack resourcePack, Feedback[] feedbacks)
        {
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
                        object[] parameters = ParametersToValuesConverter(e, sentence.Parameters);
                        response = String.Format(sentence.Phrase, parameters);
                    }
                    else
                    {
                        response = sentence.Phrase;
                    }
                }
                else if (feedback is SentenceFile)
                {
                    SentenceFile sentence = (SentenceFile)feedback;
                    string filePath = Endpoints.ResourcePacksDirectory + Endpoints.SeparatorDirectory + 
                                      resourcePack.DirectoryName + Endpoints.ResourcesDirectory +
                                      Endpoints.SeparatorDirectory + sentence.FileName + ".txt";

                    switch (sentence.FileReadMode)
                    {
                        case FileReadEnum.OneSentenceRandom:
                            response = GetSentenceHelper.ReadLineRandom(filePath);
                            break;
                        case FileReadEnum.OneSentenceSpecified:
                            response = GetSentenceHelper.ReadLineSpecified(filePath, sentence.ReadLineOfFile);
                            break;
                        case FileReadEnum.File:
                            response = GetSentenceHelper.ReadFile(filePath);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(sentence.FileReadMode), sentence.FileReadMode, "Argument not specified.");
                    }

                    if (sentence.Parameters != null)
                    {
                        object[] parameters = ParametersToValuesConverter(e, sentence.Parameters);
                        if (response != null) response = string.Format(response, parameters);
                    }
                    
                }

                // Si le message fournit par le bot à l'utilisateur est différent de null, vide ou composé uniquement d'espaces blancs.
                if (!String.IsNullOrWhiteSpace(response))
                    await e.Message.Respond(response);
            }
        }

        private static bool CheckKeywordsMatch(string requestProcessed, IReadOnlyList<string[]> keywords, KeywordsComparisonEnum comparisonMode)
        {
            // Delegate permettant de définir le mode de comparaison de la requête (en début/fin de str ou n'importe ou dans la requete)
            ComparisonModeDelegate comparisonModeDel = keyword =>
            {
                bool result;
                switch (comparisonMode)
                {
                    case KeywordsComparisonEnum.StartsWith:
                        result = requestProcessed.StartsWith(keyword);
                        break;
                    case KeywordsComparisonEnum.Contains:
                        result = requestProcessed.Contains(keyword);
                        break;
                    case KeywordsComparisonEnum.EndsWith:
                        result = requestProcessed.EndsWith(keyword);
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

        private static bool CheckRegexMatch(string request, List<string[]> regexList)
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
                if (!rowRegexSentence.Any(pattern => Regex.Match(request, pattern).Success))
                    regexMatch = false;

                index++;
            } while ((index < regexList.Count) && regexMatch);

            return regexMatch;
        }

        private static bool CheckTriggerBot(string request, bool callBotRequired)
        {
            string[] callBotContains = { "bot", "jarvis" };
            bool triggerBot = (callBotContains.Any(botname => request.Contains(botname.AddWhiteSpaceAroundString())) && callBotRequired) || !callBotRequired;

            return triggerBot;
        }

        private static object[] ParametersToValuesConverter(MessageCreateEventArgs e, ParametersEnum[] parameters)
        {
            List<object> valuesParameters = new List<object>();
            foreach (ParametersEnum item in parameters)
            {
                switch (item)
                {
                    case ParametersEnum.MessageAuthorMention:
                        valuesParameters.Add(e.Message.Author.Mention);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(parameters), parameters, "Argument not specified.");
                }
            }

            return valuesParameters.ToArray();
        }
    }
}
