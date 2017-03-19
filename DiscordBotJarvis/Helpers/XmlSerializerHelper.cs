using System;
using System.IO;
using System.Xml.Serialization;
using DiscordBotJarvis.Core;

namespace DiscordBotJarvis.Helpers
{
    public static class XmlSerializerHelper
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
            T genericObject = default(T);

            // Vérification si les paramètres en entrées de fonction ne sont pas à 'null'
            if (path == null) throw new ArgumentNullException(nameof(path));

            XmlCallbackSerializer serializer = new XmlCallbackSerializer(typeof(T), xmlRootAttribute);
            using (StreamReader reader = new StreamReader(path))
            {
                genericObject = (T) serializer.Deserialize(reader);
            }

            return genericObject;
        }
    }
}
