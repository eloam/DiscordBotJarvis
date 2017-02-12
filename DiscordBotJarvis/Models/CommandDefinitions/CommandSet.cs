using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using DiscordBotJarvis.Enums;

namespace DiscordBotJarvis.Models.CommandDefinitions
{
    [XmlInclude(typeof(Sentence))]
    [XmlInclude(typeof(SentenceFile))]
    public class CommandSet
    {
        // Property
        public Feedback[] Feedbacks { get; set; }

        [XmlArrayItem("Keywords")]
        public List<string[]> KeywordsList { get; set; }

        [XmlArrayItem("Regex")]
        public List<string[]> RegexList { get; set; }

        [DefaultValue(true)]
        public bool BotMentionRequired { get; set; }

        [DefaultValue(KeywordsComparisonEnum.Contains)]
        public KeywordsComparisonEnum KeywordsComparisonMode { get; set; }

        [XmlIgnore]
        public bool IsListKeywordsEmpty => KeywordsList?.Count > 0;

        [XmlIgnore]
        public bool IsListRegexEmpty => RegexList?.Count > 0;

        // Contructors
        public CommandSet()
        {
            KeywordsList = new List<string[]>();
            RegexList = new List<string[]>();
            BotMentionRequired = true;
            KeywordsComparisonMode = KeywordsComparisonEnum.Contains;
        }

        private CommandSet(Feedback[] feedbacks, bool botMentionRequired = true, KeywordsComparisonEnum keywordsComparisonMode = KeywordsComparisonEnum.Contains)
        {
            Feedbacks = feedbacks;
            BotMentionRequired = botMentionRequired;
            KeywordsComparisonMode = keywordsComparisonMode;
        }

        public CommandSet(Feedback[] feedbacks, List<string[]> keywords,
            bool botMentionRequired = true, KeywordsComparisonEnum keywordsComparisonMode = KeywordsComparisonEnum.Contains) : this(feedbacks, botMentionRequired, keywordsComparisonMode)
        {
            if (keywords == null)
                throw new ArgumentNullException(nameof(keywords));

            KeywordsList = keywords;
        }

        public CommandSet(Feedback[] feedbacks, List<string[]> regex, bool botMentionRequired = true) : this(feedbacks, botMentionRequired)
        {
            if (regex == null)
                throw new ArgumentNullException(nameof(regex));

            RegexList = regex;
        }

        public CommandSet(Feedback[] feedbacks, List<string[]> keywords, List<string[]> regex,
            bool botMentionRequired = true, KeywordsComparisonEnum keywordsComparisonMode = KeywordsComparisonEnum.Contains) : this(feedbacks, botMentionRequired, keywordsComparisonMode)
        {
            if (keywords == null && regex == null)
                throw new ArgumentNullException($"{nameof(keywords)}, {nameof(regex)}");

            KeywordsList = keywords;
            RegexList = regex;
        }
    }
}
