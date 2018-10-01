using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessBackground;

namespace FormsBackground
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public DTODataDetail ItemEdit { get; set; }

        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                if (ItemEdit == null)
                {
                    btnDelete.Visible = false;
                    ItemEdit = new DTODataDetail();
                }
                txtFolderUpload.Text = ItemEdit.UploadFolder;
                chkIsFile.Checked = ItemEdit.IsFile;
                txtFileName.Text = ItemEdit.FileName;
                txtHandlerLink.Text = ItemEdit.HandlerLink;
                txtHandlerKey.Text = ItemEdit.HandlerKey;
                txtSpreadsheetID.Text = ItemEdit.SpreadsheetID;
                txtSpreadsheetName.Text = ItemEdit.SpreadsheetName;
                txtRangeValue.Text = HelperExcel.GetRangeName(ItemEdit.RowValueStart, ItemEdit.ColumnValueStart, ItemEdit.RowValueEnd, ItemEdit.ColumnValueEnd);
                txtCellTitle.Text = HelperExcel.GetCellName(ItemEdit.RowTitle, ItemEdit.ColumnTitle);
                txtLinkData.Text = ItemEdit.LinkData;
                txtLinkPush.Text = ItemEdit.LinkPush;
                txtFormatPushTitle.Text = ItemEdit.FormatPushTitle;
                txtFormatPushBody.Text = ItemEdit.FormatPushBody;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Do you want save ?", "Save", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ItemEdit.UploadFolder = txtFolderUpload.Text;
                    ItemEdit.IsFile = chkIsFile.Checked;
                    ItemEdit.FileName = txtFileName.Text;
                    ItemEdit.HandlerLink = txtHandlerLink.Text;
                    ItemEdit.HandlerKey = txtHandlerKey.Text;
                    ItemEdit.SpreadsheetID = txtSpreadsheetID.Text;
                    ItemEdit.SpreadsheetName = txtSpreadsheetName.Text;
                    ItemEdit.ColumnValueStart = HelperExcel.GetColumnFromByRange(txtRangeValue.Text);
                    ItemEdit.ColumnValueEnd = HelperExcel.GetColumnToByRange(txtRangeValue.Text);
                    ItemEdit.RowValueStart = HelperExcel.GetRowFromByRange(txtRangeValue.Text);
                    ItemEdit.RowValueEnd = HelperExcel.GetRowToByRange(txtRangeValue.Text);
                    ItemEdit.ColumnTitle = HelperExcel.GetColumnByCell(txtCellTitle.Text);
                    ItemEdit.RowTitle = HelperExcel.GetRowByCell(txtCellTitle.Text);
                    ItemEdit.LinkData = txtLinkData.Text;
                    ItemEdit.LinkPush = txtLinkPush.Text;
                    ItemEdit.FormatPushTitle = txtFormatPushTitle.Text;
                    ItemEdit.FormatPushBody = txtFormatPushBody.Text;

                    this.DialogResult = DialogResult.Yes;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.No;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnFolderUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if (chkIsFile.Checked)
                {
                    OpenFileDialog op = new OpenFileDialog();
                    op.Filter = "All|*.*";
                    if (op.ShowDialog() == DialogResult.OK)
                    {
                        txtFolderUpload.Text = op.FileName;
                    }
                }
                else
                {
                    FolderBrowserDialog op = new FolderBrowserDialog();
                    if (op.ShowDialog() == DialogResult.OK)
                    {
                        txtFolderUpload.Text = op.SelectedPath;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }        

        private void btnIISGatewayData_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Filter = "JSON|*.json|All|*.*";
                if (op.ShowDialog() == DialogResult.OK)
                {
                    txtFileName.Text = op.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }        
    }
}
