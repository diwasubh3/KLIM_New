var Application;
(function (Application) {
    var Routes = (function () {
        function Routes() {
        }
        Routes.configureRoutes = function ($routeProvider) {
            $routeProvider.when("/home", { controller: "application.controllers.homeController", templateUrl: "/App/Views/home.html", controllerAs: "home" })
                .when("/about", { controller: "application.controllers.aboutController", templateUrl: "/App/Views/about.html", controllerAs: "about" });
            $routeProvider.otherwise({ redirectTo: "/home" });
        };
        return Routes;
    }());
    Routes.$inject = ["$routeProvider"];
    Application.Routes = Routes;
})(Application || (Application = {}));
//# sourceMappingURL=app.route.js.map