using DiscordBotJarvis.TextRecognitionModule.Enums;
using System.ComponentModel;
using System.Xml.Serialization;

namespace DiscordBotJarvis.TextRecognitionModule.Models.CommandDefinitions
{
    public class SentenceFile : Feedback
    {
        public string FileName { get; set; }

        [XmlArrayItem("Item")]
        public ParametersEnum[] Parameters { get; set; }

        [DefaultValue(FileReadEnum.OneSentenceRandom)]
        public FileReadEnum FileReadMode { get; set; }

        [DefaultValue(0)]
        public int ReadLineOfFile { get; set; }

        public SentenceFile()
        {
        }

        public SentenceFile(string fileName, ParametersEnum[] parameters = null,
            FileReadEnum fileReadMode = FileReadEnum.OneSentenceRandom,
            int indexSaySentence = 0)
        {
            this.FileName = fileName;
            this.Parameters = parameters;
            this.FileReadMode = fileReadMode;

            if (fileReadMode == FileReadEnum.OneSentenceSpecified)
                this.ReadLineOfFile = indexSaySentence;
        }
    }
}
