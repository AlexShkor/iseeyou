using MongoDB.Bson.Serialization.Attributes;

namespace ISeeYou.Platform.Upgrade
{
    public class UpgradeDocument
    {
        [BsonId]
        public string UpgradeNumber
        {
            get { return "Upgrade"; }
            set { /* intendent to be empty */ }
        }
        public int LatestSuccessfulUpgrade { get; set; }
    }
}