var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var ConfirmSecurityReconController = (function () {
            function ConfirmSecurityReconController(dataService, $window, $scope, $modalInstance, ngTableParams, sourcedata) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                this.isLoading = false;
                this.statusText = "Loading";
                this.confirmDelete = false;
                this.getStyle = function (column) {
                    var style = {};
                    if (column.displayWidth) {
                        style.width = column.displayWidth;
                    }
                    if (column.jsonPropertyName == 'issuer') {
                        style['width'] = 300;
                    }
                    if (column.jsonPropertyName == 'facility') {
                        style.width = null;
                    }
                    return style;
                };
                this.cancel = function () {
                    var vm = _this;
                    vm.statusText = "Closing";
                    vm.modalInstance.dismiss('cancel');
                };
                this.save = function () {
                    var vm = _this;
                    vm.confirmDelete = false;
                    vm.isLoading = true;
                    vm.statusText = "Matching";
                    vm.dataService.reconcileSecurities(vm.securities).then(function (result) {
                        vm.isLoading = false;
                        vm.modalInstance.close(result);
                    });
                };
                var vm = this;
                vm.modalInstance = $modalInstance;
                vm.dataService = dataService;
                vm.scope = $scope;
                vm.securities = sourcedata.securities;
                vm.fields = sourcedata.fields;
            }
            return ConfirmSecurityReconController;
        }());
        ConfirmSecurityReconController.$inject = ["application.services.dataService", "$window", "$scope", "$modalInstance", 'NgTableParams', 'sourcedata'];
        Controllers.ConfirmSecurityReconController = ConfirmSecurityReconController;
        angular.module("app").controller("application.controllers.confirmSecurityReconController", ConfirmSecurityReconController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=ConfirmSecurityReconController.js.map