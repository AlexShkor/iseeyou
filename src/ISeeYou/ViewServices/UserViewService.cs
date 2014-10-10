using System.Collections.Generic;
using ISeeYou.Databases;
using ISeeYou.Helpers;
using ISeeYou.Platform.Extensions;
using ISeeYou.Platform.Mongo;
using ISeeYou.Platform.ViewServices;
using ISeeYou.Views;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace ISeeYou.ViewServices
{
    public class UsersViewService : ViewServiceFiltered<UserView, UserFilter>
    {
        private readonly CryptographicHelper _crypto;

        public UsersViewService(MongoViewDatabase database, CryptographicHelper crypto)
            : base(database)
        {
            _crypto = crypto;
        }

        protected override ReadonlyMongoCollection<UserView> Items
        {
            get { return Database.Users; }
        }

        public UserView GetByUserName(string userName)
        {
            return Items.FindOne(Query<UserView>.EQ(x => x.UserName, userName));
        }

        public UserView GetByEmail(string email)
        {
            return GetById(_crypto.GetMd5Hash(email));
        }

        public override IEnumerable<IMongoQuery> BuildFilterQuery(UserFilter filter)
        {
            
                if (filter.UserId.HasValue())
                {
                    yield return Query<UserView>.EQ(x => x.Id, filter.UserId);
                }
        }


        public UserView GetUserByCredentionals(string email, string getPasswordHash)
        {
            throw new System.NotImplementedException();
        }

        public object GetByFacebookId(string facebookId)
        {
            return Items.FindOneAs<UserView>(Query<UserView>.EQ(x => x.FacebookId, facebookId));
        }

        public UserView GetUserName(string userName)
        {
            return Items.FindOne(Query<UserView>.EQ(x => x.UserName, userName));
        }
    }


    public class UserFilter : BaseFilter
    {
        public string UserId { get; set; }
    }
}