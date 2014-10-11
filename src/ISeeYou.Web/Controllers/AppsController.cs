using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using ISeeYou.Views;
using ISeeYou.ViewServices;
using ISeeYou.Vk.Helpers;
using ISeeYou.VkRanking;

namespace ISeeYou.Web.Controllers
{
    [RoutePrefix("apps")]
    public class AppsController : Controller
    {
        private readonly AppsViewService _apps;
        private readonly SubjectViewService _subjects;
        private readonly VkRanker _ranker;

        public AppsController(AppsViewService apps, SubjectViewService subjects, VkRanker ranker)
        {
            _apps = apps;
            _subjects = subjects;
            _ranker = ranker;
        }

        [GET("")]
        public ActionResult Index()
        {
            var apps = _apps.GetAll();
            return View(apps);
        }

        [POST("Add")]
        public ActionResult Add(string id)
        {
            _apps.Save(new AppView
            {
                Id = id
            });
            return RedirectToAction("Index");
        }

        [GET("authorize/{id}")]
        public ActionResult Authorize(string id)
        {
            var url = VkAuth.BuildAuthorizeUrlForMobile(id);
            var model = new AuthorizeViewModel() { Url = url, Id = id };

            return View(model);
        }


        [POST("Authorize")]
        public ActionResult Authorize(string blankPageUrl, string id)
        {
            var token = UrlUtility.ExtractToken(blankPageUrl);
            _apps.Save(new AppView
            {
                Id = id,
                Token = token
            });
            return RedirectToAction("Index", "Apps");
        }

    }
}
