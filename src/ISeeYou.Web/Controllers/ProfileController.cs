using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using ISeeYou.Platform.Mvc;
using ISeeYou.Views;
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

        [GET("index")]
        public ActionResult Index()
        {
            var model = new ProfileViewModel() {User = _users.GetById(UserId)};
            return View(model);
        }

        [GET("choose-avatar")]
        public ActionResult ChooseAvatar()
        {
            return View();
        }

    }

    public class ProfileViewModel
    {
        public UserView User { get; set; } 
    }
}
