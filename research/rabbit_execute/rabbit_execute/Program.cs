using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics;

namespace rabbit_execute
{
    class Program
    {
        private static string _RabbitHost = "";
        private static int _RabbitPort = 0;
        private static string _RabbitUserName = "";
        private static string _RabbitPassword = "";

        static void Main(string[] args)
        {
            try
            {
                _RabbitHost = System.Configuration.ConfigurationSettings.AppSettings.Get("RabbitHost");
                _RabbitPort = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings.Get("RabbitPort"));
                _RabbitUserName = System.Configuration.ConfigurationSettings.AppSettings.Get("RabbitUserName");
                _RabbitPassword = System.Configuration.ConfigurationSettings.AppSettings.Get("RabbitPassword");

                string strKey = "rabbit_execute";

                if (args != null && args.Length > 0)
                {
                    var factory = new ConnectionFactory() { HostName = _RabbitHost, Port = _RabbitPort, UserName = _RabbitUserName, Password = _RabbitPassword };
                    using (var connection = factory.CreateConnection())
                    using (var channel = connection.CreateModel())
                    {
                        var lst = new List<string>();
                        foreach (var item in args)
                        {
                            string s = item;
                            if (s.StartsWith("\""))
                                s = s.Substring(1);
                            if (s[s.Length - 1] == '"')
                                s = s.Substring(0, s.Length - 1);
                            lst.Add(s);
                        }

                        var str = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                        channel.QueueDeclare(queue: strKey, durable: false, exclusive: false, autoDelete: false, arguments: null);
                        channel.BasicPublish("", strKey, null, Encoding.Unicode.GetBytes(str));

                        LogData(str);
                    }
                }
                else
                {
                    var factory = new ConnectionFactory() { HostName = _RabbitHost, Port = _RabbitPort, UserName = _RabbitUserName, Password = _RabbitPassword };
                    using (var connection = factory.CreateConnection())
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: strKey, durable: false, exclusive: false, autoDelete: false, arguments: null);
                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += Execute_Received;
                        channel.BasicConsume(queue: strKey, autoAck: true, consumer: consumer);

                        LogData("Host start ....");
                        Console.ReadLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }

        protected static void Execute_Received(object sender, BasicDeliverEventArgs e)
        {
            string strKeyStart = string.Empty;
            try
            {
                var body = e.Body;
                string str = Encoding.Unicode.GetString(body);
                LogData("Execute_Received", true);

                var lst = Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(str);

                foreach (var item in lst)
                {
                    if (System.IO.Path.GetExtension(item) == ".exe")
                    {
                        Process.Start(item);
                        LogData("open " + item);
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }

        static int logline = 0;

        private static void LogData(string str, bool hasdate = false)
        {
            if (hasdate)
            {
                if (logline > 1200)
                {
                    Console.Clear();
                    logline = 0;
                }
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss ") + str);
            }
            else
                Console.WriteLine(str);
            logline++;
        }
    }
}
