using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ISeeYou.Views
{
    public class SubjectView
    {
        [BsonId]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime TrackingStarted { get; set; }
        public DateTime NextFetching { get; set; }
        public DateTime? FetchingStarted { get; set; }
        public DateTime? FetchingEnded { get; set; }
        public DateTime? FetchedFirstTime { get; set; }
    }
}