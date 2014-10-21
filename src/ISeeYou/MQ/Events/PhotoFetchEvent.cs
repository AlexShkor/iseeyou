using Newtonsoft.Json;

namespace ISeeYou.MQ.Events
{
    public class PhotoFetchEvent : RabbitEventBase
    {
        public PhotoFetchPayload Payload { get; set; }

        public override string RoutingKey
        {
            get { return "photo_fetch"; }
        }

        public override string Serialize()
        {
            return JsonConvert.SerializeObject(Payload);
        }

        public override void Deserialize(string message)
        {
            Payload = JsonConvert.DeserializeObject<PhotoFetchPayload>(message);
        }
    }
}