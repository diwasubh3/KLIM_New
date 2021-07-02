module Application.Controllers {
    export class WatchController {
        modalInstance: angular.ui.bootstrap.IModalServiceInstance;
		watch: Models.IWatch;
        appBasePath: string = pageOptions.appBasePath;
        isLoading: boolean = false;
        statusText: string = "Saving";
        dataService: Application.Services.Contracts.IDataService;
        rootScope: ng.IRootScopeService;

        static $inject = ["application.services.dataService", "$window", "$scope", "$modalInstance", 'sourcedata', "$rootScope"];
        constructor(dataService: Application.Services.Contracts.IDataService, $window: ng.IWindowService, $scope: angular.IScope, $modalInstance: angular.ui.bootstrap.IModalServiceInstance,
            sourcedata: Models.IWatch,$rootScope:ng.IRootScopeService) {
			this.watch = sourcedata;
            this.modalInstance = $modalInstance;
            this.rootScope = $rootScope;
            this.dataService = dataService;
            //if (this.position.watchObjectTypeId) {
            //    this.position.watchObjectTypeId = this.position.watchObjectTypeId.toString();
            //}
        }

        cancel = () => {
            var vm = this;
            vm.statusText = "Closing";
            vm.isLoading = true;
            vm.modalInstance.dismiss('cancel');
        }

        save = () => {
            var vm = this;
            vm.isLoading = true;
            vm.statusText = "Updating";
			//var watch: Models.IWatch = JSON.parse(JSON.stringify(vm.watch));
            
			if (vm.watch.watchObjectTypeId == 1) {
				vm.watch.watchObjectId = vm.watch.securityId;
			} else if (vm.watch.watchObjectTypeId == 2) {
				vm.watch.watchObjectId = vm.watch.issuerId;
            }

			vm.dataService.updateWatch(vm.watch, vm.rootScope['selectedFund'].fundCode).then((updatedpositions) => {
                vm.isLoading = false;
                vm.modalInstance.close(updatedpositions);
            }).catch(ex => {
				alert(ex);
				vm.isLoading = false;
				vm.modalInstance.close();
			});
        }

        delete = () => {
            var vm = this;
            vm.isLoading = true;
            vm.statusText = "Deleting";
			var watch: Models.IWatch = JSON.parse(JSON.stringify(vm.watch));

            vm.dataService.deleteWatch(watch).then((updatedpositions) => {
                vm.isLoading = false;
                vm.modalInstance.close(updatedpositions);
            });
        }
    }


    angular.module("app").controller("application.controllers.watchController", WatchController);
}