var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var WatchController = (function () {
            function WatchController(dataService, $window, $scope, $modalInstance, sourcedata, $rootScope) {
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
                    //var watch: Models.IWatch = JSON.parse(JSON.stringify(vm.watch));
                    if (vm.watch.watchObjectTypeId == 1) {
                        vm.watch.watchObjectId = vm.watch.securityId;
                    }
                    else if (vm.watch.watchObjectTypeId == 2) {
                        vm.watch.watchObjectId = vm.watch.issuerId;
                    }
                    vm.dataService.updateWatch(vm.watch, vm.rootScope['selectedFund'].fundCode).then(function (updatedpositions) {
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
                    var watch = JSON.parse(JSON.stringify(vm.watch));
                    vm.dataService.deleteWatch(watch).then(function (updatedpositions) {
                        vm.isLoading = false;
                        vm.modalInstance.close(updatedpositions);
                    });
                };
                this.watch = sourcedata;
                this.modalInstance = $modalInstance;
                this.rootScope = $rootScope;
                this.dataService = dataService;
                //if (this.position.watchObjectTypeId) {
                //    this.position.watchObjectTypeId = this.position.watchObjectTypeId.toString();
                //}
            }
            return WatchController;
        }());
        WatchController.$inject = ["application.services.dataService", "$window", "$scope", "$modalInstance", 'sourcedata', "$rootScope"];
        Controllers.WatchController = WatchController;
        angular.module("app").controller("application.controllers.watchController", WatchController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=WatchController.js.map