module Application.Controllers {
    export class TradeBookingController {
        dataService: Application.Services.Contracts.IDataService;
        uiService: Application.Services.Contracts.IUIService;
        rootScope: ng.IRootScopeService;
        /* securities: Array<Models.IVwSecurityDto>;*/
        securities: Array<Models.IIssuerSecurity>;
        isLoading: boolean;
        funds: Array<Models.IFund>;
        appBasePath: string = pageOptions.appBasePath;
        ngTableParams: any;
        statusText: string = "Loading";
        tableParams: any;
        customViews: Array<Models.ICustomView>;
        
        filter: ng.IFilterService;
        timeOutService: ng.ITimeoutService;
        headerFields: Array<Models.IField>;
        includeCancelled: boolean = false;
        sourceData: Models.ITradeBookingData;
        tempSecurity: Models.ITradeBooking;
        issuerSec: Models.ISecurity;
        static $inject = ["application.services.uiService", "application.services.dataService", "$rootScope", 'NgTableParams', '$filter'];
        
        constructor(uiService: Application.Services.Contracts.IUIService, dataService: Application.Services.Contracts.IDataService, $rootScope: ng.IRootScopeService,
            ngTableParams: NgTableParams, $filter: ng.IFilterService) {
            var vm = this;
            vm.dataService = dataService;
            vm.uiService = uiService;
            vm.rootScope = $rootScope;
            vm.rootScope.$emit('onActivated', 'tradebooking');
            vm.ngTableParams = ngTableParams;
            vm.filter = $filter;

            vm.isLoading = true;
            vm.loadDropdownData();
            vm.tempSecurity = <Models.ITradeBooking>{};
            vm.issuerSec = <Models.ISecurity>{};            
        }

        loadDropdownData = () => {
            var vm = this;
            vm.statusText = "Loading";
            vm.isLoading = true;
            vm.dataService.getTradeBookingData().then((tradedata) => {
                vm.sourceData = tradedata;
                vm.isLoading = false;
                vm.dataService.getIssuerSecurities().then((securities) => {
                    vm.securities = securities;
                });
            });
        }

        SetVal = () => {
            alert("ssss");
        }

        GenerateTradeXML = () => {
            var vm = this;
            console.log(vm.tempSecurity);            
            vm.statusText = "Saving";
            vm.isLoading = true;
            vm.dataService.generateTradeXML(vm.sourceData).then(data => {
                vm.isLoading = false;
            });
        }

        GetFundAllocation = () => {
            var vm = this;
            vm.isLoading = true;
            vm.dataService.getFunds().then(funds => {
                funds = funds.filter(f => f.canFilter);
                vm.funds = funds;
                vm.isLoading = false;
            });
        }

        setSelectedField = function(sec) {
            return "Vikarma";
        }

        getPositionFromTrade = (trade: Models.ITradeBooking) => {

            var security: any = trade['security'];
            return security;
        }
        
        ShowHide = (prop: string) => {
            var vm = this;
            vm[prop].toggle();
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

    angular.module("app").controller("application.controllers.tradebookingController", TradeBookingController);
}