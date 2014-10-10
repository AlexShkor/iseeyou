using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using ISeeYou.Platform.Mvc;
using ISeeYou.ViewServices;

namespace ISeeYou.Web.Controllers
{
    [RoutePrefix("profile")]
    public class ProfileController : BaseController
    {
        private readonly UsersViewService _users;

        public ProfileController(UsersViewService users)
        {
            _users = users;
        }

        [GET("view")]
        public ActionResult Index()
        {
            return View();
        }

        [GET("choose-avatar")]
        public ActionResult ChooseAvatar()
        {
            return View();
        }

    }
}
