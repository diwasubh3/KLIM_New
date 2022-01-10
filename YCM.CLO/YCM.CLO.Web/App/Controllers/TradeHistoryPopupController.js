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
                    vm.dataService.getTradeHistory(vm.securitycode, vm.portfolioName).then(function (d) {
                        vm.tradeHistoryDetails = d;
                        vm.isLoading = false;
                    });
                };
                var vm = this;
                vm.modalInstance = $modalInstance;
                vm.dataService = dataService;
                vm.scope = $scope;
                vm.windowService = $window;
                vm.sourcedata = sourcedata;
                vm.ngTableParams = ngTableParams;
                if (sourcedata.securitycode !== undefined && sourcedata.portfolioName !== undefined) {
                    vm.securitycode = sourcedata.securitycode;
                    vm.portfolioName = sourcedata.portfolioName;
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