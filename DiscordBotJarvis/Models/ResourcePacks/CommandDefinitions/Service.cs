using System;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using DiscordBotJarvis.Interfaces;

namespace DiscordBotJarvis.Models.ResourcePacks.CommandDefinitions
{
    public class Service : Feedback, IXmlDeserializationCallback
    {
        private string _serviceName;

        public string ServiceName
        {
            get { return _serviceName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("La balise est vide ou est composé uniquement d'espaces blancs.", nameof(ServiceName));

                if (!new Regex(@"(\w*)\.(\w*)").Match(value).Success)
                    throw new ArgumentException("La valeur de la balise doit correspondre au modèle suivant : " +
                                                $"[{nameof(LibraryName)}].[{nameof(ClassName)}]", nameof(ServiceName));

                _serviceName = value;
            }
        }

        [XmlIgnore]
        public string LibraryName => $"{ServiceName.Split('.')[0]}.dll";

        [XmlIgnore]
        public string ClassName => $"{ServiceName.Split('.')[1]}";

        public Service()
        {

        }

        public Service(string serviceName)
        {
            ServiceName = serviceName;
        }

        public new void OnXmlDeserialization(object sender)
        {
            if (ServiceName == null)
                throw new ArgumentNullException(nameof(ServiceName), "La balise est manquante.");
        }
    }
}
