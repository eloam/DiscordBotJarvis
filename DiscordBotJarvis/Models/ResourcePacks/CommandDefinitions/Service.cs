using System;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace DiscordBotJarvis.Models.ResourcePacks.CommandDefinitions
{
    public class Service : Feedback
    {
        private string _serviceName;

        public string ServiceName
        {
            get { return _serviceName; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));

                if (new Regex(@"(\w*)\.(\w*)").Match(value).Success)
                {
                    _serviceName = value;
                }
                else
                {
                    throw new ArgumentException(nameof(ServiceName));
                }
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
    }
}
