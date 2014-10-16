using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using ISeeYou.Platform.Mvc;
using ISeeYou.Views;
using ISeeYou.ViewServices;
using ISeeYou.Vk.Api;
using ISeeYou.Vk.Dto;
using ISeeYou.Vk.Helpers;
using MongoDB.Driver.Builders;

namespace ISeeYou.Web.Controllers
{
    [RoutePrefix("subjects")]
    public class SubjectsController : BaseController
    {
        private readonly UsersViewService _users;
        private readonly SubjectViewService _subjects;
        private readonly EventsViewService _events;

        public SubjectsController(UsersViewService users, SubjectViewService subjects, EventsViewService events)
        {
            _users = users;
            _subjects = subjects;
            _events = events;
        }

        [GET("index")]
        public ActionResult Index()
        {
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
        public async Task<ActionResult> AddSubject(SubjectViewModel model)
        {
            var id = GetSubjectIdFromUrl(model.SubjectUrl);
            var subject = GetUserFromVk(id);
            var user = _users.GetById(UserId);
            _subjects.Save(new SubjectView
            {
                Id = subject.UserId,
                Name = string.Format("{0} {1}", subject.FirstName, subject.LastName)
            });

            user.Subjects.Add(new SubjectData() { Id = subject.UserId.ToString(), Name = string.Format("{0} {1}", subject.FirstName, subject.LastName) });
            _users.Save(user);
            
            return RedirectToAction("Index", "Profile");
        }

        private string GetSubjectIdFromUrl(string url)
        {
            string id;
            id = UrlUtility.ExtractVkUserId(url);
           
            if (id.Length > 0 && char.IsDigit(id, 0))
                return id;

            return UrlUtility.LastSegment(url);
        }

        private VkUser GetUserFromVk(string id)
        {
            var api = new VkApi(null);
            return api.GetUsers(new[] {id}, new[] {"sex"})[0];
        }

        [GET("ViewSubjectEvents")]
        public ActionResult ViewSubjectEvents(int id)
        {
            var model = new SubjectEventsViewModel()
            {
                Events = _events.Items.Find(Query.EQ("SubjectId", id)).OrderByDescending(x=> x.StartDate).ToList()
            };

            return View(model);
        }

        [GET("DeleteSubject")]
        public ActionResult DeleteSubject(int id)
        {
            //_subjects.Items.Remove(Query.EQ("_id", id));
            //_events.Items.Remove(Query.EQ("SubjectId", id));
            var user = _users.GetById(UserId);

            user.Subjects.RemoveAll(s => s.Id == id.ToString());
            _users.Save(user);


            return RedirectToAction("Index", "Profile");
        }

    }


    public class SubjectViewModel
    {
        public string SubjectUrl { get; set; }
    }

    public class AuthorizeViewModel
    {
        public string Url { get; set; }
        public string Id { get; set; }
    }

    public class SubjectEventsViewModel
    {
        public List<EventView> Events { get; set; } 
    }

}
