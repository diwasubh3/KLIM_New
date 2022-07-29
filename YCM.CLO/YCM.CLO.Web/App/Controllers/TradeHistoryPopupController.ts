
module Application.Controllers {
    export class TradeHistoryPopupController {
        modalInstance: angular.ui.bootstrap.IModalServiceInstance;
        appBasePath: string = pageOptions.appBasePath;
        isLoading: boolean = false;
        statusText: string = "Loading";
        comments: string;
        sourcedata: any;
        tradeHistoryDetails: Array<Models.ITradeHistory>;
        dataService: Application.Services.Contracts.IDataService;
        ngTableParams: any;
        tableParams: any;
        lastUpdatedOn: Date;
        scope: ng.IScope;
        securitycode: string;
        issuer: string;
        weightedAveragePrice: number;
        weightedAveragePurchasePrice: number;
        weightedAverageSellPrice: number;
        windowService: ng.IWindowService;

        static $inject = ["application.services.dataService", "$window", "$scope", "$uibModalInstance", 'NgTableParams', 'sourcedata'];

        constructor(dataService: Application.Services.Contracts.IDataService, $window: ng.IWindowService, $scope: angular.IScope, $modalInstance: angular.ui.bootstrap.IModalServiceInstance, ngTableParams: NgTableParams, sourcedata: any) {
            var vm = this;
            vm.modalInstance = $modalInstance;
            vm.dataService = dataService;
            vm.scope = $scope;
            vm.windowService = $window;
            vm.sourcedata = sourcedata;
            vm.ngTableParams = ngTableParams;
            if (sourcedata.securitycode !== undefined) {
                vm.securitycode = sourcedata.securitycode;
                vm.issuer = sourcedata.issuer;
                vm.loadTradeHistoryData();
            }
            vm.lastUpdatedOn = new Date();
        }

        loadTradeHistoryData = () => {
            var vm = this;
            vm.statusText = "Loading";
            vm.isLoading = true;
            vm.dataService.getTradeHistory(vm.securitycode).then((d) => {
                vm.tradeHistoryDetails = d;
                vm.isLoading = false;
                //vm.weightedAveragePrice = 0.0;
                
                vm.weightedAveragePurchasePrice = 0.0, vm.weightedAverageSellPrice = 0.0;

                var totalPrice = 0.0, totalQuantity = 0.0;
                var totalBuy = 0.0, totalSell = 0.0, totalBuyPrice = 0.0, totalSellPrice = 0.0;
                if (vm.tradeHistoryDetails.filter(f => f.tradeType.toLowerCase() == "buy").length > 0) {
                    totalBuy = vm.tradeHistoryDetails.filter(f => f.tradeType.toLowerCase() == "buy").map(item => parseFloat(item.quantity)).reduce((prev, curr) => prev + curr, 0);
                    totalBuyPrice = vm.tradeHistoryDetails.filter(f => f.tradeType.toLowerCase() == "buy").reduce((prev, curr) => { return prev + (parseFloat(curr.quantity) * parseFloat(curr.price)) }, 0);

                    vm.weightedAveragePurchasePrice = totalBuyPrice / totalBuy;
                }

                if (vm.tradeHistoryDetails.filter(f => f.tradeType.toLowerCase() == "sell").length > 0) {
                    totalSell = vm.tradeHistoryDetails.filter(f => f.tradeType.toLowerCase() == "sell").map(item => parseFloat(item.quantity)).reduce((prev, curr) => prev + curr, 0);
                    totalSellPrice = vm.tradeHistoryDetails.filter(f => f.tradeType.toLowerCase() == "sell").reduce((prev, curr) => { return prev + (parseFloat(curr.quantity) * parseFloat(curr.price)) }, 0);

                    vm.weightedAverageSellPrice = totalSellPrice / totalSell;
                }

                //for (var i = 0; i < vm.tradeHistoryDetails.length; i++) {

                //    var trade = vm.tradeHistoryDetails[i];
                //    totalPrice += (parseFloat(trade.quantity) * parseFloat(trade.price));
                //    totalQuantity += parseFloat(trade.quantity);
                //}
                //vm.weightedAveragePrice = (totalPrice / totalQuantity);

            });
        }

        cancel = () => {
            var vm = this;
            vm.statusText = "Closing";
            vm.modalInstance.dismiss('cancel');
        }

        exportExcel = (tableId:string,sheetName:string) => {
            var table = document.getElementById(tableId) as HTMLTableElement;
            var rows = table.rows;
            var data = "";
            for (var row = 0; row < rows.length; row++) {
                for (var column = 0; column < rows[row].cells.length; column++) {
                    data += rows[row].cells[column].innerHTML.trim().replace(/,/g,'') + ",";
                }
                data += "\n";
            }
            if (navigator.msSaveBlob) {
                navigator.msSaveBlob(new Blob([data], { type: 'text/csv;charset=utf-8;' }), sheetName+".csv");
            } else {
                var a = document.createElement("a");
                a.href = 'data:attachment/csv;charset=utf-8,' + encodeURI(data);
                a.target = '_blank';
                a.download = sheetName +'.csv';
                document.body.appendChild(a);
                a.click();
            }
        };
    }

    angular.module("app").controller("application.controllers.tradeHistoryPopupController", TradeHistoryPopupController);
}
