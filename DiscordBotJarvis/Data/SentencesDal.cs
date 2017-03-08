using System.Collections.Generic;
using DiscordBotJarvis.Enums;
using DiscordBotJarvis.Models.ResourcePacks.CommandDefinitions;

namespace DiscordBotJarvis.Data
{
    public static class SentencesDal
    {
 
        public static IEnumerable<CommandSet> Example1()
        {

            return new List<CommandSet>
            {
                new CommandSet(
                    feedbacks: new Feedback[]
                    {
                        new Sentence("Bonjour {0} !", new SentenceParameters[] { SentenceParameters.MessageAuthorMention })
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
                        new SentenceFile("SayGoodbye.txt", new SentenceParameters[] { SentenceParameters.MessageAuthorMention })
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
                        new SentenceFile("SayImFine.txt", new SentenceParameters[] { SentenceParameters.MessageAuthorMention }, ReadFileMode.OneSentenceSpecified, 2)
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
