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
        // Maybe make a better name that implies that this should be used if mult. files exist in currentDir
        public static void RenameDirectory(MasterFile file, string currentDirectory, string newDirectory)
        {
            var currentDir = Directory.Exists(currentDirectory) ? new DirectoryInfo(currentDirectory) :
               throw new IOException($"Cannot rename/move the source directory '{currentDirectory}'. It does not exist.");
            if (DirectoryContainsMultipleFiles(currentDirectory))
            {
                try
                {
                    if (currentDirectory.Equals(newDirectory, StringComparison.OrdinalIgnoreCase))
                    {
                        // Get this working
                        List<MasterFile> files = FileSearcher.ExtractFilesFromFolder(currentDirectory).ToList();
                        foreach (MasterFile mf in files)
                        {
                            mf.SysIOProps["Directory"] = newDirectory;
                            mf.Filepath = Path.Combine(newDirectory, mf.SysIOProps["Name"].ToString());
                        }
                    }
                    
                    if (!Directory.Exists(newDirectory))
                        Directory.CreateDirectory(newDirectory);
                    var newFilepath = file.Filepath.Replace(currentDirectory, newDirectory);
                    File.Move(file.Filepath, newFilepath);
                }
                catch (Exception ex)
                {
                    var log = new LogWriter($"FileManipulator.RenameDirectory - Can not rename (move) '{currentDirectory}' " +
                        $"to '{newDirectory}'. {ex.GetType()}: \"{ex.Message}\"");
                }
            }
            else
                RenameDirectory(currentDir, newDirectory);
        }

        private static void RenameDirectory(DirectoryInfo currentDir, string newDirectory)
        {
            try
            {
                if (currentDir.FullName.Equals(newDirectory, StringComparison.OrdinalIgnoreCase))
                {
                    var newDir = new DirectoryInfo(newDirectory);
                    var tempPath = newDirectory.Replace(newDir.Name, @"_temp\");
                    currentDir.MoveTo(tempPath);
                }
                currentDir.MoveTo(newDirectory);
            }
            catch (Exception ex)
            {
                var log = new LogWriter($"FileManipulator.RenameDirectory - Can not rename (move) '{currentDir.FullName}' " +
                    $"to '{newDirectory}'. {ex.GetType()}: \"{ex.Message}\"");
            }
        }

        public static void RenameFile(string filepath, string newFileName)
        {
            var file = File.Exists(filepath) ? new FileInfo(filepath) :
                throw new IOException($"Cannot rename/move the source file '{filepath}'. It does not exist.");
            var currentName = file.Name;
            var newPath = filepath.Replace(currentName, newFileName);
            try
            {
                if (currentName.Equals(newFileName, StringComparison.OrdinalIgnoreCase))
                {
                    var tempPath = newPath.Replace(currentName, "_temp");
                    file.MoveTo(tempPath);
                }
                file.MoveTo(newPath);
            }
            catch (Exception ex)
            {
                var log = new LogWriter($"FileManipulator.RenameFile() - Can not rename (move) '{filepath}' to '{newPath}'. " +
                    $"{ex.GetType()}: \"{ex.Message}\"");
            }
        }

        public static bool DirectoryContainsMultipleFiles(string currentDirectory)
        {
            // Repeating lines
            var currentDir = Directory.Exists(currentDirectory) ? new DirectoryInfo(currentDirectory) :
                throw new IOException($"Cannot rename/move the source directory '{currentDirectory}'. It does not exist.");

            return (currentDir.EnumerateFiles().Count() > 1) ? true : false;
        }
    }
}
