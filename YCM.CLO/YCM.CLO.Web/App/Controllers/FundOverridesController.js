var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var FundOverridesController = (function () {
            function FundOverridesController(dataService, $rootScope, modalService, ngTableParams, $filter, timeOutService) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                this.dateOptions = {
                    formatYear: 'yy',
                    startingDay: 1
                };
                this.loadFunds = function () {
                    var vm = _this;
                    vm.statusText = "Loading";
                    vm.isLoading = true;
                    vm.dataService.getFunds().then(function (funds) {
                        vm.funds = funds;
                        vm.isLoading = false;
                        if (!vm.selectedFund) {
                            vm.onSummaryChanged(vm.rootScope['selectedFund']);
                        }
                    });
                };
                this.onSummaryChanged = function (data) {
                    var vm = _this;
                    if (data) {
                        vm.selectedFund = vm.funds.filter(function (f) { return f.fundId === data.fundId; })[0];
                        vm.updateAssetParThreshold();
                    }
                };
                this.openDate = function ($event, prop) {
                    var vm = _this;
                    $event.preventDefault();
                    vm.selectedFund[prop] = true;
                };
                this.save = function () {
                    var vm = _this;
                    vm.statusText = "Saving";
                    vm.isLoading = true;
                    vm.dataService.saveFund(vm.selectedFund).then(function (fund) {
                        vm.isLoading = false;
                        vm.savedFund = vm.selectedFund;
                        vm.rootScope.$emit('refreshSummaries', null);
                    });
                };
                this.updateAssetParThreshold = function () {
                    var vm = _this;
                    vm.assetParThreshold = vm.selectedFund.assetParPercentageThreshold * vm.selectedFund.targetPar / 100;
                };
                var vm = this;
                vm.dataService = dataService;
                vm.rootScope = $rootScope;
                vm.rootScope.$emit('onActivated', 'maintenance');
                vm.modalService = modalService;
                vm.ngTableParams = ngTableParams;
                vm.filter = $filter;
                vm.loadFunds();
                vm.rootScope.$on('onFundChanged', function (event, data) {
                    if (!vm.selectedFund) {
                        vm.onSummaryChanged(data);
                    }
                });
                vm.rootScope.$on('onFundSelectionChanged', function (event, data) {
                    vm.onSummaryChanged(data);
                });
            }
            return FundOverridesController;
        }());
        FundOverridesController.$inject = ["application.services.dataService", "$rootScope", '$modal', 'NgTableParams', '$filter', '$timeout'];
        Controllers.FundOverridesController = FundOverridesController;
        angular.module("app").controller("application.controllers.fundOverridesController", FundOverridesController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=FundOverridesController.js.map