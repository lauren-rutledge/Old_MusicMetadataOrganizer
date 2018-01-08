using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;

namespace MusicMetadataOrganizer
{
    public static class StringCleaner
    {
        // Experimenting
        static List<string> titleExceptions = new List<string>()
        {
            "a-ha",
        };

        public static string ToCleanTitleCase(string input)
        {
            if (titleExceptions.Contains(input))
                return input;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            string result = textInfo.ToTitleCase(input);
            result = LowercaseUnimportantWords(result);
            result = CapitalizeWordsAfterSymbols(result);
            result = LowercaseLetterAfterNumbers(result);
            result = RenameFeatInconsistencies(result);
            result = RenameFeatInconsistencies(result);
            return result;
        }

        private static string LowercaseUnimportantWords(string input)
        {
            string result = input;
            string lowercaseKeywords = @"\b(?<!^)(a(?:nd?)?|the|to|[io]n|from|with|of|at|by|for)(?!$)\b";
            Regex titleCaseRegex = new Regex(lowercaseKeywords, RegexOptions.IgnoreCase);
            if (titleCaseRegex.IsMatch(input))
            {
                MatchCollection matches = titleCaseRegex.Matches(input);
                result = matches.ReplaceWithLower(result);
            }
            return result;
        }

        private static string CapitalizeWordsAfterSymbols(string input)
        {
            string result = input;
            Regex wordsAfterSymbolsRegex = new Regex(@"[-|:] (\w+)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            if (wordsAfterSymbolsRegex.IsMatch(input))
            {
                MatchCollection matches = wordsAfterSymbolsRegex.Matches(input);
                for (int i = 0; i < matches.Count; i++)
                {
                    result = matches[i].Groups[1].ReplaceFirstWithUpper(result);
                }
            }

            Regex wordsAfterParenthesisRegex = new Regex(@"\((\w)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            if (wordsAfterParenthesisRegex.IsMatch(result))
            {
                MatchCollection matches = wordsAfterParenthesisRegex.Matches(result);
                for (int i = 0; i < matches.Count; i++)
                {
                    result = matches[i].Groups[1].ReplaceFirstWithUpper(result);
                }
            }
            return result;
        }

        private static string LowercaseLetterAfterNumbers(string input)
        {
            string result = input;
            Regex letterAfterNumbersRegex = new Regex(@"[0-9]+(\w)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            if (letterAfterNumbersRegex.IsMatch(input))
            {
                var matches = letterAfterNumbersRegex.Matches(input);
                result = matches.ReplaceWithLower(input);
            }
            return result;
        }

        private static string RenameFeatInconsistencies(string input)
        {
            Regex featuringAbbreviationsRegex = new Regex(@" (feat(uring|\.)?|ft) ", RegexOptions.IgnoreCase);
            return featuringAbbreviationsRegex.Replace(input, " ft. ");
        }

        public static string RemoveInvalidDirectoryChars(string directory)
        {
            if (directory.Contains(':'))
                directory = directory.Replace(":", " -");
            char[] invalidPathChars = Path.GetInvalidPathChars();
            foreach (var character in invalidPathChars)
            {
                if (directory.Contains(character))
                {
                    directory = directory.Replace(character.ToString(), string.Empty);
                }
            }
            return directory;
        }

        public static string RemoveInvalidFileNameCharacters(string fileName)
        {
            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
            foreach (char character in invalidFileNameChars)
            {
                if (fileName.Contains(character))
                {
                    if (character == ':' || character == '/')
                    {
                        fileName = fileName.Replace(character, '-');
                        continue;
                    }
                    else
                    {
                        fileName = fileName.Replace(character.ToString(), string.Empty);
                    }
                }
            }
            return fileName;
        }
        
        // More cleaning Regex
        // If there is a -, make sure there is a space before and after it
        // Ex, "Miseria Cantare - The Beginning" returns wrong from the API
    }
}
