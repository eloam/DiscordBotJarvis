using System;
using System.IO;
using System.Linq;
using System.Net;
using DiscordBotJarvis.Core;

namespace DiscordBotJarvis.Helpers
{
    public static class ResourcePacksHelper
    {
        public static bool AllSubdirectoriesResourcePackExists(string resourcePackDirectoryName, string currentCulture)
        {
            if (resourcePackDirectoryName == null) throw new ArgumentNullException(nameof(resourcePackDirectoryName));
            if (currentCulture == null) throw new ArgumentNullException(nameof(currentCulture));

            string subdirectoryCommandsPath = string.Format(EndPoints.Directory.ResourcePacksCommands, resourcePackDirectoryName, currentCulture);
            string subdirectoryResourcesPath = string.Format(EndPoints.Directory.ResourcePacksResources, resourcePackDirectoryName);
            string subdirectoryServicesPath = string.Format(EndPoints.Directory.ResourcePacksServices, resourcePackDirectoryName);

            bool allSubDirectoriesExists = Directory.Exists(subdirectoryCommandsPath)
                                           && Directory.Exists(subdirectoryResourcesPath)
                                           && Directory.Exists(subdirectoryServicesPath);

            return allSubDirectoriesExists;
        }
    }
}
