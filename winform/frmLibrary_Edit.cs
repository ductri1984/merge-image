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
    public partial class frmLibrary_Edit : Form
    {
        public frmLibrary_Edit()
        {
            InitializeComponent();
        }

        public HelperImage_Item ItemEdit { get; set; }

        private void frmLibrary_Edit_Load(object sender, EventArgs e)
        {
            txtName.Text = ItemEdit.Name;
            txtOutput.Text = ItemEdit.PathOutput;
            if (!string.IsNullOrEmpty(txtName.Text))
                txtName.Enabled = false;
            else
                txtName.Enabled = true;
        }

        private void btnOutput_Click(object sender, EventArgs e)
        {
            SaveFileDialog open = new SaveFileDialog();
            open.Filter = "jpg|*.jpg";
            if (!string.IsNullOrEmpty(txtOutput.Text))
                open.FileName = txtOutput.Text;
            if (open.ShowDialog() == DialogResult.OK)
            {
                txtOutput.Text = open.FileName;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text))
                MessageBox.Show("Input name");
            else
            {
                if (HelperImage.ExistsName(txtName.Text) && txtName.Enabled == true)
                    MessageBox.Show("Name has other item");
                else
                {
                    if (MessageBox.Show("Do you want save changes ?", "Save", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        HelperImage.SaveInfo(txtName.Text, txtOutput.Text);
                        DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
