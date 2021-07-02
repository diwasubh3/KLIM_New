using System.Web;
using System.Web.Optimization;

namespace YCM.CLO.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/YorkScripts").Include(
               "~/Scripts/jquery-{version}.js",
               "~/Scripts/bootstrap.js",
              "~/Scripts/YorkNet/YorkNet.js",
              "~/Scripts/YorkNet/YorkNetApp.js",
              "~/Scripts/YorkNet/YorkNetNav.js",
              "~/Scripts/YorkNet/YorkNetFinal.js"
              ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/Yorknet/YorkMenuBootstrapCss").Include(

                "~/Content/YorkNet/YorkBootstrap.css",
                "~/Content/Yorknet/font-awesome.css",
                "~/Content/Yorknet/GoogleFontCss.css",
                "~/Content/GoogleFontCss.css",
                "~/Content/YorkNet/Bootstrap2xTo3x.css",
                "~/Content/YorkNet/YorkBootstrapResponsive.css",
                "~/Content/YorkNet/YorkCss.css",
               "~/Content/YorkNet/YorkCssResponsive.css",
               "~/Content/bootstrap.css",
               "~/Content/bootstrap-grid.css"
                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/site.css"));
        }
    }
}