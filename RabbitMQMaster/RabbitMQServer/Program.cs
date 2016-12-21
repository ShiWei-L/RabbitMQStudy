using RabbitMQ.Client;
using System;
using System.Text;
using RabbitMQHelper;
using System.Collections.Generic;

namespace RabbitMQServer
{
    class Program
    {


        static void Main(string[] args)
        {
            var flag = true;
            while (flag)
            {

                Console.WriteLine("请输入要发布的消息 key|msg。 或者按Ctrl+ C退出");

                var msg = Console.ReadLine();

                //创建返回一个新的频道
                using (var channel = RabbitMqHelper.GetConnection().CreateModel())
                {

                    var msgs = msg.Split('|');

                    //发布一个消息
                    var msgbody = Encoding.UTF8.GetBytes(msgs[1]);

                    channel.BasicPublish("TopicExchange", routingKey: string.Empty, basicProperties: null, body: msgbody);

                    Console.Write("发布成功！");

                }
            }

            Console.ReadKey();

        }


        #region headers 模式
        //static void Main(string[] args)
        //{
        //    //创建返回一个新的频道
        //    using (var channel = RabbitMqHelper.GetConnection().CreateModel())
        //    {

        //        //创建properties
        //        var properties = channel.CreateBasicProperties();

        //        //往内容的headers中塞入值 
        //        properties.Headers = new Dictionary<string, object>()
        //        {
        //            {"user","admin" },
        //            {"pwd","123456" }
        //        };


        //        //发布一个消息
        //        var msg = Encoding.UTF8.GetBytes($"二狗子");

        //        channel.BasicPublish("headersExchange", routingKey: string.Empty, basicProperties: properties, body: msg);

        //        Console.Write("发布成功！");

        //    }

        //    Console.ReadKey();

        //}
        #endregion



        #region fanout模式
        //static void Main(string[] args)
        //{
        //    //创建返回一个新的频道
        //    using (var channel = RabbitMqHelper.GetConnection().CreateModel())
        //    {

        //        //发布一个消息
        //        var msg = Encoding.UTF8.GetBytes($"二狗子");
        //        //不需要指定routingkey,指定了也没用.因为交换机是fanout类型
        //        channel.BasicPublish("fanoutExchange", routingKey: string.Empty, basicProperties: null, body: msg);

        //        Console.Write("发布成功！");

        //    }

        //    Console.ReadKey();

        //} 
        #endregion


        #region 日志消息发布者  direct模式根据routngkey
        //static void Main(string[] args)
        //{
        //    //创建返回一个新的频道
        //    using (var channel = RabbitMqHelper.GetConnection().CreateModel())
        //    {
        //        //发布一百个消息
        //        for (var i = 0; i < 100; i++)
        //        {
        //            //对i进行求余来决定日志的级别
        //            var routingkey = i % 2 == 0 ? "info" : i % 3 == 0 ? "debug" : "error";
        //            var msg = Encoding.UTF8.GetBytes($"{i} :{routingkey}Message");
        //            channel.BasicPublish("LogExchange", routingKey: routingkey, basicProperties: null, body: msg);
        //        }

        //        Console.Write("发布成功！");

        //    }

        //    Console.ReadKey();

        //}
        #endregion



        #region 第一版与第二版 发布多条消息到交换机
        //static void Main(string[] args)
        //{
        //    //从工厂中拿到实例 本地host、用户admin
        //    var factory = new ConnectionFactory()
        //    {
        //        UserName = "admin",
        //        Password = "admin",
        //        HostName = "localhost"
        //    };

        //    //创建连接
        //    using (var connection = factory.CreateConnection())
        //    //创建返回一个新的频道
        //    using (var channel = connection.CreateModel())
        //    {
        //        //声明一个direct类型的交换机
        //        channel.ExchangeDeclare("firstExchange", "direct", true, false, null);


        //        //声明队列
        //        channel.QueueDeclare("firstTest", true, false, false, null);


        //        //绑定队列
        //        channel.QueueBind("firstTest", "firstExchange", "firstExchange_Demo_firstTest", null);


        //        //发布一百个消息

        //        for (var i = 0; i < 100; i++)
        //        {
        //            var msg = Encoding.UTF8.GetBytes($"{i} :Hello RabbitMQ");
        //            channel.BasicPublish("firstExchange", routingKey: "firstExchange_Demo_firstTest", basicProperties: null, body: msg);
        //        }

        //        Console.Write("发布成功！");

        //    }

        //    Console.ReadKey();

        //} 
        #endregion



    }
}
