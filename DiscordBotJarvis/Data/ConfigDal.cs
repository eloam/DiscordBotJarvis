using System.Collections.Generic;
using DiscordBotJarvis.Models.ResourcePacks.ConfigFile;

namespace DiscordBotJarvis.Data
{
    public class ConfigDal
    {
        public static ResourcePackConfig GetConfig()
        {
            ResourcePackConfig resourcePacksConfig = new ResourcePackConfig
            {
                Title = "Titre",
                Author = "Auteur",
                Description = "Description",
                AppVersionMinimumSupport = "0.1",
                ResourcePackVersion = "1.0"
            };

            return resourcePacksConfig;
        }
    }
}
