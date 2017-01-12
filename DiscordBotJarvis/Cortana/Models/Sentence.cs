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
        private bool callBotRequired;
        private ComparisonModeEnum comparisonMode;

        public SentencesEnum[] SaySentences
        {
            get { return saySentences; }
            set { saySentences = value; }
        }

        public string[] KeywordsOld
        {
            get { return keywordsOld; }
            set { keywordsOld = value; }
        }

        public ParametersEnum[] Parameters
        {
            get { return parameters; }
            set { parameters = value; }
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

        public Sentence(SentencesEnum[] saySentences, string[] keywords, ParametersEnum[] parameters = null, 
            bool callBotRequired = true, ComparisonModeEnum comparisonMode = ComparisonModeEnum.Contains)
        {
            this.SaySentences = saySentences;
            this.KeywordsOld = keywords;
            this.Parameters = parameters;
            this.CallBotRequired = callBotRequired;
            this.ComparisonMode = comparisonMode;
        }


        // Property
        private SentenceConfig[] sentences;
        private IEnumerable<string[]> keywords;
        private IEnumerable<Regex[]> regex;

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


        // Contructors
        private Sentence(SentenceConfig[] sentences)
        {
            this.Sentences = sentences;
        }

        public Sentence(SentenceConfig[] sentences, IEnumerable<string[]> keywords) : this (sentences)
        {
            this.Keywords = keywords;
        }

        public Sentence(SentenceConfig[] sentences, IEnumerable<Regex[]> regex) : this (sentences)
        {
            this.Regex = regex;
        }

        public Sentence(SentenceConfig[] sentences, IEnumerable<string[]> keywords, IEnumerable<Regex[]> regex) : this (sentences, keywords)
        {
            this.Regex = regex;
        }
    }
}
