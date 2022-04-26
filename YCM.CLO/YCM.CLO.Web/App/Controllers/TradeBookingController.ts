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
        allocationRuleData: Array<Models.IAllocationRule>;
        tempSecurity: Models.ITradeBooking;
        issuerSec: Models.ISecurity;
        tradebookingdetail: Array<Models.ITradeBookingDetail>;
        ViewIsOpen: boolean;
        TradeBookingIsOpen: boolean;
        isDisabled: boolean;
        isHide: boolean;
        isDisabledSettlement: boolean;
        gridApi: any;
        gridOptions: any;
        isRowSelected: any;
        gridHeight: any = { 'height': '402px' };
        cDefs: any;
        static $inject = ["application.services.uiService", "application.services.dataService", "$rootScope", 'NgTableParams', '$filter', "$scope", 'uiGridConstants', 'exportUiGridService'];

        
        constructor(uiService: Application.Services.Contracts.IUIService, dataService: Application.Services.Contracts.IDataService, $rootScope: ng.IRootScopeService,
            ngTableParams: NgTableParams, $filter: ng.IFilterService, $scope: angular.IScope, uiGridConstants: any, exportUiGridService: any) {
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
            vm.isDisabled = false;
            vm.isDisabledSettlement = false;
            vm.isHide = true;
            vm.loadDropdownData();
            vm.tempSecurity = <Models.ITradeBooking>{};
            vm.issuerSec = <Models.ISecurity>{};
            vm.tradebookingdetail = Array<Models.ITradeBookingDetail>();
            vm.allocationRuleData = Array<Models.IAllocationRule>();
            vm.scope = $scope;
            vm.tempSecurity.tradeDate = new Date();
            vm.tempSecurity.tradeType = { tradeTypeDesc: 'Buy', tradeTypeId: 1 };
            vm.tempSecurity.traders = { traderName: 'Eugene Koltunov', traderId: 1231 };
            vm.tempSecurity.interesttreatments = { description: 'Settles Without Accrued', id: 1 };
            vm.loadAllocationRule(vm.tempSecurity.tradeType);
            let tempFooter = '<div class="ui-grid-cell-contents;" style="text-align:right;padding-top:5px;">{{col.getAggregationValue() | number:2 }}</div>';
            vm.cDefs = [
                {
                    name: 'isIncluded',
                    field: "isIncluded",
                    displayName: "Include",
                    //type:"boolean",
                    enableCellEdit: false,
                    cellEditableCondition: false,
                    cellTemplate: '<div class="ui-grid-cell-contents" ><input type="checkbox" ng-model="row.entity.isIncluded" ng-change="grid.appScope.onChangeDemo(row)"/></div>',
                    width: "5%",
                },
                {
                    name: 'portfolioName',
                    field: "portfolioName",
                    displayName: "Portfolio",
                    width: "15%",
                    visible: true,
                    enableCellEdit: false,
                    enableSorting: false,
                    type: "string",
                    footerCellTemplate: '<div class="ui-grid-cell-contents" style="padding-top:5px;">Total</div>'
                },
                {
                    name: 'exposure',
                    field: "exposure",
                    displayName: "Current Exposure",
                    width: "10%",
                    visible: true,
                    enableCellEdit: false,
                    enableSorting: false,
                    cellClass: 'text-right',
                    type: "number",
                    cellFilter: 'number: 2',
                    aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                    footerCellTemplate: tempFooter
                },
                {
                    name: 'existing',
                    field: "existing",
                    displayName: "Existing Position",
                    width: "10%",
                    visible: true,
                    enableCellEdit: false,
                    enableSorting: false,
                    cellClass: 'text-right',
                    type: "number",
                    cellFilter: 'number: 2',
                    aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                    footerCellTemplate: tempFooter
                },
                {
                    name: 'allocated',
                    field: "allocated",
                    displayName: 'Auto Allocated',
                    cellClass: 'text-right',                    
                    width: "10%",
                    visible: true,
                    enableCellEdit: false,
                    enableSorting: false,
                    type: "number",
                    cellFilter: 'number: 2',
                    aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                    footerCellTemplate: tempFooter
                },
                {
                    name: 'override',
                    field: "override",
                    displayName: 'Manual Allocation',
                    width: "10%",
                    visible: true,                    
                    enableCellEdit: true,
                    enableSorting: false,
                    type: "number",
                    cellClass: 'text-right',
                    enableCellEditOnFocus: true,
                    //cellTemplate: '<div class="ui-grid-cell-contents" ><input type="text" ng-model="row.entity.override" style="height: 20px !important;text-align:right" ng-change="grid.appScope.onChangeDemo(row)"/></div>',
                    /*cellTemplate: '<div><input type="INPUT_TYPE" style="height: 20px !important;text-align:right" ng-class="\'colt\' + col.uid"\ ui-grid-editor ng-model="row.entity.override" ng-blur="alert();"></div>',*/
                    
                    cellEditableCondition: true,
                    cellFilter: 'number: 2',
                    aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                    footerCellTemplate: tempFooter
                    //cellEditableCondition: $scope.isDisabled
                },
                {
                    name: 'finalQty',
                    field: "finalQty",
                    displayName: 'Final Quantity',
                    width: "10%",
                    visible: true,
                    enableCellEdit: false,
                    enableSorting: false,
                    cellClass: 'text-right',
                    type: "string",
                    cellFilter: 'number: 2',
                    aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                    footerCellTemplate: tempFooter
                },
                {
                    name: 'tradeAmount',
                    field: "tradeAmount",
                    displayName: 'Net Amount',
                    width: "10%",
                    visible: true,
                    enableCellEdit: false,
                    enableSorting: false,
                    cellClass: 'text-right',
                    type: "string",
                    cellFilter: 'number: 2',
                    aggregationType: uiGridConstants.aggregationTypes.avg, aggregationHideLabel: true,
                    footerCellTemplate: tempFooter
                }
            ];
            
            vm.gridOptions = {
                columnDefs: vm.cDefs,
                //showGridFooter: true,
                showColumnFooter: true,
                rowHeight: 30,
                onRegisterApi(gridApi) {
                    vm.gridApi = gridApi;                    
                    gridApi.edit.on.afterCellEdit($scope, function (rowEntity, colDef, newValue, oldValue) {
                        vm.tradebookingdetail = vm.gridOptions.data;
                        for (var _i = 0; _i < vm.tradebookingdetail.length; _i++) {
                            vm.tradebookingdetail[_i].totalQuantity = vm.tempSecurity.totalQty;
                            vm.tradebookingdetail[_i].ruleName = vm.tempSecurity.allocationRule.ruleName;
                            vm.tradebookingdetail[_i].price = vm.tempSecurity.price;
                        }
                        vm.dataService.getCalculatedData(vm.tradebookingdetail).then(data => {
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

        ConvertToCurrency = (val) => {
            var vm = this;

            //vm.tempSecurity.price = price.toFixed(2);
            if (val != null && val.length > 0) {
                var returnprice = parseFloat(val.replace(/,/g, ""))
                    .toFixed(2)
                    .replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                vm.tempSecurity.price = parseFloat(returnprice);
                console.log(returnprice);
            } else {
                vm.tempSecurity.price = 0;
            }
        }

        rowHighilited = (row) => {
            var vm = this;
            console.log(row);
            vm.isRowSelected = row;
        }

        tradeTypeChangeEvent = (tradeType) => {
            var vm = this;
            vm.tempSecurity.settlemethods = undefined;
            vm.isDisabledSettlement = false;
            if (tradeType.tradeTypeId == 2) {
                vm.tempSecurity.settlemethods = { methodName: 'Assignment', methodId: 1 };
                vm.isDisabledSettlement = true;
            }
            vm.loadAllocationRule(tradeType);            
        }

        clearAll = (newBooking) => {
            var vm = this;
            vm.tempSecurity.tradeDate = new Date();
            vm.tempSecurity.tradeType = { tradeTypeDesc: 'Buy', tradeTypeId: 1 };
            vm.tempSecurity.issuerDesc = '';
            vm.tempSecurity.loanXId = '';
            vm.tempSecurity.facility = undefined;
            vm.tempSecurity.counterparty = undefined;
            vm.tempSecurity.settlemethods = undefined;            
            vm.tempSecurity.totalQty = undefined;
            vm.tempSecurity.price = undefined;
            vm.tempSecurity.allocationRule = undefined;
            vm.tempSecurity.tradeComment = '';
            vm.tempSecurity.selectedSecurity = '';
            vm.gridOptions.data = [];
            vm.TradeBookingIsOpen = true;
            vm.ViewIsOpen = false;
            vm.rootScope.$emit('onVisibilityChanged', open);
            vm.isDisabled = false;
            vm.isDisabledSettlement = false;
            vm.isHide = true;
        }

        setTradeBooking = (tradeId) => {
            var vm = this;
            vm.dataService.refreshTradeBooking(tradeId).then(data => {
                data.tradeDate = new Date(data.tradeDate);                
                vm.tempSecurity = data;                
                vm.tradebookingdetail = data.tradeBookingDetail;
                vm.gridOptions.data = data.tradeBookingDetail;
                vm.isDisabled = true;
                vm.isHide = true;
                vm.isLoading = false;
            });
        }

        onChangeDemo = function (row) {
            var vm = this;
            vm.tradebookingdetail = vm.gridOptions.data;
            for (var _i = 0; _i < vm.tradebookingdetail.length; _i++) {
                vm.tradebookingdetail[_i].totalQuantity = vm.tempSecurity.totalQty;
                vm.tradebookingdetail[_i].ruleName = vm.tempSecurity.allocationRule.ruleName;
                vm.tradebookingdetail[_i].price = vm.tempSecurity.price;
            }
            vm.dataService.getCalculatedData(vm.tradebookingdetail).then(data => {                
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
            var bodyMesg = "";
            if (vm.tempSecurity.traders == undefined) {
                bodyMesg = 'Please Select Trader From List';
            }
            if (vm.tempSecurity.tradeType == undefined) {
                bodyMesg = bodyMesg + "<br>" + 'Please Select Trade Type From List';
            }
            if (vm.tempSecurity.issuerId == undefined) {
                bodyMesg = bodyMesg + "<br>" + 'Please Select Issuer/Security From List';
            }
            if (vm.tempSecurity.facility == undefined) {
                bodyMesg = bodyMesg + "<br>" + 'Please Select Asset From List';
            }
            if (vm.tempSecurity.counterparty == undefined) {
                bodyMesg = bodyMesg + "<br>" + 'Please Select Counter Party From List';
            }
            if (allocation.allocationRule.ruleName.indexOf("Sell All") > -1) {
                vm.tempSecurity.totalQty = 0;
            }
            else {
                if (vm.tempSecurity.totalQty == undefined) {
                    bodyMesg = bodyMesg + "<br>" + 'Please Enter Total Quantity';
                }
            }            
            if (vm.tempSecurity.allocationRule == undefined) {
                bodyMesg = bodyMesg + "<br>" + 'Please Select Allocation Method From List';
            }
            if (bodyMesg != '') {
                var message: Models.IMessage = {
                    header: "Warning",
                    body: "<p><b>" + bodyMesg + "</b></p>"
                };
                vm.uiService.showMessage(message);
                return;
            }
            vm.dataService.getTradeFundAllocation(allocation.totalQty, allocation.allocationRule.ruleName, allocation.issuerId, vm.tempSecurity.price).then(allocationdata => {
                vm.tradebookingdetail = allocationdata;                
                vm.gridOptions.data = vm.tradebookingdetail;                
                if (allocation.allocationRule.ruleName.indexOf("Manual") > -1) {
                    vm.gridApi.grid.columns[2].hideColumn();
                    vm.gridApi.grid.columns[4].hideColumn();
                } else {
                    vm.gridApi.grid.columns[2].showColumn();
                    vm.gridApi.grid.columns[4].showColumn();
                }
                //if (allocation.allocationRule.ruleName.indexOf("Sell All") > -1) {
                //    vm.gridApi.grid.columns[4].hideColumn();
                //    vm.gridApi.grid.columns[5].hideColumn();
                //} else {
                //    vm.gridApi.grid.columns[4].showColumn();
                //    vm.gridApi.grid.columns[5].showColumn();
                //}
                vm.isHide = false;
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
            vm.tempSecurity.issuerDesc = sec.issuer;
        };

        loadAllocationRule = (tradeType) => {
            var vm = this;
            vm.dataService.getAllocationRule(tradeType.tradeTypeId).then((rules) => {
                vm.allocationRuleData = rules;
            });
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
                vm.dataService.getTradeBooking().then((trades) => {                    
                    vm.trades = trades;
                });
            });
            
        }

        GenerateTradeXML = () => {
            var vm = this;
            var bodyMesg = "";
            if (vm.tempSecurity.traders == undefined) {
                bodyMesg = 'Please Select Trader From List';
            }
            if (vm.tempSecurity.tradeType == undefined) {
                bodyMesg = bodyMesg + "<br>" + 'Please Select Trade Type From List';
            }
            if (vm.tempSecurity.issuerId == undefined) {
                bodyMesg = bodyMesg + "<br>" + 'Please Select Issuer/Security From List';
            }
            if (vm.tempSecurity.facility == undefined) {
                bodyMesg = bodyMesg + "<br>" + 'Please Select Asset From List';
            }
            if (vm.tempSecurity.counterparty == undefined) {
                bodyMesg = bodyMesg + "<br>" + 'Please Select Counter Party From List';
            }
            if (vm.tempSecurity.totalQty == undefined) {
                bodyMesg = bodyMesg + "<br>" + 'Please Enter Total Quantity';
            }
            if (vm.tempSecurity.allocationRule == undefined) {
                bodyMesg = bodyMesg + "<br>" + 'Please Select Allocation Method From List';
            }
            if (bodyMesg != '') {
                var message: Models.IMessage = {
                    header: "Warning",
                    body: "<p><b>" + bodyMesg + "</b></p>"
                };
                vm.uiService.showMessage(message);
                return;
            }
            
            var TotalQty = vm.tempSecurity.totalQty;
            var TotalAllocatedQty = 0, tempOverride = 0;
            for (var _i = 0; _i < vm.gridOptions.data.length; _i++) {
                debugger;
                tempOverride = (vm.tempSecurity.allocationRule.ruleName.indexOf("Manual") > -1 ? parseFloat(vm.gridOptions.data[_i].override.toString()) : parseFloat(vm.gridOptions.data[_i].finalQty.toString()) - parseFloat(vm.gridOptions.data[_i].existing.toString()))
                TotalAllocatedQty = TotalAllocatedQty + tempOverride;
            }
            if (vm.tempSecurity.allocationRule.ruleName.indexOf("Sell All") > -1) {
                vm.tempSecurity.totalQty = TotalAllocatedQty;
                TotalQty = TotalAllocatedQty;
            }
            if (Math.round(TotalAllocatedQty) != TotalQty) {
                bodyMesg = 'Total Quantity (' + TotalQty + ') and Final Quantity  (' + Math.round(TotalAllocatedQty) + ') Should Match. ';
                var message: Models.IMessage = {
                    header: "Warning",
                    body: "<p><b>" + bodyMesg + "</b></p>"
                };
                vm.uiService.showMessage(message);
                return;
            }               

            vm.tempSecurity.tradeBookingDetail = vm.gridOptions.data;
            vm.statusText = "Saving";
            vm.isLoading = true;
            
            vm.dataService.generateTradeXML(vm.tempSecurity).then(data => {
                vm.clearAll(true);
                vm.dataService.getTradeBooking().then((trades) => {
                    vm.trades = trades;
                });
                bodyMesg = 'Data Saved Successfully.';
                var message: Models.IMessage = {
                    header: "Successfull Message",
                    body: "<p><b>" + bodyMesg + "</b></p>"
                };
                
                vm.isLoading = false;
            });

        }
        ShowResponse = (trade, row) => {
            var vm = this;
            //vm.isRowSelected = 'rowselected';            
            const date = new Date(trade.tradeDate);
            var bodyMesg = "";
            if (trade.responseStatus == null)
                bodyMesg = "<font color='red'> No response received yet.</font>";
            else
                bodyMesg = trade.errorMessage;
            var message: Models.IMessage = {
                header: "Response for " + trade.issuerDesc + "  (" + trade.tradeTypeDesc + ") " + date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate(),
                body: "<p><b>" + bodyMesg + "</b></p>"
            };
            vm.uiService.showMessage(message);
        }
        
    }

    angular.module('app').controller("application.controllers.tradebookingController", TradeBookingController);
}