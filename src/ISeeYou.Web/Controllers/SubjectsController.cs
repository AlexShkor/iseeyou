using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using ISeeYou.Platform.Mvc;
using ISeeYou.ViewServices;
using ISeeYou.Vk.Api;
using ISeeYou.Vk.Helpers;
using VkAPIAsync.Wrappers.Users;

namespace ISeeYou.Web.Controllers
{
    [RoutePrefix("subjects")]
    public class SubjectsController : BaseController
    {
        private readonly UsersViewService _users;

        public SubjectsController(UsersViewService users)
        {
            _users = users;
        }

        [GET("index")]
        public ActionResult Index()
        {
            var user = _users.GetById(UserId);
            var subjects = user.Subjects;
            return View();
        }


        [GET("Authorize")]
        public ActionResult Authorize()
        {
            var url = VkAuth.BuildAuthorizeUrlForMobile("4584967");
            var model = new AuthorizeViewModel() {Url = url};

            return View(model);
        }


        [POST("Authorize")]
        public ActionResult Authorize(string blankPageUrl)
        {
            var token = UrlUtility.ExtractToken(blankPageUrl);
            var user = _users.GetById(UserId);
            user.Token = token;
            _users.Save(user);
            return RedirectToAction("Index", "Profile");
        }

        [GET("AddSubject")]
        public ActionResult AddSubject()
        {
            return View();
        }

        [POST("AddSubject")]
        public ActionResult AddSubject(SubjectViewModel model)
        {
            var id = GetSubjectFromUrl(model.SubjectUrl);

            

            return View();
        }

        private string GetSubjectFromUrl(string url)
        {
            string id;

            try
            {
                id = UrlUtility.ExtractVkUserId(url);
            }
            catch (Exception)
            {
                id = UrlUtility.LastSegment(url);
            }


            if (id.Length > 0 && char.IsDigit(id, 0))
                return id;

            var user = Users.Get(new List<string>() {id}, new List<string>(){"sex"});

            return user.Id.ToString();

        }
    }

    public class SubjectViewModel
    {
        public string SubjectUrl { get; set; }
    }

    public class AuthorizeViewModel
    {
        public string Url { get; set; }
    }
}
