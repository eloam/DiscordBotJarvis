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
                    triggers: new List<string[]>()
                    {
                        new [] { "bonjour", "bjr", "salut", "hi", "hello", "yo" }
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
                    triggers: new List<string[]>()
                    {
                        new [] { "bonne nuit", "au revoir", "j'y vais", "a+ tlm" }
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
                    triggers: new List<string[]>()
                    {
                        new [] { "comment" },
                        new [] { "vas tu", "tu vas", "ca va" },
                        new [] { "?" }
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
                    triggers: new List<string[]>()
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
                    triggers: new List<string[]>()
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
                    triggers: new List<string[]>()
                    {
                        new [] { "test plugin" }
                    },
                    botMentionRequired: false)
            };
        }
    }
}
