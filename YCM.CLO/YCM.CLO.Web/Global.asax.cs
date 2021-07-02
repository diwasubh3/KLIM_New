using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using YCM.CLO.Web.App_Start;

namespace YCM.CLO.Web
{
	public class MvcApplication : HttpApplication
	{
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Log4netConfig.ConfigureLog4Net(Server);
            AutoMapperConfig.RegisterMappings();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;
            var context = app.Context;
            // get the exception that was unhandled
            Exception ex = context.Server.GetLastError();

            // TODO: log, send the exception by mail
        }
    }
}
