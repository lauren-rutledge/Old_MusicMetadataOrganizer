using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MusicMetadataOrganizer
{
    public static class StringCleaner
    {
        private static string lowercaseKeywords = @"\b(?<!^)(a(?:nd?)?|the|to|[io]n|from|with|of)(?!$)\b";
        private static Regex regex = new Regex(lowercaseKeywords, RegexOptions.IgnoreCase);

        public static string ToActualTitleCase(string input)
        {
            var matches = regex.Matches(input);
            return matches.ReplaceWithLower(input);
        }
    }
}
