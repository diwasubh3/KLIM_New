module Application.Controllers {
    export class DayOverChangesController {
        dataService: Application.Services.Contracts.IDataService;
        uiService: Application.Services.Contracts.IUIService;
        rootScope: ng.IRootScopeService;
        isLoading: boolean;
        appBasePath: string = pageOptions.appBasePath;
        //ngTableParams: any;
        statusText: string = "Loading";
        tableParams: any;
        scope: ng.IScope;
        errorMessage: string;
        static $inject = ["application.services.uiService", "application.services.dataService", "$rootScope", 'NgTableParams', '$filter', "$scope", 'uiGridConstants', 'uiGridExporterService'];
        gridHeight: any = { 'height': '402px' };
        ratingchanges: Array<Models.IRatingChange>;
        totalparchanges: Array<Models.ITotalParChange>;
        moodyrecoverychanges: Array<Models.IMoodyRecoveryChange>;
        topBottonPriceMovers: Models.ITopBottonPriceMovers;
        pricemovers: any;
        startDate: any;
        endDate: any;
        prevDayTopCount: number;
        prevDayBottomCount: number;
        prev5DayTopCount: number;
        prev5DayBottomCount: number;
        isRatingDataAvailable: boolean = false;
        isMoodyDataAvailable: boolean = false;
        isTotalParDataAvailable: boolean = false;

        constructor(uiService: Application.Services.Contracts.IUIService, dataService: Application.Services.Contracts.IDataService, $rootScope: ng.IRootScopeService, $scope: angular.IScope, uiGridConstants: any, exportUiGridService: any) {
            var vm = this;
            vm.dataService = dataService;
            vm.uiService = uiService;
            vm.rootScope = $rootScope;
            vm.rootScope.$emit('onActivated', 'dayoverchanges');
            vm.scope = $scope;
            vm.errorMessage = "";
            vm.loadRatingChanges();
        }

        loadRatingChanges = () => {
            var vm = this;
            vm.statusText = "Loading";
            vm.isLoading = true;
            vm.dataService.getRatingchanges().then((ratechange) => {
                vm.ratingchanges = ratechange;
                vm.isLoading = false;
                if (vm.ratingchanges.length <= 0)
                    vm.isRatingDataAvailable = true;
            });
        }

        loadTotalParChanges = () => {
            var vm = this;
            vm.statusText = "Loading";
            vm.isLoading = true;
            vm.dataService.getTotalParChanges().then((totalpar) => {
                vm.totalparchanges = totalpar;
                vm.isLoading = false;
                if (vm.totalparchanges.length <= 0)
                    vm.isTotalParDataAvailable  = true;
            });
        }

        loadMoodyRecoveryDateChanges = () => {
            var vm = this;
            vm.statusText = "Loading";
            vm.isLoading = true;
            vm.dataService.getMoodyRecoveryDateChanges().then(moody => {
                vm.moodyrecoverychanges = moody;
                vm.isLoading = false;
                if (vm.moodyrecoverychanges.length <= 0)
                    vm.isMoodyDataAvailable = true;
            });
        }

        /*count <= 15 ? count : 15;*/
        loadTopBottonPriceMovers = () => {
            var vm = this;
            vm.statusText = "Loading";
            vm.isLoading = true;
            vm.dataService.getTopBottomPriceMoversChanges().then(pm => {
                vm.topBottonPriceMovers = pm;
                vm.prevDayTopCount = vm.topBottonPriceMovers.prevDayTop.length <= 15 ? vm.topBottonPriceMovers.prevDayTop.length : 15;
                vm.prevDayBottomCount = vm.topBottonPriceMovers.prevDayBottom.length <= 15 ? vm.topBottonPriceMovers.prevDayBottom.length : 15;
                vm.prev5DayTopCount = vm.topBottonPriceMovers.prev5DayTop.length <= 15 ? vm.topBottonPriceMovers.prev5DayTop.length : 15;
                vm.prev5DayBottomCount = vm.topBottonPriceMovers.prev5DayBottom.length <= 15 ? vm.topBottonPriceMovers.prev5DayBottom.length : 15;
                vm.isLoading = false;
            });
        }
    }

    angular.module('app').controller("application.controllers.DayOverChangesController", DayOverChangesController);
}