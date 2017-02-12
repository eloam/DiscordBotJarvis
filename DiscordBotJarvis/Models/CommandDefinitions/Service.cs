using System;
using System.Text.RegularExpressions;

namespace DiscordBotJarvis.Models.CommandDefinitions
{
    public class Service : Feedback
    {
        public string ServiceName { get; set; }
        public string LibraryName { get; set; }
        public string ClassName { get; set; }

        public Service(string serviceName)
        {
            Regex rgx = new Regex(@"(\w*)\.(\w*)");

            if (!rgx.Match(serviceName).Success)
                throw new ArgumentException("ServiceName is invalid. Expected : 'LibraryName.ClassName'.", nameof(serviceName));

            string[] path = serviceName.Split('.');

            ServiceName = serviceName;
            LibraryName = path[0];
            ClassName = path[1];
        }

        public Service(string libraryName, string className)
        {
            ServiceName = $"{libraryName}.{className}";
            LibraryName = libraryName;
            ClassName = className;
        }
    }
}
