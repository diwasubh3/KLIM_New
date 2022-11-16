var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var TradeBookingController = (function () {
            function TradeBookingController(uiService, dataService, $rootScope, ngTableParams, $filter, $scope, uiGridConstants, exportUiGridService) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                this.statusText = "Loading";
                this.includeCancelled = false;
                this.isCancelHide = true;
                this.gridHeight = { 'height': '402px' };
                this.check = false;
                this.isStartTradeHide = true;
                this.hasAdminPermission = false;
                this.ConvertToCurrency = function (elem) {
                    if (elem != undefined && elem != null && elem.currentTarget != undefined) {
                        var val = elem.currentTarget.value;
                        if (val != null && val.length > 0) {
                            var returnVal = parseFloat(val.replace(/,/g, ""))
                                .toFixed(2)
                                .replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                            elem.currentTarget.value = returnVal;
                        }
                        else {
                            elem.currentTarget.value = 0;
                        }
                    }
                    else
                        elem.currentTarget.value = 0;
                };
                this.rowHighilited = function (row) {
                    var vm = _this;
                    vm.isRowSelected = row;
                };
                this.tradeTypeChangeEvent = function (tradeType) {
                    var vm = _this;
                    vm.tempSecurity.settlemethods = undefined;
                    vm.isDisabledSettlement = false;
                    vm.isTradeReasonHide = true;
                    if (tradeType.tradeTypeId == 2) {
                        vm.tempSecurity.settlemethods = { methodName: 'Assignment', methodId: 1 };
                        vm.isDisabledSettlement = true;
                        vm.isTradeReasonHide = false;
                    }
                    vm.loadAllocationRule(tradeType);
                };
                this.clearAll = function (newBooking) {
                    var vm = _this;
                    vm.isRowSelected = undefined;
                    vm.tempSecurity.tradeDate = new Date();
                    vm.tempSecurity.tradeType = { tradeTypeDesc: 'Buy', tradeTypeId: 1 };
                    vm.loadAllocationRule(vm.tempSecurity.tradeType);
                    vm.tempSecurity.issuerDesc = '';
                    vm.tempSecurity.issuerId = undefined;
                    vm.tempSecurity.loanXId = '';
                    vm.tempSecurity.issuer = undefined;
                    vm.tempSecurity.facility = undefined;
                    vm.tempSecurity.counterparty = undefined;
                    vm.tempSecurity.settlemethods = undefined;
                    vm.tempSecurity.totalQty = undefined;
                    vm.tempSecurity.price = undefined;
                    vm.tempSecurity.allocationRule = undefined;
                    vm.tempSecurity.tradeComments1 = undefined;
                    vm.tempSecurity.tradeComments2 = undefined;
                    vm.tempSecurity.tradeReasons = undefined;
                    vm.tempSecurity.selectedSecurity = '';
                    vm.gridOptions.data = [];
                    vm.TradeBookingIsOpen = true;
                    vm.ViewIsOpen = false;
                    vm.rootScope.$emit('onVisibilityChanged', open);
                    vm.isDisabled = false;
                    vm.isDisabledSettlement = false;
                    vm.isHide = true;
                    vm.isColumnHide = true;
                    vm.isTradeReasonHide = true;
                    vm.isCancelHide = true;
                    if (vm.hasAdminPermission) {
                        vm.isStartTradeHide = false;
                    }
                    vm.isRowDisabled = false;
                    vm.isCommentsDisabled = false;
                    var element = document.getElementById("includedall");
                    element.checked = false;
                };
                this.getNewLoanXId = function () {
                    var vm = _this;
                    vm.tempSecurity.tradeDate = new Date();
                    vm.tempSecurity.tradeType = { tradeTypeDesc: 'Buy', tradeTypeId: 1 };
                    vm.tempSecurity.issuerDesc = '';
                    vm.tempSecurity.loanXId = '';
                    vm.tempSecurity.facility = undefined;
                    vm.tempSecurity.counterparty = undefined;
                    vm.tempSecurity.settlemethods = undefined;
                    vm.tempSecurity.totalQty = undefined;
                    vm.tempSecurity.price = undefined;
                    vm.loadAllocationRule(vm.tempSecurity.tradeType);
                    vm.tempSecurity.allocationRule = undefined;
                    vm.tempSecurity.tradeComments1 = undefined;
                    vm.tempSecurity.tradeComments2 = undefined;
                    vm.tempSecurity.tradeReasons = undefined;
                    vm.tempSecurity.selectedSecurity = '';
                    vm.gridOptions.data = [];
                    vm.TradeBookingIsOpen = true;
                    vm.ViewIsOpen = false;
                    vm.rootScope.$emit('onVisibilityChanged', open);
                    vm.isDisabled = false;
                    vm.isDisabledSettlement = false;
                    vm.isHide = true;
                    vm.isColumnHide = false;
                    vm.isTradeReasonHide = true;
                    vm.isCancelHide = true;
                    vm.isRowDisabled = false;
                    vm.isCommentsDisabled = false;
                };
                this.setTradeBooking = function (tradeId) {
                    var vm = _this;
                    vm.isRowhightlight = true;
                    vm.dataService.refreshTradeBooking(tradeId).then(function (data) {
                        data.tradeDate = new Date(data.tradeDate);
                        vm.tradeTypeChangeEvent(data.tradeType);
                        vm.tempSecurity = data;
                        vm.tradebookingdetail = data.tradeBookingDetail;
                        vm.gridOptions.data = data.tradeBookingDetail;
                        vm.tempSecurity.selectedSecurity = data.loanXId + ' ' + data.issuerDesc;
                        vm.setColumnVisibility(vm.tempSecurity);
                        vm.isRowDisabled = true;
                        vm.isCommentsDisabled = true;
                        vm.isDisabledSettlement = true;
                        vm.isDisabled = true;
                        vm.isHide = true;
                        vm.isLoading = false;
                        if (data.responseStatus == "Complete" && vm.hasAdminPermission) {
                            vm.isCancelHide = false;
                        }
                    });
                };
                this.checkSaveButton = function () {
                    var vm = _this;
                    var TotalQty = vm.tempSecurity.totalQty;
                    var TotalAllocatedQty = 0;
                    var isfinalNegative = false;
                    var isOverrideNegative = false;
                    vm.errorMessage = '';
                    for (var _i = 0; _i < vm.gridOptions.data.length; _i++) {
                        var tempOverride = 0;
                        if (vm.tempSecurity.tradeType.tradeTypeDesc == "Buy") {
                            if (vm.tempSecurity.allocationRule.ruleName.indexOf("Manual") > -1)
                                tempOverride = parseFloat(vm.gridOptions.data[_i].override.toString());
                            else if (vm.gridOptions.data[_i].isIncluded == true)
                                tempOverride = parseFloat(vm.gridOptions.data[_i].netPosition.toString());
                            //else
                            //    tempOverride = parseFloat(vm.gridOptions.data[_i].finalQty.toString());
                            if (parseFloat(vm.gridOptions.data[_i].override.toString()) < 0)
                                isOverrideNegative = true;
                        }
                        else {
                            tempOverride = parseFloat(vm.gridOptions.data[_i].netPosition.toString());
                            if (parseFloat(vm.gridOptions.data[_i].finalQty.toString()) < 0 && parseFloat(vm.gridOptions.data[_i].finalQty.toString()).toFixed(2) != "-0.00")
                                isfinalNegative = true;
                        }
                        TotalAllocatedQty = TotalAllocatedQty + tempOverride;
                    }
                    if (vm.tempSecurity.allocationRule.ruleName.indexOf("Sell All") > -1) {
                        vm.tempSecurity.totalQty = parseFloat(Math.abs(TotalAllocatedQty).toFixed(2));
                        TotalQty = Math.abs(TotalAllocatedQty);
                    }
                    var message = "";
                    if (isfinalNegative)
                        message = 'User Can Not Sell More Than Existing Position';
                    if (isOverrideNegative) {
                        if (message.trim() != '')
                            message = message + ';   User Can Not Enter Negative Values';
                        else
                            message = 'User Can Not Enter Negative Values';
                    }
                    vm.errorMessage = message;
                    if (Math.round(Math.abs(TotalAllocatedQty)).toFixed(2) != Math.round(TotalQty).toFixed(2) || isOverrideNegative == true || isfinalNegative)
                        vm.isSaveDisabled = true;
                    else {
                        vm.isSaveDisabled = false;
                    }
                };
                this.onRowCheckALL = function () {
                    var vm = this;
                    var element = document.getElementById("includedall");
                    var isChecked = element.checked;
                    if (isChecked == true) {
                        for (var _i = 0; _i < vm.gridOptions.data.length; _i++) {
                            vm.gridOptions.data[_i].isIncluded = true;
                        }
                        vm.tradebookingdetail = vm.gridOptions.data;
                        for (var _i = 0; _i < vm.tradebookingdetail.length; _i++) {
                            vm.tradebookingdetail[_i].totalQuantity = vm.tempSecurity.totalQty;
                            vm.tradebookingdetail[_i].ruleName = vm.tempSecurity.allocationRule.ruleName;
                            vm.tradebookingdetail[_i].price = vm.tempSecurity.price;
                        }
                        vm.dataService.getCalculatedData(vm.tradebookingdetail).then(function (data) {
                            vm.gridOptions.data = data;
                            vm.checkSaveButton();
                            vm.isLoading = false;
                            var elchk = document.getElementById("includedall");
                            elchk.checked = true;
                            for (var _i = 0; _i < vm.gridOptions.data.length; _i++) {
                                //  vm.gridOptions.data[_i].isIncall = true;
                                vm.gridOptions.data[_i].isIncluded = true;
                            }
                        });
                    }
                    else {
                        for (var _i = 0; _i < vm.gridOptions.data.length; _i++) {
                            // vm.gridOptions.data[_i].isIncall = false;
                            vm.gridOptions.data[_i].isIncluded = false;
                        }
                        vm.tradebookingdetail = vm.gridOptions.data;
                        for (var _i = 0; _i < vm.tradebookingdetail.length; _i++) {
                            vm.tradebookingdetail[_i].totalQuantity = vm.tempSecurity.totalQty;
                            vm.tradebookingdetail[_i].ruleName = vm.tempSecurity.allocationRule.ruleName;
                            vm.tradebookingdetail[_i].price = vm.tempSecurity.price;
                        }
                        vm.dataService.getCalculatedData(vm.tradebookingdetail).then(function (data) {
                            vm.gridOptions.data = data;
                            vm.checkSaveButton();
                            vm.isLoading = false;
                            var elchk = document.getElementById("includedall");
                            elchk.checked = false;
                            for (var _i = 0; _i < vm.gridOptions.data.length; _i++) {
                                vm.gridOptions.data[_i].isIncluded = false;
                            }
                        });
                    }
                };
                this.onRowCheckChanged = function (row) {
                    var vm = this;
                    vm.tradebookingdetail = vm.gridOptions.data;
                    for (var _i = 0; _i < vm.tradebookingdetail.length; _i++) {
                        vm.tradebookingdetail[_i].totalQuantity = vm.tempSecurity.totalQty;
                        vm.tradebookingdetail[_i].ruleName = vm.tempSecurity.allocationRule.ruleName;
                        vm.tradebookingdetail[_i].price = vm.tempSecurity.price;
                    }
                    vm.dataService.getCalculatedData(vm.tradebookingdetail).then(function (data) {
                        vm.gridOptions.data = data;
                        vm.checkSaveButton();
                        vm.isLoading = false;
                    });
                    if (row.entity.isIncluded == false) {
                        var elchk = document.getElementById("includedall");
                        elchk.checked = false;
                    }
                    //let isAllselected = false, selectedcount = 0;;
                    //for (var _i = 0; _i < vm.gridOptions.data.length; _i++) {
                    //    if (vm.gridOptions.data[_i].isIncluded)
                    //        selectedcount++;
                    //}
                    //console.log("selectedcount:" + selectedcount);
                    //console.log("row.isIncluded:" + row.entity.isIncluded);
                    //if (selectedcount === vm.gridOptions.data.length) {
                    //    var elchk = <HTMLInputElement>document.getElementById("includedall");
                    //    elchk.checked = true;
                    //}
                    //else if (!row.entity.isIncluded) {
                    //        var elchk = <HTMLInputElement>document.getElementById("includedall");
                    //        elchk.checked = false;
                    //    }
                };
                this.OnCellLeave = function (row) {
                    var vm = this;
                    // alert('Called');
                    //vm.dataService.getCalculatedData(vm.gridOptions.data).then(data => {
                    //    vm.gridOptions.data = data;
                    //    vm.gridOptions.footerTemplate = '<div class="ui-grid-bottom-panel" style="text-align: center">I am a Custom Grid Footer</div>';
                    //    vm.isLoading = false;
                    //});
                };
                this.GetFundAllocation = function (allocation) {
                    var vm = _this;
                    vm.isLoading = true;
                    vm.isRowhightlight = false;
                    var bodyMesg = "";
                    if (vm.tempSecurity.tradeDate == undefined) {
                        bodyMesg = 'Please Select Trade Date';
                    }
                    //if (vm.tempSecurity.traders == undefined) {
                    //    bodyMesg = 'Please Select Trader From List';
                    //}
                    if (vm.tempSecurity.tradeType == undefined) {
                        bodyMesg = bodyMesg + "<br>" + 'Please Select Trade Type From List';
                    }
                    if (vm.isColumnHide == true) {
                        if (vm.tempSecurity.issuerId == undefined) {
                            bodyMesg = bodyMesg + "<br>" + 'Please Select Issuer/Security From List';
                        }
                    }
                    else {
                        //if (vm.tempSecurity.issuer == undefined) {
                        //    bodyMesg = bodyMesg + "<br>" + 'Please Select Issuer From List';
                        //}
                        //else {
                        //    vm.tempSecurity.issuerId = vm.tempSecurity.issuer.issuerId;
                        //    vm.tempSecurity.issuerDesc = vm.tempSecurity.issuer.issuerDesc;
                        //}
                    }
                    //if (vm.tempSecurity.facility == undefined) {
                    //    bodyMesg = bodyMesg + "<br>" + 'Please Select Asset From List';
                    //}
                    if (vm.tempSecurity.counterparty == undefined) {
                        bodyMesg = bodyMesg + "<br>" + 'Please Select Counter Party From List';
                    }
                    if (vm.tempSecurity.allocationRule == undefined) {
                        bodyMesg = bodyMesg + "<br>" + 'Please Select Allocation Method From List';
                    }
                    else {
                        if (allocation.allocationRule.ruleName.indexOf("Sell All") > -1) {
                            vm.tempSecurity.totalQty = 1;
                        }
                        else {
                            if (vm.tempSecurity.totalQty == undefined) {
                                bodyMesg = bodyMesg + "<br>" + 'Please Enter Total Quantity';
                            }
                        }
                    }
                    if (vm.tempSecurity.totalQty != null) {
                        var FinalQtyAmt = vm.tempSecurity.totalQty.toString().replace(/,/g, "");
                        vm.tempSecurity.totalQty = parseFloat(FinalQtyAmt);
                    }
                    ////convert to m and k
                    //var finalQty = ConvertToLong(vm.tempSecurity.totalQty);
                    //if (finalQty == "not valid") {
                    //    bodyMesg = bodyMesg + "<br>" + 'Qty is not valid.';
                    //}
                    //  else { vm.tempSecurity.totalQty = finalQty }
                    // var s =   convertTOM()
                    //if (vm.tempSecurity.totalQty == )
                    //
                    //  console.log(s);
                    //alert()
                    if (vm.tempSecurity.price == undefined || isNaN(Number(vm.tempSecurity.price.toString())) == true) {
                        bodyMesg = bodyMesg + "<br>" + 'Please Enter Price';
                    }
                    if (allocation.allocationRule.ruleName.indexOf("TargetPar") > -1) {
                        vm.gridApi.grid.columns[2].displayName = "Target Par";
                        vm.gridApi.grid.columns[3].displayName = "Existing Position";
                    }
                    else {
                        vm.gridApi.grid.columns[2].displayName = "Existing Position";
                        vm.gridApi.grid.columns[3].displayName = "Current Exposure";
                    }
                    if (bodyMesg != '') {
                        var message = {
                            header: "Warning",
                            body: "<p><b>" + bodyMesg + "</b></p>"
                        };
                        vm.uiService.showMessage(message);
                        return;
                    }
                    vm.dataService.getTradeFundAllocation(allocation).then(function (allocationdata) {
                        vm.tradebookingdetail = allocationdata;
                        vm.gridOptions.data = vm.tradebookingdetail;
                        vm.isRowDisabled = true;
                        vm.isCommentsDisabled = false;
                        vm.isDisabledSettlement = true;
                        vm.setColumnVisibility(allocation);
                        vm.isHide = false;
                        vm.isSaveDisabled = true;
                        vm.isLoading = false;
                        vm.checkSaveButton();
                    });
                };
                this.setColumnVisibility = function (tradebook) {
                    var vm = _this;
                    vm.gridApi.grid.columns[3].showColumn();
                    vm.gridApi.grid.columns[4].showColumn();
                    vm.gridApi.grid.columns[5].showColumn();
                    vm.gridApi.grid.columns[6].showColumn();
                    if (tradebook.allocationRule.ruleName.indexOf("Manual") > -1) {
                        vm.gridApi.grid.columns[3].hideColumn();
                        vm.gridApi.grid.columns[4].hideColumn();
                        vm.gridApi.grid.columns[6].hideColumn();
                    }
                    if (tradebook.allocationRule.ruleName.indexOf("Sell All") > -1) {
                        vm.gridApi.grid.columns[4].hideColumn();
                        vm.gridApi.grid.columns[5].hideColumn();
                    }
                    if (tradebook.allocationRule.ruleName.indexOf("Position") > -1) {
                        vm.gridApi.grid.columns[3].hideColumn();
                    }
                };
                this.refreshGrid = function () {
                    var vm = _this;
                    vm.gridApi.grid.refresh();
                };
                this.setFacility = function (sec) {
                    var vm = _this;
                    vm.tempSecurity.facility = { facilityDesc: sec.facilityDesc, facilityId: sec.facilityId };
                    vm.tempSecurity.loanXId = sec.securityCode;
                    vm.tempSecurity.issuerId = sec.issuerId;
                    vm.tempSecurity.issuerDesc = sec.issuer;
                };
                this.setAsset = function (sec) {
                    var vm = _this;
                    vm.tempSecurity.facility = { facilityDesc: sec.facilityDesc, facilityId: sec.facilityId };
                    vm.tempSecurity.issuerId = sec.issuerId;
                    vm.tempSecurity.issuerDesc = sec.issuer;
                };
                this.loadAllocationRule = function (tradeType) {
                    var vm = _this;
                    vm.dataService.getAllocationRule(tradeType.tradeTypeId).then(function (rules) {
                        vm.allocationRuleData = rules;
                    });
                };
                this.setIssuerDesc = function (sec) {
                    var vm = _this;
                    if (sec.issuerId == undefined) {
                        vm.tempSecurity.issuerId = 0;
                        vm.tempSecurity.issuerDesc = sec;
                    }
                    else {
                        vm.tempSecurity.issuerDesc = sec.issuer;
                    }
                };
                this.loadDropdownData = function () {
                    var vm = _this;
                    vm.statusText = "Loading";
                    vm.isLoading = true;
                    vm.dataService.getTradeBookingData().then(function (tradedata) {
                        vm.sourceData = tradedata;
                        vm.isLoading = false;
                        vm.dataService.getIssuerSecurities().then(function (securities) {
                            vm.securities = securities;
                        });
                        vm.dataService.getTradeBooking().then(function (trades) {
                            vm.trades = trades;
                        });
                        vm.dataService.getIssuerList().then(function (issuers) {
                            vm.issuers = issuers;
                        });
                        var permArray = vm.sourceData.permissions.filter(function (item) {
                            return item.toUpperCase() == "ADMIN";
                        });
                        if (permArray && permArray.length) {
                            vm.hasAdminPermission = true;
                            vm.isStartTradeHide = false;
                        }
                    });
                };
                this.GetTradeBookingHistory = function () {
                    var vm = _this;
                    vm.statusText = "Loading";
                    vm.isLoading = true;
                    vm.dataService.getTradeBookingHistory().then(function (alltrades) {
                        vm.alltrades = alltrades;
                        vm.isLoading = false;
                    });
                };
                this.GenerateTradeXML = function () {
                    var vm = _this;
                    vm.isSaveDisabled = true;
                    var bodyMesg = "";
                    if (vm.tempSecurity.tradeDate == undefined) {
                        bodyMesg = 'Please Select Trade Date';
                    }
                    //if (vm.tempSecurity.traders == undefined) {
                    //    bodyMesg = 'Please Select Trader From List';
                    //}
                    if (vm.tempSecurity.tradeType == undefined) {
                        bodyMesg = bodyMesg + "<br>" + 'Please Select Trade Type From List';
                    }
                    if (vm.isColumnHide == true) {
                        if (vm.tempSecurity.issuerId == undefined) {
                            bodyMesg = bodyMesg + "<br>" + 'Please Select Issuer/Security From List';
                        }
                    }
                    else {
                        //if (vm.tempSecurity.issuerId == undefined)
                        //    vm.tempSecurity.issuerDesc = vm.tempSecurity.issuer;
                        //if (vm.tempSecurity.issuer == undefined) {
                        //    bodyMesg = bodyMesg + "<br>" + 'Please Select Issuer From List';
                        //}
                        //else {
                        //    vm.tempSecurity.issuerId = vm.tempSecurity.issuer.issuerId;
                        //    vm.tempSecurity.issuerDesc = vm.tempSecurity.issuer.issuerDesc;
                        //}
                    }
                    //if (vm.tempSecurity.facility == undefined) {
                    //    bodyMesg = bodyMesg + "<br>" + 'Please Select Asset From List';
                    //}
                    if (vm.tempSecurity.counterparty == undefined) {
                        bodyMesg = bodyMesg + "<br>" + 'Please Select Counter Party From List';
                    }
                    if (vm.tempSecurity.totalQty == undefined) {
                        bodyMesg = bodyMesg + "<br>" + 'Please Enter Total Quantity';
                    }
                    if (vm.tempSecurity.allocationRule == undefined) {
                        bodyMesg = bodyMesg + "<br>" + 'Please Select Allocation Method From List';
                    }
                    else {
                        if (vm.tempSecurity.allocationRule.ruleName.indexOf("Sell All") > -1) {
                            vm.tempSecurity.totalQty = 0;
                        }
                        else {
                            if (vm.tempSecurity.totalQty == undefined) {
                                bodyMesg = bodyMesg + "<br>" + 'Please Enter Total Quantity';
                            }
                        }
                    }
                    if (vm.tempSecurity.tradeType.tradeTypeDesc == "Sell") {
                        if (vm.tempSecurity.tradeReasons == undefined) {
                            bodyMesg = bodyMesg + "<br>" + 'Please Select Trade Reason';
                        }
                    }
                    if (vm.tempSecurity.tradeComments1.comment == undefined) {
                        bodyMesg = bodyMesg + "<br>" + 'Please Select Trade Comment';
                    }
                    if (bodyMesg != '') {
                        var message = {
                            header: "Warning",
                            body: "<p><b>" + bodyMesg + "</b></p>"
                        };
                        vm.uiService.showMessage(message);
                        vm.isSaveDisabled = false;
                        return;
                    }
                    var TotalQty = vm.tempSecurity.totalQty;
                    var TotalAllocatedQty = 0;
                    for (var _i = 0; _i < vm.gridOptions.data.length; _i++) {
                        var tempOverride = 0;
                        if (vm.tempSecurity.allocationRule.ruleName.indexOf("Manual") > -1)
                            tempOverride = parseFloat(vm.gridOptions.data[_i].override.toString());
                        else if (vm.gridOptions.data[_i].isIncluded == true)
                            tempOverride = parseFloat(vm.gridOptions.data[_i].netPosition.toString());
                        //else
                        //    tempOverride = parseFloat(vm.gridOptions.data[_i].finalQty.toString());
                        TotalAllocatedQty = TotalAllocatedQty + tempOverride;
                    }
                    if (vm.tempSecurity.allocationRule.ruleName.indexOf("Sell All") > -1) {
                        vm.tempSecurity.totalQty = parseFloat(Math.abs(TotalAllocatedQty).toFixed(2));
                        TotalQty = TotalAllocatedQty;
                    }
                    if (Math.round(Math.abs(TotalAllocatedQty)).toFixed(2) != Math.round(Math.abs(TotalQty)).toFixed(2)) {
                        bodyMesg = 'Total Quantity (' + Number(TotalQty).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,') + ') and Final Allocated Quantity  (' + Math.round(TotalAllocatedQty).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,') + ') Should Match. ';
                        var message = {
                            header: "Warning",
                            body: "<p><b>" + bodyMesg + "</b></p>"
                        };
                        vm.uiService.showMessage(message);
                        vm.isSaveDisabled = false;
                        return;
                    }
                    vm.tempSecurity.tradeBookingDetail = vm.gridOptions.data;
                    vm.statusText = "Saving";
                    vm.isLoading = true;
                    vm.dataService.generateTradeXML(vm.tempSecurity).then(function (data) {
                        vm.clearAll(true);
                        vm.dataService.getTradeBooking().then(function (trades) {
                            vm.trades = trades;
                        });
                        bodyMesg = 'Data Saved Successfully.';
                        var message = {
                            header: "Successfull Message",
                            body: "<p><b>" + bodyMesg + "</b></p>"
                        };
                        vm.uiService.showMessage(message);
                        vm.isLoading = false;
                    });
                };
                this.getFilteredTrades = function () {
                    var vm = _this;
                    vm.isStartTradeHide = true;
                    vm.dataService.getFilteredTrades(vm.startDate.toLocaleDateString(), vm.endDate.toLocaleDateString()).then(function (data) {
                        vm.alltrades = data;
                        vm.isLoading = false;
                    });
                };
                this.setStartTradeVisibilty = function (visible) {
                    var vm = _this;
                    if (visible) {
                        vm.isStartTradeHide = visible;
                    }
                    else {
                        if (vm.hasAdminPermission) {
                            vm.isStartTradeHide = visible;
                        }
                    }
                };
                this.CancelTrade = function () {
                    var vm = _this;
                    vm.isSaveDisabled = true;
                    var bodyMesg = "";
                    if (vm.tempSecurity.counterparty == undefined) {
                        bodyMesg = bodyMesg + "<br>" + 'Please Select Counter Party From List';
                    }
                    if (bodyMesg != '') {
                        var message = {
                            header: "Warning",
                            body: "<p><b>" + bodyMesg + "</b></p>"
                        };
                        vm.uiService.showMessage(message);
                        vm.isSaveDisabled = false;
                        return;
                    }
                    vm.statusText = "Saving";
                    vm.isLoading = true;
                    vm.dataService.cancelTrade(vm.tempSecurity).then(function (data) {
                        vm.clearAll(true);
                        vm.dataService.getTradeBooking().then(function (trades) {
                            vm.trades = trades;
                        });
                        bodyMesg = 'Trade Cancel XML generated Successfully.';
                        var message = {
                            header: "Successfull Message",
                            body: "<p><b>" + bodyMesg + "</b></p>"
                        };
                        vm.uiService.showMessage(message);
                        vm.isLoading = false;
                    });
                };
                this.ShowResponse = function (trade, row) {
                    var vm = _this;
                    //vm.isRowSelected = 'rowselected';            
                    var date = new Date(trade.tradeDate);
                    var bodyMesg = "";
                    if (trade.responseStatus == null)
                        bodyMesg = "<font color='red'> No response received yet.</font>";
                    else
                        bodyMesg = trade.errorMessage;
                    var message = {
                        header: "Response for " + trade.issuerDesc + "  (" + trade.tradeTypeDesc + ") " + date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate(),
                        body: "<p><b>" + bodyMesg + "</b></p>"
                    };
                    vm.uiService.showMessage(message);
                };
                this.ExportToCSV = function () {
                    var vm = _this;
                    var activeTab = "alltrades";
                    var active = $("ul.nav.nav-tabs.tradesHistory li.active a").attr('href');
                    if (active === "#Current") {
                        activeTab = "trades";
                    }
                    if (vm[activeTab] && vm[activeTab].length > 0) {
                        var CsvData = [];
                        vm[activeTab].forEach(function (line) {
                            var reportDate = new Date(line.tradeDate);
                            var csvLine = {
                                tradeDate: reportDate.getDate() + "/" + (reportDate.getMonth() + 1) + "/" + reportDate.getFullYear(),
                                tradeTypeDesc: line.tradeTypeDesc,
                                issuerDesc: line.issuerDesc,
                                loanXId: line.loanXId,
                                counterparty: line.partyName,
                                settlementMethod: line.settleMethod,
                                allocationMethod: line.ruleName,
                                price: line.price,
                                cLO1Allocation: line.pendingSubmitQty.toFixed(2),
                                totalQty: line.totalQty,
                                responseStatus: line.responseStatus,
                                tradeComment1: line.tradeComment1,
                                tradeComment2: line.tradeComment2,
                                tradeReason: line.tradeReason
                            };
                            CsvData.push(csvLine);
                        });
                        vm.exportToCsv('tradebooking.csv', CsvData);
                    }
                };
                //ExportToPDF = () => {
                //    var vm = this;
                //    alert();
                //    debugger;
                //    var docDefinition = {
                //        pageOrientation: "Potrait",
                //        pageSize: "A4",
                //gripClickcontent: [{
                //            style: 'tableStyle',
                //            table: {
                //                headerRows: 1,                        
                //                body: vm.trades
                //            }
                //        }]
                //    };
                //    console.log(vm.exportUiGridService);
                //    vm.exportUiGridService.downloadPDF("tradebooking.pdf", docDefinition);
                //}
                this.exportToCsv = function (filename, rows) {
                    if (!rows || !rows.length) {
                        return;
                    }
                    var separator = ',';
                    var keys = Object.keys(rows[0]);
                    var csvContent = keys.join(separator) +
                        '\n' +
                        rows.map(function (row) {
                            return keys.map(function (k) {
                                var cell = row[k] === null || row[k] === undefined ? '' : row[k];
                                cell = cell instanceof Date
                                    ? cell.toLocaleString()
                                    : cell.toString().replace(/"/g, '""');
                                if (cell.search(/("|,|\n)/g) >= 0) {
                                    cell = "\"" + cell + "\"";
                                }
                                return cell;
                            }).join(separator);
                        }).join('\n');
                    var blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
                    if (navigator.msSaveBlob) {
                        navigator.msSaveBlob(blob, filename);
                    }
                    else {
                        var link = document.createElement('a');
                        if (link.download !== undefined) {
                            // Browsers that support HTML5 download attribute
                            var url = URL.createObjectURL(blob);
                            link.setAttribute('href', url);
                            link.setAttribute('download', filename);
                            link.style.visibility = 'hidden';
                            document.body.appendChild(link);
                            link.click();
                            document.body.removeChild(link);
                        }
                    }
                };
                this.ConvertAmount = function ConvertAmount(val) {
                    var vm = this;
                    ////convert to m and k
                    var bodyMesg = "";
                    //var x = <HTMLInputElement>document.getElementById("txtQuantity");
                    //alert(x.textContent)
                    var finalQty = ConvertToLong(val);
                    if (finalQty == "not valid") {
                        bodyMesg = bodyMesg + "<br>" + 'Qty is not valid.';
                    }
                    else {
                        //var b = <HTMLInputElement>document.getElementById("txtQuantity");
                        vm.tempSecurity.totalQty = finalQty;
                    }
                };
                var vm = this;
                var yesterday = new Date(new Date());
                yesterday.setDate(yesterday.getDate() - 1);
                var lastMonthdate = new Date(new Date());
                lastMonthdate.setDate(lastMonthdate.getDate() - 30);
                vm.startDate = lastMonthdate;
                vm.endDate = yesterday;
                vm.dataService = dataService;
                vm.uiService = uiService;
                vm.rootScope = $rootScope;
                vm.rootScope.$emit('onActivated', 'tradebooking');
                vm.ngTableParams = ngTableParams;
                vm.filter = $filter;
                vm.ViewIsOpen = true;
                vm.TradeBookingIsOpen = false;
                vm.isLoading = true;
                vm.isDisabled = false;
                vm.isDisabledSettlement = false;
                vm.isHide = true;
                vm.isColumnHide = true;
                vm.isTradeReasonHide = true;
                vm.isCancelHide = true;
                if (vm.hasAdminPermission) {
                    vm.isStartTradeHide = false;
                }
                vm.isSaveDisabled = true;
                vm.isRowDisabled = false;
                vm.isCommentsDisabled = false;
                vm.isRowhightlight = false;
                vm.loadDropdownData();
                vm.tempSecurity = {};
                vm.issuerSec = {};
                vm.tradebookingdetail = Array();
                vm.allocationRuleData = Array();
                vm.scope = $scope;
                vm.exportUiGridService = exportUiGridService;
                vm.tempSecurity.tradeDate = new Date();
                vm.tempSecurity.tradeType = { tradeTypeDesc: 'Buy', tradeTypeId: 1 };
                vm.tempSecurity.traders = { traderName: 'Eugene Koltunov', traderId: 1231 };
                vm.tempSecurity.interesttreatments = { description: 'Settles Without Accrued', id: 1 };
                vm.errorMessage = "";
                vm.loadAllocationRule(vm.tempSecurity.tradeType);
                vm.check = false;
                var tempFooter = '<div class="ui-grid-cell-contents;" style="text-align:right;padding-top:5px;">{{col.getAggregationValue() | number:2 }}</div>';
                vm.cDefs = [
                    {
                        name: 'isIncluded',
                        field: "isIncluded",
                        displayName: "Include",
                        headerCellTemplate: '<div class="ui-grid-cell-contents" ><input type="checkbox"  id="includedall" ng-model="check"  ng-change="grid.appScope.onRowCheckALL(check)"  ng-true-value="true" ng-false-value="false" /> Include All</div>',
                        //  type: "boolean",
                        enableCellEdit: false,
                        enableSorting: false,
                        cellEditableCondition: false,
                        cellTemplate: '<div class="ui-grid-cell-contents" ><input type="checkbox" ng-model="row.entity.isIncluded" ng-change="grid.appScope.onRowCheckChanged(row)"/></div>',
                        width: "10%",
                        cellClass: function (grid, row, col, rowRenderIndex, colRenderIndex) {
                            if (row.entity.isIncluded == true && row.entity.portfolioName == "York CLO-1 Ltd." && vm.isRowhightlight == true) {
                                return 'highlightrowleft';
                            }
                            return '';
                        },
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
                        footerCellTemplate: '<div class="ui-grid-cell-contents" style="padding-top:5px;">Total</div>',
                        cellClass: function (grid, row, col, rowRenderIndex, colRenderIndex) {
                            if (row.entity.isIncluded == true && row.entity.portfolioName == "York CLO-1 Ltd." && vm.isRowhightlight == true) {
                                return 'highlightrowleft';
                            }
                            return 'text-left';
                        },
                    },
                    {
                        name: 'existing',
                        field: "existing",
                        displayName: "Existing Position",
                        width: "10%",
                        visible: true,
                        enableCellEdit: false,
                        enableSorting: false,
                        headerCellClass: 'text-right',
                        type: "number",
                        cellFilter: 'number: 2',
                        aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                        footerCellTemplate: tempFooter,
                        cellClass: function (grid, row, col, rowRenderIndex, colRenderIndex) {
                            if (row.entity.isIncluded == true && row.entity.portfolioName == "York CLO-1 Ltd." && vm.isRowhightlight == true) {
                                return 'highlightrowright';
                            }
                            return 'text-right';
                        },
                    },
                    {
                        name: 'exposure',
                        field: "exposure",
                        displayName: "Current Exposure",
                        width: "10%",
                        visible: true,
                        enableCellEdit: false,
                        enableSorting: false,
                        headerCellClass: 'text-right',
                        type: "number",
                        cellFilter: 'number: 2',
                        aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                        footerCellTemplate: tempFooter,
                        cellClass: function (grid, row, col, rowRenderIndex, colRenderIndex) {
                            if (row.entity.isIncluded == true && row.entity.portfolioName == "York CLO-1 Ltd." && vm.isRowhightlight == true) {
                                return 'highlightrowright';
                            }
                            return 'text-right';
                        },
                    },
                    {
                        name: 'allocated',
                        field: "allocated",
                        displayName: 'Auto Allocated',
                        width: "10%",
                        visible: true,
                        enableCellEdit: false,
                        enableSorting: false,
                        type: "number",
                        cellFilter: 'number: 2',
                        headerCellClass: 'text-right',
                        aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                        footerCellTemplate: tempFooter,
                        cellClass: function (grid, row, col, rowRenderIndex, colRenderIndex) {
                            if (row.entity.isIncluded == true && row.entity.portfolioName == "York CLO-1 Ltd." && vm.isRowhightlight == true) {
                                return 'highlightrowright';
                            }
                            return 'text-right';
                        },
                    },
                    {
                        name: 'override',
                        field: "override",
                        displayName: 'Manual Override',
                        width: "10%",
                        visible: true,
                        enableCellEdit: true,
                        enableSorting: false,
                        type: "text",
                        headerCellClass: 'text-right',
                        enableCellEditOnFocus: true,
                        //cellTemplate: '<div class="ui-grid-cell-contents" ><input type="text" ng-model="row.entity.override" style="height: 20px !important;text-align:right" ng-change="grid.appScope.onChangeDemo(row)"/></div>',
                        /*cellTemplate: '<div><input type="INPUT_TYPE" style="height: 20px !important;text-align:right" ng-class="\'colt\' + col.uid"\ ui-grid-editor ng-model="row.entity.override" ng-blur="alert();"></div>',*/
                        cellEditableCondition: true,
                        cellFilter: 'number: 2',
                        aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                        footerCellTemplate: tempFooter,
                        cellClass: function (grid, row, col, rowRenderIndex, colRenderIndex) {
                            if (parseFloat(row.entity.finalQty) < 0 && parseFloat(row.entity.finalQty).toFixed(2) != "-0.00") {
                                return 'red';
                            }
                            if (row.entity.isIncluded == true && row.entity.portfolioName == "York CLO-1 Ltd." && vm.isRowhightlight == true) {
                                return 'highlightrowright';
                            }
                            return 'text-right';
                        },
                    },
                    {
                        name: 'netPosition',
                        field: "netPosition",
                        displayName: 'Net Allocation',
                        width: "10%",
                        visible: true,
                        enableCellEdit: false,
                        enableSorting: false,
                        headerCellClass: 'text-right',
                        type: "string",
                        cellFilter: 'number: 2',
                        aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                        footerCellTemplate: tempFooter,
                        cellClass: function (grid, row, col, rowRenderIndex, colRenderIndex) {
                            if (row.entity.isIncluded == true && row.entity.portfolioName == "York CLO-1 Ltd." && vm.isRowhightlight == true) {
                                return 'highlightrowright';
                            }
                            return 'text-right';
                        },
                    },
                    {
                        name: 'finalQty',
                        field: "finalQty",
                        displayName: 'Final Position',
                        width: "10%",
                        visible: true,
                        enableCellEdit: false,
                        enableSorting: false,
                        headerCellClass: 'text-right',
                        type: "string",
                        aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                        footerCellTemplate: tempFooter,
                        cellClass: function (grid, row, col, rowRenderIndex, colRenderIndex) {
                            if (parseFloat(row.entity.finalQty) < 0 && parseFloat(row.entity.finalQty).toFixed(2) != "-0.00") {
                                return 'red';
                            }
                            if (row.entity.isIncluded == true && row.entity.portfolioName == "York CLO-1 Ltd." && vm.isRowhightlight == true) {
                                return 'highlightrowright';
                            }
                            return 'text-right';
                        },
                        cellFilter: 'number: 2',
                        cellTooltip: 'Final quantity can not be less than zero'
                    },
                    {
                        name: 'tradeAmount',
                        field: "tradeAmount",
                        displayName: 'Net Amount',
                        width: "10%",
                        visible: true,
                        enableCellEdit: false,
                        enableSorting: false,
                        headerCellClass: 'text-right',
                        type: "string",
                        cellFilter: 'number: 2',
                        aggregationType: uiGridConstants.aggregationTypes.sum, aggregationHideLabel: true,
                        footerCellTemplate: tempFooter,
                        cellClass: function (grid, row, col, rowRenderIndex, colRenderIndex) {
                            if (row.entity.isIncluded == true && row.entity.portfolioName == "York CLO-1 Ltd." && vm.isRowhightlight == true) {
                                return 'highlightrowright';
                            }
                            return 'text-right';
                        },
                    }
                ];
                vm.gridOptions = {
                    columnDefs: vm.cDefs,
                    //showGridFooter: true,
                    enableSelectAll: true,
                    //multiSelect: true,
                    enableRowHeaderSelection: true,
                    showColumnFooter: true,
                    exporterCsvFilename: 'TradeBooking.csv',
                    exporterExcelFilename: 'TradeBooking',
                    rowHeight: 30,
                    onRegisterApi: function (gridApi) {
                        vm.gridApi = gridApi;
                        gridApi.edit.on.afterCellEdit($scope, function (rowEntity, colDef, newValue, oldValue) {
                            vm.tradebookingdetail = vm.gridOptions.data;
                            for (var _i = 0; _i < vm.tradebookingdetail.length; _i++) {
                                vm.tradebookingdetail[_i].totalQuantity = vm.tempSecurity.totalQty;
                                vm.tradebookingdetail[_i].ruleName = vm.tempSecurity.allocationRule.ruleName;
                                vm.tradebookingdetail[_i].price = vm.tempSecurity.price;
                                vm.tradebookingdetail[_i].tradeType = vm.tempSecurity.tradeType.tradeTypeDesc;
                                if (vm.tradebookingdetail[_i].override != null) {
                                    var override = ConvertToLong(vm.tradebookingdetail[_i].override);
                                    var FinalAmtOvr = override.toString().replace(/,/g, "");
                                    vm.tradebookingdetail[_i].override = parseFloat(FinalAmtOvr);
                                }
                                if (vm.tradebookingdetail[_i].override > 0)
                                    vm.tradebookingdetail[_i].isIncluded = true;
                            }
                            vm.dataService.getCalculatedData(vm.tradebookingdetail).then(function (data) {
                                vm.gridOptions.data = data;
                                vm.checkSaveButton();
                                vm.isLoading = false;
                            });
                        });
                    },
                };
                vm.gridOptions.appScopeProvider = vm;
            }
            TradeBookingController.prototype.onVisibilityChanged = function (open) {
                var vm = this;
                vm.rootScope.$emit('onVisibilityChanged', open);
            };
            return TradeBookingController;
        }());
        TradeBookingController.$inject = ["application.services.uiService", "application.services.dataService", "$rootScope", 'NgTableParams', '$filter', "$scope", 'uiGridConstants', 'uiGridExporterService'];
        Controllers.TradeBookingController = TradeBookingController;
        function ConvertToLong(strval) {
            // var strval = "";
            var si = [
                { s: "k" },
                { s: "m" }
            ];
            var result = "";
            for (var i = 0; i < si.length; i++) {
                var name = si[i];
                if (strval.toString().toLowerCase().includes(name.s) == true) {
                    if (name.s.toString().toLowerCase() == "m") {
                        var dval = strval.toString().toLowerCase().split("m");
                        result = (dval[0] * 1000000).toString();
                        var options = { style: 'currency', currency: 'USD', minimumFractionDigits: 4 };
                        return (new Intl.NumberFormat('en-US', options).format(parseFloat(result))).replace("$", "");
                        // break;
                    }
                    if (name.s.toString().toLowerCase() == "k") {
                        var dval = strval.toString().toLowerCase().split("k");
                        result = (dval[0] * 1000).toString();
                        var options = { style: 'currency', currency: 'USD', minimumFractionDigits: 4 };
                        return (new Intl.NumberFormat('en-US', options).format(parseFloat(result))).replace("$", "");
                        // break;
                    }
                }
            }
            if ($.isNumeric(strval)) {
                result = strval;
                var options = { style: 'currency', currency: 'USD', minimumFractionDigits: 4 };
                return (new Intl.NumberFormat('en-US', options).format(parseFloat(result))).replace("$", "");
                // return result;
            }
            else {
                return "not valid";
            }
        }
        angular.module('app').controller("application.controllers.tradebookingController", TradeBookingController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=TradeBookingController.js.map