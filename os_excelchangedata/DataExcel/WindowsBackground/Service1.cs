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

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Timer _timer = null;
        private Timer _timerReset = null;
        private int _reset = 0;
        private bool _islog = false;
        private string _datapath = string.Empty;

        protected override void OnStart(string[] args)
        {
            try
            {
                _datapath = System.Configuration.ConfigurationSettings.AppSettings.Get("data");
                if (System.IO.File.Exists(_datapath))
                {
                    Business.ReadFile(_datapath);
                    if (Business.Data != null)
                        _islog = Business.Data.IsLog;

                    _timer = new Timer(30000);
                    _timer.Elapsed += Timer_Elapsed;
                    _timer.Enabled = true;

                    _timerReset = new Timer(60000);//1p reset                
                    _timerReset.Elapsed += TimerReset_Elapsed;
                    _timerReset.Enabled = false;
                }
                else
                    this.Stop();
            }
            catch (Exception ex)
            {
                LogError(ex);
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
                log4net.LogicalThreadContext.Properties["Reset"] = _reset + "";
                log4net.LogicalThreadContext.Properties["Date"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff");
                log4net.LogicalThreadContext.Properties["DateTicks"] = DateTime.Now.Ticks.ToString();
                log4net.LogicalThreadContext.Properties["StackTrace"] = "";
                log.Info(message);
            }
        }

        private void LogError(Exception ex)
        {
            if (_islog && ex != null)
            {
                log4net.LogicalThreadContext.Properties["Reset"] = _reset + "";
                log4net.LogicalThreadContext.Properties["Date"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff");
                log4net.LogicalThreadContext.Properties["DateTicks"] = DateTime.Now.Ticks.ToString();
                log4net.LogicalThreadContext.Properties["StackTrace"] = ex.StackTrace;
                log.Error(ex.Message);
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
