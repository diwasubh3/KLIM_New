module Application.Controllers {
    export class BuySellTradeController {
        modalInstance: angular.ui.bootstrap.IModalServiceInstance;
        selectedIssuerId:number;
        appBasePath: string = pageOptions.appBasePath;
        isLoading: boolean = false;
        statusText: string = "Loading";
        tradeDetailsPath: string;
        selectedPosition: Models.IPosition;
        selectedFund: Models.IFund;
        calculatedTotalNewAllocation: number;
        calculatedTotalFinalAllocation: number;
        selectedSecurityId:number;
        securities: Array<Models.IVwSecurityDto>;
        selectedSecurity:Models.IVwSecurityDto;
        dataService: Application.Services.Contracts.IDataService;
        isTempLoanActive: boolean = false;
        isAddNewTradeActive: boolean = true;
        inSecuritySelectMode: boolean = false;
        sourceData: Models.ITradeSourceData;
        origTradeAllocations: Array<Models.ITradeAllocation>;
        ngTableParams: any;
        trade:Models.ITrade;
        tableParams: any;
        scope: ng.IScope;
        invalidTradeAmountAndTotalAllocation: boolean = false;
        fields: Array<Models.IField>;
        warning: string;
        dateOptions: any = {
            formatYear: 'yy',
            startingDay: 1
        };
        tempSecurity:Models.ITempSecurity;
        windowService: ng.IWindowService;

        static $inject = ["application.services.dataService", "$window", "$scope", "$uibModalInstance", 'NgTableParams','sourcedata'];
        
        constructor(dataService: Application.Services.Contracts.IDataService, $window: ng.IWindowService, $scope: angular.IScope, $modalInstance: angular.ui.bootstrap.IModalServiceInstance,
            ngTableParams: NgTableParams,sourcedata:any) {
            var vm = this;
            vm.tradeDetailsPath = pageOptions.appBasePath + 'app/views/addnewtrade-tradedetails.htm';
            vm.modalInstance = $modalInstance;
            vm.dataService = dataService;
            vm.scope = $scope;
            vm.windowService = $window;
            
            vm.ngTableParams = ngTableParams;
            
            if (!(sourcedata.trade && sourcedata.trade.tradeId))
            {
                vm.trade = <Models.ITrade>{};

                if (sourcedata.fund) {
                    vm.selectedFund = sourcedata.fund;
                }

                if (sourcedata.position)
                {
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
                    vm.tempSecurity = <Models.ITempSecurity>{};
                    vm.inSecuritySelectMode = true;
                }    
                
            }
            else {
                vm.trade = sourcedata.trade;
                vm.selectedSecurity = sourcedata.trade.security;
                vm.calculateTotalAllocations();
            }    
        }

        onTradeTypeChange = () => {
            var vm = this;
            if (typeof (vm.trade.tradeType) != 'undefined' && vm.trade.tradeType.toString().length) {
                vm.trade.isBuy = (parseInt(vm.trade.tradeType.toString()) === 0);
                if (vm.selectedPosition) {
                    vm.trade.bidOfferPrice = vm.trade.isBuy ? vm.selectedPosition.offer : vm.selectedPosition.bid;
                    vm.refreshFinalAllocations();
                }
            }
        }

        refreshFinalAllocations = () => {
            var vm = this;
            if (vm.trade.tradeAllocations && vm.trade.tradeAllocations.length) {
                vm.trade.tradeAllocations.forEach((ta: Application.Models.ITradeAllocation) => {
                    vm.calculateFinalAllocation(<Application.Models.ITradeAllocation>ta);
                });
                vm.calculateTotalAllocations();
            }
        }

        calculateFinalAllocation = (tradeallocation: Models.ITradeAllocation) => {
            var vm = this;
            if (typeof (tradeallocation.newAllocation) == 'undefined' || tradeallocation.newAllocation == null ||  !tradeallocation.newAllocation.toString().length) {
                tradeallocation.finalAllocation = parseFloat(tradeallocation.currentAllocation.toString().replace(/[^\d\.]/g, ''));
            } else {
                tradeallocation.finalAllocation = vm.trade.isBuy ? (parseFloat(tradeallocation.newAllocation.toString().replace(/[^\d\.]/g, '')) + parseFloat(tradeallocation.currentAllocation.toString().replace(/[^\d\.]/g, ''))) :
                    (parseFloat(tradeallocation.currentAllocation.toString().replace(/[^\d\.]/g, '')) - parseFloat(tradeallocation.newAllocation.toString().replace(/[^\d\.]/g, '')));
            }

            vm.calculateTotalAllocations();
        }

        calculateTotalAllocations = () => {
            var vm = this;
            vm.calculatedTotalNewAllocation = 0;
            vm.calculatedTotalFinalAllocation = 0;
            vm.trade.tradeAllocations.forEach(ta => {
                vm.calculatedTotalNewAllocation += parseFloat(ta.newAllocation ? ta.newAllocation.toString() : '0');
                vm.calculatedTotalFinalAllocation += parseFloat(ta.finalAllocation ? ta.finalAllocation.toString() : '0');
            });
            vm.checkForMismatchTradeAmountAndTotalAllocation();
        }

        checkForMismatchTradeAmountAndTotalAllocation = () => {
            var vm = this;
            var tradeamountObj = parseFloat(vm.trade.tradeAmount ? vm.trade.tradeAmount.toString().replace(/[^\d\.]/g, '') : '0');
            vm.invalidTradeAmountAndTotalAllocation = Math.round(tradeamountObj) != Math.round(vm.calculatedTotalNewAllocation);
        }

        calculateAllocation = (tradeallocation: Models.ITradeAllocation) => {
            var vm = this;
            if (typeof (tradeallocation.finalAllocation) == 'undefined' || !tradeallocation.finalAllocation.toString().length) {
                tradeallocation.newAllocation = null;
                return;
            }
            tradeallocation.newAllocation = vm.trade.isBuy ? (parseFloat(tradeallocation.finalAllocation.toString().replace(/[^\d\.]/g, '')) - parseFloat(tradeallocation.currentAllocation.toString().replace(/[^\d\.]/g, ''))) :
                (parseFloat(tradeallocation.currentAllocation.toString().replace(/[^\d\.]/g, '')) - parseFloat(tradeallocation.finalAllocation.toString().replace(/[^\d\.]/g, '')));

            vm.calculateTotalAllocations();
        }

		calculateAllAllocations = () => {
			var vm = this;
			vm.trade.tradeAllocations.forEach(ta => {
				vm.calculateAllocation(ta);
			});
		}

        sellAll = () => {
            var vm = this;

            if (vm.trade.sellAll) {

                vm.trade.tradeAllocations.forEach((tr:Models.ITradeAllocation) => {
                    tr.newAllocation = tr.currentAllocation;
                    vm.calculateFinalAllocation(<Models.ITradeAllocation>tr);
                });
                vm.trade.tradeAmount = vm.calculatedTotalNewAllocation;
                vm.checkForMismatchTradeAmountAndTotalAllocation();
            }
        }


        openDate = ($event: any, openProp: string, closeProp: string) => {
            var vm = this;
            $event.preventDefault();
            vm.tempSecurity[openProp] = true;
            vm.tempSecurity[closeProp] = false;
        };


        loadTradeAllocations = () => {
            var vm = this;
            vm.isLoading = true;
            vm.statusText = "Loading";

            vm.dataService.getTradeAllocations(vm.selectedSecurityId).then(tradeallocations => {
                vm.trade.tradeAllocations = tradeallocations;
                vm.origTradeAllocations = JSON.parse(JSON.stringify(tradeallocations));
                vm.isLoading = false;

                if (!vm.trade.bidOfferPrice && tradeallocations && tradeallocations.length && (tradeallocations[0].bid || tradeallocations[0].offer)) {
                    vm.trade.bidOfferPrice = vm.trade.isBuy ? (tradeallocations[0].offer.length ? parseFloat(tradeallocations[0].offer) : null) : (tradeallocations[0].bid.length? parseFloat(tradeallocations[0].bid):null);
                }
            });
        }

        loadTradeSourceData = () => {
            var vm = this;
            vm.isLoading = true;

            vm.dataService.getTradeSourceData().then(tradesourcedata => {
                vm.sourceData = tradesourcedata;
                vm.isLoading = false;
            });
        }

        cancel = () => {
            var vm = this;
            vm.statusText = "Closing";
            vm.modalInstance.dismiss('cancel');
        }

        clearAll = () => {
            var vm = this;
            vm.trade.comments = '';
            vm.trade.tradeAmount = undefined;
            vm.trade.tradePrice = undefined;
            vm.trade.keepOnBlotter = false;
            vm.trade.sellAll = false;
            vm.trade.tradeAllocations = JSON.parse(JSON.stringify(vm.origTradeAllocations));
            vm.calculatedTotalNewAllocation = undefined;
            vm.calculatedTotalFinalAllocation = undefined;
        }

        loadBloombergData = () => {
            var vm = this;
            
            if (vm.tempSecurity.securityCode) {
                vm.isLoading = true;
                vm.statusText = "Reading Bloomberg Data";
                vm.dataService.getBloombergData(vm.tempSecurity.securityCode).then((tempsecurity: Models.ITempSecurity) => {


                    vm.tempSecurity.bBGId = vm.tempSecurity.securityCode + " Corp";
                    vm.tempSecurity.gicsIndustry = tempsecurity.gicsIndustry;
                    vm.tempSecurity.maturityDate = tempsecurity.maturityDate;
                    vm.tempSecurity.callDate = tempsecurity.callDate;
                    vm.tempSecurity.spread = tempsecurity.spread;

                    if (tempsecurity.lienType && tempsecurity.lienType.lienTypeId) {
                        vm.tempSecurity.lienType = vm.sourceData.lienTypes.filter((lientype: Models.ILienType) => {
                            return lientype.lienTypeId === tempsecurity.lienType.lienTypeId;
                        })[0];
                    }

                    if (tempsecurity.facility && tempsecurity.facility.facilityId) {
                        var existingfacilities = vm.sourceData.facilities.filter((facility: Models.IFacility) => { return facility.facilityId === tempsecurity.facility.facilityId });
                        if (!existingfacilities.length) {
                            var newfacility = JSON.parse(JSON.stringify(tempsecurity.facility));
                            vm.sourceData.facilities.push(newfacility);
                            vm.tempSecurity.facility = newfacility;
                        } else {
                            vm.tempSecurity.facility = existingfacilities[0];
                        }
                        vm.tempSecurity.facility = vm.sourceData.facilities.filter((facility: Models.IFacility) => { return facility.facilityId === tempsecurity.facility.facilityId })[0];
                    }
                    
                    if (tempsecurity.issuer && tempsecurity.issuer.issuerId) {
                        var existingIssuers = vm.sourceData.issuers.filter((issuer: Models.IIssuer) => { return issuer.issuerId === tempsecurity.issuer.issuerId });
                        if (!existingIssuers.length) {
                            var newissuer = JSON.parse(JSON.stringify(tempsecurity.issuer));
                            vm.sourceData.issuers.push(newissuer);
                            vm.tempSecurity.issuer = newissuer;
                        } else {
                            vm.tempSecurity.issuer = existingIssuers[0];
                        }
                    }
                    vm.isLoading = false;
                });
            }
        }

        onSelectedSecurity = () => {
            var vm = this;
            if (vm.selectedSecurity) {
                vm.isLoading = true;
                vm.statusText = "Loading";
                vm.selectedSecurityId = vm.selectedSecurity.securityId;
                if (vm.selectedSecurityId && vm.selectedSecurity.securityCode) {
                    vm.dataService.getPositions(vm.selectedSecurity.securityCode, vm.sourceData.funds[0].fundCode).then(positions => {
                        if (positions.length) {
                            vm.selectedPosition = positions[0];
                        }
                        vm.loadTradeAllocations();
                    });

                } else {
                    vm.loadTradeAllocations();       
                }

                vm.trade.securityId = vm.selectedSecurityId;
                vm.onTradeTypeChange();
            }
        }

        onAddNewTradeSelected = () => {
            var vm = this;
            if (!vm.selectedSecurity && vm.tempSecurity.securityCode && vm.tempSecurity.issuer && vm.tempSecurity.facility && vm.tempSecurity.lienType) {
                vm.selectedSecurity = <Models.IVwSecurityDto>{};
                vm.selectedSecurity.securityName = 'LX_' + vm.tempSecurity.securityCode + " | " + vm.tempSecurity.issuer.issuerDesc + " | " + vm.tempSecurity.facility.facilityDesc;
                vm.selectedSecurity.securityId = 0;
                vm.onSelectedSecurity();
            }
               
        }

        activateAddNewTrade = () => {
            var vm = this;
            if (vm.tempSecurity.securityCode && vm.tempSecurity.issuer && vm.tempSecurity.facility && vm.tempSecurity.lienType) {
                vm.selectedSecurity = <Models.IVwSecurityDto>{};
                vm.selectedSecurity.securityName = 'LX_' + vm.tempSecurity.securityCode + " | " + vm.tempSecurity.issuer.issuerDesc + " | " + vm.tempSecurity.facility.facilityDesc;
                vm.selectedSecurity.securityId = 0;
                vm.onSelectedSecurity();
            }
            vm.isAddNewTradeActive = true;
        }

        saveTrade = () => {
			var vm = this;
            vm.dataService.saveTrades(vm.trade, !(vm.selectedPosition && !vm.inSecuritySelectMode)).then(result => {
                if (result) {
                    if (vm.selectedPosition && !vm.inSecuritySelectMode) {
                        vm.dataService.getPositions(vm.selectedPosition.securityCode, vm.selectedFund.fundCode).then(positions => {
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
        }

        save = () => {
            var vm = this;
            vm.isLoading = true;
            vm.statusText = "Saving";

	        vm.calculateAllAllocations();
			if (vm.invalidTradeAmountAndTotalAllocation) {
				vm.isLoading = false;
				return;
			}

	        if (!vm.selectedSecurity.securityId && vm.tempSecurity && vm.tempSecurity.securityCode) {
                vm.dataService.saveTempSecurity(vm.tempSecurity).then(security => {
                    vm.trade.security = security;
                    vm.trade.securityId = security.securityId;
                    vm.saveTrade();
                });
            } else {
                vm.saveTrade();
            }
        }

        cancelTrade = () => {
            var vm = this;
            vm.trade.isCancelled = true;
            vm.save();
        }
    }

    angular.module("app").controller("application.controllers.buySellTradeController", BuySellTradeController);
}