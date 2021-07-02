module Application.Controllers {
    export class ShowMessageController {
        modalInstance: angular.ui.bootstrap.IModalServiceInstance;
        appBasePath: string = pageOptions.appBasePath;
        isLoading: boolean = false;
        statusText: string = "Loading";
        
        dataService: Application.Services.Contracts.IDataService;
        data:any;
        tableParams: any;
        scope:ng.IScope;
        
        static $inject = ["application.services.dataService", "$window", "$scope", "$modalInstance", 'NgTableParams', 'sourcedata'];
        constructor(dataService: Application.Services.Contracts.IDataService, $window: ng.IWindowService, $scope: angular.IScope, $modalInstance: angular.ui.bootstrap.IModalServiceInstance,
            ngTableParams: NgTableParams,
            sourcedata: number) {
            var vm = this;
            vm.modalInstance = $modalInstance;
            vm.dataService = dataService;
            vm.scope = $scope;
            vm.data = sourcedata;
        }

        cancel = () => {
            var vm = this;
            vm.statusText = "Closing";
            vm.modalInstance.dismiss('cancel');
        }

        okay = () => {
            var vm = this;
            vm.modalInstance.close(true);
        }
    }

    angular.module("app").controller("application.controllers.showMessageController", ShowMessageController);
}