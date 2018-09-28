using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using BusinessBackground;

namespace WindowsBackground
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        private Timer _timer = null;
        private Timer _timerReset = null;
        private int _reset = 0;
        private bool _islog = false;
        private string _folderpath = string.Empty;
        private string _datapath = string.Empty;
        private string _logpath = string.Empty;

        protected override void OnStart(string[] args)
        {
            try
            {
                _folderpath = System.Configuration.ConfigurationSettings.AppSettings.Get("folder");
                _datapath = System.Configuration.ConfigurationSettings.AppSettings.Get("data");
                _logpath = System.Configuration.ConfigurationSettings.AppSettings.Get("log");
                string timer = System.Configuration.ConfigurationSettings.AppSettings.Get("timer");
                //System.IO.File.AppendAllLines(@"C:\testlog.txt", new List<string> { "start" });
                if (System.IO.File.Exists(_datapath) && System.IO.Directory.Exists(_folderpath) && !string.IsNullOrEmpty(_logpath) && !string.IsNullOrEmpty(timer))
                {
                    //System.IO.File.AppendAllLines(@"C:\testlog.txt", new List<string> { "passparam" });

                    int intTimer = Convert.ToInt32(timer);

                    Business.SetFolderPath(_folderpath);
                    Business.ReadFile(_datapath);
                    if (Business.Data != null)
                        _islog = Business.Data.IsLog;

                    //System.IO.File.AppendAllLines(@"C:\testlog.txt", new List<string> { "pass read file" });

                    LogInfo("Start service");

                    _timer = new Timer(intTimer);
                    _timer.Elapsed += Timer_Elapsed;
                    _timer.Enabled = true;

                    _timerReset = new Timer(60000);//1p reset                
                    _timerReset.Elapsed += TimerReset_Elapsed;
                    _timerReset.Enabled = false;
                }
                else
                    throw new Exception("fail");
            }
            catch (Exception ex)
            {
                //System.IO.File.AppendAllLines(@"C:\testlog.txt", new List<string> { ex.Message });
                //System.IO.File.AppendAllLines(@"C:\testlog.txt", new List<string> { ex.StackTrace });
                throw ex;
            }
        }

        protected void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                _timer.Enabled = false;

                LogInfo("Run task");
                Business.RunData((string str) =>
                {
                    LogInfo(str);
                });
                LogInfo("End task");

                Business.ReadFile(_datapath);
                if (Business.Data != null)
                    _islog = Business.Data.IsLog;

                _timer.Enabled = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                _timerReset.Enabled = true;
            }
        }

        protected void TimerReset_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                _timerReset.Enabled = false;

                LogInfo("Reset service (" + _reset + ")");
                _reset++;

                _timer.Enabled = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        private void LogInfo(string message)
        {
            if (_islog && !string.IsNullOrEmpty(message))
            {
                //log4net.LogicalThreadContext.Properties["Reset"] = _reset + "";
                //log4net.LogicalThreadContext.Properties["Date"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff");
                //log4net.LogicalThreadContext.Properties["DateTicks"] = DateTime.Now.Ticks.ToString();
                //log4net.LogicalThreadContext.Properties["StackTrace"] = "";
                //log.Info(message);

                string str = string.Format("]-[{0}]-[{1}]-[{2}]-[{3}]-[", _reset, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), DateTime.Now.Ticks.ToString(), message);
                System.IO.File.AppendAllLines(_logpath, new List<string> { str });
            }
        }

        private void LogError(Exception ex)
        {
            if (_islog && ex != null)
            {
                //log4net.LogicalThreadContext.Properties["Reset"] = _reset + "";
                //log4net.LogicalThreadContext.Properties["Date"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff");
                //log4net.LogicalThreadContext.Properties["DateTicks"] = DateTime.Now.Ticks.ToString();
                //log4net.LogicalThreadContext.Properties["StackTrace"] = ex.StackTrace;
                //log.Error(ex.Message);
                string str = string.Format("]-[{0}]-[{1}]-[{2}]-[{3}]-[{4}]-[", _reset, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), DateTime.Now.Ticks.ToString(), "", ex.StackTrace);
                System.IO.File.AppendAllLines(_logpath, new List<string> { str });
            }
        }

        protected override void OnStop()
        {
            try
            {
                if (_timer != null)
                    _timer.Enabled = false;
                if (_timerReset != null)
                    _timerReset.Enabled = false;
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }
    }
}
