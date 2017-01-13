using DiscordBotCaptainObvious.Cortana.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotCaptainObvious.Cortana.Helpers
{
    static class GetSentenceHelper
    {
        private static readonly object syncLock = new object();


        // Old
        private static IEnumerable<string> BuildListSentencesOld(SentencesEnum sentence)
        {
            string sentenceType = $"Say{sentence.ToString()}";
            string destinationSentenceFile = $"../../TextRecognitionModule/Resources/{sentenceType}.txt";

            return File.ReadAllText(destinationSentenceFile).Split('\n').ToList();
        }

        public static string SayOld(SentencesEnum sentence, int pos)
        {
            List<String> lstSentences = BuildListSentencesOld(sentence).ToList();
            return lstSentences[pos];
        }

        public static string SayRandomOld(SentencesEnum sentence)
        {
            List<String> lstSentences = BuildListSentencesOld(sentence).ToList();
            int nbelements = lstSentences.Count();
            int idSentence = 0;

            if (nbelements > 1)
            {
                Random random = new Random();

                lock (syncLock)
                    idSentence = random.Next(nbelements);
            }

            return lstSentences[idSentence];
        }

        // New
        private static IEnumerable<string> BuildListSentences(string sentencesFile)
        {
            string sentenceType = $"Say{sentencesFile}";
            string destinationSentenceFile = $"../../TextRecognitionModule/Resources/{sentenceType}.txt";

            return File.ReadAllText(destinationSentenceFile).Split('\n').ToList();
        }

        public static string Say(string sentencesFile, int pos)
        {
            List<String> lstSentences = BuildListSentences(sentencesFile).ToList();
            return lstSentences[pos];
        }

        public static string SayRandom(string sentencesFile)
        {
            List<String> lstSentences = BuildListSentences(sentencesFile).ToList();
            int nbelements = lstSentences.Count();
            int idSentence = 0;

            if (nbelements > 1)
            {
                Random random = new Random();

                lock (syncLock)
                    idSentence = random.Next(nbelements);
            }

            return lstSentences[idSentence];
        }
    }
}
