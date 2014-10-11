using MongoDB.Bson.Serialization.Attributes;

namespace ISeeYou.Documents
{
    public class SourceDocument
    {
        [BsonId]
        public int Id { get; set; }

        public int Rank { get; set; }

        public int SubjectId { get; set; }

        public int Calls { get; set; }
    }
}