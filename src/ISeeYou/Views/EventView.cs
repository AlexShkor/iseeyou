using System;
using MongoDB.Bson.Serialization.Attributes;

namespace ISeeYou.Views
{
    public class EventView
    {
        [BsonId]
        public int Id { get; set; }
        public string Type { get; set; }
        public int SubjectId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int SourceId { get; set; }
        public string Image { get; set; }
    }
}