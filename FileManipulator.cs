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
        public static void RenameDirectory(MasterFile mf, string currentDirectory, string newDirectory)
        {
            var dir = new DirectoryInfo(currentDirectory);
            try
            {
                if (currentDirectory.Equals(newDirectory, StringComparison.OrdinalIgnoreCase))
                {
                    var tempPath = newDirectory.Replace(mf.TagLibProps["Album"].ToString(), @"_temp\");
                    dir.MoveTo(tempPath);
                }
                dir.MoveTo(newDirectory);
            }
            catch (IOException ex)
            {
                var log = new LogWriter($"Can not rename (move) '{mf.Filepath}' to '{newDirectory}'. IOException: \"{ex.Message}\"");
            }
            catch (Exception ex)
            {
                var log = new LogWriter($"Can not rename (move) '{mf.Filepath}' to '{newDirectory}'. \"{ex.Message}\"");
            }
        }

        public static void RenameFile(MasterFile mf, string currentFileName, string newFileName)
        {
            var file = new FileInfo(mf.Filepath);
            var newPath = mf.Filepath.Replace(currentFileName, newFileName);
            try
            {
                if (currentFileName.Equals(newFileName, StringComparison.OrdinalIgnoreCase))
                {
                    var tempPath = newPath.Replace(currentFileName, "_temp");
                    file.MoveTo(tempPath);
                }
                file.MoveTo(newPath);
            }
            catch (IOException ex)
            {
                var log = new LogWriter($"Can not rename (move) '{mf.Filepath}' to '{newPath}'. IOException: \"{ex.Message}\"");
            }
            catch (Exception ex)
            {
                var log = new LogWriter($"Can not rename (move) '{mf.Filepath}' to '{newPath}'. \"{ex.Message}\"");
            }
        }
    }
}
