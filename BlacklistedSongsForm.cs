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
    public partial class BlacklistedSongsForm : Form
    {
        private BlacklistedSongsForm()
        {
            InitializeComponent();
        }

        public BlacklistedSongsForm(List<MasterFile> blacklistedFiles)
        {
            InitializeComponent();
            this.Show();
            SetupBlacklistedSongsListView();
            var blacklistedSongs = new List<ListViewItem>();

            foreach (MasterFile file in blacklistedFiles)
            {
                ListViewItem blacklistedSong = new ListViewItem(file.ToString());
                blacklistedSong.SubItems.Add(file.TagLibProps["Artist"].ToString());
                blacklistedSong.SubItems.Add(file.TagLibProps["Title"].ToString());
                blacklistedSong.SubItems.Add(file.TagLibProps["Album"].ToString());
                blacklistedSong.ToolTipText = file.Filepath;
                blacklistedSongs.Add(blacklistedSong);
            }
            blacklistedSongsListView.Items.AddRange(blacklistedSongs.ToArray());
            blacklistedSongsListView.Refresh();
        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            
        }

        private void SetupBlacklistedSongsListView()
        {
            blacklistedSongsListView.View = View.Details;

            blacklistedSongsListView.ShowItemToolTips = true;
            blacklistedSongsListView.AllowColumnReorder = true;
            blacklistedSongsListView.CheckBoxes = true;
            blacklistedSongsListView.FullRowSelect = true;
            blacklistedSongsListView.GridLines = true;

            // Create columns for the items and subitems.
            blacklistedSongsListView.Columns.Add("File", 310, HorizontalAlignment.Left);
            blacklistedSongsListView.Columns.Add("Artist", 60, HorizontalAlignment.Left);
            blacklistedSongsListView.Columns.Add("Title", 160, HorizontalAlignment.Left);
            blacklistedSongsListView.Columns.Add("Album", 160, HorizontalAlignment.Left);
        }
    }
}
