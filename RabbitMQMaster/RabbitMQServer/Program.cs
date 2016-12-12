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
                //使用默认的交换机

                //声明队列
                channel.QueueDeclare("firstTest", true, false, false, null);
                //发布消息
                var msg = Encoding.UTF8.GetBytes("Hello RabbitMQ");
                channel.BasicPublish(string.Empty, routingKey: "firstTest", basicProperties: null, body: msg);
            }

        }
    }
}
    