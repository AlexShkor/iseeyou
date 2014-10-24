using System;
using ISeeYou.Platform.Mongo;
using ISeeYou.Views;
using ISeeYou.ViewServices;
using StructureMap;

namespace ISeeYou.Web
{
    public class CouponsInitializer
    {
        public static void Setup()
        {
            var coupons = ObjectFactory.Container.GetInstance<CouponsViewService>();
            if(coupons.Items.Count() > 0) return;
            var settings = ObjectFactory.Container.GetInstance<SiteSettings>();
            var id = ObjectFactory.Container.GetInstance<IdGenerator>().Generate();
            var coupon = new CouponView()
            {
                Id = id,
                Token = "paralect",
                Amount = settings.CouponsAmount,
                StartOn = DateTime.UtcNow
            };
            coupons.Save(coupon);
        }
    }
}