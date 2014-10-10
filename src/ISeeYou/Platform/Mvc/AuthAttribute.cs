using System.Web.Mvc;
using ISeeYou.Common.Account;

namespace ISeeYou.Platform.Mvc
{
    public class AuthAttribute: AuthorizeAttribute
    {
        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            var user = httpContext.Session[AppConstants.UserIdentitySessionKey] as UserIdentity;
            return user != null; 
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    Data = new
                    {
                        RequestAuthentication = true
                    }
                };
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}