using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MusicMetadataOrganizer
{
    public static class FileSearcher
    {
        private static string _directory;
        private static List<MasterFile> files = new List<MasterFile>();

        [STAThread]
        public static List<MasterFile> ExtractFiles()
        {
            SelectDirectory();
            ExtractFiles(_directory);
            return files;
        }

        internal static IEnumerable<MasterFile> ExtractFilesFromFolder(string directory)
        {
            foreach (var path in Directory.EnumerateFiles(directory))
            {
                if (IsMediaFile(path))
                    yield return MasterFile.GetMasterFileFromFilepath(path);
            }
        }

        private static void SelectDirectory()
        {
            var folderBrowser = new FolderBrowserDialog
            {
                SelectedPath = @"Z:\Music"
            };
            if (folderBrowser.ShowDialog() == DialogResult.OK)
                _directory = folderBrowser.SelectedPath;
            else
            {
                Environment.Exit(1);
                Application.Exit();
            }
        }

        private static void ExtractFiles(string directory)
        {
            var filesInFolder = Directory.EnumerateFiles(directory, "", SearchOption.AllDirectories);

            foreach (var path in Directory.EnumerateFiles(directory))
            {
                if (IsMediaFile(path))
                    files.Add(MasterFile.GetMasterFileFromFilepath(path));
            }

            foreach (var subdirectory in Directory.EnumerateDirectories(directory))
            {
                ExtractFiles(subdirectory);
            }
        }

        private static string[] mediaExtensions =
        {
            ".AAC", ".AIFF", ".APE", ".ASF", ".AA", ".AAX", ".FLAC", ".MKA", ".M4A", ".MP3",
            ".MPC", ".OGG", ".RIFF", ".WV"
            // Took out .MPG, .MPEG, .MKV, .MP4, & .AVI
        };

        private static bool IsMediaFile(string path)
        {
            return mediaExtensions.Contains(Path.GetExtension(path), StringComparer.OrdinalIgnoreCase);
        }
    }
}