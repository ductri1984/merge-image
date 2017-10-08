using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MyHelper;

namespace winform
{
    public partial class frmSetting : Form
    {
        public frmSetting()
        {
            InitializeComponent();
        }

        private void frmSetting_Load(object sender, EventArgs e)
        {
            var setting = HelperSetting.GetSetting();
            if (setting != null)
            {
                if (!string.IsNullOrEmpty(setting.FileData))
                    txtFileData.Text = setting.FileData;
                if (!string.IsNullOrEmpty(setting.FolderBackground))
                    txtFolderBackground.Text = setting.FolderBackground;
                if (!string.IsNullOrEmpty(setting.FolderPointer))
                    txtFolderPointer.Text = setting.FolderPointer;
            }
        }

        private void btnFileData_Click(object sender, EventArgs e)
        {
            SaveFileDialog open = new SaveFileDialog();
            open.Filter = "json|*.json";
            if (!string.IsNullOrEmpty(txtFileData.Text))
                open.FileName = txtFileData.Text;
            if (open.ShowDialog() == DialogResult.OK)
            {
                txtFileData.Text = open.FileName;
            }
        }

        private void btnFolderLibrary_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dir = new FolderBrowserDialog();
            if (!string.IsNullOrEmpty(txtFolderBackground.Text))
                dir.SelectedPath = txtFolderBackground.Text;
            if (dir.ShowDialog() == DialogResult.OK)
            {
                txtFolderBackground.Text = dir.SelectedPath;
            }
        }

        private void btnFolderPointer_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dir = new FolderBrowserDialog();
            if (!string.IsNullOrEmpty(txtFolderPointer.Text))
                dir.SelectedPath = txtFolderPointer.Text;
            if (dir.ShowDialog() == DialogResult.OK)
            {
                txtFolderPointer.Text = dir.SelectedPath;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want save changes ?", "Save", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                HelperSetting.SetSetting(new HelperSetting_Item
                {
                    FileData = txtFileData.Text,
                    FolderBackground = txtFolderBackground.Text,
                    FolderPointer = txtFolderPointer.Text
                });
                this.DialogResult = DialogResult.OK;
            }            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
