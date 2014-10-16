using Newtonsoft.Json.Linq;

namespace ISeeYou.MQ.Events
{
    public class SubjectAddedEvent : RabbitEventBase
    {
        public override string RoutingKey
        {
            get { return "subject_added"; }
        }

        public string Id { get; set; }

        public override string Serialize()
        {
            return "{ id: 1232341234 }";
            // don't need it here
            throw new System.NotImplementedException();
        }

        public override void Deserialize(string message)
        {
            dynamic msg = JObject.Parse(message);

            Id = msg.id;
        }
    }

    
}