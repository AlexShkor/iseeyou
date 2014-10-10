using System;
using System.Web;
using System.Web.Security;
using ISeeYou.Helpers;
using ISeeYou.Platform.Domain;
using ISeeYou.Views;
using ISeeYou.ViewServices;

namespace ISeeYou.Common.Account
{
    public class Claims
    {
        public const string Admin = "Admin";
    }

    public class AuthenticationService
    {
        private readonly UsersViewService _users;
        private readonly CryptographicHelper _crypto;
        private readonly ICommandBus _bus;

        public AuthenticationService(UsersViewService users, CryptographicHelper crypto, ICommandBus bus)
        {
            _users = users;
            _crypto = crypto;
            _bus = bus;
        }

        public bool Logon(string userName, string password, bool persist)
        {
            var user = _users.GetByEmail(userName);
            if (user != null && _crypto.GetPasswordHash(password, user.PasswordSalt) == user.PasswordHash)
            {
                var authTicket = new FormsAuthenticationTicket(
                    1,
                    user.Email,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(20),
                    persist,
                    null);

                var encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                HttpContext.Current.Response.Cookies.Add(authCookie);

                return true;
            }

            return false;
        }

        public void LoginVerifiedUser(UserView user, bool persist)
        {
            var authTicket = new FormsAuthenticationTicket(
                1,
                user.Email,
                DateTime.Now,
                DateTime.Now.AddMinutes(30),
                persist,null);

            var encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            HttpContext.Current.Response.Cookies.Add(authCookie);
        }

        public void LogOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}