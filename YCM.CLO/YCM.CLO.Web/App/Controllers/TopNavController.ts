
module Application.Controllers {
    export class TopNavController {
        modalService: angular.ui.bootstrap.IModalService;
        rootScope: ng.IRootScopeService;
        summaryTableParams: any;
        testResultsTableParams: any;
        trendTableParams: any;
        ngTableParams: any;
        ngTestTableParamas: any;
        ngTrendTableParams: any;
        uiService: Application.Services.Contracts.IUIService;
        statusText: string;
        isLoading: boolean;
        summaryData: Array<Application.Models.ISummary>;
        testResultsData: Array<Application.Models.ITestResults>;
        trendsData: Array<Application.Models.ITrends>
        dataService: Application.Services.Contracts.IDataService;
        window: ng.IWindowService;
        activeView: string;
        filter: ng.IFilterService;
        selectedFund: Models.ISummary;
        selectedTest: Models.ITestResults;
        fundRestrictionTypes: Array<Models.IFundRestrictionsTypes>;
        fundRestrictions: Array<Models.IFundRestriction>;
        appBasePath: string = pageOptions.appBasePath;
        isAutoRefresh: boolean;
        hideNotActive: boolean = false;
        fundRestrictionFields: Array<Models.IField>;
        interval: ng.IIntervalService;
        testResultIsOpen: boolean;
        startDate: any;
        endDate: any;
        //trendTypeId: number =1;
        trendtypes: Array<Models.ITrendType>;
        trendType: any;
        trendPeriod: Array<Models.ITrendPeriod>;
        period: any;
        selectedTrend: any;
        tooltipTextTemplate: string = '<div>__CONTENT__</div>';
        isDateDisabled = false;
        trendsChart: any;
        timeoutService: ng.ITimeoutService;
        selectedCLOs: any;
        source: any;
        originalElement: any;
        imagePromises: any;
       


       
        static $inject = ['$timeout',"$uibModal","application.services.uiService", "application.services.dataService", "$rootScope", 'NgTableParams', '$filter', '$window', '$interval'];
        constructor($timeout: ng.ITimeoutService,modalService: angular.ui.bootstrap.IModalService,uiService: Application.Services.Contracts.IUIService, dataService: Application.Services.Contracts.IDataService, $rootScope: ng.IRootScopeService, ngTableParams: NgTableParams, $filter: ng.IFilterService, $window: ng.IWindowService, $interval: ng.IIntervalService) {
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
            
            const yesterday = new Date(new Date())
            yesterday.setDate(yesterday.getDate() - 1);
            const lastMonthdate = new Date(new Date())
            lastMonthdate.setDate(lastMonthdate.getDate() - 45);
            vm.startDate = lastMonthdate;
            vm.endDate = yesterday;
            vm.selectedCLOs = [];

            vm.rootScope.$on('onActivated', (event, data) => {
                vm.activeView = data;
                vm.rootScope['activeView'] = data;
            });

            vm.rootScope.$on('refreshSummaries', (event, data: Models.ISummary) => {
                vm.refreshSummariesAndFundRestrictions();
            });

            vm.rootScope.$on('refreshFundRestrictions', (event, data: Models.ISummary) => {
                vm.refreshFundRestrictions();
            });
            vm.loadPeriod();
            vm.loadTrendTypes();
            vm.loadData();
            vm.loadTestResults();
            //vm.loadTrends();

            vm.interval(vm.checkForAutoRefresh, 300000);
        }

        checkForAutoRefresh = () => {
            var vm = this;
            if (vm.isAutoRefresh) {
                vm.refreshSummariesAndFundRestrictions();
                vm.rootScope.$emit('onAutoRefresh', vm.selectedFund);
            }
        }

        refreshSummariesAndFundRestrictions = () => {
            var vm = this;
            vm.refreshSummaries();
            vm.refreshFundRestrictions();
        }

        refreshSummaries = () => {
            var vm = this;
            vm.dataService.loadSummaryData().then((summaries) => {
                var selectedFundCode = vm.selectedFund.fundCode;
                vm.summaryData = summaries;
                vm.setParamsTable();
                vm.window.setTimeout(() => {
                    if (summaries.length) {
                        vm.selectFund(summaries.filter(f => { return f.fundCode == selectedFundCode })[0]);
                        vm.setBackgroundStyleBasedOnFundRestrictions();
                    }
                }, 100);
            });
        }

        refreshFundRestrictions = () => {
            var vm = this;
            vm.dataService.loadFundRestrictions(null).then(fundrestrictions => {
                vm.fundRestrictions = fundrestrictions;
                vm.setBackgroundStyleBasedOnFundRestrictions();
            });
        }

        setBackgroundStyleBasedOnFundRestrictions = () => {
            var vm = this;
            vm.dataService.loadFundRestrictionFields().then(fields => {
                vm.summaryData.forEach(summary => {
                    vm.fundRestrictionFields.forEach((fundRestrictionField: Models.IField) => {
                        var rowStyles = vm.getBackgroundColorStyle(<string>fundRestrictionField.jsonPropertyName, summary);

                        var jsonPropName = fundRestrictionField.jsonPropertyName;
                        var bodJsonPropName = 'bod' + vm.uiService.capitalizeFirstLetter(fundRestrictionField.jsonPropertyName);
                        summary[jsonPropName + 'BgStyle'] = rowStyles.topRow;
                        summary[bodJsonPropName + 'BgStyle'] = rowStyles.bodRow;
                        summary[jsonPropName + 'Tooltip'] = rowStyles.toolTipText;
                        summary[bodJsonPropName + 'Tooltip'] = rowStyles.toolTipText;


                    });
                });
            });
        }

        hideUnhideNotActive = () => {
            var vm = this;
            vm.hideNotActive = !vm.hideNotActive;
            vm.summaryData.forEach(summary => {
                summary.inActive = vm.hideNotActive;
            });
            vm.rootScope.$emit('onFundVisibleRowsChanged', vm.summaryData.filter(fundRow => {
                return (vm.selectedFund == fundRow && fundRow.inActive);
            }).length);
        }

        loadData = () => {
            var vm = this;
            vm.statusText = "Loading";
            vm.isLoading = true;
            vm.window.setTimeout(() => {
                vm.dataService.loadFundRestrictionsTypes().then(fundrestrictionTypes => {
                    vm.fundRestrictionTypes = fundrestrictionTypes;
                    vm.dataService.loadFundRestrictionFields().then(fundRestrictionFields => {
                        vm.fundRestrictionFields = fundRestrictionFields;
                        vm.dataService.loadFundRestrictions(null).then(fundrestrictions => {
                            vm.fundRestrictions = fundrestrictions;
                            if (!pageOptions.TestResults)
                                vm.dataService.loadSummaryData().then((summaries) => {
                                    pageOptions.TestResults = summaries;
                                    vm.setSummaryData(summaries);
                                });
                            else
                                vm.setSummaryData(pageOptions.TestResults);
                        });
                    });
                });
            });
        }

        loadTestResults = () => {
            var vm = this;
            vm.statusText = "Loading";
            vm.isLoading = true;
            vm.window.setTimeout(() => {

                if (!pageOptions.TestResultsNew)
                    vm.dataService.loadTestResults().then((testResultsData) => {
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
        }

        loadTrendTypes = () => {
            var vm = this;
            vm.statusText = "Loading";
            vm.isLoading = true;

            vm.dataService.loadTrendTypes().then(types => {
                vm.trendtypes = types;
                vm.trendType = vm.trendtypes[0];
                vm.isLoading = false;
            });
        }

        loadPeriod = () => {
            var vm = this;
            vm.statusText = "Loading";
            vm.isLoading = true;

            vm.dataService.loadPeriod().then(periods => {
                vm.trendPeriod = periods;
                
                vm.isLoading = false;
                var defaultPeriod = periods.filter(function (period) {
                    return period.isDefault === true;
                });
                vm.period = defaultPeriod[0];
                vm.loadTrends(true);
                vm.disableTrendDates();
            });
        }


        loadTrends = (isLoadCharts :any) => {
            var vm = this;
            vm.statusText = "Loading";
            vm.isLoading = true;
            var trendTypeId;
            //pass params 1- trendtype frdate todate
            //vm.window.setTimeout(() => {
            if (vm.trendType) trendTypeId = vm.trendType.typeID;
            else trendTypeId = 1; // Default
            var periodId = vm.period ? vm.period.periodId : 1;
            
            vm.dataService.loadTrends(vm.startDate.toLocaleDateString(), vm.endDate.toLocaleDateString(), trendTypeId, periodId).then((trendResultsData) => {
                pageOptions.TrendsResult = trendResultsData;
                vm.setTrendsData(trendResultsData, isLoadCharts);
                vm.setTrendTableParams();
            });
            vm.isLoading = false;
        }

        showCharts = () => {
            var vm = this;
            vm.uiService.showChartsPopup(vm.modalService, vm.trendsData, vm.trendPeriod, vm.trendtypes, vm.period, vm.trendType );
        }

        setTrendsData = (trendResultsData: Models.ITrends[],isLoadCharts:any) => {
            var vm = this;
            var redColor = { 'background-color': 'lightcoral', 'color': '#333333' };
            var greenColor = { 'background-color': 'lightgreen', 'color': '#333333'};

            trendResultsData.forEach((tData,idx) => {
                for (var i = 1; i < 12; i++) {
                    tData["fundOvercollateralization" + i] = (tData["fundOvercollateralization" + i] / 1000000).toFixed(1);
                    tData["fundOvercollateralization" + i + "Old"] = (tData["fundOvercollateralization" + i + "Old"] / 1000000).toFixed(1);
                    if (tData["fundOvercollateralization" + i] === '0.0') {
                        tData["fundOvercollateralization" + i] = '';
                        tData["fund" + i + "BgStyle"] = '';
                    } else {
                        if (trendResultsData[idx + 1]) {
                            var diffCollateralization = trendResultsData[idx]["fundOvercollateralization" + i] - (trendResultsData[idx + 1]["fundOvercollateralization" + i]/ 1000000);
                            tData["fund" + i + "BgStyle"] = diffCollateralization > 0.3 ? greenColor : diffCollateralization < -0.3 ? redColor : "";
                        }
                        
                    }
                 

                }
                
            });
            vm.trendsData = trendResultsData;
            if (isLoadCharts) {
                vm.loadCharts()
            }
        }

        setTestResultsData = (testResultsData: Models.ITestResults[]) => {
            var vm = this;

            testResultsData.forEach(function (tData) {

                var CLO = 1;
                var redColor = { 'background-color': 'RED', 'color': '#333333' };
                var yellowColor = { 'background-color': 'yellow', 'color': 'rgb(51, 51, 51)' };
                //var blackFontColor = { 'color': 'black' };
                var redFontColor = { 'color': 'red' }
                for (var clo = 1; clo < 12; clo++) {
                    if (tData["fund" + clo + "OutcomeDisplay"] && tData["fund" + clo + "OutcomeDisplay"].includes("$")) {//&& typeof tData["fund" + clo + "Outcome"] === "number"
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
                        } else if (tData.condition === "<=") {
                            breachCondition = ">";
                            dangerCondition = ">=";
                        }
                        var calcTooltip = tData["calculatedColumn" + clo].toString();
                        var breachToolTip = tData["fund" + clo + "Breach"].toString()
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
                        tData["fund" + clo + "OutcomeTooltip"] = tData["hideDanger"] ? breachDiv :  dangerDiv + breachDiv;
                        //console.log("<div>DANGER &nbsp;" + tData.condition + "&nbsp;" + tData["calculatedColumn" + clo].toString() + "</div><div>BREACH&nbsp;" + tData.condition + "&nbsp;" + tData["fund" + clo + "Breach"].toString() + "</div>");
                        if (tData["fund" + clo + "OutcomeDisplay"] !== "N/A") {
                            if (tData.condition === ">=") {
                                tData["fund" + clo + "OutcomeBgStyle"] = tData["fund" + clo + "Outcome"] < tData["fund" + clo + "Breach"] ? redColor :
                                    tData["fund" + clo + "Outcome"] <= tData["calculatedColumn" + clo] && tData["fund" + clo + "Outcome"] >= tData["fund" + clo + "Breach"] && !tData["hideDanger"]? yellowColor : {};
                            } else if (tData.condition === "<=") {
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
            Object.keys(dataWithGroups).forEach(key => {
                var groupStyle = {
                    "font-size": "small",
                    "font-weight": "bold"
                }
                groupByData.push({ 'testDisplayName': key, 'testDisplayNameBgStyle': groupStyle });
                dataWithGroups[key].forEach(tData => {
                    groupByData.push(tData)
                });
                groupByData.push({});
            });
            //vm.testResultsData = testResultsData;
            vm.testResultsData = groupByData;
            vm.selectTest(groupByData[1])
            //console.log(testResultsData);
        }

        setSummaryData = (summaries: Models.ISummary[]) => {
            var vm = this;

            vm.summaryData = summaries;
            vm.setBackgroundStyleBasedOnFundRestrictions();

            vm.window.setTimeout(() => {
                vm.isLoading = false;
                vm.setParamsTable();
                if (summaries.length) {
                    vm.selectFund(summaries[0]);
                }
            });
        }

        periodChangeEvent = () => {
            this.disableTrendDates ()
        }

        disableTrendDates = () => {
            var vm = this;
            if (vm.period.periodName === "Daily"){
                vm.isDateDisabled = false;
            }else {
                vm.isDateDisabled = true;
            }
        }

        getBackgroundColorStyle = (fieldName: string, summaryRow: Models.ISummary) => {
            var vm = this;
            var color: string = '';
            var bodColor: string = '';
            var fieldFundRestrictions = vm.fundRestrictions.filter(fr => {
                return fr.jsonPropertyName == fieldName;
            });
            var rowStyle: any = { toolTipText: '' };

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
                vm.fundRestrictionTypes.forEach(frt => {
                    var fundRestrictions = fieldFundRestrictions.filter(fieldFundRestriction => { return fieldFundRestriction.fundRestrictionTypeId == frt.fundRestrictionTypeId && fieldFundRestriction.fundId == summaryRow.fundId });
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
                rowStyle.topRow = { 'background-color': color, 'color': '#333333' }
            } else {
                rowStyle.topRow = null;
            }

            if (bodColor) {
                rowStyle.bodRow = { 'background-color': bodColor, 'color': '#333333' }
            } else {
                rowStyle.bodRow = null;
            }

            return rowStyle;
        }

        selectFund = (fundRow: Models.ISummary) => {
            var vm = this;
            if (!vm.selectedFund || (vm.selectedFund && vm.selectedFund.fundId !== fundRow.fundId)) {
                vm.rootScope.$emit('onFundSelectionChanged', fundRow);
            }
            if (vm.selectedFund != fundRow) {
                vm.selectedFund = fundRow;
                vm.rootScope['selectedFund'] = fundRow;
                vm.rootScope.$emit('onFundChanged', fundRow);
            }
        }

        selectTest = (testRow: Models.ITestResults) => {
            var vm = this;

            if (vm.selectedTest != testRow) {
                vm.selectedTest = testRow;
                vm.rootScope['selectedTest'] = testRow;

            }
        }

        selectTrend = (trend: any) => {
            var vm = this;

            if (vm.selectedTrend != trend) {
                vm.selectedTrend = trend;
                vm.rootScope['selectedTrend'] = trend;

            }
        }

        selectPar = (fundRow: Models.ISummary) => {
            //alert(fundRow.par);
        }

        onTestResultsVisibilityChanged(open: boolean): void {
            var vm = this;
            vm.rootScope.$emit('onTestResultsVisibilityChanged', open);
        }

        getAssetParRestrictions(fieldName: string, fundId: number): Models.IFundRestriction[] {
            var vm = this;
            var fieldFundRestrictions = vm.fundRestrictions.filter(fr => {
                return fr.fieldName == fieldName && fr.fundId == fundId;
            });
            return fieldFundRestrictions;
        }

        downloadLoanPositions = (fundId: number) => {
            var vm = this;
            var fieldFundRestrictions = vm.getAssetParRestrictions('AssetPar', fundId);
            //var fieldFundRestrictions = vm.fundRestrictions.filter(fr => {
            //	return fr.fieldName == 'AssetPar' && fr.fundId == fundId;
            //});
            var rest = fieldFundRestrictions[0];
            if (rest.isDifferenceOverThreshold)
                vm.dataService.downloadLoanPositions(fundId);
        }

        downloadReInvestCash = (Url: string) => {
            var vm = this;
            vm.dataService.downloadReInvestCash(Url);
        }

        setParamsTable = () => {
            var vm = this;
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
        }

        setTestTableParams = () => {
            var vm = this;
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
        }

        setTrendTableParams = () => {
            var vm = this;
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
        }


        loadCharts = () => {
            var vm = this;
            var trendsData = vm.trendsData.slice();// Slice is added to clone data of trendsdata.
            trendsData = trendsData.sort((a, b) => {
                return new Date(a.trendDate).getTime() - new Date(b.trendDate).getTime();
            });
            //Add Filter function to segrgate data
            trendsData.forEach(function (tData,idx) {
                let hasBlankData = false
                for (var i = 1; i < 11; i++) {
                    if (tData["fundOvercollateralization" + i] === '') {
                        hasBlankData = true;
                        break;
                    }
                }
                if (hasBlankData) {
                    trendsData[idx].hasBlankData = true;
                }
            })

            trendsData = trendsData.filter(function (tData) {
                return tData.hasBlankData !== true;
            })

            vm.timeoutService(function () {
                if (vm.trendsChart) {
                    vm.trendsChart.destroy();
                }
                var chartDiv = document.getElementById('trendsChart')
                var fundColors = ['', '#36a2eb', '#cc65fe', '#00ff80', '#7320BD', '#ff99ff', '#0000ff', '#009999', '#cc0000', '#003399', '#ff6384'];
                var chartDataSet = [];
                for (var i = 1; i < 11; i++) {
                    let hidden = true;
                    if (vm.selectedCLOs.length === 0) {
                        hidden = i === 1 ? false : true;
                    } else {
                        let activeClo =  vm.selectedCLOs.filter(function (clo) {
                            return  clo == 'CLO' + i
                        });
                        if (activeClo.length) {
                            hidden = false;
                        }
                    }
                  
                    var fundDataSet = {
                        label: 'CLO' + i,
                        data: trendsData.map(row => row["fundOvercollateralization" + i]),
                        borderColor: fundColors[i],
                        fill: false,
                        lineTension: 0.4,
                        hidden: hidden,
                        type: 'line'
                    }
                    if (!hidden) {
                        vm.selectedCLOs.push(fundDataSet.label)
                    }
                    chartDataSet.push(fundDataSet);
                }
                const newLegendClickHandler = function (e, legendItem, legend) {
                 
                    const index = legendItem.datasetIndex;
                    const ci = legend.chart;
                    if (ci.isDatasetVisible(index)) {
                        ci.data.datasets[index].hidden = true;
                        ci.hide(index);
                        vm.selectedCLOs = vm.selectedCLOs.filter(function (clo) {
                            return clo !== legendItem.text;
                        });
                        legendItem.hidden = true;
                    } else {
                        ci.data.datasets[index].hidden = false;
                        ci.show(index);
                        legendItem.hidden = false;
                        vm.selectedCLOs.push(legendItem.text);
                    }

                }




                vm.trendsChart = new vm.window.Chart(
                    chartDiv,
                    {

                        data: {
                            labels: trendsData.map(row => row.trendDate),
                            datasets: chartDataSet

                        },
                        options: {
                            showLines: true,
                            plugins: {
                                legend: {
                                    labels: {
                                        usePointStyle: true,
                                        generateLabels: (chart) => {
                                            //console.log(chart);
                                            let pointStyle = [];
                                            chart.data.datasets.forEach((dataset, index) => {
                                                if (dataset.hidden === true) {
                                                    pointStyle.push({ type: 'crossRot', color: ' #000000' })
                                                } else {
                                                    pointStyle.push({ type: 'circle', color: fundColors[index + 1] })
                                                }

                                            })
                                            return chart.data.datasets.map(
                                                (dataset, index) => ({
                                                    text: dataset.label,
                                                    fillStyle: pointStyle[index].color,
                                                    strokeStyle: pointStyle[index].color,
                                                    pointStyle: pointStyle[index].type,
                                                    hidden: false,
                                                    datasetIndex: index

                                                })
                                            )
                                        }
                                    },
                                    onClick: newLegendClickHandler
                                }
                            },
                            maintainAspectRatio: false

                        },


                    }
                );





            })

        }


        exportToPdf = () => {
            var vm = this;
            var active = $("ul.nav.nav-tabs.testResultsTab li.active a").attr('href')
            active = active.split('#')[1];
            var lodingDiv = document.getElementById('lodingDiv');
            lodingDiv.hidden = false;
            vm.window.setTimeout(() => {
                var dateid = vm.summaryData.length > 0 ? + vm.summaryData[0].dateId.toString().substr(4, 2) + '/' + vm.summaryData[0].dateId.toString().substr(6, 2) + '/' + vm.summaryData[0].dateId.toString().substr(0, 4) : ''
                //var pdf = new vm.window.jsPDF('l', 'pt', 'letter');
                var pdfSettings = {
                    startRow: 0,
                    rowsPerPage: 45,
                    downloadFileName: '',
                    imageHeight : 190
                }
                if (active === "testResult1") {
                    active = "tblSummaryBody";
                    pdfSettings.downloadFileName = 'Summary_';
                } else if (active === "testResult2") {
                    active = "tblTestResult2";
                    pdfSettings.downloadFileName = 'WSOCompliance_';
                } else if (active === "trends") {
                    active = "tbTrends";
                    pdfSettings.downloadFileName = 'Trends_';
                } else if (active === "trendCharts") {
                    pdfSettings.rowsPerPage = 0;
                    pdfSettings.downloadFileName = 'TrendCharts';
                    pdfSettings.imageHeight = 150;
                }

                vm.originalElement = document.getElementById(active);
                var sourceArray = [];

                if (vm.originalElement.tBodies && vm.originalElement.tBodies.length) {
                    vm.source = vm.originalElement.cloneNode(true);
                    let reminder = vm.source.tBodies.length % pdfSettings.rowsPerPage;
                    let quotient: number = vm.source.tBodies.length / pdfSettings.rowsPerPage;
                    let pages = 0;
                    pages = reminder > 0 ? Math.floor(quotient) + 1 : quotient;

                    var startRow = pdfSettings.startRow, endrow = pdfSettings.rowsPerPage;

                    for (var i = 0; i < pages; i++) {
                        vm.source = vm.originalElement.cloneNode(true);
                        vm.source.id = vm.source.id + i;
                        var bodLength = Object.keys(vm.source.tBodies).length;
                        for (var idx = bodLength - 1; idx >= 0; idx--) {
                            if (idx >= startRow && idx <= endrow) {
                                //console.log(idx);
                            } else {
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
                } else {
                    sourceArray.push(vm.originalElement)
                }
                var canvasImages = [];
                vm.imagePromises = [];

                for (var x = 0; x <= sourceArray.length - 1; x++) {
                    vm.imagePromises.push(vm.imagePromise(canvasImages, sourceArray[x], pdfSettings));
                }

                vm.window.Promise.all(vm.imagePromises).then(function (data) {

                    let pdf = new vm.window.jspdf.jsPDF('l');


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

        }

        imagePromise = (canvasImages, src, pdfSettings) => {
            var vm = this;
            console.log(src.id);
            var imageHeight = pdfSettings.imageHeight;//as a4 Page Size in mm is 209 mm , 15 mm is added as border
            if (src.tBodies && src.tBodies.length < pdfSettings.rowsPerPage) {
                var rowHeight = imageHeight / pdfSettings.rowsPerPage;
                imageHeight = rowHeight * src.tBodies.length;
            }
            var imageElement = document.getElementById(src.id);
            return new vm.window.Promise((resolve, reject) => {
               
                vm.window.html2canvas(imageElement, {
                    scale: 2,
                    allowTaint: true,
                    useCORS: true
                }).then(function (canvas) {
                    canvasImages.push({ image: canvas, imageHeight: imageHeight });
                    resolve(canvas);
                });
            })
        }

        exportToCSV = () => {
            var vm = this;
            var activeTab = "trendsData"
            var CsvData = [];
            var active = $("ul.nav.nav-tabs.testResultsTab li.active a").attr('href')
            var dateid = vm.summaryData.length > 0 ?  + vm.summaryData[0].dateId.toString().substr(4, 2) + '/' + vm.summaryData[0].dateId.toString().substr(6, 2) + '/' + vm.summaryData[0].dateId.toString().substr(0, 4)  : ''
            if (active === "#testResult1") {
                vm.summaryData.forEach(line => {
                    //let reportDate = new Date(line.tradeDate);
                    let csvLine = {
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
                        'B-%': line.bMinusToAssetParPct +( line.bMinusToAssetParPct != null && line.bMinusToAssetParPct.toString().length ? '%' : ''),
                        'WAL Cushion': line.walCushion,
                        'Time to Reinvest': line.timeToReinvest,
                        "Moody's Recovery": line.wsoMoodyRecovery,
                        'Diversity': line.wsoDiversity,
                        'WA Bid': line.bid,
                        'WAPP': line.wapp,
                        'BB MVOC': line.bbmvoc +( line.bbmvoc != null && line.bbmvoc.toString().length ? '%' : ''),
                        'Clean Nav': line.cleanNav
                    }
                    CsvData.push(csvLine);
                });
                vm.exportToCsv('Summary_' + dateid+'.csv', CsvData);
              
            } else if (active === "#testResult2") {
                vm.testResultsData.forEach(line => {
                    //let reportDate = new Date(line.tradeDate);
                    let csvLine = {
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
                    }
                    CsvData.push(csvLine);
                });
                vm.exportToCsv('Compliance_' + dateid +'.csv', CsvData);

            } else if (active === "#trends" || active==="#trendCharts") {
                vm.trendsData.forEach(line => {
                    //let reportDate = new Date(line.tradeDate);
                    let csvLine = {
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
                    }
                    CsvData.push(csvLine);
                });
                vm.exportToCsv('trendata_' + dateid +'.csv', CsvData);
            }
        }
        exportToCsv = (filename: string, rows: object[]) => {
            if (!rows || !rows.length) {
                return;
            }
            const separator = ',';
            const keys = Object.keys(rows[0]);
            const csvContent =
                keys.join(separator) +
                '\n' +
                rows.map(row => {
                    return keys.map(k => {
                        let cell = row[k] === null || row[k] === undefined ? '' : row[k];
                        cell = cell instanceof Date
                            ? cell.toLocaleString()
                            : cell.toString().replace(/"/g, '""');
                        if (cell.search(/("|,|\n)/g) >= 0) {
                            cell = `"${cell}"`;
                        }
                        return cell;
                    }).join(separator);
                }).join('\n');

            const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
            if (navigator.msSaveBlob) { // IE 10+
                navigator.msSaveBlob(blob, filename);
            } else {
                const link = document.createElement('a');
                if (link.download !== undefined) {
                    // Browsers that support HTML5 download attribute
                    const url = URL.createObjectURL(blob);
                    link.setAttribute('href', url);
                    link.setAttribute('download', filename);
                    link.style.visibility = 'hidden';
                    document.body.appendChild(link);
                    link.click();
                    document.body.removeChild(link);
                }
            }
        }
    }

    angular.module("app").controller("topNavController", TopNavController);
}