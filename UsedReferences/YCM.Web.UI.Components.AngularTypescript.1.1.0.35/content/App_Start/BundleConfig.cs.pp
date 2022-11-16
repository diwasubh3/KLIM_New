using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace $rootnamespace$
{
    public static class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));


            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate.unobtrusive.js",
                "~/Scripts/jquery.validate.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/YorkScripts").Include(
                "~/Scripts/YorkNet/YorkNet.js",
				"~/Scripts/YorkNet/YorkNetApp.js",
                "~/Scripts/YorkNet/YorkNetNav.js",
                "~/Scripts/YorkNet/YorkNetAll.js",
                "~/Scripts/jquery.unhandledexception.js"));

            bundles.Add(new ScriptBundle("~/bundles/VendorJS").Include(
                "~/Scripts/angular.js",
                "~/Scripts/angular-route.js",
                "~/Scripts/moment.js",
                "~/Scripts/file-input/bootstrap.file-input.js",
                "~/Scripts/angular-ui/ui-bootstrap-tpls.js",
                "~/Scripts/DatePicker/angular-bootstrap-datepicker.js",
                "~/Scripts/ng-table.js",
                "~/Scripts/file-input/ng-upload.js",
                "~/Scripts/bootstrap-contextmenu.js",
                 "~/Scripts/angular-sanitize.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/AppJS").Include(
               "~/App/app.route.js",
               "~/App/app.js",
               "~/App/Models/UserModel.js",
               "~/App/Services/IHomeService.js",
               "~/App/Services/HomeService.js",
               "~/App/Directives/Directives.js",
               "~/App/Factories/DeviceType.js",
               "~/App/Factories/HttpWrapperFactory.js",
               "~/App/Controllers/TopNavController.js",
               "~/App/Controllers/HomeController.js",
               "~/App/Controllers/AboutController.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/YorkScriptsFinal").Include(
                "~/Scripts/YorkNet/YorkNetFinal.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-{version}.js"));

            bundles.Add(new StyleBundle("~/Content/Yorknet/YorkMenuBootstrapCss").Include(
				"~/Content/YorkNet/YorkBootstrap.css", "~/Content/Yorknet/font-awesome.css"
                ));

			bundles.Add(new StyleBundle("~/Content/BootstrapCss").Include(
				"~/Content/bootstrap/bootstrap.css", "~/Content/YorkNet/Bootstrap2xTo3x.css"));


            bundles.Add(new StyleBundle("~/Content/BootstrapCssResponsive").Include(
                "~/Content/YorkNet/YorkBootstrapResponsive.css"));

            bundles.Add(new StyleBundle("~/Content/JqueryValCss").Include(
                "~/Content/bootstrap-mvc-validation.css",
                "~/Content/validation-error.css"));

            bundles.Add(new StyleBundle("~/Content/YorkCss").Include(
                "~/Content/YorkNet/YorkCss.css",
				"~/Content/YorkNet/YorkCssResponsive.css"));

			bundles.Add(new StyleBundle("~/Content/YorkCssSP").Include("~/Content/less/YorkCssSP.css",
                "~/Content/YorkNet/YorkCssResponsiveSP.css"));

        }
    }
}