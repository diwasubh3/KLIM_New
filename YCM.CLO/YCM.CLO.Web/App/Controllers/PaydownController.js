var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var PaydownController = (function () {
            function PaydownController(dataService, $window, $scope, $modalInstance, sourcedata, $rootScope) {
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
                    debugger;
                    var vm = _this;
                    vm.isLoading = true;
                    vm.statusText = "Updating";
                    //var paydown: Models.IPaydown = JSON.parse(JSON.stringify(vm.paydown));
                    if (vm.paydown.paydownObjectTypeId == 1) {
                        vm.paydown.paydownObjectId = vm.paydown.securityId;
                    }
                    else if (vm.paydown.paydownObjectTypeId == 2) {
                        vm.paydown.paydownObjectId = vm.paydown.issuerId;
                    }
                    vm.dataService.updatePaydown(vm.paydown, vm.rootScope['selectedFund'].fundCode).then(function (updatedpositions) {
                        vm.isLoading = false;
                        vm.modalInstance.close(updatedpositions);
                    }).catch(function (ex) {
                        alert(ex);
                        vm.isLoading = false;
                        vm.modalInstance.close();
                    });
                };
                this.delete = function () {
                    var vm = _this;
                    vm.isLoading = true;
                    vm.statusText = "Deleting";
                    var paydown = JSON.parse(JSON.stringify(vm.paydown));
                    vm.dataService.deletePaydown(paydown).then(function (updatedpositions) {
                        vm.isLoading = false;
                        vm.modalInstance.close(updatedpositions);
                    });
                };
                this.paydown = sourcedata;
                this.modalInstance = $modalInstance;
                this.rootScope = $rootScope;
                this.dataService = dataService;
                //if (this.position.paydownObjectTypeId) {
                //    this.position.paydownObjectTypeId = this.position.paydownObjectTypeId.toString();
                //}
            }
            return PaydownController;
        }());
        PaydownController.$inject = ["application.services.dataService", "$window", "$scope", "$modalInstance", 'sourcedata', "$rootScope"];
        Controllers.PaydownController = PaydownController;
        angular.module("app").controller("application.controllers.paydownController", PaydownController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=PaydownController.js.map