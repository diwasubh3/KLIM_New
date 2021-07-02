/// <reference path="../../scripts/typings/lodash/lodash.d.ts" />

module Application.Controllers {
    export class FundTriggersController {
        dataService: Application.Services.Contracts.IDataService;
        rootScope: ng.IRootScopeService;
		//scope: ng.IScope;
        isLoading: boolean;
        fundRestrictions: Array<Application.Models.IFundRestriction>;
        fundRestrictionTypes: Array<Application.Models.IFundRestrictionsTypes>;
        operators: Array<Models.IOperator>;
        appBasePath: string = pageOptions.appBasePath;
        ngTableParams: any;
        statusText: string = "Loading";
        tableParams: any;
        modalService: angular.ui.bootstrap.IModalService;
        filter: ng.IFilterService;
        selectedFund: Models.IFund;
        selectedSummary: Models.ISummary;
        funds : Array<Models.IFund>;
        timeOutService: ng.ITimeoutService;
        fundRestrictionDict: any;
		//static $inject = ["application.services.dataService", "$scope", "$rootScope", '$modal', 'NgTableParams', '$filter', '$timeout'];
	    static $inject = ["application.services.dataService", "$rootScope", '$modal', 'NgTableParams', '$filter', '$timeout'];

		//constructor(dataService: Application.Services.Contracts.IDataService, $scope: ng.IScope, $rootScope: ng.IRootScopeService, modalService: angular.ui.bootstrap.IModalService, ngTableParams: NgTableParams, $filter: ng.IFilterService, timeOutService: ng.ITimeoutService) {
	    constructor(dataService: Application.Services.Contracts.IDataService, $rootScope: ng.IRootScopeService, modalService: angular.ui.bootstrap.IModalService, ngTableParams: NgTableParams, $filter: ng.IFilterService, timeOutService: ng.ITimeoutService) {
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
            vm.rootScope.$on('onFundChanged', (event, data: Models.ISummary) => {
                if (vm.funds && !vm.selectedFund) {
                    vm.onSummaryChanged(data);
                }
            });

            vm.rootScope.$on('onFundSelectionChanged', (event, data: Models.ISummary) => {
                vm.onSummaryChanged(data);
            });

        }

        loadFunds = () => {
            var vm = this;
            vm.isLoading = true;
            vm.dataService.getFunds().then(funds => {
                vm.funds = funds;

                vm.dataService.loadFundRestrictionsTypes().then((d) => {
                    vm.fundRestrictionTypes = d;
                    vm.fundRestrictionTypes.forEach(frt => {
                        frt.displayColorStyle = { 'background-color': frt.displayColor };
                    });
                    vm.dataService.loadOperators().then(operators => {
                        vm.operators = operators;
                        vm.isLoading = false;
                        if (vm.rootScope['selectedFund']) {
                            vm.onSummaryChanged(vm.rootScope['selectedFund']);
                        }
                    });
                });
            });
        }

        onSummaryChanged = (data: Models.ISummary) => {
            var vm = this;
            if (data) {
                vm.selectedSummary = data;
                vm.selectedFund = vm.funds.filter(f => { return f.fundId === vm.selectedSummary.fundId })[0];
                vm.loadData();
            }
        }

        loadData = () => {
            var vm = this;
            vm.statusText = "Loading";
            if (vm.fundRestrictionDict[vm.selectedFund.fundId]) {
                vm.fundRestrictions = vm.fundRestrictionDict[vm.selectedFund.fundId];
            } else {
                vm.isLoading = true;
                vm.dataService.loadFundRestrictions(vm.selectedFund.fundId).then(fundrestrictions => {
                    vm.fundRestrictions = fundrestrictions;
                    vm.fundRestrictionDict[vm.selectedFund.fundId] = fundrestrictions;
                    vm.isLoading = false;
                });
            }
        }

		save = () => {
			var vm = this;
			vm.statusText = "Loading";
			vm.isLoading = true;

			vm.dataService.saveFundRestrictions(vm.fundRestrictions).then(fundRestrictions => {
			    vm.fundRestrictions = fundRestrictions;
			    vm.fundRestrictionDict[vm.selectedFund.fundId] = fundRestrictions;
			    vm.rootScope.$emit('refreshFundRestrictions', null);
			    vm.isLoading = false;
			});
		}
    }

    angular.module("app").controller("application.controllers.fundTriggersController", FundTriggersController);
} 