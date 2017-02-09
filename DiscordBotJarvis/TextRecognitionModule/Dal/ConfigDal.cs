using DiscordBotJarvis.TextRecognitionModule.Models.CreateConfigFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotJarvis.TextRecognitionModule.Dal
{
    public class ConfigDal
    {
        public static ConfigFile GetConfig()
        {
            ConfigFile cf = new ConfigFile();

            // General
            cf.Title = "Titre";
            cf.Author = "Auteur";
            cf.Description = "Description";
            cf.Language = "fr-FR";
            cf.AppVersion = 0.1;
            cf.ResourcePackVersion = 1.0;

            // Paths
            cf.Paths.Files.Add("Changelog", ".");
            cf.Paths.Files.Add("ReadmeFile", ".");

            cf.Paths.Folders.Add("CommandDefinitions", "/CommandDefinitions");
            cf.Paths.Folders.Add("Mods", "/Mods");
            cf.Paths.Folders.Add("Resources", "/Resources");
            cf.Paths.Folders.Add("SentenceFiles", "/SentenceFiles");
            cf.Paths.Folders.Add("Texts", "/Texts");

            return cf;
        }
    }
}
