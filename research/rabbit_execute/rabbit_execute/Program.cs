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
                        var str = Newtonsoft.Json.JsonConvert.SerializeObject(args);
                        channel.QueueDeclare(queue: strKey, durable: false, exclusive: false, autoDelete: false, arguments: null);
                        channel.BasicPublish("", strKey, null, Encoding.Unicode.GetBytes(str));
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

                        Console.WriteLine("Host start ....");
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
                Console.WriteLine("Execute_Received");

                var lst = Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(str);

                foreach (var item in lst)
                {
                    if (System.IO.Path.GetExtension(item) == ".exe")
                    {
                        Process.Start(item);
                        Console.WriteLine("open " + item);
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
    }
}
