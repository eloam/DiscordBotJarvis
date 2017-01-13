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

            return sentences;
        }

        public static IEnumerable<Sentence> BuildListSentencesOld()
        {
            // Créationde la liste temporaire des "Sentences"
            List<Sentence> sentences = new List<Sentence>();

            // Say "Hello"
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Hello }, new string[] { "bonjour" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }, false, ComparisonModeEnum.StartsWith));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Hello }, new string[] { "bjr" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }, false, ComparisonModeEnum.StartsWith));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Hello }, new string[] { "salut" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }, false, ComparisonModeEnum.StartsWith));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Hello }, new string[] { "hi" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }, false, ComparisonModeEnum.StartsWith));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Hello }, new string[] { "hello" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }, false, ComparisonModeEnum.StartsWith));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Hello }, new string[] { "yo" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }, false, ComparisonModeEnum.StartsWith));

            // Say "Goodbye"
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Goodbye }, new string[] { "bye" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }, false, ComparisonModeEnum.StartsWith));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Goodbye }, new string[] { "a+" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }, false, ComparisonModeEnum.StartsWith));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Goodbye }, new string[] { "++" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }, false, ComparisonModeEnum.StartsWith));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Goodbye }, new string[] { "@+" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }, false, ComparisonModeEnum.StartsWith));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Goodbye }, new string[] { "au revoir" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }, false));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Goodbye }, new string[] { "j'y vais" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }, false));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Goodbye }, new string[] { "j'y go" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }, false));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Goodbye }, new string[] { "je go" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }, false));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Goodbye }, new string[] { "bonne nuit" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }, false));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Goodbye }, new string[] { "tchuss" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }, false));

            // Say "De rien"
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.DeRien }, new string[] { "merci" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.DeRien }, new string[] { "remerci" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.DeRien }, new string[] { "nice" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.DeRien }, new string[] { "thank you" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.DeRien }, new string[] { "thanks" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.DeRien }, new string[] { "thx" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.DeRien }, new string[] { "ty" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }));

            // Say punchline sentence
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Punchline }, new string[] { "punchline" }));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Punchline }, new string[] { "connard" }));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Punchline }, new string[] { "putain" }));

            // Say obvious sentence
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Obvious }, new string[] { "obvious" }));

            // Say russian sentence
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Russian }, new string[] { "russie" }));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.Russian }, new string[] { "russes" }));

            // Say "Tout à fait Thierry"
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.ToutAFait }, new string[] { "n'est ce pas captain ?" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }, true, ComparisonModeEnum.EndsWith));

            // Say "Stats compte de jeu Overwatch"
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.StatsJeuOverwatch }, new string[] { "stats", "overwatch" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.StatsJeuOverwatch }, new string[] { "stats", "ow" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }));

            // Say "Play CS GO song"
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.YesOrder, SentencesEnum.PlayMusiqueCsGo }, new string[] { "musique", "cs" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.YesOrder, SentencesEnum.PlayMusiqueCsGo }, new string[] { "musique", "csgo" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }));
            sentences.Add(new Sentence(new SentencesEnum[] { SentencesEnum.YesOrder, SentencesEnum.PlayMusiqueCsGo }, new string[] { "musique", "cs", "go" }, new ParametersEnum[] { ParametersEnum.MessageAuthorMention }));

            // Return list sentences
            return sentences;
        }
    }
}
