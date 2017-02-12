using System.Collections.Generic;

namespace DiscordBotJarvis.Models.CreateConfigFile
{
    public class Path
    {
        public Dictionary<string, string> Files { get; set; }
        public Dictionary<string, string> Folders { get; set; }

        public Path()
        {
            Files = new Dictionary<string, string>();
            Folders = new Dictionary<string, string>();
        }

        public Path(Dictionary<string, string> files, Dictionary<string, string> folders)
        {
            Files = files;
            Folders = folders;
        }
    }
}
