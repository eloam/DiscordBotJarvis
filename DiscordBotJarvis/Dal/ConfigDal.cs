using DiscordBotJarvis.Models.CreateConfigFile;

namespace DiscordBotJarvis.Dal
{
    public class ConfigDal
    {
        public static ConfigFile GetConfig()
        {
            ConfigFile cf = new ConfigFile
            {
                Title = "Titre",
                Author = "Auteur",
                Description = "Description",
                Language = "fr-FR",
                AppVersion = 0.1,
                ResourcePackVersion = 1.0
            };

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
