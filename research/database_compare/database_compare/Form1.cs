﻿using System;
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
                txtSourceServer.Text = "dev.ooc.vn,15000";
                txtSourceDatabase.Text = "oocempty.setting";
                txtSourceUser.Text = "sa";
                txtSourcePassword.Text = "@QWE123$%^";
                txtTargetServer.Text = "dev.ooc.vn,15000";
                txtTargetDatabase.Text = "oocdev.setting";
                txtTargetUser.Text = "sa";
                txtTargetPassword.Text = "@QWE123$%^";

                _dtSource = new DataTable();
                _dtSource.Columns.Add("IsSuccess", typeof(bool));
                _dtSource.Columns.Add("TableName", typeof(string));
                _dtSource.Columns.Add("Note", typeof(string));
                _dtSource.Columns.Add("Columns", typeof(int));
                _dtSource.Columns.Add("Foreign", typeof(int));
                _dtSource.Columns.Add("ForeignText", typeof(string));
                _dtSource.Columns.Add("id", typeof(int));
                _dtSource.Columns.Add("IsIdentity", typeof(bool));
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
                    //cols.[status]=128 and cols.colstat=1 and cols.isnullable=0 -> identity
                    string sqlColumns = "SELECT cols.name,cols.id,cols.xtype,cols.[length],isnullable,colorder,cols.[status],cols.colstat FROM syscolumns cols inner join sysobjects tbl ON cols.id=tbl.id where tbl.xtype='U' order by colorder";
                    //cols.xtype = 56:int,231:nvarchar,62:float,60:money,61:datetime,99:ntext,104:bit,127:bigint
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
                        row["IsIdentity"] = _dtSourceColumns.Select("id=" + row["id"] + " and status=128 and colstat=1 and isnullable=0").Length > 0;

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
                        row["IsIdentity"] = _dtTargetColumns.Select("id=" + row["id"] + " and status=128 and colstat=1 and isnullable=0").Length > 0;

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
                                string strSourceSort = string.Empty;
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

                //GetIDtoLong();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GetIDtoLong()
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataRow itemSource in _dtSource.Rows)
            {
                //foreach (DataRow detail in _dtSourceColumns.Select("id=" + itemSource["id"]))
                //{
                //    if (detail["name"] == null || detail["name"] != DBNull.Value)
                //    {
                //        string str = detail["name"].ToString();
                //        if (str == "ID" || str.StartsWith("ID"))
                //        {
                //            if(str == "ID")
                //            {
                //                sb.AppendLine($"ALTER TABLE [dbo].[{itemSource["TableName"]}] DROP CONSTRAINT [PK_dbo.{itemSource["TableName"]}]");
                //                sb.AppendLine("GO");
                //            }

                //            if (detail["isnullable"].ToString() == "0")
                //            {
                //                sb.AppendLine($"ALTER TABLE [dbo].[{itemSource["TableName"]}] ALTER COLUMN [{str}] bigint NOT NULL");
                //            }
                //            else
                //            {
                //                sb.AppendLine($"ALTER TABLE [dbo].[{itemSource["TableName"]}] ALTER COLUMN [{str}] bigint NULL");
                //            }
                //            sb.AppendLine("GO");

                //            if (str == "ID")
                //            {
                //                sb.AppendLine($"ALTER TABLE [dbo].[{itemSource["TableName"]}] ADD CONSTRAINT [PK_dbo.{itemSource["TableName"]}] PRIMARY KEY([ID])");
                //                sb.AppendLine("GO");
                //            }
                //        }
                //    }
                //}

                if (itemSource["TableName"].ToString() != "__MigrationHistory" && itemSource["IsSuccess"].ToString() != "True")
                {
                    //sb.AppendLine($"ALTER TABLE [dbo].[{itemSource["TableName"]}] ADD [CreatedFrom] nvarchar(2000) NULL");
                    //sb.AppendLine("GO");
                    //sb.AppendLine($"ALTER TABLE [dbo].[{itemSource["TableName"]}] ADD [ModifiedFrom] nvarchar(2000) NULL");
                    //sb.AppendLine("GO");

                    sb.AppendLine($"ALTER TABLE [dbo].[{itemSource["TableName"]}] ADD [Remark] NVARCHAR(max) NULL");
                    sb.AppendLine("GO");
                    sb.AppendLine($"IF NOT EXISTS (SELECT 'Y' FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{itemSource["TableName"]}' AND COLUMN_NAME = 'Sort')");
                    sb.AppendLine("BEGIN");
                    sb.AppendLine($"ALTER TABLE [dbo].[{itemSource["TableName"]}] ADD [Sort] int NULL");
                    sb.AppendLine("END");
                    sb.AppendLine("GO");
                    sb.AppendLine($"UPDATE [dbo].[{itemSource["TableName"]}] set [Sort] = 1");
                    sb.AppendLine("GO");
                    sb.AppendLine($"ALTER TABLE [dbo].[{itemSource["TableName"]}] ALTER COLUMN [Sort] int NOT NULL");
                    sb.AppendLine("GO");
                }
            }
            string strdata = sb.ToString();
        }

        private void btnGenScript_Click(object sender, EventArgs e)
        {
            try
            {
                if (_dtSource.Rows.Count > 0 && _dtTarget.Rows.Count > 0)
                {
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
                        throw new Exception("Can't connect source database");
                    }
                    int totalRow = 0;
                    int totalFile = 1;
                    string strFolder = btnGenScript_Click_GenFolder();
                    StringBuilder sb = new StringBuilder();
                    foreach (DataRow itemSource in _dtSource.Select("IsSuccess=1"))
                    {
                        #region gen data
                        var drs = _dtTarget.Select("TableName='" + itemSource["TableName"] + "'");
                        if (drs.Length > 0 && Convert.ToBoolean(drs[0]["IsSuccess"]))
                        {
                            DataRow itemTarget = drs[0];
                            DataRow[] colsSource = _dtSourceColumns.Select("id=" + itemSource["id"]);

                            var sqlTable = "select * from dbo." + itemSource["TableName"];
                            var dtData = new DataTable();
                            var cmd = new SqlCommand();
                            cmd.Connection = cnnSource;
                            cmd.CommandText = sqlTable;
                            var adt = new SqlDataAdapter(cmd);
                            adt.Fill(dtData);

                            if (dtData.Rows.Count > 0 && colsSource.Length > 0)
                            {
                                Dictionary<string, int> dicCol = new Dictionary<string, int>();
                                string strDataStart = "INSERT [dbo].[" + itemSource["TableName"] + "] (";
                                foreach (DataRow itemCol in colsSource)
                                {
                                    string colName = itemCol["name"].ToString();
                                    int colType = Convert.ToInt32(itemCol["xtype"]);

                                    if (!dicCol.ContainsKey(colName))
                                    {
                                        dicCol.Add(colName, colType);
                                        strDataStart += "[" + colName + "],";
                                    }
                                }
                                strDataStart = strDataStart.Substring(0, strDataStart.Length - 1) + ") VALUES(";

                                bool isIdentity = Convert.ToBoolean(itemTarget["IsIdentity"]);
                                if (isIdentity)
                                {
                                    sb.AppendLine("SET IDENTITY_INSERT [dbo].[" + itemSource["TableName"] + "] ON");
                                    sb.AppendLine("GO");
                                }

                                foreach (DataRow itemData in dtData.Rows)
                                {
                                    string strData = string.Empty;
                                    foreach (var itemCol in dicCol)
                                    {
                                        var obj = itemData[itemCol.Key];
                                        if (obj == DBNull.Value || obj == null)
                                        {
                                            strData += ",NULL";
                                        }
                                        else
                                        {
                                            #region tyle
                                            //34 image
                                            //35 text
                                            //36 uniqueidentifier
                                            //48 tinyint
                                            //52 smallint
                                            //56 int
                                            //58 smalldatetime
                                            //59 real
                                            //60 money
                                            //61 datetime
                                            //62 float
                                            //98 sql_variant
                                            //99 ntext
                                            //104 bit
                                            //106 decimal
                                            //108 numeric
                                            //122 smallmoney
                                            //127 bigint
                                            //165 varbinary
                                            //167 varchar
                                            //173 binary
                                            //175 char
                                            //189 timestamp
                                            //231 nvarchar
                                            //231 sysname
                                            //239 nchar
                                            //241 xml
                                            #endregion
                                            switch (itemCol.Value)
                                            {
                                                //string
                                                case 35:
                                                case 167:
                                                case 231:
                                                case 239:
                                                    string str = obj.ToString();
                                                    str = str.Replace("'", "''");
                                                    strData += ",N'" + str + "'";
                                                    break;
                                                //datetime
                                                case 61:
                                                    strData += ",'" + Convert.ToDateTime(obj).ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
                                                    break;
                                                default:
                                                    strData += "," + obj.ToString();
                                                    break;
                                            }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(strData))
                                        strData = strData.Substring(1);
                                    strData = strDataStart + strData + ")";
                                    sb.AppendLine(strData);
                                    sb.AppendLine("GO");
                                    totalRow++;
                                }

                                if (isIdentity)
                                {
                                    sb.AppendLine("SET IDENTITY_INSERT [dbo].[" + itemSource["TableName"] + "] OFF");
                                    sb.AppendLine("GO");
                                }
                            }
                        }
                        #endregion

                        #region gen file
                        if (totalRow > 200000)
                        {
                            btnGenScript_Click_GenFile(strFolder, sb, ref totalFile);
                            totalRow = 0;
                        }
                        #endregion
                    }
                    if (totalRow > 0)
                    {
                        btnGenScript_Click_GenFile(strFolder, sb, ref totalFile);
                    }

                    System.Diagnostics.Process.Start("explorer.exe", strFolder);
                    //sqlcmd /S DESKTOP-KT6Q0LA\MSSQLSERVER01 /d ooc.kpi2 -E -i"%%G" -f 65001

                }
                else
                    MessageBox.Show("Not found source and target");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string btnGenScript_Click_GenFolder()
        {
            string str = System.IO.Path.Combine(Application.StartupPath, DateTime.Now.ToString("yyyyMMddHH"));
            if (System.IO.Directory.Exists(str))
            {
                System.IO.Directory.Delete(str, true);
            }
            System.IO.Directory.CreateDirectory(str);
            return str;
        }

        private void btnGenScript_Click_GenFile(string strFolder, StringBuilder sb, ref int totalFile)
        {
            string str = System.IO.Path.Combine(strFolder, "file" + totalFile + ".sql");
            if (System.IO.File.Exists(str))
            {
                totalFile++;
                str = System.IO.Path.Combine(strFolder, "file" + totalFile + ".sql");
                if (System.IO.File.Exists(str))
                    throw new Exception("exists file temp");
            }
            System.IO.File.AppendAllText(str, sb.ToString(), Encoding.Unicode);
            totalFile++;
        }


    }
}
