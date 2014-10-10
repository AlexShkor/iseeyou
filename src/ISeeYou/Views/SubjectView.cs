using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ISeeYou.Views
{
    public class SubjectView
    {
        [BsonId]
        public int Id { get; set; }

        public List<int> Sources { get; set; } 
    }
}