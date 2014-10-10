using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;

namespace ISeeYou.Platform.Mongo
{
    public class NoDefaultPropertyIdConvention : IClassMapConvention
    {
        public string Name
        {
            get { return "No Default Property Id Convention"; }
        }

        public void Apply(BsonClassMap classMap)
        {
            classMap.SetIdMember(null);
        }
    }
}