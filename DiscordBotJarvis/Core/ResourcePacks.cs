using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using DiscordBotJarvis.Helpers;
using DiscordBotJarvis.Models.ResourcePacks;
using DiscordBotJarvis.Models.ResourcePacks.CommandDefinitions;
using DiscordBotJarvis.Models.ResourcePacks.ConfigFile;
using System.Xml.Serialization;

namespace DiscordBotJarvis.Core
{
    public static class ResourcePacks
    {
        /// <summary>
        /// Charger tous les ressources packs définies dans un répertoire
        /// </summary>
        public static List<ResourcePack> LoadAll(string currentCulture)
        {
            // Vérification si le paramètre n'est pas 'null'
            if (currentCulture == null) throw new ArgumentNullException(nameof(currentCulture));

            // Déclaration de la variable destiné à contenir l'ensemble des commandes des différents packs de ressources
            List<ResourcePack> resourcePacks = new List<ResourcePack>();

            // Lecture du dossier ResourcesPacks, s'il n'existe pas on évalue pas le reste de la fonction
            if (!Directory.Exists(EndPoints.Directory.ResourcePacks)) return resourcePacks;

            // Liste de tous les répertoires ayant le fichier Config.Xml à la racine du sous-dossier
            string[] resourcesPacksPaths = Directory.GetDirectories(EndPoints.Directory.ResourcePacks);

            // Lecture de tous les répertoires (ResourcePacks)
            foreach (string resourcePackPath in resourcesPacksPaths)
            {
                ResourcePack currentResourcePack = Load(resourcePackPath, currentCulture);
                
                if (currentResourcePack?.Commands != null)
                    resourcePacks.Add(currentResourcePack);           
            }

            return resourcePacks;
        }

        private static ResourcePack Load(string resourcePackPath, string currentCulture)
        {
            // Vérification si les paramètres ne sont pas 'null'
            if (resourcePackPath == null) throw new ArgumentNullException(nameof(resourcePackPath));
            if (currentCulture == null) throw new ArgumentNullException(nameof(currentCulture));

            ResourcePack currentResourcePack = new ResourcePack
            {
                // Obtenir le nom du dossier contenant le pack de ressources
                DirectoryName = new DirectoryInfo(resourcePackPath).Name
            };

            // Emplacement du fichier "Config.xml"
            string configFilePath = string.Format(EndPoints.Path.ResourcePacksConfigFile, currentResourcePack.DirectoryName);

            // On vérifie si le pack de ressources possède à sa racine un fichier "Config.xml"
            if (!File.Exists(configFilePath)) return null;

            // On vérifie qu'il existe les sous-répertoies suivants : "CommandDefinitions", "Resources" et "Services"
            if (!ResourcePacksHelper.AllSubdirectoriesResourcePackExists(currentResourcePack.DirectoryName, currentCulture)) return null;

            // Lecture du fichier "Config.xml" du ressource pack
            try
            {
                currentResourcePack.Config = XmlSerializerHelper.Decode<ResourcePackConfig>(configFilePath);
            }
            catch (Exception e)
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [ERROR] Error reading the \"Config.xml\" file from the \"{currentResourcePack.DirectoryName}\" resource pack.");
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [ERROR] {e.Message} {e.InnerException?.Message}");
                return null;
            }

            // Liste de tous les fichiers xml présent dans le dossier "CommandDefinitions"
            string commandsSubdirectoryPath = string.Format(EndPoints.Directory.ResourcePacksCommands, currentResourcePack.DirectoryName, currentCulture);
            string[] xmlFilesPaths = Directory.GetFiles(commandsSubdirectoryPath, "*.xml");

            // Si aucun fichier xml de définition de commands est trouvé, on passe a la lecture du pack de ressources suivant
            if (xmlFilesPaths.Length == 0)
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [ERROR] No command definition file found in the \"Commands\" folder of the \"{currentResourcePack.DirectoryName}\" resource pack.");
                return null;
            }

            // Parcours de tous les fichiers xml présent dans "CommandDefinitions"
            foreach (string xmlFilePath in xmlFilesPaths)
            {
                try
                {
                   // XmlSchemaSet schemaSet = new XmlSchemaSet();
                    //schemaSet.Add("test", @"D:\Documents\Visual Studio 2015\Projects\DiscordBotJarvis\DiscordBotJarvis\ResourcePacks\Exemple_fr-FR\Commands\fr-FR\Full.xsd");
                    IEnumerable<CommandSet> commands = XmlSerializerHelper.Decode<List<CommandSet>>(xmlFilePath, new XmlRootAttribute("CommandDefinitions"));

                    currentResourcePack.Commands.Add(Path.GetFileName(xmlFilePath), commands);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [ERROR] Error reading the \"{new DirectoryInfo(xmlFilePath).Name}\" file from the \"{currentResourcePack.DirectoryName}\" resource pack.");
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [ERROR] {e.Message} {e.InnerException?.Message}");
                }
            }

            if (currentResourcePack.Commands.Count <= 0) return null;
            
            // On charge le resource pack (valide)
            return currentResourcePack;
        }
    }
}
