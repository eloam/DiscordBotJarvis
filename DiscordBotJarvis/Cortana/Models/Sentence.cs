using DiscordBotCaptainObvious.Cortana.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotCaptainObvious.Cortana.Models
{
    public class Sentence
    {
        private SentencesEnum[] saySentences;
        private string[] keywords;
        private ParametersEnum[] parameters;
        private bool callBotRequired;
        private ComparisonModeEnum comparisonMode;

        public SentencesEnum[] SaySentences
        {
            get { return saySentences; }
            set { saySentences = value; }
        }

        public string[] Keywords
        {
            get { return keywords; }
            set { keywords = value; }
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
            this.Keywords = keywords;
            this.Parameters = parameters;
            this.CallBotRequired = callBotRequired;
            this.ComparisonMode = comparisonMode;
        }
    }
}
