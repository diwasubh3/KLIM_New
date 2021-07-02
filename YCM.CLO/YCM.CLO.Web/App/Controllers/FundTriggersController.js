/// <reference path="../../scripts/typings/lodash/lodash.d.ts" />
var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var FundTriggersController = (function () {
            //constructor(dataService: Application.Services.Contracts.IDataService, $scope: ng.IScope, $rootScope: ng.IRootScopeService, modalService: angular.ui.bootstrap.IModalService, ngTableParams: NgTableParams, $filter: ng.IFilterService, timeOutService: ng.ITimeoutService) {
            function FundTriggersController(dataService, $rootScope, modalService, ngTableParams, $filter, timeOutService) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                this.statusText = "Loading";
                this.loadFunds = function () {
                    var vm = _this;
                    vm.isLoading = true;
                    vm.dataService.getFunds().then(function (funds) {
                        vm.funds = funds;
                        vm.dataService.loadFundRestrictionsTypes().then(function (d) {
                            vm.fundRestrictionTypes = d;
                            vm.fundRestrictionTypes.forEach(function (frt) {
                                frt.displayColorStyle = { 'background-color': frt.displayColor };
                            });
                            vm.dataService.loadOperators().then(function (operators) {
                                vm.operators = operators;
                                vm.isLoading = false;
                                if (vm.rootScope['selectedFund']) {
                                    vm.onSummaryChanged(vm.rootScope['selectedFund']);
                                }
                            });
                        });
                    });
                };
                this.onSummaryChanged = function (data) {
                    var vm = _this;
                    if (data) {
                        vm.selectedSummary = data;
                        vm.selectedFund = vm.funds.filter(function (f) { return f.fundId === vm.selectedSummary.fundId; })[0];
                        vm.loadData();
                    }
                };
                this.loadData = function () {
                    var vm = _this;
                    vm.statusText = "Loading";
                    if (vm.fundRestrictionDict[vm.selectedFund.fundId]) {
                        vm.fundRestrictions = vm.fundRestrictionDict[vm.selectedFund.fundId];
                    }
                    else {
                        vm.isLoading = true;
                        vm.dataService.loadFundRestrictions(vm.selectedFund.fundId).then(function (fundrestrictions) {
                            vm.fundRestrictions = fundrestrictions;
                            vm.fundRestrictionDict[vm.selectedFund.fundId] = fundrestrictions;
                            vm.isLoading = false;
                        });
                    }
                };
                this.save = function () {
                    var vm = _this;
                    vm.statusText = "Loading";
                    vm.isLoading = true;
                    vm.dataService.saveFundRestrictions(vm.fundRestrictions).then(function (fundRestrictions) {
                        vm.fundRestrictions = fundRestrictions;
                        vm.fundRestrictionDict[vm.selectedFund.fundId] = fundRestrictions;
                        vm.rootScope.$emit('refreshFundRestrictions', null);
                        vm.isLoading = false;
                    });
                };
                var vm = this;
                vm.dataService = dataService;
                vm.rootScope = $rootScope;
                //vm.scope = $scope;
                vm.rootScope.$emit('onActivated', 'maintenance');
                vm.modalService = modalService;
                vm.ngTableParams = ngTableParams;
                vm.filter = $filter;
                vm.fundRestrictionDict = {};
                vm.loadFunds();
                vm.rootScope.$on('onFundChanged', function (event, data) {
                    if (vm.funds && !vm.selectedFund) {
                        vm.onSummaryChanged(data);
                    }
                });
                vm.rootScope.$on('onFundSelectionChanged', function (event, data) {
                    vm.onSummaryChanged(data);
                });
            }
            return FundTriggersController;
        }());
        //static $inject = ["application.services.dataService", "$scope", "$rootScope", '$modal', 'NgTableParams', '$filter', '$timeout'];
        FundTriggersController.$inject = ["application.services.dataService", "$rootScope", '$modal', 'NgTableParams', '$filter', '$timeout'];
        Controllers.FundTriggersController = FundTriggersController;
        angular.module("app").controller("application.controllers.fundTriggersController", FundTriggersController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=FundTriggersController.js.map