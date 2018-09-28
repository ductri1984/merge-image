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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string _tempfile = "temp.json";
        private DTOTemp _dtotemp = default(DTOTemp);
        private DTOData _dtodata = default(DTOData);

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                Business.SetFolderPath(Application.StartupPath);
                _tempfile = System.IO.Path.Combine(Application.StartupPath, _tempfile);
                if (System.IO.File.Exists(_tempfile))
                {
                    string str = System.IO.File.ReadAllText(_tempfile);
                    if (!string.IsNullOrEmpty(str))
                    {
                        try
                        {
                            _dtotemp = Newtonsoft.Json.JsonConvert.DeserializeObject<DTOTemp>(str);
                        }
                        catch
                        {
                            _dtotemp = default(DTOTemp);
                        }
                    }
                }
                if (_dtotemp != null)
                {
                    txtChooseJson.Text = _dtotemp.JsonPath;
                    txtChooseLog.Text = _dtotemp.LogPath;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnChooseJson_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Filter = "JSON|*.json|All|*.*";
                if (op.ShowDialog() == DialogResult.OK)
                {
                    txtChooseJson.Text = op.FileName;
                    SaveTemp();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnChooseLog_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Filter = "Text|*.txt|All|*.*";
                if (op.ShowDialog() == DialogResult.OK)
                {
                    txtChooseLog.Text = op.FileName;
                    SaveTemp();
                }
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
                if (!string.IsNullOrEmpty(txtChooseJson.Text))
                {
                    if (System.IO.File.Exists(txtChooseJson.Text))
                    {
                        string str = System.IO.File.ReadAllText(txtChooseJson.Text);
                        _dtodata = default(DTOData);
                        if (!string.IsNullOrEmpty(str))
                        {
                            try
                            {
                                _dtodata = Newtonsoft.Json.JsonConvert.DeserializeObject<DTOData>(str);
                            }
                            catch
                            {
                                _dtodata = default(DTOData);
                            }
                        }
                        if (_dtodata == null)
                        {
                            _dtodata = new DTOData();
                            _dtodata.ListDetails = new List<DTODataDetail>();
                        }
                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (_dtodata != null)
                {
                    var data = dataGridView1.SelectedRows[0].DataBoundItem;
                    if (data is DTODataDetail)
                    {
                        Form2 frm = new Form2();
                        frm.ItemEdit = data as DTODataDetail;
                        var res = frm.ShowDialog();
                        if (res == DialogResult.Yes)
                        {
                            var find = _dtodata.ListDetails.FirstOrDefault(c => c.ID == frm.ItemEdit.ID);
                            if (find != null)
                            {
                                find.UploadFolder = frm.ItemEdit.UploadFolder;
                                find.IsFile = frm.ItemEdit.IsFile;
                                find.FileName = frm.ItemEdit.FileName;
                                find.HandlerLink = frm.ItemEdit.HandlerLink;
                                find.HandlerKey = frm.ItemEdit.HandlerKey;
                                find.ColumnValueStart = frm.ItemEdit.ColumnValueStart;
                                find.ColumnValueEnd = frm.ItemEdit.ColumnValueEnd;
                                find.RowValueStart = frm.ItemEdit.RowValueStart;
                                find.RowValueEnd = frm.ItemEdit.RowValueEnd;
                                find.ColumnTitle = frm.ItemEdit.ColumnTitle;
                                find.RowTitle = frm.ItemEdit.RowTitle;
                                find.LinkData = frm.ItemEdit.LinkData;
                                find.LinkPush = frm.ItemEdit.LinkPush;
                                find.FormatPushTitle = frm.ItemEdit.FormatPushTitle;
                                find.FormatPushBody = frm.ItemEdit.FormatPushBody;

                                dataGridView1.DataSource = null;
                                dataGridView1.DataSource = _dtodata.ListDetails;
                            }
                        }
                        else if (res == DialogResult.No)
                        {
                            var find = _dtodata.ListDetails.FirstOrDefault(c => c.UploadFolder == frm.ItemEdit.UploadFolder);
                            if (find != null)
                            {
                                _dtodata.ListDetails.Remove(find);

                                dataGridView1.DataSource = null;
                                dataGridView1.DataSource = _dtodata.ListDetails;
                            }
                        }
                    }
                }
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
                if (_dtodata != null)
                {
                    if (!string.IsNullOrEmpty(txtChooseJson.Text))
                    {
                        _dtodata.IsLog = chkIsLog.Checked;
                        _dtodata.IsStop = chkIsStop.Checked;
                        string str = Newtonsoft.Json.JsonConvert.SerializeObject(_dtodata);
                        if (System.IO.File.Exists(txtChooseJson.Text))
                            System.IO.File.Delete(txtChooseJson.Text);
                        System.IO.File.AppendAllText(txtChooseJson.Text, str, Encoding.UTF8);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtChooseJson.Text))
                {
                    List<string> strs = new List<string>();
                    Business.ReadFile(txtChooseJson.Text);
                    Business.RunData((string str) =>
                    {
                        strs.Add(str);
                        txtLog.Text = string.Join(Environment.NewLine, strs);
                    }, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLoadLog_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtChooseLog.Text))
                {
                    if (System.IO.File.Exists(txtChooseLog.Text))
                    {
                        string str = System.IO.File.ReadAllText(txtChooseLog.Text);
                        txtLog.Text = str;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtChooseLog.Text))
                {
                    if (System.IO.File.Exists(txtChooseLog.Text))
                    {
                        System.IO.File.WriteAllText(txtChooseLog.Text, "");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SaveTemp()
        {
            _dtotemp = new DTOTemp();
            _dtotemp.JsonPath = txtChooseJson.Text;
            _dtotemp.LogPath = txtChooseLog.Text;
            string str = Newtonsoft.Json.JsonConvert.SerializeObject(_dtotemp);
            if (System.IO.File.Exists(_tempfile))
                System.IO.File.Delete(_tempfile);
            System.IO.File.AppendAllText(_tempfile, str, Encoding.UTF8);
        }

        private void LoadData()
        {
            if (_dtodata != null)
            {
                chkIsLog.Checked = _dtodata.IsLog;
                chkIsStop.Checked = _dtodata.IsStop;
                dataGridView1.DataSource = _dtodata.ListDetails;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (_dtodata != null)
                {
                    Form2 frm = new Form2();
                    if (frm.ShowDialog() == DialogResult.Yes)
                    {
                        string id = Guid.NewGuid().ToString();
                        while (_dtodata.ListDetails.Where(c => c.ID == id).Count() > 0)
                        {
                            id = Guid.NewGuid().ToString();
                        }
                        frm.ItemEdit.ID = id;
                        _dtodata.ListDetails.Add(frm.ItemEdit);

                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = _dtodata.ListDetails;
                    }
                }
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
    }
}
