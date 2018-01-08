using MetadataUpdaterGUI;
using MusicMetadataOrganizer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            checkFileLocationButton.Enabled = true;
        }

        private void CheckMetadataButton_Click(object sender, EventArgs e)
        {
            var updates = new List<UpdateHelper>();
            displayFilesBox.Clear();
            displayFilesBox.Text = "Checking metadata, please wait..." + Environment.NewLine;
            displayFilesBox.Refresh();
            foreach (MasterFile masterFile in files)
            {
                if ((bool)masterFile.TagLibProps["IsCover"])
                    continue;
                // if (file.CheckForUpdates == false)
                //    continue;
                var songFromAPI = GracenoteWebAPI.Query(masterFile);
                var results = songFromAPI.CheckMetadataEquality(masterFile);
                // Do this part in the mf.update method
                var propertiesToVerify = results.Where(pair => pair.Value == false)
                  .Where(pair => pair.Key == "Artist" || pair.Key == "Album" || pair.Key == "Title")
                  .Select(pair => pair.Key);
                if (propertiesToVerify.Count() > 0)
                {
                    displayFilesBox.Text += $"\"{masterFile}\" has new or different data. Updating... {Environment.NewLine}";
                    displayFilesBox.Refresh();
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
                    var update = new UpdateHelper(masterFile, propertiesToVerify.ToList(), oldProps, newProps);
                    updates.Add(update);
                    masterFile.Update(songFromAPI, propertiesToVerify);
                }
                else
                {
                    displayFilesBox.Text += $"\"{masterFile}\" has no new or different data. Not updating. {Environment.NewLine}";
                }
            }
            if (updates.Count > 0)
            {
                var updater = new UpdaterForm(updates);
            }
            saveButton.Enabled = true;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
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

        private void CheckFileLocationButton_Click(object sender, EventArgs e)
        {
            displayFilesBox.Clear();
            foreach (MasterFile masterFile in files)
            {
                masterFile.Save();
                displayFilesBox.Text += $"{masterFile} has been moved to {masterFile.Filepath}. {Environment.NewLine}";
            }
        }
    }
}
