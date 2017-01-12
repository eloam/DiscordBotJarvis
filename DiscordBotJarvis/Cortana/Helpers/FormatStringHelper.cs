using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotCaptainObvious.Cortana.Helpers
{
    static class FormatStringHelper
    {
        public static string NormalizeQuery(string request)
        {
            string normalizedString = request.Trim().ToLower();
            normalizedString = RemoveDiacritics(normalizedString);

            return normalizedString;
        }

        public static string RemoveDiacritics(string request)
        {
            var normalizedString = request.Normalize(NormalizationForm.FormD);
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
