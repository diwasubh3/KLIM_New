var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var AddUpdateAnalystResearchController = (function () {
            function AddUpdateAnalystResearchController(dataService, $window, $scope, $modalInstance, ngTableParams, sourcedata) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                this.isLoading = false;
                this.statusText = "Loading";
                this.dateFormat = 'MM/dd/yyyy';
                this.dateOptions = {
                    formatYear: 'yy',
                    startingDay: 1
                };
                this.openDate = function ($event, so, field) {
                    $event.preventDefault();
                    so['is' + field.jsonPropertyName + 'opened'] = true;
                };
                this.loadAnalystResearches = function (addnewanalystresearch) {
                    var vm = _this;
                    vm.isLoading = true;
                    if (vm.selectedIssuer && vm.selectedIssuer.issuerId) {
                        vm.dataService.getAnalystResearches(null, vm.selectedIssuer.issuerId).then(function (analystresearches) {
                            if (analystresearches.length) {
                                vm.data = analystresearches[0].analystResearches;
                                vm.businessDescription = vm.data[0].businessDescription;
                            }
                            else {
                                vm.data = [];
                            }
                            if (addnewanalystresearch) {
                                vm.addNewAnalystResearch();
                            }
                            vm.setEditMode();
                            vm.isLoading = false;
                        });
                    }
                    else {
                        vm.isLoading = false;
                    }
                };
                this.setValuesFomPlaceHolders = function () {
                    var vm = _this;
                    if (vm.data && vm.data.length && !vm.data[0].analystResearchId) {
                        for (var i = 0; i < vm.fields.length; i++) {
                            var d = vm.data[0];
                            var field = vm.fields[i];
                            if ((typeof (d[field.jsonPropertyName + 'PlaceHolder']) != 'undefined' && d[field.jsonPropertyName + 'PlaceHolder'] != null && d[field.jsonPropertyName + 'PlaceHolder'].toString().length) &&
                                (typeof (d[field.jsonPropertyName]) == 'undefined' || !d[field.jsonPropertyName].toString().length)) {
                                d[field.jsonPropertyName] = d[field.jsonPropertyName + 'PlaceHolder'];
                            }
                        }
                    }
                    if (vm.data && vm.data.length) {
                        if (vm.data[0].cloAnalystUserId) {
                            vm.data[0].cloAnalyst = vm.analysts.cloAnalysts.filter(function (a) { return a.userId == vm.data[0].cloAnalystUserId; })[0].fullName;
                        }
                        else {
                            vm.data[0].cloAnalyst = null;
                        }
                        if (vm.data[0].hfAnalystUserId) {
                            vm.data[0].hfAnalyst = vm.analysts.hFnalysts.filter(function (a) { return a.userId == vm.data[0].hfAnalystUserId; })[0].fullName;
                        }
                        else {
                            vm.data[0].hfAnalyst = null;
                        }
                        if (vm.data[0].asOfDate) {
                            vm.data[0].asOfDate = moment(new Date(vm.data[0].asOfDate.toString())).format("MM/DD/YYYY");
                        }
                    }
                };
                this.setEditMode = function () {
                    var vm = _this;
                    if (vm.data && vm.data.length) {
                        vm.data[0].isEditMode = true;
                        for (var i = 1; i < vm.data.length; i++) {
                            vm.data[i].isEditMode = false;
                        }
                    }
                };
                this.getStyle = function (field) {
                    var style = {};
                    if (field.fieldName === 'AsOfDate' || field.fieldName === 'CLOAnalyst' || field.fieldName === 'HFAnalyst') {
                        style.width = 120;
                    }
                    else {
                        style.width = 87;
                    }
                    return style;
                };
                this.addNewAnalystResearch = function () {
                    var vm = _this;
                    if (vm.selectedIssuer && vm.selectedIssuer.issuerId) {
                        var newAnalystResearch = {
                            issuerId: vm.selectedIssuer.issuerId
                        };
                        if (vm.data.length && vm.data[0]) {
                            newAnalystResearch.businessDescription = vm.data[0].businessDescription;
                        }
                        vm.setValuesFomPlaceHolders();
                        if (vm.data.length) {
                            vm.fields.forEach(function (field) {
                                newAnalystResearch[field.jsonPropertyName + 'PlaceHolder'] = vm.data[0][field.jsonPropertyName];
                            });
                            if (newAnalystResearch['asOfDate' + 'PlaceHolder']) {
                                newAnalystResearch['asOfDate' + 'PlaceHolder'] = moment(new Date(newAnalystResearch['asOfDate' + 'PlaceHolder'].toString())).format("MM/DD/YYYY");
                            }
                            newAnalystResearch.cloAnalystUserId = vm.data[0].cloAnalystUserId;
                            newAnalystResearch.hfAnalystUserId = vm.data[0].hfAnalystUserId;
                            if (vm.data[0].asOfDate) {
                                newAnalystResearch.minAsOfDate = moment(new Date(vm.data[0].asOfDate.toString())).add('days', 1).format("MM/DD/YYYY");
                            }
                        }
                        vm.data.splice(0, 0, newAnalystResearch);
                        vm.setEditMode();
                    }
                };
                this.loadData = function () {
                    var vm = _this;
                    vm.isLoading = true;
                    vm.dataService.getAnalystResearchHeaderFields().then(function (fields) {
                        vm.fields = fields.analystResearchFields;
                        vm.dataService.getIssuers().then(function (issuers) {
                            vm.issuers = issuers;
                            if (vm.selectedIssuerId) {
                                vm.selectedIssuer = vm.issuers.filter(function (s) { return s.issuerId === vm.selectedIssuerId; })[0];
                            }
                            vm.dataService.getAnalysts().then(function (analysts) {
                                vm.analysts = analysts;
                                vm.loadAnalystResearches(false);
                            });
                        });
                    });
                };
                this.cancel = function () {
                    var vm = _this;
                    vm.statusText = "Closing";
                    vm.modalInstance.dismiss('cancel');
                };
                this.save = function () {
                    var vm = _this;
                    vm.isLoading = true;
                    vm.statusText = "Updating";
                    vm.setValuesFomPlaceHolders();
                    var analystResearches = JSON.parse(JSON.stringify(vm.data));
                    analystResearches.forEach(function (ar) {
                        ar.businessDescription = vm.businessDescription;
                    });
                    if (analystResearches.length) {
                        vm.dataService.saveAnalystResearches(analystResearches).then(function (result) {
                            vm.isLoading = false;
                            vm.modalInstance.close(result);
                        });
                    }
                    else {
                        vm.modalInstance.dismiss('cancel');
                    }
                };
                var vm = this;
                vm.modalInstance = $modalInstance;
                vm.dataService = dataService;
                vm.scope = $scope;
                vm.ngTableParams = ngTableParams;
                vm.selectedIssuerId = sourcedata;
                vm.scope.$watch(function () {
                    return vm.selectedIssuer;
                }, function (newValue, oldValue) {
                    if (newValue != oldValue && newValue.issuerId && !vm.isLoading && !vm.selectedIssuerId) {
                        vm.businessDescription = null;
                        vm.loadAnalystResearches(true);
                    }
                });
                vm.loadData();
            }
            return AddUpdateAnalystResearchController;
        }());
        AddUpdateAnalystResearchController.$inject = ["application.services.dataService", "$window", "$scope", "$uibModalInstance", 'NgTableParams', 'sourcedata'];
        Controllers.AddUpdateAnalystResearchController = AddUpdateAnalystResearchController;
        angular.module("app").controller("application.controllers.addUpdateAnalystResearchController", AddUpdateAnalystResearchController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=AddUpdateAnalystResearchController.js.map