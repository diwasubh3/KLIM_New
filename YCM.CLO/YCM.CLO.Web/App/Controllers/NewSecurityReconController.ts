module Application.Controllers {
    export class NewSecurityReconController {
        dataService: Application.Services.Contracts.IDataService;
        rootScope: ng.IRootScopeService;

        isLoading: boolean;
        data: Array<Application.Models.ISecurityRecon>;
        appBasePath: string = pageOptions.appBasePath;
        ngTableParams: any;
        statusText: string;
        tableParams: any;
        modalService: angular.ui.bootstrap.IModalService;
        filter: ng.IFilterService;
        timeOutService: ng.ITimeoutService;
        headerFields: Array<Models.IField>;
        canSelectWSO: boolean = true;
        canSelectYCM:boolean = true;
		static $inject = ["application.services.dataService", "$rootScope", '$uibModal', 'NgTableParams', '$filter', '$timeout'];

        constructor(dataService: Application.Services.Contracts.IDataService, $rootScope: ng.IRootScopeService, modalService: angular.ui.bootstrap.IModalService, ngTableParams: NgTableParams, $filter: ng.IFilterService, timeOutService: ng.ITimeoutService) {
            var vm = this;
            vm.dataService = dataService;
            vm.rootScope = $rootScope;
            vm.rootScope.$emit('onActivated', 'wso');
            vm.modalService = modalService;
            vm.ngTableParams = ngTableParams;
            vm.filter = $filter;
            vm.isLoading = true;
            vm.dataService.getSecurityReconHeaderFields().then((sf) => {
                vm.loadData(sf);
            });

            vm.rootScope.$on('onFundChanged', (event, data: Models.ISummary) => {
                vm.loadSecuritiesForRecon();
            });
        }

        loadData = (headerFields: Array<Models.IField>) => {
            var vm = this;
            if (!vm.headerFields) {
                vm.headerFields = headerFields;
            }
            vm.loadSecuritiesForRecon();
            vm.statusText = "Loading";
        }

        loadSecuritiesForRecon = () => {
            var vm = this;
            vm.isLoading = true;
            vm.canSelectWSO = true;
            vm.canSelectYCM = true;
            vm.dataService.getSecuritiesForRecon().then((d) => {
                vm.data = d;
                vm.isLoading = false;
                vm.setParamsTable();
            });
        }

        selectForMatch = (security: Models.ISecurityRecon) => {
            var vm = this;
            vm['canSelect' + security.source] = !security.checked;
            
        }

        confirmSecurityRecon = () => {
            var vm = this;
            var securities = vm.data.filter((s: Models.ISecurityRecon) => { return s.checked; });

            var modalInstance = vm.modalService.open({
                templateUrl: pageOptions.appBasePath + 'app/views/confirmsecurityrecon.html?v=' + pageOptions.version,
                controller: 'application.controllers.confirmSecurityReconController',
                controllerAs: 'confirmsecurityrecon',
                size: 'x-lg',
                resolve: {
                    sourcedata: () => {
                        var modelSourceData = {securities:securities,fields:vm.headerFields};
                        return modelSourceData;
                    }
                }
            });

            modalInstance.result.then((result: Array<Models.IParameterValue>) => {
                if (result) {
                    vm.loadSecuritiesForRecon();
                }
            }, () => { });
        }

        getStyle = (column: Models.IField, data: Models.ISecurityOverride) => {
            var style = {};
            style['width'] = column.displayWidth;
            if (column.jsonPropertyName == 'issuer') {
                style['width'] = 300;
            }
            if (column.jsonPropertyName == 'facility') {
                style['width'] = null;
            }
           
            if (data) {
                style['border-width'] = 0;
            }
            return style;
        }

        setParamsTable = () => {
            var vm = this;
            
            vm.tableParams = new vm.ngTableParams({
                page: 1,
                noPager: true,
                count: 10000,
                sorting: { issuer:"asc" }, 
                filtering: {
                    searchText: ''
                }
            }, {
                    total: 1,
                    counts: [],
                    dataset: vm.data
                });
        }
    }

    angular.module("app").controller("application.controllers.newSecurityReconController", NewSecurityReconController);
} 