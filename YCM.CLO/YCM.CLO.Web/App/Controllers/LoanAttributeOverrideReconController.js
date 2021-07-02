var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var LoanAttributeOverrideReconController = (function () {
            function LoanAttributeOverrideReconController(uiService, dataService, $rootScope, modalService, ngTableParams, $filter, timeOutService) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                this.loadData = function (headerFields) {
                    var vm = _this;
                    if (!vm.headerFields) {
                        vm.headerFields = headerFields;
                    }
                    vm.loadLoanAttributeOverrides(null);
                    vm.statusText = "Loading";
                };
                this.loadLoanAttributeOverrides = function (securityId) {
                    var vm = _this;
                    vm.isLoading = true;
                    vm.dataService.getLoanAttributeOverrides(securityId).then(function (d) {
                        if (!securityId) {
                            vm.data = d;
                        }
                        else {
                            var security = vm.data.filter(function (d) { return d.securityId === securityId; })[0];
                            if (d && d.length) {
                                security.loanAttributeOverrides = d[0].loanAttributeOverrides;
                            }
                            else {
                                vm.data.splice(vm.data.indexOf(security), 1);
                            }
                        }
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
                this.endSecurityOverride = function (loanAttributeOverride) {
                    var vm = _this;
                    vm.isLoading = true;
                    vm.dataService.endSecurityOverride(loanAttributeOverride).then(function (d) {
                        if (d.status) {
                            vm.loadLoanAttributeOverrides(loanAttributeOverride.securityId);
                            var message = {
                                header: "Please note:", body: "<p><b>You decided to accept the WSO Value.</b></p><br/><br/>" +
                                    "<p><b>The system has therefore gone ahead and ended the York override value <br/>effective: " + d.endDate + "</b></p>"
                            };
                            vm.uiService.showMessage(message);
                        }
                        else {
                            vm.isLoading = false;
                        }
                    });
                };
                this.resetConflict = function (loanAttributeOverride) {
                    var vm = _this;
                    vm.isLoading = true;
                    vm.dataService.resetSecurityOverrideConflict(loanAttributeOverride).then(function (d) {
                        if (d.status) {
                            vm.loadLoanAttributeOverrides(loanAttributeOverride.securityId);
                        }
                        else {
                            vm.isLoading = false;
                        }
                    });
                };
                this.capitalizeFirstLetter = function (match) {
                    return angular.uppercase(match.charAt(0)) + match.slice(1);
                };
                this.getStyle = function (column, data) {
                    var style = {};
                    style['width'] = column.displayWidth;
                    if (column.fieldTitle === 'ATTRIBUTE') {
                        style['width'] = 150;
                    }
                    return style;
                };
                this.setParamsTable = function () {
                    var vm = _this;
                    var filterSearchText = function (data, filterValues) {
                        filterValues.searchText = filterValues.searchText.toLowerCase();
                        return data.filter(function (item) {
                            item.loanAttributeOverrides.forEach(function (s) {
                                s['isVisible'] = !filterValues.searchText || s.searchText.indexOf(filterValues.searchText) >= 0;
                            });
                            return item.loanAttributeOverrides.filter(function (s) { return s['isVisible']; }).length;
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
                            'securityCode': 'asc'
                        }
                    }, {
                        total: 1,
                        counts: [],
                        dataset: vm.data,
                        filterOptions: { filterFn: filterSearchText }
                    });
                };
                var vm = this;
                vm.uiService = uiService;
                vm.dataService = dataService;
                vm.rootScope = $rootScope;
                vm.rootScope.$emit('onActivated', 'wso');
                vm.modalService = modalService;
                vm.ngTableParams = ngTableParams;
                vm.filter = $filter;
                vm.isLoading = true;
                vm.dataService.getLoanAttributeOverrideReconHeaderFields().then(function (sf) {
                    vm.loadData(sf);
                });
                vm.rootScope.$on('onAutoRefresh', function (event, data) {
                    vm.loadLoanAttributeOverrides(null);
                });
                vm.rootScope.$emit('refreshSummaries', null);
            }
            return LoanAttributeOverrideReconController;
        }());
        LoanAttributeOverrideReconController.$inject = ["application.services.uiService", "application.services.dataService", "$rootScope", '$modal', 'NgTableParams', '$filter', '$timeout'];
        Controllers.LoanAttributeOverrideReconController = LoanAttributeOverrideReconController;
        angular.module("app").controller("application.controllers.loanAttributeOverrideReconController", LoanAttributeOverrideReconController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=LoanAttributeOverrideReconController.js.map