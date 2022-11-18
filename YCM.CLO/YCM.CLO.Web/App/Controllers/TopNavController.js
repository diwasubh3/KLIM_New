var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var TopNavController = (function () {
            function TopNavController(modalService, uiService, dataService, $rootScope, ngTableParams, $filter, $window, $interval) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                this.hideNotActive = false;
                this.tooltipTextTemplate = '<div>__CONTENT__</div>';
                this.isDateDisabled = false;
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
                this.loadTestResults = function () {
                    var vm = _this;
                    vm.statusText = "Loading";
                    vm.isLoading = true;
                    vm.window.setTimeout(function () {
                        if (!pageOptions.TestResultsNew)
                            vm.dataService.loadTestResults().then(function (testResultsData) {
                                pageOptions.TestResultsNew = testResultsData;
                                vm.setTestResultsData(testResultsData);
                                //vm.setSummaryData(summaries);
                                vm.setTestTableParams();
                            });
                        else {
                            vm.setTestResultsData(pageOptions.TestResultsNew);
                            vm.setTestTableParams();
                        }
                        //pageOptions.TestResults = summaries;
                        //vm.setSummaryData(summaries);
                    });
                };
                this.loadTrendTypes = function () {
                    var vm = _this;
                    vm.statusText = "Loading";
                    vm.isLoading = true;
                    vm.dataService.loadTrendTypes().then(function (types) {
                        vm.trendtypes = types;
                        vm.trendType = vm.trendtypes[0];
                        vm.isLoading = false;
                    });
                };
                this.loadPeriod = function () {
                    var vm = _this;
                    vm.statusText = "Loading";
                    vm.isLoading = true;
                    vm.dataService.loadPeriod().then(function (periods) {
                        vm.trendPeriod = periods;
                        vm.isLoading = false;
                        var defaultPeriod = periods.filter(function (period) {
                            return period.isDefault === true;
                        });
                        vm.period = defaultPeriod[0];
                        vm.loadTrends();
                        vm.disableTrendDates();
                    });
                };
                this.loadTrends = function () {
                    var vm = _this;
                    vm.statusText = "Loading";
                    vm.isLoading = true;
                    var trendTypeId;
                    //pass params 1- trendtype frdate todate
                    //vm.window.setTimeout(() => {
                    if (vm.trendType)
                        trendTypeId = vm.trendType.typeID;
                    else
                        trendTypeId = 1; // Default
                    var periodId = vm.period ? vm.period.periodId : 1;
                    vm.dataService.loadTrends(vm.startDate.toLocaleDateString(), vm.endDate.toLocaleDateString(), trendTypeId, periodId).then(function (trendResultsData) {
                        pageOptions.TrendsResult = trendResultsData;
                        vm.setTrendsData(trendResultsData);
                        vm.setTrendTableParams();
                    });
                    //if (!pageOptions.TrendsResult) {
                    //    //trendTypeId = vm.trendtype.typeID;
                    //    //if (trendTypeId == 'undefined' || trendTypeId <= 0)
                    //    //    trendTypeId = 1;
                    //    vm.dataService.loadTrends(vm.trendTypeId, vm.startDate.toLocaleDateString(), vm.endDate.toLocaleDateString()).then((trendResultsData) => {
                    //        //debugger;
                    //        pageOptions.TrendsResult = trendResultsData;
                    //        vm.setTrendsData(trendResultsData);
                    //        vm.setTrendTableParams();
                    //    });
                    //}
                    //else {
                    //    vm.setTrendsData(pageOptions.TrendsResult);
                    //    vm.setTrendTableParams();
                    //}
                    vm.isLoading = false;
                    //});
                };
                this.showCharts = function () {
                    var vm = _this;
                    vm.uiService.showChartsPopup(vm.modalService, vm.trendsData);
                };
                this.setTrendsData = function (trendResultsData) {
                    var vm = _this;
                    var redColor = { 'background-color': 'lightcoral', 'color': '#333333' };
                    var greenColor = { 'background-color': 'lightgreen', 'color': '#333333' };
                    trendResultsData.forEach(function (tData, idx) {
                        for (var i = 1; i < 12; i++) {
                            tData["fundOvercollateralization" + i] = (tData["fundOvercollateralization" + i] / 1000000).toFixed(1);
                            tData["fundOvercollateralization" + i + "Old"] = (tData["fundOvercollateralization" + i + "Old"] / 1000000).toFixed(1);
                            if (tData["fundOvercollateralization" + i] === '0.0') {
                                tData["fundOvercollateralization" + i] = '';
                                tData["fund" + i + "BgStyle"] = '';
                            }
                            else {
                                if (trendResultsData[idx + 1]) {
                                    var diffCollateralization = trendResultsData[idx]["fundOvercollateralization" + i] - (trendResultsData[idx + 1]["fundOvercollateralization" + i] / 1000000);
                                    tData["fund" + i + "BgStyle"] = diffCollateralization > 0.3 ? greenColor : diffCollateralization < -0.3 ? redColor : "";
                                }
                            }
                        }
                    });
                    vm.trendsData = trendResultsData;
                };
                this.setTestResultsData = function (testResultsData) {
                    var vm = _this;
                    testResultsData.forEach(function (tData) {
                        var clo = 1;
                        var redColor = { 'background-color': 'RED', 'color': '#333333' };
                        var yellowColor = { 'background-color': 'yellow', 'color': 'rgb(51, 51, 51)' };
                        //var blackFontColor = { 'color': 'black' };
                        var redFontColor = { 'color': 'red' };
                        for (var clo = 1; clo < 12; clo++) {
                            if (tData["fund" + clo + "OutcomeDisplay"] && tData["fund" + clo + "OutcomeDisplay"].includes("$")) {
                                var testOutcome = tData["fund" + clo + "OutcomeDisplay"].split("$")[1].replaceAll(",", '');
                                if (tData["testDisplayName"] === "OC vs Target Par") {
                                    tData["fund" + clo + "OutcomeBgStyle"] = testOutcome < 0 ? redFontColor : '';
                                }
                                var formatter = new Intl.NumberFormat('en-US', {
                                    //style: 'currency',
                                    //currency: 'USD',
                                    // These options are needed to round to whole numbers if that's what you want.
                                    //minimumFractionDigits: 0, // (this suffices for whole numbers, but will print 2500.10 as $2,500.1)
                                    maximumFractionDigits: 0 // (causes 2500.99 to be printed as $2,501)
                                });
                                tData["fund" + clo + "OutcomeDisplay"] = formatter.format(testOutcome).toString();
                            }
                            if (tData.condition && tData["fund" + clo + "OutcomeDisplay"]) {
                                var breachCondition = '', dangerCondition = '';
                                if (tData.condition === ">=") {
                                    breachCondition = "<";
                                    dangerCondition = "<=";
                                }
                                else if (tData.condition === "<=") {
                                    breachCondition = ">";
                                    dangerCondition = ">=";
                                }
                                var calcTooltip = tData["calculatedColumn" + clo].toString();
                                var breachToolTip = tData["fund" + clo + "Breach"].toString();
                                if (tData["fund" + clo + "OutcomeDisplay"].includes(".")) {
                                    calcTooltip = tData["calculatedColumn" + clo].toFixed(2).toString();
                                    breachToolTip = tData["fund" + clo + "Breach"].toFixed(2).toString();
                                }
                                if (tData["fund" + clo + "OutcomeDisplay"].includes("%")) {
                                    calcTooltip = calcTooltip + "%";
                                    breachToolTip = breachToolTip + "%";
                                }
                                tData["fund" + clo + "OutcomeTooltip"] = "<div>DANGER &nbsp;" + dangerCondition + "&nbsp;" + calcTooltip + "</div><div>BREACH&nbsp;" + breachCondition + "&nbsp;" + breachToolTip + "</div>";
                                //console.log("<div>DANGER &nbsp;" + tData.condition + "&nbsp;" + tData["calculatedColumn" + clo].toString() + "</div><div>BREACH&nbsp;" + tData.condition + "&nbsp;" + tData["fund" + clo + "Breach"].toString() + "</div>");
                                if (tData["fund" + clo + "OutcomeDisplay"] !== "N/A") {
                                    if (tData.condition === ">=") {
                                        tData["fund" + clo + "OutcomeBgStyle"] = tData["fund" + clo + "Outcome"] < tData["fund" + clo + "Breach"] ? redColor :
                                            tData["fund" + clo + "Outcome"] <= tData["calculatedColumn" + clo] && tData["fund" + clo + "Outcome"] >= tData["fund" + clo + "Breach"] ? yellowColor : {};
                                    }
                                    else if (tData.condition === "<=") {
                                        tData["fund" + clo + "OutcomeBgStyle"] = tData["fund" + clo + "Outcome"] > tData["fund" + clo + "Breach"] ? redColor :
                                            tData["fund" + clo + "Outcome"] >= tData["calculatedColumn" + clo] && tData["fund" + clo + "Outcome"] <= tData["fund" + clo + "Breach"] ? yellowColor : {};
                                    }
                                }
                            }
                        }
                        if (tData["testHoverNote"] !== "") {
                            tData["testDisplayNameTooltip"] = tData["testHoverNote"];
                        }
                    });
                    var groupBy = function (xs, key) {
                        return xs.reduce(function (rv, x) {
                            (rv[x[key]] = rv[x[key]] || []).push(x);
                            return rv;
                        }, {});
                    };
                    var groupByData = [];
                    var dataWithGroups = groupBy(testResultsData, 'testCategoryName');
                    Object.keys(dataWithGroups).forEach(function (key) {
                        var groupStyle = {
                            "font-size": "small",
                            "font-weight": "bold"
                        };
                        groupByData.push({ 'testDisplayName': key, 'testDisplayNameBgStyle': groupStyle });
                        dataWithGroups[key].forEach(function (tData) {
                            groupByData.push(tData);
                        });
                        groupByData.push({});
                    });
                    //vm.testResultsData = testResultsData;
                    vm.testResultsData = groupByData;
                    vm.selectTest(groupByData[1]);
                    //console.log(testResultsData);
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
                this.periodChangeEvent = function () {
                    _this.disableTrendDates();
                };
                this.disableTrendDates = function () {
                    var vm = _this;
                    if (vm.period.periodName === "Daily") {
                        vm.isDateDisabled = false;
                    }
                    else {
                        vm.isDateDisabled = true;
                    }
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
                this.selectTest = function (testRow) {
                    var vm = _this;
                    if (vm.selectedTest != testRow) {
                        vm.selectedTest = testRow;
                        vm.rootScope['selectedTest'] = testRow;
                    }
                };
                this.selectTrend = function (trend) {
                    var vm = _this;
                    if (vm.selectedTrend != trend) {
                        vm.selectedTrend = trend;
                        vm.rootScope['selectedTrend'] = trend;
                    }
                };
                this.selectPar = function (fundRow) {
                    //alert(fundRow.par);
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
                this.setTestTableParams = function () {
                    var vm = _this;
                    //if (!vm.summaryTableParams) {
                    vm.testResultsTableParams = new vm.ngTestTableParamas({
                        page: 1,
                        noPager: true,
                        count: 10000
                        //sorting: {
                        //    'fundCode': 'asc'
                        //}
                    }, {
                        total: 1,
                        counts: [],
                        dataset: vm.testResultsData
                    });
                };
                this.setTrendTableParams = function () {
                    var vm = _this;
                    //if (!vm.summaryTableParams) {
                    vm.trendTableParams = new vm.ngTrendTableParams({
                        page: 1,
                        noPager: true,
                        count: 10000
                        //sorting: {
                        //    'fundCode': 'asc'
                        //}
                    }, {
                        total: 1,
                        counts: [],
                        dataset: vm.trendsData
                    });
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
                vm.ngTestTableParamas = ngTableParams;
                vm.ngTrendTableParams = ngTableParams;
                vm.modalService = modalService;
                var yesterday = new Date(new Date());
                yesterday.setDate(yesterday.getDate() - 1);
                var lastMonthdate = new Date(new Date());
                lastMonthdate.setDate(lastMonthdate.getDate() - 45);
                vm.startDate = lastMonthdate;
                vm.endDate = yesterday;
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
                vm.loadPeriod();
                vm.loadTrendTypes();
                vm.loadData();
                vm.loadTestResults();
                //vm.loadTrends();
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
        TopNavController.$inject = ["$uibModal", "application.services.uiService", "application.services.dataService", "$rootScope", 'NgTableParams', '$filter', '$window', '$interval'];
        Controllers.TopNavController = TopNavController;
        angular.module("app").controller("topNavController", TopNavController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=TopNavController.js.map