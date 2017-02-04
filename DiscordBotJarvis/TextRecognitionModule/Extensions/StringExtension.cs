using System.Globalization;
using System.Text;

namespace DiscordBotJarvis.TextRecognitionModule.Extensions
{
    public static class StringExtension
    {
        public static string AddWhiteSpaceAroundString(this string str)
        {
            return " " + str + " ";
        }

        public static string ReplaceSpecialsChar(this string str)
        {
            return str.Replace("-", "");
        }

        public static string RemoveDiacritics(this string str)
        {
            var normalizedString = str.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
