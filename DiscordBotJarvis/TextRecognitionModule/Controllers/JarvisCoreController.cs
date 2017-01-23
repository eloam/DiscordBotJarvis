using DiscordBotJarvis.TextRecognitionModule.Enums;
using DiscordBotJarvis.TextRecognitionModule.Extensions;
using DiscordBotJarvis.TextRecognitionModule.Helpers;
using DiscordBotJarvis.TextRecognitionModule.Models;
using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DiscordBotJarvis.TextRecognitionModule.Controllers
{
    static class CortanaCore
    {
        delegate bool comparisonModeDelegate(string keyword);

        public static void ExecuteQuery(MessageCreateEventArgs e, IEnumerable<Sentence> sentences)
        {
            string request = e.Message.Content.Trim().AddWhiteSpaceAroundString().ToLower().RemoveDiacritics();
            string response = string.Empty;

            // On parcours toutes les lignes de la liste de phrases
            foreach (Sentence sentence in sentences)
            {
                bool keywordsMatch = CheckKeywordsMatch(request, (List<string[]>)sentence.Keywords ?? null, (List<Regex[]>)sentence.Regex ?? null, sentence.ComparisonMode);
                bool triggerBot = CheckTriggerBot(request, sentence.CallBotRequired);

                if (keywordsMatch && triggerBot)
                {
                    // On parcours toutes les objets SentenceConfig afin d'afficher leurs contenues
                    foreach (SentenceConfig sentenceConfig in sentence.Sentences)
                    {
                        string[] parameters = sentenceConfig.Parameters != null ? ConvertParametersToValues(e, sentenceConfig.Parameters) : new string[0];
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

        private static bool CheckKeywordsMatch(string request, List<string[]> keywords, List<Regex[]> regex, ComparisonModeEnum comparisonMode)
        {
            // On determine si la liste de mot-clés et de regex sont à l'état null
            bool lstKeywordsIsNull = keywords == null ? true : false;
            bool lstRegexIsNull = regex == null ? true : false;

            // Si les deux boolean sont à false :
            if (lstKeywordsIsNull && lstRegexIsNull)
                throw new ArgumentNullException("Au moins un des deux liste de tableaux, keywords ou regex doivent être valorisées.");

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

            // Déclaration des booléens indiquants si la requetes correspond aux mots clés et expression regulières de l'objet Sentence'
            bool keywordsMatch = false;
            bool regexMatch = false;

            // Recherche si la requête de l'utilisateur correspond aux mots-clés de l'objet Sentence
            if (!lstKeywordsIsNull)
            {
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
            }
            
            // Recherche si la requête de l'utilisateur correspond aux expressions régulières de l'objet Sentence
            if (!lstRegexIsNull)
            {
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
            }

            // Determination du résultat
            bool resultMatch = false;
            if (!lstKeywordsIsNull && !lstRegexIsNull)
                resultMatch = keywordsMatch && regexMatch;
            else if (!lstKeywordsIsNull)
                resultMatch = keywordsMatch;
            else if (!lstRegexIsNull)
                resultMatch = regexMatch;

            return resultMatch;
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

        private static string[] ConvertParametersToValues(MessageCreateEventArgs e, ParametersEnum[] parameters)
        {
            List<string> valuesParameters = new List<string>();
            foreach (ParametersEnum item in parameters)
            {
                switch (item)
                {
                    case ParametersEnum.MessageAuthorMention:
                        valuesParameters.Add(e.Message.Author.Mention);
                        break;
                    default:
                        break;
                }
            }

            return valuesParameters.ToArray();
        }
    }
}
