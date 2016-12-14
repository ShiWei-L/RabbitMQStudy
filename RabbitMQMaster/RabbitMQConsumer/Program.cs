using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

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
                //不再使用直接拉取的方式
                //var result = channel.BasicGet("firstTest", true);
                //var msg = Encoding.UTF8.GetString(result.Body);
                // Console.WriteLine(msg);

                //使用订阅的方式
                //这里的创建队列,是为了防止 消费 在 生产 之前
                channel.QueueDeclare("firstTest", true, false, false, null);
                //绑定队列 
                channel.ExchangeDeclare("firstExchange", "direct", true, false, null);
                channel.QueueBind("firstTest", "firstExchange", "firstExchange_Demo_firstTest", null);



                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (sender, e) =>
                {
                    var msg = Encoding.UTF8.GetString(e.Body);

                    Console.WriteLine(msg);
                };

                //进行消费
                channel.BasicConsume("firstTest", true, consumer);

                Console.ReadKey();

            }
        }
    }
}
