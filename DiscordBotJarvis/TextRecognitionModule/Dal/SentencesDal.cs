using DiscordBotCaptainObvious.Cortana.Enums;
using DiscordBotCaptainObvious.Cortana.Models;
using DiscordBotJarvis.Cortana.Enums;
using DiscordBotJarvis.Cortana.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

            // Say "I'm fine"
            sentences.Add(new Sentence(
                sentences: new SentenceConfig[]
                {
                    new SentenceConfig("SayImFine")
                },
                keywords: new List<string[]>()
                {
                    new string[] { "comment" },
                    new string[] { "vas tu", "tu vas", "ca va" }
                }));
            sentences.Add(new Sentence(
                sentences: new SentenceConfig[]
                {
                    new SentenceConfig("SayImFine")
                },
                keywords: new List<string[]>()
                {
                    new string[] { "tu vas bien", "la forme" }
                }));

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
                    new string[] { "joue", "jouer", "play" },
                    new string[] { "musique", "music" },
                    new string[] { "cs", "csgo", "cs go" }
                }));

            // Play a song
            sentences.Add(new Sentence(
                sentences: new SentenceConfig[]
                {
                    new SentenceConfig("SayYesOrder", new ParametersEnum[] { ParametersEnum.MessageAuthorMention }),
                    new SentenceConfig("PlaySong")
                },
                keywords: new List<string[]>()
                {
                    new string[] { "joue", "jouer", "play" },
                    new string[] { "musique", "music" }
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
                    new SentenceConfig("SayToutAFait", new ParametersEnum[] { ParametersEnum.MessageAuthorMention })
                },
                keywords: new List<string[]>()
                {
                    new string[] { "n'est ce pas jarvis ?", "n'est ce pas bot ?"}
                }));

            // Say "Quel est ton battletag"
            sentences.Add(new Sentence(
                sentences: new SentenceConfig[]
                {
                    new SentenceConfig("SayStatsJeuOverwatch", new ParametersEnum[] { ParametersEnum.MessageAuthorMention })
                },
                keywords: new List<string[]>()
                {
                    new string[] { "stats" },
                    new string[] { "overwatch", "ow" }
                }));

            // Regex Battletag (UserName#9999)
            sentences.Add(new Sentence(
                sentences: new SentenceConfig[]
                {
                    new SentenceConfig("SayStatsJeuOverwatch", new ParametersEnum[] { ParametersEnum.MessageAuthorMention }, SentenceExtractionTypeEnum.OneSentenceSpecified, 0)
                },
                regex: new List<Regex[]>()
                {
                    new Regex[] { new Regex(@"([a-zA-ZÀÁÂÃÄÅÇÑñÇçÈÉÊËÌÍÎÏÒÓÔÕÖØÙÚÛÜÝàáâãäåçèéêëìíîïðòóôõöøùúûüýÿ])([\wÀÁÂÃÄÅÇÑñÇçÈÉÊËÌÍÎÏÒÓÔÕÖØÙÚÛÜÝàáâãäåçèéêëìíîïðòóôõöøùúûüýÿ]{2,11})#\d{4}") },
                },
                callBotRequired: false));

            return sentences;
        }
    }
}
