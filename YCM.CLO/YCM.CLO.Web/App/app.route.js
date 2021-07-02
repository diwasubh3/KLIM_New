var Application;
(function (Application) {
    var Routes = (function () {
        function Routes() {
        }
        Routes.configureRoutes = function ($locationProvider, $routeProvider) {
            $locationProvider.hashPrefix('');
            $routeProvider.when("/top10Bottom10", { controller: "application.controllers.top10Bottom10Controller", templateUrl: pageOptions.appBasePath + "/App/Views/top10bottom10.html?v=" + pageOptions.version, controllerAs: "top10bottom10" })
                .when("/positions", { controller: "application.controllers.positionsController", templateUrl: function () {
                    var hurl = pageOptions.appBasePath + "/App/Views/positions.html?v=" + pageOptions.version;
                    //alert(dataService == null);
                    //alert($http == null);
                    return hurl;
                },
                controllerAs: "positions" })
                .when("/trades", { controller: "application.controllers.tradesController", templateUrl: pageOptions.appBasePath + "/App/Views/trades.html?v=" + pageOptions.version, controllerAs: "trades" })
                .when("/tradeswapping", { controller: "application.controllers.tradeSwappingController", templateUrl: pageOptions.appBasePath + "/App/Views/tradeswapping.html?v=" + pageOptions.version, controllerAs: "tradeswapping" })
                .when("/fundtriggers", { controller: "application.controllers.fundTriggersController", templateUrl: pageOptions.appBasePath + "/App/Views/fundtriggers.html?v=" + pageOptions.version, controllerAs: "fundtriggers" })
                .when("/securityoverrides", { controller: "application.controllers.securityOverridesController", templateUrl: pageOptions.appBasePath + "/App/Views/securityoverrides.html?v=" + pageOptions.version, controllerAs: "securityoverrides" })
                .when("/addnewsecurity", { controller: "application.controllers.addNewSecurityController", templateUrl: pageOptions.appBasePath + "/App/Views/addnewsecurity.html?v=" + pageOptions.version, controllerAs: "newsecurity" })
                .when("/newsecurityrecon", { controller: "application.controllers.newSecurityReconController", templateUrl: pageOptions.appBasePath + "/App/Views/newsecurityrecon.html?v=" + pageOptions.version, controllerAs: "newsecurityrecon" })
                .when("/fundoverrides", { controller: "application.controllers.fundOverridesController", templateUrl: pageOptions.appBasePath + "/App/Views/fundoverrides.html?v=" + pageOptions.version, controllerAs: "fundoverrides" })
                .when("/parameters", { controller: "application.controllers.parametersController", templateUrl: pageOptions.appBasePath + "/App/Views/parameters.html?v=" + pageOptions.version, controllerAs: "parameters" })
                .when("/bidofferupload", { controller: "application.controllers.bidOfferUploadController", templateUrl: pageOptions.appBasePath + "/App/Views/bidofferupload.html?v=" + pageOptions.version, controllerAs: "bidofferupload" })
                .when("/collateralqualitymatrix", { controller: "application.controllers.collateralQualityMatrixController", templateUrl: pageOptions.appBasePath + "/App/Views/collateralqualitymatrix.html?v=" + pageOptions.version, controllerAs: "collateralqualitymatrix" })
                .when("/loanattributeoverriderecon", { controller: "application.controllers.loanAttributeOverrideReconController", templateUrl: pageOptions.appBasePath + "/App/Views/loanattributeoverriderecon.html?v=" + pageOptions.version, controllerAs: "laor" })
                .when("/t1", { controller: "application.controllers.buySellTradeController", templateUrl: pageOptions.appBasePath + "/App/Views/buyselltrade.html?v=" + pageOptions.version, controllerAs: "buyselltrade" })
                .when("/analystresearch", { controller: "application.controllers.analystResearchController", templateUrl: pageOptions.appBasePath + "/App/Views/analystresearch.html?v=" + pageOptions.version, controllerAs: "analystresearch" })
                .when("/admin", { controller: "application.controllers.adminController", templateUrl: pageOptions.appBasePath + "/App/Views/admin.html?v=" + pageOptions.version, controllerAs: "admin" })
                .when("/reporting", { controller: "application.controllers.reportingController", templateUrl: pageOptions.appBasePath + "/App/Views/reporting.html?v=" + pageOptions.version, controllerAs: "reporting" });
            $routeProvider.otherwise({ redirectTo: "/positions" });
        };
        return Routes;
    }());
    Routes.$inject = ["$locationProvider", "$routeProvider"];
    Application.Routes = Routes;
})(Application || (Application = {}));
//# sourceMappingURL=app.route.js.map