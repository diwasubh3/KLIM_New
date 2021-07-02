var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var AnalystResearchController = (function () {
            function AnalystResearchController(dataService, $rootScope, modalService, ngTableParams, $filter, timeOutService) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                this.loadData = function () {
                    var vm = _this;
                    vm.statusText = "Loading";
                    vm.isLoading = true;
                    vm.dataService.getAnalystResearchHeaderFields().then(function (d) {
                        vm.analystResearchHeaderFields = d;
                        vm.loadAnalystResearches();
                    });
                };
                this.getStyle = function (column, data, isIssuer) {
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
                    }
                    else {
                        if (column.fieldTitle.indexOf("ANALYST") >= 0) {
                            style['width'] = 126.77;
                        }
                        else {
                            style['width'] = 106.77;
                        }
                    }
                    return style;
                };
                this.editAnalystResearch = function (analystResearch) {
                    if (!analystResearch.analystResearchId) {
                        analystResearch = {};
                    }
                    var vm = _this;
                    var modalInstance = vm.modalService.open({
                        templateUrl: pageOptions.appBasePath + 'app/views/addupdateanalystresearch.html?v=' + pageOptions.version,
                        controller: 'application.controllers.addUpdateAnalystResearchController',
                        controllerAs: 'ar',
                        size: 'x-lg',
                        resolve: {
                            sourcedata: function () {
                                var modelSourceData = analystResearch.issuerId;
                                return modelSourceData;
                            }
                        }
                    });
                    modalInstance.result.then(function (result) {
                        if (result) {
                            vm.loadAnalystResearches();
                        }
                    }, function () { });
                };
                this.loadAnalystResearches = function () {
                    var vm = _this;
                    vm.isLoading = true;
                    vm.dataService.getAnalystResearches(vm.selectedLoadType.Id, null).then(function (d) {
                        vm.data = d;
                        vm.isLoading = false;
                        vm.setParamsTable();
                    });
                };
                this.setParamsTable = function () {
                    var vm = _this;
                    var filterSearchText = function (data, filterValues) {
                        filterValues.searchText = filterValues.searchText.toLowerCase();
                        return data.filter(function (item) {
                            item.analystResearches.forEach(function (s) {
                                s['isVisible'] = !filterValues.searchText || s.searchText.indexOf(filterValues.searchText) >= 0;
                            });
                            return item.analystResearches.filter(function (s) { return s['isVisible']; }).length;
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
                            'issuer': 'asc'
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
                vm.rootScope.$emit('onActivated', 'analystresearch');
                vm.modalService = modalService;
                vm.ngTableParams = ngTableParams;
                vm.filter = $filter;
                vm.loadTypes = [{ Id: 0, Text: 'Current Research Only' }, { Id: 1, Text: 'All Research ' }];
                vm.selectedLoadType = this.loadTypes[0];
                vm.rootScope.$on('onAutoRefresh', function (event, data) {
                    vm.loadData();
                });
                this.loadData();
            }
            return AnalystResearchController;
        }());
        AnalystResearchController.$inject = ["application.services.dataService", "$rootScope", '$modal', 'NgTableParams', '$filter', '$timeout'];
        Controllers.AnalystResearchController = AnalystResearchController;
        angular.module("app").controller("application.controllers.analystResearchController", AnalystResearchController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=AnalystResearchController.js.map