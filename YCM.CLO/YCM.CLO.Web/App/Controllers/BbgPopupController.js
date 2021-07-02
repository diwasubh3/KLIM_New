var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var BbgPopupController = (function () {
            function BbgPopupController(dataService, $scope, $modalInstance, sourcedata) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                this.isLoading = false;
                this.statusText = "Loading";
                this.cancel = function () {
                    var vm = _this;
                    vm.statusText = "Closing";
                    vm.modalInstance.dismiss('cancel');
                };
                this.save = function () {
                    var vm = _this;
                    vm.isLoading = true;
                    vm.statusText = "Saving";
                    var security = {
                        securityId: vm.selectedPosition.securityId,
                        bbgId: vm.selectedPosition.bbgId
                    };
                    vm.dataService.updateSecurity(security).then(function (result) {
                        vm.isLoading = false;
                        vm.error = null;
                        vm.modalInstance.close(result);
                    }).catch(function (err) {
                        vm.error = err;
                    });
                    //if (vm.updateType == "bbg") {
                    //	vm.dataService.updateSecurity(security).then(result => {
                    //		vm.isLoading = false;
                    //		vm.error = null;
                    //		vm.modalInstance.close(result);
                    //	}).catch(err => {
                    //		vm.error = err;
                    //	});
                    //}
                };
                var vm = this;
                vm.modalInstance = $modalInstance;
                vm.dataService = dataService;
                vm.scope = $scope;
                vm.selectedPosition = sourcedata.position;
                vm.updateType = sourcedata.updateType;
                vm.oldBbgId = vm.selectedPosition.bbgId;
            }
            return BbgPopupController;
        }());
        BbgPopupController.$inject = ["application.services.dataService", "$scope", "$uibModalInstance", 'sourcedata'];
        Controllers.BbgPopupController = BbgPopupController;
        angular.module("app").controller("application.controllers.bbgPopupController", BbgPopupController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=BbgPopupController.js.map