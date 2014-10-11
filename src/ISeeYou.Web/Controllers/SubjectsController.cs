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
using ISeeYou.Vk.Helpers;
using VkAPIAsync.Wrappers.Users;

namespace ISeeYou.Web.Controllers
{
    [RoutePrefix("subjects")]
    public class SubjectsController : BaseController
    {
        private readonly UsersViewService _users;
        private readonly SubjectViewService _subjects;

        public SubjectsController(UsersViewService users, SubjectViewService subjects)
        {
            _users = users;
            _subjects = subjects;
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
        public async Task<ActionResult> AddSubject(SubjectViewModel model)
        {
            var id = GetSubjectFromUrl(model.SubjectUrl);
            var subject = await GetUserFromVk(id);
            var user = _users.GetById(UserId);
            _subjects.Save(new SubjectView
            {
                Id = subject.Id.Value,
                Name = string.Format("{0} {1}", subject.FirstName, subject.LastName),
                Token = user.Token
            });
            return View();
        }

        private string GetSubjectFromUrl(string url)
        {
            string id;
            
            id = UrlUtility.ExtractVkUserId(url);
           
            if (id.Length > 0 && char.IsDigit(id, 0))
                return id;

            return UrlUtility.LastSegment(url);
        }

        private async Task<User> GetUserFromVk(string id)
        {
            return (await Users.Get(new List<string>() {id}, new List<string>() {"sex"}))[0];
        }



    }



//
//    public static async Task<string> Send(string url)
//        {
//            try
//            {
//                _request = (HttpWebRequest) WebRequest.Create(new Uri(url));
//                _request.Timeout = Timeout;
//                _request.UserAgent = "VkAPI.NET " + VkAPI.Version;
//
//                _response = (HttpWebResponse)(await _request.GetResponseAsync());
//
//                Stream responseStream = _response.GetResponseStream();
//
//                if (responseStream == null)
//                {
//                    throw new ApiRequestNullResult("Не получено никаких данных");
//                }
//                var sr = new StreamReader(responseStream);
//         
//                var result = await sr.ReadToEndAsync();
//
//                _response.Close();
//                responseStream.Dispose();
//                sr.Dispose();
//
//                return result;
//            }
//            catch (Exception e)
//            {
//                throw new ApiRequestNullResult("Неизвестная ошибка: " + e.Message);
//            }
//        }






    public class SubjectViewModel
    {
        public string SubjectUrl { get; set; }
    }

    public class AuthorizeViewModel
    {
        public string Url { get; set; }
    }
}
