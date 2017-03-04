using System.Collections.Generic;
using DiscordBotJarvis.Enums;
using DiscordBotJarvis.Models.ResourcePacks.CommandDefinitions;

namespace DiscordBotJarvis.Data
{
    public static class SentencesDal
    {
        public static IEnumerable<CommandSet> BuildListSentences()
        {
            // Création de la liste temporaire des "Sentences"
            return new List<CommandSet>
            {
                // Say "Hello"
                new CommandSet(
                    feedbacks: new Feedback[]
                    {
                        new Sentence("Bonjour {0} !", new ParametersEnum[] {ParametersEnum.MessageAuthorMention})
                    },
                    keywords: new List<string[]>()
                    {
                        new string[] {"bonjour"}
                    },
                    botMentionRequired: false),
                new CommandSet(
                    feedbacks: new Feedback[]
                    {
                        new SentenceFile("SayHello", new ParametersEnum[] {ParametersEnum.MessageAuthorMention})
                    },
                    keywords: new List<string[]>()
                    {
                        new string[] {"bjr", "salut", "salut", "hi", "hello", "yo"}
                    },
                    botMentionRequired: false,
                    keywordsComparisonMode: KeywordsComparisonEnum.StartsWith),

                // Say "Goodbye"
                new CommandSet(
                    feedbacks: new Feedback[]
                    {
                        new SentenceFile("SayGoodbye", new ParametersEnum[] {ParametersEnum.MessageAuthorMention})
                    },
                    keywords: new List<string[]>()
                    {
                        new string[] {"bye", "a+", "++", "@+"}
                    },
                    botMentionRequired: false,
                    keywordsComparisonMode: KeywordsComparisonEnum.StartsWith),
                new CommandSet(
                    feedbacks: new Feedback[]
                    {
                        new SentenceFile("SayGoodbye", new ParametersEnum[] {ParametersEnum.MessageAuthorMention})
                    },
                    keywords: new List<string[]>()
                    {
                        new string[] {"au revoir", "j'y vais", "j'y go", "je go", "bonne nuit", "tchuss"}
                    },
                    botMentionRequired: false),
                // Say "I'm fine"
                new CommandSet(
                    feedbacks: new Feedback[]
                    {
                        new SentenceFile("SayImFine")
                    },
                    keywords: new List<string[]>()
                    {
                        new string[] {"comment"},
                        new string[] {"vas tu", "tu vas", "ca va"}
                    }),
                new CommandSet(
                    feedbacks: new Feedback[]
                    {
                        new SentenceFile("SayImFine")
                    },
                    keywords: new List<string[]>()
                    {
                        new string[] {"tu vas bien", "la forme"}
                    }),
                // Say "De rien"
                new CommandSet(
                    feedbacks: new Feedback[]
                    {
                        new SentenceFile("SayDeRien", new ParametersEnum[] {ParametersEnum.MessageAuthorMention})
                    },
                    keywords: new List<string[]>()
                    {
                        new string[] {"merci", "remerci", "nice", "thank you", "thanks", "thx", "ty"}
                    }),
                // Play csgo russian song
                new CommandSet(
                    feedbacks: new Feedback[]
                    {
                        new SentenceFile("SayYesOrder", new ParametersEnum[] {ParametersEnum.MessageAuthorMention}),
                        new SentenceFile("PlayCsGoRussianSong")
                    },
                    keywords: new List<string[]>()
                    {
                        new string[] {"joue", "jouer", "play"},
                        new string[] {"musique", "music"},
                        new string[] {"cs", "csgo", "cs go"}
                    }),
                // Play a song
                new CommandSet(
                    feedbacks: new Feedback[]
                    {
                        new SentenceFile("SayYesOrder", new ParametersEnum[] {ParametersEnum.MessageAuthorMention}),
                        new SentenceFile("PlaySong")
                    },
                    keywords: new List<string[]>()
                    {
                        new string[] {"joue", "jouer", "play"},
                        new string[] {"musique", "music"}
                    }),
                // Say punchline sentence
                new CommandSet(
                    feedbacks: new Feedback[]
                    {
                        new SentenceFile("SayPunchlineSentence")
                    },
                    keywords: new List<string[]>()
                    {
                        new string[] {"punchline", "connard", "putain", "encule"}
                    }),
                // Say obvious sentence
                new CommandSet(
                    feedbacks: new Feedback[]
                    {
                        new SentenceFile("SayObviousSentence")
                    },
                    keywords: new List<string[]>()
                    {
                        new string[] {"obvious"}
                    }),
                // Say russian sentence
                new CommandSet(
                    feedbacks: new Feedback[]
                    {
                        new SentenceFile("SayRussianInsult")
                    },
                    keywords: new List<string[]>()
                    {
                        new string[] {"insulte"},
                        new string[] {"russie", "russe", "russes", "polonais", "russian"},
                    }),
                // Say "tout a fait"
                new CommandSet(
                    feedbacks: new Feedback[]
                    {
                        new SentenceFile("SayToutAFait", new ParametersEnum[] {ParametersEnum.MessageAuthorMention})
                    },
                    keywords: new List<string[]>()
                    {
                        new string[] {"n'est ce pas jarvis ?", "n'est ce pas bot ?"}
                    }),
                // Say "Quel est ton battletag"
                new CommandSet(
                    feedbacks: new Feedback[]
                    {
                        new SentenceFile("SayStatsJeuOverwatch",
                            new ParametersEnum[] {ParametersEnum.MessageAuthorMention})
                    },
                    keywords: new List<string[]>()
                    {
                        new string[] {"stats"},
                        new string[] {"overwatch", "ow"}
                    }),
                // Regex Battletag (UserName#9999)
                new CommandSet(
                    feedbacks: new Feedback[]
                    {
                        new SentenceFile("SayStatsJeuOverwatch",
                            new ParametersEnum[] {ParametersEnum.MessageAuthorMention},
                            ReadFileMode.OneSentenceSpecified, 0)
                    },
                    regex: new List<string[]>()
                    {
                        new string[]
                        {
                            @"([a-zA-ZÀÁÂÃÄÅÇÑñÇçÈÉÊËÌÍÎÏÒÓÔÕÖØÙÚÛÜÝàáâãäåçèéêëìíîïðòóôõöøùúûüýÿ])([\wÀÁÂÃÄÅÇÑñÇçÈÉÊËÌÍÎÏÒÓÔÕÖØÙÚÛÜÝàáâãäåçèéêëìíîïðòóôõöøùúûüýÿ]{2,11})#\d{4}"
                        },
                    },
                    botMentionRequired: false)
            };
        }


        public static IEnumerable<CommandSet> Example1()
        {

            return new List<CommandSet>
            {
                new CommandSet(
                    feedbacks: new Feedback[]
                    {
                        new Sentence("Bonjour {0} !", new ParametersEnum[] { ParametersEnum.MessageAuthorMention })
                    },
                    keywords: new List<string[]>()
                    {
                        new string[] { "bonjour", "bjr", "salut", "hi", "hello", "yo" }
                    },
                    botMentionRequired: false)
            };
        }

        public static IEnumerable<CommandSet> Example2()
        {

            return new List<CommandSet>
            {
                new CommandSet(
                    feedbacks: new Feedback[]
                    {
                        new SentenceFile("SayGoodbye.txt", new ParametersEnum[] { ParametersEnum.MessageAuthorMention })
                    },
                    keywords: new List<string[]>()
                    {
                        new string[] { "bonne nuit", "au revoir", "j'y vais", "a+ tlm" }
                    })
            };
        }

        public static IEnumerable<CommandSet> Example3()
        {

            return new List<CommandSet>
            {
                new CommandSet(
                    feedbacks: new Feedback[]
                    {
                        new SentenceFile("SayImFine.txt", new ParametersEnum[] { ParametersEnum.MessageAuthorMention }, ReadFileMode.OneSentenceSpecified, 2)
                    },
                    keywords: new List<string[]>()
                    {
                        new string[] { "comment" },
                        new string[] { "vas tu", "tu vas", "ca va" },
                        new string[] { "?" }
                    })
            };
        }

        public static IEnumerable<CommandSet> Example4()
        {

            return new List<CommandSet>
            {
                new CommandSet(
                    feedbacks: new Feedback[]
                    {
                        new SentenceFile("PlaySong.txt")
                    },
                    regex: new List<string[]>()
                    {
                        new string[] { "(joue|jouer)(.*)(musique)" }
                    })
            };
        }

        public static IEnumerable<CommandSet> Example5()
        {

            return new List<CommandSet>
            {
                new CommandSet(
                    feedbacks: new Feedback[]
                    {
                        new SentenceFile("Help.txt", null, ReadFileMode.File)
                    },
                    regex: new List<string[]>()
                    {
                        new string[] { "(!help)" }
                    },
                    botMentionRequired: false)
            };
        }

        public static IEnumerable<CommandSet> Example6()
        {

            return new List<CommandSet>
            {
                new CommandSet(
                    feedbacks: new Feedback[]
                    {
                        new Service("ServiceTest.FirstService")
                    },
                    keywords: new List<string[]>()
                    {
                        new string[] { "test plugin" }
                    },
                    botMentionRequired: false)
            };
        }
    }
}
