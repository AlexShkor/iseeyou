using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using Facebook;
using ISeeYou.Authentication;
using ISeeYou.Domain.Aggregates.User.Commands;
using ISeeYou.Helpers;
using ISeeYou.Platform.Mongo;
using ISeeYou.Platform.Mvc;
using ISeeYou.ViewServices;
using ISeeYou.Web.Models.Security;
using MongoDB.Bson;
using RestSharp.Extensions;

namespace ISeeYou.Web.Controllers
{
    [RoutePrefix("account")]
    public class AccountController : BaseController
    {
        private readonly UsersViewService _usersService;
        private readonly CryptographicHelper _cryptoHelper;
        private readonly IdGenerator _idGenerator;
        private readonly SiteSettings _settings;
        private readonly AuthenticationService _authenticationService;
        private readonly FacebookClient _fb;

        public string FacebookCallbackUri
        {
            get
            {
                return Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, "").Replace(":" + Request.Url.Port, "") +
                                   Url.Action("FacebookCallback");
            }
        }

        public AccountController(
            UsersViewService usersService, 
            CryptographicHelper cryptoHelper, 
            IdGenerator idGenerator, 
            AuthenticationService authenticationService, 
            FacebookClientFactory fbFactory,
            SiteSettings settings)
        {
            _usersService = usersService;
            _cryptoHelper = cryptoHelper;
            _idGenerator = idGenerator;
            _authenticationService = authenticationService;
            _fb = fbFactory.GetClient();
            _settings = settings;
        }

        [GET("login")]
        public ActionResult Login(string returnUrl)
        {
            return View(new LoginModel{ReturnUrl = returnUrl, RememberMe = true});
        }

        [POST("login")]
        public ActionResult Login(LoginModel loginModel)
        {
            var user = _authenticationService.ValidateUser(loginModel.Email, loginModel.Password);
            var returnUrl = loginModel.ReturnUrl;
            if (user == null)
            {
                ModelState.AddModelError("", "Login failed");
                return View(loginModel);
            }

            _authenticationService.LoginUser(user.Email,user.UserName, true);
            //UserName = user.Username;
            if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                      && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
            {
                return Redirect(returnUrl);
            }
            return Redirect("/");
        }

        [GET("LogOff")]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            System.Web.HttpContext.Current.Session.Abandon();
            System.Web.HttpContext.Current.Session.Clear();

            return Redirect("/");
        }

        [GET("Register")]
        public ActionResult Register(string returnUrl)
        {
            return View(new SignUpModel(){ReturnUrl = returnUrl});
        }

        [POST("Register")]
        public ActionResult Register(SignUpModel signUpModel)
        {
            if (!ModelState.IsValid) return View(signUpModel);

            if (EmailExists(signUpModel.EmailAddress))
            {
                ModelState.AddModelError("", "Email address already in use");
            }
            if (UserNameExists(signUpModel.Username))
            {
                ModelState.AddModelError("", "UserName already in use");
            }
            if (!ModelState.IsValid) return View(signUpModel);

            var cmd = new CreateUser
            {
                Email = signUpModel.EmailAddress,
                Password = signUpModel.Password,
                UserName = signUpModel.Username
            };
            Send(cmd);
            _authenticationService.LoginUser(cmd.Email,cmd.UserName, true);
            if (signUpModel.ReturnUrl.HasValue())
            {
                return Redirect(signUpModel.ReturnUrl);
            }
            return Redirect("/");
        }

        private string GetIdForNewUser()
        {
            return Request.IsAuthenticated ? _idGenerator.Generate() : UserId;
        }

        [FacebookAuthorize]
        [GET("ProcessFacebook")]
        public ActionResult ProcessFacebook(string returnUrl)
        {
            if (Request.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                Session.Abandon();
                Session.Clear();
            }
            dynamic fbUser = _fb.Get("me");
            var user = _usersService.GetByFacebookId((string) fbUser.id);
            if (user == null)
            {
                var cmd = new CreateUser
                {
                    Email = fbUser.email,
                    FacebookId = fbUser.id,
                    UserName = fbUser.name
                };
                Send(cmd);
            }
            _authenticationService.LoginUser(fbUser.email,fbUser.name, true);
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return Redirect("/");
        }

        [GET("LoginWithFacebook")]
        public ActionResult LoginWithFacebook(string returnUrl)
        {
            var csrfToken = Guid.NewGuid().ToString();
            Session[SessionKeys.FbCsrfToken] = csrfToken;
            var state = Convert.ToBase64String(Encoding.UTF8.GetBytes(_fb.SerializeJson(new { returnUrl = returnUrl, csrf = csrfToken })));
            const string scope = "user_about_me,email";
            var fbLoginUrl = _fb.GetLoginUrl(
                new
                {
                    client_id = _settings.FacebookAppId,
                    client_secret = _settings.FacebookSecretKey,
                    redirect_uri = FacebookCallbackUri,
                    response_type = "code",
                    scope = scope,
                    state = state
                });
            return Redirect(fbLoginUrl.AbsoluteUri);
        }

        private ActionResult RedirectToProcessFacebook(string returnUrl = null)
        {
            return RedirectToAction("ProcessFacebook", new { returnUrl });
        }

        [GET("FacebookCallback")]
        public ActionResult FacebookCallback(string code, string state)
        {
            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(state))
                return RedirectToProcessFacebook();

            // first validate the csrf token
            dynamic decodedState;
            try
            {
                decodedState = _fb.DeserializeJson(Encoding.UTF8.GetString(Convert.FromBase64String(state)), null);
                var exepectedCsrfToken = Session[SessionKeys.FbCsrfToken] as string;
                // make the fb_csrf_token invalid
                Session[SessionKeys.FbCsrfToken] = null;

                if (!(decodedState is IDictionary<string, object>) || !decodedState.ContainsKey("csrf") || string.IsNullOrWhiteSpace(exepectedCsrfToken) || exepectedCsrfToken != decodedState.csrf)
                {
                    return RedirectToProcessFacebook();
                }
            }
            catch
            {
                // log exception
                return RedirectToProcessFacebook();
            }

            try
            {
                dynamic result = _fb.Post("oauth/access_token",
                                          new
                                          {
                                              client_id = _settings.FacebookAppId,
                                              client_secret = _settings.FacebookSecretKey,
                                              redirect_uri = FacebookCallbackUri,
                                              code = code
                                          });
                Session[SessionKeys.FbAccessToken] = result.access_token;
                if (result.ContainsKey("expires"))
                    Session[SessionKeys.FbExpiresIn] = DateTime.Now.AddSeconds(result.expires);

                if (decodedState.ContainsKey("returnUrl"))
                {
                    return RedirectToProcessFacebook(decodedState.returnUrl);
                }

                return RedirectToProcessFacebook();
            }
            catch
            {
                // log exception
                return RedirectToProcessFacebook();
            }
        }

        [GET("change-password")]
        [GET("assets/templates/change-password", IgnoreRoutePrefix = true)]
        public ActionResult ChangePassword()
        {
            return PartialView(new ChangePasswordModel());
        }

        [POST("change-password")]
        public ActionResult ChangePasswordPost(ChangePasswordModel model)
        {
            var user = _usersService.GetById(UserId);
            if (user.PasswordHash != _cryptoHelper.GetPasswordHash(model.OldPassword, user.PasswordSalt))
            {
                ModelState.AddModelError("OldPassword", "Old password is incorrect.");
            }
            if (ModelState.IsValid)
            {
                var cmd = new ChangePassword
                {
                    Id = user.Id,
                    NewPassword = model.NewPassword,
                };
                Send(cmd);
                return Redirect("/");
            }
            return View("ChangePassword",model);
        }


        [GET("forgot")]
        public ActionResult Forgot(string email)
        {
            return View((object)email);
        }

        [POST("reset")]
        public ActionResult Reset(string email)
        {
            var user = _usersService.GetByEmail(email);
            if (user == null)
            {
                ModelState.AddModelError("email", "User with this email is not registred.");
            }
            if (ModelState.IsValid)
            {
                var newPass = GenerateNewPassword(7);
                var cmd = new ResetPassword
                {
                    Id = email
                };
                Send(cmd);
                return Redirect("/");
            }
            return View("Forgot",(object)email);
        }

        private string GenerateNewPassword(int length)
        {
            var result = "";
            var symbols = ObjectId.GenerateNewId().ToString().ToCharArray();
            var random = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < length; i++)
            {
                result += symbols[random.Next(symbols.Length - 1)];           
            }
            return result;
        }

        bool EmailExists(string email)
        {
            return _usersService.GetByEmail(email) != null;
        }

        bool UserNameExists(string userName)
        {
            return _usersService.GetUserName(userName) != null;
        }
    }
}