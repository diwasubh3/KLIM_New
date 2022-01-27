var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var TradeHistoryPopupController = (function () {
            function TradeHistoryPopupController(dataService, $window, $scope, $modalInstance, ngTableParams, sourcedata) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                this.isLoading = false;
                this.statusText = "Loading";
                this.loadTradeHistoryData = function () {
                    var vm = _this;
                    vm.statusText = "Loading";
                    vm.isLoading = true;
                    vm.dataService.getTradeHistory(vm.securitycode).then(function (d) {
                        vm.tradeHistoryDetails = d;
                        vm.isLoading = false;
                    });
                };
                this.cancel = function () {
                    var vm = _this;
                    vm.statusText = "Closing";
                    vm.modalInstance.dismiss('cancel');
                };
                var vm = this;
                vm.modalInstance = $modalInstance;
                vm.dataService = dataService;
                vm.scope = $scope;
                vm.windowService = $window;
                vm.sourcedata = sourcedata;
                vm.ngTableParams = ngTableParams;
                if (sourcedata.securitycode !== undefined) {
                    vm.securitycode = sourcedata.securitycode;
                    vm.issuer = sourcedata.issuer;
                    vm.loadTradeHistoryData();
                }
                vm.lastUpdatedOn = new Date();
            }
            return TradeHistoryPopupController;
        }());
        TradeHistoryPopupController.$inject = ["application.services.dataService", "$window", "$scope", "$uibModalInstance", 'NgTableParams', 'sourcedata'];
        Controllers.TradeHistoryPopupController = TradeHistoryPopupController;
        angular.module("app").controller("application.controllers.tradeHistoryPopupController", TradeHistoryPopupController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=TradeHistoryPopupController.js.map