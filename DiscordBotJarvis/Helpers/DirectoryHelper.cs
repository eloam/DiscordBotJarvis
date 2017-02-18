using System.IO;
using System.Linq;

namespace DiscordBotJarvis.Helpers
{
    public static class DirectoryHelper
    {
        public static bool AllDirectoriesExists(params string[] paths)
        {
            bool result = true;

            int index = 0;
            do
            {
                if (!Directory.Exists(paths[index]))
                    result = false;

                index++;
            } while ((index < paths.Length) && result);

            return result;
        }
    }
}
