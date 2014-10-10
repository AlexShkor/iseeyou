using System.Web.Optimization;
using MongoDB.Bson;

namespace ISeeYou.Web
{
    public class BundleConfig
    {

        public static readonly string ContentVersion = ObjectId.GenerateNewId().ToString();

        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            //BundleTable.EnableOptimizations = true;

            var style = new StyleBundle("~/content/landing")
               .Include("~/assets/vendor/bootstrap/css/bootstrap.css")
               .Include("~/assets/app/css/common.css");
            style.Transforms.Add(new CssMinify());
            bundles.Add(style);

            style = new StyleBundle("~/content/css")
                .Include("~/assets/vendor/bootstrap/css/bootstrap.css")
                .Include("~/assets/vendor/bootstrap/css/darkstrap.css")
                .Include("~/assets/app/css/common.css")
                .Include("~/assets/app/css/auctionrobot.css")
                .Include("~/assets/app/css/m-buttons.css")
                .Include("~/assets/app/css/landing.css")
                .Include("~/Assets/vendor/font-awesome/css/font-awesome.css")
                .Include("~/Assets/vendor/angular-spiner/css/rzslider.css");

            bundles.Add(style);

            style = new StyleBundle("~/content/common")
                .Include("~/assets/vendor/bootstrap/css/bootstrap.css")
                .Include("~/assets/app/css/common.css")
                .Include("~/assets/app/css/game.css")
                .Include("~/assets/app/css/auctionrobot.css");

            bundles.Add(style);

            style = new StyleBundle("~/content/bootstrap")
                .Include("~/assets/vendor/bootstrap/css/bootstrap.css");

            bundles.Add(style);

            style = new StyleBundle("~/content/login")
                .Include("~/assets/app/css/login.css");
            //.Include("~/assets/app/css/demo.css")
            //.Include("~/assets/app/css/animate-custom.css");

            bundles.Add(style);

            style = new StyleBundle("~/assets/vendor/jui/themes/base/jui")
                .Include("~/assets/vendor/jui/themes/base/jquery-ui.css")
                .Include("~/assets/vendor/jui/themes/base/jquery.ui.dialog.css");

            bundles.Add(style);
        }
    }
}