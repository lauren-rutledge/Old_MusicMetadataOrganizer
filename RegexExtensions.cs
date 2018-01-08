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

        internal static string ReplaceFirstWithUpper(this Group group, string source)
        {
            string original = group.Value;
            string replacement = original[0].ToString().ToUpper() + original.Substring(1);
            return group.Replace(source, replacement);
        }

        internal static string Replace(this Match match, string source, string replacement)
        {
            return source.Substring(0, match.Index) + replacement + source.Substring(match.Index + match.Length);
        }

        internal static string Replace(this Group group, string source, string replacement)
        {
            return source.Substring(0, group.Index) + replacement + source.Substring(group.Index + group.Length);
        }
    }
}
