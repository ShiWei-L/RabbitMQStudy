using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //从工厂中拿到实例 本地host、用户admin
            var factory = new ConnectionFactory()
            {
                UserName = "admin",
                Password = "admin",
                HostName = "localhost"
            };

            //创建连接
            using (var connection = factory.CreateConnection())
            //创建返回一个新的频道
            using (var channel = connection.CreateModel())
            {
                //声明一个direct类型的交换机
                channel.ExchangeDeclare("firstExchange", "direct", true, false, null);


                //声明队列
                channel.QueueDeclare("firstTest", true, false, false, null);


                //绑定队列
                channel.QueueBind("firstTest", "firstExchange", "firstExchange_Demo_firstTest", null);


                //发布一百个消息

                for (var i = 0; i < 100; i++)
                {
                    var msg = Encoding.UTF8.GetBytes($"{i} :Hello RabbitMQ");
                    channel.BasicPublish("firstExchange", routingKey: "firstExchange_Demo_firstTest", basicProperties: null, body: msg);
                }

                Console.Write("发布成功！");

            }

            Console.ReadKey();

        }
    }
}
