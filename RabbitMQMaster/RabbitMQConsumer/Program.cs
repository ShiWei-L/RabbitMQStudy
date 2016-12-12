using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
namespace RabbitMQConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            //consumber端
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "admin",
                Password = "admin"
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                //获取消息
                var result = channel.BasicGet("firstTest", true);

                var msg = Encoding.UTF8.GetString(result.Body);
                //打印消息
                Console.WriteLine(msg);

            }
        }
    }
}
