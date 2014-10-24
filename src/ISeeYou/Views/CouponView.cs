using System;
using MongoDB.Bson.Serialization.Attributes;

namespace ISeeYou.Views
{
    public class CouponView
    {
        [BsonId]
        public string Id { get; set; }
        public long Amount { get; set; }
        public DateTime StartOn { get; set; }
        public DateTime? FinishOn { get; set; }
    }
}
