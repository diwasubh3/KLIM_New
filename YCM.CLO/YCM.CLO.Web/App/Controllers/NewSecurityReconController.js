var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var NewSecurityReconController = (function () {
            function NewSecurityReconController(dataService, $rootScope, modalService, ngTableParams, $filter, timeOutService) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                this.canSelectWSO = true;
                this.canSelectYCM = true;
                this.loadData = function (headerFields) {
                    var vm = _this;
                    if (!vm.headerFields) {
                        vm.headerFields = headerFields;
                    }
                    vm.loadSecuritiesForRecon();
                    vm.statusText = "Loading";
                };
                this.loadSecuritiesForRecon = function () {
                    var vm = _this;
                    vm.isLoading = true;
                    vm.canSelectWSO = true;
                    vm.canSelectYCM = true;
                    vm.dataService.getSecuritiesForRecon().then(function (d) {
                        vm.data = d;
                        vm.isLoading = false;
                        vm.setParamsTable();
                    });
                };
                this.selectForMatch = function (security) {
                    var vm = _this;
                    vm['canSelect' + security.source] = !security.checked;
                };
                this.confirmSecurityRecon = function () {
                    var vm = _this;
                    var securities = vm.data.filter(function (s) { return s.checked; });
                    var modalInstance = vm.modalService.open({
                        templateUrl: pageOptions.appBasePath + 'app/views/confirmsecurityrecon.html?v=' + pageOptions.version,
                        controller: 'application.controllers.confirmSecurityReconController',
                        controllerAs: 'confirmsecurityrecon',
                        size: 'x-lg',
                        resolve: {
                            sourcedata: function () {
                                var modelSourceData = { securities: securities, fields: vm.headerFields };
                                return modelSourceData;
                            }
                        }
                    });
                    modalInstance.result.then(function (result) {
                        if (result) {
                            vm.loadSecuritiesForRecon();
                        }
                    }, function () { });
                };
                this.getStyle = function (column, data) {
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
                };
                this.setParamsTable = function () {
                    var vm = _this;
                    vm.tableParams = new vm.ngTableParams({
                        page: 1,
                        noPager: true,
                        count: 10000,
                        sorting: { issuer: "asc" },
                        filtering: {
                            searchText: ''
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
                vm.rootScope.$emit('onActivated', 'wso');
                vm.modalService = modalService;
                vm.ngTableParams = ngTableParams;
                vm.filter = $filter;
                vm.isLoading = true;
                vm.dataService.getSecurityReconHeaderFields().then(function (sf) {
                    vm.loadData(sf);
                });
                vm.rootScope.$on('onFundChanged', function (event, data) {
                    vm.loadSecuritiesForRecon();
                });
            }
            return NewSecurityReconController;
        }());
        NewSecurityReconController.$inject = ["application.services.dataService", "$rootScope", '$uibModal', 'NgTableParams', '$filter', '$timeout'];
        Controllers.NewSecurityReconController = NewSecurityReconController;
        angular.module("app").controller("application.controllers.newSecurityReconController", NewSecurityReconController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=NewSecurityReconController.js.map