using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace CLO.Admin
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

            //bundles.Add(new ScriptBundle("~/bundles/YorkPluginScripts").Include(""));

            bundles.Add(new ScriptBundle("~/bundles/YorkScriptsFinal").Include(
                "~/Scripts/YorkNet/YorkNetFinal.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-{version}.js"));

            bundles.Add(new StyleBundle("~/Content/Yorknet/YorkMenuBootstrapCss").Include(
				"~/Content/YorkNet/YorkBootstrap.css", "~/Content/Yorknet/font-awesome.css", "~/Content/GoogleFontCss.css"
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