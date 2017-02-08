using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace winform
{
    public partial class frmPointer : Form
    {
        public frmPointer()
        {
            InitializeComponent();
        }

        private void mnuAdd_Click(object sender, EventArgs e)
        {
            frmPointer_Add frm = new frmPointer_Add();
            if (frm.ShowDialog() == DialogResult.OK)
            {

            }
        }

        private void frmPointer_Load(object sender, EventArgs e)
        {

        }
    }
}
