namespace DiscordBotJarvis.Models.CreateConfigFile
{
    public class ConfigFile
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public double AppVersion { get; set; }
        public double ResourcePackVersion { get; set; }
        public Path Paths { get; set; }

        public ConfigFile()
        {
            Paths = new Path();
        }

        public ConfigFile(string title, string author, string description, string language, double appVersion, double resourcePackVersion, Path paths)
        {
            Title = title;
            Author = author;
            Description = description;
            Language = language;
            AppVersion = appVersion;
            ResourcePackVersion = resourcePackVersion;
            Paths = paths;
        }
    }
}
