var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var BuySellTradeController = (function () {
            function BuySellTradeController(dataService, $window, $scope, $modalInstance, ngTableParams, sourcedata) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                this.isLoading = false;
                this.statusText = "Loading";
                this.isTempLoanActive = false;
                this.isAddNewTradeActive = true;
                this.inSecuritySelectMode = false;
                this.invalidTradeAmountAndTotalAllocation = false;
                this.dateOptions = {
                    formatYear: 'yy',
                    startingDay: 1
                };
                this.onTradeTypeChange = function () {
                    var vm = _this;
                    if (typeof (vm.trade.tradeType) != 'undefined' && vm.trade.tradeType.toString().length) {
                        vm.trade.isBuy = (parseInt(vm.trade.tradeType.toString()) === 0);
                        if (vm.selectedPosition) {
                            vm.trade.bidOfferPrice = vm.trade.isBuy ? vm.selectedPosition.offer : vm.selectedPosition.bid;
                            vm.refreshFinalAllocations();
                        }
                    }
                };
                this.refreshFinalAllocations = function () {
                    var vm = _this;
                    if (vm.trade.tradeAllocations && vm.trade.tradeAllocations.length) {
                        vm.trade.tradeAllocations.forEach(function (ta) {
                            vm.calculateFinalAllocation(ta);
                        });
                        vm.calculateTotalAllocations();
                    }
                };
                this.calculateFinalAllocation = function (tradeallocation) {
                    var vm = _this;
                    if (typeof (tradeallocation.newAllocation) == 'undefined' || tradeallocation.newAllocation == null || !tradeallocation.newAllocation.toString().length) {
                        tradeallocation.finalAllocation = parseFloat(tradeallocation.currentAllocation.toString().replace(/[^\d\.]/g, ''));
                    }
                    else {
                        tradeallocation.finalAllocation = vm.trade.isBuy ? (parseFloat(tradeallocation.newAllocation.toString().replace(/[^\d\.]/g, '')) + parseFloat(tradeallocation.currentAllocation.toString().replace(/[^\d\.]/g, ''))) :
                            (parseFloat(tradeallocation.currentAllocation.toString().replace(/[^\d\.]/g, '')) - parseFloat(tradeallocation.newAllocation.toString().replace(/[^\d\.]/g, '')));
                    }
                    vm.calculateTotalAllocations();
                };
                this.calculateTotalAllocations = function () {
                    var vm = _this;
                    vm.calculatedTotalNewAllocation = 0;
                    vm.calculatedTotalFinalAllocation = 0;
                    vm.trade.tradeAllocations.forEach(function (ta) {
                        vm.calculatedTotalNewAllocation += parseFloat(ta.newAllocation ? ta.newAllocation.toString() : '0');
                        vm.calculatedTotalFinalAllocation += parseFloat(ta.finalAllocation ? ta.finalAllocation.toString() : '0');
                    });
                    vm.checkForMismatchTradeAmountAndTotalAllocation();
                };
                this.checkForMismatchTradeAmountAndTotalAllocation = function () {
                    var vm = _this;
                    var tradeamountObj = parseFloat(vm.trade.tradeAmount ? vm.trade.tradeAmount.toString().replace(/[^\d\.]/g, '') : '0');
                    vm.invalidTradeAmountAndTotalAllocation = Math.round(tradeamountObj) != Math.round(vm.calculatedTotalNewAllocation);
                };
                this.calculateAllocation = function (tradeallocation) {
                    var vm = _this;
                    if (typeof (tradeallocation.finalAllocation) == 'undefined' || !tradeallocation.finalAllocation.toString().length) {
                        tradeallocation.newAllocation = null;
                        return;
                    }
                    tradeallocation.newAllocation = vm.trade.isBuy ? (parseFloat(tradeallocation.finalAllocation.toString().replace(/[^\d\.]/g, '')) - parseFloat(tradeallocation.currentAllocation.toString().replace(/[^\d\.]/g, ''))) :
                        (parseFloat(tradeallocation.currentAllocation.toString().replace(/[^\d\.]/g, '')) - parseFloat(tradeallocation.finalAllocation.toString().replace(/[^\d\.]/g, '')));
                    vm.calculateTotalAllocations();
                };
                this.calculateAllAllocations = function () {
                    var vm = _this;
                    vm.trade.tradeAllocations.forEach(function (ta) {
                        vm.calculateAllocation(ta);
                    });
                };
                this.sellAll = function () {
                    var vm = _this;
                    if (vm.trade.sellAll) {
                        vm.trade.tradeAllocations.forEach(function (tr) {
                            tr.newAllocation = tr.currentAllocation;
                            vm.calculateFinalAllocation(tr);
                        });
                        vm.trade.tradeAmount = vm.calculatedTotalNewAllocation;
                        vm.checkForMismatchTradeAmountAndTotalAllocation();
                    }
                };
                this.openDate = function ($event, openProp, closeProp) {
                    var vm = _this;
                    $event.preventDefault();
                    vm.tempSecurity[openProp] = true;
                    vm.tempSecurity[closeProp] = false;
                };
                this.loadTradeAllocations = function () {
                    var vm = _this;
                    vm.isLoading = true;
                    vm.statusText = "Loading";
                    vm.dataService.getTradeAllocations(vm.selectedSecurityId).then(function (tradeallocations) {
                        vm.trade.tradeAllocations = tradeallocations;
                        vm.origTradeAllocations = JSON.parse(JSON.stringify(tradeallocations));
                        vm.isLoading = false;
                        if (!vm.trade.bidOfferPrice && tradeallocations && tradeallocations.length && (tradeallocations[0].bid || tradeallocations[0].offer)) {
                            vm.trade.bidOfferPrice = vm.trade.isBuy ? (tradeallocations[0].offer.length ? parseFloat(tradeallocations[0].offer) : null) : (tradeallocations[0].bid.length ? parseFloat(tradeallocations[0].bid) : null);
                        }
                    });
                };
                this.loadTradeSourceData = function () {
                    var vm = _this;
                    vm.isLoading = true;
                    vm.dataService.getTradeSourceData().then(function (tradesourcedata) {
                        vm.sourceData = tradesourcedata;
                        vm.isLoading = false;
                    });
                };
                this.cancel = function () {
                    var vm = _this;
                    vm.statusText = "Closing";
                    vm.modalInstance.dismiss('cancel');
                };
                this.clearAll = function () {
                    var vm = _this;
                    vm.trade.comments = '';
                    vm.trade.tradeAmount = undefined;
                    vm.trade.tradePrice = undefined;
                    vm.trade.keepOnBlotter = false;
                    vm.trade.sellAll = false;
                    vm.trade.tradeAllocations = JSON.parse(JSON.stringify(vm.origTradeAllocations));
                    vm.calculatedTotalNewAllocation = undefined;
                    vm.calculatedTotalFinalAllocation = undefined;
                };
                this.loadBloombergData = function () {
                    var vm = _this;
                    if (vm.tempSecurity.securityCode) {
                        vm.isLoading = true;
                        vm.statusText = "Reading Bloomberg Data";
                        vm.dataService.getBloombergData(vm.tempSecurity.securityCode).then(function (tempsecurity) {
                            vm.tempSecurity.bBGId = vm.tempSecurity.securityCode + " Corp";
                            vm.tempSecurity.gicsIndustry = tempsecurity.gicsIndustry;
                            vm.tempSecurity.maturityDate = tempsecurity.maturityDate;
                            vm.tempSecurity.callDate = tempsecurity.callDate;
                            vm.tempSecurity.spread = tempsecurity.spread;
                            if (tempsecurity.lienType && tempsecurity.lienType.lienTypeId) {
                                vm.tempSecurity.lienType = vm.sourceData.lienTypes.filter(function (lientype) {
                                    return lientype.lienTypeId === tempsecurity.lienType.lienTypeId;
                                })[0];
                            }
                            if (tempsecurity.facility && tempsecurity.facility.facilityId) {
                                var existingfacilities = vm.sourceData.facilities.filter(function (facility) { return facility.facilityId === tempsecurity.facility.facilityId; });
                                if (!existingfacilities.length) {
                                    var newfacility = JSON.parse(JSON.stringify(tempsecurity.facility));
                                    vm.sourceData.facilities.push(newfacility);
                                    vm.tempSecurity.facility = newfacility;
                                }
                                else {
                                    vm.tempSecurity.facility = existingfacilities[0];
                                }
                                vm.tempSecurity.facility = vm.sourceData.facilities.filter(function (facility) { return facility.facilityId === tempsecurity.facility.facilityId; })[0];
                            }
                            if (tempsecurity.issuer && tempsecurity.issuer.issuerId) {
                                var existingIssuers = vm.sourceData.issuers.filter(function (issuer) { return issuer.issuerId === tempsecurity.issuer.issuerId; });
                                if (!existingIssuers.length) {
                                    var newissuer = JSON.parse(JSON.stringify(tempsecurity.issuer));
                                    vm.sourceData.issuers.push(newissuer);
                                    vm.tempSecurity.issuer = newissuer;
                                }
                                else {
                                    vm.tempSecurity.issuer = existingIssuers[0];
                                }
                            }
                            vm.isLoading = false;
                        });
                    }
                };
                this.onSelectedSecurity = function () {
                    var vm = _this;
                    if (vm.selectedSecurity) {
                        vm.isLoading = true;
                        vm.statusText = "Loading";
                        vm.selectedSecurityId = vm.selectedSecurity.securityId;
                        if (vm.selectedSecurityId && vm.selectedSecurity.securityCode) {
                            vm.dataService.getPositions(vm.selectedSecurity.securityCode, vm.sourceData.funds[0].fundCode).then(function (positions) {
                                if (positions.length) {
                                    vm.selectedPosition = positions[0];
                                }
                                vm.loadTradeAllocations();
                            });
                        }
                        else {
                            vm.loadTradeAllocations();
                        }
                        vm.trade.securityId = vm.selectedSecurityId;
                        vm.onTradeTypeChange();
                    }
                };
                this.onAddNewTradeSelected = function () {
                    var vm = _this;
                    if (!vm.selectedSecurity && vm.tempSecurity.securityCode && vm.tempSecurity.issuer && vm.tempSecurity.facility && vm.tempSecurity.lienType) {
                        vm.selectedSecurity = {};
                        vm.selectedSecurity.securityName = 'LX_' + vm.tempSecurity.securityCode + " | " + vm.tempSecurity.issuer.issuerDesc + " | " + vm.tempSecurity.facility.facilityDesc;
                        vm.selectedSecurity.securityId = 0;
                        vm.onSelectedSecurity();
                    }
                };
                this.activateAddNewTrade = function () {
                    var vm = _this;
                    if (vm.tempSecurity.securityCode && vm.tempSecurity.issuer && vm.tempSecurity.facility && vm.tempSecurity.lienType) {
                        vm.selectedSecurity = {};
                        vm.selectedSecurity.securityName = 'LX_' + vm.tempSecurity.securityCode + " | " + vm.tempSecurity.issuer.issuerDesc + " | " + vm.tempSecurity.facility.facilityDesc;
                        vm.selectedSecurity.securityId = 0;
                        vm.onSelectedSecurity();
                    }
                    vm.isAddNewTradeActive = true;
                };
                this.saveTrade = function () {
                    var vm = _this;
                    vm.dataService.saveTrades(vm.trade, !(vm.selectedPosition && !vm.inSecuritySelectMode)).then(function (result) {
                        if (result) {
                            if (vm.selectedPosition && !vm.inSecuritySelectMode) {
                                vm.dataService.getPositions(vm.selectedPosition.securityCode, vm.selectedFund.fundCode).then(function (positions) {
                                    vm.isLoading = false;
                                    vm.modalInstance.close(positions);
                                });
                            }
                            else {
                                vm.isLoading = false;
                                vm.modalInstance.close(result);
                            }
                        }
                        else {
                            vm.isLoading = false;
                        }
                    });
                };
                this.save = function () {
                    var vm = _this;
                    vm.isLoading = true;
                    vm.statusText = "Saving";
                    vm.calculateAllAllocations();
                    if (vm.invalidTradeAmountAndTotalAllocation) {
                        vm.isLoading = false;
                        return;
                    }
                    if (!vm.selectedSecurity.securityId && vm.tempSecurity && vm.tempSecurity.securityCode) {
                        vm.dataService.saveTempSecurity(vm.tempSecurity).then(function (security) {
                            vm.trade.security = security;
                            vm.trade.securityId = security.securityId;
                            vm.saveTrade();
                        });
                    }
                    else {
                        vm.saveTrade();
                    }
                };
                this.cancelTrade = function () {
                    var vm = _this;
                    vm.trade.isCancelled = true;
                    vm.save();
                };
                var vm = this;
                vm.tradeDetailsPath = pageOptions.appBasePath + 'app/views/addnewtrade-tradedetails.htm';
                vm.modalInstance = $modalInstance;
                vm.dataService = dataService;
                vm.scope = $scope;
                vm.windowService = $window;
                vm.ngTableParams = ngTableParams;
                if (!(sourcedata.trade && sourcedata.trade.tradeId)) {
                    vm.trade = {};
                    if (sourcedata.fund) {
                        vm.selectedFund = sourcedata.fund;
                    }
                    if (sourcedata.position) {
                        vm.selectedPosition = sourcedata.position;
                        vm.selectedSecurity = sourcedata.position;
                        vm.selectedSecurityId = vm.selectedPosition.securityId;
                        vm.trade.isBuy = sourcedata.isBuy;
                        vm.trade.securityId = sourcedata.position.securityId;
                        vm.trade.bidOfferPrice = vm.trade.isBuy ? vm.selectedPosition.offer : vm.selectedPosition.bid;
                        vm.loadTradeAllocations();
                    }
                    else {
                        vm.loadTradeSourceData();
                        vm.tempSecurity = {};
                        vm.inSecuritySelectMode = true;
                    }
                }
                else {
                    vm.trade = sourcedata.trade;
                    vm.selectedSecurity = sourcedata.trade.security;
                    vm.calculateTotalAllocations();
                }
            }
            return BuySellTradeController;
        }());
        BuySellTradeController.$inject = ["application.services.dataService", "$window", "$scope", "$uibModalInstance", 'NgTableParams', 'sourcedata'];
        Controllers.BuySellTradeController = BuySellTradeController;
        angular.module("app").controller("application.controllers.buySellTradeController", BuySellTradeController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=BuySellTradeController.js.map