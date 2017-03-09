using System;
using System.Collections.Generic;
using DiscordBotJarvis.Interfaces;
using DiscordBotJarvis.Models.ResourcePacks.CommandDefinitions;
using DiscordBotJarvis.Models.ResourcePacks.ConfigFile;

namespace DiscordBotJarvis.Models.ResourcePacks
{
    public class ResourcePack : IXmlDeserializationCallback
    {
        public string DirectoryName;
        public ResourcePackConfig Config { get; set; }
        public Dictionary<string, IEnumerable<CommandSet>> Commands { get; set; }

        public ResourcePack()
        {
            Config = new ResourcePackConfig();
            Commands = new Dictionary<string, IEnumerable<CommandSet>>();
        }

        public ResourcePack(string directoryName, ResourcePackConfig config, Dictionary<string, IEnumerable<CommandSet>> commands)
        {
            DirectoryName = directoryName;
            Config = config;
            Commands = commands;
        }

        public void OnXmlDeserialization(object sender)
        {
            throw new NotImplementedException();
        }
    }
}
