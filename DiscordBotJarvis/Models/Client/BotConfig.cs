using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;

namespace DiscordBotJarvis.Models.Client
{
    public class BotConfig
    {
        private string _resourcePacksCurrentCulture;

        [DefaultValue("Lastest.log")]
        public string LogFileName { get; set; }

        [DefaultValue("fr-FR")]
        public string ResourcePacksCurrentCulture {
            get { return _resourcePacksCurrentCulture; }
            set
            {
                CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures);
                CultureInfo cultureInfo = cultures.FirstOrDefault(c => c.Name.Equals(value, StringComparison.OrdinalIgnoreCase)) ?? new CultureInfo("fr-FR");

                CultureInfo = cultureInfo;
                _resourcePacksCurrentCulture = cultureInfo.Name;
            } }

        [XmlIgnore]
        public CultureInfo CultureInfo { get; set; }

        [DefaultValue(new[] { "bot" })]
        public string[] BotNames { get; set; }

        public BotConfig()
        {
            ResourcePacksCurrentCulture = "fr-FR";
            LogFileName = "Lastest.log";
            BotNames = new[] {"bot"};
        }

        public BotConfig(string logFileName, string resourcePacksCurrentCulture, string[] botNames)
        {
            LogFileName = logFileName;
            ResourcePacksCurrentCulture = resourcePacksCurrentCulture;
            BotNames = botNames;
        }
    }
}
