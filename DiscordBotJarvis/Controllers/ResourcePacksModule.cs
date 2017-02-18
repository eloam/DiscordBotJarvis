using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using DiscordBotJarvis.Helpers;
using DiscordBotJarvis.Models.ResourcePacks;
using DiscordBotJarvis.Models.ResourcePacks.CommandDefinitions;

namespace DiscordBotJarvis.Controllers
{
    public static class ResourcePackModule
    {
        /// <summary>
        /// Charger tous les ressources packs définies dans un répertoire
        /// </summary>
        public static IEnumerable<ResourcePack> LoadAll(string currentCulture)
        {
            // Déclaration de la variable destiné à contenir l'ensemble des commandes des différents packs de ressources
            List<ResourcePack> resourcePacks = new List<ResourcePack>();

            // Lecture du dossier ResourcesPacks, s'il n'existe pas on évalue pas le reste de la fonction
            if (!Directory.Exists(Endpoints.ResourcePacksDirectory)) return resourcePacks;

            // Liste de tous les répertoires ayant le fichier Config.Xml à la racine du sous-dossier
            string[] resourcesPacksPaths = Directory.GetDirectories(Endpoints.ResourcePacksDirectory);

            // Lecture de tous les répertoires (ResourcePacks)
            foreach (string resourcePackPath in resourcesPacksPaths)
            {
                // Load one
                ResourcePack currentResourcePack = new ResourcePack();

                // Obtenir le nom du dossier contenant le pack de ressources
                currentResourcePack.DirectoryName = resourcePackPath.Split('\\').LastOrDefault();

                // Emplacement du fichier "Config.xml"
                string configFilePath = resourcePackPath + Endpoints.ConfigFileName;

                // Emplacement du répertoire "CommandDefinitions"
                string commandDefinitionsDirectoryPath = resourcePackPath + Endpoints.CommandsDirectory + 
                                                         Endpoints.SeparatorDirectory + currentCulture;

                // Emplacement du répertoire "Resources"
                string resourcesDirectoryPath = resourcePackPath + Endpoints.ResourcesDirectory;

                // Emplacement du répertoire "Services"
                string servicesDirectoryPath = resourcePackPath + Endpoints.ServicesDirectory;

                // On vérifie si le pack de ressources possède à sa racine un fichier "Config.xml"
                if (!File.Exists(configFilePath)) continue;

                /* On vérifie qu'il existe les sous-dossiers suivants :
                 * "CommandDefinitions", "Resources" et "Services"
                 */
                if (!DirectoryHelper.AllDirectoriesExists(commandDefinitionsDirectoryPath, resourcesDirectoryPath, servicesDirectoryPath)) continue;

                // Lecture du fichier "Config.xml" du ressource pack
                try
                {
                    //currentResourcePack.Infos = XmlSerializationHelper.Decode<ResourcePackConfig>(configFilePath);
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [ERROR] Error reading the \"Config.xml\" file from the \"{currentResourcePack.DirectoryName}\" resource pack.");
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [ERROR] {e.Message} {e.InnerException?.Message}");
                    continue;
                }

                // Liste de tous les fichiers xml présent dans le dossier "CommandDefinitions"
                string[] xmlFilesPaths = Directory.GetFiles(commandDefinitionsDirectoryPath, "*.xml");

                // Si aucun fichier xml de définition de commands est trouvé, on passe a la lecture du pack de ressources suivant
                if (xmlFilesPaths.Length == 0)
                {
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [ERROR] No command definition file found in the \"Commands\" folder of the \"{currentResourcePack.DirectoryName}\" resource pack.");
                    continue;
                }

                // Parcours de tous les fichiers xml présent dans "CommandDefinitions"
                foreach (string xmlFilePath in xmlFilesPaths)
                {
                    try
                    {
                        currentResourcePack.CommandsDictionary.Add(
                            Path.GetFileName(xmlFilePath), 
                            XmlSerializationHelper.Decode<List<CommandSet>>(xmlFilePath, new XmlRootAttribute("CommandDefinitions")));
                    }
                    catch (InvalidOperationException e)
                    {
                        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [ERROR] Error reading the \"Config.xml\" file from the \"{currentResourcePack.DirectoryName}\" resource pack.");
                        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [ERROR] {e.Message} {e.InnerException?.Message}");
                    }
                }

                if (currentResourcePack.CommandsDictionary.Count > 0)
                {
                    resourcePacks.Add(currentResourcePack);
                }
            }

            return resourcePacks;
        }


    }
}
