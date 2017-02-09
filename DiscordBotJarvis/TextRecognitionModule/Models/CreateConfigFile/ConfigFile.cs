namespace DiscordBotJarvis.TextRecognitionModule.Models.CreateConfigFile
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
            this.Paths = new Path();
        }


        public ConfigFile(string title, string author, string description, string language, double appVersion, double resourcePackVersion, Path paths)
        {
            this.Title = title;
            this.Author = author;
            this.Description = description;
            this.Language = language;
            this.AppVersion = appVersion;
            this.ResourcePackVersion = resourcePackVersion;
            this.Paths = paths;
        }
    }
}
