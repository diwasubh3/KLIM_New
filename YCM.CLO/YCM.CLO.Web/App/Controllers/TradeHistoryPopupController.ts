
module Application.Controllers {
    export class TradeHistoryPopupController {
        modalInstance: angular.ui.bootstrap.IModalServiceInstance;
        appBasePath: string = pageOptions.appBasePath;
        isLoading: boolean = false;
        statusText: string = "Loading";
        comments: string;
        sourcedata: any;
        tradeHistoryDetails: Array<Models.ITradeHistory>;
        dataService: Application.Services.Contracts.IDataService;
        ngTableParams: any;
        tableParams: any;
        lastUpdatedOn: Date;
        scope: ng.IScope;
        securitycode: string;
        portfolioName: string;
        windowService: ng.IWindowService;

        static $inject = ["application.services.dataService", "$window", "$scope", "$uibModalInstance", 'NgTableParams', 'sourcedata'];

        constructor(dataService: Application.Services.Contracts.IDataService, $window: ng.IWindowService, $scope: angular.IScope, $modalInstance: angular.ui.bootstrap.IModalServiceInstance, ngTableParams: NgTableParams, sourcedata: any) {
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

        loadTradeHistoryData = () => {
            var vm = this;
            vm.statusText = "Loading";
            vm.isLoading = true;
            vm.dataService.getTradeHistory(vm.securitycode, vm.portfolioName).then((d) => {
                vm.tradeHistoryDetails = d;
                vm.isLoading = false;
            });
        }
        
    }

    angular.module("app").controller("application.controllers.tradeHistoryPopupController", TradeHistoryPopupController);
}
