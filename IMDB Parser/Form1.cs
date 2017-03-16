using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using IMDB_Parser.Parsers;

namespace IMDB_Parser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            InitializeComboBox();
        }

        private void InitializeComboBox()
        {
            foreach(var item in Parsers.Parsers.GetParser())
            {
                DropDownList.Items.Add(item);
            }
        }

        private void ParseNowButton_Click(object sender, EventArgs e)
        {
            CheckFiles();

            EnableButtons(false);

            DoParsing();
        }

        private async void DoParsing()
        {
            try
            {
                bool parseSuccessful = false;
                IParser parser = DropDownList.SelectedItem as IParser;

                await Task.Run(() =>
                {
                    if (parser != null)
                    {
                        parseSuccessful = parser.Parse(OpenText.Text, SaveText.Text);
                    }
                });
                
                if (parseSuccessful)
                {
                    ShowSuccessMessage("Successfull Parsed\nThere could be some misformed data which is not parsed. Check logfile for more info");
                }
                else
                {
                    ShowErrorMessage("Not Parsed");
                }

            }
            catch (Exception err)
            {
                ShowErrorMessage(err.Message);
            }
            EnableButtons(true);
        }

        private void CheckFiles()
        {

            if (string.IsNullOrEmpty(OpenText.Text) || string.IsNullOrEmpty(SaveText.Text))
            {
                ShowErrorMessage("Files not selected!");
            }
            else if (!File.Exists(OpenText.Text))
            {
                ShowErrorMessage("File doesn't exist");
            }
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowSuccessMessage(string message)
        {
            MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void EnableButtons(bool enable)
        {
            SaveButton.Enabled = enable;
            OpenButton.Enabled = enable;
            ParseNowButton.Enabled = enable;
            ProgressBar.Visible = !enable;
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "IMDB list files | *.list";
            dialog.Title = "Open IMDB list file";

            if(dialog.ShowDialog() == DialogResult.OK && File.Exists(dialog.FileName))
            {
                OpenText.Text = dialog.FileName;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Comma Seperated Files | *.csv";
            dialog.Title = "Save CSV file";
            dialog.CheckPathExists = true;
            dialog.OverwritePrompt = true;

            if(dialog.ShowDialog() == DialogResult.OK)
            {
                SaveText.Text = dialog.FileName;
            }
        }

        private void OpenText_Leave(object sender, EventArgs e)
        {
            if(!File.Exists(OpenText.Text))
            {
                OpenText.Text = "";
            }
        }
    }
}
