var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var TopNavController = (function () {
            function TopNavController($rootScope, $scope) {
                var _this = this;
                this.rootScope = $rootScope;
                this.scope = $scope;
                this.scope.activeView = 'home';
                $rootScope.$on('onActivated', function (event, data) {
                    _this.scope.activeView = data;
                });
            }
            return TopNavController;
        }());
        TopNavController.$inject = ["$rootScope", "$scope"];
        Controllers.TopNavController = TopNavController;
        angular.module("app").controller("topNavController", ['$rootScope', '$scope', function ($rs, scope) { return new TopNavController($rs, scope); }]);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=TopNavController.js.map