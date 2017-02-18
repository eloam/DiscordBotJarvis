using System.ComponentModel;
using System.Globalization;
using DiscordBotJarvis.Enums;

namespace DiscordBotJarvis.Models.Settings
{
    public class AppConfig
    {
        public string ResourcePacksCurrentCulture { get; set; }
        public ModeApplicationExecutionEnum ExectureMode { get; set; }

        public AppConfig()
        {
        }

        public AppConfig(string resourcePacksCurrentCulture, ModeApplicationExecutionEnum executeMode)
        {
            ResourcePacksCurrentCulture = resourcePacksCurrentCulture;
            ExectureMode = executeMode;
        }
    }
}
