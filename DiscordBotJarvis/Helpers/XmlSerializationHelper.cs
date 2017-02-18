using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DiscordBotJarvis.Models.ResourcePacks.CommandDefinitions;
using DiscordBotJarvis.Models.Settings;

namespace DiscordBotJarvis.Helpers
{
    public static class XmlSerializationHelper
    {
        public static void Encode<T>(object obj, string path, XmlRootAttribute xmlRootAttribute = null)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType(), xmlRootAttribute);
            using (StreamWriter writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, obj);
            }
        }

        public static T Decode<T>(string path, XmlRootAttribute xmlRootAttribute = null)
        {
            T commands;

            XmlSerializer serializer = new XmlSerializer(typeof(T), xmlRootAttribute);
            using (StreamReader reader = new StreamReader(path))
            {
                commands = (T) serializer.Deserialize(reader);
            }

            return commands;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public static string Test()
        {
            throw new ArgumentNullException();
        }
    }
}
