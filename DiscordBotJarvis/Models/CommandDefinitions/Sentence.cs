using System.Xml.Serialization;
using DiscordBotJarvis.Enums;

namespace DiscordBotJarvis.Models.CommandDefinitions
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
            Phrase = phrase;
        }

        public Sentence(string phrase, ParametersEnum[] parameters) : this(phrase)
        {
            Parameters = parameters;
        }
    }
}
