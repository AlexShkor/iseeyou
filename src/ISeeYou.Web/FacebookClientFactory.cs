using System.Web;
using Facebook;
using ISeeYou.Platform.Mvc;

namespace ISeeYou.Web
{
    public class FacebookClientFactory
    {
        public FacebookClient GetClient()
        {
            var facebookClient = new FacebookClient();
            try
            {
                var token = HttpContext.Current.Session[BaseController.SessionKeys.FbAccessToken] as string;
                facebookClient.AccessToken = token;
            }
            catch
            {

            }
            return facebookClient;
        }
    }
}