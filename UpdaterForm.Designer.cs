namespace MetadataUpdaterGUI
{
    partial class UpdaterForm
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
            this.confirmChangesButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.checkAllCheckBox = new System.Windows.Forms.CheckBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.pendingChangesListView = new System.Windows.Forms.ListView();
            this.testLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // confirmChangesButton
            // 
            this.confirmChangesButton.Location = new System.Drawing.Point(25, 434);
            this.confirmChangesButton.Name = "confirmChangesButton";
            this.confirmChangesButton.Size = new System.Drawing.Size(346, 23);
            this.confirmChangesButton.TabIndex = 2;
            this.confirmChangesButton.Text = "Confirm";
            this.confirmChangesButton.UseVisualStyleBackColor = true;
            this.confirmChangesButton.Click += new System.EventHandler(this.ConfirmChangesButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(186, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(338, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Uncheck any changes you do not wish to save.";
            // 
            // checkAllCheckBox
            // 
            this.checkAllCheckBox.AutoSize = true;
            this.checkAllCheckBox.Location = new System.Drawing.Point(25, 411);
            this.checkAllCheckBox.Name = "checkAllCheckBox";
            this.checkAllCheckBox.Size = new System.Drawing.Size(69, 17);
            this.checkAllCheckBox.TabIndex = 4;
            this.checkAllCheckBox.Text = "Select all";
            this.checkAllCheckBox.UseVisualStyleBackColor = true;
            this.checkAllCheckBox.CheckedChanged += new System.EventHandler(this.CheckAllCheckBox_CheckedChanged);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(380, 434);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(360, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // pendingChangesListView
            // 
            this.pendingChangesListView.Location = new System.Drawing.Point(25, 86);
            this.pendingChangesListView.Name = "pendingChangesListView";
            this.pendingChangesListView.Size = new System.Drawing.Size(715, 319);
            this.pendingChangesListView.TabIndex = 7;
            this.pendingChangesListView.UseCompatibleStateImageBehavior = false;
            // 
            // testLabel
            // 
            this.testLabel.AutoSize = true;
            this.testLabel.Location = new System.Drawing.Point(266, 67);
            this.testLabel.Name = "testLabel";
            this.testLabel.Size = new System.Drawing.Size(35, 13);
            this.testLabel.TabIndex = 8;
            this.testLabel.Text = "label2";
            // 
            // UpdaterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 469);
            this.Controls.Add(this.testLabel);
            this.Controls.Add(this.pendingChangesListView);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.checkAllCheckBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.confirmChangesButton);
            this.MaximizeBox = false;
            this.Name = "UpdaterForm";
            this.Text = "Verify Changes";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button confirmChangesButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkAllCheckBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ListView pendingChangesListView;
        private System.Windows.Forms.Label testLabel;
    }
}