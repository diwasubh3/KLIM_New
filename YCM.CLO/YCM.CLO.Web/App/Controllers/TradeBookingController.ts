module Application.Controllers {
    export class TradeBookingController {
        dataService: Application.Services.Contracts.IDataService;
        uiService: Application.Services.Contracts.IUIService;
        rootScope: ng.IRootScopeService;
        /* securities: Array<Models.IVwSecurityDto>;*/
        securities: Array<Models.IIssuerSecurity>;
        isLoading: boolean;
        funds: Array<Models.IFund>;
        trades: Array<Models.ITradeBooking>;
        appBasePath: string = pageOptions.appBasePath;
        ngTableParams: any;
        statusText: string = "Loading";
        tableParams: any;
        customViews: Array<Models.ICustomView>;
        scope: ng.IScope;
        filter: ng.IFilterService;
        timeOutService: ng.ITimeoutService;
        headerFields: Array<Models.IField>;
        includeCancelled: boolean = false;
        sourceData: Models.ITradeBookingData;
        tempSecurity: Models.ITradeBooking;
        issuerSec: Models.ISecurity;
        tradebookingdetail: Array<Models.ITradeBookingDetail>;
        ViewIsOpen: boolean;
        TradeBookingIsOpen: boolean;
        gridApi: any;
        gridOptions: any;
        gridHeight: any = { 'height': '402px' };
        static $inject = ["application.services.uiService", "application.services.dataService", "$rootScope", 'NgTableParams', '$filter', "$scope"];

        
        constructor(uiService: Application.Services.Contracts.IUIService, dataService: Application.Services.Contracts.IDataService, $rootScope: ng.IRootScopeService,
            ngTableParams: NgTableParams, $filter: ng.IFilterService, $scope: angular.IScope) {
            var vm = this;
            vm.dataService = dataService;
            vm.uiService = uiService;
            vm.rootScope = $rootScope;
            vm.rootScope.$emit('onActivated', 'tradebooking');
            vm.ngTableParams = ngTableParams;
            vm.filter = $filter;
            vm.ViewIsOpen= true;
            vm.TradeBookingIsOpen= false;
            vm.isLoading = true;
            vm.loadDropdownData();
            vm.tempSecurity = <Models.ITradeBooking>{};
            vm.issuerSec = <Models.ISecurity>{};            
            vm.scope = $scope;
            vm.tempSecurity.tradeDate = new Date();
            vm.tempSecurity.tradeType = { tradeTypeDesc: 'Buy', tradeTypeId: 1 };
            vm.tempSecurity.traders = { traderName: 'Eugene Koltunov', traderId: 1231 };
            vm.tempSecurity.interesttreatments = { description: 'Settles Without Accrued', id: 1 };
            const cDefs = [
                {
                    name: 'IsSkipped',
                    field: "IsSkipped",
                    displayName: "Skip",
                    cellTemplate: '<div class="ui-grid-cell-contents" ><input type="checkbox" ng-model="row.entity.isSkipped" ng-change="grid.appScope.onChangeDemo(row)"/></div>',
                    width: "5%",
                },
                {
                    name: 'portfolioName',
                    field: "portfolioName",
                    displayName: "Portfolio",
                    width: "20%",
                    visible: true,
                    
                    enableSorting: false,
                    type: "string"
                },
                {
                    name: 'existing',
                    field: "existing",
                    displayName: "Existing Position",
                    width: "15%",
                    visible: true,
                    enableCellEdit: false,
                    enableSorting: false,
                    cellClass: 'text-right',
                    type: "string",
                    cellFilter: 'number: 2'
                },
                {
                    name: 'allocated',
                    field: "allocated",
                    displayName: 'Allocated Position',
                    cellClass: 'text-right',                    
                    width: "15%",
                    visible: true,
                    enableCellEdit: false,
                    enableSorting: false,
                    type: "string",
                    cellFilter: 'number: 2'
                },
                {
                    name: 'override',
                    field: "override",
                    displayName: 'Manual Allocation',
                    width: "15%",
                    visible: true,                    
                    enableCellEdit: true,
                    enableSorting: false,
                    type: "number",
                    cellClass: 'text-right',
                    /*cellTemplate: '<div><input type="INPUT_TYPE" style="height: 20px !important;text-align:right" ng-class="\'colt\' + col.uid"\ ui-grid-editor ng-model="row.entity.override" ng-change="grid.appScope.OnCellLeave(row)"></div>',*/
                    /*cellEditableCondition: true,*/
                    cellFilter: 'number: 2'
                },
                {
                    name: 'finalQty',
                    field: "finalQty",
                    displayName: 'Final Quantity',
                    width: "15%",
                    visible: true,
                    enableCellEdit: false,
                    enableSorting: false,
                    cellClass: 'text-right',
                    type: "string",
                    cellFilter: 'number: 2'
                },
                {
                    name: 'tradeAmount',
                    field: "tradeAmount",
                    displayName: 'Trade Amount',
                    width: "15%",
                    visible: false,
                    enableCellEdit: false,
                    enableSorting: false,
                    cellClass: 'text-right',
                    type: "string",
                    cellFilter: 'number: 2'
                }
            ];
            
            vm.gridOptions = {
                columnDefs: cDefs,                
                rowHeight: 30,
                onRegisterApi(gridApi) {
                    vm.gridApi = gridApi;                    
                    gridApi.edit.on.afterCellEdit($scope, function (rowEntity, colDef, newValue, oldValue) {                        
                        vm.dataService.getCalculatedData(vm.gridOptions.data).then(data => {
                            vm.gridOptions.data = data;
                            vm.isLoading = false;
                        });
                    });
                },
            }
            vm.gridOptions.appScopeProvider = vm;            
        }

        onVisibilityChanged(open: boolean): void {
            var vm = this;
            vm.rootScope.$emit('onVisibilityChanged', open);
        }

        clearAll = () => {
            var vm = this;
            vm.tempSecurity.tradeType = undefined;
            vm.tempSecurity.issuer = '';
            vm.tempSecurity.loanXId = '';
            vm.tempSecurity.facility = undefined;
            vm.tempSecurity.counterparty = undefined;
            vm.tempSecurity.settlemethods = undefined;            
            vm.tempSecurity.totalQty = undefined;
            vm.tempSecurity.price = undefined;
            vm.tempSecurity.allocationRule = undefined;
            vm.tempSecurity.tradeComment = '';
            vm.tempSecurity.selectedSecurity = '';
            vm.tempSecurity.tradeBookingDetail.length = 0;
            vm.gridOptions.data = vm.tempSecurity.tradeBookingDetail;
        }

        onChangeDemo = function (row) {
            var vm = this;
            vm.dataService.getCalculatedData(vm.gridOptions.data).then(data => {
                vm.gridOptions.data = data;
                vm.gridOptions.footerTemplate = '<div class="ui-grid-bottom-panel" style="text-align: center">I am a Custom Grid Footer</div>';
                vm.isLoading = false;
            });
        }

        OnCellLeave = function (row) {
            var vm = this;
            alert('Called');
            //vm.dataService.getCalculatedData(vm.gridOptions.data).then(data => {
            //    vm.gridOptions.data = data;
            //    vm.gridOptions.footerTemplate = '<div class="ui-grid-bottom-panel" style="text-align: center">I am a Custom Grid Footer</div>';
            //    vm.isLoading = false;
            //});
        }

        GetFundAllocation = (allocation) => {
            var vm = this;
            vm.isLoading = true;
            if (allocation.totalQty == undefined) {
                alert('Please Enter Total Quantity');
                return;
            }
            
            if (allocation.allocationRule == undefined) {
                alert('Please Select Allocation Method From List');
                return;
            }
            vm.dataService.getTradeFundAllocation(allocation.totalQty, allocation.allocationRule.ruleName).then(allocationdata => {                
                vm.tradebookingdetail = allocationdata;
                vm.gridOptions.data = vm.tradebookingdetail;                 
                vm.isLoading = false;
            });
        }

        refreshGrid = () => {
            var vm = this;
            vm.gridApi.grid.refresh();
        }


        setFacility = (sec) => {
            var vm = this;
            vm.tempSecurity.facility = { facilityDesc: sec.facilityDesc, facilityId: sec.facilityId };
            vm.tempSecurity.loanXId = sec.securityCode;
            vm.tempSecurity.issuerId = sec.issuerId;
            vm.tempSecurity.issuer = sec.issuer;
        };

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
                vm.dataService.getTradeBooking().then((trades) => {                    
                    vm.trades = trades;
                });
            });
        }

        GenerateTradeXML = () => {
            var vm = this;
            debugger;
            if (vm.tempSecurity.traders == undefined) {
                alert('Please Select Trader From List');
                return;
            }
            if (vm.tempSecurity.tradeType == undefined) {
                alert('Please Select Trade Type From List');
                return;
            }
            if (vm.tempSecurity.issuerId == undefined) {
                alert('Please Select Issuer/Security From List');
                return;
            }
            if (vm.tempSecurity.facility == undefined) {
                alert('Please Select Asset From List');
                return;
            }
            if (vm.tempSecurity.counterparty == undefined) {
                alert('Please Select Counter Party From List');
                return;
            }
            if (vm.tempSecurity.totalQty == undefined) {
                alert('Please Enter Total Quantity');
                return;
            }
            //if (vm.tempSecurity.allocationRule == undefined) {
            //    alert('Please Select Allocation Method From List');
            //    return;
            //}
            var TotalQty = vm.tempSecurity.totalQty;
            var TotalAllocatedQty = 0;
            for (var _i = 0; _i < vm.gridOptions.data.length; _i++) {
                TotalAllocatedQty = TotalAllocatedQty + parseFloat(vm.gridOptions.data[_i].finalQty.toString());
            }           
            if (Math.round(TotalAllocatedQty) != TotalQty) {
                alert('Total Quantity (' + TotalQty + ') and Final Quantity  (' + Math.round(TotalAllocatedQty) + ') Should Match. ');
                return;
            }
                

            vm.tempSecurity.tradeBookingDetail = vm.gridOptions.data;
            vm.statusText = "Saving";
            vm.isLoading = true;
            
            vm.dataService.generateTradeXML(vm.tempSecurity).then(data => {
                vm.tempSecurity = data;
                vm.tempSecurity.tradeBookingDetail = data.tradeBookingDetail;
                vm.gridOptions.data = data.tradeBookingDetail;
                vm.tempSecurity.tradeDate = new Date();
                alert('Data Saved');
                vm.isLoading = false;
            });
        }        

        getPositionFromTrade = (trade: Models.ITradeBooking) => {

            var security: any = trade['security'];
            return security;
        }
        
        ShowHide = (prop: string) => {
            var vm = this;
            vm[prop].toggle();
        }      
    }

    angular.module("app").controller("application.controllers.tradebookingController", TradeBookingController);
}