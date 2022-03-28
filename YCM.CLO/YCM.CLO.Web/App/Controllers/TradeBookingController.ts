module Application.Controllers {
    export class TradeBookingController {
        dataService: Application.Services.Contracts.IDataService;
        uiService: Application.Services.Contracts.IUIService;
        rootScope: ng.IRootScopeService;

        isLoading: boolean;
        data: Array<Application.Models.ITradeBooking>;
        appBasePath: string = pageOptions.appBasePath;
        ngTableParams: any;
        statusText: string = "Loading";
        selectedFund: Models.ISummary;
        tableParams: any;
        customViews: Array<Models.ICustomView>;
        modalService: angular.ui.bootstrap.IModalService;
        filter: ng.IFilterService;
        timeOutService: ng.ITimeoutService;
        headerFields: Array<Models.IField>;
        includeCancelled: boolean = false;
        sourceData: Models.ITradeBooking;
        tempSecurity: Models.ITradeBooking;
        static $inject = ["application.services.uiService", "application.services.dataService", "$rootScope", '$uibModal', 'NgTableParams', '$filter', '$timeout'];

        constructor(uiService: Application.Services.Contracts.IUIService, dataService: Application.Services.Contracts.IDataService, $rootScope: ng.IRootScopeService, modalService: angular.ui.bootstrap.IModalService, ngTableParams: NgTableParams, $filter: ng.IFilterService, timeOutService: ng.ITimeoutService) {
            var vm = this;
            vm.dataService = dataService;
            vm.uiService = uiService;
            vm.rootScope = $rootScope;
            vm.rootScope.$emit('onActivated', 'tradebooking');
            vm.modalService = modalService;
            vm.ngTableParams = ngTableParams;
            vm.filter = $filter;

            vm.isLoading = true;
            vm.loadDropdownData();
        }

        loadDropdownData = () => {
            var vm = this;
            vm.statusText = "Loading";
            vm.isLoading = true;
            vm.dataService.getTradeBookingSourceData().then((d) => {
                vm.sourceData = d;
                vm.isLoading = false;
            })
        }

        getPositionFromTrade = (trade: Models.ITradeBooking) => {

            var security: any = trade['security'];
            return security;
        }
        
        ShowHide = (prop: string) => {
            var vm = this;
            vm[prop].toggle();
        }

        
        select = (trade: Models.ITradeBooking) => {
        }


        getSortedCustomViews = (views: Array<Models.ICustomView>) => {
            var vm = this;
            var separatorName = "---------------";
            (<any>views).forEach(x => x.isDisabled = false);
            var sorted = views.sort(function (a, b) {
                return ((a.isPublic === b.isPublic) ? 0 : b.isPublic ? -1 : 1) || a.viewName.toLowerCase().localeCompare(b.viewName.toLowerCase());
            });
            vm.insertViewSeparator(separatorName, sorted);
            vm.customViews = sorted;
        }

        insertViewSeparator = (separatorName: string, views: Array<Models.ICustomView>) => {
            var vm = this;
            var pubs = views.filter(x => x.isPublic);
            var privates = views.filter(x => !x.isPublic);
            if (pubs.length && privates.length) {
                //var sorted = _.sortBy(views, 'isPublic');
                var firstPubIndex = (<any>views).findIndex(x => x.isPublic);
                var separator = <Models.ICustomView>{ viewName: separatorName, isDisabled: true, isDefault: false };
                views.splice(firstPubIndex, 0, separator);
            }
        }        
    }

    angular.module("app").controller("application.controllers.TradeBookingController", TradeBookingController);
}