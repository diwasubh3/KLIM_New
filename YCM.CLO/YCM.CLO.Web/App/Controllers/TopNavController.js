var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var TopNavController = (function () {
            function TopNavController(uiService, dataService, $rootScope, ngTableParams, $filter, $window, $interval) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                this.hideNotActive = false;
                this.tooltipTextTemplate = '<div>__CONTENT__</div>';
                this.checkForAutoRefresh = function () {
                    var vm = _this;
                    if (vm.isAutoRefresh) {
                        vm.refreshSummariesAndFundRestrictions();
                        vm.rootScope.$emit('onAutoRefresh', vm.selectedFund);
                    }
                };
                this.refreshSummariesAndFundRestrictions = function () {
                    var vm = _this;
                    vm.refreshSummaries();
                    vm.refreshFundRestrictions();
                };
                this.refreshSummaries = function () {
                    var vm = _this;
                    vm.dataService.loadSummaryData().then(function (summaries) {
                        var selectedFundCode = vm.selectedFund.fundCode;
                        vm.summaryData = summaries;
                        vm.setParamsTable();
                        vm.window.setTimeout(function () {
                            if (summaries.length) {
                                vm.selectFund(summaries.filter(function (f) { return f.fundCode == selectedFundCode; })[0]);
                                vm.setBackgroundStyleBasedOnFundRestrictions();
                            }
                        }, 100);
                    });
                };
                this.refreshFundRestrictions = function () {
                    var vm = _this;
                    vm.dataService.loadFundRestrictions(null).then(function (fundrestrictions) {
                        vm.fundRestrictions = fundrestrictions;
                        vm.setBackgroundStyleBasedOnFundRestrictions();
                    });
                };
                this.setBackgroundStyleBasedOnFundRestrictions = function () {
                    var vm = _this;
                    vm.dataService.loadFundRestrictionFields().then(function (fields) {
                        vm.summaryData.forEach(function (summary) {
                            vm.fundRestrictionFields.forEach(function (fundRestrictionField) {
                                var rowStyles = vm.getBackgroundColorStyle(fundRestrictionField.jsonPropertyName, summary);
                                var jsonPropName = fundRestrictionField.jsonPropertyName;
                                var bodJsonPropName = 'bod' + vm.uiService.capitalizeFirstLetter(fundRestrictionField.jsonPropertyName);
                                summary[jsonPropName + 'BgStyle'] = rowStyles.topRow;
                                summary[bodJsonPropName + 'BgStyle'] = rowStyles.bodRow;
                                summary[jsonPropName + 'Tooltip'] = rowStyles.toolTipText;
                                summary[bodJsonPropName + 'Tooltip'] = rowStyles.toolTipText;
                            });
                        });
                    });
                };
                this.hideUnhideNotActive = function () {
                    var vm = _this;
                    vm.hideNotActive = !vm.hideNotActive;
                    vm.summaryData.forEach(function (summary) {
                        summary.inActive = vm.hideNotActive;
                    });
                    vm.rootScope.$emit('onFundVisibleRowsChanged', vm.summaryData.filter(function (fundRow) {
                        return (vm.selectedFund == fundRow && fundRow.inActive);
                    }).length);
                };
                this.loadData = function () {
                    var vm = _this;
                    vm.statusText = "Loading";
                    vm.isLoading = true;
                    vm.window.setTimeout(function () {
                        vm.dataService.loadFundRestrictionsTypes().then(function (fundrestrictionTypes) {
                            vm.fundRestrictionTypes = fundrestrictionTypes;
                            vm.dataService.loadFundRestrictionFields().then(function (fundRestrictionFields) {
                                vm.fundRestrictionFields = fundRestrictionFields;
                                vm.dataService.loadFundRestrictions(null).then(function (fundrestrictions) {
                                    vm.fundRestrictions = fundrestrictions;
                                    if (!pageOptions.TestResults)
                                        vm.dataService.loadSummaryData().then(function (summaries) {
                                            pageOptions.TestResults = summaries;
                                            vm.setSummaryData(summaries);
                                        });
                                    else
                                        vm.setSummaryData(pageOptions.TestResults);
                                });
                            });
                        });
                    });
                };
                this.setSummaryData = function (summaries) {
                    var vm = _this;
                    vm.summaryData = summaries;
                    vm.setBackgroundStyleBasedOnFundRestrictions();
                    vm.window.setTimeout(function () {
                        vm.isLoading = false;
                        vm.setParamsTable();
                        if (summaries.length) {
                            vm.selectFund(summaries[0]);
                        }
                    });
                };
                this.getBackgroundColorStyle = function (fieldName, summaryRow) {
                    var vm = _this;
                    var color = '';
                    var bodColor = '';
                    var fieldFundRestrictions = vm.fundRestrictions.filter(function (fr) {
                        return fr.jsonPropertyName == fieldName;
                    });
                    var rowStyle = { toolTipText: '' };
                    if (fieldFundRestrictions.length) {
                        //TODO - unify logic, maybe move to c# controller
                        //var assetParFundRestrictions = fieldFundRestrictions.filter(fieldFundRestriction => { return fieldFundRestriction.fieldName == 'AssetPar' && fieldFundRestriction.fundId == summaryRow.fundId });
                        if (fieldName == 'assetPar') {
                            //var assetParFundRestrictions = vm.getAssetParRestrictions('AssetPar', summaryRow.fundId);
                            //if (assetParFundRestrictions.length) {
                            //	color = assetParFundRestrictions[0].displayColor;
                            //	if (!rowStyle.toolTipText)
                            //		rowStyle.toolTipText = assetParFundRestrictions[0].fundRestrictionToolTip;
                            //}
                        }
                        vm.fundRestrictionTypes.forEach(function (frt) {
                            var fundRestrictions = fieldFundRestrictions.filter(function (fieldFundRestriction) { return fieldFundRestriction.fundRestrictionTypeId == frt.fundRestrictionTypeId && fieldFundRestriction.fundId == summaryRow.fundId; });
                            if (fundRestrictions.length) {
                                if (eval(summaryRow[fieldName] + fundRestrictions[0].operatorVal + fundRestrictions[0].restrictionValue)) {
                                    color = frt.displayColor;
                                }
                                if (eval(summaryRow['bod' + vm.uiService.capitalizeFirstLetter(fieldName)] + fundRestrictions[0].operatorVal + fundRestrictions[0].restrictionValue)) {
                                    bodColor = frt.displayColor;
                                }
                                var tooltipText = vm.tooltipTextTemplate.replace('__CONTENT__', (frt.fundRestrictionTypeName.replace('IN ', '').replace(' ', '&nbsp;') + '&nbsp;' + fundRestrictions[0].operatorCode.toString() + '&nbsp;' + vm.filter('currency')(fundRestrictions[0].restrictionValue, '', 2)));
                                rowStyle.toolTipText += tooltipText;
                            }
                        });
                    }
                    if (color) {
                        rowStyle.topRow = { 'background-color': color, 'color': '#333333' };
                    }
                    else {
                        rowStyle.topRow = null;
                    }
                    if (bodColor) {
                        rowStyle.bodRow = { 'background-color': bodColor, 'color': '#333333' };
                    }
                    else {
                        rowStyle.bodRow = null;
                    }
                    return rowStyle;
                };
                this.selectFund = function (fundRow) {
                    var vm = _this;
                    if (!vm.selectedFund || (vm.selectedFund && vm.selectedFund.fundId !== fundRow.fundId)) {
                        vm.rootScope.$emit('onFundSelectionChanged', fundRow);
                    }
                    if (vm.selectedFund != fundRow) {
                        vm.selectedFund = fundRow;
                        vm.rootScope['selectedFund'] = fundRow;
                        vm.rootScope.$emit('onFundChanged', fundRow);
                    }
                };
                this.downloadLoanPositions = function (fundId) {
                    var vm = _this;
                    var fieldFundRestrictions = vm.getAssetParRestrictions('AssetPar', fundId);
                    //var fieldFundRestrictions = vm.fundRestrictions.filter(fr => {
                    //	return fr.fieldName == 'AssetPar' && fr.fundId == fundId;
                    //});
                    var rest = fieldFundRestrictions[0];
                    if (rest.isDifferenceOverThreshold)
                        vm.dataService.downloadLoanPositions(fundId);
                };
                this.downloadReInvestCash = function (Url) {
                    var vm = _this;
                    vm.dataService.downloadReInvestCash(Url);
                };
                this.setParamsTable = function () {
                    var vm = _this;
                    //if (!vm.summaryTableParams) {
                    vm.summaryTableParams = new vm.ngTableParams({
                        page: 1,
                        noPager: true,
                        count: 10000
                        //sorting: {
                        //    'fundCode': 'asc'
                        //}
                    }, {
                        total: 1,
                        counts: [],
                        dataset: vm.summaryData
                    });
                    //} else {
                    //    vm.summaryTableParams.reload();
                    //}
                };
                var vm = this;
                vm.uiService = uiService;
                vm.rootScope = $rootScope;
                vm.ngTableParams = ngTableParams;
                vm.dataService = dataService;
                vm.activeView = 'home';
                vm.window = $window;
                vm.filter = $filter;
                vm.interval = $interval;
                vm.rootScope.$on('onActivated', function (event, data) {
                    vm.activeView = data;
                    vm.rootScope['activeView'] = data;
                });
                vm.rootScope.$on('refreshSummaries', function (event, data) {
                    vm.refreshSummariesAndFundRestrictions();
                });
                vm.rootScope.$on('refreshFundRestrictions', function (event, data) {
                    vm.refreshFundRestrictions();
                });
                vm.loadData();
                vm.interval(vm.checkForAutoRefresh, 300000);
            }
            TopNavController.prototype.onTestResultsVisibilityChanged = function (open) {
                var vm = this;
                vm.rootScope.$emit('onTestResultsVisibilityChanged', open);
            };
            TopNavController.prototype.getAssetParRestrictions = function (fieldName, fundId) {
                var vm = this;
                var fieldFundRestrictions = vm.fundRestrictions.filter(function (fr) {
                    return fr.fieldName == fieldName && fr.fundId == fundId;
                });
                return fieldFundRestrictions;
            };
            return TopNavController;
        }());
        TopNavController.$inject = ["application.services.uiService", "application.services.dataService", "$rootScope", 'NgTableParams', '$filter', '$window', '$interval'];
        Controllers.TopNavController = TopNavController;
        angular.module("app").controller("topNavController", TopNavController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=TopNavController.js.map