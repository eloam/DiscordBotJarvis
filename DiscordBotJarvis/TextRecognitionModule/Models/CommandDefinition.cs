using DiscordBotJarvis.TextRecognitionModule.Enums;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DiscordBotJarvis.TextRecognitionModule.Models
{
    public class CommandDefinition
    {
        // Property
        private Sentence[] sentences;
        private IEnumerable<string[]> keywords;
        private IEnumerable<Regex[]> regex;
        private bool callBotRequired;
        private ComparisonModeEnum comparisonMode;

        public Sentence[] Sentences
        {
            get { return sentences; }
            set { sentences = value; }
        }

        public IEnumerable<string[]> Keywords
        {
            get { return keywords; }
            set { keywords = value; }
        }

        public IEnumerable<Regex[]> Regex
        {
            get { return regex; }
            set { regex = value; }
        }

        public bool CallBotRequired
        {
            get { return callBotRequired; }
            set { callBotRequired = value; }
        }

        public ComparisonModeEnum ComparisonMode
        {
            get { return comparisonMode; }
            set { comparisonMode = value; }
        }

        public bool IsListKeywordsEmpty
        {
            get
            {
                return keywords == null ? true : false;
            }
        }

        public bool IsListRegexEmpty
        {
            get
            {
                return regex == null ? true : false;
            }
        }

        // Contructors
        private CommandDefinition(Sentence[] sentences, bool callBotRequired = true, ComparisonModeEnum comparisonMode = ComparisonModeEnum.Contains)
        {
            this.Sentences = sentences;
            this.CallBotRequired = callBotRequired;
            this.ComparisonMode = comparisonMode;
        }

        public CommandDefinition(Sentence[] sentences, IEnumerable<string[]> keywords, 
            bool callBotRequired = true, ComparisonModeEnum comparisonMode = ComparisonModeEnum.Contains) : this (sentences, callBotRequired, comparisonMode)
        {
            this.Keywords = keywords;
        }

        public CommandDefinition(Sentence[] sentences, IEnumerable<Regex[]> regex, 
            bool callBotRequired = true, ComparisonModeEnum comparisonMode = ComparisonModeEnum.Contains) : this (sentences, callBotRequired, comparisonMode)
        {
            this.Regex = regex;
        }

        public CommandDefinition(Sentence[] sentences, IEnumerable<string[]> keywords, IEnumerable<Regex[]> regex, 
            bool callBotRequired = true, ComparisonModeEnum comparisonMode = ComparisonModeEnum.Contains) : this (sentences, keywords, callBotRequired, comparisonMode)
        {
            this.Regex = regex;
        }
    }
}
