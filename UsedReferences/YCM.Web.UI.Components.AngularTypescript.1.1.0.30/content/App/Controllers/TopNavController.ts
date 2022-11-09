
module Application.Controllers {
    export class TopNavController {
        rootScope: ng.IRootScopeService;
        scope: any;
        activeView: string;
        static $inject = ["$rootScope", "$scope"];
        constructor($rootScope: ng.IRootScopeService, $scope: any) {
            this.rootScope = $rootScope;
            this.scope = $scope;
            this.scope.activeView = 'home';
            $rootScope.$on('onActivated', (event, data) => {
                this.scope.activeView = data;
            });
        }
    }

    angular.module("app").controller("topNavController",
        ['$rootScope', '$scope', ($rs, scope) => new TopNavController($rs, scope)]);
}