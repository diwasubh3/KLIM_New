var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var TopNavController = (function () {
            function TopNavController($timeout, modalService, uiService, dataService, $rootScope, ngTableParams, $filter, $window, $interval) {
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
                        vm.loadTrends(true);
                        vm.disableTrendDates();
                    });
                };
                this.loadTrends = function (isLoadCharts) {
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
                        vm.setTrendsData(trendResultsData, isLoadCharts);
                        vm.setTrendTableParams();
                    });
                    vm.isLoading = false;
                };
                this.showCharts = function () {
                    var vm = _this;
                    vm.uiService.showChartsPopup(vm.modalService, vm.trendsData, vm.trendPeriod, vm.trendtypes, vm.period, vm.trendType);
                };
                this.setTrendsData = function (trendResultsData, isLoadCharts) {
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
                    if (isLoadCharts) {
                        vm.loadCharts();
                    }
                };
                this.setTestResultsData = function (testResultsData) {
                    var vm = _this;
                    testResultsData.forEach(function (tData) {
                        var CLO = 1;
                        var redColor = { 'background-color': 'RED', 'color': '#333333' };
                        var yellowColor = { 'background-color': 'yellow', 'color': 'rgb(51, 51, 51)' };
                        //var blackFontColor = { 'color': 'black' };
                        var redFontColor = { 'color': 'red' };
                        for (var clo = 1; clo < 12; clo++) {
                            if (tData["fund" + clo + "OutcomeDisplay"] && tData["fund" + clo + "OutcomeDisplay"].includes("$")) {
                                var testOutcome = tData["fund" + clo + "OutcomeDisplay"].split("$")[1].replaceAll(",", '');
                                if (testOutcome.includes(")")) {
                                    testOutcome = '-' + testOutcome.replace(')', '');
                                }
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
                                var dangerDiv = "<div>DANGER &nbsp;" + dangerCondition + "&nbsp;" + calcTooltip + "</div>";
                                var breachDiv = "<div>BREACH&nbsp;" + breachCondition + "&nbsp;" + breachToolTip + "</div>";
                                tData["fund" + clo + "OutcomeTooltip"] = tData["hideDanger"] ? breachDiv : dangerDiv + breachDiv;
                                //console.log("<div>DANGER &nbsp;" + tData.condition + "&nbsp;" + tData["calculatedColumn" + clo].toString() + "</div><div>BREACH&nbsp;" + tData.condition + "&nbsp;" + tData["fund" + clo + "Breach"].toString() + "</div>");
                                if (tData["fund" + clo + "OutcomeDisplay"] !== "N/A") {
                                    if (tData.condition === ">=") {
                                        tData["fund" + clo + "OutcomeBgStyle"] = tData["fund" + clo + "Outcome"] < tData["fund" + clo + "Breach"] ? redColor :
                                            tData["fund" + clo + "Outcome"] <= tData["calculatedColumn" + clo] && tData["fund" + clo + "Outcome"] >= tData["fund" + clo + "Breach"] && !tData["hideDanger"] ? yellowColor : {};
                                    }
                                    else if (tData.condition === "<=") {
                                        tData["fund" + clo + "OutcomeBgStyle"] = tData["fund" + clo + "Outcome"] > tData["fund" + clo + "Breach"] ? redColor :
                                            tData["fund" + clo + "Outcome"] >= tData["calculatedColumn" + clo] && tData["fund" + clo + "Outcome"] <= tData["fund" + clo + "Breach"] && !tData["hideDanger"] ? yellowColor : {};
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
                this.loadCharts = function () {
                    var vm = _this;
                    var trendsData = vm.trendsData.slice(); // Slice is added to clone data of trendsdata.
                    trendsData = trendsData.sort(function (a, b) {
                        return new Date(a.trendDate).getTime() - new Date(b.trendDate).getTime();
                    });
                    //Add Filter function to segrgate data
                    trendsData.forEach(function (tData, idx) {
                        var hasBlankData = false;
                        for (var i = 1; i < 11; i++) {
                            if (tData["fundOvercollateralization" + i] === '') {
                                hasBlankData = true;
                                break;
                            }
                        }
                        if (hasBlankData) {
                            trendsData[idx].hasBlankData = true;
                        }
                    });
                    trendsData = trendsData.filter(function (tData) {
                        return tData.hasBlankData !== true;
                    });
                    vm.timeoutService(function () {
                        if (vm.trendsChart) {
                            vm.trendsChart.destroy();
                        }
                        var chartDiv = document.getElementById('trendsChart');
                        var fundColors = ['', '#36a2eb', '#cc65fe', '#00ff80', '#7320BD', '#ff99ff', '#0000ff', '#009999', '#cc0000', '#003399', '#ff6384'];
                        var chartDataSet = [];
                        for (var i = 1; i < 11; i++) {
                            var hidden = true;
                            if (vm.selectedCLOs.length === 0) {
                                hidden = i === 1 ? false : true;
                            }
                            else {
                                var activeClo = vm.selectedCLOs.filter(function (clo) {
                                    return clo == 'CLO' + i;
                                });
                                if (activeClo.length) {
                                    hidden = false;
                                }
                            }
                            var fundDataSet = {
                                label: 'CLO' + i,
                                data: trendsData.map(function (row) { return row["fundOvercollateralization" + i]; }),
                                borderColor: fundColors[i],
                                fill: false,
                                lineTension: 0.4,
                                hidden: hidden,
                                type: 'line'
                            };
                            if (!hidden) {
                                vm.selectedCLOs.push(fundDataSet.label);
                            }
                            chartDataSet.push(fundDataSet);
                        }
                        var newLegendClickHandler = function (e, legendItem, legend) {
                            var index = legendItem.datasetIndex;
                            var ci = legend.chart;
                            if (ci.isDatasetVisible(index)) {
                                ci.data.datasets[index].hidden = true;
                                ci.hide(index);
                                vm.selectedCLOs = vm.selectedCLOs.filter(function (clo) {
                                    return clo !== legendItem.text;
                                });
                                legendItem.hidden = true;
                            }
                            else {
                                ci.data.datasets[index].hidden = false;
                                ci.show(index);
                                legendItem.hidden = false;
                                vm.selectedCLOs.push(legendItem.text);
                            }
                        };
                        vm.trendsChart = new vm.window.Chart(chartDiv, {
                            data: {
                                labels: trendsData.map(function (row) { return row.trendDate; }),
                                datasets: chartDataSet
                            },
                            options: {
                                showLines: true,
                                plugins: {
                                    legend: {
                                        labels: {
                                            usePointStyle: true,
                                            generateLabels: function (chart) {
                                                //console.log(chart);
                                                var pointStyle = [];
                                                chart.data.datasets.forEach(function (dataset, index) {
                                                    if (dataset.hidden === true) {
                                                        pointStyle.push({ type: 'crossRot', color: ' #000000' });
                                                    }
                                                    else {
                                                        pointStyle.push({ type: 'circle', color: fundColors[index + 1] });
                                                    }
                                                });
                                                return chart.data.datasets.map(function (dataset, index) { return ({
                                                    text: dataset.label,
                                                    fillStyle: pointStyle[index].color,
                                                    strokeStyle: pointStyle[index].color,
                                                    pointStyle: pointStyle[index].type,
                                                    hidden: false,
                                                    datasetIndex: index
                                                }); });
                                            }
                                        },
                                        onClick: newLegendClickHandler
                                    }
                                },
                                maintainAspectRatio: false
                            },
                        });
                    });
                };
                this.exportToPdf = function () {
                    var vm = _this;
                    var active = $("ul.nav.nav-tabs.testResultsTab li.active a").attr('href');
                    active = active.split('#')[1];
                    var lodingDiv = document.getElementById('lodingDiv');
                    lodingDiv.hidden = false;
                    vm.window.setTimeout(function () {
                        var dateid = vm.summaryData.length > 0 ? +vm.summaryData[0].dateId.toString().substr(4, 2) + '/' + vm.summaryData[0].dateId.toString().substr(6, 2) + '/' + vm.summaryData[0].dateId.toString().substr(0, 4) : '';
                        //var pdf = new vm.window.jsPDF('l', 'pt', 'letter');
                        var pdfSettings = {
                            startRow: 0,
                            rowsPerPage: 45,
                            downloadFileName: '',
                            imageHeight: 190
                        };
                        if (active === "testResult1") {
                            active = "tblSummaryBody";
                            pdfSettings.downloadFileName = 'Summary_';
                        }
                        else if (active === "testResult2") {
                            active = "tblTestResult2";
                            pdfSettings.downloadFileName = 'WSOCompliance_';
                        }
                        else if (active === "trends") {
                            active = "tbTrends";
                            pdfSettings.downloadFileName = 'Trends_';
                        }
                        else if (active === "trendCharts") {
                            pdfSettings.rowsPerPage = 0;
                            pdfSettings.downloadFileName = 'TrendCharts';
                            pdfSettings.imageHeight = 150;
                        }
                        vm.originalElement = document.getElementById(active);
                        var sourceArray = [];
                        if (vm.originalElement.tBodies && vm.originalElement.tBodies.length) {
                            vm.source = vm.originalElement.cloneNode(true);
                            var reminder = vm.source.tBodies.length % pdfSettings.rowsPerPage;
                            var quotient = vm.source.tBodies.length / pdfSettings.rowsPerPage;
                            var pages = 0;
                            pages = reminder > 0 ? Math.floor(quotient) + 1 : quotient;
                            var startRow = pdfSettings.startRow, endrow = pdfSettings.rowsPerPage;
                            for (var i = 0; i < pages; i++) {
                                vm.source = vm.originalElement.cloneNode(true);
                                vm.source.id = vm.source.id + i;
                                var bodLength = Object.keys(vm.source.tBodies).length;
                                for (var idx = bodLength - 1; idx >= 0; idx--) {
                                    if (idx >= startRow && idx <= endrow) {
                                        //console.log(idx);
                                    }
                                    else {
                                        if (vm.source.tBodies[idx])
                                            vm.source.tBodies[idx].parentElement.removeChild(vm.source.tBodies[idx]);
                                    }
                                }
                                sourceArray.push(vm.source.cloneNode(true));
                                startRow = endrow + 1;
                                endrow = startRow + pdfSettings.rowsPerPage - 1;
                            }
                            if (sourceArray.length) {
                                for (var x = 0; x <= sourceArray.length - 1; x++) {
                                    document.body.appendChild(sourceArray[x]);
                                }
                            }
                        }
                        else {
                            sourceArray.push(vm.originalElement);
                        }
                        var canvasImages = [];
                        vm.imagePromises = [];
                        for (var x = 0; x <= sourceArray.length - 1; x++) {
                            vm.imagePromises.push(vm.imagePromise(canvasImages, sourceArray[x], pdfSettings));
                        }
                        vm.window.Promise.all(vm.imagePromises).then(function (data) {
                            var pdf = new vm.window.jspdf.jsPDF('l');
                            lodingDiv.hidden = true;
                            canvasImages.forEach(function (canvas, idx) {
                                pdf.addImage(canvas.image, "JPEG", 15, 15, 265, canvas.imageHeight);
                                if (canvasImages.length - 1 !== idx) {
                                    pdf.addPage();
                                }
                            });
                            if (active !== "trendCharts") {
                                for (var x = 0; x <= sourceArray.length - 1; x++) {
                                    document.body.removeChild(sourceArray[x]);
                                }
                            }
                            sourceArray = [];
                            pdf.save(pdfSettings.downloadFileName + dateid + ".pdf");
                        });
                    }, 500);
                    //vm.window.html2canvas(vm.source, {
                    //    allowTaint: true,
                    //    useCORS: true
                    //}).then(function (canvas) {
                    //    let pdf = new vm.window.jsPDF('l');
                    //    pdf.setFontSize(20)
                    //    pdf.addImage(canvas, "JPEG",0,0,280,205)
                    //    pdf.save("testenrty.pdf")
                    //});
                };
                this.imagePromise = function (canvasImages, src, pdfSettings) {
                    var vm = _this;
                    console.log(src.id);
                    var imageHeight = pdfSettings.imageHeight; //as a4 Page Size in mm is 209 mm , 15 mm is added as border
                    if (src.tBodies && src.tBodies.length < pdfSettings.rowsPerPage) {
                        var rowHeight = imageHeight / pdfSettings.rowsPerPage;
                        imageHeight = rowHeight * src.tBodies.length;
                    }
                    var imageElement = document.getElementById(src.id);
                    return new vm.window.Promise(function (resolve, reject) {
                        vm.window.html2canvas(imageElement, {
                            scale: 2,
                            allowTaint: true,
                            useCORS: true
                        }).then(function (canvas) {
                            canvasImages.push({ image: canvas, imageHeight: imageHeight });
                            resolve(canvas);
                        });
                    });
                };
                this.exportToCSV = function () {
                    var vm = _this;
                    var activeTab = "trendsData";
                    var CsvData = [];
                    var active = $("ul.nav.nav-tabs.testResultsTab li.active a").attr('href');
                    var dateid = vm.summaryData.length > 0 ? +vm.summaryData[0].dateId.toString().substr(4, 2) + '/' + vm.summaryData[0].dateId.toString().substr(6, 2) + '/' + vm.summaryData[0].dateId.toString().substr(0, 4) : '';
                    if (active === "#testResult1") {
                        vm.summaryData.forEach(function (line) {
                            //let reportDate = new Date(line.tradeDate);
                            var csvLine = {
                                'CLO': line.fundCode,
                                'Total Par': line.par,
                                'Asset Par': line.assetPar,
                                'Principal Cash': line.principalCash,
                                '%Cash': line.cashPer + (line.cashPer != null && line.cashPer.toString().length ? '%' : ''),
                                'Reinvest Cash': line.reInvestCash,
                                'WAS': line.wsoSpread,
                                'SOFR': line.sOFR,
                                'WARF': line.wsowarf,
                                'B3%': line.b3ToAssetParPct + (line.b3ToAssetParPct != null && line.b3ToAssetParPct.toString().length ? '%' : ''),
                                'B-%': line.bMinusToAssetParPct + (line.bMinusToAssetParPct != null && line.bMinusToAssetParPct.toString().length ? '%' : ''),
                                'WAL Cushion': line.walCushion,
                                'Time to Reinvest': line.timeToReinvest,
                                "Moody's Recovery": line.wsoMoodyRecovery,
                                'Diversity': line.wsoDiversity,
                                'WA Bid': line.bid,
                                'WAPP': line.wapp,
                                'BB MVOC': line.bbmvoc + (line.bbmvoc != null && line.bbmvoc.toString().length ? '%' : ''),
                                'Clean Nav': line.cleanNav
                            };
                            CsvData.push(csvLine);
                        });
                        vm.exportToCsv('Summary_' + dateid + '.csv', CsvData);
                    }
                    else if (active === "#testResult2") {
                        vm.testResultsData.forEach(function (line) {
                            //let reportDate = new Date(line.tradeDate);
                            var csvLine = {
                                'Test Name': line.testDisplayName,
                                CLO1: line.fund1OutcomeDisplay,
                                CLO2: line.fund2OutcomeDisplay,
                                CLO3: line.fund3OutcomeDisplay,
                                CLO4: line.fund4OutcomeDisplay,
                                CLO5: line.fund5OutcomeDisplay,
                                CLO6: line.fund6OutcomeDisplay,
                                CLO7: line.fund7OutcomeDisplay,
                                CLO8: line.fund8OutcomeDisplay,
                                CLO9: line.fund9OutcomeDisplay,
                                CLO10: line.fund10OutcomeDisplay,
                                CLO11: line.fund11OutcomeDisplay
                            };
                            CsvData.push(csvLine);
                        });
                        vm.exportToCsv('Compliance_' + dateid + '.csv', CsvData);
                    }
                    else if (active === "#trends" || active === "#trendCharts") {
                        vm.trendsData.forEach(function (line) {
                            //let reportDate = new Date(line.tradeDate);
                            var csvLine = {
                                DATE: line.trendDate,
                                CLO1: line.fundOvercollateralization1,
                                CLO2: line.fundOvercollateralization2,
                                CLO3: line.fundOvercollateralization3,
                                CLO4: line.fundOvercollateralization4,
                                CLO5: line.fundOvercollateralization5,
                                CLO6: line.fundOvercollateralization6,
                                CLO7: line.fundOvercollateralization7,
                                CLO8: line.fundOvercollateralization8,
                                CLO9: line.fundOvercollateralization9,
                                CLO10: line.fundOvercollateralization10,
                                CLO11: line.fundOvercollateralization11
                            };
                            CsvData.push(csvLine);
                        });
                        vm.exportToCsv('trendata_' + dateid + '.csv', CsvData);
                    }
                };
                this.exportToCsv = function (filename, rows) {
                    if (!rows || !rows.length) {
                        return;
                    }
                    var separator = ',';
                    var keys = Object.keys(rows[0]);
                    var csvContent = keys.join(separator) +
                        '\n' +
                        rows.map(function (row) {
                            return keys.map(function (k) {
                                var cell = row[k] === null || row[k] === undefined ? '' : row[k];
                                cell = cell instanceof Date
                                    ? cell.toLocaleString()
                                    : cell.toString().replace(/"/g, '""');
                                if (cell.search(/("|,|\n)/g) >= 0) {
                                    cell = "\"" + cell + "\"";
                                }
                                return cell;
                            }).join(separator);
                        }).join('\n');
                    var blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
                    if (navigator.msSaveBlob) {
                        navigator.msSaveBlob(blob, filename);
                    }
                    else {
                        var link = document.createElement('a');
                        if (link.download !== undefined) {
                            // Browsers that support HTML5 download attribute
                            var url = URL.createObjectURL(blob);
                            link.setAttribute('href', url);
                            link.setAttribute('download', filename);
                            link.style.visibility = 'hidden';
                            document.body.appendChild(link);
                            link.click();
                            document.body.removeChild(link);
                        }
                    }
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
                vm.timeoutService = $timeout;
                var yesterday = new Date(new Date());
                yesterday.setDate(yesterday.getDate() - 1);
                var lastMonthdate = new Date(new Date());
                lastMonthdate.setDate(lastMonthdate.getDate() - 45);
                vm.startDate = lastMonthdate;
                vm.endDate = yesterday;
                vm.selectedCLOs = [];
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
        TopNavController.$inject = ['$timeout', "$uibModal", "application.services.uiService", "application.services.dataService", "$rootScope", 'NgTableParams', '$filter', '$window', '$interval'];
        Controllers.TopNavController = TopNavController;
        angular.module("app").controller("topNavController", TopNavController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=TopNavController.js.map