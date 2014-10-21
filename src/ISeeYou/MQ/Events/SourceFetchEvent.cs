using Newtonsoft.Json;

namespace ISeeYou.MQ.Events
{
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
}