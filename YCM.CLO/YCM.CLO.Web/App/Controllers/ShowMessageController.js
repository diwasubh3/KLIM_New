var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var ShowMessageController = (function () {
            function ShowMessageController(dataService, $window, $scope, $modalInstance, ngTableParams, sourcedata) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                this.isLoading = false;
                this.statusText = "Loading";
                this.cancel = function () {
                    var vm = _this;
                    vm.statusText = "Closing";
                    vm.modalInstance.dismiss('cancel');
                };
                this.okay = function () {
                    var vm = _this;
                    vm.modalInstance.close(true);
                };
                var vm = this;
                vm.modalInstance = $modalInstance;
                vm.dataService = dataService;
                vm.scope = $scope;
                vm.data = sourcedata;
            }
            return ShowMessageController;
        }());
        ShowMessageController.$inject = ["application.services.dataService", "$window", "$scope", "$modalInstance", 'NgTableParams', 'sourcedata'];
        Controllers.ShowMessageController = ShowMessageController;
        angular.module("app").controller("application.controllers.showMessageController", ShowMessageController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=ShowMessageController.js.map