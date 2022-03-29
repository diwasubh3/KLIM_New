module Application.Controllers {
    export class PaydownController {
        modalInstance: angular.ui.bootstrap.IModalServiceInstance;
		paydown: Models.IPaydown;
        appBasePath: string = pageOptions.appBasePath;
        isLoading: boolean = false;
        statusText: string = "Saving";
        dataService: Application.Services.Contracts.IDataService;
        rootScope: ng.IRootScopeService;

        static $inject = ["application.services.dataService", "$window", "$scope", "$modalInstance", 'sourcedata', "$rootScope"];
        constructor(dataService: Application.Services.Contracts.IDataService, $window: ng.IWindowService, $scope: angular.IScope, $modalInstance: angular.ui.bootstrap.IModalServiceInstance,
            sourcedata: Models.IPaydown,$rootScope:ng.IRootScopeService) {
            this.paydown = sourcedata;
            this.modalInstance = $modalInstance;
            this.rootScope = $rootScope;
            this.dataService = dataService;
            //if (this.position.paydownObjectTypeId) {
            //    this.position.paydownObjectTypeId = this.position.paydownObjectTypeId.toString();
            //}
        }

        cancel = () => {
            var vm = this;
            vm.statusText = "Closing";
            vm.isLoading = true;
            vm.modalInstance.dismiss('cancel');
        }

        save = () => {
            debugger;
            var vm = this;
            vm.isLoading = true;
            vm.statusText = "Updating";
			//var paydown: Models.IPaydown = JSON.parse(JSON.stringify(vm.paydown));
            
            if (vm.paydown.paydownObjectTypeId == 1) {
                vm.paydown.paydownObjectId = vm.paydown.securityId;
            } else if (vm.paydown.paydownObjectTypeId == 2) {
                vm.paydown.paydownObjectId = vm.paydown.issuerId;
            }

            vm.dataService.updatePaydown(vm.paydown, vm.rootScope['selectedFund'].fundCode).then((updatedpositions) => {
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
			var paydown: Models.IPaydown = JSON.parse(JSON.stringify(vm.paydown));

            vm.dataService.deletePaydown(paydown).then((updatedpositions) => {
                vm.isLoading = false;
                vm.modalInstance.close(updatedpositions);
            });
        }
    }


    angular.module("app").controller("application.controllers.paydownController", PaydownController);
}