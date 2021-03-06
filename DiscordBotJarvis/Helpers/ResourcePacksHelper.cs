﻿using System;
using System.Globalization;
using System.IO;
using DiscordBotJarvis.Core;

namespace DiscordBotJarvis.Helpers
{
    public static class ResourcePacksHelper
    {
        public static bool AllSubdirectoriesResourcePackExists(string resourcePackDirectoryName, CultureInfo cultureInfo)
        {
            if (resourcePackDirectoryName == null) throw new ArgumentNullException(nameof(resourcePackDirectoryName));
            if (cultureInfo == null) throw new ArgumentNullException(nameof(cultureInfo));

            string subdirectoryCommandsPath = string.Format(EndPoints.Directory.ResourcePacksCommands, resourcePackDirectoryName, cultureInfo);
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
