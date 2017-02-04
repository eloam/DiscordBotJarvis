using DiscordBotJarvis.TextRecognitionModule.Enums;
using DiscordBotJarvis.TextRecognitionModule.Extensions;
using DiscordBotJarvis.TextRecognitionModule.Helpers;
using DiscordBotJarvis.TextRecognitionModule.Models;
using DSharpPlus;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiscordBotJarvis.TextRecognitionModule.Controllers
{
    static class JarvisCoreController
    {
        delegate bool comparisonModeDelegate(string keyword);

        public static void ExecuteQuery(MessageCreateEventArgs e, IEnumerable<CommandDefinition> commandDefinitions)
        {
            // Message de l'utilisateur non modifiés
            string request = e.Message.Content;
            // Requête de l'utilisateur modifiées afin de pouvoir effectuées correctement la vérifications par mots-clés
            string requestPrepared = request.ToLower().Trim().AddWhiteSpaceAroundString().RemoveDiacritics().ReplaceSpecialsChar();
            // Message(s) fournit par le bot à l'utilisateur
            string response = string.Empty;

            // On parcours toutes les lignes de la liste de phrases
            foreach (CommandDefinition command in commandDefinitions)
            {
                if (command.IsListKeywordsEmpty && command.IsListRegexEmpty)
                    throw new ArgumentNullException("Au moins un des deux liste de tableaux, keywords ou regex doivent être valorisées.");

                bool keywordsMatch = false;
                if (!command.IsListKeywordsEmpty)
                    keywordsMatch = CheckKeywordsMatch(requestPrepared, (List<string[]>)command.Keywords ?? null, command.ComparisonMode);

                bool regexMatch = false;
                if (!command.IsListRegexEmpty)
                    regexMatch = CheckRegexMatch(request, (List<Regex[]>)command.Regex ?? null);

                // Determination du résultat
                bool resultMatch = false;
                if (!command.IsListKeywordsEmpty && !command.IsListRegexEmpty)
                    resultMatch = keywordsMatch && regexMatch;
                else if (!command.IsListKeywordsEmpty)
                    resultMatch = keywordsMatch;
                else if (!command.IsListRegexEmpty)
                    resultMatch = regexMatch;

                // On regarde si le bot a besoin d'être appelé et que si c'est le cas, que son nom figure dans la requête de l'utilisateur
                bool triggerBot = CheckTriggerBot(request, command.CallBotRequired);

                // Si le résultat correspond aux mots-clés et expressions régulières définit dans l'objet et si le bot doit être appelé, alors...
                if (resultMatch && triggerBot)
                {
                    // On parcours toutes les objets SentenceConfig afin d'afficher leurs contenus
                    foreach (SentenceFile sentenceConfig in command.Sentences)
                    {
                        string[] parameters = sentenceConfig.Parameters != null ? ConvertParametersToValues(e, sentenceConfig.Parameters, request) : new string[0];
                        switch (sentenceConfig.SentenceExtractionType)
                        {
                            case SentenceExtractionTypeEnum.OneSentenceRandom:
                                response = String.Format(GetSentenceHelper.SayRandom(sentenceConfig.Filename), parameters);
                                break;
                            case SentenceExtractionTypeEnum.OneSentenceSpecified:
                                response = String.Format(GetSentenceHelper.Say(sentenceConfig.Filename, sentenceConfig.IndexSaySentence), parameters);
                                break;
                            case SentenceExtractionTypeEnum.File:
                                response = String.Format(GetSentenceHelper.ReadFile(sentenceConfig.Filename), parameters);
                                break;
                            default:
                                break;
                        }

                        e.Message.Respond(response);
                    }
                    break;
                }
                else
                {
                    continue;
                }
            }
        }

        private static bool CheckKeywordsMatch(string request, List<string[]> keywords, ComparisonModeEnum comparisonMode)
        {
            // Delegate permettant de définir le mode de comparaison de la requête (en début/fin de str ou n'importe ou dans la requete)
            comparisonModeDelegate comparisonModeDel = keyword =>
            {
                bool result = false;
                switch (comparisonMode)
                {
                    case ComparisonModeEnum.StartsWith:
                        result = request.StartsWith(keyword);
                        break;
                    case ComparisonModeEnum.Contains:
                        result = request.Contains(keyword);
                        break;
                    case ComparisonModeEnum.EndsWith:
                        result = request.EndsWith(keyword);
                        break;
                    default:
                        break;
                }
                return result;
            };

            // Déclaration d'un booléen indiquant si la requete correspond aux mots clés définit dans l'objet Sentence'
            bool keywordsMatch = true;

            // Recherche si la requête de l'utilisateur correspond aux mots-clés de l'objet Sentence
            int index = 0;
            keywordsMatch = true;
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

        private static bool CheckRegexMatch(string request, List<Regex[]> regex)
        {
            // Déclaration d'un booléen indiquant si la requete correspond aux expressions regulières définit dans l'objet Sentence'
            bool regexMatch = false;

            // Recherche si la requête de l'utilisateur correspond aux expressions régulières de l'objet Sentence
            int index = 0;
            regexMatch = true;
            do
            {
                // Obtenir à partir de la liste de regex, la ligne correspond a son indice
                Regex[] rowRegexSentence = regex[index];

                // Si au moins un des mot-clé est trouvé dans la liste, on continue la vérification pour tableaux de regex suivants,
                // dans le cas contraite on sort de la boucle
                if (!rowRegexSentence.Any(pattern => Regex.Match(request, pattern.ToString()).Success))
                    regexMatch = false;

                index++;
            } while ((index < regex.Count) && regexMatch);

            return regexMatch;
        }

        private static bool CheckTriggerBot(string request, bool callBotRequired)
        {
            string[] callBotContains = new string[] { "bot", "jarvis" };
            bool triggerBot = false;

            if ((callBotContains.Any(botname => request.Contains(botname.AddWhiteSpaceAroundString())) && callBotRequired) || !callBotRequired)
            {
                triggerBot = true;
            }

            return triggerBot;
        }

        private static string[] ConvertParametersToValues(MessageCreateEventArgs e, ParametersEnum[] parameters, String request)
        {
            List<string> valuesParameters = new List<string>();
            foreach (ParametersEnum item in parameters)
            {
                switch (item)
                {
                    case ParametersEnum.MessageAuthorMention:
                        valuesParameters.Add(e.Message.Author.Mention);
                        break;
                    case ParametersEnum.StatsGameOverwatch:
                        String battletag = request.Replace("#", "-");
                        String url = $"https://owapi.net/api/v3/u/{battletag}/stats";


                        HttpWebRequest webrequest = (HttpWebRequest)HttpWebRequest.Create(url);
                        webrequest.UserAgent = "discordbotjarvis";
                        try
                        {
                            using (StreamReader reader = new StreamReader(webrequest.GetResponse().GetResponseStream()))
                            {
                                string res = reader.ReadToEnd();
                            }
                        }
                        catch (WebException ex)
                        {
                            using (System.Net.WebResponse webresponse = ex.Response)
                            {
                                HttpWebResponse httpResponse = (HttpWebResponse)webresponse;
                                Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                                using (Stream data = webresponse.GetResponseStream())
                                using (var reader = new StreamReader(data))
                                {
                                    string text = reader.ReadToEnd();
                                    Console.WriteLine(text);
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            return valuesParameters.ToArray();
        }
    }
}
