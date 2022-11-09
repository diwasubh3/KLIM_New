var app = angular.module('app', ['ngRoute']);

// configure our routes
app.config(['$routeProvider',function ($routeProvider) {
    $routeProvider

        // route for the home page
        .when('/', {
            templateUrl: '/App/Views/home.html',
            controller: 'mainController'
        })
        .when('/home', {
            templateUrl: '/App/Views/home.html',
            controller: 'mainController'
        })
        // route for the about page
        .when('/about', {
            templateUrl: '/App/Views/about.html',
            controller: 'aboutController'
        });
}]);

app.factory("deviceType", function () { var t = "Phone", n = "Desktop"; return /BlackBerry/.test(navigator.userAgent) ? n = "BB10" : /IEMobile/.test(navigator.userAgent) ? n = "IEMobile" : /iPhone|iPod/.test(navigator.userAgent) ? n = "iPhone" : /iPad/.test(navigator.userAgent) ? (t = "Tablet", n = "iPad") : /Android/.test(navigator.userAgent) ? (/Mobile/.test(navigator.userAgent) || (t = "Tablet"), n = "Android") : t = n, { formFactor: t, type: n } });