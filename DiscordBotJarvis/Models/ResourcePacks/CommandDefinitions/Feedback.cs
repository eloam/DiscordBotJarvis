using System;
using DiscordBotJarvis.Interfaces;

namespace DiscordBotJarvis.Models.ResourcePacks.CommandDefinitions
{
    public abstract class Feedback : IXmlDeserializationCallback
    {
        public void OnXmlDeserialization(object sender)
        {
            throw new NotImplementedException();
        }
    }
}
