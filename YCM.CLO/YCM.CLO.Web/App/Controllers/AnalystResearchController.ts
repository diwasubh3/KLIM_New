module Application.Controllers {
    export class AnalystResearchController {
        dataService: Application.Services.Contracts.IDataService;
        rootScope: ng.IRootScopeService;

        isLoading: boolean;
        analystResearchHeaderFields: Models.IAnalystResearchHeaderFileds;
        data: Array<Models.IGroupedAnalystRefresh>;
        selectedLoadType: any;
        loadTypes :any;
        appBasePath: string = pageOptions.appBasePath;
        ngTableParams: any;
        statusText: string;
        tableParams: any;
        modalService: angular.ui.bootstrap.IModalService;
        filter: ng.IFilterService;
        timeOutService: ng.ITimeoutService;
        static $inject = ["application.services.dataService", "$rootScope", '$modal', 'NgTableParams', '$filter', '$timeout'];


        constructor(dataService: Application.Services.Contracts.IDataService, $rootScope: ng.IRootScopeService, modalService: angular.ui.bootstrap.IModalService, ngTableParams: NgTableParams, $filter: ng.IFilterService, timeOutService: ng.ITimeoutService) {
            var vm = this;
            vm.dataService = dataService;
            vm.rootScope = $rootScope;
            vm.rootScope.$emit('onActivated', 'analystresearch');
            vm.modalService = modalService;
            vm.ngTableParams = ngTableParams;
            vm.filter = $filter;
            vm.loadTypes = [{ Id: 0, Text: 'Current Research Only' }, { Id: 1, Text: 'All Research ' }];
            vm.selectedLoadType = this.loadTypes[0];
            vm.rootScope.$on('onAutoRefresh', (event, data: Models.ISummary) => {
                vm.loadData();
            });
            this.loadData();
        }

        loadData = () => {
            var vm = this;
            vm.statusText = "Loading";
            vm.isLoading = true;
            vm.dataService.getAnalystResearchHeaderFields().then((d) => {
                vm.analystResearchHeaderFields = d;
                vm.loadAnalystResearches();
            });
        }

        getStyle = (column: Models.IField, data: Models.IGroupedAnalystRefresh, isIssuer:boolean) => {
            var style = {};
            if (isIssuer) {
                if (column.fieldName !== "Issuer") {
                    if (column.fieldName == 'CLOAnalyst' || column.fieldName == 'HFAnalyst' || column.fieldName == 'LastUpdatedOn') {
                        style['width'] = column.displayWidth + 40;
                    }
                }

                if (data) {
                    style['border-width'] = 0;
                }
            } else {
                if (column.fieldTitle.indexOf("ANALYST") >= 0) {
                    style['width'] = 126.77;
                } else {
                    style['width'] = 106.77;
                }
            }

            return style;
        }



        editAnalystResearch = (analystResearch: Models.IAnalystResearch) => {
            if (!analystResearch.analystResearchId) {
                analystResearch = <Models.IAnalystResearch>{};
            }

            var vm = this;
            var modalInstance = vm.modalService.open({
                templateUrl: pageOptions.appBasePath + 'app/views/addupdateanalystresearch.html?v=' + pageOptions.version,
                controller: 'application.controllers.addUpdateAnalystResearchController',
                controllerAs: 'ar',
                size: 'x-lg',
                resolve: {
                    sourcedata: () => {
                        var modelSourceData = analystResearch.issuerId;
                        return modelSourceData;
                    }
                }
            });

            modalInstance.result.then((result: Array<Models.IParameterValue>) => {
                if (result) {
                    vm.loadAnalystResearches();
                }
            }, () => { });
        }


        loadAnalystResearches = () => {
            var vm = this;
            vm.isLoading = true;
            vm.dataService.getAnalystResearches(vm.selectedLoadType.Id, null).then((d) => {
                vm.data = d;
                vm.isLoading = false;
                vm.setParamsTable();
            });
        }

        setParamsTable = () => {
            var vm = this;

            var filterSearchText = (data, filterValues) => {
                filterValues.searchText = filterValues.searchText.toLowerCase();
                
                return data.filter(item => {
                    item.analystResearches.forEach(s => {
                        s['isVisible'] = !filterValues.searchText || s.searchText.indexOf(filterValues.searchText) >= 0;
                    });
                    return item.analystResearches.filter(s => { return s['isVisible'] }).length;
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
                    'issuer': 'asc'
                }
            },{
                    total: 1,
                    counts: [],
                    dataset: vm.data,
                    filterOptions: { filterFn: filterSearchText }
             });
        }
    }

    angular.module("app").controller("application.controllers.analystResearchController", AnalystResearchController);
} 