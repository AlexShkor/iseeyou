using System.Web.Mvc;
using ISeeYou.Authentication;
using ISeeYou.Platform.Domain;
using ISeeYou.Platform.Domain.Interfaces;
using MongoDB.Bson;
using StructureMap.Attributes;

namespace ISeeYou.Platform.Mvc
{
    public abstract class BaseController : AsyncController
    {
        private const string DefaultUsername = "Guest";

        public static class SessionKeys
        {
            public const string UserId = "_UserId";
            public const string UserName = "_UserName";
            public const string FbCsrfToken = "fb_csrf_token";
            public const string FbAccessToken = "fb_access_token";
            public const string FbExpiresIn = "fb_expires_in";
        }

        [SetterProperty]
        public ICommandBus CommandBus { get; set; }

        protected string UserId
        {
            get
            {
                return Request.IsAuthenticated ? ((AkqIdentity)User.Identity).Id : null;
            }
        }

        protected string UserName
        {
            get
            {
                return Request.IsAuthenticated ? User.Identity.Name : null;
            }
        }

        protected string GenerateId()
        {
            return ObjectId.GenerateNewId().ToString();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }

        protected void Send(params ICommand[] commands)
        {
            foreach (var command in commands)
            {
                command.Metadata.UserId = UserId;
            }
            CommandBus.Send(commands);
        }
    }
}
