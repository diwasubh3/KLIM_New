var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var ReportingController = (function () {
            function ReportingController(dataService, $rootScope, modalService, ngTableParams, $filter, timeOutService) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                this.statusText = 'Loading';
                this.reportDisplay = 'block';
                this.capitalStructureDisplay = 'none';
                this.cloStatsDisplay = 'none';
                this.env = pageOptions.env;
                this.defaultedLoansDisplay = 'none';
                this.equityDisplay = 'none';
                this.canEdit = pageOptions.canEditReportData;
                this.dateOptions = {
                    formatYear: 'yy',
                    startingDay: 1
                };
                this.loadData = function () {
                    var vm = _this;
                    vm.statusText = "Loading";
                    vm.isLoading = true;
                    vm.dataService.getReportingData().then(function (d) {
                        vm.data = d;
                        vm.data.fundAssetClasses.forEach(function (fa) {
                            fa.startDate = new Date(fa.startDate.toString());
                            fa.endDate = new Date(fa.endDate.toString());
                            if (!vm.selectedFund) {
                                vm.onSummaryChanged(vm.rootScope['selectedFund']);
                            }
                        });
                        vm.isLoading = false;
                    });
                };
                this.onSummaryChanged = function (data) {
                    var vm = _this;
                    if (data) {
                        vm.selectedSummary = data;
                        vm.selectedFund = vm.data.funds.filter(function (f) { return f.fundId === vm.selectedSummary.fundId; })[0];
                    }
                    else {
                        vm.selectedFund = vm.data.funds[0];
                    }
                };
                this.addNewSecurityOverride = function () {
                    var vm = _this;
                    vm.data.equityOverrides.push({ fundId: vm.selectedFund.fundId, isDeleted: false });
                };
                this.canSave = function () {
                    var vm = _this;
                    var hasInvalidData = false;
                    for (var i = 0; i < vm.data.equityOverrides.length; i++) {
                        if (!vm.data.equityOverrides[i].securityCode) {
                            hasInvalidData = true;
                            break;
                        }
                    }
                    return !hasInvalidData;
                };
                this.getFilteredAssetClasses = function () {
                    var vm = _this;
                    return vm.data.fundAssetClasses.filter(function (f) { return f.fundId == vm.selectedFund.fundId; });
                };
                this.save = function () {
                    var vm = _this;
                    vm.isLoading = true;
                    vm.dataService.saveReportingData(vm.data).then(function (data) {
                        vm.data = data;
                        vm.data.fundAssetClasses.forEach(function (fa) {
                            fa.startDate = new Date(fa.startDate.toString());
                            fa.endDate = new Date(fa.endDate.toString());
                        });
                        vm.selectedFund = vm.data.funds.filter(function (f) { return f.fundId === vm.selectedFund.fundId; })[0];
                        vm.isLoading = false;
                    });
                };
                this.deleteSecurityOverride = function (eo) {
                    var vm = _this;
                    if (eo.id) {
                        eo.isDeleted = true;
                    }
                    else {
                        vm.data.equityOverrides.splice(vm.data.equityOverrides.indexOf(eo), 1);
                    }
                };
                this.openDate = function ($event, src, prop) {
                    var vm = _this;
                    $event.preventDefault();
                    src[prop] = true;
                };
                this.openTab = function (tabName) {
                    var vm = _this;
                    vm.reportDisplay = 'none';
                    vm.capitalStructureDisplay = 'none';
                    vm.cloStatsDisplay = 'none';
                    vm.defaultedLoansDisplay = 'none';
                    vm.equityDisplay = 'none';
                    switch (tabName) {
                        case 'Report':
                            vm.reportDisplay = 'block';
                            break;
                        case 'Capital Structure':
                            vm.capitalStructureDisplay = 'block';
                            break;
                        case 'CLO Stats':
                            vm.cloStatsDisplay = 'block';
                            break;
                        case 'Defaulted Loans':
                            vm.defaultedLoansDisplay = 'block';
                            break;
                        case 'Equity':
                            vm.equityDisplay = 'block';
                            break;
                        default:
                    }
                };
                var vm = this;
                vm.dataService = dataService;
                vm.rootScope = $rootScope;
                vm.rootScope.$emit('onActivated', 'reporting');
                vm.modalService = modalService;
                vm.ngTableParams = ngTableParams;
                vm.filter = $filter;
                vm.rootScope.$on('onFundChanged', function (event, data) {
                    if (vm.data.funds) {
                        vm.onSummaryChanged(data);
                    }
                });
                vm.rootScope.$on('onFundSelectionChanged', function (event, data) {
                    vm.onSummaryChanged(data);
                });
                vm.loadData();
            }
            return ReportingController;
        }());
        ReportingController.$inject = ["application.services.dataService", "$rootScope", '$uibModal', 'NgTableParams', '$filter', '$timeout'];
        Controllers.ReportingController = ReportingController;
        angular.module("app").controller("application.controllers.reportingController", ReportingController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=ReportingController.js.map