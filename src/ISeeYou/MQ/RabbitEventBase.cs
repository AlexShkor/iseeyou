using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Serializers;

namespace ISeeYou.MQ
{
    public abstract class RabbitEventBase
    {
        public abstract string RoutingKey { get; }

        public abstract string Serialize();

        public abstract void Deserialize(string message);

        public byte[] ToMessage()
        {
            return Encoding.Default.GetBytes(Serialize());
        }


    }
}
