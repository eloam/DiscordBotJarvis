using DiscordBotCaptainObvious.Cortana.Enums;
using DiscordBotJarvis.Cortana.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotJarvis.Cortana.Models
{
    public class SentenceConfig
    {
        private string nameSentenceFile;
        private ParametersEnum[] parameters;
        private SentenceExtractionTypeEnum sentenceExtractionType;
        private int indexSaySentence;
        private bool callBotRequired;
        private ComparisonModeEnum comparisonMode;


        public string NameSentenceFile
        {
            get { return nameSentenceFile; }
            set { nameSentenceFile = value; }
        }

        public ParametersEnum[] Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }

        public SentenceExtractionTypeEnum SentenceExtractionType
        {
            get { return sentenceExtractionType; }
            set { sentenceExtractionType = value; }
        }

        public int IndexSaySentence
        {
            get { return indexSaySentence; }
            set { indexSaySentence = value; }
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

        public SentenceConfig(string nameSentenceFile, ParametersEnum[] parameters = null, bool callBotRequired = true, 
            ComparisonModeEnum comparisonMode = ComparisonModeEnum.Contains, 
            SentenceExtractionTypeEnum sentenceExtractionType = SentenceExtractionTypeEnum.OneSentenceRandom, 
            int indexSaySentence = 0)
        {
            this.NameSentenceFile = nameSentenceFile;
            this.Parameters = parameters;
            this.IndexSaySentence = indexSaySentence;
            this.CallBotRequired = callBotRequired;
            this.ComparisonMode = comparisonMode;
            this.SentenceExtractionType = sentenceExtractionType;
            this.IndexSaySentence = indexSaySentence;
        }
    }
}
