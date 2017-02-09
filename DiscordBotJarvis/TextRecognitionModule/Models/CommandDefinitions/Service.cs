using System;
using System.Text.RegularExpressions;

namespace DiscordBotJarvis.TextRecognitionModule.Models.CommandDefinitions
{
    public class Service : Feedback
    {
        private string serviceName;
        private string libraryName;
        private string className;

        public string ServiceName
        {
            get { return serviceName; }
            set { serviceName = value; }
        }

        public string LibraryName
        {
            get { return libraryName; }
            set { libraryName = value; }
        }

        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }

        public Service(string serviceName)
        {
            Regex rgx = new Regex(@"(\w*)\.(\w*)");

            if (!rgx.Match(serviceName).Success)
                throw new ArgumentException("ServiceName is invalid. Expected : 'LibraryName.ClassName'.", "serviceName");

            string[] path = serviceName.Split('.');

            this.ServiceName = serviceName;
            this.LibraryName = path[0];
            this.ClassName = path[1];
        }

        public Service(string libraryName, string className)
        {
            this.ServiceName = $"{libraryName}.{className}";
            this.LibraryName = libraryName;
            this.ClassName = className;
        }
    }
}
