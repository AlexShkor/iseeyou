using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISeeYou.Platform.Mvc;
using ISeeYou.ViewServices;
using ISeeYou.Vk.Helpers;

namespace ISeeYou.Web.Controllers
{
    public class SubjectsController : BaseController
    {
        private readonly UsersViewService _users;

        public SubjectsController(UsersViewService users)
        {
            _users = users;
        }

        public ActionResult Index()
        {
            var user = _users.GetById(UserId);
            var subjects = user.Subjects;
            return View();
        }


        [HttpGet]
        public ActionResult Authorize(string id)
        {
            var url = VkAuth.BuildAuthorizeUrlForMobile("4185172");
            return View(url);
        }

        [HttpPost]
        public ActionResult Authorize(string groupId, string blankPageUrl)
        {
            var token = UrlUtility.ExtractToken(blankPageUrl);
            var user = _users.GetById(UserId);
            user.Token = token;
            _users.Save(user);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(SubjectViewModel model)
        {
            return View();
        }
    }

    public class SubjectViewModel
    {
    }
}
