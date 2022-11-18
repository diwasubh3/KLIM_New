var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var ChartsPopupController = (function () {
            function ChartsPopupController(dataService, $window, $scope, $modalInstance, ngTableParams, $timeout, sourcedata) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                this.isLoading = false;
                this.statusText = "Loading";
                this.isDateDisabled = false;
                this.loadCharts = function () {
                    var vm = _this;
                    var trendsData = vm.trendsData;
                    trendsData = trendsData.sort(function (a, b) {
                        return new Date(a.trendDate).getTime() - new Date(b.trendDate).getTime();
                    });
                    vm.timeoutService(function () {
                        if (vm.trendsChart) {
                            vm.trendsChart.destroy();
                        }
                        var chartDiv = document.getElementById('trendsChart');
                        var fundColors = ['', '#36a2eb', '#cc65fe', '#00ff80', '#66ff33', '#ff99ff', '#0000ff', '#009999', '#cc0000', '#003399', '#ff6384'];
                        var chartDataSet = [];
                        for (var i = 1; i < 11; i++) {
                            var fundDataSet = {
                                label: 'CLO' + i,
                                data: trendsData.map(function (row) { return row["fundOvercollateralization" + i]; }),
                                borderColor: fundColors[i],
                                fill: false,
                                tensions: 0.1
                            };
                            chartDataSet.push(fundDataSet);
                        }
                        vm.trendsChart = new vm.windowService.Chart(chartDiv, {
                            type: 'line',
                            data: {
                                labels: trendsData.map(function (row) { return row.trendDate; }),
                                datasets: chartDataSet
                            },
                            options: {
                                showLines: true,
                                legend: {
                                    display: true,
                                    labels: {
                                        fontColor: 'rgb(255, 99, 132)'
                                    }
                                }
                            }
                        });
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
                this.cancel = function () {
                    var vm = _this;
                    vm.statusText = "Closing";
                    vm.modalInstance.dismiss('cancel');
                };
                this.loadTrends = function () {
                    var vm = _this;
                    vm.statusText = "Loading";
                    vm.isLoading = true;
                    var trendTypeId;
                    if (vm.trendType)
                        trendTypeId = vm.trendType.typeID;
                    else
                        trendTypeId = 1; // Default
                    var periodId = vm.period ? vm.period.periodId : 1;
                    vm.dataService.loadTrends(vm.startDate.toLocaleDateString(), vm.endDate.toLocaleDateString(), trendTypeId, periodId).then(function (trendResultsData) {
                        pageOptions.TrendsResult = trendResultsData;
                        vm.setTrendsData(trendResultsData);
                    });
                    vm.isLoading = false;
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
                    vm.loadCharts();
                };
                var vm = this;
                vm.windowService = $window;
                vm.timeoutService = $timeout;
                vm.dataService = dataService;
                vm.modalInstance = $modalInstance;
                vm.sourcedata = sourcedata;
                vm.trendsData = vm.sourcedata.trendsData;
                vm.trendtypes = vm.sourcedata.trendtypes;
                vm.trendType = vm.sourcedata.trendType;
                vm.trendPeriod = vm.sourcedata.trendPeriod;
                vm.period = vm.sourcedata.period;
                var yesterday = new Date(new Date());
                yesterday.setDate(yesterday.getDate() - 1);
                var lastMonthdate = new Date(new Date());
                lastMonthdate.setDate(lastMonthdate.getDate() - 45);
                vm.startDate = lastMonthdate;
                vm.endDate = yesterday;
                vm.loadCharts();
                vm.disableTrendDates();
            }
            return ChartsPopupController;
        }());
        ChartsPopupController.$inject = ["application.services.dataService", "$window", "$scope", "$uibModalInstance", 'NgTableParams', '$timeout', 'sourcedata'];
        Controllers.ChartsPopupController = ChartsPopupController;
        angular.module("app").controller("application.controllers.chartsPopupController", ChartsPopupController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=ChartsPopupController.js.map