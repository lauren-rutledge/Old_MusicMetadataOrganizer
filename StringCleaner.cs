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
            "37mm",
            "21st Century", // s in 21st was capitalized
            "Old Soul Song (For", // f in For was lowercased
        };

        private static string lowercaseKeywords = @"\b(?<!^)(a(?:nd?)?|the|to|[io]n|from|with|of|at|by|for)(?!$)\b";
        private static Regex regex = new Regex(lowercaseKeywords, RegexOptions.IgnoreCase);

        public static string ToActualTitleCase(string input)
        {
            var matches = regex.Matches(input);
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            var allFirstLettersUppercase = textInfo.ToTitleCase(input);
            return matches.ReplaceWithLower(allFirstLettersUppercase);
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
                    // Probably will remove this error logging once all this code is clean and functional
                    var errorMessage = $"FileManipulator.RenameDirectory - '{directory}' contains an invalid character for a " +
                            $"directory name: '{character}'. Invalid character removed.";
                    var log = new LogWriter(errorMessage);
                }
            }
            return directory;
        }

        public static string RemoveInvalidFileNameCharacters(string fileName)
        {
            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
            foreach (var character in invalidFileNameChars)
            {
                if (fileName.Contains(character))
                {
                    if (character == ':')
                    {
                        fileName = fileName.Replace(':', '-');
                        continue;
                    }
                    else
                    {
                        var errorMessage = $"FileManipulator.RenameFile() - '{fileName}' contains an invalid character for a file name: " +
                            $"'{character}'. Invalid character removed.";
                        var log = new LogWriter(errorMessage);
                        fileName = fileName.Replace(character.ToString(), string.Empty);
                    }
                }
            }
            return fileName;
        }
    }
}
