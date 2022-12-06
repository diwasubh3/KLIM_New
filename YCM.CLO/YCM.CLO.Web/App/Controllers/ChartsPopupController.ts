
module Application.Controllers {

    
    export class ChartsPopupController {
        modalInstance: angular.ui.bootstrap.IModalServiceInstance;
        appBasePath: string = pageOptions.appBasePath;
        isLoading: boolean = false;
        statusText: string = "Loading";
        sourcedata: any;
        tradeHistoryDetails: Array<Models.ITradeHistory>;
        dataService: Application.Services.Contracts.IDataService;
        scope: ng.IScope;
        windowService: ng.IWindowService;
        timeoutService: ng.ITimeoutService;
        trendsData: any;
        trendPeriod: any;
        trendtypes: any;
        period: any;
        trendType: any;
        startDate: any;
        endDate: any;
        isDateDisabled = false;
        trendsChart: any;
   
       

        static $inject = ["application.services.dataService", "$window", "$scope", "$uibModalInstance", 'NgTableParams', '$timeout', 'sourcedata'];

        constructor(dataService: Application.Services.Contracts.IDataService, $window: ng.IWindowService, $scope: angular.IScope, $modalInstance: angular.ui.bootstrap.IModalServiceInstance, ngTableParams: NgTableParams, $timeout: ng.ITimeoutService, sourcedata: any) {
            var vm = this;
            vm.windowService = $window;
            vm.timeoutService = $timeout;
            vm.dataService = dataService;
            vm.modalInstance = $modalInstance;
            vm.sourcedata = sourcedata;
            vm.trendsData = vm.sourcedata.trendsData
            vm.trendtypes = vm.sourcedata.trendtypes;
            vm.trendType = vm.sourcedata.trendType;
            vm.trendPeriod = vm.sourcedata.trendPeriod;
            vm.period = vm.sourcedata.period;
            const yesterday = new Date(new Date())
            yesterday.setDate(yesterday.getDate() - 1);
            const lastMonthdate = new Date(new Date())
            lastMonthdate.setDate(lastMonthdate.getDate() - 45);
            vm.startDate = lastMonthdate;
            vm.endDate = yesterday;

            vm.loadCharts();
            vm.disableTrendDates();
        }

      

        loadCharts = () => {
            var vm = this;
            var trendsData = vm.trendsData;
            trendsData = trendsData.sort((a, b) => {
                return new Date(a.trendDate).getTime() - new Date(b.trendDate).getTime();
            });

            vm.timeoutService(function () {
                if (vm.trendsChart) {
                    vm.trendsChart.destroy();
                }
                var chartDiv = document.getElementById('trendsChart')
                var fundColors = ['', '#36a2eb', '#cc65fe', '#00ff80', '#66ff33', '#ff99ff', '#0000ff', '#009999', '#cc0000', '#003399','#ff6384'];
                var chartDataSet = [];
                for (var i = 1; i < 11; i++) {
                    var fundDataSet = {
                        label: 'CLO' + i,
                        data: trendsData.map(row => row["fundOvercollateralization" + i]),
                        borderColor: fundColors[i],
                        fill: false,
                        lineTension: 0.4,
                        hidden: i === 1 ? false : true,
                        type: 'line'
                    }
                    chartDataSet.push(fundDataSet);
                }
                const newLegendClickHandler = function (e, legendItem, legend) {
                    var check = this;
                    const index = legendItem.datasetIndex;
                    const ci = legend.chart;
                    if (ci.isDatasetVisible(index)) {
                        ci.data.datasets[index].hidden = true;
                        ci.hide(index);
                        legendItem.hidden = true;
                    } else {
                        ci.data.datasets[index].hidden = false;
                        ci.show(index);
                        legendItem.hidden = false;
                    }

                }.bind(this)

                

                
                vm.trendsChart = new vm.windowService.Chart(
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
                                            chart.data.datasets.forEach((dataset,index) => {
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
                            }
                         
                        },
                 
                      
                    }
                );

                



            })
           
        }

        periodChangeEvent = () => {
            this.disableTrendDates()
        }

        disableTrendDates = () => {
            var vm = this;
            if (vm.period.periodName === "Daily") {
                vm.isDateDisabled = false;
            } else {
                vm.isDateDisabled = true;
            }
        }
        cancel = () => {
            var vm = this;
            vm.statusText ="Closing"
            vm.modalInstance.dismiss('cancel');
        }
        loadTrends = () => {
            var vm = this;
            vm.statusText = "Loading";
            vm.isLoading = true;
            var trendTypeId;
            if (vm.trendType) trendTypeId = vm.trendType.typeID;
            else trendTypeId = 1; // Default
            var periodId = vm.period ? vm.period.periodId : 1;

            vm.dataService.loadTrends(vm.startDate.toLocaleDateString(), vm.endDate.toLocaleDateString(), trendTypeId, periodId).then((trendResultsData) => {
                pageOptions.TrendsResult = trendResultsData;
                vm.setTrendsData(trendResultsData);
            });
            vm.isLoading = false;
        }

        setTrendsData = (trendResultsData: Models.ITrends[]) => {
            var vm = this;
            var redColor = { 'background-color': 'lightcoral', 'color': '#333333' };
            var greenColor = { 'background-color': 'lightgreen', 'color': '#333333' };

            trendResultsData.forEach((tData, idx) => {
                for (var i = 1; i < 12; i++) {
                    tData["fundOvercollateralization" + i] = (tData["fundOvercollateralization" + i] / 1000000).toFixed(1);
                    tData["fundOvercollateralization" + i + "Old"] = (tData["fundOvercollateralization" + i + "Old"] / 1000000).toFixed(1);
                    if (tData["fundOvercollateralization" + i] === '0.0') {
                        tData["fundOvercollateralization" + i] = '';
                        tData["fund" + i + "BgStyle"] = '';
                    } else {
                        if (trendResultsData[idx + 1]) {
                            var diffCollateralization = trendResultsData[idx]["fundOvercollateralization" + i] - (trendResultsData[idx + 1]["fundOvercollateralization" + i] / 1000000);
                            tData["fund" + i + "BgStyle"] = diffCollateralization > 0.3 ? greenColor : diffCollateralization < -0.3 ? redColor : "";
                        }

                    }


                }

            });
            vm.trendsData = trendResultsData;
            vm.loadCharts();
        }
       
    }

    angular.module("app").controller("application.controllers.chartsPopupController", ChartsPopupController);
}
