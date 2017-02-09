using DiscordBotJarvis.TextRecognitionModule.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace DiscordBotJarvis.TextRecognitionModule.Models.CommandDefinitions
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
        public bool IsListKeywordsEmpty
        {
            get
            {
                return this.KeywordsList == null ? true : false;
            }
        }

        [XmlIgnore]
        public bool IsListRegexEmpty
        {
            get
            {
                return this.RegexList == null ? true : false;
            }
        }

        // Contructors
        public CommandSet()
        {
            this.KeywordsList = new List<string[]>();
            this.RegexList = new List<string[]>();
            this.BotMentionRequired = true;
            this.KeywordsComparisonMode = KeywordsComparisonEnum.Contains;
        }

        private CommandSet(Feedback[] feedbacks, bool botMentionRequired = true, KeywordsComparisonEnum keywordsComparisonMode = KeywordsComparisonEnum.Contains)
        {
            this.Feedbacks = feedbacks;
            this.BotMentionRequired = botMentionRequired;
            this.KeywordsComparisonMode = keywordsComparisonMode;
        }

        public CommandSet(Feedback[] feedbacks, List<string[]> keywords,
            bool botMentionRequired = true, KeywordsComparisonEnum keywordsComparisonMode = KeywordsComparisonEnum.Contains) : this(feedbacks, botMentionRequired, keywordsComparisonMode)
        {
            if (keywords == null)
                throw new ArgumentException("Keywords is null.", "keywords");

            this.KeywordsList = keywords;
        }

        public CommandSet(Feedback[] feedbacks, List<string[]> regex, bool botMentionRequired = true) : this(feedbacks, botMentionRequired)
        {
            if (regex == null)
                throw new ArgumentException("Keywords is null.", "keywords");

            this.RegexList = regex;
        }

        public CommandSet(Feedback[] feedbacks, List<string[]> keywords, List<string[]> regex,
            bool botMentionRequired = true, KeywordsComparisonEnum keywordsComparisonMode = KeywordsComparisonEnum.Contains) : this(feedbacks, botMentionRequired, keywordsComparisonMode)
        {
            if (keywords == null && regex == null)
                throw new ArgumentException("Keywords and Regex are null.", "keywords, regex");

            this.KeywordsList = keywords;
            this.RegexList = regex;
        }
    }
}
