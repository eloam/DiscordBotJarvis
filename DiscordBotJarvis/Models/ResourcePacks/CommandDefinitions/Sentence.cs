using System;
using System.Xml.Serialization;
using DiscordBotJarvis.Core;
using DiscordBotJarvis.Enums;
using DiscordBotJarvis.Extensions;
using DiscordBotJarvis.Interfaces;

namespace DiscordBotJarvis.Models.ResourcePacks.CommandDefinitions
{
    public class Sentence : Feedback, IXmlDeserializationCallback
    {
        private string _phrase;

        public string Phrase
        {
            get { return _phrase; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("La balise est vide ou est composé uniquement d'espaces blancs.", nameof(Phrase));

                _phrase = value;
            }
        }

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

        public new void OnXmlDeserialization(object sender)
        {
            if (Phrase == null) throw new ArgumentNullException(nameof(Phrase), "La balise est manquante.");
        }
    }
}