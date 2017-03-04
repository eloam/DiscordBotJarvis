using System;
using System.Text.RegularExpressions;
using DiscordBotJarvis.Interfaces;

namespace DiscordBotJarvis.Models.ResourcePacks.ConfigFile
{
    public class ResourcePackConfig : IXmlDeserializationCallback
    {
        private string _title;
        private string _author;
        private string _appVersionMinimumSupport;
        private string _resourcePackVersion;

        public string Title
        {
            get { return _title; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("La balise est vide ou est composé uniquement d'espaces blancs.", nameof(Title));

                _title = value;
            }
            
        }

        public string Author
        {
            get { return _author; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("La balise est vide ou est composé uniquement d'espaces blancs.", nameof(Author));

                _author = value;
            }
        }

        public string Description { get; set; }

        public string AppVersionMinimumSupport
        {
            get { return _appVersionMinimumSupport; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("La balise est vide ou est composé uniquement d'espaces blancs.", nameof(AppVersionMinimumSupport));

                if (!new Regex(@"(\w*)\.(\w*)").Match(value).Success)
                    throw new ArgumentException("La valeur de la balise doit correspondre au modèle suivant : " +
                                                "'[MajorVersion].[MinorVersion]'", nameof(AppVersionMinimumSupport));

                _appVersionMinimumSupport = value;
            }
        }

        public string ResourcePackVersion
        {
            get { return _resourcePackVersion; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("La balise est vide ou est composé uniquement d'espaces blancs.", nameof(ResourcePackVersion));

                if (!new Regex(@"(\w*)\.(\w*)").Match(value).Success)
                    throw new ArgumentException("La valeur de la balise doit correspondre au modèle suivant : " +
                                                "'[MajorVersion].[MinorVersion]'", nameof(ResourcePackVersion));

                _resourcePackVersion = value;
            }
        }

        public string UpdateUri { get; set; }

        public string Sha1 { get; set; }

        public ResourcePackConfig()
        {
        }

        public ResourcePackConfig(string title, string author, string description, string appVersionMinimumSupport, string resourcePackVersion, 
            string updateUri = null, string sha1 = null)
        {
            Title = title;
            Author = author;
            Description = description;
            AppVersionMinimumSupport = appVersionMinimumSupport;
            ResourcePackVersion = resourcePackVersion;
            UpdateUri = updateUri;
            Sha1 = sha1;
        }

        public void OnXmlDeserialization(object sender)
        {         
            if (Title == null) throw new ArgumentNullException(nameof(Title), "La balise est manquante.");
            if (Author == null) throw new ArgumentNullException(nameof(Author), "La balise est manquante.");
            if (AppVersionMinimumSupport == null) throw new ArgumentNullException(nameof(AppVersionMinimumSupport), "La balise est manquante.");
            if (ResourcePackVersion == null) throw new ArgumentNullException(nameof(ResourcePackVersion), "La balise est manquante.");
        }
    }
}
