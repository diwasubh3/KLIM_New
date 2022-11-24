using System.Web.Optimization;

namespace YCM.CLO.Web
{
	public static class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/lodash.min.js",
                "~/Scripts/file-saver/filesaver.min.js"
                ));


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

            bundles.Add(new Bundle("~/bundles/VendorJS").Include(
                 "~/Scripts/angular.min.js",
                 "~/Scripts/angular-route.min.js",
                 "~/Scripts/angular-sanitize.js",
                 "~/Scripts/moment.js",
                 "~/Scripts/file-input/bootstrap.file-input.js",
                 "~/Scripts/angular-ui/ui-bootstrap-tpls.js",
                 "~/Scripts/DatePicker/angular-bootstrap-datepicker.js",
                 "~/Scripts/ng-table.js",
                 "~/Scripts/file-input/ng-upload.js",
                 "~/Scripts/context-menu/contextmenu.js",
                 "~/Scripts/angular-sanitize.js",
                 "~/Scripts/ui-grid.min.js",
                 "~/Scripts/angular-animate.js",
                 "~/Scripts/angular-ui/export/csv.min.js",
                 "~/Scripts/angular-ui/export/pdfmake/pdfmake.min.js",
                 "~/Scripts/angular-ui/export/pdfmake/vfs_fonts.js",
                 "~/Scripts/bootstrap-toggle.js",
                 "~/Scripts/charts/chart.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/AppJS").Include(
               "~/App/app.route.js",
               "~/App/app.js",
               "~/App/Directives/exportUiGridService.js",
                "~/App/Directives/Directives.js",
               "~/Scripts/formatting/angular-formatting.js",
               "~/App/Models/UserModel.js",
               "~/App/Services/IDataService.js",
               "~/App/Services/DataService.js",
               "~/App/Services/IUIService.js",
               "~/App/Services/UIService.js",
               "~/App/Factories/DeviceType.js",
               "~/App/Factories/HttpWrapperFactory.js",
               "~/App/Controllers/TopNavController.js",
               "~/App/Controllers/Top10Bottom10Controller.js",
               "~/App/Controllers/PositionsController.js",
			   "~/App/Controllers/TestResultsController.js",
               "~/App/Controllers/TradesController.js",
               "~/App/Controllers/FundTriggersController.js",
               "~/App/Controllers/SecurityOverridesController.js",
               "~/App/Controllers/FundOverridesController.js",
               "~/App/Controllers/ParametersController.js",
               "~/App/Controllers/AnalystResearchController.js",
               "~/App/Controllers/WatchController.js",
               "~/App/Controllers/AddUpdateParamterValueController.js",
               "~/App/Controllers/AddUpdateSecurityOverrideController.js",
               "~/App/Controllers/AddNewSecurityController.js",
               "~/App/Controllers/NewSecurityReconController.js",
               "~/App/Controllers/AddUpdateAnalystResearchController.js",
               "~/App/Controllers/BidOfferUploadController.js",
               "~/App/Controllers/ConfirmSecurityReconController.js",
               "~/App/Controllers/LoanAttributeOverrideReconController.js",
               "~/App/Controllers/ShowMessageController.js",
               "~/App/Controllers/BuySellTradeController.js",
			   "~/App/Controllers/AnalystResearchPopupController.js",
               "~/App/Controllers/TradeHistoryPopupController.js",
               "~/App/Controllers/ViewEditorPopupController.js",
			   "~/App/Controllers/ConfirmCustomViewDeleteController.js",
			   "~/App/Controllers/BbgPopupController.js",
               "~/App/Controllers/TradeSwappingController.js",
               "~/App/Controllers/ConfirmTradeSwapRunController.js",
               "~/App/Controllers/LoanComparisonController.js",
               "~/App/Controllers/CustomModalFilterCtrl.js",
               "~/App/Controllers/AdminController.js",
               "~/App/Controllers/FilterPositionsController.js",
               "~/App/Controllers/CollateralQualityMatrixController.js",
               "~/App/Controllers/ConfirmMatrixPointController.js",
               "~/App/Controllers/ReportingController.js",
               "~/App/Controllers/PaydownController.js",
               "~/App/Controllers/TradeBookingController.js",
               "~/App/Controllers/DayOverChangesController.js",
                "~/App/Controllers/ChartsPopupController.js"
               ));

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

            bundles.Add(new StyleBundle("~/Content/AppCss").Include(
                "~/Content/Site.css",
                "~/Content/font-awesome.min.css",
                "~/Content/ui-grid.css",
                "~/Content/bootstrap-toggle.css"
                ));

            bundles.Add(new StyleBundle("~/Content/YorkCssSP").Include("~/Content/less/YorkCssSP.css",
                "~/Content/YorkNet/YorkCssResponsiveSP.css"));

        }
    }
}