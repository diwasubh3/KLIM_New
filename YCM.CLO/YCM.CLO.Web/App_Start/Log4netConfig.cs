using System.IO;

namespace YCM.CLO.Web
{
    public class Log4netConfig
    {
        public static void ConfigureLog4Net(System.Web.HttpServerUtility server)
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo(server.MapPath("~/Web.config")));
        }
    }
}