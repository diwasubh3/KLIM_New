module Application.Controllers {
    export class AboutController {
        
        rootScope: ng.IRootScopeService;

        isLoading: boolean;
        data: Array<Application.Models.UserModel>;
        appBasePath: string = pageOptions.appBasePath;

        statusText: string;
        modalService: angular.ui.bootstrap.IModalService;
        static $inject = ["$rootScope", '$modal'];

        constructor($rootScope: ng.IRootScopeService, modalService: angular.ui.bootstrap.IModalService) {
            
            this.rootScope = $rootScope;
            this.rootScope.$emit('onActivated', 'about');
            this.modalService = modalService;
            this.loadData();
        }

        loadData = () => {
            
        }

    }

    angular.module("app").controller("application.controllers.aboutController", AboutController);
}  