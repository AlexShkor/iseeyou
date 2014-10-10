using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using ISeeYou.Platform.Mvc;

namespace ISeeYou.Web.Controllers
{
    [RoutePrefix("home")]
    [Authorize]
    public class HomeController : BaseController
    {
        [GET("")]
        [GET("", IgnoreRoutePrefix = true)]
        public ActionResult Index()
        {

            return RedirectToAction("Index", "Subjects");
            //return View("Templates/Empty","_BaseLayout");
        }
    }
}