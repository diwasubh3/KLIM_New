module Application.Controllers {
    export class ConfirmSecurityReconController {
        modalInstance: angular.ui.bootstrap.IModalServiceInstance;
        securityOverride: Models.IPosition;
        
        appBasePath: string = pageOptions.appBasePath;
        isLoading: boolean = false;
        statusText: string = "Loading";
        securities : Array<Models.ISecurityRecon>;
        dataService: Application.Services.Contracts.IDataService;
        
        data:Array<Models.ISecurityOverride>;
        scope:ng.IScope;
        fields: Array<Models.IField>;
       
        confirmDelete:boolean=false;
        overrideFields: Array<Models.IField>;
        static $inject = ["application.services.dataService", "$window", "$scope", "$modalInstance", 'NgTableParams', 'sourcedata'];
        constructor(dataService: Application.Services.Contracts.IDataService, $window: ng.IWindowService, $scope: angular.IScope, $modalInstance: angular.ui.bootstrap.IModalServiceInstance,
            ngTableParams: NgTableParams,
            sourcedata: any) {
            var vm = this;
            vm.modalInstance = $modalInstance;
            vm.dataService = dataService;
            vm.scope = $scope;
            vm.securities = sourcedata.securities;
            vm.fields = sourcedata.fields;
        }

        getStyle = (column: Models.IField) => {
            var style:any = {};
            if (column.displayWidth) {
                style.width = column.displayWidth  ;
            }
            if (column.jsonPropertyName == 'issuer') {
                style['width'] = 300;
            }
            if (column.jsonPropertyName == 'facility') {
                style.width = null;
            }
           
            return style;
        }

        cancel = () => {
            var vm = this;
            vm.statusText = "Closing";
            vm.modalInstance.dismiss('cancel');
        }

        save = () => {
        var vm = this;
        vm.confirmDelete = false;
            vm.isLoading = true;
            vm.statusText = "Matching";
            vm.dataService.reconcileSecurities(vm.securities).then((result) => {
                vm.isLoading = false;
                vm.modalInstance.close(result);
            });
        }
    }

    angular.module("app").controller("application.controllers.confirmSecurityReconController", ConfirmSecurityReconController);
}