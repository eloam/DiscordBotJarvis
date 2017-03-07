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

            if (!allSubDirectoriesExists)
            {
                if (!Directory.Exists(subdirectoryCommandsPath))
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [ERROR] L'emplacement suivant n'existe pas ou est incorrect : {subdirectoryCommandsPath}.");
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [ERROR] Il se peut que le pack de ressource ne supporte pas la langue qui est définie dans le fichier de configuration AppConfig.xml.");

                if (!Directory.Exists(subdirectoryResourcesPath))
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [ERROR] L'emplacement suivant n'existe pas ou est incorrect : {subdirectoryResourcesPath}");

                if (!Directory.Exists(subdirectoryServicesPath))
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [ERROR] L'emplacement suivant n'existe pas ou est incorrect : {subdirectoryServicesPath}");
            }

            return allSubDirectoriesExists;
        }
    }
}
