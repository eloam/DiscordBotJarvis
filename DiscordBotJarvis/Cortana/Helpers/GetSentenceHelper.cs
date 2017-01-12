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

        private static IEnumerable<string> BuildListSentences(SentencesEnum sentence)
        {
            string sentenceType = $"Say{sentence.ToString()}";
            string destinationSentenceFile = $"../../Cortana/Resources/{sentenceType}.txt";

            return File.ReadAllText(destinationSentenceFile).Split('\n').ToList();
        }

        public static string Say(SentencesEnum sentence, int pos)
        {
            List<String> lstSentences = BuildListSentences(sentence).ToList();
            return lstSentences[pos];
        }

        public static string SayRandom(SentencesEnum sentence)
        {
            List<String> lstSentences = BuildListSentences(sentence).ToList();
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
