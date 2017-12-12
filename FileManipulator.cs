using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicMetadataOrganizer
{
    public static class FileManipulator
    {
        public static void RenameDirectory(string currentDirectory, string newDirectory)
        {
            var currentDir = Directory.Exists(currentDirectory) ? new DirectoryInfo(currentDirectory) : 
                throw new IOException($"Cannot rename/move the source directory {currentDirectory}. It does not exist.");
            char[] invalidPathChars = Path.GetInvalidPathChars();
            foreach (var character in invalidPathChars)
            {
                if (newDirectory.Contains(character))
                {
                    var errorMessage = $"Can not rename '{currentDirectory}' to '{newDirectory}'. " +
                        $"{newDirectory} contains an invalid character for a file name: {character}.";
                    var log = new LogWriter(errorMessage);
                    throw new ArgumentException(errorMessage);
                }
            }
            var newDir = new DirectoryInfo(newDirectory);
            try
            {
                if (currentDirectory.Equals(newDirectory, StringComparison.OrdinalIgnoreCase))
                {
                    var tempPath = newDirectory.Replace(newDir.Name, @"_temp\");
                    currentDir.MoveTo(tempPath);
                }
                currentDir.MoveTo(newDirectory);
            }
            catch (IOException ex)
            {
                var log = new LogWriter($"Can not rename (move) '{currentDirectory}' to '{newDirectory}'. {ex.GetType()}: \"{ex.Message}\"");
            }
            catch (ArgumentException ex)
            {
                var log = new LogWriter($"Can not rename (move) '{currentDirectory}' to '{newDirectory}'. {ex.GetType()}: \"{ex.Message}\"");
            }
            catch (Exception ex)
            {
                var log = new LogWriter($"Can not rename (move) '{currentDirectory}' to '{newDirectory}'. {ex.GetType()}: \"{ex.Message}\"");
            }
        }

        public static void RenameFile(string filepath, string newName)
        {
            var file = File.Exists(filepath) ? new FileInfo(filepath) : 
                throw new IOException($"Cannot rename/move the source file {filepath}. It does not exist.");
            var currentName = file.Name;
            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
            foreach (var character in invalidFileNameChars)
            {
                if (newName.Contains(character))
                {
                    var errorMessage = $"Can not rename '{currentName}' to '{newName}'. " +
                        $"{newName} contains an invalid character for a file name: {character}.";
                    var log = new LogWriter(errorMessage);
                    throw new ArgumentException(errorMessage);
                }
            }
            var newPath = filepath.Replace(currentName, newName);
            try
            {
                if (currentName.Equals(newName, StringComparison.OrdinalIgnoreCase))
                {
                    var tempPath = newPath.Replace(currentName, "_temp");
                    file.MoveTo(tempPath);
                }
                file.MoveTo(newPath);
            }
            catch (IOException ex)
            {
                var log = new LogWriter($"Can not rename (move) '{filepath}' to '{newPath}'. {ex.GetType()}: \"{ex.Message}\"");
            }
            catch (Exception ex)
            {
                var log = new LogWriter($"Can not rename (move) '{filepath}' to '{newPath}'. {ex.GetType()}: \"{ex.Message}\"");
            }
        }
    }
}
