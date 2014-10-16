using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ISeeYou.Documents
{
    public class SourceDocument
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public int SourceId { get; set; }

        public int Rank { get; set; }

        public int SubjectId { get; set; }

        public int Calls { get; set; }
    }

    public class SourceStats
    {
        [BsonId]
        public int SourceId { get; set; }

        public DateTime Fetched { get; set; }

        public int Count { get; set; }
    }
}