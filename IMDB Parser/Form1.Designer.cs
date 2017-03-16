namespace IMDB_Parser
{
    partial class Form1
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
            this.OpenButton = new System.Windows.Forms.Button();
            this.OpenText = new System.Windows.Forms.TextBox();
            this.SaveText = new System.Windows.Forms.TextBox();
            this.SaveButton = new System.Windows.Forms.Button();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.DropDownList = new System.Windows.Forms.ComboBox();
            this.ParseNowButton = new System.Windows.Forms.Button();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.SuspendLayout();
            // 
            // OpenButton
            // 
            this.OpenButton.Location = new System.Drawing.Point(350, 4);
            this.OpenButton.Name = "OpenButton";
            this.OpenButton.Size = new System.Drawing.Size(75, 22);
            this.OpenButton.TabIndex = 0;
            this.OpenButton.Text = "Open";
            this.OpenButton.UseVisualStyleBackColor = true;
            this.OpenButton.Click += new System.EventHandler(this.OpenButton_Click);
            // 
            // OpenText
            // 
            this.OpenText.Location = new System.Drawing.Point(12, 5);
            this.OpenText.Name = "OpenText";
            this.OpenText.Size = new System.Drawing.Size(332, 20);
            this.OpenText.TabIndex = 1;
            this.OpenText.Leave += new System.EventHandler(this.OpenText_Leave);
            // 
            // SaveText
            // 
            this.SaveText.Location = new System.Drawing.Point(12, 31);
            this.SaveText.Name = "SaveText";
            this.SaveText.Size = new System.Drawing.Size(332, 20);
            this.SaveText.TabIndex = 3;
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(350, 30);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 22);
            this.SaveButton.TabIndex = 2;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // ProgressBar
            // 
            this.ProgressBar.Location = new System.Drawing.Point(0, 85);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ProgressBar.Size = new System.Drawing.Size(438, 20);
            this.ProgressBar.Step = 1;
            this.ProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.ProgressBar.TabIndex = 4;
            this.ProgressBar.Visible = false;
            // 
            // DropDownList
            // 
            this.DropDownList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DropDownList.FormattingEnabled = true;
            this.DropDownList.Location = new System.Drawing.Point(12, 57);
            this.DropDownList.Name = "DropDownList";
            this.DropDownList.Size = new System.Drawing.Size(332, 21);
            this.DropDownList.TabIndex = 5;
            // 
            // ParseNowButton
            // 
            this.ParseNowButton.Location = new System.Drawing.Point(350, 56);
            this.ParseNowButton.Name = "ParseNowButton";
            this.ParseNowButton.Size = new System.Drawing.Size(75, 23);
            this.ParseNowButton.TabIndex = 6;
            this.ParseNowButton.Text = "Parse Now";
            this.ParseNowButton.UseVisualStyleBackColor = true;
            this.ParseNowButton.Click += new System.EventHandler(this.ParseNowButton_Click);
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 105);
            this.Controls.Add(this.ParseNowButton);
            this.Controls.Add(this.DropDownList);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.SaveText);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.OpenText);
            this.Controls.Add(this.OpenButton);
            this.Name = "Form1";
            this.Text = "IMDB Parser v0.1";
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OpenButton;
        private System.Windows.Forms.TextBox OpenText;
        private System.Windows.Forms.TextBox SaveText;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.ProgressBar ProgressBar;
        private System.Windows.Forms.ComboBox DropDownList;
        private System.Windows.Forms.Button ParseNowButton;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
    }
}

