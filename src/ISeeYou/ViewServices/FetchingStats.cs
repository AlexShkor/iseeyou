using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ISeeYou.ViewServices
{
    public class FetchingStats
    {
        [BsonId]
        public string Id { get; set; }

        public int SourceId { get; set; }
        public List<int> Subjects { get; set; }
        public int Rank { get; set; }
        public DateTime Processed { get; set; }
        public long Elapsed { get; set; }
    }
}