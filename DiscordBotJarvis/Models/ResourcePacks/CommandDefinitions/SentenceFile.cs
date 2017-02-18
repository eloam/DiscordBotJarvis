using System.ComponentModel;
using System.Xml.Serialization;
using DiscordBotJarvis.Enums;

namespace DiscordBotJarvis.Models.ResourcePacks.CommandDefinitions
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
            FileName = fileName;
            Parameters = parameters;
            FileReadMode = fileReadMode;

            if (fileReadMode == FileReadEnum.OneSentenceSpecified)
                ReadLineOfFile = indexSaySentence;
        }
    }
}
