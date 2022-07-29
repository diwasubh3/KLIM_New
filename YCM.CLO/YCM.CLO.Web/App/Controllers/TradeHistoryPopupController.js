var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var TradeHistoryPopupController = (function () {
            function TradeHistoryPopupController(dataService, $window, $scope, $modalInstance, ngTableParams, sourcedata) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                this.isLoading = false;
                this.statusText = "Loading";
                this.loadTradeHistoryData = function () {
                    var vm = _this;
                    vm.statusText = "Loading";
                    vm.isLoading = true;
                    vm.dataService.getTradeHistory(vm.securitycode).then(function (d) {
                        vm.tradeHistoryDetails = d;
                        vm.isLoading = false;
                        //vm.weightedAveragePrice = 0.0;
                        vm.weightedAveragePurchasePrice = 0.0, vm.weightedAverageSellPrice = 0.0;
                        var totalPrice = 0.0, totalQuantity = 0.0;
                        var totalBuy = 0.0, totalSell = 0.0, totalBuyPrice = 0.0, totalSellPrice = 0.0;
                        if (vm.tradeHistoryDetails.filter(function (f) { return f.tradeType.toLowerCase() == "buy"; }).length > 0) {
                            totalBuy = vm.tradeHistoryDetails.filter(function (f) { return f.tradeType.toLowerCase() == "buy"; }).map(function (item) { return parseFloat(item.quantity); }).reduce(function (prev, curr) { return prev + curr; }, 0);
                            totalBuyPrice = vm.tradeHistoryDetails.filter(function (f) { return f.tradeType.toLowerCase() == "buy"; }).reduce(function (prev, curr) { return prev + (parseFloat(curr.quantity) * parseFloat(curr.price)); }, 0);
                            vm.weightedAveragePurchasePrice = totalBuyPrice / totalBuy;
                        }
                        if (vm.tradeHistoryDetails.filter(function (f) { return f.tradeType.toLowerCase() == "sell"; }).length > 0) {
                            totalSell = vm.tradeHistoryDetails.filter(function (f) { return f.tradeType.toLowerCase() == "sell"; }).map(function (item) { return parseFloat(item.quantity); }).reduce(function (prev, curr) { return prev + curr; }, 0);
                            totalSellPrice = vm.tradeHistoryDetails.filter(function (f) { return f.tradeType.toLowerCase() == "sell"; }).reduce(function (prev, curr) { return prev + (parseFloat(curr.quantity) * parseFloat(curr.price)); }, 0);
                            vm.weightedAverageSellPrice = totalSellPrice / totalSell;
                        }
                        //for (var i = 0; i < vm.tradeHistoryDetails.length; i++) {
                        //    var trade = vm.tradeHistoryDetails[i];
                        //    totalPrice += (parseFloat(trade.quantity) * parseFloat(trade.price));
                        //    totalQuantity += parseFloat(trade.quantity);
                        //}
                        //vm.weightedAveragePrice = (totalPrice / totalQuantity);
                    });
                };
                this.cancel = function () {
                    var vm = _this;
                    vm.statusText = "Closing";
                    vm.modalInstance.dismiss('cancel');
                };
                this.exportExcel = function (tableId, sheetName) {
                    var table = document.getElementById(tableId);
                    var rows = table.rows;
                    var data = "";
                    for (var row = 0; row < rows.length; row++) {
                        for (var column = 0; column < rows[row].cells.length; column++) {
                            data += rows[row].cells[column].innerHTML.trim().replace(/,/g, '') + ",";
                        }
                        data += "\n";
                    }
                    if (navigator.msSaveBlob) {
                        navigator.msSaveBlob(new Blob([data], { type: 'text/csv;charset=utf-8;' }), sheetName + ".csv");
                    }
                    else {
                        var a = document.createElement("a");
                        a.href = 'data:attachment/csv;charset=utf-8,' + encodeURI(data);
                        a.target = '_blank';
                        a.download = sheetName + '.csv';
                        document.body.appendChild(a);
                        a.click();
                    }
                };
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
            return TradeHistoryPopupController;
        }());
        TradeHistoryPopupController.$inject = ["application.services.dataService", "$window", "$scope", "$uibModalInstance", 'NgTableParams', 'sourcedata'];
        Controllers.TradeHistoryPopupController = TradeHistoryPopupController;
        angular.module("app").controller("application.controllers.tradeHistoryPopupController", TradeHistoryPopupController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=TradeHistoryPopupController.js.map