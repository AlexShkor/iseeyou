using System;
using MongoDB.Bson.Serialization.Attributes;

namespace ISeeYou.Views
{
    public class TrackingMark
    {
        [BsonId]
        public string Id { get; set; }

        public DateTime Tracked { get; set; }
    }
}