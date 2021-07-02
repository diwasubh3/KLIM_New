using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(YCM.CLO.Web.Startup))]
namespace YCM.CLO.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
