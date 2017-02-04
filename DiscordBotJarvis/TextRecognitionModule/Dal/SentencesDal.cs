using DiscordBotJarvis.TextRecognitionModule.Enums;
using DiscordBotJarvis.TextRecognitionModule.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DiscordBotJarvis.TextRecognitionModule.Dal
{
    public static class SentencesDal
    {
        public static IEnumerable<CommandDefinition> BuildListSentences()
        {
            // Créationde la liste temporaire des "Sentences"
            List<CommandDefinition> commandDefinitions = new List<CommandDefinition>();

            // Say "Hello"
            commandDefinitions.Add(new CommandDefinition(
                sentences: new SentenceFile[]
                {
                    new SentenceFile("SayHello", new ParametersEnum[] { ParametersEnum.MessageAuthorMention })
                },
                keywords: new List<string[]>()
                {
                    new string[] { "bonjour", "bjr", "salut", "salut", "hi", "hello", "yo" }
                },
                callBotRequired: false,
                comparisonMode: ComparisonModeEnum.StartsWith));

            // Say "Goodbye"
            commandDefinitions.Add(new CommandDefinition(
                sentences: new SentenceFile[]
                {
                    new SentenceFile("SayGoodbye", new ParametersEnum[] { ParametersEnum.MessageAuthorMention })
                },
                keywords: new List<string[]>()
                {
                    new string[] { "bye", "a+", "++", "@+" }
                },
                callBotRequired: false,
                comparisonMode: ComparisonModeEnum.StartsWith));
            commandDefinitions.Add(new CommandDefinition(
                sentences: new SentenceFile[]
                {
                    new SentenceFile("SayGoodbye", new ParametersEnum[] { ParametersEnum.MessageAuthorMention })
                },
                keywords: new List<string[]>()
                {
                    new string[] { "au revoir", "j'y vais", "j'y go", "je go", "bonne nuit", "tchuss" }
                },
                callBotRequired: false));

            // Say "I'm fine"
            commandDefinitions.Add(new CommandDefinition(
                sentences: new SentenceFile[]
                {
                    new SentenceFile("SayImFine")
                },
                keywords: new List<string[]>()
                {
                    new string[] { "comment" },
                    new string[] { "vas tu", "tu vas", "ca va" }
                }));
            commandDefinitions.Add(new CommandDefinition(
                sentences: new SentenceFile[]
                {
                    new SentenceFile("SayImFine")
                },
                keywords: new List<string[]>()
                {
                    new string[] { "tu vas bien", "la forme" }
                }));

            // Say "De rien"
            commandDefinitions.Add(new CommandDefinition(
                sentences: new SentenceFile[]
                {
                    new SentenceFile("SayDeRien", new ParametersEnum[] { ParametersEnum.MessageAuthorMention })
                },
                keywords: new List<string[]>()
                {
                    new string[] { "merci", "remerci", "nice", "thank you", "thanks", "thx", "ty" }
                }));

            // Play csgo russian song
            commandDefinitions.Add(new CommandDefinition(
                sentences: new SentenceFile[]
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
            commandDefinitions.Add(new CommandDefinition(
                sentences: new SentenceFile[]
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
            commandDefinitions.Add(new CommandDefinition(
                sentences: new SentenceFile[]
                {
                    new SentenceFile("SayPunchlineSentence")
                },
                keywords: new List<string[]>()
                {
                    new string[] { "punchline", "connard", "putain", "encule" }
                }));

            // Say obvious sentence
            commandDefinitions.Add(new CommandDefinition(
                sentences: new SentenceFile[]
                {
                    new SentenceFile("SayObviousSentence")
                },
                keywords: new List<string[]>()
                {
                    new string[] { "obvious" }
                }));

            // Say russian sentence
            commandDefinitions.Add(new CommandDefinition(
                sentences: new SentenceFile[]
                {
                    new SentenceFile("SayRussianInsult")
                },
                keywords: new List<string[]>()
                {
                    new string[] { "insulte"},
                    new string[] { "russie", "russe", "russes", "polonais", "russian" },
                }));

            // Say "tout a fait"
            commandDefinitions.Add(new CommandDefinition(
                sentences: new SentenceFile[]
                {
                    new SentenceFile("SayToutAFait", new ParametersEnum[] { ParametersEnum.MessageAuthorMention })
                },
                keywords: new List<string[]>()
                {
                    new string[] { "n'est ce pas jarvis ?", "n'est ce pas bot ?"}
                }));

            // Say "Quel est ton battletag"
            commandDefinitions.Add(new CommandDefinition(
                sentences: new SentenceFile[]
                {
                    new SentenceFile("SayStatsJeuOverwatch", new ParametersEnum[] { ParametersEnum.MessageAuthorMention })
                },
                keywords: new List<string[]>()
                {
                    new string[] { "stats" },
                    new string[] { "overwatch", "ow" }
                }));

            // Regex Battletag (UserName#9999)
            commandDefinitions.Add(new CommandDefinition(
                sentences: new SentenceFile[]
                {
                    new SentenceFile("SayStatsJeuOverwatch", new ParametersEnum[] { ParametersEnum.StatsGameOverwatch }, SentenceExtractionTypeEnum.OneSentenceSpecified, 0)
                },
                regex: new List<Regex[]>()
                {
                    new Regex[] { new Regex(@"([a-zA-ZÀÁÂÃÄÅÇÑñÇçÈÉÊËÌÍÎÏÒÓÔÕÖØÙÚÛÜÝàáâãäåçèéêëìíîïðòóôõöøùúûüýÿ])([\wÀÁÂÃÄÅÇÑñÇçÈÉÊËÌÍÎÏÒÓÔÕÖØÙÚÛÜÝàáâãäåçèéêëìíîïðòóôõöøùúûüýÿ]{2,11})#\d{4}") },
                },
                callBotRequired: false));

            return commandDefinitions;
        }
    }
}
