using DiscordBotCaptainObvious.Cortana.Enums;
using DiscordBotCaptainObvious.Cortana.Models;
using DiscordBotJarvis.Cortana.Enums;
using DiscordBotJarvis.Cortana.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotJarvis.Cortana.Dal
{
    public static class SentencesDal
    {
        public static IEnumerable<Sentence> BuildListSentences()
        {
            // Créationde la liste temporaire des "Sentences"
            List<Sentence> sentences = new List<Sentence>();

            // Say "Hello"
            sentences.Add(new Sentence(
                sentences: new SentenceConfig[]
                {
                    new SentenceConfig("SayHello", new ParametersEnum[] { ParametersEnum.MessageAuthorMention })
                },
                keywords: new List<string[]>()
                {
                    new string[] { "bonjour", "bjr", "salut", "salut", "hi", "hello", "yo" }
                },
                callBotRequired: false,
                comparisonMode: ComparisonModeEnum.StartsWith));

            // Say "Goodbye"
            sentences.Add(new Sentence(
                sentences: new SentenceConfig[]
                {
                    new SentenceConfig("SayGoodbye", new ParametersEnum[] { ParametersEnum.MessageAuthorMention })
                },
                keywords: new List<string[]>()
                {
                    new string[] { "bye", "a+", "++", "@+" }
                },
                callBotRequired: false,
                comparisonMode: ComparisonModeEnum.StartsWith));
            sentences.Add(new Sentence(
                sentences: new SentenceConfig[]
                {
                    new SentenceConfig("SayGoodbye", new ParametersEnum[] { ParametersEnum.MessageAuthorMention })
                },
                keywords: new List<string[]>()
                {
                    new string[] { "au revoir", "j'y vais", "j'y go", "je go", "bonne nuit", "tchuss" }
                },
                callBotRequired: false));

            // Say "De rien"
            sentences.Add(new Sentence(
                sentences: new SentenceConfig[]
                {
                    new SentenceConfig("SayDeRien", new ParametersEnum[] { ParametersEnum.MessageAuthorMention })
                },
                keywords: new List<string[]>()
                {
                    new string[] { "merci", "remerci", "nice", "thank you", "thanks", "thx", "ty" }
                }));

            // Play csgo russian song
            sentences.Add(new Sentence(
                sentences: new SentenceConfig[]
                {
                    new SentenceConfig("SayYesOrder", new ParametersEnum[] { ParametersEnum.MessageAuthorMention }),
                    new SentenceConfig("PlayCsGoRussianSong")
                },
                keywords: new List<string[]>()
                {
                    new string[] { "musique" },
                    new string[] { "cs", "csgo", "cs go" }
                }));

            // Say punchline sentence
            sentences.Add(new Sentence(
                sentences: new SentenceConfig[]
                {
                    new SentenceConfig("SayPunchlineSentence")
                },
                keywords: new List<string[]>()
                {
                    new string[] { "punchline", "connard", "putain", "encule" }
                }));

            // Say obvious sentence
            sentences.Add(new Sentence(
                sentences: new SentenceConfig[]
                {
                    new SentenceConfig("SayObviousSentence")
                },
                keywords: new List<string[]>()
                {
                    new string[] { "obvious" }
                }));

            // Say russian sentence
            sentences.Add(new Sentence(
                sentences: new SentenceConfig[]
                {
                    new SentenceConfig("SayRussianInsult")
                },
                keywords: new List<string[]>()
                {
                    new string[] { "insulte"},
                    new string[] { "russie", "russe", "russes", "polonais", "russian" },
                }));

            // Say "tout a fait"
            sentences.Add(new Sentence(
                sentences: new SentenceConfig[]
                {
                    new SentenceConfig("SayToutAFait")
                },
                keywords: new List<string[]>()
                {
                    new string[] { "n'est ce pas jarvis ?", "n'est ce pas bot ?"}
                }));

            // Say "Quel est ton battletag"
            sentences.Add(new Sentence(
                sentences: new SentenceConfig[]
                {
                    new SentenceConfig("SayStatsJeuOverwatch")
                },
                keywords: new List<string[]>()
                {
                    new string[] { "stats" },
                    new string[] { "overwatch", "ow" }
                }));

            return sentences;
        }
    }
}
