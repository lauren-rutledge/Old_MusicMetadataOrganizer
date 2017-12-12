using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;

namespace MusicMetadataOrganizer
{
    public static class StringCleaner
    {
        private static string lowercaseKeywords = @"\b(?<!^)(a(?:nd?)?|the|to|[io]n|from|with|of|at|by|for)(?!$)\b";
        private static Regex regex = new Regex(lowercaseKeywords, RegexOptions.IgnoreCase);

        public static string ToActualTitleCase(string input)
        {
            var matches = regex.Matches(input);
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            var allFirstLettersUppercase = textInfo.ToTitleCase(input);
            return matches.ReplaceWithLower(allFirstLettersUppercase);
        }
    }
}
