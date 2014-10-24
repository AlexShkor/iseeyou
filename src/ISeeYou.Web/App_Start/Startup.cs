using ISeeYou.Web;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace ISeeYou.Web
{

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ContainerConfig.Configure(app);
            CouponsInitializer.Setup();
        }
    }
}