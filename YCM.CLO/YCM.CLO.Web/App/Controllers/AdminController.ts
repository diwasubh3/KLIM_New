module Application.Controllers {
    export class AdminController {
        dataService: Application.Services.Contracts.IDataService;
        uiService: Application.Services.Contracts.IUIService;
        rootScope: ng.IRootScopeService;

        isLoading: boolean;
        data: Array<Application.Models.ITrade>;
        appBasePath: string = pageOptions.appBasePath;
        ngTableParams: any;
        statusText: string = "Loading";
        selectedFund: Models.ISummary;
        tableParams: any;
        modalService: angular.ui.bootstrap.IModalService;
        filter: ng.IFilterService;
        timeOutService: ng.ITimeoutService;
        error: string;
        success: string;
        
        static $inject = ["application.services.uiService","application.services.dataService", "$rootScope", '$modal', 'NgTableParams', '$filter', '$timeout'];

        constructor(uiService: Application.Services.Contracts.IUIService, dataService: Application.Services.Contracts.IDataService, $rootScope: ng.IRootScopeService, modalService: angular.ui.bootstrap.IModalService, ngTableParams: NgTableParams, $filter: ng.IFilterService, timeOutService: ng.ITimeoutService) {
            var vm = this;
            vm.dataService = dataService;
            vm.uiService = uiService;
            vm.rootScope = $rootScope;
            vm.rootScope.$emit('onActivated', 'admin');
            vm.modalService = modalService;
            vm.ngTableParams = ngTableParams;
            vm.filter = $filter;

            if (vm.rootScope['selectedFund']) {
                vm.selectedFund = vm.rootScope['selectedFund'];
            }
            
            vm.rootScope.$on('onFundSelectionChanged', (event, data: Models.ISummary) => {
                vm.selectedFund = data;
            });

            vm.rootScope.$on('onAutoRefresh', (event, data: Models.ISummary) => {
                vm.selectedFund = data;
            });

        }


        process = (url,status) => {
            var vm = this;
            vm.isLoading = true;
            vm.statusText = status + "...";
            vm.success = "";
            vm.error = "";
            vm.dataService.process(url).then(d => {
                vm.isLoading = false;
                vm.success = "Calculation - Finished"
            }).catch(function (e) {
                vm.isLoading = false;
                vm.error = "Error occurred, please contact IT."
                console.log('Error: ', e);
                throw e;
            });
        }



        

    }

    angular.module("app").controller("application.controllers.adminController", AdminController);
} 