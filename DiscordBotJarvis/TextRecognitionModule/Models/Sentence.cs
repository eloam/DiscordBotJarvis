using DiscordBotCaptainObvious.Cortana.Enums;
using DiscordBotJarvis.Cortana.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiscordBotCaptainObvious.Cortana.Models
{
    public class Sentence
    {
        // old
        private SentencesEnum[] saySentences;
        private string[] keywordsOld;
        private ParametersEnum[] parameters;
        private bool callBotRequiredOld;
        private ComparisonModeEnum comparisonModeOld;

        [Obsolete]
        public SentencesEnum[] SaySentences
        {
            get { return saySentences; }
            set { saySentences = value; }
        }

        [Obsolete]
        public string[] KeywordsOld
        {
            get { return keywordsOld; }
            set { keywordsOld = value; }
        }

        [Obsolete]
        public ParametersEnum[] Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }

        [Obsolete]
        public bool CallBotRequiredOld
        {
            get { return callBotRequiredOld; }
            set { callBotRequiredOld = value; }
        }

        [Obsolete]
        public ComparisonModeEnum ComparisonModeOld
        {
            get { return comparisonMode; }
            set { comparisonMode = value; }
        }


        [Obsolete]
        public Sentence(SentencesEnum[] saySentences, string[] keywords, ParametersEnum[] parameters = null, 
            bool callBotRequiredOld = true, ComparisonModeEnum comparisonMode = ComparisonModeEnum.Contains)
        {
            this.SaySentences = saySentences;
            this.KeywordsOld = keywords;
            this.Parameters = parameters;
            this.CallBotRequiredOld = callBotRequiredOld;
            this.ComparisonMode = comparisonMode;
        }


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
            this.ComparisonMode = comparisonMode;
        }

        public Sentence(SentenceConfig[] sentences, IEnumerable <string[]> keywords, 
            bool callBotRequired = true, ComparisonModeEnum comparisonMode = ComparisonModeEnum.Contains) : this (sentences, callBotRequired ,comparisonMode)
        {
            this.Keywords = keywords;
        }

        public Sentence(SentenceConfig[] sentences, IEnumerable <Regex[]> regex, 
            bool callBotRequired = true, ComparisonModeEnum comparisonMode = ComparisonModeEnum.Contains) : this (sentences, callBotRequired, comparisonMode)
        {
            this.Regex = regex;
        }

        public Sentence(SentenceConfig[] sentences, IEnumerable <string[]> keywords, IEnumerable<Regex[]> regex, 
            bool callBotRequired = true, ComparisonModeEnum comparisonMode = ComparisonModeEnum.Contains) : this (sentences, keywords, callBotRequired, comparisonMode)
        {
            this.Regex = regex;
        }
    }
}
