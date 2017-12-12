using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MusicMetadataOrganizer
{
    internal static class RegexExtensions
    {
        internal static string ReplaceWithLower(this MatchCollection matches, string source)
        {
            foreach (var match in matches.Cast<Match>())
            {
                source = match.ReplaceWithLower(source);
            }
            return source;
        }

        internal static string ReplaceWithLower(this Match match, string source)
        {
            return source.Substring(0, match.Index) + match.Value.ToLower() + source.Substring(match.Index + match.Length);
        }

        internal static string Replace(this Match match, string source, string replacement)
        {
            return source.Substring(0, match.Index) + replacement + source.Substring(match.Index + match.Length);
        }

        internal static string Replace(this Group match, string source, string replacement)
        {
            return source.Substring(0, match.Index) + replacement + source.Substring(match.Index + match.Length);
        }
    }
}
