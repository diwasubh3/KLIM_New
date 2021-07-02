var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var FilterPositionsController = (function () {
            function FilterPositionsController(uiService, dataService, $window, $scope, $modalInstance, ngTableParams, positionData) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                this.isLoading = false;
                this.statusText = "Loading";
                this.alwaysCalculateFields = ['Bid', 'Offer', 'Spread', 'MoodyRecovery', 'SeniorLeverage', 'WARF', 'WARF Recovery', 'Spread', 'WARF'];
                this.dateOptions = {
                    formatYear: 'yy',
                    startingDay: 1
                };
                this.loadData = function () {
                    var vm = _this;
                    vm.isLoading = true;
                    if (!vm.positionfilter.operators) {
                        vm.dataService.loadOperators().then(function (operators) {
                            vm.positionfilter.operators = operators;
                            vm.positionfilter.operators.splice(0, 0, { operatorCode: '', operatorId: null, operatorVal: null });
                            vm.operators = vm.positionfilter.operators;
                            if (!vm.positionfilter.ratingsDictionary) {
                                vm.dataService.getRatings().then(function (ratings) {
                                    vm.positionfilter.ratingsDictionary = {};
                                    for (var i = 0; i < ratings.length; i++) {
                                        vm.positionfilter.ratingsDictionary[ratings[i].ratingDesc] = ratings[i].rank;
                                    }
                                    vm.loadAllFieldGroups();
                                });
                            }
                        });
                    }
                    else {
                        vm.operators = vm.positionfilter.operators;
                        vm.loadAllFieldGroups();
                    }
                };
                this.loadAllFieldGroups = function () {
                    var vm = _this;
                    if (!vm.positionfilter.security) {
                        vm.dataService.getFilterFieldGroups().then(function (fieldgroups) {
                            vm.positionfilter.fixedfields = [];
                            vm.positionfilter.security = [];
                            vm.positionfilter.analyst = [];
                            vm.positionfilter.ratings = [];
                            vm.loadFieldGroup(fieldgroups, 5, "fixedfields");
                            vm.loadFieldGroup(fieldgroups, 1, "security");
                            vm.loadFieldGroup(fieldgroups, 2, "ratings");
                            vm.loadFieldGroup(fieldgroups, 3, "analyst");
                            vm.isLoading = false;
                        });
                    }
                    else {
                        vm.isLoading = false;
                        vm.setFields("fixedfields");
                        vm.setFields("security");
                        vm.setFields("ratings");
                        vm.setFields("analyst");
                    }
                };
                this.loadFieldGroup = function (fieldgroups, fieldGroupId, filterField) {
                    var vm = _this;
                    var fields = fieldgroups.filter(function (fg) { return fg.fieldGroupId == fieldGroupId; })[0].fields;
                    if (fields && fields.length) {
                        fields = vm.uiService.sortArrayBySortOrderAsc(fields, "filterOrder");
                        fields.forEach(function (field) {
                            vm.positionfilter[filterField].push({
                                field: field,
                                lowerBound: { operator: vm.operators[0] },
                                upperBound: { operator: vm.operators[0] }
                            });
                        });
                    }
                    vm.setFields(filterField);
                };
                this.setFields = function (filterField) {
                    var vm = _this;
                    for (var i = 0; i < vm.positionfilter[filterField].length; i++) {
                        vm.positionfilter[filterField][i].isDisabled = vm.uiFields.filter(function (f) { return f.fieldName == vm.positionfilter[filterField][i].field.fieldName; }).length == 0;
                        if (!vm.positionfilter[filterField][i].isDisabled && vm.positionfilter[filterField][i].field.fieldType == 1) {
                            if (filterField == 'ratings') {
                                var collection = vm.filterCollections[vm.positionfilter[filterField][i].field.jsonPropertyName + 's'];
                                for (var j = 0; j < collection.length; j++) {
                                    var val = vm.positionfilter.ratingsDictionary[collection[j].label];
                                    if (val == undefined) {
                                        val = -2;
                                    }
                                    collection[j].value = val;
                                }
                                vm.positionfilter[filterField][i].sourceCollection = JSON.parse(JSON.stringify(collection));
                            }
                            else {
                                vm.positionfilter[filterField][i].sourceCollection = vm.filterCollections[vm.positionfilter[filterField][i].field.jsonPropertyName + 's'];
                            }
                        }
                    }
                };
                this.openDate = function ($event, filter) {
                    $event.preventDefault();
                    if (!filter.date) {
                        filter.date = { opened: false };
                    }
                    filter.date.opened = true;
                };
                this.calculateAverages = function () {
                    var vm = _this;
                    vm.uiService.updateFilterStatistics(vm.positions, vm.positionfilter, "issuer");
                    var filtered = vm.positions.filter(function (x) { return x.isFilterSuccess; });
                    vm.updateAverages("fixedfields", filtered, vm.positionfilter);
                    vm.updateAverages("ratings", filtered, vm.positionfilter);
                    vm.updateAverages("security", filtered, vm.positionfilter);
                    vm.updateAverages("analyst", filtered, vm.positionfilter);
                };
                this.updateAverages = function (field, positions, positionFilter) {
                    var vm = _this;
                    positionFilter[field].forEach(function (y) {
                        var sum = 0;
                        if (y.field.fieldType != 3 && y.field.fieldType != 1 && ((y.lowerBound && y.lowerBound.value != null && y.lowerBound.value.toString().length > 0) || (y.upperBound && y.upperBound.value != null && y.upperBound.value.toString().length > 0) || vm.alwaysCalculateFields.indexOf(y.field.fieldName) >= 0)) {
                            positions.forEach(function (x) {
                                sum += parseFloat(x[y.field.jsonPropertyName] != null && x[y.field.jsonPropertyName].toString().length > 0 ? x[y.field.jsonPropertyName].toString().replace(/,/g, '') : '0');
                            });
                            y.average = sum / positionFilter.loanCount;
                        }
                    });
                };
                this.cancel = function () {
                    var vm = _this;
                    vm.statusText = "Closing";
                    vm.reset();
                    vm.modalInstance.close(false);
                };
                this.filter = function () {
                    var vm = _this;
                    vm.modalInstance.close(true);
                };
                this.resetFieldGroup = function (positionFilter, filterField) {
                    var vm = _this;
                    positionFilter[filterField].forEach(function (filt) {
                        filt.lowerBound.value = null;
                        filt.upperBound.value = null;
                        filt.lowerBound.operator = vm.operators[0];
                        filt.upperBound.operator = vm.operators[0];
                        filt.average = null;
                    });
                };
                this.reset = function () {
                    var vm = _this;
                    vm.positionfilter.funds.forEach(function (f) { return f.isNotSelected = false; });
                    vm.resetFieldGroup(vm.positionfilter, "fixedfields");
                    vm.resetFieldGroup(vm.positionfilter, "security");
                    vm.resetFieldGroup(vm.positionfilter, "ratings");
                    vm.resetFieldGroup(vm.positionfilter, "analyst");
                };
                var vm = this;
                vm.modalInstance = $modalInstance;
                vm.dataService = dataService;
                vm.scope = $scope;
                vm.windowService = $window;
                vm.positions = positionData.positions;
                vm.positionfilter = positionData.positionfilter;
                vm.filterCollections = positionData.filterCollections;
                vm.uiFields = positionData.uiFields;
                vm.ngTableParams = ngTableParams;
                vm.uiService = uiService;
                vm.loadData();
            }
            return FilterPositionsController;
        }());
        FilterPositionsController.$inject = ["application.services.uiService", "application.services.dataService", "$window", "$scope", "$uibModalInstance", 'NgTableParams', 'positionData'];
        Controllers.FilterPositionsController = FilterPositionsController;
        angular.module("app").controller("application.controllers.filterPositionsController", FilterPositionsController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=FilterPositionsController.js.map