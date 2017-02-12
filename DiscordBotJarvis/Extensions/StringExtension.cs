using System.Globalization;
using System.Text;

namespace DiscordBotJarvis.Extensions
{
    public static class StringExtension
    {
        public static string AddWhiteSpaceAroundString(this string str) => " " + str + " ";

        public static string ReplaceSpecialsChar(this string str) => str.Replace("-", " ");

        public static string RemoveDiacritics(this string str)
        {
            string normalizedString = str.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string ProcessingUserRequest(this string str) 
            => str.RemoveDiacritics().ReplaceSpecialsChar().ToLower().Trim().AddWhiteSpaceAroundString();
    }
}
