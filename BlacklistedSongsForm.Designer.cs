namespace MetadataUpdaterGUI
{
    partial class BlacklistedSongsForm
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
            this.blacklistedSongsListView = new System.Windows.Forms.ListView();
            this.instructionLabel = new System.Windows.Forms.Label();
            this.confirmButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // blacklistedSongsListView
            // 
            this.blacklistedSongsListView.Location = new System.Drawing.Point(43, 55);
            this.blacklistedSongsListView.Name = "blacklistedSongsListView";
            this.blacklistedSongsListView.Size = new System.Drawing.Size(520, 251);
            this.blacklistedSongsListView.TabIndex = 8;
            this.blacklistedSongsListView.UseCompatibleStateImageBehavior = false;
            // 
            // instructionLabel
            // 
            this.instructionLabel.AutoSize = true;
            this.instructionLabel.Location = new System.Drawing.Point(43, 36);
            this.instructionLabel.Name = "instructionLabel";
            this.instructionLabel.Size = new System.Drawing.Size(262, 13);
            this.instructionLabel.TabIndex = 9;
            this.instructionLabel.Text = "Uncheck any songs you do not wish to be blacklisted.";
            // 
            // confirmButton
            // 
            this.confirmButton.Location = new System.Drawing.Point(250, 312);
            this.confirmButton.Name = "confirmButton";
            this.confirmButton.Size = new System.Drawing.Size(75, 23);
            this.confirmButton.TabIndex = 10;
            this.confirmButton.Text = "Confirm";
            this.confirmButton.UseVisualStyleBackColor = true;
            this.confirmButton.Click += new System.EventHandler(this.ConfirmButton_Click);
            // 
            // BlacklistedSongsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(656, 361);
            this.Controls.Add(this.confirmButton);
            this.Controls.Add(this.instructionLabel);
            this.Controls.Add(this.blacklistedSongsListView);
            this.Name = "BlacklistedSongsForm";
            this.Text = "Blacklisted Songs";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView blacklistedSongsListView;
        private System.Windows.Forms.Label instructionLabel;
        private System.Windows.Forms.Button confirmButton;
    }
}