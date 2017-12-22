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

namespace MetadataUpdater
{
    public partial class Updater : Form
    {
        internal Updater(List<UpdateHelper> updates)
        {
            InitializeComponent();
            this.Show();
            pendingChangesListView.Items.Clear();
            SetupPendingChangesListView();

            var listViewItems = new List<ListViewItem>();
            foreach (UpdateHelper update in updates)
            {
                for (int i = 0; i < update.PropsToChange.Count(); i++)
                {
                    ListViewItem updateInfo = new ListViewItem(update.File.ToString());
                    updateInfo.SubItems.Add(update.PropsToChange[i]);
                    updateInfo.SubItems.Add(update.OldVals[i]);
                    updateInfo.SubItems.Add(update.NewVals[i]);
                    updateInfo.ToolTipText = update.File.Filepath;
                    listViewItems.Add(updateInfo);
                }
            }
            pendingChangesListView.Items.AddRange(listViewItems.ToArray());
            pendingChangesListView.Refresh();
        }

        private void CheckAllCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < pendingChangesListView.Items.Count; i++)
            {
                if (checkAllCheckBox.Checked)
                    pendingChangesListView.Items[i].Checked = true;
                else
                    pendingChangesListView.Items[i].Checked = false;
            }
            pendingChangesListView.Refresh();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SetupPendingChangesListView()
        {
            pendingChangesListView.View = View.Details;

            pendingChangesListView.ShowItemToolTips = true;

            // Get rid of this? - Allows the user to edit item text.
            pendingChangesListView.LabelEdit = true;
            pendingChangesListView.AllowColumnReorder = true;
            pendingChangesListView.CheckBoxes = true;
            pendingChangesListView.FullRowSelect = true;
            pendingChangesListView.GridLines = true;
            // pendingChangesListView.Sorting = SortOrder.Ascending;

            // Create columns for the items and subitems.
            // Width of -2 indicates auto-size.
            pendingChangesListView.Columns.Add("File to change", 310, HorizontalAlignment.Left);
            pendingChangesListView.Columns.Add("Metadata that is being changed", 60, HorizontalAlignment.Left);
            pendingChangesListView.Columns.Add("Old value", 160, HorizontalAlignment.Left);
            pendingChangesListView.Columns.Add("New value", 160, HorizontalAlignment.Left);
        }

        private void PendingChangesListView_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {

            //MessageBox.Show(e.Item.Text);
        }
    }
}
