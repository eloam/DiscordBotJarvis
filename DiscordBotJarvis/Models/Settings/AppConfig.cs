using System.ComponentModel;
using DiscordBotJarvis.Enums;

namespace DiscordBotJarvis.Models.Settings
{
    public class AppConfig
    {
        [DefaultValue("fr-FR")]
        public string ResourcePacksCurrentCulture { get; set; }
   
        [DefaultValue("Lastest.log")]
        public string LogFileName { get; set; }

        [DefaultValue(AppExecutionMode.None)]
        public AppExecutionMode ExecuteMode { get; set; }

        public AppConfig()
        {
            ResourcePacksCurrentCulture = "fr-FR";
            LogFileName = "Lastest.log";
            ExecuteMode = AppExecutionMode.None;
        }

        public AppConfig(string resourcePacksCurrentCulture, string logFileName, AppExecutionMode executeMode)
        {
            ResourcePacksCurrentCulture = resourcePacksCurrentCulture;
            LogFileName = logFileName;
            ExecuteMode = executeMode;
        }
    }
}
