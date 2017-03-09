using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using DiscordBotJarvis.Enums;
using DiscordBotJarvis.Interfaces;

namespace DiscordBotJarvis.Models.ResourcePacks.CommandDefinitions
{
    [XmlInclude(typeof(Sentence))]
    [XmlInclude(typeof(SentenceFile))]
    [XmlInclude(typeof(Service))]
    public class CommandSet : IXmlFeedbacksDeserializationCallback
    {
        // Property
        public Feedback[] Feedbacks { get; set; }
 
        [XmlArrayItem("Keywords")]
        public List<string[]> KeywordsList { get; set; }

        [XmlArrayItem("Regex")]
        public List<string[]> RegexList { get; set; }

        [DefaultValue(true)]
        public bool BotMentionRequired { get; set; }

        [DefaultValue(KeywordsComparison.Contains)]
        public KeywordsComparison KeywordsComparisonMode { get; set; }

        [XmlIgnore]
        public bool IsListKeywordsEmpty => KeywordsList?.Count == 0;

        [XmlIgnore]
        public bool IsListRegexEmpty => RegexList?.Count == 0;

        // Contructors
        public CommandSet()
        {
            KeywordsList = new List<string[]>();
            RegexList = new List<string[]>();
            BotMentionRequired = true;
            KeywordsComparisonMode = KeywordsComparison.Contains;
        }

        private CommandSet(Feedback[] feedbacks, bool botMentionRequired = true, KeywordsComparison keywordsComparisonMode = KeywordsComparison.Contains)
        {
            Feedbacks = feedbacks;
            BotMentionRequired = botMentionRequired;
            KeywordsComparisonMode = keywordsComparisonMode;
        }

        public CommandSet(Feedback[] feedbacks, List<string[]> keywords,
            bool botMentionRequired = true, KeywordsComparison keywordsComparisonMode = KeywordsComparison.Contains) : this(feedbacks, botMentionRequired, keywordsComparisonMode)
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
            bool botMentionRequired = true, KeywordsComparison keywordsComparisonMode = KeywordsComparison.Contains) : this(feedbacks, botMentionRequired, keywordsComparisonMode)
        {
            if (keywords == null && regex == null)
                throw new ArgumentNullException($"{nameof(keywords)}, {nameof(regex)}");

            KeywordsList = keywords;
            RegexList = regex;
        }

        public void OnXmlDeserialization(object sender)
        {
        }
    }
}
