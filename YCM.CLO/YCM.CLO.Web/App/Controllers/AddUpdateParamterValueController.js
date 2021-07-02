var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var AddUpdateParameterValueController = (function () {
            function AddUpdateParameterValueController(dataService, $window, $scope, $modalInstance, sourcedata) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                this.isLoading = false;
                this.statusText = "Saving";
                this.cancel = function () {
                    var vm = _this;
                    vm.statusText = "Closing";
                    vm.isLoading = true;
                    vm.modalInstance.dismiss('cancel');
                };
                this.save = function () {
                    var vm = _this;
                    vm.isLoading = true;
                    vm.statusText = "Updating";
                    var parametrValue = JSON.parse(JSON.stringify(vm.parameterValue));
                    vm.parameterValue.parameterType = null;
                    vm.dataService.updateParameterValue(parametrValue).then(function (updatedParamterVal) {
                        vm.isLoading = false;
                        vm.modalInstance.close(updatedParamterVal);
                    });
                };
                var vm = this;
                this.parameterValue = sourcedata;
                this.modalInstance = $modalInstance;
                this.dataService = dataService;
                this.dataService.loadParameterTypes().then(function (paramTypes) { return vm.parameterTypes = paramTypes; });
            }
            return AddUpdateParameterValueController;
        }());
        AddUpdateParameterValueController.$inject = ["application.services.dataService", "$window", "$scope", "$modalInstance", 'sourcedata'];
        Controllers.AddUpdateParameterValueController = AddUpdateParameterValueController;
        angular.module("app").controller("application.controllers.addUpdateParameterValueController", AddUpdateParameterValueController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=AddUpdateParamterValueController.js.map