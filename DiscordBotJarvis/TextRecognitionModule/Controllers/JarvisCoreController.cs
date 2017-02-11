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
    public static class JarvisCoreController
    {
        delegate bool comparisonModeDelegate(string keyword);

        public static void ExecuteQuery(MessageCreateEventArgs e, IEnumerable<CommandSet> commandsList)
        {
            // Requête de l'utilisateur
            string request = e.Message.Content;
            // Requête de l'utilisateur après traitement
            string requestProcessed = request.ProcessingUserRequest();

            // On parcours toutes les lignes de la liste de phrases
            foreach (CommandSet command in commandsList)
            {
                if (command.IsListKeywordsEmpty && command.IsListRegexEmpty)
                    throw new ArgumentNullException("Au moins un des deux liste de tableaux, keywords ou regex doivent être valorisées.");

                // On regarde si les arguments (mots-clés et expressions regulières) correspondent à la requête
                bool resultMatch = ArgumentsMatch(request, requestProcessed, command);

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

        private static bool ArgumentsMatch(string request, string requestProcessed, CommandSet command)
        {
            // Resultat définitif
            bool resultMatch = false;
            // Indique si l'objet de type Feedback en cours de traitement correspond aux mot-clés et regex definies dans la requête provenant de l'utilisateur
            bool keywordsMatch = false;
            bool regexMatch = false;

            if (!command.IsListKeywordsEmpty)
                keywordsMatch = CheckKeywordsMatch(requestProcessed, command.KeywordsList as List<string[]>, command.KeywordsComparisonMode);

            if (!command.IsListRegexEmpty)
                regexMatch = CheckRegexMatch(request, command.RegexList as List<string[]>);

            // Determination du résultat
            if (!command.IsListKeywordsEmpty && !command.IsListRegexEmpty)
                resultMatch = keywordsMatch && regexMatch;
            else if (!command.IsListKeywordsEmpty)
                resultMatch = keywordsMatch;
            else if (!command.IsListRegexEmpty)
                resultMatch = regexMatch;

            return resultMatch;
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
                    string[] parameters = sentence.Parameters != null ? ParametersToStringValuesConverter(e, sentence.Parameters) : new string[0];

                    response = String.Format(sentence.Phrase, parameters);
                }
                else if (feedback is SentenceFile)
                {
                    SentenceFile sentence = (SentenceFile)feedback;

                    string[] parameters = sentence.Parameters != null ? ParametersToStringValuesConverter(e, sentence.Parameters) : new string[0];
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

                // Si le message fournit par le bot à l'utilisateur est différent de null, vide ou composé uniquement d'espaces blancs.
                if (!String.IsNullOrWhiteSpace(response))
                    e.Message.Respond(response);
            }
        }

        private static bool CheckKeywordsMatch(string requestProcessed, List<string[]> keywords, KeywordsComparisonEnum comparisonMode)
        {
            // Delegate permettant de définir le mode de comparaison de la requête (en début/fin de str ou n'importe ou dans la requete)
            comparisonModeDelegate comparisonModeDel = keyword =>
            {
                bool result = false;
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

        private static string[] ParametersToStringValuesConverter(MessageCreateEventArgs e, ParametersEnum[] parameters)
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
