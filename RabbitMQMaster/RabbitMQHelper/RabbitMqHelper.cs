using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMQHelper
{

    /// <summary>
    /// RabbitMQ帮助类
    /// </summary>
    public class RabbitMqHelper
    {
        private static IConnection Connection;

        /// <summary>
        /// 获取连接对象
        /// </summary>
        /// <returns></returns>
        public static IConnection GetConnection()
        {
            if (Connection == null)
            {
                //从工厂中拿到实例 本地host、用户admin
                var factory = new ConnectionFactory()
                {
                    UserName = "admin",
                    Password = "admin",
                    HostName = "localhost"
                };
                Connection = factory.CreateConnection();
                return Connection;
            }
            return Connection;
        }
    }
}
