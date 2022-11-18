
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
       
        static $inject = ["$uibModal","application.services.uiService", "application.services.dataService", "$rootScope", 'NgTableParams', '$filter', '$window', '$interval'];
        constructor(modalService: angular.ui.bootstrap.IModalService,uiService: Application.Services.Contracts.IUIService, dataService: Application.Services.Contracts.IDataService, $rootScope: ng.IRootScopeService, ngTableParams: NgTableParams, $filter: ng.IFilterService, $window: ng.IWindowService, $interval: ng.IIntervalService) {
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
            
            const yesterday = new Date(new Date())
            yesterday.setDate(yesterday.getDate() - 1);
            const lastMonthdate = new Date(new Date())
            lastMonthdate.setDate(lastMonthdate.getDate() - 45);
            vm.startDate = lastMonthdate;
            vm.endDate = yesterday;

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
                vm.loadTrends();
                vm.disableTrendDates();
            });
        }


        loadTrends = () => {
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
        }

        showCharts = () => {
            var vm = this;
            vm.uiService.showChartsPopup(vm.modalService, vm.trendsData);
        }

        setTrendsData = (trendResultsData: Models.ITrends[]) => {
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
        }

        setTestResultsData = (testResultsData: Models.ITestResults[]) => {
            var vm = this;

            testResultsData.forEach(function (tData) {

                var clo = 1;
                var redColor = { 'background-color': 'RED', 'color': '#333333' };
                var yellowColor = { 'background-color': 'yellow', 'color': 'rgb(51, 51, 51)' };
                //var blackFontColor = { 'color': 'black' };
                var redFontColor = { 'color': 'red' }
                for (var clo = 1; clo < 12; clo++) {
                    if (tData["fund" + clo + "OutcomeDisplay"] && tData["fund" + clo + "OutcomeDisplay"].includes("$")) {//&& typeof tData["fund" + clo + "Outcome"] === "number"
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
                        tData["fund" + clo + "OutcomeTooltip"] = "<div>DANGER &nbsp;" + dangerCondition + "&nbsp;" + calcTooltip + "</div><div>BREACH&nbsp;" + breachCondition + "&nbsp;" + breachToolTip + "</div>";
                        //console.log("<div>DANGER &nbsp;" + tData.condition + "&nbsp;" + tData["calculatedColumn" + clo].toString() + "</div><div>BREACH&nbsp;" + tData.condition + "&nbsp;" + tData["fund" + clo + "Breach"].toString() + "</div>");
                        if (tData["fund" + clo + "OutcomeDisplay"] !== "N/A") {
                            if (tData.condition === ">=") {
                                tData["fund" + clo + "OutcomeBgStyle"] = tData["fund" + clo + "Outcome"] < tData["fund" + clo + "Breach"] ? redColor :
                                    tData["fund" + clo + "Outcome"] <= tData["calculatedColumn" + clo] && tData["fund" + clo + "Outcome"] >= tData["fund" + clo + "Breach"] ? yellowColor : {};
                            } else if (tData.condition === "<=") {
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
    }

    angular.module("app").controller("topNavController", TopNavController);
}