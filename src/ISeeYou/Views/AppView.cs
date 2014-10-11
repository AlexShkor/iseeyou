using MongoDB.Bson.Serialization.Attributes;

namespace ISeeYou.Views
{
    public class AppView
    {
        [BsonId]
        public string Id { get; set; }

        public string Token { get; set; }
    }
}