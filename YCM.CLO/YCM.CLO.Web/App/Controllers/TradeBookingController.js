var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var TradeBookingController = (function () {
            function TradeBookingController(uiService, dataService, $rootScope, modalService, ngTableParams, $filter, timeOutService) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                this.statusText = "Loading";
                this.includeCancelled = false;
                this.loadDropdownData = function () {
                    var vm = _this;
                    vm.statusText = "Loading";
                    vm.isLoading = true;
                    vm.dataService.getTradeBookingSourceData().then(function (d) {
                        vm.sourceData = d;
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
                vm.modalService = modalService;
                vm.ngTableParams = ngTableParams;
                vm.filter = $filter;
                vm.isLoading = true;
                vm.loadDropdownData();
            }
            return TradeBookingController;
        }());
        TradeBookingController.$inject = ["application.services.uiService", "application.services.dataService", "$rootScope", '$uibModal', 'NgTableParams', '$filter', '$timeout'];
        Controllers.TradeBookingController = TradeBookingController;
        angular.module("app").controller("application.controllers.TradeBookingController", TradeBookingController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=TradeBookingController.js.map