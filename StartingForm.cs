using MusicMetadataOrganizer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace MetadataUpdaterGUI
{
    public partial class StartingForm : Form
    {
        List<MasterFile> files;
        public StartingForm()
        {
            InitializeComponent();
            files = new List<MasterFile>();
        }

        private void SelectFilesButton_Click(object sender, EventArgs e)
        {
            files.Clear();
            files = FileSearcher.ExtractFiles();
            displayFilesBox.Clear();
            for (int i = 0; i < files.Count; i++)
            {
                if (i == files.Count - 1)
                {
                    displayFilesBox.Text += files[i];
                }
                else
                {
                    displayFilesBox.Text += files[i] + Environment.NewLine;
                }
                displayFilesBox.Refresh();
            }
            // rearrange these when the form is ordered better
            checkMetadataButton.Enabled = true;
            saveButton.Enabled = false;
            blacklistedSongsButton.Enabled = true;
        }

        private void CheckMetadataButton_Click(object sender, EventArgs e)
        {
            displayFilesBox.Clear();
            displayFilesBox.Text = "Checking metadata, please wait..." + Environment.NewLine;
            displayFilesBox.Refresh();
            List<UpdateHelper> updates = ProcessMasterFileUpdates();
            if (updates.Count > 0)
            {
                var updater = new UpdaterForm(updates);
            }
            saveButton.Enabled = true;
        }

        private List<UpdateHelper> ProcessMasterFileUpdates()
        {
            var updates = new List<UpdateHelper>();
            foreach (MasterFile masterFile in files)
            {
                if ((bool)masterFile.TagLibProps["IsCover"] || masterFile.CheckForUpdates == false)
                    continue;
                GracenoteSong songFromAPI = masterFile.TagLibProps["Album"].ToString() == "Singles" ? 
                    GracenoteWebAPI.Query(masterFile, includeAlbumInQuery: false) : 
                    GracenoteWebAPI.Query(masterFile, includeAlbumInQuery: true);
                var results = songFromAPI.CheckMetadataEquality(masterFile);
                var propertiesToVerify = results.Where(pair => pair.Value == false)
                  .Where(pair => pair.Key == "Artist" || pair.Key == "Album" || pair.Key == "Title")
                  .Select(pair => pair.Key);

                if (propertiesToVerify.Count() > 0)
                {
                    displayFilesBox.Text += $"\"{masterFile}\" has new or different data. Updating... {Environment.NewLine}";
                    displayFilesBox.Refresh();
                    UpdateHelper update = CreateUpdateHelper(masterFile, songFromAPI, propertiesToVerify);
                    updates.Add(update);
                    masterFile.Update(songFromAPI, propertiesToVerify);
                }
                else
                {
                    displayFilesBox.Text += $"\"{masterFile}\" has no new or different data. Not updating. {Environment.NewLine}";
                }
            }
            return updates;
        }

        private static UpdateHelper CreateUpdateHelper(MasterFile masterFile, GracenoteSong songFromAPI, IEnumerable<string> propertiesToVerify)
        {
            var oldProps = new List<string>();
            var newProps = new List<string>();
            foreach (var property in propertiesToVerify)
            {
                oldProps.Add(masterFile.TagLibProps[property].ToString());
                switch (property)
                {
                    case "Artist":
                        newProps.Add(songFromAPI.Artist);
                        break;
                    case "Album":
                        newProps.Add(songFromAPI.Album);
                        break;
                    case "Title":
                        newProps.Add(songFromAPI.Title);
                        break;
                    default:
                        break;
                }
            }
            return new UpdateHelper(masterFile, propertiesToVerify.ToList(), oldProps, newProps);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            /*
            displayFilesBox.Clear();
            for (int i = 0; i < files.Count; i++)
            {
                files[i].Save();
                if (i == files.Count - 1)
                {
                    displayFilesBox.Text += $"\"{files[i]}\" has been saved.";
                }
                else
                {
                    displayFilesBox.Text += $"\"{files[i]}\" has been saved. {Environment.NewLine}";
                }
                displayFilesBox.Refresh();
            }
            saveButton.Enabled = false;
            */

            displayFilesBox.Clear();
            for (int i = 0; i < UpdaterForm.Updates.Count; i++)
            {
                //if (UpdaterForm.Updates[i] == null)
                //    continue;
                UpdaterForm.Updates[i].File.Save();
                if (i == UpdaterForm.Updates.Count - 1)
                    displayFilesBox.Text += $"\"{UpdaterForm.Updates[i].File}\" has been saved.";
                else
                    displayFilesBox.Text += $"\"{UpdaterForm.Updates[i].File}\" has been saved. {Environment.NewLine}";
                displayFilesBox.Refresh();
            }
            saveButton.Enabled = false;
        }

        private void BlacklistedSongsButton_Click(object sender, EventArgs e)
        {
            List<MasterFile> blacklistedSongs;
            try
            {
                blacklistedSongs = UpdaterForm.AllFiles.Where(mf => mf.CheckForUpdates == false).ToList();
                if (blacklistedSongs == null || blacklistedSongs.Count() == 0)
                    MessageBox.Show("There are no blacklisted songs.");
                var blacklistedSongsForm = new BlacklistedSongsForm(blacklistedSongs);

            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("There are no blacklisted songs.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message} + {ex.GetType().ToString()}.");
                throw ex;
            }
        }
    }
}
