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
        private InfoData _dto = default(InfoData);
        private InfoData _dto1 = default(InfoData);
        private InfoData _dto2 = default(InfoData);
        private InfoData _dto3 = default(InfoData);
        private System.Timers.Timer _timerGet = null;
        private System.Timers.Timer _timerGetReset = null;
        private System.Timers.Timer _timerSend = null;
        private System.Timers.Timer _timerSendReset = null;
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
                var timersend = System.Configuration.ConfigurationManager.AppSettings.Get("TimerSend");
                var logdata = System.Configuration.ConfigurationManager.AppSettings.Get("LogData");
                _servername = System.Configuration.ConfigurationManager.AppSettings.Get("ServerName");

                var rabbitHost = System.Configuration.ConfigurationManager.AppSettings.Get("RabbitHost");
                var rabbitPort = System.Configuration.ConfigurationManager.AppSettings.Get("RabbitPort");
                var rabbitUserName = System.Configuration.ConfigurationManager.AppSettings.Get("RabbitUserName");
                var rabbitPassword = System.Configuration.ConfigurationManager.AppSettings.Get("RabbitPassword");
                var rabbitKey = System.Configuration.ConfigurationManager.AppSettings.Get("RabbitKey");

                var apiLink = System.Configuration.ConfigurationManager.AppSettings.Get("APILink");
                var apiParam = System.Configuration.ConfigurationManager.AppSettings.Get("APIParam");

                if (!string.IsNullOrEmpty(timersend) && !string.IsNullOrEmpty(logdata) && !string.IsNullOrEmpty(_servername))
                {
                    int i = Convert.ToInt32(timersend);
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

                        _dto = GetInfo();

                        LogInfo("Start service (" + _servername + ")");
                        _timerGet = new System.Timers.Timer(2000);//2s
                        _timerGet.Elapsed += TimerGet_Elapsed;
                        _timerGet.Enabled = true;
                        _timerGetReset = new System.Timers.Timer(600000);//10p reset
                        _timerGetReset.Enabled = false;
                        _timerGetReset.Elapsed += TimerGetReset_Elapsed;

                        _timerSend = new System.Timers.Timer(i);
                        _timerSend.Elapsed += TimerGet_Elapsed;
                        _timerSend.Enabled = true;
                        _timerSendReset = new System.Timers.Timer(600000);//10p reset
                        _timerSendReset.Enabled = false;
                        _timerSendReset.Elapsed += TimerGetReset_Elapsed;
                    }
                    else
                        throw new Exception("TimerSend fail");
                }
                else
                    throw new Exception("Config fail");
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        protected void TimerGet_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                _timerGet.Enabled = false;

                _dto = GetInfo();

                _timerGet.Enabled = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                _timerGetReset.Enabled = true;
            }
        }

        protected void TimerGetReset_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                _timerGetReset.Enabled = false;

                LogInfo("Reset service (" + _reset + ")");
                _reset++;

                _timerGet.Enabled = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        protected void TimerSend_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                _timerSend.Enabled = false;

                if (!string.IsNullOrEmpty(_rabbitHost) && !string.IsNullOrEmpty(_rabbitKey) && _rabbitPort > 0)
                {
                    var factory = new ConnectionFactory() { HostName = _rabbitHost, Port = _rabbitPort, UserName = _rabbitUserName, Password = _rabbitPassword };
                    using (var connection = factory.CreateConnection())
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: _rabbitKey, durable: false, exclusive: false, autoDelete: false, arguments: null);
                        string str = Newtonsoft.Json.JsonConvert.SerializeObject(_dto);
                        channel.BasicPublish("", _rabbitKey, null, Encoding.Unicode.GetBytes(str));
                    }
                }

                if (!string.IsNullOrEmpty(_apiLink))
                {
                    string s = APICall(_dto).Result;
                }

                _timerSend.Enabled = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                _timerSendReset.Enabled = true;
            }
        }

        protected void TimerSendReset_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                _timerSendReset.Enabled = false;

                LogInfo("Reset service (" + _reset + ")");
                _reset++;

                _timerSend.Enabled = true;
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
                if (_timerGet != null)
                    _timerGet.Enabled = false;
                if (_timerGetReset != null)
                    _timerGetReset.Enabled = false;
                if (_timerSend != null)
                    _timerSend.Enabled = false;
                if (_timerSendReset != null)
                    _timerSendReset.Enabled = false;

                _timerGet = null;
                _timerGetReset = null;
                _timerSend = null;
                _timerSendReset = null;
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
                if (_islog)
                {
                    log4net.LogicalThreadContext.Properties["Reset"] = _reset + "";
                    log4net.LogicalThreadContext.Properties["Date"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff");
                    log4net.LogicalThreadContext.Properties["DateTicks"] = DateTime.Now.Ticks.ToString();
                    log4net.LogicalThreadContext.Properties["StackTrace"] = "";
                    log.Info(message);
                }
            }
        }

        private void LogError(Exception ex)
        {
            if (ex != null)
            {
                log4net.LogicalThreadContext.Properties["Reset"] = _reset + "";
                log4net.LogicalThreadContext.Properties["Date"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff");
                log4net.LogicalThreadContext.Properties["DateTicks"] = DateTime.Now.Ticks.ToString();
                log4net.LogicalThreadContext.Properties["StackTrace"] = Newtonsoft.Json.JsonConvert.SerializeObject(ex);
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
            result.RAMFreeMB = ramMBUsage.NextValue();
            result.HDDFreePercent = hddPerUsage.NextValue();
            result.HDDFreeMB = hddMBUsage.NextValue();
            result.HDDHigh = false;
            result.RAMHigh = false;
            result.CPUHigh = false;
            PerformanceCounter.CloseSharedResources();

            if (_dto2 != null)
            {
                _dto1 = new InfoData
                {
                    ServerName = _dto2.ServerName,
                    RabbitDate = _dto2.RabbitDate,
                    CPUUsagePercent = _dto2.CPUUsagePercent,
                    RAMFreeMB = _dto2.RAMFreeMB,
                    HDDFreePercent = _dto2.HDDFreePercent,
                    HDDFreeMB = _dto2.HDDFreeMB,
                    HDDHigh = _dto2.HDDHigh,
                    RAMHigh = _dto2.RAMHigh,
                    CPUHigh = _dto2.CPUHigh
                };
            }
            if (_dto3 != null)
            {
                _dto2 = new InfoData
                {
                    ServerName = _dto3.ServerName,
                    RabbitDate = _dto3.RabbitDate,
                    CPUUsagePercent = _dto3.CPUUsagePercent,
                    RAMFreeMB = _dto3.RAMFreeMB,
                    HDDFreePercent = _dto3.HDDFreePercent,
                    HDDFreeMB = _dto3.HDDFreeMB,
                    HDDHigh = _dto3.HDDHigh,
                    RAMHigh = _dto3.RAMHigh,
                    CPUHigh = _dto3.CPUHigh
                };
            }
            if (_dto != null)
            {
                _dto3 = new InfoData
                {
                    ServerName = _dto.ServerName,
                    RabbitDate = _dto.RabbitDate,
                    CPUUsagePercent = _dto.CPUUsagePercent,
                    RAMFreeMB = _dto.RAMFreeMB,
                    HDDFreePercent = _dto.HDDFreePercent,
                    HDDFreeMB = _dto.HDDFreeMB,
                    HDDHigh = _dto.HDDHigh,
                    RAMHigh = _dto.RAMHigh,
                    CPUHigh = _dto.CPUHigh
                };
            }
            if (_dto3 != null && _dto2 != null)
            {
                int cpu = 0;
                if (_dto2.CPUUsagePercent > 90)
                    cpu++;
                else if (_dto2.CPUUsagePercent > 85)
                    cpu += 2;
                if (_dto3.CPUUsagePercent > 90)
                    cpu++;
                else if (_dto3.CPUUsagePercent > 85)
                    cpu += 2;
                if (result.CPUUsagePercent > 90)
                    cpu++;
                else if (result.CPUUsagePercent > 85)
                    cpu += 2;
                result.CPUHigh = cpu >= 5;

                int ram = 0;
                if (_dto2.RAMFreeMB > 500)
                    ram++;
                else if (_dto2.RAMFreeMB > 200)
                    ram += 2;
                if (_dto3.RAMFreeMB > 500)
                    ram++;
                else if (_dto3.RAMFreeMB > 200)
                    ram += 2;
                if (result.RAMFreeMB > 500)
                    ram++;
                else if (result.RAMFreeMB > 200)
                    ram += 2;
                result.RAMHigh = ram >= 5;

                int hdd = 0;
                if (_dto2.HDDFreePercent > 10)
                    hdd++;
                else if (_dto2.HDDFreePercent > 5)
                    hdd += 2;
                if (_dto3.HDDFreePercent > 10)
                    hdd++;
                else if (_dto3.HDDFreePercent > 5)
                    hdd += 2;
                if (result.HDDFreePercent > 10)
                    hdd++;
                else if (result.HDDFreePercent > 5)
                    hdd += 2;
                result.HDDHigh = hdd >= 5;
            }

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
