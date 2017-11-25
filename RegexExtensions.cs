using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MusicMetadataOrganizer
{
    public static class RegexExtensions
    {
        public static string Replace(this MatchCollection matches, string source, string replacement)
        {
            foreach (var match in matches.Cast<Match>())
            {
                source = match.Replace(source, replacement);
            }
            return source;
        }
        public static string Replace(this Match match, string source, string replacement)
        {
            return source.Substring(0, match.Index) + match.Value.ToLower() + source.Substring(match.Index + match.Length);
        }
    }
}
