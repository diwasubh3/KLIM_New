var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var ConfirmTradeSwapRunController = (function () {
            function ConfirmTradeSwapRunController(dataService, $window, $scope, $modalInstance, sourcedata) {
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
            return ConfirmTradeSwapRunController;
        }());
        ConfirmTradeSwapRunController.$inject = ["application.services.dataService", "$window", "$scope", "$modalInstance", 'sourcedata'];
        Controllers.ConfirmTradeSwapRunController = ConfirmTradeSwapRunController;
        angular.module("app").controller("application.controllers.confirmTradeSwapRunController", ConfirmTradeSwapRunController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=ConfirmTradeSwapRunController.js.map