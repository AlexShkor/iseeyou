using System;
using Microsoft.AspNet.SignalR.Infrastructure;
using RabbitMQ.Client;

namespace ISeeYou.MQ
{
    public class RabbitMqPublisher : IDisposable
    {
        private string Host { get; set; }
        private string User { get; set; }
        private string Password { get; set; }
        private string ExchangeName { get; set; }

        private readonly IModel _channel;
        private readonly IConnection _connection;

        public RabbitMqPublisher(string host, string user, string password, string exchangeName)
        {
            Host = host;
            User = user;
            Password = password;
            ExchangeName = exchangeName;

            var connectionFactory = new ConnectionFactory
            {
                HostName = Host,
                UserName = User,
                Password = Password
            };

            _connection = connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, true);
        }

        public void Publish(RabbitEventBase e)
        {
            try
            {
                var props = _channel.CreateBasicProperties();
                props.SetPersistent(true);
                _channel.BasicPublish(ExchangeName, e.RoutingKey, props, e.ToMessage());
            }
            catch
            {
                // todo: add logging here
            }
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }
    }
}
