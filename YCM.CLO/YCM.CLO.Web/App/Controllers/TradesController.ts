module Application.Controllers {
    export class TradesController {
        dataService: Application.Services.Contracts.IDataService;
        uiService: Application.Services.Contracts.IUIService;
        rootScope: ng.IRootScopeService;

        isLoading: boolean;
        data: Array<Application.Models.ITrade>;
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
        selectedRow:Models.ITrade;
		static $inject = ["application.services.uiService", "application.services.dataService", "$rootScope", '$uibModal', 'NgTableParams', '$filter', '$timeout'];

        constructor(uiService: Application.Services.Contracts.IUIService, dataService: Application.Services.Contracts.IDataService, $rootScope: ng.IRootScopeService, modalService: angular.ui.bootstrap.IModalService, ngTableParams: NgTableParams, $filter: ng.IFilterService, timeOutService: ng.ITimeoutService) {
            var vm = this;
            vm.dataService = dataService;
            vm.uiService = uiService;
            vm.rootScope = $rootScope;
            vm.rootScope.$emit('onActivated', 'trades');
            vm.modalService = modalService;
            vm.ngTableParams = ngTableParams;
            vm.filter = $filter;

            if (vm.rootScope['selectedFund']) {
                vm.selectedFund = vm.rootScope['selectedFund'];
            }
            
            vm.rootScope.$on('onFundSelectionChanged', (event, data: Models.ISummary) => {
                vm.selectedFund = data;
                if (!vm.selectedFund) {
                    vm.loadData();
                }
            });

            vm.rootScope.$on('onAutoRefresh', (event, data: Models.ISummary) => {
                vm.selectedFund = data;
                vm.loadData();
            });

            vm.isLoading = true;
            vm.loadHeaderFields();

        }

        getPositionFromTrade = (trade: Models.ITrade) => {
            
            var security: any = trade['security'];
            security.watchId = trade.watchId;
            security.isOnWatch = trade.isOnWatch;
            security.watchObjectTypeId = trade.watchObjectTypeId;
            security.watchObjectId = trade.watchObjectId;
            security.watchComments = trade.watchComments;
            security.watchLastUpdatedOn = trade.watchLastUpdatedOn;
            security.watchUser = trade.watchUser;

            return security;
        }

        showMenu = (trade: Models.ITrade) => {
            var vm = this;
            vm.selectedRow = trade;
            var menus = [];

            if (trade.isBuy) {
                menus.push(this.buyMenuOption);
            } else if (trade.isSell) {
                menus.push(this.sellMenuOption);
            }

            menus.push(null);

            //if (trade.forCompare) {
            //    menus.push(this.removeFromLoanCompariosnMenuOption);
            //}
            //else {
            //    menus.push(this.addToLoanCompariosnMenuOption);
            //}    

            if (trade.isOnWatch) {
                menus.push(this.editWatchListMenuOption);
                menus.push(this.offWatchListMenuOption);
            } else {
                menus.push(this.onWatchListMenuOption);
            }

            return menus;
        }

        buyMenuOption: any = ['Buy', () => {
            var vm = this;
            if (vm.selectedRow) {
                vm.uiService.showBuySellModal(vm.selectedFund, vm.getPositionFromTrade(vm.selectedRow), vm.modalService, true, vm.loadDataWithSummaryRefresh);
            }
        }];

        sellMenuOption = ['Sell', () => {
            var vm = this;
            if (vm.selectedRow) {
                vm.uiService.showBuySellModal(vm.selectedFund, vm.getPositionFromTrade(vm.selectedRow), vm.modalService, false, vm.loadDataWithSummaryRefresh);
            }
        }];

        addToLoanCompariosnMenuOption = ['Add To Loan Comparison', () => {
            var vm = this;
            if (vm.selectedRow) {
                vm.selectedRow.forCompare = true;
            }
        }];

        select = (trade: Models.ITrade) => {
            trade.forCompare = !trade.forCompare;
        }
        

        removeFromLoanCompariosnMenuOption = ['Remove From Loan Comparison', () => {
            var vm = this;
            if (vm.selectedRow) {
                vm.selectedRow.forCompare = false;
            }
        }];

        onWatchListMenuOption = [
            'On Watch List', () => {
                var vm = this;
                if (vm.selectedRow) {
					vm.uiService.showWatchModal(vm.getPositionFromTrade(vm.selectedRow), vm.modalService, false, 1, vm.updateTradeFromPosition);
                }
            }
        ];

        offWatchListMenuOption = ['Off Watch List', () => {
            var vm = this;
            if (vm.selectedRow) {
				vm.uiService.showWatchModal(vm.getPositionFromTrade(vm.selectedRow), vm.modalService, true, 1, vm.updateTradeFromPosition);
            }
        }];

        editWatchListMenuOption = ['Edit Watch List', () => {
            var vm = this;
            if (vm.selectedRow) {
				vm.uiService.showWatchModal(vm.getPositionFromTrade(vm.selectedRow), vm.modalService, false, 1, vm.updateTradeFromPosition);
            }
        }];


        updateTradeFromPosition = (updatedPositions: Array<Models.IPosition>) => {
            var vm = this;
            
            if (updatedPositions.length) {
                updatedPositions.forEach(position => {
                vm.data.forEach(trade => {
                    if (trade.security.securityCode == position.securityCode) {
                        trade.isOnWatch = position.isOnWatch;
                        trade.watchId = position.watchId;
                        trade.watchObjectTypeId = position.watchObjectTypeId;
                        trade.watchObjectId = position.watchObjectId;
                        trade.watchComments = position.watchComments;
                        trade.watchLastUpdatedOn = position.watchLastUpdatedOn;
                        trade.watchUser = position.watchUser;
                        vm.uiService.processTooltip(trade);
                    }
                    });
                });
            }
        }

        clearLoanComparisons = (positions: Array<Models.IPosition>) => {
            var vm = this;
            vm.data.filter((f: Models.ITrade) => { return f.forCompare; }).forEach(p => { p.forCompare = false; })
        }

        comapreLoans = () => {
            var vm = this;
            var positionsForComparison = vm.data.filter((f: Models.ITrade) => { return f.forCompare; });
            if (positionsForComparison.length) {
                vm.uiService.showLoanComparisonModal(vm.rootScope['selectedFund'], -1, positionsForComparison, vm.customViews, vm.modalService, vm.clearLoanComparisons);
            }
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

        loadHeaderFields = () => {
            var vm = this;
            vm.isLoading = true;
            vm.statusText = "Loading";
            vm.dataService.getTradesHeaderFields().then(headers => {
                vm.headerFields = headers;
                vm.loadData();
            });
        }

        editTrade = (trade: Models.ITrade) => {
            var vm = this;
            var tr = trade ? trade : <Models.ITrade> { tradeAllocations:[] };
            vm.uiService.showTradeModel(tr, vm.modalService, vm.resetTrade);
        }

        resetTrade = (trade: Models.ITrade) => {
            var vm = this;
            var trades = vm.data.filter(tr => tr.tradeId == trade.tradeId);
            if (trades.length) {
                var tradeIndex = vm.data.indexOf(trades[0]);

                if (trade.isCancelled && !vm.includeCancelled) {
                    vm.data.splice(tradeIndex, 1);
                }
                else {
                    trade['$showRows'] = vm.data[tradeIndex]['$showRows'];
                    vm.data[tradeIndex] = trade;
                }
            }
            else {
                vm.data.push(trade);
            }
            
            vm.setParamsTable();    
            vm.rootScope.$emit('refreshSummaries', null);
        }

        loadData = () => {
            var vm = this;
            vm.statusText = "Loading";
            vm.isLoading = true;
            if (vm.selectedFund) {
                vm.dataService.getTrades(vm.includeCancelled, vm.selectedFund.fundCode).then((d) => {

                    if (vm.data)
                    {
                        var pinnedSecurities = _.map(vm.data.filter(p => { return p.forCompare; }), "securityId");

                        d.forEach(p => {
                            if (pinnedSecurities.indexOf(p.securityId) >= 0) {
                                p.forCompare = true;
                            }
                        });    
                   }

                    vm.data = d;



                    vm.data.forEach(t => {
                        vm.uiService.processTooltip(t);
                    });
                    
                    vm.isLoading = false;
                    vm.setParamsTable();
                }).then(() => {
	                vm.dataService.getCustomViews().then(views => {
		                vm.getSortedCustomViews(views);
	                });
                });
            } else {
                vm.isLoading = false;
            }    
        }

        loadDataWithSummaryRefresh = () => {
            var vm = this;
            vm.rootScope.$emit('refreshSummaries', null);
            vm.loadData();
        }

        sortTableParams = (columnName: string) => {
            var vm = this;
            var sortObj = {};
            sortObj[columnName] = vm.tableParams.isSortBy(columnName, 'asc') ? 'desc' : 'asc';
            vm.tableParams.sorting(sortObj);
        }

        getStyle = (column: Models.IField, data: Models.ISecurityOverride,index:number) => {
            var style = {};
            style['width'] = column.displayWidth;
            if (data) {
                if (index == 0) {
                    style['border-width'] = '0 0 0 0';
                }
                else    
                {
                    style['border-width'] = '1px 0 0px 0';
                }
                
            }
            if (data && data.isHistorical) {
                style['color'] = 'lightgray';
            }

            if (column.jsonPropertyName != 'securityName')
            {
                style['text-align'] = 'right';
            }

            return style;
        }

        resetFilter = (prop: string) => {
            var vm = this;
            if (vm.tableParams.filter()[prop]) {
                vm.tableParams.filter()[prop] = undefined;
            }
            else {
                vm.tableParams.filter()[prop] = true;
            }
        }


        setParamsTable = () => {
            var vm = this;

            var filterSearchText = (data, filterValues) => {
                var filtered = data;
                if (filterValues.searchText) {
                    filterValues.searchText = filterValues.searchText.toLowerCase();
                    filtered = filtered.filter(item => {
                        item.tradeAllocations.forEach(s => {
                            s['isVisible'] = !filterValues.searchText || s.searchText.indexOf(filterValues.searchText) >= 0;
                        });
                        return item.tradeAllocations.filter(s => { return s['isVisible'] }).length;
                    });
                }

                if (filterValues.isOnWatch) {
                    filtered = filtered.filter(item => {
                        return item.isOnWatch == true;
                    });
                }

                if (filterValues.isOnAlert) {
                    filtered = filtered.filter(item => {
                        return item.isOnAlert == true;
                    });
                }

                if (filterValues.isBuy) {
                    filtered = filtered.filter(item => {
                        return item.isBuy == true;
                    });
                }

                if (filterValues.isSell) {
                    filtered = filtered.filter(item => {
                        return item.isSell == true;
                    });
                }


                return filtered;
            }
            vm.tableParams = new vm.ngTableParams({
                page: 1,
                noPager: true,
                count: 10000,
                filtering: {
                    searchText: ''
                },
                sorting: {
                    'security.securityName': 'asc'
                }
            }, {
                    total: 1,
                    counts: [],
                    dataset: vm.data,
                    filterOptions: { filterFn: filterSearchText }
                });
        }

    }

    angular.module("app").controller("application.controllers.tradesController", TradesController);
} 