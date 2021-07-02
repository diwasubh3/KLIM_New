module Application.Controllers {
    export class AddUpdateParameterValueController {
        modalInstance: angular.ui.bootstrap.IModalServiceInstance;
        parameterValue: Models.IParameterValue;
        appBasePath: string = pageOptions.appBasePath;
        isLoading: boolean = false;
        statusText: string = "Saving";
        parameterTypes:Array<Models.IParameterType>;
        dataService: Application.Services.Contracts.IDataService;

        static $inject = ["application.services.dataService", "$window", "$scope", "$modalInstance", 'sourcedata'];
        constructor(dataService: Application.Services.Contracts.IDataService, $window: ng.IWindowService, $scope: angular.IScope, $modalInstance: angular.ui.bootstrap.IModalServiceInstance,
            sourcedata: Models.IParameterValue) {
            var vm = this;
            this.parameterValue = sourcedata;
            this.modalInstance = $modalInstance;
            this.dataService = dataService;
            this.dataService.loadParameterTypes().then(paramTypes => vm.parameterTypes = paramTypes);
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
            var parametrValue: Models.IParameterValue = JSON.parse(JSON.stringify(vm.parameterValue));
            vm.parameterValue.parameterType = null;
            vm.dataService.updateParameterValue(parametrValue).then((updatedParamterVal) => {
                vm.isLoading = false;
                vm.modalInstance.close(updatedParamterVal);
            });
        }

    }


angular.module("app").controller("application.controllers.addUpdateParameterValueController", AddUpdateParameterValueController);
}