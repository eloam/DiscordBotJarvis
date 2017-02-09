using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotJarvis.TextRecognitionModule.Models.CreateConfigFile
{
    public class Path
    {
        public Dictionary<string, string> Files { get; set; }
        public Dictionary<string, string> Folders { get; set; }

        public Path()
        {
            this.Files = new Dictionary<string, string>();
            this.Folders = new Dictionary<string, string>();
        }

        public Path(Dictionary<string, string> files, Dictionary<string, string> folders)
        {
            this.Files = files;
            this.Folders = folders;
        }
    }
}
