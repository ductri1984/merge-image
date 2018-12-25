﻿using System;
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
        private PerformanceCounter _cpuUsage = default(PerformanceCounter);
        private InfoData _dto = default(InfoData);
        private List<InfoData> _lstDTO = new List<InfoData>();
        private int _secondCheck = 5;
        private double _totalPhysicalMemory = 0;
        private float _cpuHighPercent = 5;
        private float _ramHighPercent = 5;
        private float _hddHighPercent = 5;
        private System.Timers.Timer _timerGet = null;
        private System.Timers.Timer _timerGetReset = null;
        private System.Timers.Timer _timerSend = null;
        private System.Timers.Timer _timerSendReset = null;
        private int _reset = 0;
        private bool _islog = false;
        private bool _isSendHigh = false;
        private string _servername = string.Empty;
        private string _rabbitHost = string.Empty;
        private int _rabbitPort = 0;
        private string _rabbitUserName = string.Empty;
        private string _rabbitPassword = string.Empty;
        private string _rabbitKey = string.Empty;
        private string _apiLink = string.Empty;
        private string _apiParam = string.Empty;
        private string _batchFileInit = string.Empty;
        private string _batchFileCheck = string.Empty;
        private string _batchFileClear = string.Empty;
        private bool _batchClear = false;
        private bool _batchRun = false;
        private bool _batchComplete = false;
        private string _batchLastOutput = string.Empty;

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

                var cpuHighPercent = System.Configuration.ConfigurationManager.AppSettings.Get("CPUHighPercent");
                var ramHighPercent = System.Configuration.ConfigurationManager.AppSettings.Get("RAMHighPercent");
                var hddHighPercent = System.Configuration.ConfigurationManager.AppSettings.Get("HDDHighPercent");
                var sendHigh = System.Configuration.ConfigurationManager.AppSettings.Get("SendHigh");
                var secondCheck = System.Configuration.ConfigurationManager.AppSettings.Get("SecondCheck");
                _batchFileInit = System.Configuration.ConfigurationManager.AppSettings.Get("BatchInit");
                _batchFileCheck = System.Configuration.ConfigurationManager.AppSettings.Get("BatchCheck");
                _batchFileClear = System.Configuration.ConfigurationManager.AppSettings.Get("BatchClear");

                if (!string.IsNullOrEmpty(timersend) && !string.IsNullOrEmpty(logdata) && !string.IsNullOrEmpty(_servername) && !string.IsNullOrEmpty(sendHigh) && !string.IsNullOrEmpty(secondCheck))
                {
                    int i = Convert.ToInt32(timersend);
                    _secondCheck = Convert.ToInt32(secondCheck);
                    if (i > 0 && _secondCheck > 5)
                    {
                        _lstDTO = new List<InfoData>();
                        _islog = logdata == "true";
                        _isSendHigh = sendHigh == "true";
                        if (!string.IsNullOrEmpty(rabbitPort))
                            _rabbitPort = Convert.ToInt32(rabbitPort);
                        _rabbitHost = rabbitHost;
                        _rabbitUserName = rabbitUserName;
                        _rabbitPassword = rabbitPassword;
                        _rabbitKey = rabbitKey;

                        _apiLink = apiLink;
                        _apiParam = apiParam;

                        _batchClear = false;
                        _batchRun = false;
                        _batchComplete = false;

                        if (!string.IsNullOrEmpty(cpuHighPercent))
                            _cpuHighPercent = Convert.ToSingle(cpuHighPercent);
                        if (!string.IsNullOrEmpty(ramHighPercent))
                            _ramHighPercent = Convert.ToSingle(ramHighPercent);
                        if (!string.IsNullOrEmpty(hddHighPercent))
                            _hddHighPercent = Convert.ToSingle(hddHighPercent);
                        if (_cpuHighPercent < 1 || _ramHighPercent < 1 || _hddHighPercent < 1)
                            throw new Exception("HighPercent fail");

                        Microsoft.VisualBasic.Devices.ComputerInfo ci = new Microsoft.VisualBasic.Devices.ComputerInfo();
                        _totalPhysicalMemory = (ci.TotalPhysicalMemory / 1024) * 0.001;
                        _cpuUsage = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                        _dto = GetInfo();

                        LogInfo("Start service (" + _servername + ")");
                        _timerGet = new System.Timers.Timer(1000);//1s
                        _timerGet.Elapsed += TimerGet_Elapsed;
                        _timerGet.Enabled = true;
                        _timerGetReset = new System.Timers.Timer(600000);//10p reset
                        _timerGetReset.Enabled = false;
                        _timerGetReset.Elapsed += TimerGetReset_Elapsed;

                        _timerSend = new System.Timers.Timer(i);
                        _timerSend.Elapsed += TimerSend_Elapsed;
                        _timerSend.Enabled = true;
                        _timerSendReset = new System.Timers.Timer(600000);//10p reset
                        _timerSendReset.Enabled = false;
                        _timerSendReset.Elapsed += TimerSendReset_Elapsed;
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
                if (_islog)
                    LogInfo("TimerGet: " + Newtonsoft.Json.JsonConvert.SerializeObject(_dto));

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

                if (_islog)
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

                if (_islog)
                    LogInfo("TimerSend start");

                bool flag = true;
                if (_isSendHigh)
                {
                    flag = _dto.CPUHigh || _dto.RAMHigh || _dto.HDDHigh;
                }

                if (_islog)
                    LogInfo("TimerSend data: " + Newtonsoft.Json.JsonConvert.SerializeObject(_dto));

                if (flag)
                {
                    if (!string.IsNullOrEmpty(_rabbitHost) && !string.IsNullOrEmpty(_rabbitKey) && _rabbitPort > 0)
                    {
                        var factory = new ConnectionFactory() { HostName = _rabbitHost, Port = _rabbitPort, UserName = _rabbitUserName, Password = _rabbitPassword };
                        using (var connection = factory.CreateConnection())
                        using (var channel = connection.CreateModel())
                        {
                            channel.QueueDeclare(queue: _rabbitKey, durable: false, exclusive: false, autoDelete: false, arguments: null);
                            string str = Newtonsoft.Json.JsonConvert.SerializeObject(_dto);
                            channel.BasicPublish("", _rabbitKey, null, Encoding.Unicode.GetBytes(str));

                            if (_islog)
                                LogInfo("TimerSend send rabbit");
                        }
                    }

                    if (!string.IsNullOrEmpty(_apiLink))
                    {
                        string s = APICall(_dto).Result;

                        if (_islog)
                            LogInfo("TimerSend send api");
                    }

                    _lstDTO.Clear();
                }
                if (!_batchClear)
                {
                    if (!string.IsNullOrEmpty(_batchFileClear))
                    {
                        if (_islog)
                            LogInfo("batch clear");

                        ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", "/c " + _batchFileClear);
                        processInfo.CreateNoWindow = true;
                        processInfo.UseShellExecute = false;
                        processInfo.RedirectStandardError = true;
                        processInfo.RedirectStandardOutput = true;

                        var process = Process.Start(processInfo);
                        process.OutputDataReceived += (object senderprocess, DataReceivedEventArgs eprocess) =>
                        {
                            if (_islog)
                                LogInfo("output>>" + eprocess.Data);
                        };
                        process.BeginOutputReadLine();

                        process.ErrorDataReceived += (object senderprocess, DataReceivedEventArgs eprocess) =>
                        {
                            if (_islog)
                                LogInfo("error>>" + eprocess.Data);
                        };
                        process.BeginErrorReadLine();
                        process.Start();
                        process.Close();

                        _batchClear = true;
                    }
                }
                else if (!_batchComplete)
                {
                    if (_islog)
                        LogInfo("_batchLastOutput: " + _batchLastOutput);

                    if (!_batchRun)
                    {
                        if (_islog)
                            LogInfo("batch init");

                        if (!string.IsNullOrEmpty(_batchFileInit) && !string.IsNullOrEmpty(_batchFileCheck))
                        {
                            ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", "/c " + _batchFileInit);
                            processInfo.CreateNoWindow = true;
                            processInfo.UseShellExecute = false;
                            processInfo.RedirectStandardError = true;
                            processInfo.RedirectStandardOutput = true;

                            var process = Process.Start(processInfo);
                            process.OutputDataReceived += (object senderprocess, DataReceivedEventArgs eprocess) =>
                            {
                                if (_islog)
                                    LogInfo("output>>" + eprocess.Data);
                            };
                            process.BeginOutputReadLine();

                            process.ErrorDataReceived += (object senderprocess, DataReceivedEventArgs eprocess) =>
                            {
                                if (_islog)
                                    LogInfo("error>>" + eprocess.Data);
                            };
                            process.BeginErrorReadLine();
                            process.Start();
                            process.Close();
                        }

                        _batchLastOutput = string.Empty;
                        _batchRun = true;
                    }
                    else if (!string.IsNullOrEmpty(_batchLastOutput))
                    {
                        if (_islog)
                            LogInfo("batch complete");

                        if (_batchLastOutput == "true")
                        {
                            _batchComplete = true;                            
                        }
                        else
                            throw new Exception("fail batch file");
                    }
                    else
                    {
                        if (_islog)
                            LogInfo("batch check");

                        ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", "/c " + _batchFileCheck);
                        processInfo.CreateNoWindow = true;
                        processInfo.UseShellExecute = false;
                        processInfo.RedirectStandardError = true;
                        processInfo.RedirectStandardOutput = true;

                        var process = Process.Start(processInfo);
                        process.OutputDataReceived += (object senderprocess, DataReceivedEventArgs eprocess) =>
                        {
                            if (_islog)
                                LogInfo("output>>" + eprocess.Data);
                            if (_batchLastOutput != "true")
                                _batchLastOutput = eprocess.Data;
                            if (!string.IsNullOrEmpty(_batchLastOutput))
                                _batchLastOutput = _batchLastOutput.ToLower().Trim();
                        };
                        process.BeginOutputReadLine();

                        process.ErrorDataReceived += (object senderprocess, DataReceivedEventArgs eprocess) =>
                        {
                            if (_islog)
                                LogInfo("error>>" + eprocess.Data);
                        };
                        process.BeginErrorReadLine();
                        process.Start();
                        process.Close();
                    }
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

                if (_islog)
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
                log4net.LogicalThreadContext.Properties["StackTrace"] = Newtonsoft.Json.JsonConvert.SerializeObject(ex);
                log.Error(ex.Message);
            }
        }

        private InfoData GetInfo()
        {
            var result = new InfoData();
            //var cpuUsage = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            var ramMBUsage = new PerformanceCounter("Memory", "Available MBytes");
            var hddPerUsage = new PerformanceCounter("LogicalDisk", "% Free Space", "_Total", true);
            var hddMBUsage = new PerformanceCounter("LogicalDisk", "Free Megabytes", "_Total", true);

            result.ServerName = _servername;
            result.RabbitDate = DateTime.Now;
            result.CPUUsagePercent = _cpuUsage.NextValue();
            result.RAMFreeMB = ramMBUsage.NextValue();
            result.HDDFreePercent = hddPerUsage.NextValue();
            result.HDDFreeMB = hddMBUsage.NextValue();
            result.HDDHigh = false;
            result.RAMHigh = false;
            result.CPUHigh = false;
            PerformanceCounter.CloseSharedResources();

            if (_lstDTO != null)
            {
                if (_lstDTO.Count >= _secondCheck)
                {
                    int cpu = 0;
                    int ram = 0;
                    int hdd = 0;

                    foreach (var item in _lstDTO)
                    {
                        if (item.CPUUsagePercent > _cpuHighPercent)
                            cpu += 2;
                        else if (item.CPUUsagePercent > _cpuHighPercent - 5)
                            cpu++;
                        if (item.RAMFreeMB / (_totalPhysicalMemory / 100) < _ramHighPercent)
                            ram += 2;
                        else if (item.RAMFreeMB / (_totalPhysicalMemory / 100) < _ramHighPercent + 5)
                            ram++;
                        if (item.HDDFreePercent < _hddHighPercent)
                            hdd += 2;
                        else if (item.HDDFreePercent < _hddHighPercent + 5)
                            hdd++;
                    }
                    if (result.CPUUsagePercent > _cpuHighPercent)
                        cpu += 2;
                    else if (result.CPUUsagePercent > _cpuHighPercent - 5)
                        cpu++;
                    if (result.RAMFreeMB / (_totalPhysicalMemory / 100) < _ramHighPercent)
                        ram += 2;
                    else if (result.RAMFreeMB / (_totalPhysicalMemory / 100) < _ramHighPercent + 5)
                        ram++;
                    if (result.HDDFreePercent < _hddHighPercent)
                        hdd += 2;
                    else if (result.HDDFreePercent < _hddHighPercent + 5)
                        hdd++;

                    var check = ((_secondCheck + 1) * 2) - ((_secondCheck + 1) / 4);
                    result.CPUHigh = cpu >= check;
                    result.RAMHigh = ram >= check;
                    result.HDDHigh = hdd >= check;

                    if (_islog)
                        LogInfo(string.Format("CPU:{1}>={0} RAM:{2}>={0} HDD:{3}>={0}", check, cpu, ram, hdd));

                    _lstDTO.RemoveAt(_lstDTO.Count - 1);
                }

                _lstDTO.Insert(0, new InfoData
                {
                    ServerName = result.ServerName,
                    RabbitDate = result.RabbitDate,
                    CPUUsagePercent = result.CPUUsagePercent,
                    RAMFreeMB = result.RAMFreeMB,
                    HDDFreePercent = result.HDDFreePercent,
                    HDDFreeMB = result.HDDFreeMB,
                    HDDHigh = result.HDDHigh,
                    RAMHigh = result.RAMHigh,
                    CPUHigh = result.CPUHigh
                });
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
