using System.Collections.Generic;
using System.IO;
using DiscordBotJarvis.Core;
using DiscordBotJarvis.Helpers;
using DiscordBotJarvis.Models.Queries;
using DiscordBotJarvis.Models.ResourcePacks;

namespace DiscordBotJarvis.Models.Client
{
    public class BotClient
    {
        public BotConfig Config { get; set; }

        public IEnumerable<ResourcePack> Commands { get; set; }

        public Logger Logger { get; set; }

        public BotClient(string configFilePath = "BotConfig.xml")
        {
            Config = File.Exists(configFilePath) ? XmlSerializerHelper.Decode<BotConfig>(configFilePath) : new BotConfig();
            Commands = ResourcePacksManage.LoadAll(Config.CultureInfo);
            Logger = new Logger(Config.LogFileName);
        }

        public string[] Query(UserQuery userQuery)
            => TextRecognition.ExecuteQuery(Config, userQuery, Commands);
    }
}
