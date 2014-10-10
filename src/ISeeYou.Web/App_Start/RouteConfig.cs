using System.Web.Mvc;
using System.Web.Routing;

namespace ISeeYou.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               "Default", "{*path}",
                defaults: new { controller = "Subjects", action = "Index" }
            );
        }
    }
}