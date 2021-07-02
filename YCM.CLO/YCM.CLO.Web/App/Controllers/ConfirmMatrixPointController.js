var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var ConfirmMatrixPointController = (function () {
            function ConfirmMatrixPointController(dataService, $window, $scope, $modalInstance, sourcedata) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                this.isLoading = false;
                this.statusText = "Saving";
                this.cancel = function () {
                    var vm = _this;
                    vm.modalInstance.dismiss('cancel');
                };
                this.proceed = function () {
                    var vm = _this;
                    vm.modalInstance.close({ confirm: true });
                };
                this.fundCode = sourcedata.fundCode;
                this.matrixPoint = sourcedata.matrixPoint;
                this.modalInstance = $modalInstance;
                this.dataService = dataService;
            }
            return ConfirmMatrixPointController;
        }());
        ConfirmMatrixPointController.$inject = ["application.services.dataService", "$window", "$scope", "$modalInstance", 'sourcedata'];
        Controllers.ConfirmMatrixPointController = ConfirmMatrixPointController;
        angular.module("app").controller("application.controllers.confirmMatrixPointController", ConfirmMatrixPointController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=ConfirmMatrixPointController.js.map