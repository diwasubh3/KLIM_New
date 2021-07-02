module Application.Controllers {
    export class ParametersController {
        dataService: Application.Services.Contracts.IDataService;
        rootScope: ng.IRootScopeService;

        isLoading: boolean;
        data: Array<Application.Models.IParameterValue>;
        parameterTypes: Array<Models.IParameterType>;
        newParameter:Models.IParameterValue;
        appBasePath: string = pageOptions.appBasePath;
        ngTableParams: any;
        statusText: string;
        tableParams: any;
        modalService: angular.ui.bootstrap.IModalService;
        filter: ng.IFilterService;
        timeOutService: ng.ITimeoutService;
		static $inject = ["application.services.dataService", "$rootScope", '$uibModal', 'NgTableParams', '$filter', '$timeout'];


        constructor(dataService: Application.Services.Contracts.IDataService, $rootScope: ng.IRootScopeService, modalService: angular.ui.bootstrap.IModalService, ngTableParams: NgTableParams, $filter: ng.IFilterService, timeOutService: ng.ITimeoutService) {
            var vm = this;
            vm.dataService = dataService;
            vm.rootScope = $rootScope;
            vm.rootScope.$emit('onActivated', 'maintenance');
            vm.modalService = modalService;
            vm.ngTableParams = ngTableParams;
            vm.filter = $filter;
            vm.loadData();
            vm.newParameter = <Models.IParameterValue>{};
            vm.rootScope.$on('onAutoRefresh', (event, data: Models.ISummary) => {
                vm.loadData();
            });
        }

        loadData = () => {
            var vm = this;
            vm.statusText = "Loading";
            vm.isLoading = true;
            vm.dataService.loadParameterTypes().then(pts => {
                vm.parameterTypes = pts;
                vm.loadParamValues();
            });
        }

        loadParamValues = () => {
            var vm = this;
            vm.dataService.loadParameterValues().then((d) => {
                vm.data = d;
                vm.isLoading = false;
                vm.setParamsTable();
            });
        }

        sortTableParams = (columnName: string) => {
            var vm = this;
            var sortObj = {};
            sortObj[columnName] = vm.tableParams.isSortBy(columnName, 'asc') ? 'desc' : 'asc';
            vm.tableParams.sorting(sortObj);
        }

        edit = (parameterValue: Models.IParameterValue) => {

            if (!parameterValue) {
                parameterValue = <Models.IParameterValue> {};
            }

            var vm = this;
            var modalInstance = vm.modalService.open({
                templateUrl: pageOptions.appBasePath + 'app/views/addupdateparametervalue.html?v=' + pageOptions.version,
                controller: 'application.controllers.addUpdateParameterValueController',
                controllerAs:'pv',
                size: 'md',
                resolve: {
                    sourcedata: () => {
                        var modelSourceData = JSON.parse(JSON.stringify(parameterValue));
                        return modelSourceData;
                    }
                }
            });

            modalInstance.result.then((parameterValue: Array<Models.IParameterValue>) => {
                vm.loadParamValues();
            }, () => { });
        }

        setParamsTable = () => {
            var vm = this;

                vm.tableParams = new vm.ngTableParams({
                    page: 1,
                    noPager: true,
                    count: 10000,
                    filtering: {
                        searchText: ''
                    },
                    sorting: {
                        'parameterType.parameterTypeName': 'asc'
                    }
                }, {
                        total: 1,
                        counts: [],
                        dataset:vm.data
                    });
           
        }

    }

    angular.module("app").controller("application.controllers.parametersController", ParametersController);
} 