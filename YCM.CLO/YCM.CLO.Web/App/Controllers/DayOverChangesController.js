var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var DayOverChangesController = (function () {
            function DayOverChangesController(uiService, dataService, $rootScope, $scope, uiGridConstants, exportUiGridService) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                //ngTableParams: any;
                this.statusText = "Loading";
                this.gridHeight = { 'height': '402px' };
                this.loadRatingChanges = function () {
                    var vm = _this;
                    vm.statusText = "Loading";
                    vm.isLoading = true;
                    vm.dataService.getRatingchanges().then(function (ratechange) {
                        vm.ratingchanges = ratechange;
                        vm.isLoading = false;
                    });
                };
                this.loadTotalParChanges = function () {
                    var vm = _this;
                    vm.statusText = "Loading";
                    vm.isLoading = true;
                    vm.dataService.getTotalParChanges().then(function (totalpar) {
                        vm.totalparchanges = totalpar;
                        vm.isLoading = false;
                    });
                };
                this.loadMoodyRecoveryDateChanges = function () {
                    var vm = _this;
                    vm.statusText = "Loading";
                    vm.isLoading = true;
                    vm.dataService.getMoodyRecoveryDateChanges().then(function (moody) {
                        vm.moodyrecoverychanges = moody;
                        vm.isLoading = false;
                    });
                };
                /*count <= 15 ? count : 15;*/
                this.loadTopBottonPriceMovers = function () {
                    var vm = _this;
                    vm.statusText = "Loading";
                    vm.isLoading = true;
                    vm.dataService.getTopBottomPriceMoversChanges().then(function (pm) {
                        vm.topBottonPriceMovers = pm;
                        vm.prevDayTopCount = vm.topBottonPriceMovers.prevDayTop.length <= 15 ? vm.topBottonPriceMovers.prevDayTop.length : 15;
                        vm.prevDayBottomCount = vm.topBottonPriceMovers.prevDayBottom.length <= 15 ? vm.topBottonPriceMovers.prevDayBottom.length : 15;
                        vm.prev5DayTopCount = vm.topBottonPriceMovers.prev5DayTop.length <= 15 ? vm.topBottonPriceMovers.prev5DayTop.length : 15;
                        vm.prev5DayBottomCount = vm.topBottonPriceMovers.prev5DayBottom.length <= 15 ? vm.topBottonPriceMovers.prev5DayBottom.length : 15;
                        vm.isLoading = false;
                    });
                };
                var vm = this;
                vm.dataService = dataService;
                vm.uiService = uiService;
                vm.rootScope = $rootScope;
                vm.rootScope.$emit('onActivated', 'dayoverchanges');
                vm.scope = $scope;
                vm.errorMessage = "";
                vm.loadRatingChanges();
            }
            return DayOverChangesController;
        }());
        DayOverChangesController.$inject = ["application.services.uiService", "application.services.dataService", "$rootScope", 'NgTableParams', '$filter', "$scope", 'uiGridConstants', 'uiGridExporterService'];
        Controllers.DayOverChangesController = DayOverChangesController;
        angular.module('app').controller("application.controllers.DayOverChangesController", DayOverChangesController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=DayOverChangesController.js.map