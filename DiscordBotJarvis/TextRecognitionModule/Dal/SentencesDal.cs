using DiscordBotJarvis.TextRecognitionModule.Enums;
using DiscordBotJarvis.TextRecognitionModule.Models.CommandDefinitions;
using System.Collections.Generic;

namespace DiscordBotJarvis.TextRecognitionModule.Dal
{
    public static class SentencesDal
    {
        public static IEnumerable<CommandSet> BuildListSentences()
        {
            // Créationde la liste temporaire des "Sentences"
            List<CommandSet> listCommands = new List<CommandSet>();

            // Say "Hello"
            listCommands.Add(new CommandSet(
                feedbacks: new Sentence[]
                {
                    new Sentence("Bonjour {0} !", new ParametersEnum[] { ParametersEnum.MessageAuthorMention })
                },
                keywords: new List<string[]>()
                {
                    new string[] { "bonjour" }
                },
                botMentionRequired: false));

            listCommands.Add(new CommandSet(
                feedbacks: new SentenceFile[]
                {
                    new SentenceFile("SayHello", new ParametersEnum[] { ParametersEnum.MessageAuthorMention })
                },
                keywords: new List<string[]>()
                {
                    new string[] { "bjr", "salut", "salut", "hi", "hello", "yo" }
                },
                botMentionRequired: false,
                keywordsComparisonMode: KeywordsComparisonEnum.StartsWith));

            // Say "Goodbye"
            listCommands.Add(new CommandSet(
                feedbacks: new SentenceFile[]
                {
                    new SentenceFile("SayGoodbye", new ParametersEnum[] { ParametersEnum.MessageAuthorMention })
                },
                keywords: new List<string[]>()
                {
                    new string[] { "bye", "a+", "++", "@+" }
                },
                botMentionRequired: false,
                keywordsComparisonMode: KeywordsComparisonEnum.StartsWith));

            listCommands.Add(new CommandSet(
                feedbacks: new SentenceFile[]
                {
                    new SentenceFile("SayGoodbye", new ParametersEnum[] { ParametersEnum.MessageAuthorMention })
                },
                keywords: new List<string[]>()
                {
                    new string[] { "au revoir", "j'y vais", "j'y go", "je go", "bonne nuit", "tchuss" }
                },
                botMentionRequired: false));

            // Say "I'm fine"
            listCommands.Add(new CommandSet(
                feedbacks: new SentenceFile[]
                {
                    new SentenceFile("SayImFine")
                },
                keywords: new List<string[]>()
                {
                    new string[] { "comment" },
                    new string[] { "vas tu", "tu vas", "ca va" }
                }));

            listCommands.Add(new CommandSet(
                feedbacks: new SentenceFile[]
                {
                    new SentenceFile("SayImFine")
                },
                keywords: new List<string[]>()
                {
                    new string[] { "tu vas bien", "la forme" }
                }));

            // Say "De rien"
            listCommands.Add(new CommandSet(
                feedbacks: new SentenceFile[]
                {
                    new SentenceFile("SayDeRien", new ParametersEnum[] { ParametersEnum.MessageAuthorMention })
                },
                keywords: new List<string[]>()
                {
                    new string[] { "merci", "remerci", "nice", "thank you", "thanks", "thx", "ty" }
                }));

            // Play csgo russian song
            listCommands.Add(new CommandSet(
                feedbacks: new SentenceFile[]
                {
                    new SentenceFile("SayYesOrder", new ParametersEnum[] { ParametersEnum.MessageAuthorMention }),
                    new SentenceFile("PlayCsGoRussianSong")
                },
                keywords: new List<string[]>()
                {
                    new string[] { "joue", "jouer", "play" },
                    new string[] { "musique", "music" },
                    new string[] { "cs", "csgo", "cs go" }
                }));

            // Play a song
            listCommands.Add(new CommandSet(
                feedbacks: new SentenceFile[]
                {
                    new SentenceFile("SayYesOrder", new ParametersEnum[] { ParametersEnum.MessageAuthorMention }),
                    new SentenceFile("PlaySong")
                },
                keywords: new List<string[]>()
                {
                    new string[] { "joue", "jouer", "play" },
                    new string[] { "musique", "music" }
                }));

            // Say punchline sentence
            listCommands.Add(new CommandSet(
                feedbacks: new SentenceFile[]
                {
                    new SentenceFile("SayPunchlineSentence")
                },
                keywords: new List<string[]>()
                {
                    new string[] { "punchline", "connard", "putain", "encule" }
                }));

            // Say obvious sentence
            listCommands.Add(new CommandSet(
                feedbacks: new SentenceFile[]
                {
                    new SentenceFile("SayObviousSentence")
                },
                keywords: new List<string[]>()
                {
                    new string[] { "obvious" }
                }));

            // Say russian sentence
            listCommands.Add(new CommandSet(
                feedbacks: new SentenceFile[]
                {
                    new SentenceFile("SayRussianInsult")
                },
                keywords: new List<string[]>()
                {
                    new string[] { "insulte"},
                    new string[] { "russie", "russe", "russes", "polonais", "russian" },
                }));

            // Say "tout a fait"
            listCommands.Add(new CommandSet(
                feedbacks: new SentenceFile[]
                {
                    new SentenceFile("SayToutAFait", new ParametersEnum[] { ParametersEnum.MessageAuthorMention })
                },
                keywords: new List<string[]>()
                {
                    new string[] { "n'est ce pas jarvis ?", "n'est ce pas bot ?"}
                }));

            // Say "Quel est ton battletag"
            listCommands.Add(new CommandSet(
                feedbacks: new SentenceFile[]
                {
                    new SentenceFile("SayStatsJeuOverwatch", new ParametersEnum[] { ParametersEnum.MessageAuthorMention })
                },
                keywords: new List<string[]>()
                {
                    new string[] { "stats" },
                    new string[] { "overwatch", "ow" }
                }));

            // Regex Battletag (UserName#9999)
            listCommands.Add(new CommandSet(
                feedbacks: new SentenceFile[]
                {
                    new SentenceFile("SayStatsJeuOverwatch", new ParametersEnum[] { ParametersEnum.StatsGameOverwatch }, FileReadEnum.OneSentenceSpecified, 0)
                },
                regex: new List<string[]>()
                {
                    new string[] { @"([a-zA-ZÀÁÂÃÄÅÇÑñÇçÈÉÊËÌÍÎÏÒÓÔÕÖØÙÚÛÜÝàáâãäåçèéêëìíîïðòóôõöøùúûüýÿ])([\wÀÁÂÃÄÅÇÑñÇçÈÉÊËÌÍÎÏÒÓÔÕÖØÙÚÛÜÝàáâãäåçèéêëìíîïðòóôõöøùúûüýÿ]{2,11})#\d{4}" },
                },
                botMentionRequired: false));

            return listCommands;
        }
    }
}
