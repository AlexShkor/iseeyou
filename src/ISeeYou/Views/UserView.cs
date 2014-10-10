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

        public UserView()
        {
            Subjects = new List<SubjectData>();
        }
    }

    public class SubjectData
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}