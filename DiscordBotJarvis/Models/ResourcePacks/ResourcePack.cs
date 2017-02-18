using System.Collections.Generic;
using DiscordBotJarvis.Models.ResourcePacks.CommandDefinitions;
using DiscordBotJarvis.Models.ResourcePacks.ConfigFile;

namespace DiscordBotJarvis.Models.ResourcePacks
{
    public class ResourcePack
    {
        public string DirectoryName;
        public ResourcePackConfig Infos { get; set; }
        public Dictionary<string, List<CommandSet>> CommandsDictionary { get; set; }

        public ResourcePack()
        {
                Infos = new ResourcePackConfig();
                CommandsDictionary = new Dictionary<string, List<CommandSet>>();
        }

        public ResourcePack(string directoryName, ResourcePackConfig infos, Dictionary<string, List<CommandSet>> commandsDictionary)
        {
            DirectoryName = directoryName;
            Infos = infos;
            CommandsDictionary = commandsDictionary;
        }
    }
}
