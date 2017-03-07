using System;
using System.ComponentModel;
using System.Xml.Serialization;
using DiscordBotJarvis.Core;
using DiscordBotJarvis.Enums;
using DiscordBotJarvis.Extensions;
using DiscordBotJarvis.Interfaces;

namespace DiscordBotJarvis.Models.ResourcePacks.CommandDefinitions
{
    public class SentenceFile : Feedback, IXmlDeserializationCallback
    {
        private string _fileName;

        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("La balise est vide ou est composé uniquement d'espaces blancs.", nameof(FileName));

                _fileName = value;
            }
        }

        [XmlArrayItem("Item")]
        public SentenceParameters[] Parameters { get; set; }

        [DefaultValue(ReadFileMode.OneSentenceRandom)]
        public ReadFileMode FileReadMode { get; set; }

        [DefaultValue(0)]
        public int ReadLineOfFile { get; set; }

        public SentenceFile()
        {
        }

        public SentenceFile(string fileName, SentenceParameters[] parameters = null,
            ReadFileMode readFileMode = ReadFileMode.OneSentenceRandom,
            int indexSaySentence = 0)
        {
            FileName = fileName;
            Parameters = parameters;
            FileReadMode = readFileMode;

            if (readFileMode == ReadFileMode.OneSentenceSpecified)
                ReadLineOfFile = indexSaySentence;
        }

        public new void OnXmlDeserialization(object sender)
        {
            if (FileName == null) throw new ArgumentNullException(nameof(FileName), "La balise est manquante.");
        }
    }
}
