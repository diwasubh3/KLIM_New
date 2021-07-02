var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var LoanComparisonController = (function () {
            function LoanComparisonController(dataService, $window, $scope, $modalInstance, ngTableParams, sourcedata) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                this.isLoading = false;
                this.statusText = "Loading";
                this.dateOptions = {
                    formatYear: 'yy',
                    startingDay: 1
                };
                this.sortFieldArraySortOrderAsc = function (arrayToSort) {
                    return arrayToSort.sort(function (a, b) {
                        return a.sortOrder - b.sortOrder;
                    });
                };
                this.sortCustomViewFieldArraySortOrderAsc = function (arrayToSort) {
                    return arrayToSort.sort(function (a, b) {
                        return a.sortOrder - b.sortOrder;
                    });
                };
                this.loadData = function () {
                    var vm = _this;
                    vm.isLoading = true;
                    vm.dataService.loadFixedFields().then(function (fixedfields) {
                        vm.dataService.loadPositionViewFieldGroups().then(function (d) {
                            vm.fieldGroups = d;
                            var selectAll = {
                                fieldGroupId: -1,
                                fieldGroupName: 'Select All',
                                fields: []
                            };
                            fixedfields = vm.sortFieldArraySortOrderAsc(fixedfields);
                            d.forEach(function (fg) {
                                var fields = vm.sortFieldArraySortOrderAsc(fg.fields);
                                if (fg.fieldGroupName == 'Security') {
                                    fields = [].concat(fixedfields).concat(fields);
                                }
                                fields.forEach(function (f) {
                                    selectAll.fields.push(f);
                                });
                            });
                            vm.fieldGroups.splice(0, 0, selectAll);
                            vm.selectedFieldGroup = vm.fieldGroups[0];
                            vm.customViews.forEach(function (v) {
                                if (v.customViewFields) {
                                    var cvfs = v.customViewFields.filter(function (x) { return x.fieldId != 132 && x.fieldId != 133; });
                                    cvfs.forEach(function (x) {
                                        var fields = selectAll.fields.filter(function (f) { return f.fieldId == x.fieldId; });
                                        if (fields.length) {
                                            x.field = fields[0];
                                        }
                                    });
                                    v.customViewFields = vm.sortCustomViewFieldArraySortOrderAsc(cvfs);
                                }
                            });
                            vm.dataService.getPositionsForSecurities(vm.securityIds, vm.fund.fundCode).then(function (positions) {
                                vm.positions = positions;
                                vm.isLoading = false;
                            });
                        });
                    });
                };
                this.removePosition = function (pos) {
                    var vm = _this;
                    var posIndex = vm.positions.indexOf(pos);
                    vm.positions.splice(posIndex, 1);
                };
                this.cancel = function () {
                    var vm = _this;
                    vm.statusText = "Closing";
                    vm.modalInstance.dismiss('cancel');
                };
                this.save = function () {
                    var vm = _this;
                    vm.modalInstance.close(true);
                };
                var vm = this;
                vm.modalInstance = $modalInstance;
                vm.dataService = dataService;
                vm.scope = $scope;
                vm.windowService = $window;
                vm.sourcedata = sourcedata;
                vm.ngTableParams = ngTableParams;
                vm.selectedViewId = sourcedata.selectedViewId;
                if (sourcedata.fund) {
                    vm.fund = sourcedata.fund;
                }
                if (sourcedata.customViews) {
                    vm.customViews = sourcedata.customViews;
                    var actuals = vm.customViews.filter(function (x) { return !x.viewName.startsWith("---"); });
                    if (actuals.length) {
                        var selected = actuals.filter(function (x) { return x.viewId == vm.selectedViewId; });
                        if (selected.length)
                            vm.selectedCustomView = selected[0];
                        else
                            vm.selectedCustomView = actuals[0];
                    }
                }
                if (sourcedata.positions) {
                    vm.securityIds = sourcedata.positions.map(function (p) { return p.securityId; });
                    vm.loadData();
                }
            }
            return LoanComparisonController;
        }());
        LoanComparisonController.$inject = ["application.services.dataService", "$window", "$scope", "$uibModalInstance", 'NgTableParams', 'sourcedata'];
        Controllers.LoanComparisonController = LoanComparisonController;
        angular.module("app").controller("application.controllers.loanComparisonController", LoanComparisonController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=LoanComparisonController.js.map