using DiscordBotJarvis.TextRecognitionModule.Enums;
using System.Xml.Serialization;

namespace DiscordBotJarvis.TextRecognitionModule.Models.CommandDefinitions
{
    public class Sentence : Feedback
    {
        public string Phrase { get; set; }

        [XmlArrayItem("Item")]
        public ParametersEnum[] Parameters { get; set; }

        public Sentence()
        {
        }

        public Sentence(string phrase)
        {
            this.Phrase = phrase;
        }

        public Sentence(string phrase, ParametersEnum[] parameters) : this(phrase)
        {
            this.Parameters = parameters;
        }
    }
}
