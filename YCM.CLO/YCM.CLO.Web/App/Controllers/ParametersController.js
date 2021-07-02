var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var ParametersController = (function () {
            function ParametersController(dataService, $rootScope, modalService, ngTableParams, $filter, timeOutService) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                this.loadData = function () {
                    var vm = _this;
                    vm.statusText = "Loading";
                    vm.isLoading = true;
                    vm.dataService.loadParameterTypes().then(function (pts) {
                        vm.parameterTypes = pts;
                        vm.loadParamValues();
                    });
                };
                this.loadParamValues = function () {
                    var vm = _this;
                    vm.dataService.loadParameterValues().then(function (d) {
                        vm.data = d;
                        vm.isLoading = false;
                        vm.setParamsTable();
                    });
                };
                this.sortTableParams = function (columnName) {
                    var vm = _this;
                    var sortObj = {};
                    sortObj[columnName] = vm.tableParams.isSortBy(columnName, 'asc') ? 'desc' : 'asc';
                    vm.tableParams.sorting(sortObj);
                };
                this.edit = function (parameterValue) {
                    if (!parameterValue) {
                        parameterValue = {};
                    }
                    var vm = _this;
                    var modalInstance = vm.modalService.open({
                        templateUrl: pageOptions.appBasePath + 'app/views/addupdateparametervalue.html?v=' + pageOptions.version,
                        controller: 'application.controllers.addUpdateParameterValueController',
                        controllerAs: 'pv',
                        size: 'md',
                        resolve: {
                            sourcedata: function () {
                                var modelSourceData = JSON.parse(JSON.stringify(parameterValue));
                                return modelSourceData;
                            }
                        }
                    });
                    modalInstance.result.then(function (parameterValue) {
                        vm.loadParamValues();
                    }, function () { });
                };
                this.setParamsTable = function () {
                    var vm = _this;
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
                        dataset: vm.data
                    });
                };
                var vm = this;
                vm.dataService = dataService;
                vm.rootScope = $rootScope;
                vm.rootScope.$emit('onActivated', 'maintenance');
                vm.modalService = modalService;
                vm.ngTableParams = ngTableParams;
                vm.filter = $filter;
                vm.loadData();
                vm.newParameter = {};
                vm.rootScope.$on('onAutoRefresh', function (event, data) {
                    vm.loadData();
                });
            }
            return ParametersController;
        }());
        ParametersController.$inject = ["application.services.dataService", "$rootScope", '$uibModal', 'NgTableParams', '$filter', '$timeout'];
        Controllers.ParametersController = ParametersController;
        angular.module("app").controller("application.controllers.parametersController", ParametersController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=ParametersController.js.map