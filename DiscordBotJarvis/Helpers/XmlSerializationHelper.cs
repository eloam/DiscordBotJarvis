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
            // Vérification si les paramètres en entrées de fonction ne sont pas à 'null'
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (path == null) throw new ArgumentNullException(nameof(path));

            XmlSerializer serializer = new XmlSerializer(obj.GetType(), xmlRootAttribute);
            using (StreamWriter writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, obj);
            }
        }

        public static T Decode<T>(string path, XmlRootAttribute xmlRootAttribute = null)
        {
            // Vérification si les paramètres en entrées de fonction ne sont pas à 'null'
            if (path == null) throw new ArgumentNullException(nameof(path));

            T commands;

            XmlSerializer serializer = new XmlSerializer(typeof(T), xmlRootAttribute);
            using (StreamReader reader = new StreamReader(path))
            {
                commands = (T) serializer.Deserialize(reader);
            }

            return commands;
        }
    }
}
