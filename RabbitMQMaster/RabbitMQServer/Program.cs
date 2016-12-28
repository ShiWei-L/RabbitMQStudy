using RabbitMQ.Client;
using System;
using System.Text;
using RabbitMQHelper;
using System.Collections.Generic;
using RabbitMQ.Client.MessagePatterns;
using RabbitMQ.Client.Events;
using System.Diagnostics;

namespace RabbitMQServer
{
    class Program
    {


        static void Main(string[] args)
        {

            //创建返回一个新的频道
            using (var channel = RabbitMqHelper.GetConnection().CreateModel())
            {
                var watch = new Stopwatch();
                try
                {
                    watch.Start();
                    //channel.TxSelect();
                    for (var i = 0; i < 10000; i++)
                    {
                        channel.BasicPublish(string.Empty, "testqueue", null, Encoding.UTF8.GetBytes($"这是{i}个消息"));
                    }
                    //channel.TxCommit();
                    //等待发布成功并返回发布状态
                    // bool isok = channel.WaitForConfirms(new TimeSpan(1, 20, 30));
                    watch.Stop();
                    Console.WriteLine($"发布一万条没有消息确认,耗时{watch.ElapsedMilliseconds}毫秒");
                    Console.ReadKey();
                }
                catch (Exception e)
                {
                    //watch.Stop();
                    //Console.WriteLine($"发布一万条使用了Confirm,耗时{watch.ElapsedMilliseconds}毫秒");
                    //回退
                    //channel.TxRollback();
                }

            }

        }

        #region 死信与优先级别
        //static void Main(string[] args)
        //{

        //    //创建返回一个新的频道
        //    using (var channel = RabbitMqHelper.GetConnection().CreateModel())
        //    {

        //        channel.QueueDeclare("priorityQueue", true, false, false, new Dictionary<string, object> { { "x-max-priority", 5 } });

        //        var properties = channel.CreateBasicProperties();

        //        for (var i = 0; i < 6; i++)
        //        {
        //            properties.Priority = (byte)i;
        //            channel.BasicPublish(string.Empty, "priorityQueue", properties, Encoding.UTF8.GetBytes($"{i}级别的消息"));
        //        }

        //        Console.ReadKey();

        //    }

        //} 
        #endregion

        #region dead letter
        //static void Main(string[] args)
        //{

        //    //创建返回一个新的频道
        //    using (var channel = RabbitMqHelper.GetConnection().CreateModel())
        //    {

        //        channel.BasicPublish(string.Empty, "testqueue", null, Encoding.UTF8.GetBytes("我五秒后就会消失"));

        //        Console.ReadKey();

        //    }

        //} 
        #endregion


        #region 声明queue的参数们的作用
        //static void Main(string[] args)
        //{

        //    //创建返回一个新的频道
        //    using (var channel = RabbitMqHelper.GetConnection().CreateModel())
        //    {

        //        //声明一个queue，最大长度10，最大大小2048bytes
        //        channel.QueueDeclare("queue", true, false, false, new Dictionary<string, object>
        //        {
        //            { "x-max-length", 10 },
        //            { "x-max-length-bytes", 2048}
        //        });


        //        //声明一个queue，queue五秒内而且未被任何形式的消费,则被删除
        //        channel.QueueDeclare("queue", true, false, false, new Dictionary<string, object> { { "x-expires", 5000 } });


        //        //声明一个queue，里面的内容自发布起五秒后被删除
        //        channel.QueueDeclare("messagettlqueue", true, false, false, new Dictionary<string, object> { { "x-message-ttl", 5000 } });

        //        var properties = channel.CreateBasicProperties();
        //        //设置过期时间
        //        properties.Expiration = "5000";
        //        channel.BasicPublish(null, "queue", properties, Encoding.UTF8.GetBytes("我五秒后就会消失"));

        //        ////创建一个rpc queue
        //        //channel.QueueDeclare("testQueue", true, true, false, null,);


        //        //using (var channel2 = RabbitMqHelper.GetConnection().CreateModel())
        //        //{
        //        //    var consumer = new EventingBasicConsumer(channel2);
        //        //    channel2.BasicGet("testQueue", true);
        //        //}


        //        //using (var connection = RabbitMqHelper.GetNewConnection())
        //        //using (var channel2 = connection.CreateModel())
        //        //{
        //        //    var consumer = new EventingBasicConsumer(channel2);
        //        //    channel2.BasicGet("testQueue", true);
        //        //}


        //        Console.ReadKey();

        //    }

        //} 
        #endregion


        #region rpc server
        //static void Main(string[] args)
        //{

        //    //创建返回一个新的频道
        //    using (var channel = RabbitMqHelper.GetConnection().CreateModel())
        //    {

        //        //创建一个rpc queue
        //        channel.QueueDeclare("RpcQueue", true, false, false, null);

        //        SimpleRpcServer rpc = new SmsSimpleRpcServer(new Subscription(channel, "RpcQueue"));

        //        Console.WriteLine("服务端启动成功");

        //        rpc.MainLoop();
        //        Console.ReadKey();

        //    }

        //} 
        #endregion


        #region topic 模式
        //static void Main(string[] args)
        //{
        //    var flag = true;
        //    while (flag)
        //    {

        //        Console.WriteLine("请输入要发布的消息 key|msg。 或者按Ctrl+ C退出");

        //        var msg = Console.ReadLine();

        //        //创建返回一个新的频道
        //        using (var channel = RabbitMqHelper.GetConnection().CreateModel())
        //        {

        //            var msgs = msg.Split('|');

        //            //发布一个消息
        //            var msgbody = Encoding.UTF8.GetBytes(msgs[1]);

        //            channel.BasicPublish("TopicExchange", routingKey: string.Empty, basicProperties: null, body: msgbody);

        //            Console.Write("发布成功！");

        //        }
        //    }

        //    Console.ReadKey();

        //} 
        #endregion


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



    /// <summary>
    /// 发送短信的Rpc
    /// </summary>
    public class SmsSimpleRpcServer : SimpleRpcServer
    {
        public SmsSimpleRpcServer(Subscription subscription) : base(subscription)
        {
        }

        /// <summary>
        /// 执行完成后进行h回调
        /// </summary>
        /// <param name="isRedelivered"></param>
        /// <param name="requestProperties"></param>
        /// <param name="body"></param>
        /// <param name="replyProperties"></param>
        /// <returns></returns>
        public override byte[] HandleSimpleCall(bool isRedelivered, IBasicProperties requestProperties, byte[] body, out IBasicProperties replyProperties)
        {
            replyProperties = null;
            return Encoding.UTF8.GetBytes($"给{Encoding.UTF8.GetString(body)}发送短信成功");
        }


        /// <summary>
        /// 进行处理
        /// </summary>
        /// <param name="evt"></param>
        public override void ProcessRequest(BasicDeliverEventArgs evt)
        {
            // todo.....
            base.ProcessRequest(evt);
        }
    }
}
