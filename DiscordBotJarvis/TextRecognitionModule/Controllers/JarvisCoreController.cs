using DiscordBotJarvis.TextRecognitionModule.Enums;
using DiscordBotJarvis.TextRecognitionModule.Extensions;
using DiscordBotJarvis.TextRecognitionModule.Helpers;
using DiscordBotJarvis.TextRecognitionModule.Models.CommandDefinitions;
using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DiscordBotJarvis.TextRecognitionModule.Controllers
{
    static class JarvisCoreController
    {
        delegate bool comparisonModeDelegate(string keyword);

        public static void ExecuteQuery(MessageCreateEventArgs e, IEnumerable<CommandSet> commandsList)
        {
            // Requête de l'utilisateur
            string request = e.Message.Content;
            // Requête de l'utilisateur après traitement
            string requestPrepared = request.ToLower().Trim().AddWhiteSpaceAroundString().RemoveDiacritics().ReplaceSpecialsChar();

            // On parcours toutes les lignes de la liste de phrases
            foreach (CommandSet command in commandsList)
            {
                if (command.IsListKeywordsEmpty && command.IsListRegexEmpty)
                    throw new ArgumentNullException("Au moins un des deux liste de tableaux, keywords ou regex doivent être valorisées.");

                bool keywordsMatch = false;
                if (!command.IsListKeywordsEmpty)
                    keywordsMatch = CheckKeywordsMatch(requestPrepared, (List<string[]>)command.KeywordsList ?? null, command.KeywordsComparisonMode);

                bool regexMatch = false;
                if (!command.IsListRegexEmpty)
                    regexMatch = CheckRegexMatch(request, (List<string[]>)command.RegexList ?? null);

                // Determination du résultat
                bool resultMatch = false;
                if (!command.IsListKeywordsEmpty && !command.IsListRegexEmpty)
                    resultMatch = keywordsMatch && regexMatch;
                else if (!command.IsListKeywordsEmpty)
                    resultMatch = keywordsMatch;
                else if (!command.IsListRegexEmpty)
                    resultMatch = regexMatch;

                // On regarde si le bot a besoin d'être appelé et que si c'est le cas, que son nom figure dans la requête de l'utilisateur
                bool triggerBot = CheckTriggerBot(request, command.BotMentionRequired);

                // Si le résultat correspond aux mots-clés et expressions régulières définit dans l'objet et si le bot doit être appelé, alors...
                if (resultMatch && triggerBot)
                {
                    DisplayFeedbacks(e, command.Feedbacks);
                    break;
                }
            }
        }

        private static void DisplayFeedbacks(MessageCreateEventArgs e, Feedback[] feedbacks)
        {
            // On parcours toutes les objets SentenceConfig afin d'afficher leurs contenus
            foreach (Feedback feedback in feedbacks)
            {
                // Message(s) fournit par le bot à l'utilisateur
                string response = null;

                if (feedback is Sentence)
                {
                    Sentence sentence = (Sentence)feedback;
                    string[] parameters = sentence.Parameters != null ? ConvertParametersToValues(e, sentence.Parameters) : new string[0];

                    response = String.Format(sentence.Phrase, parameters);
                }
                else if (feedback is SentenceFile)
                {
                    SentenceFile sentence = (SentenceFile)feedback;

                    string[] parameters = sentence.Parameters != null ? ConvertParametersToValues(e, sentence.Parameters) : new string[0];
                    switch (sentence.FileReadMode)
                    {
                        case FileReadEnum.OneSentenceRandom:
                            response = String.Format(GetSentenceHelper.SayRandom(sentence.FileName), parameters);
                            break;
                        case FileReadEnum.OneSentenceSpecified:
                            response = String.Format(GetSentenceHelper.Say(sentence.FileName, sentence.ReadLineOfFile), parameters);
                            break;
                        case FileReadEnum.File:
                            response = String.Format(GetSentenceHelper.ReadFile(sentence.FileName), parameters);
                            break;
                        default:
                            break;
                    }
                }

                // Si le message fournit par le bot à l'utilisateur est différent de null, vide ou composer uniquement d'espaces blancs.
                if (!String.IsNullOrWhiteSpace(response))
                    e.Message.Respond(response);
            }
        }

        private static bool CheckKeywordsMatch(string request, List<string[]> keywords, KeywordsComparisonEnum comparisonMode)
        {
            // Delegate permettant de définir le mode de comparaison de la requête (en début/fin de str ou n'importe ou dans la requete)
            comparisonModeDelegate comparisonModeDel = keyword =>
            {
                bool result = false;
                switch (comparisonMode)
                {
                    case KeywordsComparisonEnum.StartsWith:
                        result = request.StartsWith(keyword);
                        break;
                    case KeywordsComparisonEnum.Contains:
                        result = request.Contains(keyword);
                        break;
                    case KeywordsComparisonEnum.EndsWith:
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

        private static bool CheckRegexMatch(string request, List<string[]> regexList)
        {
            // Déclaration d'un booléen indiquant si la requete correspond aux expressions regulières définit dans l'objet Sentence'
            bool regexMatch = false;

            // Recherche si la requête de l'utilisateur correspond aux expressions régulières de l'objet Sentence
            int index = 0;
            regexMatch = true;
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
                }
            }

            return valuesParameters.ToArray();
        }
    }
}
