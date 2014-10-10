using System;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using ISeeYou.Hubs;
using ISeeYou.Platform.Mvc;

namespace ISeeYou.Web.Controllers
{
    [RoutePrefix("chat")]
    public class ChatController : BaseController
    {
        [POST("send")]
        public ActionResult Send(string message)
        {
            UsersHub.CurrentContext.Clients.All.chatMessage(new
            {
                Content = message,
                Time = DateTime.Now.ToShortTimeString(),
                Name = UserName
            });
            return new ContentResult();
        }

    }
}
