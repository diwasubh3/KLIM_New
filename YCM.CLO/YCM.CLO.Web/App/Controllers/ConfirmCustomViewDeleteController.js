var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var ConfirmCustomViewDeleteController = (function () {
            function ConfirmCustomViewDeleteController(dataService, $window, $scope, $modalInstance, sourcedata) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                this.isLoading = false;
                this.statusText = "Saving";
                this.cancel = function () {
                    var vm = _this;
                    vm.modalInstance.dismiss('cancel');
                };
                this.proceed = function () {
                    var vm = _this;
                    vm.modalInstance.close({ confirm: true });
                };
                this.tradeSwap = sourcedata;
                this.modalInstance = $modalInstance;
                this.dataService = dataService;
            }
            return ConfirmCustomViewDeleteController;
        }());
        ConfirmCustomViewDeleteController.$inject = ["application.services.dataService", "$window", "$scope", "$modalInstance", 'sourcedata'];
        Controllers.ConfirmCustomViewDeleteController = ConfirmCustomViewDeleteController;
        angular.module("app").controller("application.controllers.confirmCustomViewDeleteController", ConfirmCustomViewDeleteController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=ConfirmCustomViewDeleteController.js.map