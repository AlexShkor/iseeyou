using MongoDB.Bson;

namespace ISeeYou.Platform.Mongo
{
    public class IdGenerator 
    {
        public string Generate()
        {
            return ObjectId.GenerateNewId().ToString();
        }
    }
}
