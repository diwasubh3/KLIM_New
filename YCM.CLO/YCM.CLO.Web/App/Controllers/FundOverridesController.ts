module Application.Controllers {
    export class FundOverridesController {
        dataService: Application.Services.Contracts.IDataService;
        rootScope: ng.IRootScopeService;

        isLoading: boolean;
        data: Array<Application.Models.UserModel>;
        appBasePath: string = pageOptions.appBasePath;
        ngTableParams: any;
        statusText: string;
        dateOptions: any = {
            formatYear: 'yy',
            startingDay: 1
        };
		tableParams: any;
		assetParThreshold: number;
        modalService: angular.ui.bootstrap.IModalService;
        filter: ng.IFilterService;
        selectedFund: Models.IFund;
        savedFund: Models.IFund;
        selectedSummary: Models.ISummary;
        funds: Array<Models.IFund>;

        timeOutService: ng.ITimeoutService;
        static $inject = ["application.services.dataService", "$rootScope", '$modal', 'NgTableParams', '$filter', '$timeout'];

        constructor(dataService: Application.Services.Contracts.IDataService, $rootScope: ng.IRootScopeService, modalService: angular.ui.bootstrap.IModalService, ngTableParams: NgTableParams, $filter: ng.IFilterService, timeOutService: ng.ITimeoutService) {
            var vm = this;
            vm.dataService = dataService;
            vm.rootScope = $rootScope;
            vm.rootScope.$emit('onActivated', 'maintenance');
            vm.modalService = modalService;
            vm.ngTableParams = ngTableParams;
			vm.filter = $filter;
            vm.loadFunds();

            vm.rootScope.$on('onFundChanged', (event, data: Models.ISummary) => {
                if (!vm.selectedFund)  {
                    vm.onSummaryChanged(data);
                }
            });

            vm.rootScope.$on('onFundSelectionChanged', (event, data: Models.ISummary) => {
                vm.onSummaryChanged(data);
            });
        }

		loadFunds = () => {
            var vm = this;
            vm.statusText = "Loading";
            vm.isLoading = true;
            vm.dataService.getFunds().then(funds => {
                vm.funds = funds;
                vm.isLoading = false;
                if (!vm.selectedFund) {
                    vm.onSummaryChanged(vm.rootScope['selectedFund']);
                }
            });
        }

        onSummaryChanged = (data: Models.ISummary) => {
            var vm = this;
            if (data) {
				vm.selectedFund = vm.funds.filter(f => { return f.fundId === data.fundId })[0];
				vm.updateAssetParThreshold();
            }
        }

        openDate = ($event: any, prop: any) => {
            var vm = this;
            $event.preventDefault();
            vm.selectedFund[prop] = true;
        }

        save = () => {
            var vm = this;
            vm.statusText = "Saving";
            vm.isLoading = true;
            vm.dataService.saveFund(vm.selectedFund).then(fund => {
                vm.isLoading = false;
                vm.savedFund = vm.selectedFund;
                vm.rootScope.$emit('refreshSummaries', null);
            });
        }

		updateAssetParThreshold = () => {
			var vm = this;
			vm.assetParThreshold = vm.selectedFund.assetParPercentageThreshold * vm.selectedFund.targetPar / 100;
		}
	}

    angular.module("app").controller("application.controllers.fundOverridesController", FundOverridesController);
} 