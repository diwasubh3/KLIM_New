module Application.Controllers {
    export class AddNewSecurityController {
        dataService: Application.Services.Contracts.IDataService;
        rootScope: ng.IRootScopeService;

        isLoading: boolean;
        data: Array<Application.Models.ISecurityOverride>;
        appBasePath: string = pageOptions.appBasePath;
        ngTableParams: any;
        statusText: string;
        tableParams: any;
        modalService: angular.ui.bootstrap.IModalService;
        filter: ng.IFilterService;
        timeOutService: ng.ITimeoutService;
        securityOverrideTypes: any;
        headerFields: Array<Models.IField>;
        selectedSecurityOverrideType: any;
        static $inject = ["application.services.dataService", "$rootScope", '$modal', 'NgTableParams', '$filter', '$timeout'];

        constructor(dataService: Application.Services.Contracts.IDataService, $rootScope: ng.IRootScopeService, modalService: angular.ui.bootstrap.IModalService, ngTableParams: NgTableParams, $filter: ng.IFilterService, timeOutService: ng.ITimeoutService) {
            var vm = this;
            vm.dataService = dataService;
            vm.rootScope = $rootScope;
            vm.rootScope.$emit('onActivated', 'maintenance');
            vm.modalService = modalService;
            vm.ngTableParams = ngTableParams;
            vm.filter = $filter;
            vm.securityOverrideTypes = [{ Id: 0, Text: 'Active Overrides' }, { Id: 1, Text: 'Historical Overrides' }, { Id: 2, Text: 'All Overrides' }];
            vm.selectedSecurityOverrideType = this.securityOverrideTypes[0];
            vm.isLoading = true;
            vm.dataService.getSecurityOverrideHeaderFields().then((sf) => {
                vm.loadData(sf);
            });
        }

        loadData = (headerFields: Array<Models.IField>) => {
            var vm = this;
            if (!vm.headerFields) {
                vm.headerFields = headerFields;
            }
            vm.loadSecurityOverrides();
            vm.statusText = "Loading";
        }

        loadSecurityOverrides = () => {
            var vm = this;
            vm.isLoading = true;
            vm.dataService.getGroupedSecurityOverrides(vm.selectedSecurityOverrideType.Id, null).then((d) => {
                vm.data = d;
                vm.isLoading = false;
                vm.setParamsTable();
            });
        }

        editSecurityOverride = (securityOverride: Models.ISecurityOverride) => {
            if (!securityOverride) {
                securityOverride = <Models.ISecurityOverride>{};
            }

            var vm = this;
            var modalInstance = vm.modalService.open({
                templateUrl: pageOptions.appBasePath + 'app/views/addupdatesecurityoverride.html?v=' + pageOptions.version,
                controller: 'application.controllers.addUpdateSecurityOverrideController',
                controllerAs: 'so',
                size: 'x-lg',
                resolve: {
                    sourcedata: () => {
                        var modelSourceData = securityOverride.securityId;
                        return modelSourceData;
                    }
                }
            });

            modalInstance.result.then((result: Array<Models.IParameterValue>) => {
                if (result) {
                    vm.loadData(vm.headerFields);
                }
            }, () => { });
        }

        getStyle = (column: Models.IField, data: Models.ISecurityOverride) => {
            var style = {};
            style['width'] = column.displayWidth;
            if (data) {
                style['border-width'] = 0;
            }
            if (data && data.isHistorical) {
                style['color'] = 'lightgray';
            }
            return style;
        }

        setParamsTable = () => {
            var vm = this;

            var filterSearchText = (data, filterValues) => {
                filterValues.searchText = filterValues.searchText.toLowerCase();
                return data.filter(item => {
                    item.securities.forEach(s => {
                        s['isVisible'] = !filterValues.searchText || s.searchText.indexOf(filterValues.searchText) >= 0;
                    });
                    return item.securities.filter(s => { return s['isVisible'] }).length;
                });
            }
            vm.tableParams = new vm.ngTableParams({
                page: 1,
                noPager: true,
                count: 10000,
                filtering: {
                    searchText: ''
                },
                sorting: {
                    'securityCode': 'asc',
                    'issuerDesc': 'asc',
                    'facilityDesc': 'asc'
                }
            }, {
                    total: 1,
                    counts: [],
                    dataset: vm.data,
                    filterOptions: { filterFn: filterSearchText }
                });



        }



    }

    angular.module("app").controller("application.controllers.addNewSecurityController", AddNewSecurityController);
} 