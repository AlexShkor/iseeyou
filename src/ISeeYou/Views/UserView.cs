using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Uniform;

namespace ISeeYou.Views
{
    public class UserView
    {
        [DocumentId, BsonId]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime CreationDate { get; set; }
        public string FacebookId { get; set; }
        public long Cash { get; set; }
        public string AvatarId { get; set; }
        public string Token { get; set; }
        public List<SubjectData> Subjects { get; set; }

        public string BraintreeCustomerId { get; set; }

        public UserView()
        {
            Subjects = new List<SubjectData>();
        }
    }

    public class SubjectData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string SubscriptionId { get; set; }
        public DateTime? NextPayment { get; set; }
        public DateTime? Stopped { get; set; }

        public bool Paid
        {
            get { return NextPayment.HasValue && NextPayment > DateTime.UtcNow; }
        }

        public void SetPayment()
        {
            NextPayment = DateTime.UtcNow.AddMonths(1);
            Stopped = null;
        }
    }
}