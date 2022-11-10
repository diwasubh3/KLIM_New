var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var AboutController = (function () {
            function AboutController($rootScope, modalService) {
                this.appBasePath = pageOptions.appBasePath;
                this.loadData = function () {
                };
                this.rootScope = $rootScope;
                this.rootScope.$emit('onActivated', 'about');
                this.modalService = modalService;
                this.loadData();
            }
            return AboutController;
        }());
        AboutController.$inject = ["$rootScope", '$modal'];
        Controllers.AboutController = AboutController;
        angular.module("app").controller("application.controllers.aboutController", AboutController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=AboutController.js.map