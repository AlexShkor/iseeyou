using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISeeYou.MQ;
using ISeeYou.Views;
using ISeeYou.ViewServices;
using ISeeYou.Vk.Api;
using Newtonsoft.Json;
using StructureMap;

namespace ISeeYou.PhotoProducer
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = ObjectFactory.Container;
            new Bootstrapper().ConfigureSettings(container);
            new Bootstrapper().ConfigureMongoDb(container);
            var api = new VkApi();
            const string host = "dubina.by";
            const string user = "spypie";
            const string pwd = "GM9SGQoLngSaJYZ";
            const string fetchingExchange = "spypie_sources";
            const string type = "photo";

            var subjectAddedConsumer = new RabbitMqConsumer<SourceFetchEvent>(host, user, pwd, fetchingExchange);
            subjectAddedConsumer.On += source =>
            {
            };
            subjectAddedConsumer.Start();
        }
    }



    public class SourceFetchEvent : RabbitEventBase
    {
        public SourceFetchPayload Payload { get; set; }

        public override string RoutingKey
        {
            get { return "source_fetch"; }
        }

        public override string Serialize()
        {
            return JsonConvert.SerializeObject(Payload);
        }

        public override void Deserialize(string message)
        {
            Payload = JsonConvert.DeserializeObject<SourceFetchPayload>(message);
        }
    }

    public class SourceFetchPayload
    {
        public int UserId { get; set; }
    }
}
