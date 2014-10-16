using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using ISeeYou.Platform.Domain.Messages;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ISeeYou.MQ
{
    public class RabbitMqConsumer<TRabbitEvent> : IDisposable where TRabbitEvent: RabbitEventBase, new()
    {
        //private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private IModel _channel;
        private IConnection _connection;
        private bool _started;

        private readonly ConcurrentDictionary<string, QueueingBasicConsumer> _underlyingConsumers
            = new ConcurrentDictionary<string, QueueingBasicConsumer>();

        private string Host { get; set; }
        private string User { get; set; }
        private string Password { get; set; }
        private string ExchangeName { get; set; }
        private string QueueName { get; set; }
        private TimeSpan _timeout = TimeSpan.FromMilliseconds(200);

        public event Action<TRabbitEvent> On = e => { }; 

        public RabbitMqConsumer(string host, string user, string password, string exchangeName)
        {
            Host = host;
            User = user;
            Password = password;
            QueueName = string.Format("auto_q_{0}", GetRoutingKey());
            ExchangeName = exchangeName;
        }

        public void Start()
        {
            if (_started)
                return;

            _started = true;

            var fac = new ConnectionFactory
            {
                HostName = Host,
                Password = Password,
                UserName = User,
            };

            //Log.Info("Creating connection...");
            _connection = fac.CreateConnection();

            //Log.Info("Creating channel...");
            _channel = _connection.CreateModel();

            BindQueue();

            while (true)
            {
                QueueDeliveryMessage<TRabbitEvent> message;
                if (Consume(out message))
                {
                    BasicAck(message.DeliveryTag, true);
                    On(message.Event);
                }
            }
        }

        ~RabbitMqConsumer()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //Log.Info("Closing channel...");
                _channel.Dispose();

                //Log.Info("Closing connection...");
                _connection.Dispose();
            }

            //
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        //
        // PRIVATE METHODS
        //

        private void BindQueue()
        {
            //Log.InfoFormat("Declaring exchange ({0}.{1})...", Host, FormatExchangeName(exchangeName));

            _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, true);

            //Log.InfoFormat("Exchange declared: ({0}.{1})...", Host, FormatExchangeName(exchangeName));
            //Log.InfoFormat("Declaring queue ({0}.{1})...", Host, FormatQueueName(queueName));

            _channel.QueueDeclare(QueueName, true, false, false, null);

            //Log.InfoFormat("Queue declared: ({0}.{1})...", Host, FormatQueueName(queueName));

            //Log.InfoFormat("Binding queue ({0}.{1}) to exchange ({2}) with routing keys: {3}...", Host,
            //    FormatQueueName(queueName),
            //    FormatExchangeName(exchangeName), String.Join(",", routingKeys));

            _channel.QueueBind(QueueName, ExchangeName, GetRoutingKey());

            //Log.InfoFormat("Queue ({0}.{1}) bound to exchange ({2}).", Host, FormatQueueName(queueName),
            //    FormatExchangeName(exchangeName));
        }

        private bool Consume(out QueueDeliveryMessage<TRabbitEvent> message)
        {
            var consumer = GetConsumer(QueueName);

            BasicDeliverEventArgs result;
            if (consumer.Queue.Dequeue((int)_timeout.TotalMilliseconds, out result))
            {
                //Log.InfoFormat("Message successfully dequeued. Deserializing...");

                var msg = Encoding.UTF8.GetString(result.Body);

                //Log.Info("Message successfully deserialized: " + json);
                var e = new TRabbitEvent();
                e.Deserialize(msg);

                message = new QueueDeliveryMessage<TRabbitEvent>
                {
                    DeliveryTag = result.DeliveryTag,
                    Event = e
                };

                return true;
            }

            message = null;
            return false;
        }
        private void BasicAck(ulong deliveryTag, bool multiple)
        {
            _channel.BasicAck(deliveryTag, multiple);
        }

        private string GetRoutingKey()
        {
            return new TRabbitEvent().RoutingKey;
        }

        private QueueingBasicConsumer GetConsumer(string queueName)
        {
            var consumer = _underlyingConsumers.GetOrAdd(queueName, q =>
            {
                var c = new QueueingBasicConsumer(_channel);
                _channel.BasicConsume(q, false, c);

                return c;
            });

            return consumer;
        }

        private string FormatExchangeName(string exchangeName)
        {
            return string.Format("[{0}]", exchangeName);
        }

        private string FormatQueueName(string queueName)
        {
            return string.Format("[{0}]", queueName);
        }
    }
}
