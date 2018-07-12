using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private DataExcel _data = new DataExcel();

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                _data.SetApplicationFolder(Application.StartupPath);

                txtTemplate.Text = _data.FileTemplatePath;
                txtDTO.Text = _data.FolderDTOPath;
                txtAPI.Text = _data.FolderAPIPath;
                txtAPIConfig.Text = _data.FileAPIConfigPath;
                txtAPIHelp.Text = _data.FileAPIHelpPath;
                txtAPIVersion.Text = _data.APIVersion;
                txtTestingCase.Text = _data.FileCollectionPath;
                txtTestingEnv.Text = _data.FileEnvironmentPath;

                _data.FileTemplatePath = txtTemplate.Text;
                _data.LoadTemplate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Filter = "All|*.xls;*.xlsx|Excel 2007|*.xls|Excel|*.xlsx";
                if (op.ShowDialog() == DialogResult.OK)
                {
                    txtTemplate.Text = op.FileName;
                    _data.FileTemplatePath = txtTemplate.Text;
                    _data.LoadTemplate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnTemplateNew_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog op = new SaveFileDialog();
                op.Filter = "All|*.xls;*.xlsx|Excel 2007|*.xls|Excel|*.xlsx";
                if (op.ShowDialog() == DialogResult.OK)
                {
                    txtTemplateNew.Text = op.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnJson_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog op = new SaveFileDialog();
                op.Filter = "json|*.json";
                if (op.ShowDialog() == DialogResult.OK)
                {
                    txtJson.Text = op.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                _data.FileTemplateNewPath = txtTemplateNew.Text;
                _data.FolderMoviePath = txtMovieFolder.Text;
                _data.FileJsonPath = txtJson.Text;
                _data.GenData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnMovieFolder_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog op = new FolderBrowserDialog();
                if (op.ShowDialog() == DialogResult.OK)
                {
                    txtMovieFolder.Text = op.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
