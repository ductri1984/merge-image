using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace database_compare
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private DataTable _dtSource = null;
        private DataTable _dtSourceColumns = null;
        private DataTable _dtSourceForeign = null;
        private DataTable _dtTarget = null;
        private DataTable _dtTargetColumns = null;
        private DataTable _dtTargetForeign = null;

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                txtSourceServer.Text = "125.212.248.161";
                txtSourceDatabase.Text = "tms_dev";
                txtSourceUser.Text = "tms_dev";
                txtSourcePassword.Text = "dev123";
                txtTargetServer.Text = "192.168.1.252";
                txtTargetDatabase.Text = "tms_dev";
                txtTargetUser.Text = "tms_dev";
                txtTargetPassword.Text = "dev123";

                _dtSource = new DataTable();
                _dtSource.Columns.Add("IsSuccess", typeof(bool));
                _dtSource.Columns.Add("TableName", typeof(string));
                _dtSource.Columns.Add("Note", typeof(string));
                _dtSource.Columns.Add("Columns", typeof(int));
                _dtSource.Columns.Add("Foreign", typeof(int));
                _dtSource.Columns.Add("ForeignText", typeof(string));
                _dtSource.Columns.Add("id", typeof(int));
                _dtSourceColumns = new DataTable();
                _dtSourceForeign = new DataTable();
                _dtTarget = _dtSource.Clone();
                _dtTargetColumns = new DataTable();
                _dtTargetForeign = new DataTable();

                gvSource.CellFormatting += gv_CellFormatting;
                gvTarget.CellFormatting += gv_CellFormatting;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void gv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                DataGridViewRow row = (sender as DataGridView).Rows[e.RowIndex];
                DataRowView dv = row.DataBoundItem as DataRowView;
                if (Convert.ToBoolean(dv["IsSuccess"]))
                    e.CellStyle.BackColor = Color.PaleGreen;
                else
                    e.CellStyle.BackColor = Color.LightPink;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                _dtSource.Clear();
                _dtSourceColumns.Clear();
                _dtSourceForeign.Clear();
                _dtTarget.Clear();
                _dtTargetColumns.Clear();
                _dtTargetForeign.Clear();

                string strError = string.Empty;
                string strConnect = "Data Source=" + txtSourceServer.Text + ";Initial Catalog=" + txtSourceDatabase.Text + ";uid=" + txtSourceUser.Text + ";pwd=" +
                    txtSourcePassword.Text;
                SqlConnection cnnSource = new SqlConnection(strConnect);
                try
                {
                    cnnSource.Open();
                    cnnSource.Close();
                }
                catch
                {
                    strError += "Can't connect source database\n";
                }
                strConnect = "Data Source=" + txtTargetServer.Text + ";Initial Catalog=" + txtTargetDatabase.Text + ";uid=" + txtTargetUser.Text + ";pwd=" +
                    txtTargetPassword.Text;
                SqlConnection cnnTarget = new SqlConnection(strConnect);
                try
                {
                    cnnTarget.Open();
                    cnnTarget.Close();
                }
                catch
                {
                    strError += "Can't connect target database\n";
                }
                if (strError != string.Empty)
                {
                    MessageBox.Show(strError);
                }
                else
                {
                    string sqlTable = "SELECT name,id FROM sysobjects where xtype='U'";
                    string sqlColumns = "SELECT cols.name,cols.id,cols.xtype,cols.[length],isnullable FROM syscolumns cols inner join sysobjects tbl ON cols.id=tbl.id where tbl.xtype='U' order by cols.name";
                    //56:int,231:nvarchar,62:float,60:money,61:date,99:ntext,104:bit,127:bigint
                    string sqlForeign = "SELECT K_Table = FK.TABLE_NAME, FK_Column = CU.COLUMN_NAME,PK_Table = PK.TABLE_NAME,PK_Column = PT.COLUMN_NAME,Constraint_Name = C.CONSTRAINT_NAME " +
                        "FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS C INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS FK " +
                        "ON C.CONSTRAINT_NAME = FK.CONSTRAINT_NAME INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS PK " +
                        "ON C.UNIQUE_CONSTRAINT_NAME = PK.CONSTRAINT_NAME INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE CU " +
                        "ON C.CONSTRAINT_NAME = CU.CONSTRAINT_NAME INNER JOIN ( " +
                            "SELECT i1.TABLE_NAME, i2.COLUMN_NAME " +
                            "FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS i1 " +
                            "INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE i2 ON i1.CONSTRAINT_NAME = i2.CONSTRAINT_NAME " +
                            "WHERE i1.CONSTRAINT_TYPE = 'PRIMARY KEY' ) PT " +
                        "ON PT.TABLE_NAME = PK.TABLE_NAME ORDER BY CONSTRAINT_NAME";
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cnnSource;
                    cmd.CommandText = sqlColumns;
                    SqlDataAdapter adt = new SqlDataAdapter(cmd);
                    adt.Fill(_dtSourceColumns);
                    cmd.CommandText = sqlForeign;
                    adt = new SqlDataAdapter(cmd);
                    adt.Fill(_dtSourceForeign);
                    cmd.CommandText = sqlTable;
                    cnnSource.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        var row = _dtSource.NewRow();
                        row["TableName"] = dr["name"];
                        row["id"] = dr["id"];
                        row["IsSuccess"] = true;
                        row["Columns"] = _dtSourceColumns.Select("id=" + row["id"]).Length;
                        row["Foreign"] = _dtSourceForeign.Select("PK_Table='" + row["TableName"] + "'").Length;
                        _dtSource.Rows.Add(row);
                    }
                    cnnSource.Close();

                    cmd = new SqlCommand();
                    cmd.Connection = cnnTarget;
                    cmd.CommandText = sqlColumns;
                    adt = new SqlDataAdapter(cmd);
                    adt.Fill(_dtTargetColumns);
                    cmd.CommandText = sqlForeign;
                    adt = new SqlDataAdapter(cmd);
                    adt.Fill(_dtTargetForeign);
                    cmd.CommandText = sqlTable;
                    cnnTarget.Open();
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        var row = _dtTarget.NewRow();
                        row["TableName"] = dr["name"];
                        row["id"] = dr["id"];
                        row["IsSuccess"] = true;
                        row["Columns"] = _dtTargetColumns.Select("id=" + row["id"]).Length;
                        row["Foreign"] = _dtTargetForeign.Select("PK_Table='" + row["TableName"] + "'").Length;
                        _dtTarget.Rows.Add(row);
                    }
                    cnnTarget.Close();

                    foreach (DataRow itemSource in _dtSource.Select("IsSuccess=1"))
                    {
                        var drs = _dtTarget.Select("TableName='" + itemSource["TableName"] + "'");
                        if (drs.Length == 0)
                        {
                            itemSource["IsSuccess"] = false;
                            itemSource["Note"] = "Can't find in target";
                        }
                        else
                        {
                            var flag = true;
                            string note = string.Empty;
                            var itemTarget = drs[0];
                            if (_dtTarget.Select("TableName='" + itemSource["TableName"] + "' and Columns=" + itemSource["Columns"] + " and Foreign=" + itemSource["Foreign"]).Length == 0)
                            {
                                flag = false;
                                note = "Source has difference total columns or foreign with target";
                            }
                            else
                            {
                                string strSource = string.Empty;
                                string strSourceType = string.Empty;
                                string strSourceNull = string.Empty;
                                foreach (DataRow detail in _dtSourceColumns.Select("id=" + itemSource["id"]))
                                {
                                    strSource += detail["name"] + ":" + detail["xtype"] + ":" + detail["length"] + ":" + detail["isnullable"] + ";";
                                    strSourceType += detail["xtype"] + ";";
                                    strSourceNull += detail["isnullable"] + ";";
                                }
                                string strTarget = string.Empty;
                                string strTargetType = string.Empty;
                                string strTargetNull = string.Empty;
                                foreach (DataRow detail in _dtTargetColumns.Select("id=" + itemTarget["id"]))
                                {
                                    strTarget += detail["name"] + ":" + detail["xtype"] + ":" + detail["length"] + ":" + detail["isnullable"] + ";";
                                    strTargetType += detail["xtype"] + ";";
                                    strTargetNull += detail["isnullable"] + ";";
                                }
                                if (strSource != strTarget)
                                {
                                    flag = false;
                                    if (strSourceType != strTargetType)
                                    {
                                        note = "Source has difference columns type with target";
                                    }
                                    else if (strSourceNull != strTargetNull)
                                    {
                                        note = "Source has difference columns null with target";
                                    }
                                    else
                                    {
                                        note = "Source has difference columns with target";
                                    }
                                }
                                else
                                {
                                    strSource = string.Empty;
                                    foreach (DataRow detail in _dtSourceForeign.Select("PK_Table='" + itemSource["TableName"] + "'"))
                                    {
                                        strSource += detail["Constraint_Name"] + ";";
                                    }
                                    strTarget = string.Empty;
                                    foreach (DataRow detail in _dtTargetForeign.Select("PK_Table='" + itemTarget["TableName"] + "'"))
                                    {
                                        strTarget += detail["Constraint_Name"] + ";";
                                    }
                                    if (strSource != strTarget)
                                    {
                                        flag = false;
                                        note = "Source has difference foreign with target";
                                        itemSource["ForeignText"] = strSource;
                                        itemTarget["ForeignText"] = strTarget;
                                    }
                                }
                            }

                            itemSource["IsSuccess"] = flag;
                            itemSource["Note"] = note;
                            itemTarget["IsSuccess"] = flag;
                            itemTarget["Note"] = note;
                        }

                    }
                }

                gvSource.DataSource = _dtSource;
                gvTarget.DataSource = _dtTarget;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
