var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var SecurityOverridesController = (function () {
            function SecurityOverridesController(dataService, $rootScope, modalService, ngTableParams, $filter, timeOutService) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                this.loadData = function (headerFields) {
                    var vm = _this;
                    if (!vm.headerFields) {
                        vm.headerFields = headerFields;
                    }
                    vm.loadSecurityOverrides();
                    vm.statusText = "Loading";
                };
                this.loadSecurityOverrides = function () {
                    var vm = _this;
                    vm.isLoading = true;
                    vm.dataService.getGroupedSecurityOverrides(vm.selectedSecurityOverrideType.Id, null).then(function (d) {
                        vm.data = d;
                        vm.isLoading = false;
                        vm.setParamsTable();
                    });
                };
                this.editSecurityOverride = function (securityOverride) {
                    if (!securityOverride) {
                        securityOverride = {};
                    }
                    var vm = _this;
                    var modalInstance = vm.modalService.open({
                        templateUrl: pageOptions.appBasePath + 'app/views/addupdatesecurityoverride.html?v=' + pageOptions.version,
                        controller: 'application.controllers.addUpdateSecurityOverrideController',
                        controllerAs: 'so',
                        size: 'x-lg',
                        resolve: {
                            sourcedata: function () {
                                var modelSourceData = securityOverride.securityId;
                                return modelSourceData;
                            }
                        }
                    });
                    modalInstance.result.then(function (result) {
                        if (result) {
                            vm.loadData(vm.headerFields);
                        }
                    }, function () { });
                };
                this.getStyle = function (column, data) {
                    var style = {};
                    style['width'] = column.displayWidth;
                    if (data) {
                        style['border-width'] = 0;
                    }
                    if (data && data.isHistorical) {
                        style['color'] = 'lightgray';
                    }
                    return style;
                };
                this.setParamsTable = function () {
                    var vm = _this;
                    var filterSearchText = function (data, filterValues) {
                        filterValues.searchText = filterValues.searchText.toLowerCase();
                        return data.filter(function (item) {
                            item.securities.forEach(function (s) {
                                s['isVisible'] = !filterValues.searchText || s.searchText.indexOf(filterValues.searchText) >= 0;
                            });
                            return item.securities.filter(function (s) { return s['isVisible']; }).length;
                        });
                    };
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
                };
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
                vm.dataService.getSecurityOverrideHeaderFields().then(function (sf) {
                    vm.loadData(sf);
                });
                vm.rootScope.$on('onAutoRefresh', function (event, data) {
                    vm.loadSecurityOverrides();
                });
            }
            return SecurityOverridesController;
        }());
        SecurityOverridesController.$inject = ["application.services.dataService", "$rootScope", '$uibModal', 'NgTableParams', '$filter', '$timeout'];
        Controllers.SecurityOverridesController = SecurityOverridesController;
        angular.module("app").controller("application.controllers.securityOverridesController", SecurityOverridesController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=SecurityOverridesController.js.map