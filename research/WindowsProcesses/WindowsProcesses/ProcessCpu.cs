using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Globalization;

namespace WindowsProcesses
{
    // events ..
    public delegate void NewProcessEvent(ProcessInfo TempProcess);
    public delegate void ProcessCloseEvent(ProcessInfo TempProcess);
    public delegate void ProcessUpdateEvent(ProcessInfo TempProcess);

    public class ProcessCpu
    {
        const Process CLOSED_PROCESS = null;
        const ProcessInfo PROCESS_INFO_NOT_FOUND = null;

        public static event NewProcessEvent CallNewProcess;
        public static event ProcessCloseEvent CallProcessClose;
        public static event ProcessUpdateEvent CallProcessUpdate;

        public static ProcessInfo[] ProcessList;
        public static double CpuUsagePercent;
        private static int ProcessIndex;
        public static CultureInfo ValueFormat = new CultureInfo("en-US");
        private static PerformanceCounter TotalCpuUsage = new PerformanceCounter("Process", "% Processor Time", "Idle");
        private static float TotalCpuUsageValue;

        public static void Init()
        {
            ValueFormat.NumberFormat.NumberDecimalDigits = 1;
        }

        public static void UpdateProcessList()
        {
            // this updates the cpu usage
            Process[] NewProcessList = Process.GetProcesses();
            UpdateCpuUsagePercent(NewProcessList);
            UpdateExistingProcesses(NewProcessList);
            AddNewProcesses(NewProcessList);
        }

        private static void UpdateCpuUsagePercent(Process[] NewProcessList)
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

        private static void UpdateExistingProcesses(Process[] NewProcessList)
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
                    CallProcessClose(TempProcess);
                else
                {
                    TempProcessList[ProcessIndex++] = GetProcessInfo(TempProcess, CurrentProcess);
                    CallProcessUpdate(TempProcess);
                }
            }

            ProcessList = TempProcessList;
        }

        private static Process ProcessExists(Process[] NewProcessList, int ID)
        {
            // checks to see if we already loaded the process
            foreach (Process TempProcess in NewProcessList)
                if (TempProcess.Id == ID)
                    return TempProcess;

            return CLOSED_PROCESS;
        }

        private static ProcessInfo GetProcessInfo(ProcessInfo TempProcess, Process CurrentProcess)
        {
            // gets the process name , id, and cpu usage
            if (CurrentProcess.Id == 0)
                TempProcess.CpuUsage = (TotalCpuUsageValue).ToString("F", ValueFormat);
            else
            {
                long NewCpuUsage = (long)CurrentProcess.TotalProcessorTime.TotalMilliseconds;

                TempProcess.CpuUsage = ((NewCpuUsage - TempProcess.OldCpuUsage) / CpuUsagePercent).ToString("F", ValueFormat);
                TempProcess.OldCpuUsage = NewCpuUsage;
            }

            return TempProcess;
        }

        private static void AddNewProcesses(Process[] NewProcessList)
        {
            // loads a new processes
            foreach (Process NewProcess in NewProcessList)
                if (!ProcessInfoExists(NewProcess))
                    AddNewProcess(NewProcess);
        }

        private static bool ProcessInfoExists(Process NewProcess)
        {
            // checks if the process info is already loaded
            if (ProcessList == null) return false;

            foreach (ProcessInfo TempProcess in ProcessList)
                if (TempProcess != PROCESS_INFO_NOT_FOUND && TempProcess.ID == NewProcess.Id)
                    return true;

            return false;
        }

        private static ProcessInfo ProcessInfoByID(int ID)
        {
            // gets the process info by it's id
            if (ProcessList == null) return PROCESS_INFO_NOT_FOUND;

            for (int i = 0; i < ProcessList.Length; i++)
                if (ProcessList[i] != PROCESS_INFO_NOT_FOUND && ProcessList[i].ID == ID)
                    return ProcessList[i];

            return PROCESS_INFO_NOT_FOUND;

        }

        private static void AddNewProcess(Process NewProcess)
        {
            // loads a new process
            ProcessInfo NewProcessInfo = new ProcessInfo();

            NewProcessInfo.Name = NewProcess.ProcessName;
            NewProcessInfo.ID = NewProcess.Id;

            ProcessList[ProcessIndex++] = GetProcessInfo(NewProcessInfo, NewProcess);
            CallNewProcess(NewProcessInfo);
        }
    }

    // holds the process info
    public class ProcessInfo
    {
        public string Name;
        public string CpuUsage;
        public int ID;
        public long OldCpuUsage;
    }
}
