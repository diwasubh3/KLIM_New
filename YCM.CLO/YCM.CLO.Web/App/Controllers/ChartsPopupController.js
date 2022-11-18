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
                this.loadChartData = function () {
                    var vm = _this;
                    var trendsData = vm.sourcedata.trendsData;
                    trendsData = trendsData.sort(function (a, b) {
                        return new Date(a.trendDate).getTime() - new Date(b.trendDate).getTime();
                    });
                    vm.timeoutService(function () {
                        var check = document.getElementById('acquisitions');
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
                        var trendsChart = new vm.windowService.Chart(document.getElementById('trendsChart'), {
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
                this.cancel = function () {
                    var vm = _this;
                    vm.statusText = "Closing";
                    vm.modalInstance.dismiss('cancel');
                };
                var vm = this;
                vm.windowService = $window;
                vm.timeoutService = $timeout;
                vm.modalInstance = $modalInstance;
                vm.sourcedata = sourcedata;
                vm.loadChartData();
            }
            return ChartsPopupController;
        }());
        ChartsPopupController.$inject = ["application.services.dataService", "$window", "$scope", "$uibModalInstance", 'NgTableParams', '$timeout', 'sourcedata'];
        Controllers.ChartsPopupController = ChartsPopupController;
        angular.module("app").controller("application.controllers.chartsPopupController", ChartsPopupController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=ChartsPopupController.js.map