﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicMetadataOrganizer
{
    // Maybe make static? 
    public class FileSearcher
    {
        private string _directory;
        public string Directory
        {
            get
            {
                return _directory;
            }
        }

        private List<MasterFile> files = new List<MasterFile>();

        [STAThread]
        public List<MasterFile> ExtractFiles()
        {
            var spinner = new ConsoleSpinner();
            SelectDirectory();
            spinner.Start();
            ExtractFiles(Directory);
            spinner.Stop();
            return files;
        }

        private void SelectDirectory()
        {
            var folderBrowser = new FolderBrowserDialog
            {
                RootFolder = Environment.SpecialFolder.MyComputer
            };
            folderBrowser.SelectedPath = @"Z:\Music";
            if (folderBrowser.ShowDialog() == DialogResult.OK)
                _directory = folderBrowser.SelectedPath;
            else
                Environment.Exit(1);
        }

        private void ExtractFiles(string directory)
        {
            var filesInFolder = System.IO.Directory.GetFiles(directory, "", SearchOption.AllDirectories);

            foreach (var path in System.IO.Directory.GetFiles(directory))
            {
                if (IsMediaFile(path.ToString()))
                    files.Add(MasterFile.GetMasterFileFromFilepath(path));
            }

            foreach (var subdirectory in System.IO.Directory.EnumerateDirectories(directory))
            {
                ExtractFiles(subdirectory);  
            }
        }

        private static string[] mediaExtensions = 
        {
            ".AAC", ".AIFF", ".APE", ".ASF", ".AA", ".AAX", ".FLAC", ".MKA", ".M4A", ".MP3",
            ".MPC", ".OGG", ".RIFF", ".WV", ".MKV", ".MP4"
            // Took out .MPG, .MPEG, & .AVI
        };

        private static bool IsMediaFile(string path)
        {
            return mediaExtensions.Contains(Path.GetExtension(path), StringComparer.OrdinalIgnoreCase);
        }
    }
}
