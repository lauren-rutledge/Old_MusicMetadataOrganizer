using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MusicMetadataOrganizer
{
    public static class FileManipulator
    {
        public static void RenameFolder(MasterFile file, string currentDirectory, string newDirectory)
        {
            DirectoryInfo currentDir = Directory.Exists(currentDirectory) ? new DirectoryInfo(currentDirectory) :
                throw new IOException($"Cannot rename/move the source directory '{currentDirectory}'. It does not exist.");
            try
            {
                if (!Directory.Exists(newDirectory))
                {
                    Directory.CreateDirectory(newDirectory);
                }
                if (currentDirectory.Equals(newDirectory, StringComparison.OrdinalIgnoreCase))
                {
                    var tempPath = newDirectory.Replace(newDirectory, @"_temp\");
                    // If this ends up throwing an error, create the temp path first and use Directory.Move(); 
                    // Make sure it gets deleted after the move
                    currentDir.MoveTo(tempPath);
                    file.Filepath = Path.Combine(tempPath, file.SysIOProps["Name"].ToString());
                }
                var newFilepath = file.Filepath.Replace(currentDirectory, newDirectory);
                File.Move(file.Filepath, newFilepath);
                file.Filepath = newFilepath;
                file.SysIOProps["Directory"] = newDirectory;
            }
            catch (IOException ex)
            {
                var log = new LogWriter($"FileManipulator.RenameFolder - Can not rename (move) '{currentDirectory}' " +
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

        public static void DeleteEmptyFolders(DirectoryInfo folder)
        {
            // Checks to see if the current folder has any files
            List<FileInfo> files = folder.EnumerateFiles().Where(f => f.Extension != ".db").ToList();
            if (files.Count() > 0)
                return;
            // Checks to see if the parent folder has any files
            if (folder.Parent.EnumerateFiles().Count() > 0)
            {
                folder.Delete();
                return;
            }

            bool deleteParentFolder = true;

            // Iterates over the parent folder's subdirectories to see if any of them have files
            foreach (DirectoryInfo directory in folder.Parent.EnumerateDirectories())
            {
                List<FileInfo> directoryFiles = directory.EnumerateFiles().Where(f => f.Extension != ".db").ToList();
                if (directoryFiles.Count() > 0)
                    deleteParentFolder = false;
            }
            if (deleteParentFolder)
            {
                try
                {
                    folder.Parent.Delete(true);
                }
                catch (IOException ex)
                {
                    var log = new LogWriter($"FileManipulator.DeleteEmptyParentFolder() - Can not delete '{folder.Parent.FullName}'. " +
                                       $"{ex.GetType()}: \"{ex.Message}\"");
                }
            }
        }
    }
}
