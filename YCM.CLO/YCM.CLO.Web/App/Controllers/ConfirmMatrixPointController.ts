module Application.Controllers {
	export class ConfirmMatrixPointController {
		modalInstance: angular.ui.bootstrap.IModalServiceInstance;
        fundCode: string;
        matrixPoint: number;
		appBasePath: string = pageOptions.appBasePath;
		isLoading: boolean = false;
		statusText: string = "Saving";
		dataService: Application.Services.Contracts.IDataService;

		static $inject = ["application.services.dataService", "$window", "$scope", "$modalInstance", 'sourcedata'];
		constructor(dataService: Application.Services.Contracts.IDataService, $window: ng.IWindowService, $scope: angular.IScope, $modalInstance: angular.ui.bootstrap.IModalServiceInstance,
            sourcedata: any) {
            this.fundCode = sourcedata.fundCode;
            this.matrixPoint = sourcedata.matrixPoint;
			this.modalInstance = $modalInstance;
			this.dataService = dataService;
		}

		cancel = () => {
			var vm = this;
			vm.modalInstance.dismiss('cancel');
		}

		proceed = () => {
			var vm = this;
			vm.modalInstance.close({ confirm: true });
		}

	}

	angular.module("app").controller("application.controllers.confirmMatrixPointController", ConfirmMatrixPointController);
}