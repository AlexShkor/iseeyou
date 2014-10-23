using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private const int PAGE_SIZE = 20;

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
                Name = string.Format("{0} {1}", subject.FirstName, subject.LastName),
                TrackingStarted = DateTime.UtcNow
            });

            user.Subjects.Add(new SubjectData() { Id = subject.UserId.ToString(), Name = string.Format("{0} {1}", subject.FirstName, subject.LastName) });
            _users.Save(user);
            
            return RedirectToAction("Create", "Payments", new {id = subject.UserId});
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
            ViewBag.UserId = id;
            var model = new SubjectEventsViewModel()
            {
                Events = _events.Items.Find(Query.EQ("SubjectId", id)).OrderByDescending(x=> x.StartDate).Take(PAGE_SIZE).ToList()
            };

            return View(model);
        }

        [GET("GetItems")]
        public JsonResult GetItems(int id, int page = 2)
        {
            var events =
                _events.Items.Find(Query.EQ("SubjectId", id))
                    .OrderByDescending(x => x.StartDate)
                    .Skip((page - 1)*PAGE_SIZE)
                    .Take(PAGE_SIZE)
                    .ToList();
            return Json(events, JsonRequestBehavior.AllowGet);
        }

        [GET("GetMasonryItems")]
        public JsonResult GetMasonryItems(int id, int page = 2)
        {
            var events =
                _events.Items.Find(Query.EQ("SubjectId", id))
                    .OrderByDescending(x => x.StartDate)
                    .Skip((page - 1) * PAGE_SIZE)
                    .Take(PAGE_SIZE)
                    .Select(ConvertToMasonryItem)
                    .ToList();
            return Json(events, JsonRequestBehavior.AllowGet);
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

        private string ConvertToMasonryItem(EventView evt)
        {
            var str = new StringBuilder();
            str.Append("<div class=\"masonry-item\">");
            str.Append(String.Format("<div class=\"image\"><img src=\"{0}\"/></div>", evt.ImageBig));
            str.Append("<div class=\"source\"><a href=\"http://www.vk.com/id" + evt.SourceId +
                       "\" target=\"_blank\">vk.com/id" + evt.SourceId + "</a></div>");
            str.Append("<a href=\"https://vk.com/photo" + evt.SourceId + "_" + evt.PhotoId + "\">" + (evt.StartDate.HasValue ? evt.StartDate.Value.ToString("dd/MM/yyyy") : "") + "</a>");
            str.Append("<div class=\"like\"><span class=\"glyphicon glyphicon-heart\"></span></div>");
            str.Append("</div>");

            return str.ToString();
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
