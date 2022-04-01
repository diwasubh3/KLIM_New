var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var TradeBookingController = (function () {
            function TradeBookingController(uiService, dataService, $rootScope, ngTableParams, $filter) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                this.statusText = "Loading";
                this.includeCancelled = false;
                this.loadDropdownData = function () {
                    var vm = _this;
                    vm.statusText = "Loading";
                    vm.isLoading = true;
                    vm.dataService.getTradeBookingData().then(function (tradedata) {
                        vm.sourceData = tradedata;
                        vm.isLoading = false;
                    });
                };
                this.GenerateTradeXML = function () {
                    var vm = _this;
                    vm.statusText = "Saving";
                    vm.isLoading = true;
                    vm.dataService.generateTradeXML(vm.sourceData).then(function (data) {
                        vm.isLoading = false;
                    });
                };
                this.searchIssuerSec = function (name) {
                    console.log('called');
                    var vm = _this;
                    vm.isLoading = true;
                    vm.dataService.getIssuerSecData(name).then(function (data) {
                        vm.issuerSec = data;
                        vm.isLoading = false;
                    });
                };
                this.getPositionFromTrade = function (trade) {
                    var security = trade['security'];
                    return security;
                };
                this.ShowHide = function (prop) {
                    var vm = _this;
                    vm[prop].toggle();
                };
                this.select = function (trade) {
                };
                this.getSortedCustomViews = function (views) {
                    var vm = _this;
                    var separatorName = "---------------";
                    views.forEach(function (x) { return x.isDisabled = false; });
                    var sorted = views.sort(function (a, b) {
                        return ((a.isPublic === b.isPublic) ? 0 : b.isPublic ? -1 : 1) || a.viewName.toLowerCase().localeCompare(b.viewName.toLowerCase());
                    });
                    vm.insertViewSeparator(separatorName, sorted);
                    vm.customViews = sorted;
                };
                this.insertViewSeparator = function (separatorName, views) {
                    var vm = _this;
                    var pubs = views.filter(function (x) { return x.isPublic; });
                    var privates = views.filter(function (x) { return !x.isPublic; });
                    if (pubs.length && privates.length) {
                        //var sorted = _.sortBy(views, 'isPublic');
                        var firstPubIndex = views.findIndex(function (x) { return x.isPublic; });
                        var separator = { viewName: separatorName, isDisabled: true, isDefault: false };
                        views.splice(firstPubIndex, 0, separator);
                    }
                };
                var vm = this;
                vm.dataService = dataService;
                vm.uiService = uiService;
                vm.rootScope = $rootScope;
                vm.rootScope.$emit('onActivated', 'tradebooking');
                vm.ngTableParams = ngTableParams;
                vm.filter = $filter;
                vm.isLoading = true;
                vm.loadDropdownData();
                vm.tempSecurity = {};
                vm.issuerSec = {};
            }
            return TradeBookingController;
        }());
        TradeBookingController.$inject = ["application.services.uiService", "application.services.dataService", "$rootScope", 'NgTableParams', '$filter'];
        Controllers.TradeBookingController = TradeBookingController;
        angular.module("app").controller("application.controllers.tradebookingController", TradeBookingController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=TradeBookingController.js.map