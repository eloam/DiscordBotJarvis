using DiscordBotCaptainObvious.Cortana.Enums;
using DiscordBotCaptainObvious.Cortana.Helpers;
using DiscordBotCaptainObvious.Cortana.Models;
using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotCaptainObvious.Cortana.Controllers
{
    static class CortanaCore
    {
        delegate bool comparisonModeDelegate(string keyword);

        public static void ExecuteQuery(MessageCreateEventArgs e, IEnumerable<Sentence> sentences)
        {
            string request = FormatStringHelper.NormalizeQuery(e.Message.Content);
            string response = string.Empty;

            string[] callBotContains = new string[] { "bot", "jarvis" };

            foreach (Sentence item in sentences)
            {
                comparisonModeDelegate comparisonModeDel = keyword =>
                {
                    bool result = false;
                    switch (item.ComparisonMode)
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

                if (item.Keywords.All(keyword => comparisonModeDel(keyword)))
                {
                    if ((callBotContains.Any(botname => request.Contains(botname) && item.CallBotRequired)) || !item.CallBotRequired)
                    {
                        string[] parameters = item.Parameters != null ? ConvertParametersToValues(e, item.Parameters) : new string[0];
                        foreach (SentencesEnum sentence in item.SaySentences)
                        {
                            e.Message.Respond(String.Format(GetSentenceHelper.SayRandom(sentence), parameters));
                        }
                        break;
                    }
                }
            }
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
