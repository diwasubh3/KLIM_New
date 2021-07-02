var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var AdminController = (function () {
            function AdminController(uiService, dataService, $rootScope, modalService, ngTableParams, $filter, timeOutService) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                this.statusText = "Loading";
                this.process = function (url, status) {
                    var vm = _this;
                    vm.isLoading = true;
                    vm.statusText = status + "...";
                    vm.success = "";
                    vm.error = "";
                    vm.dataService.process(url).then(function (d) {
                        vm.isLoading = false;
                        vm.success = "Calculation - Finished";
                    }).catch(function (e) {
                        vm.isLoading = false;
                        vm.error = "Error occurred, please contact IT.";
                        console.log('Error: ', e);
                        throw e;
                    });
                };
                var vm = this;
                vm.dataService = dataService;
                vm.uiService = uiService;
                vm.rootScope = $rootScope;
                vm.rootScope.$emit('onActivated', 'admin');
                vm.modalService = modalService;
                vm.ngTableParams = ngTableParams;
                vm.filter = $filter;
                if (vm.rootScope['selectedFund']) {
                    vm.selectedFund = vm.rootScope['selectedFund'];
                }
                vm.rootScope.$on('onFundSelectionChanged', function (event, data) {
                    vm.selectedFund = data;
                });
                vm.rootScope.$on('onAutoRefresh', function (event, data) {
                    vm.selectedFund = data;
                });
            }
            return AdminController;
        }());
        AdminController.$inject = ["application.services.uiService", "application.services.dataService", "$rootScope", '$modal', 'NgTableParams', '$filter', '$timeout'];
        Controllers.AdminController = AdminController;
        angular.module("app").controller("application.controllers.adminController", AdminController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=AdminController.js.map