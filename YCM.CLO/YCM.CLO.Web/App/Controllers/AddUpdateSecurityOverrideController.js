var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var AddUpdateSecurityOverrideController = (function () {
            function AddUpdateSecurityOverrideController(dataService, $window, $scope, $modalInstance, ngTableParams, sourcedata) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                this.isLoading = false;
                this.statusText = "Loading";
                this.dateFormat = 'MM/dd/yyyy';
                this.dateOptions = {
                    formatYear: 'yy',
                    startingDay: 1
                };
                this.confirmDelete = false;
                this.openDate = function ($event, so, field) {
                    $event.preventDefault();
                    so['iseffectiveFromopened'] = false;
                    so['iseffectiveToopened'] = false;
                    so['is' + field.jsonPropertyName + 'opened'] = true;
                };
                this.setSelectedField = function (securityOverride) {
                    var vm = _this;
                    securityOverride.isDirty = true;
                    securityOverride.field = vm.overrideFields.filter(function (f) { return f.fieldId === securityOverride.fieldId; })[0];
                    securityOverride.existingValue = vm.selectedSecurity['orig' + vm.capitalizeFirstLetter(securityOverride.field.jsonPropertyName)].toString();
                };
                this.loadSecurityOverrides = function (addnewsecurity) {
                    var vm = _this;
                    vm.isLoading = true;
                    vm.confirmDelete = false;
                    if (vm.selectedSecurity && vm.selectedSecurity.securityId) {
                        vm.dataService.getSecurityOverrides(null, vm.selectedSecurity.securityId).then(function (securityoverrides) {
                            vm.data = securityoverrides;
                            if (addnewsecurity) {
                                vm.addNewSecurityOverride();
                            }
                            vm.isLoading = false;
                        });
                    }
                    else {
                        vm.isLoading = false;
                    }
                };
                this.addNewSecurityOverride = function () {
                    var vm = _this;
                    vm.confirmDelete = false;
                    if (vm.selectedSecurity && vm.selectedSecurity.securityId) {
                        var newSecurityOverride = {
                            securityId: vm.selectedSecurity.securityId
                        };
                        vm.data.splice(0, 0, newSecurityOverride);
                    }
                    vm.validateIsRequired();
                };
                this.capitalizeFirstLetter = function (match) {
                    if (match) {
                        return angular.uppercase(match.charAt(0)) + match.slice(1);
                    }
                    else {
                        return match;
                    }
                };
                this.loadData = function () {
                    var vm = _this;
                    vm.isLoading = true;
                    vm.confirmDelete = false;
                    vm.dataService.getSaveSecurityOverrideHeaderFields().then(function (fields) {
                        vm.fields = fields;
                        vm.dataService.getSecurityOverrideableFields().then(function (overridefields) {
                            vm.overrideFields = overridefields;
                            vm.dataService.getCurrentSecurities().then(function (securities) {
                                vm.securities = securities;
                                if (vm.selectedSecurityId) {
                                    vm.selectedSecurity = vm.securities.filter(function (s) { return s.securityId == vm.selectedSecurityId; })[0];
                                }
                                vm.loadSecurityOverrides(false);
                            });
                        });
                    });
                };
                this.getStyle = function (column) {
                    var style = {};
                    if (column.displayWidth) {
                        style.width = column.displayWidth;
                    }
                    if (column.fieldTitle === 'ATTRIBUTE') {
                        style.width = null;
                    }
                    return style;
                };
                this.cancel = function () {
                    var vm = _this;
                    vm.statusText = "Closing";
                    vm.modalInstance.dismiss('cancel');
                };
                this.validateIsRequired = function () {
                    var vm = _this;
                    for (var j = 0; j < vm.data.length; j++) {
                        if (vm.data[j].field && vm.data[j].field.fieldTitle) {
                            vm.data[j]['fieldTitle'] = vm.data[j].field.fieldTitle;
                        }
                        else {
                            vm.data[j]['fieldTitle'] = '';
                        }
                    }
                    var groupByField = _.chain(vm.data).groupBy('fieldTitle').map(function (v, i) {
                        return {
                            field: i,
                            items: v
                        };
                    }).value();
                    groupByField.forEach(function (g) {
                        if (g.items.length) {
                            g.items[0]['iseffectiveTorequired'] = false;
                            g.items[0]['iseffectiveToinvalid'] = false;
                            for (var i = 1; i < g.items.length; i++) {
                                g.items[i]['iseffectiveTorequired'] = true;
                                if (g.items[i - 1]['effectiveFrom'] && g.items[i]['effectiveTo']) {
                                    var fromDate = new Date(g.items[i - 1]['effectiveFrom'].toString());
                                    var toDate = new Date(g.items[i]['effectiveTo'].toString());
                                    if (toDate >= fromDate) {
                                        g.items[i]['iseffectiveToinvalid'] = true;
                                    }
                                    else {
                                        g.items[i]['iseffectiveToinvalid'] = false;
                                    }
                                }
                            }
                        }
                    });
                };
                this.save = function (confirmedDelete) {
                    var vm = _this;
                    vm.confirmDelete = false;
                    if (!confirmedDelete && vm.data.length && vm.data.filter(function (d) { return d.isDeleted; }).length === vm.data.length) {
                        vm.warning = 'You have deleted all of the attribute overrides for this loan. Are you sure you want to proceed ?';
                        vm.confirmDelete = true;
                    }
                    else {
                        vm.isLoading = true;
                        vm.statusText = "Updating";
                        var dirtySecurityOverrides = JSON.parse(JSON.stringify(vm.data));
                        if (dirtySecurityOverrides.length) {
                            dirtySecurityOverrides.forEach(function (d) { return d.field = null; });
                            vm.dataService.saveSecurityOverides(dirtySecurityOverrides).then(function (result) {
                                vm.isLoading = false;
                                vm.modalInstance.close(result);
                            });
                        }
                        else {
                            vm.modalInstance.dismiss('cancel');
                        }
                    }
                };
                var vm = this;
                vm.modalInstance = $modalInstance;
                vm.dataService = dataService;
                vm.scope = $scope;
                vm.ngTableParams = ngTableParams;
                vm.selectedSecurityId = sourcedata;
                vm.scope.$watch(function () {
                    return vm.selectedSecurity;
                }, function (newValue, oldValue) {
                    if (newValue != oldValue && newValue.securityId && !vm.isLoading && !vm.selectedSecurityId) {
                        vm.loadSecurityOverrides(true);
                    }
                });
                vm.loadData();
            }
            return AddUpdateSecurityOverrideController;
        }());
        AddUpdateSecurityOverrideController.$inject = ["application.services.dataService", "$window", "$scope", "$modalInstance", 'NgTableParams', 'sourcedata'];
        Controllers.AddUpdateSecurityOverrideController = AddUpdateSecurityOverrideController;
        angular.module("app").controller("application.controllers.addUpdateSecurityOverrideController", AddUpdateSecurityOverrideController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=AddUpdateSecurityOverrideController.js.map