using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DiscordBotJarvis.Helpers
{
    public static class GetSentenceHelper
    {
        private static readonly object SyncLock = new object();

        private static IEnumerable<string> BuildListSentences(string filePath) => File.ReadAllLines(filePath).ToList();

        public static string ReadFile(string filePath) => File.ReadAllText(filePath);

        public static string ReadLineSpecified(string filePath, int line)
        {
            int index = line - 1;

            List<string> lstSentences = BuildListSentences(filePath).ToList();

            return index < lstSentences.Count ? lstSentences[index] : ReadLineRandom(filePath);
        }

        public static string ReadLineRandom(string filePath)
        {
            List<string> lstSentences = BuildListSentences(filePath).ToList();
            int idSentence = 0;

            if (lstSentences?.Count > 1)
            {
                Random random = new Random();

                lock (SyncLock)
                    idSentence = random.Next(lstSentences.Count);
            }

            return lstSentences[idSentence];
        }

    }
}
