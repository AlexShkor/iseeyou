using ISeeYou.Databases;
using ISeeYou.Platform.ViewServices;
using ISeeYou.Views;
using MongoDB.Driver;

namespace ISeeYou.ViewServices
{
    public class AppsViewService : ViewService<AppView>
    {
        public AppsViewService(MongoViewDatabase database)
            : base(database)
        {
        }

        public override MongoCollection<AppView> Items
        {
            get { return Database.Apps; }
        }
    }
}