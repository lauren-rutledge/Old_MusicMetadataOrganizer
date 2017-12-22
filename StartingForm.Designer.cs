namespace MetadataUpdaterGUI
{
    partial class StartingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.selectFilesButton = new System.Windows.Forms.Button();
            this.checkMetadataButton = new System.Windows.Forms.Button();
            this.displayFilesBox = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // selectFilesButton
            // 
            this.selectFilesButton.Location = new System.Drawing.Point(282, 398);
            this.selectFilesButton.Name = "selectFilesButton";
            this.selectFilesButton.Size = new System.Drawing.Size(128, 23);
            this.selectFilesButton.TabIndex = 1;
            this.selectFilesButton.Text = "Select files";
            this.selectFilesButton.UseVisualStyleBackColor = true;
            this.selectFilesButton.Click += new System.EventHandler(this.SelectFilesButton_Click);
            // 
            // checkMetadataButton
            // 
            this.checkMetadataButton.Enabled = false;
            this.checkMetadataButton.Location = new System.Drawing.Point(282, 427);
            this.checkMetadataButton.Name = "checkMetadataButton";
            this.checkMetadataButton.Size = new System.Drawing.Size(128, 23);
            this.checkMetadataButton.TabIndex = 2;
            this.checkMetadataButton.Text = "Check metadata";
            this.checkMetadataButton.UseVisualStyleBackColor = true;
            this.checkMetadataButton.Click += new System.EventHandler(this.CheckMetadataButton_Click);
            // 
            // displayFilesBox
            // 
            this.displayFilesBox.Location = new System.Drawing.Point(24, 12);
            this.displayFilesBox.Multiline = true;
            this.displayFilesBox.Name = "displayFilesBox";
            this.displayFilesBox.Size = new System.Drawing.Size(644, 363);
            this.displayFilesBox.TabIndex = 3;
            this.displayFilesBox.Text = "No files selected.";
            // 
            // saveButton
            // 
            this.saveButton.Enabled = false;
            this.saveButton.Location = new System.Drawing.Point(282, 456);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(128, 23);
            this.saveButton.TabIndex = 4;
            this.saveButton.Text = "Save new data";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // StartingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 508);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.displayFilesBox);
            this.Controls.Add(this.checkMetadataButton);
            this.Controls.Add(this.selectFilesButton);
            this.MaximizeBox = false;
            this.Name = "StartingForm";
            this.Text = "Music Metadata Organizer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button selectFilesButton;
        private System.Windows.Forms.Button checkMetadataButton;
        private System.Windows.Forms.TextBox displayFilesBox;
        private System.Windows.Forms.Button saveButton;
    }
}

