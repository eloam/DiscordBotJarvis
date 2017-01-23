using DiscordBotJarvis.TextRecognitionModule.Enums;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DiscordBotJarvis.TextRecognitionModule.Models
{
    public class Sentence
    {
        // Property
        private SentenceConfig[] sentences;
        private IEnumerable<string[]> keywords;
        private IEnumerable<Regex[]> regex;
        private bool callBotRequired;
        private ComparisonModeEnum comparisonMode;

        public SentenceConfig[] Sentences
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

        // Contructors
        private Sentence(SentenceConfig[] sentences, bool callBotRequired = true, ComparisonModeEnum comparisonMode = ComparisonModeEnum.Contains)
        {
            this.Sentences = sentences;
            this.CallBotRequired = callBotRequired;
            this.ComparisonMode = comparisonMode;
        }

        public Sentence(SentenceConfig[] sentences, IEnumerable<string[]> keywords, 
            bool callBotRequired = true, ComparisonModeEnum comparisonMode = ComparisonModeEnum.Contains) : this (sentences, callBotRequired, comparisonMode)
        {
            this.Keywords = keywords;
        }

        public Sentence(SentenceConfig[] sentences, IEnumerable<Regex[]> regex, 
            bool callBotRequired = true, ComparisonModeEnum comparisonMode = ComparisonModeEnum.Contains) : this (sentences, callBotRequired, comparisonMode)
        {
            this.Regex = regex;
        }

        public Sentence(SentenceConfig[] sentences, IEnumerable<string[]> keywords, IEnumerable<Regex[]> regex, 
            bool callBotRequired = true, ComparisonModeEnum comparisonMode = ComparisonModeEnum.Contains) : this (sentences, keywords, callBotRequired, comparisonMode)
        {
            this.Regex = regex;
        }
    }
}
