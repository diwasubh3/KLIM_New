module Application.Controllers {
	export class BbgPopupController {
		modalInstance: angular.ui.bootstrap.IModalServiceInstance;
		appBasePath: string = pageOptions.appBasePath;
		isLoading: boolean = false;
		statusText: string = "Loading";
		selectedPosition: Models.IPosition;
		dataService: Application.Services.Contracts.IDataService;
		oldBbgId: string;
		scope: ng.IScope;
		warning: string;
		error: string;
		updateType: string;

		static $inject = ["application.services.dataService", "$scope", "$uibModalInstance", 'sourcedata'];

		constructor(dataService: Application.Services.Contracts.IDataService, $scope: angular.IScope
			, $modalInstance: angular.ui.bootstrap.IModalServiceInstance, sourcedata: any) {
			var vm = this;
			vm.modalInstance = $modalInstance;
			vm.dataService = dataService;
			vm.scope = $scope;

			vm.selectedPosition = sourcedata.position;
			vm.updateType = sourcedata.updateType;
			vm.oldBbgId = vm.selectedPosition.bbgId;
		}

		cancel = () => {
			var vm = this;
			vm.statusText = "Closing";
			vm.modalInstance.dismiss('cancel');
		}

		save = () => {
			var vm = this;
			vm.isLoading = true;
			vm.statusText = "Saving";
			var security = <Application.Models.ISecurity>{
				securityId: vm.selectedPosition.securityId
				, bbgId: vm.selectedPosition.bbgId
			}
			vm.dataService.updateSecurity(security).then(result => {
				vm.isLoading = false;
				vm.error = null;
				vm.modalInstance.close(result);
			}).catch(err => {
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
		}

	}

	angular.module("app").controller("application.controllers.bbgPopupController", BbgPopupController);
}