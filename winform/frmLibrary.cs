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
    public partial class frmLibrary : Form
    {
        public frmLibrary()
        {
            InitializeComponent();
        }

        private void frmLibrary_Load(object sender, EventArgs e)
        {
            try
            {
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadData()
        {
            var lst = HelperImage.List();
            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("PathBackground", typeof(string));
            dt.Columns.Add("PathOutput", typeof(string));
            foreach (var item in lst)
            {
                dt.Rows.Add(item.Name, item.PathBackground, item.PathOutput);
            }
            DataView dv = dt.DefaultView;
            if (!string.IsNullOrEmpty(txtSearch.Text))
                dv.RowFilter = "Name like '%" + txtSearch.Text + "%'";
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = dv;
        }

        private void mnuAdd_Click(object sender, EventArgs e)
        {
            frmLibrary_Edit frm = new frmLibrary_Edit();
            frm.ItemEdit = new HelperImage_Item();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {            
            if (e.ColumnIndex == 0)
            {
                DataRowView dv = dataGridView1.Rows[e.RowIndex].DataBoundItem as DataRowView;
                var item = HelperImage.GetByName(dv["Name"].ToString());
                frmLibrary_Edit frm = new frmLibrary_Edit();
                frm.ItemEdit = item;
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
            else if (e.ColumnIndex == 1)
            {
                DataRowView dv = dataGridView1.Rows[e.RowIndex].DataBoundItem as DataRowView;
                if (MessageBox.Show("Do you want delete this item ?", "Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    HelperImage.Delete(dv["Name"].ToString());
                    LoadData();
                }
            }
            else if (e.ColumnIndex == 2 || e.ColumnIndex == 3)
            {
                DataRowView dv = dataGridView1.Rows[e.RowIndex].DataBoundItem as DataRowView;
                var item = HelperImage.GetByName(dv["Name"].ToString());
                HelperImage.SetItemChange(item);
                this.Close();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
