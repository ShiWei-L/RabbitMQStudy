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
                    HostName = "192.168.1.117",
                    UserName = "admin",
                    Password = "admin",
                    AutomaticRecoveryEnabled = true,
                    TopologyRecoveryEnabled = true,
                    Port = 25672


                };


                Connection = factory.CreateConnection();
                return Connection;
            }
            return Connection;
        }

        public static IConnection GetNewConnection()
        {

            //从工厂中拿到实例 本地host、用户admin
            var factory = new ConnectionFactory()
            {
                HostName = "192.168.1.117",
                UserName = "admin",
                Password = "admin",
                AutomaticRecoveryEnabled = true,
                TopologyRecoveryEnabled = true,
                Port = 25672

            };


            Connection = factory.CreateConnection();
            return Connection;

        }
    }
}
