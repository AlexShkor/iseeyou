using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ISeeYou.Views
{
    public class SubjectView
    {
        [BsonId]
        public string Id { get; set; }

        public List<string> Sources { get; set; } 
    }
}