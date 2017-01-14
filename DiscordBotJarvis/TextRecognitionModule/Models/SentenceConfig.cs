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
        private string filename;
        private ParametersEnum[] parameters;
        private SentenceExtractionTypeEnum sentenceExtractionType;
        private int indexSaySentence;

        public string Filename
        {
            get { return filename; }
            set { filename = value; }
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

        public SentenceConfig(string filename, ParametersEnum[] parameters = null,
            SentenceExtractionTypeEnum sentenceExtractionType = SentenceExtractionTypeEnum.OneSentenceRandom, 
            int indexSaySentence = 0)
        {
            this.Filename = filename;
            this.Parameters = parameters;
            this.SentenceExtractionType = sentenceExtractionType;
            this.IndexSaySentence = indexSaySentence;
        }
    }
}
