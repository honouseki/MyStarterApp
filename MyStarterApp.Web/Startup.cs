using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyStarterApp.Web.Startup))]
namespace MyStarterApp.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
