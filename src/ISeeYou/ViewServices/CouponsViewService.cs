using System.Linq;
using ISeeYou.Databases;
using ISeeYou.Platform.ViewServices;
using ISeeYou.Views;
using MongoDB.Driver;

namespace ISeeYou.ViewServices
{
    public class CouponsViewService :  ViewService<CouponView>
    {
        public CouponsViewService(MongoViewDatabase database) : base(database)
        {
        }

        public override MongoCollection<CouponView> Items
        {
            get { return Database.Coupons; }
        }

        public CouponView GetCoupon()
        {
            return Items.FindAll().OrderByDescending(x => x.StartOn).FirstOrDefault();
        }
    }
}
