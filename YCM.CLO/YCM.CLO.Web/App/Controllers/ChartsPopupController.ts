
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
        
        


        static $inject = ["application.services.dataService", "$window", "$scope", "$uibModalInstance", 'NgTableParams', '$timeout', 'sourcedata'];

        constructor(dataService: Application.Services.Contracts.IDataService, $window: ng.IWindowService, $scope: angular.IScope, $modalInstance: angular.ui.bootstrap.IModalServiceInstance, ngTableParams: NgTableParams, $timeout: ng.ITimeoutService, sourcedata:any) {
            var vm = this;
            vm.windowService = $window;
            vm.timeoutService = $timeout;
            vm.modalInstance = $modalInstance;
            vm.sourcedata = sourcedata;
            vm.loadChartData();
        }

        loadChartData = () => {
            var vm = this;
            var trendsData = vm.sourcedata.trendsData;
            trendsData = trendsData.sort((a, b) => {
                return new Date(a.trendDate).getTime() - new Date(b.trendDate).getTime();
            });

            vm.timeoutService(function () {
                var check = document.getElementById('acquisitions');
                var fundColors = ['', '#36a2eb', '#cc65fe', '#00ff80', '#66ff33', '#ff99ff', '#0000ff', '#009999', '#cc0000', '#003399','#ff6384'];
                var chartDataSet = [];
                for (var i = 1; i < 11; i++) {
                    var fundDataSet = {
                        label: 'CLO' + i,
                        data: trendsData.map(row => row["fundOvercollateralization" + i]),
                        borderColor: fundColors[i],
                        fill: false,
                        tensions: 0.1
                    }
                    chartDataSet.push(fundDataSet);
                }
              
                var trendsChart = new vm.windowService.Chart(
                    document.getElementById('trendsChart'),
                    {
                        type: 'line',
                        data: {
                            labels: trendsData.map(row => row.trendDate),
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
                      
                    }
                );

            })
           
        }


        cancel = () => {
            var vm = this;
            vm.statusText ="Closing"
            vm.modalInstance.dismiss('cancel');
        }
       
    }

    angular.module("app").controller("application.controllers.chartsPopupController", ChartsPopupController);
}
