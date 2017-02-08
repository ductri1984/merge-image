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
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void mnuSetting_Click(object sender, EventArgs e)
        {
            var frm = new frmSetting();
            frm.ShowDialog();
        }

        private void mnuLibrary_Click(object sender, EventArgs e)
        {

        }

        private void mnuAbout_Click(object sender, EventArgs e)
        {

        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                HelperSetting.SetFilePath(Application.StartupPath + "\\" + HelperSetting.FileName);
                var setting = HelperSetting.GetSetting();
                if (string.IsNullOrEmpty(setting.FileData) || string.IsNullOrEmpty(setting.FolderBackground) || string.IsNullOrEmpty(setting.FolderPointer))
                {
                    var frm = new frmSetting();
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
            }
        }
    }
}
