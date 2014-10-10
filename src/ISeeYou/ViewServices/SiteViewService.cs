using ISeeYou.Databases;
using ISeeYou.Platform.Mongo;
using ISeeYou.Platform.ViewServices;
using ISeeYou.Views;

namespace ISeeYou.ViewServices
{
    public class SitesViewService : ViewService<SiteView>
    {
        public SitesViewService(MongoViewDatabase database)
            : base(database)
        {
        }

        protected override ReadonlyMongoCollection<SiteView> Items
        {
            get { return Database.Sites; }
        }

        public SiteView GetSite()
        {
            return GetById(SiteSettings.SiteId);
        }
    }
}