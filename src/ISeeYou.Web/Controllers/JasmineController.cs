using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;

namespace ISeeYou.Web.Controllers
{
    [RoutePrefix("jasmine")]
    public class JasmineController : Controller
    {
        [GET("")]
        public ActionResult Index()
        {
            return View("JasmineTests");
        }
    }
}
