using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQHelper;
using System.Collections.Generic;
using RabbitMQ.Client.MessagePatterns;

namespace RabbitMQConsumer
{
    class Program
    {

        static void Main(string[] args)
        {


            using (var channel = RabbitMqHelper.GetConnection().CreateModel())
            {
                //创建client的rpc
                SimpleRpcClient client = new SimpleRpcClient(channel, new PublicationAddress(exchangeType: ExchangeType.Direct, exchangeName: string.Empty, routingKey: "RpcQueue"));
                bool flag = true;
                var sendmsg = "";
                while (flag)
                {
                    Console.WriteLine("请输入要发送的消息");
                    sendmsg = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(sendmsg))
                    {
                        Console.Write("请输入消息");
                        continue;
                    }

                    var msg = client.Call(Encoding.UTF8.GetBytes(sendmsg));

                    Console.WriteLine(Encoding.UTF8.GetString(msg));


                }

                Console.ReadKey();

            }
        }
        #region topic 模式
        //static void Main(string[] args)
        //{

        //    bool flag = true;
        //    var key = "";
        //    while (flag)
        //    {
        //        Console.WriteLine("请输入路由正则  .代表一个字符 *代表零到多个字符");
        //        key = Console.ReadLine();
        //        if (string.IsNullOrWhiteSpace(key))
        //        {
        //            Console.Write("请输入路由");
        //            continue;
        //        }
        //        else
        //            flag = false;

        //    }

        //    using (var channel = RabbitMqHelper.GetConnection().CreateModel())
        //    {
        //        //根据声明使用的队列
        //        var QueueName = key + "Queue";

        //        //声明交换机 headers模式
        //        channel.ExchangeDeclare("TopicExchange", ExchangeType.Topic, true, false);

        //        channel.QueueDeclare(QueueName, true, false, false, null);
        //        //进行绑定
        //        channel.QueueBind(QueueName, "TopicExchange", key, null);

        //        //创建consumbers
        //        var consumer = new EventingBasicConsumer(channel);

        //        consumer.Received += (sender, e) =>
        //        {
        //            var msg = Encoding.UTF8.GetString(e.Body);

        //            Console.WriteLine($"{e.RoutingKey}：{msg}");
        //        };

        //        //进行消费
        //        channel.BasicConsume(QueueName, true, consumer);

        //        Console.ReadKey();

        //    }
        //} 
        #endregion


        #region headers exchange模式
        //static void Main(string[] args)
        //{

        //    bool flag = true;
        //    string pattern = "";
        //    while (flag)
        //    {
        //        Console.WriteLine("请选择headers匹配模式  1(any)/2(all)");
        //        pattern = Console.ReadLine();
        //        if (pattern == "1" || pattern == "2")
        //            flag = false;
        //        else
        //            Console.Write("请做出正确的选择");
        //    }



        //    using (var channel = RabbitMqHelper.GetConnection().CreateModel())
        //    {
        //        //根据声明使用的队列
        //        var headersType = pattern == "1" ? "any" : "all";

        //        //声明交换机 headers模式
        //        channel.ExchangeDeclare("headersExchange", ExchangeType.Headers, true, false);

        //        channel.QueueDeclare("headersQueue", true, false, false, null);
        //        //进行绑定
        //        channel.QueueBind("headersQueue", "headersExchange", string.Empty, new Dictionary<string, object>
        //        {
        //            //第一个匹配格式 ，第二与第三个则是匹配项
        //            { "x-match",headersType},
        //            { "user","admin"},
        //            { "pwd","123456"}
        //        });

        //        //创建consumbers
        //        var consumer = new EventingBasicConsumer(channel);

        //        consumer.Received += (sender, e) =>
        //        {
        //            var msg = Encoding.UTF8.GetString(e.Body);

        //            Console.WriteLine($"{msg}");
        //        };

        //        //进行消费
        //        channel.BasicConsume("headersQueue", true, consumer);

        //        Console.ReadKey();

        //    }
        //}
        #endregion


        #region fanout模式 
        //static void Main(string[] args)
        //{

        //    bool flag = true;
        //    string pattern = "";
        //    while (flag)
        //    {
        //        Console.WriteLine("请选择Ccnsumer模式  1(发短信)/2(发邮件)");
        //        pattern = Console.ReadLine();
        //        if (pattern == "1" || pattern == "2")
        //            flag = false;
        //        else
        //            Console.Write("请做出正确的选择");
        //    }



        //    using (var channel = RabbitMqHelper.GetConnection().CreateModel())
        //    {

        //        //声明交换机 Fanout模式
        //        channel.ExchangeDeclare("fanoutExchange", ExchangeType.Fanout, true, false, null);
        //        //根据声明使用的队列
        //        var queueName = pattern == "1" ? "sms" : "emai";
        //        channel.QueueDeclare(queueName, true, false, false, null);
        //        //进行绑定
        //        channel.QueueBind(queueName, "fanoutExchange", string.Empty, null);

        //        //创建consumbers
        //        var consumer = new EventingBasicConsumer(channel);

        //        consumer.Received += (sender, e) =>
        //        {
        //            var msg = Encoding.UTF8.GetString(e.Body);

        //            var action = (pattern == "1" ? "发短信" : "发邮件");
        //            Console.WriteLine($"给{msg}{action}");
        //        };

        //        //进行消费
        //        channel.BasicConsume(queueName, true, consumer);

        //        Console.ReadKey();

        //    }
        //}
        #endregion

        #region 日志消息消费者 direct模式根据routingkey
        //static void Main(string[] args)
        //{

        //    bool flag = true;
        //    string level = "";
        //    while (flag)
        //    {
        //        Console.WriteLine("请指定要接收的消息级别");
        //        level = Console.ReadLine();
        //        if (level == "info" || level == "error" || level == "debug")
        //            flag = false;
        //        else
        //            Console.Write("仅支持info、debug与error级别");
        //    }



        //    using (var channel = RabbitMqHelper.GetConnection().CreateModel())
        //    {

        //        //声明交换机 direct模式
        //        channel.ExchangeDeclare("LogExchange", "direct", true, false, null);
        //        //根据声明使用的队列
        //        var queueName = level == "info" ? "Log_else" : level == "debug" ? "Log_else" : "Log_error";
        //        channel.QueueDeclare(queueName, true, false, false, null);
        //        //进行绑定
        //        channel.QueueBind(queueName, "LogExchange", level, null);


        //        //创建consumbers
        //        var consumer = new EventingBasicConsumer(channel);

        //        consumer.Received += (sender, e) =>
        //        {
        //            var msg = Encoding.UTF8.GetString(e.Body);

        //            Console.WriteLine(msg);
        //        };

        //        //进行消费
        //        channel.BasicConsume(queueName, true, consumer);

        //        Console.ReadKey();

        //    }
        //}
        #endregion

        #region 第一版与第二版简单Demo、交换机队列绑定
        //static void Main(string[] args)
        //{
        //    //consumber端
        //    var factory = new ConnectionFactory()
        //    {
        //        HostName = "localhost",
        //        UserName = "admin",
        //        Password = "admin"
        //    };

        //    using (var connection = factory.CreateConnection())
        //    using (var channel = connection.CreateModel())
        //    {
        //        //获取消息
        //        #region 第一版直接拉取
        //        //不再使用直接拉取的方式
        //        //var result = channel.BasicGet("firstTest", true);
        //        //var msg = Encoding.UTF8.GetString(result.Body);
        //        // Console.WriteLine(msg); 
        //        #endregion

        //        //使用订阅的方式
        //        //这里的创建队列,是为了防止 消费 在 生产 之前
        //        channel.QueueDeclare("firstTest", true, false, false, null);
        //        //绑定队列 
        //        channel.ExchangeDeclare("firstExchange", "direct", true, false, null);
        //        channel.QueueBind("firstTest", "firstExchange", "firstExchange_Demo_firstTest", null);



        //        var consumer = new EventingBasicConsumer(channel);

        //        consumer.Received += (sender, e) =>
        //        {
        //            var msg = Encoding.UTF8.GetString(e.Body);

        //            Console.WriteLine(msg);
        //        };

        //        //进行消费
        //        channel.BasicConsume("firstTest", true, consumer);

        //        Console.ReadKey();

        //    }
        //}
        #endregion



    }
}
