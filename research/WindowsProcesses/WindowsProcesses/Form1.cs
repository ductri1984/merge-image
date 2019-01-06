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
using System.Globalization;

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
                //_dicCPU = new Dictionary<string, PerformanceCounter>();
                //Process[] processList = Process.GetProcesses();
                //foreach (Process process in processList)
                //{
                //    try
                //    {
                //        if (!_dicCPU.ContainsKey(process.ProcessName))
                //        {
                //            var per = new PerformanceCounter("Process", "% Processor Time", process.ProcessName);
                //            per.NextValue();

                //            _dicCPU.Add(process.ProcessName, per);
                //        }
                //    }
                //    catch
                //    {

                //    }
                //}
                dt.Columns.Add("ImageName", typeof(string));
                dt.Columns.Add("CPU", typeof(string));
                dt.Columns.Add("Memory", typeof(string));
                dt.Columns.Add("PID", typeof(string));
                dt.Columns.Add("UserName", typeof(string));                
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
                timer1.Enabled = false;
                LoadData();
                timer1.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Dictionary<string, PerformanceCounter> _dicCPU;
        DataTable dt = new DataTable();

        public void LoadData()
        {
            // this updates the cpu usage
            //Process[] NewProcessList = Process.GetProcesses();
            Process[] NewProcessList = System.Diagnostics.Process.GetProcesses().Where(p =>
            {
                bool hasException = false;
                try { IntPtr x = p.Handle; }
                catch { hasException = true; }
                return !hasException;
            }).ToArray();


            UpdateCpuUsagePercent(NewProcessList);
            UpdateExistingProcesses(NewProcessList);
            AddNewProcesses(NewProcessList);

            dt.Rows.Clear();

            if (ProcessList != null)
            {
                foreach (var item in ProcessList)
                {
                    dt.Rows.Add(
                    item.Name,
                    item.CpuUsage,
                    BytesToReadableValue(item.PrivateMemorySize64),
                    item.ID.ToString(),
                    "",                    
                    "");
                }
            }

            //Process[] processList = Process.GetProcesses();
            //foreach (Process process in processList)
            //{
            //    if (_dicCPU.ContainsKey(process.ProcessName))
            //    {
            //        dt.Rows.Add(
            //        process.ProcessName,
            //        process.Id.ToString(),
            //        "",
            //        _dicCPU[process.ProcessName].NextValue(),
            //        BytesToReadableValue(process.PrivateMemorySize64),
            //        "");
            //    }
            //}

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dt;
        }

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

        private PerformanceCounter TotalCpuUsage = new PerformanceCounter("Process", "% Processor Time", "Idle");
        private float TotalCpuUsageValue;
        private ProcessInfo[] ProcessList;
        const Process CLOSED_PROCESS = null;
        const ProcessInfo PROCESS_INFO_NOT_FOUND = null;
        public double CpuUsagePercent;
        private int ProcessIndex;
        private CultureInfo ValueFormat = new CultureInfo("en-US");

        private void UpdateCpuUsagePercent(Process[] NewProcessList)
        {
            // total the cpu usage then divide to get the usage of 1%
            double Total = 0;
            ProcessInfo TempProcessInfo;
            TotalCpuUsageValue = TotalCpuUsage.NextValue();

            foreach (Process TempProcess in NewProcessList)
            {
                if (TempProcess.Id == 0) continue;

                TempProcessInfo = ProcessInfoByID(TempProcess.Id);
                if (TempProcessInfo == PROCESS_INFO_NOT_FOUND)
                    Total += TempProcess.TotalProcessorTime.TotalMilliseconds;
                else
                    Total += TempProcess.TotalProcessorTime.TotalMilliseconds - TempProcessInfo.OldCpuUsage;
            }
            CpuUsagePercent = Total / (100 - TotalCpuUsageValue);
        }

        private void UpdateExistingProcesses(Process[] NewProcessList)
        {
            // updates the cpu usage of already loaded processes
            if (ProcessList == null)
            {
                ProcessList = new ProcessInfo[NewProcessList.Length];
                return;
            }

            ProcessInfo[] TempProcessList = new ProcessInfo[NewProcessList.Length];
            ProcessIndex = 0;

            foreach (ProcessInfo TempProcess in ProcessList)
            {
                Process CurrentProcess = ProcessExists(NewProcessList, TempProcess.ID);

                if (CurrentProcess == CLOSED_PROCESS)
                {

                }
                else
                {
                    TempProcessList[ProcessIndex++] = GetProcessInfo(TempProcess, CurrentProcess);
                }
            }

            ProcessList = TempProcessList;
        }

        private void AddNewProcesses(Process[] NewProcessList)
        {
            // loads a new processes
            foreach (Process NewProcess in NewProcessList)
                if (!ProcessInfoExists(NewProcess))
                    AddNewProcess(NewProcess);
        }

        private ProcessInfo ProcessInfoByID(int ID)
        {
            // gets the process info by it's id
            if (ProcessList == null) return PROCESS_INFO_NOT_FOUND;

            for (int i = 0; i < ProcessList.Length; i++)
                if (ProcessList[i] != PROCESS_INFO_NOT_FOUND && ProcessList[i].ID == ID)
                    return ProcessList[i];

            return PROCESS_INFO_NOT_FOUND;
        }

        private Process ProcessExists(Process[] NewProcessList, int ID)
        {
            // checks to see if we already loaded the process
            foreach (Process TempProcess in NewProcessList)
                if (TempProcess.Id == ID)
                    return TempProcess;

            return CLOSED_PROCESS;
        }

        private ProcessInfo GetProcessInfo(ProcessInfo TempProcess, Process CurrentProcess)
        {
            // gets the process name , id, and cpu usage
            if (CurrentProcess.Id == 0)
                TempProcess.CpuUsage = (TotalCpuUsageValue).ToString("F", ValueFormat);
            else
            {
                long NewCpuUsage = (long)CurrentProcess.TotalProcessorTime.TotalMilliseconds;

                double cpu = ((NewCpuUsage - TempProcess.OldCpuUsage) / CpuUsagePercent);
                if (cpu < 0)
                {
                    cpu = -(cpu / 10);
                }
                TempProcess.CpuUsage = cpu.ToString("F", ValueFormat);
                TempProcess.OldCpuUsage = NewCpuUsage;
                TempProcess.PrivateMemorySize64 = CurrentProcess.PrivateMemorySize64;
            }

            return TempProcess;
        }

        private bool ProcessInfoExists(Process NewProcess)
        {
            // checks if the process info is already loaded
            if (ProcessList == null) return false;

            foreach (ProcessInfo TempProcess in ProcessList)
                if (TempProcess != PROCESS_INFO_NOT_FOUND && TempProcess.ID == NewProcess.Id)
                    return true;

            return false;
        }

        private void AddNewProcess(Process NewProcess)
        {
            // loads a new process
            ProcessInfo NewProcessInfo = new ProcessInfo();

            NewProcessInfo.Name = NewProcess.ProcessName;
            NewProcessInfo.ID = NewProcess.Id;

            ProcessList[ProcessIndex++] = GetProcessInfo(NewProcessInfo, NewProcess);
        }
    }
}
