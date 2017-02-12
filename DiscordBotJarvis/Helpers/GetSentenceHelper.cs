using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DiscordBotJarvis.Helpers
{
    internal static class GetSentenceHelper
    {
        private static readonly object SyncLock = new object();

        private static IEnumerable<string> BuildListSentences(string filename)
        {
            string destinationSentenceFile = $"ResourcePacks/fr-FR/{filename}.txt";

            return File.ReadAllLines(destinationSentenceFile).ToList();
        }

        public static string Say(string filename, int pos)
        {
            List<String> lstSentences = BuildListSentences(filename).ToList();
            return lstSentences[pos];
        }

        public static string SayRandom(string filename)
        {
            List<string> lstSentences = BuildListSentences(filename).ToList();
            int nbelements = lstSentences.Count;
            int idSentence = 0;

            if (nbelements > 1)
            {
                Random random = new Random();

                lock (SyncLock)
                    idSentence = random.Next(nbelements);
            }

            return lstSentences[idSentence];
        }

        public static string ReadFile(string filename)
        {
            return File.ReadAllText($"../../TextRecognitionModule/Resources/{filename}.txt");
        }
    }
}
