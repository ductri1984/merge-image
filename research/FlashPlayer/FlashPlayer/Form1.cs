using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlashPlayer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void mnuOpen_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Filter = "Flash files|*.swf";
                if (op.ShowDialog() == DialogResult.OK)
                {
                    axShockwaveFlash1.Movie = op.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void mnuPlay_Click(object sender, EventArgs e)
        {
            try
            {
                axShockwaveFlash1.Playing = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void mnuPause_Click(object sender, EventArgs e)
        {
            try
            {
                axShockwaveFlash1.Playing = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void mnuShowOnTop_Click(object sender, EventArgs e)
        {
            try
            {
                if (mnuShowOnTop.Checked)
                {
                    this.TopMost = false;
                    mnuShowOnTop.Checked = false;
                }
                else
                {
                    this.TopMost = true;
                    mnuShowOnTop.Checked = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
