module Application.Controllers {
    export class Top10Bottom10Controller {
        dataService: Application.Services.Contracts.IDataService;
        uiService: Application.Services.Contracts.IUIService;

        rootScope: ng.IRootScopeService;
        
        isLoading: boolean;

        appBasePath: string = pageOptions.appBasePath;
        ngTableParams:any;
        statusText: string = "Loading";
        
        tableParamsRight: any;
        tableParamsLeft: any;
        
        modalService: angular.ui.bootstrap.IModalService;
        filter: ng.IFilterService;
        timeOutService: ng.ITimeoutService;
        rules: Array<Models.IRule>;
        selectedRule: Models.IRule;
        selectedFund: Models.ISummary;

        topFilterCollections: any;
        topFilterObj: {};

        bottomFilterCollections: any;
        bottomFilterObj: {};

        topFields: Array<Models.IField>;
        bottomFields: Array<Models.IField>;
        uiGridConstants: any;
        topGridApi: any;
        bottomGridApi: any;
        selectedGridRow: Models.IPosition;
        topGridOptions: any;
        bottomGridOptions: any;
        fixedFields: Array<Models.IField>;
        topPinnedSecurities: any;
        bottomPinnedSecurities: any;
        windowService: ng.IWindowService;
		customViews: Array<Models.ICustomView>;
        showMenu = (position: Models.IPosition, hasWatch: boolean) => {
            var vm = this;
            vm.selectedGridRow = position;
            var menus = [];
            if (position.hasBuyTrade) {
                menus.push(this.buyMenuOption);
            } else if (position.hasSellTrade) {
                menus.push(this.sellMenuOption);
            } else {
                menus.push(this.buyMenuOption);
                menus.push(this.sellMenuOption);
            }

            menus.push(null);

            //if (position.forCompare) {
            //    menus.push(this.removeFromLoanCompariosnMenuOption);
            //}
            //else {
            //    menus.push(this.addToLoanCompariosnMenuOption);
            //}    

            if (hasWatch) {
                menus.push(this.editWatchListMenuOption);
                menus.push(this.offWatchListMenuOption);
            } else {
                menus.push(this.onWatchListMenuOption);
            }

            return menus;
        }

        updatePositions = (updatedPositions: Array<Models.IPosition>) => {
            var vm = this;
            
            updatedPositions.forEach(up => {
                var changedPositions = vm.topGridOptions.data.filter(p => p.securityCode === up.securityCode);
                if (changedPositions.length) {
                    changedPositions[0].watchId = up.watchId;
                    changedPositions[0].watchObjectTypeId = up.watchObjectTypeId;
                    changedPositions[0].watchObjectId = up.watchObjectId;
                    changedPositions[0].isOnWatch = up.isOnWatch;
                    changedPositions[0].watchComments = up.watchComments;
                    changedPositions[0].watchUser = up.watchUser;
                    changedPositions[0].watchLastUpdatedOn = up.watchLastUpdatedOn;

                    changedPositions[0].hasBuyTrade = up.hasBuyTrade;
                    changedPositions[0].hasSellTrade = up.hasSellTrade;
                    changedPositions[0].trades = up.trades;
                    changedPositions[0].exposure = up.exposure;
                    changedPositions[0].totalPar = up.totalPar;
                    changedPositions[0].pctExposure = up.pctExposure;

                    changedPositions[0].clO1PctExposure = up.clO1PctExposure;
                    changedPositions[0].clO1NumExposure = up.clO1NumExposure;
                    changedPositions[0].clO2PctExposure = up.clO2PctExposure;
                    changedPositions[0].clO2NumExposure = up.clO2NumExposure;
                    changedPositions[0].clO3PctExposure = up.clO3PctExposure;
                    changedPositions[0].clO3NumExposure = up.clO3NumExposure;
                    changedPositions[0].clO4PctExposure = up.clO4PctExposure;
                    changedPositions[0].clO4NumExposure = up.clO4NumExposure;
                    changedPositions[0].clO1Exposure = up.clO1Exposure;
                    changedPositions[0].clO2Exposure = up.clO2Exposure;
                    changedPositions[0].clO3Exposure = up.clO3Exposure;
                    changedPositions[0].clO4Exposure = up.clO4Exposure;

                    vm.uiService.processTooltip(changedPositions[0]);
                }
                changedPositions = vm.bottomGridOptions.data.filter(p => p.securityCode === up.securityCode);
                if (changedPositions.length) {
                    changedPositions[0].watchId = up.watchId;
                    changedPositions[0].watchObjectTypeId = up.watchObjectTypeId;
                    changedPositions[0].watchObjectId = up.watchObjectId;
                    changedPositions[0].isOnWatch = up.isOnWatch;
                    changedPositions[0].watchComments = up.watchComments;
                    changedPositions[0].watchUser = up.watchUser;
                    changedPositions[0].watchLastUpdatedOn = up.watchLastUpdatedOn;

                    changedPositions[0].hasBuyTrade = up.hasBuyTrade;
                    changedPositions[0].hasSellTrade = up.hasSellTrade;
                    changedPositions[0].trades = up.trades;
                    changedPositions[0].exposure = up.exposure;
                    changedPositions[0].totalPar = up.totalPar;
                    changedPositions[0].pctExposure = up.pctExposure;

                    changedPositions[0].clO1PctExposure = up.clO1PctExposure;
                    changedPositions[0].clO1NumExposure = up.clO1NumExposure;
                    changedPositions[0].clO2PctExposure = up.clO2PctExposure;
                    changedPositions[0].clO2NumExposure = up.clO2NumExposure;
                    changedPositions[0].clO3PctExposure = up.clO3PctExposure;
                    changedPositions[0].clO3NumExposure = up.clO3NumExposure;
                    changedPositions[0].clO4PctExposure = up.clO4PctExposure;
                    changedPositions[0].clO4NumExposure = up.clO4NumExposure;
                    changedPositions[0].clO1Exposure = up.clO1Exposure;
                    changedPositions[0].clO2Exposure = up.clO2Exposure;
                    changedPositions[0].clO3Exposure = up.clO3Exposure;
                    changedPositions[0].clO4Exposure = up.clO4Exposure;

                    vm.uiService.processTooltip(changedPositions[0]);
                }
            });
            vm.rootScope.$emit('refreshSummaries', null);
        }


        clearLoanComparisons = (positions: Array<Models.IPosition>) => {
            var vm = this;
            vm.bottomGridApi.selection.clearSelectedRows();
            vm.topGridApi.selection.clearSelectedRows();
        }

        comapreLoans = () => {
            var vm = this;
            var positionsForComparison = vm.bottomGridApi.selection.getSelectedRows().concat(vm.topGridApi.selection.getSelectedRows());
			if (positionsForComparison.length) {
				var views = 
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

        buyMenuOption = ['Buy', () => {
            var vm = this;
            if (vm.selectedGridRow) {
                vm.uiService.showBuySellModal(vm.selectedFund, vm.selectedGridRow, vm.modalService, true, vm.updatePositions);
            }
        }];

        sellMenuOption = ['Sell', () => {
            var vm = this;
            if (vm.selectedGridRow) {
                vm.uiService.showBuySellModal(vm.selectedFund, vm.selectedGridRow, vm.modalService, false, vm.updatePositions);
            }
        }];

        onWatchListMenuOption = [
            'On Watch List', () => {
                var vm = this;
                if (vm.selectedGridRow) {
					vm.uiService.showWatchModal(vm.uiService.createWatch(vm.selectedGridRow), vm.modalService, false, 1, vm.updatePositions);
                }
            }
        ];

        offWatchListMenuOption = ['Off Watch List', () => {
            var vm = this;
            if (vm.selectedGridRow) {
				vm.uiService.showWatchModal(vm.uiService.createWatch(vm.selectedGridRow), vm.modalService, true, 1, vm.updatePositions);
            }
        }];

        editWatchListMenuOption = ['Edit Watch List', () => {
            var vm = this;
            if (vm.selectedGridRow) {
				vm.uiService.showWatchModal(vm.uiService.createWatch(vm.selectedGridRow), vm.modalService, false, 1, vm.updatePositions);
            }
        }];

        addToLoanCompariosnMenuOption = ['Add To Loan Comparison', () => {
            var vm = this;
            if (vm.selectedGridRow) {
                vm.selectedGridRow.forCompare = true;
            }
        }];

        removeFromLoanCompariosnMenuOption = ['Remove From Loan Comparison', () => {
            var vm = this;
            vm.bottomGridApi.selection.clearSelectedRows();
            vm.topGridApi.selection.clearSelectedRows();
        }];


		static $inject = ["application.services.uiService", "application.services.dataService", "$rootScope", '$uibModal', '$filter', '$timeout', 'uiGridConstants','$window'];   
        constructor(uiService: Application.Services.Contracts.IUIService, dataService: Application.Services.Contracts.IDataService, $rootScope: ng.IRootScopeService, modalService: angular.ui.bootstrap.IModalService, $filter: ng.IFilterService, timeOutService: ng.ITimeoutService, uiGridConstants: any, windowService: ng.IWindowService) {
            var vm = this;
            vm.uiService = uiService;
            vm.windowService = windowService;
            vm.dataService = dataService;
            vm.rootScope = $rootScope;
            vm.rootScope.$emit('onActivated', 'top10bottom10');
            vm.modalService = modalService;
            vm.topGridOptions = {
                rowHeight: 20,
                enableFiltering: true,
                enablePinning: true,
                onRegisterApi(gridApi) {
                    vm.topGridApi = gridApi;
                },
                appScopeProvider: this,
                columnDefs: []
            };

            vm.bottomGridOptions = {
                rowHeight: 20,
                enableFiltering: true,
                enablePinning: true,
                onRegisterApi(gridApi) {
                    vm.bottomGridApi = gridApi;
                },
                appScopeProvider: this,
                columnDefs: []
            };


            vm.filter = $filter;
            vm.isLoading = true;
            vm.dataService.loadFixedFields().then((fixedfields) => {
                vm.fixedFields = fixedfields;
                vm.loadData();
            });

            
            vm.uiGridConstants = uiGridConstants;

            vm.rootScope.$on('onFundSelectionChanged', (event, data: Models.ISummary) => {
                
                vm.onFundChanged(data);
            });

            vm.rootScope.$on('onAutoRefresh', (event, data: Models.ISummary) => {
                vm.onFundChanged(data);
            });
        }

        filterBy = (columnIndex: number) => {
            var vm = this;
            if (!vm.topGridApi.grid.columns[columnIndex].filters[0].term) {
                vm.topGridApi.grid.columns[columnIndex].filters[0].term = true;
                vm.bottomGridApi.grid.columns[columnIndex].filters[0].term = true;
            } else {
                vm.topGridApi.grid.columns[columnIndex].filters[0].term = null;
                vm.bottomGridApi.grid.columns[columnIndex].filters[0].term = null;
            }
            vm.topGridApi.grid.refresh();
            vm.bottomGridApi.grid.refresh();
        }

        loadData = () => {
            
            var vm = this;
            vm.statusText = "Loading";
            vm.isLoading = true;
            vm.dataService.loadRules().then((d) => {
                vm.rules = d;
                if (d.length)
                {
                    vm.selectedRule = vm.rules[0];
                    if (vm.rootScope['selectedFund'])
                    {
                        vm.selectedFund = vm.rootScope['selectedFund'];
                    }
                    vm.onRuleChanged();    
                }
            }).then(() => {
	            vm.dataService.getCustomViews().then(views => {
		            vm.getSortedCustomViews(views);
	            });
            });
        }

        onRuleChanged = () => {
            var vm = this;
            
            if (vm.selectedRule) {

                vm.isLoading = true;
                vm.dataService.loadTop10Bottom10(vm.selectedFund, vm.selectedRule.ruleId).then((d) => {


                    if (vm.topGridOptions.data) {
                        vm.topPinnedSecurities = _.map(vm.topGridApi.selection.getSelectedRows(), "securityId");
                    }

                    if (vm.bottomGridOptions.data) {
                        vm.bottomPinnedSecurities = _.map(vm.bottomGridApi.selection.getSelectedRows(), "securityId");

                        
                    }

                    vm.topGridOptions.data = d.topPositions;
                    vm.bottomGridOptions.data = d.bottomPositions;

                    vm.windowService.setTimeout(() => {
                        d.topPositions.forEach(p => {
                            if (vm.topPinnedSecurities.indexOf(p.securityId) >= 0) {
                                vm.topGridApi.selection.selectRow(p);
                            }
                        });
                        d.bottomPositions.forEach(p => {
                            if (vm.bottomPinnedSecurities.indexOf(p.securityId) >= 0) {
                                vm.bottomGridApi.selection.selectRow(p);
                            }
                        });
                    });

                    vm.fixedFields[vm.fixedFields.length - 1].hidden = true;

                    vm.topFields = [].concat(vm.fixedFields);
                    vm.bottomFields = [].concat(vm.fixedFields);

                    vm.topFilterCollections = {};
                    vm.bottomFilterCollections = {};

                    vm.selectedRule.ruleFields.filter(r => r.ruleSectionType.ruleSectionName === "Top").forEach(rf => vm.topFields.push(rf.field));
                    vm.selectedRule.ruleFields.filter(r => r.ruleSectionType.ruleSectionName === "Bottom").forEach(rf => vm.bottomFields.push(rf.field));

                    vm.uiService.createCollectionFilters(vm.topGridOptions.data, vm, 'topFilterObj', 'topFilterCollections', vm.topFields);
                    vm.uiService.processSearchText(vm.topGridOptions.data, vm.topFields);

                    vm.uiService.createCollectionFilters(vm.bottomGridOptions.data, vm, 'bottomFilterObj', 'bottomFilterCollections', vm.bottomFields);
                    vm.uiService.processSearchText(vm.bottomGridOptions.data, vm.bottomFields);

                    vm.uiService.createColumnDefs(vm, 'topGridOptions', 'topFilterCollections', 'highlightFilteredHeader', vm.topFields);
                    vm.uiService.createColumnDefs(vm, 'bottomGridOptions', 'bottomFilterCollections', 'highlightFilteredHeader', vm.bottomFields);

                    vm.isLoading = false;
                });
            }
        
        }

        highlightFilteredHeader = (row, rowRenderIndex, col, colRenderIndex) => {
            if (col.filters[0].term) {
                return 'header-filtered';
            } else {
                return '';
            }
        };

        sortingAlgorithm = (a, b, rowA, rowB, direction) => {
            var vm = this;

            var nulls = vm.topGridApi.core.sortHandleNulls(a, b);
            if (nulls !== null) {
                return nulls;
            } else {
                var l = a ? a.toLowerCase() : '', m = b ? b.toLowerCase() : '';
                return l === m ? 0 : l > m ? 1 : -1;
            }
        }

        refreshGrids = () => {
            var vm = this;
            vm.bottomGridApi.grid.columns[3].filters[0].term = vm.topGridApi.grid.columns[3].filters[0].term;
            vm.topGridApi.grid.refresh();
            vm.bottomGridApi.grid.refresh();
        }


        sortingNumberAlgorithm = (a, b, rowA, rowB, direction) => {
            var vm = this;
            var nulls = vm.topGridApi.core.sortHandleNulls(a, b);
            if (nulls !== null) {
                return nulls;
            } else {
                var s3 = a ? a.toString().split([',']).join('') : '';
                var s4 = b ? b.toString().split(',').join('') : '';
                if (!(isNaN(s3) && isNaN(s4))) {
                    return Number(s3) - Number(s4);
                }
                return 0;
            }
        }

        sortingPercentageAlgorithm = (a, b, rowA, rowB, direction) => {
            var vm = this;
            var nulls = vm.topGridApi.core.sortHandleNulls(a, b);
            if (nulls !== null) {
                return nulls;
            } else {
                var s3 = a ? a.toString().split([' %']).join('') : '';
                var s4 = b ? b.toString().split(' %').join('') : '';
                if (!(isNaN(s3) && isNaN(s4))) {
                    return Number(s3) - Number(s4);
                }
                return 0;
            }
        }

        sortingDateAlgorithm = (a, b, rowA, rowB, direction) => {

            var vm = this;
            var nulls = vm.topGridApi.core.sortHandleNulls(a, b);
            if (nulls !== null) {
                return nulls;
            } else {
                var c: any = new Date(a);
                var d: any = new Date(b);
                return c - d;
            }
        }


        onFundChanged = (data: Models.ISummary) => {
            var vm = this;
            vm.selectedFund = data;
            if (vm.selectedRule.ruleId != 2 || !vm.topGridOptions.data)
            {
                vm.onRuleChanged();
            }

        }   

      
    }

    angular.module("app").controller("application.controllers.top10Bottom10Controller", Top10Bottom10Controller);
} 