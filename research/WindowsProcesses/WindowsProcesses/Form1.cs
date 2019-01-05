using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Dynamic;
using System.Diagnostics;

namespace WindowsProcesses
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                _dicCPU = new Dictionary<string, PerformanceCounter>();
                Process[] processList = Process.GetProcesses();
                foreach (Process process in processList)
                {
                    try
                    {
                        if (!_dicCPU.ContainsKey(process.ProcessName))
                        {
                            var per = new PerformanceCounter("Process", "% Processor Time", process.ProcessName);
                            per.NextValue();

                            _dicCPU.Add(process.ProcessName, per);
                        }
                    }
                    catch
                    {

                    }
                }
                dt.Columns.Add("ImageName", typeof(string));
                dt.Columns.Add("PID", typeof(string));
                dt.Columns.Add("UserName", typeof(string));
                dt.Columns.Add("CPU", typeof(string));
                dt.Columns.Add("Memory", typeof(string));
                dt.Columns.Add("Description", typeof(string));

                timer1.Enabled = true;
                //LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
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

        Dictionary<string, PerformanceCounter> _dicCPU;
        DataTable dt = new DataTable();
        
        public void LoadData()
        {
            dt.Rows.Clear();

            Process[] processList = Process.GetProcesses();
            foreach (Process process in processList)
            {
                if (_dicCPU.ContainsKey(process.ProcessName))
                {
                    dt.Rows.Add(
                    process.ProcessName,
                    process.Id.ToString(),
                    "",
                    _dicCPU[process.ProcessName].NextValue(),
                    BytesToReadableValue(process.PrivateMemorySize64),
                    "");
                }
            }

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dt;
        }

        /// <summary>
        /// Returns an Expando object with the description and username of a process from the process ID.
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        public ExpandoObject GetProcessExtraInformation(int processId)
        {
            // Query the Win32_Process
            string query = "Select * From Win32_Process Where ProcessID = " + processId;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection processList = searcher.Get();

            // Create a dynamic object to store some properties on it
            dynamic response = new ExpandoObject();
            response.Description = "";
            response.Username = "Unknown";

            foreach (ManagementObject obj in processList)
            {
                // Retrieve username 
                string[] argList = new string[] { string.Empty, string.Empty };
                int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                if (returnVal == 0)
                {
                    // return Username
                    response.Username = argList[0];

                    // You can return the domain too like (PCDesktop-123123\Username using instead
                    //response.Username = argList[1] + "\\" + argList[0];
                }

                // Retrieve process description if exists
                if (obj["ExecutablePath"] != null)
                {
                    try
                    {
                        FileVersionInfo info = FileVersionInfo.GetVersionInfo(obj["ExecutablePath"].ToString());
                        response.Description = info.FileDescription;
                    }
                    catch { }
                }
            }

            return response;
        }

        /// <summary>
        /// Method that converts bytes to its human readable value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public string BytesToReadableValue(long number)
        {
            List<string> suffixes = new List<string> { " B", " KB", " MB", " GB", " TB", " PB" };

            for (int i = 0; i < suffixes.Count; i++)
            {
                long temp = number / (int)Math.Pow(1024, i + 1);

                if (temp == 0)
                {
                    return (number / (int)Math.Pow(1024, i)) + suffixes[i];
                }
            }

            return number.ToString();
        }
    }
}
