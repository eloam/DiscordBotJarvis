using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace DiscordBotJarvis.Models.Client
{
    public class BotConfig
    {
        private string[] _botNames;
        private string _resourcePacksCurrentCulture;

        [DefaultValue(new[] { "bot" })]
        public string[] BotNames
        {
            get { return _botNames; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(_botNames), "La balise est manquante.");

                if (value.Length > 0)
                    SetRegexBotNames(value);

                _botNames = value;
            }
        }

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

        [XmlIgnore]
        public Regex BotNamesRegex { get; set; }

        public BotConfig()
        {
            BotNames = new[] { "bot" };
            ResourcePacksCurrentCulture = "fr-FR";
            LogFileName = "Lastest.log";
        }

        public BotConfig(string logFileName, string resourcePacksCurrentCulture, string[] botNames)
        {
            BotNames = botNames;
            LogFileName = logFileName;
            ResourcePacksCurrentCulture = resourcePacksCurrentCulture;
        }

        private void SetRegexBotNames(string[] botNames)
        {
            // On réalise une concaténation des différents nom du bot afin de regrouper en une seule chaîne de caractères (regex)
            StringBuilder botNameConcatenate = new StringBuilder().Append("(^| )(");
            for (int item = 0; item < botNames.Length; item++)
            {
                botNameConcatenate.Append(botNames[item]);
                botNameConcatenate.Append(item < botNames.Length - 1 ? "|" : @")($|\.| )");
            }

            BotNamesRegex = new Regex(botNameConcatenate.ToString(), 
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture | RegexOptions.Compiled);
        }
    }
}
