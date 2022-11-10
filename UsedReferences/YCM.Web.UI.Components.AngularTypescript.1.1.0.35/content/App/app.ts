((): void => {
    var mainApp = angular.module("app", ['ngRoute', 'ngUpload', 'ui.bootstrap', 'ng-bootstrap-datepicker', 'ngTable']);
    mainApp.config(['$routeProvider', Application.Routes.configureRoutes]);
})();
 