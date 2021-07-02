module Application.Controllers {
    export class ConfirmTradeSwapRunController {
        modalInstance: angular.ui.bootstrap.IModalServiceInstance;
        tradeSwap : Models.ITradeSwapDefinition;
        appBasePath: string = pageOptions.appBasePath;
        isLoading: boolean = false;
        statusText: string = "Saving";
        dataService: Application.Services.Contracts.IDataService;

        static $inject = ["application.services.dataService", "$window", "$scope", "$modalInstance", 'sourcedata'];
        constructor(dataService: Application.Services.Contracts.IDataService, $window: ng.IWindowService, $scope: angular.IScope, $modalInstance: angular.ui.bootstrap.IModalServiceInstance,
            sourcedata: Models.ITradeSwapDefinition) {
            this.tradeSwap = sourcedata;
            this.modalInstance = $modalInstance;
            this.dataService = dataService;
        }

        cancel = () => {
            var vm = this;
            vm.modalInstance.dismiss('cancel');
        }

        proceed = () => {
            var vm = this;
            vm.modalInstance.close({confirm:true});
        }
        
    }

    angular.module("app").controller("application.controllers.confirmTradeSwapRunController", ConfirmTradeSwapRunController);
}