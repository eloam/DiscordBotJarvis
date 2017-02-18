namespace DiscordBotJarvis.Models.ResourcePacks.ConfigFile
{
    public class ResourcePackConfig
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string AppVersionMinimumSupport { get; set; }
        public string ResourcePackVersion { get; set; }
        public string UpdateUri { get; set; }
        public string Sha1 { get; set; }

        public ResourcePackConfig()
        {
        }

        public ResourcePackConfig(string title, string author, string description, string appVersionMinimumSupport, string resourcePackVersion, 
            string updateUri = null, string sha1 = null)
        {
            Title = title;
            Author = author;
            Description = description;
            AppVersionMinimumSupport = appVersionMinimumSupport;
            ResourcePackVersion = resourcePackVersion;
            UpdateUri = updateUri;
            Sha1 = sha1;
        }
    }
}
