using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace service_performance
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //private static List<RabbitServerInfo> _lst = new List<RabbitServerInfo>();
        private System.Timers.Timer _timer = null;
        private System.Timers.Timer _timerReset = null;
        private int _reset = 0;
        private bool _islog = false;
        private string _servername = string.Empty;
        private string _rabbitHost = string.Empty;
        private int _rabbitPort = 0;
        private string _rabbitUserName = string.Empty;
        private string _rabbitPassword = string.Empty;
        private string _rabbitKey = string.Empty;
        private string _apiLink = string.Empty;
        private string _apiParam = string.Empty;

        protected override void OnStart(string[] args)
        {
            try
            {
                var timerget = System.Configuration.ConfigurationManager.AppSettings.Get("TimerGet");
                var logdata = System.Configuration.ConfigurationManager.AppSettings.Get("LogData");
                _servername = System.Configuration.ConfigurationManager.AppSettings.Get("ServerName");

                var rabbitHost = System.Configuration.ConfigurationManager.AppSettings.Get("RabbitHost");
                var rabbitPort = System.Configuration.ConfigurationManager.AppSettings.Get("RabbitPort");
                var rabbitUserName = System.Configuration.ConfigurationManager.AppSettings.Get("RabbitUserName");
                var rabbitPassword = System.Configuration.ConfigurationManager.AppSettings.Get("RabbitPassword");
                var rabbitKey = System.Configuration.ConfigurationManager.AppSettings.Get("RabbitKey");

                var apiLink = System.Configuration.ConfigurationManager.AppSettings.Get("APILink");
                var apiParam = System.Configuration.ConfigurationManager.AppSettings.Get("APIParam");

                if (!string.IsNullOrEmpty(timerget) && !string.IsNullOrEmpty(logdata) && !string.IsNullOrEmpty(_servername))
                {
                    int i = Convert.ToInt32(timerget);
                    if (i > 0)
                    {
                        _islog = logdata == "true";
                        if (!string.IsNullOrEmpty(rabbitPort))
                            _rabbitPort = Convert.ToInt32(rabbitPort);
                        _rabbitHost = rabbitHost;
                        _rabbitUserName = rabbitUserName;
                        _rabbitPassword = rabbitPassword;
                        _rabbitKey = rabbitKey;

                        _apiLink = apiLink;
                        _apiParam = apiParam;

                        LogInfo("Start service (" + _servername + ")");
                        _timer = new System.Timers.Timer(i);
                        _timer.Elapsed += Timer_Elapsed;
                        _timer.Enabled = true;

                        _timerReset = new System.Timers.Timer(600000);//10p reset
                        _timerReset.Enabled = false;
                        _timerReset.Elapsed += TimerReset_Elapsed;
                    }
                }

                //if (!string.IsNullOrEmpty(rabbitHost) && !string.IsNullOrEmpty(rabbitPort) && !string.IsNullOrEmpty(rabbitUserName) && !string.IsNullOrEmpty(rabbitPassword) && !string.IsNullOrEmpty(rabbitKey))
                //{
                //    var splitHost = rabbitHost.Split(',');
                //    var splitPort = rabbitPort.Split(',');
                //    var splitUserName = rabbitUserName.Split(',');
                //    var splitPassword = rabbitPassword.Split(',');
                //    var splitKey = rabbitKey.Split(',');

                //    if (splitHost.Length > 0 && splitHost.Length == splitPort.Length && splitHost.Length == splitUserName.Length && splitHost.Length == splitPassword.Length && splitHost.Length == splitKey.Length)
                //    {
                //        LogInfo("Lấy dữ liệu thiết lập");
                //        for (int i = 0; i < splitHost.Length; i++)
                //        {
                //            var strHost = splitHost[i];
                //            var strPort = splitPort[i];
                //            var strUserName = splitUserName[i];
                //            var strPassword = splitPassword[i];
                //            var strKey = splitKey[i];

                //            if (!string.IsNullOrEmpty(strHost) && !string.IsNullOrEmpty(strPort) && !string.IsNullOrEmpty(strUserName) && !string.IsNullOrEmpty(strPassword) && !string.IsNullOrEmpty(strKey))
                //            {
                //                var item = new RabbitServerInfo
                //                {
                //                    Host = strHost,
                //                    Port = Convert.ToInt32(strPort),
                //                    UserName = strUserName,
                //                    Password = strPassword,
                //                    Key = strKey
                //                };
                //                _lst.Add(item);
                //            }
                //        }
                //        if (_lst.Count > 0)
                //        {
                //            int timerupdate = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings.Get("TimerUpdate"));
                //            LogInfo("Start service (" + _lst.Count + ")");
                //            _timer = new Timer(timerupdate);
                //            _timer.Elapsed += Timer_Elapsed;
                //            _timer.Enabled = true;

                //            _timerReset = new Timer(600000);//10p reset
                //            _timerReset.Enabled = false;
                //            _timerReset.Elapsed += TimerReset_Elapsed;
                //        }
                //        else
                //            throw new Exception("Không lấy được danh sách");
                //    }
                //    else
                //        throw new Exception("Dữ liệu thiết lập không đúng");
                //}
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        protected void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                _timer.Enabled = false;

                var dto = GetInfo();

                if (!string.IsNullOrEmpty(_rabbitHost) && !string.IsNullOrEmpty(_rabbitKey) && _rabbitPort > 0)
                {
                    var factory = new ConnectionFactory() { HostName = _rabbitHost, Port = _rabbitPort, UserName = _rabbitUserName, Password = _rabbitPassword };
                    using (var connection = factory.CreateConnection())
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: _rabbitKey, durable: false, exclusive: false, autoDelete: false, arguments: null);
                        string str = Newtonsoft.Json.JsonConvert.SerializeObject(dto);
                        channel.BasicPublish("", _rabbitKey, null, Encoding.Unicode.GetBytes(str));
                    }
                }

                if (!string.IsNullOrEmpty(_apiLink))
                {
                    string s = APICall(dto).Result;
                }

                _timer.Enabled = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                _timerReset.Enabled = true;
            }
        }

        protected void TimerReset_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
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

        protected override void OnStop()
        {
            try
            {
                if (_timer != null)
                    _timer.Enabled = false;
                if (_timerReset != null)
                    _timerReset.Enabled = false;

                _timer = null;
                _timerReset = null;
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        private void LogInfo(string message)
        {
            if (!string.IsNullOrEmpty(message))
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
            if (ex != null)
            {
                log4net.LogicalThreadContext.Properties["Reset"] = _reset + "";
                log4net.LogicalThreadContext.Properties["Date"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff");
                log4net.LogicalThreadContext.Properties["DateTicks"] = DateTime.Now.Ticks.ToString();
                log4net.LogicalThreadContext.Properties["StackTrace"] = ex.StackTrace;
                log.Error(ex.Message);
            }
        }

        private InfoData GetInfo()
        {
            var result = new InfoData();
            var cpuUsage = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            var ramMBUsage = new PerformanceCounter("Memory", "Available MBytes");
            var hddPerUsage = new PerformanceCounter("LogicalDisk", "% Free Space", "_Total", true);
            var hddMBUsage = new PerformanceCounter("LogicalDisk", "Free Megabytes", "_Total", true);

            result.ServerName = _servername;
            result.RabbitDate = DateTime.Now;
            result.CPUUsagePercent = cpuUsage.NextValue();
            result.CPUUsagePercent = cpuUsage.NextValue();
            result.CPUUsagePercent = cpuUsage.NextValue();
            result.CPUUsagePercent = cpuUsage.NextValue();
            PerformanceCounter.CloseSharedResources();

            return result;
        }

        private async Task<string> APICall(InfoData dto)
        {
            bool successStatus = false;
            try
            {
                using (var client = new System.Net.Http.HttpClient())
                {
                    var result = string.Empty;

                    Uri url = new Uri(_apiLink);
                    client.BaseAddress = url;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.Timeout = TimeSpan.FromHours(1);
                    string str = Newtonsoft.Json.JsonConvert.SerializeObject(dto);
                    if (!string.IsNullOrEmpty(_apiParam))
                        str = "{\"" + _apiParam + "\":" + str + "}";
                    System.Net.Http.StringContent content = new System.Net.Http.StringContent(str, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url.AbsolutePath, content);
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        successStatus = true;
                        result = await response.Content.ReadAsStringAsync();
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError || response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        string s = await response.Content.ReadAsStringAsync();
                        throw new Exception(s);
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        throw new Exception("fail Unauthorized");
                    else
                        throw new Exception("fail API Host");
                    return result;
                }
            }
            catch (AggregateException ex)
            {
                // a web request timeout 
                throw new Exception("timeouts: " + successStatus + " : " + Newtonsoft.Json.JsonConvert.SerializeObject(ex));
            }
            catch (TaskCanceledException ex)
            {
                // a web request timeout 
                throw new Exception("timeouts: " + successStatus + " : " + Newtonsoft.Json.JsonConvert.SerializeObject(ex));
            }
            catch (OperationCanceledException ex)
            {
                // because timeouts are still possible
                throw new Exception("timeouts: " + successStatus + " : " + Newtonsoft.Json.JsonConvert.SerializeObject(ex));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
