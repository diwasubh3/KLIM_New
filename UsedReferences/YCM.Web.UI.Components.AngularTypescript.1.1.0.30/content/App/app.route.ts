module Application {
    declare var pageOptions: any;
    export class Routes {

        static $inject = ["$routeProvider"];
        static configureRoutes($routeProvider: ng.route.IRouteProvider) {

            $routeProvider.when("/home", { controller: "application.controllers.homeController", templateUrl: "/App/Views/home.html", controllerAs: "home" })
                .when("/about", { controller: "application.controllers.aboutController", templateUrl: "/App/Views/about.html" , controllerAs: "about" });

            $routeProvider.otherwise({ redirectTo: "/home" });
        }
    }

}
 