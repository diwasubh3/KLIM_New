
module Application.Controllers {
    export class PositionsController {

        static gridState: GridState;


        dataService: Application.Services.Contracts.IDataService;
        uiService: Application.Services.Contracts.IUIService;

        rootScope: ng.IRootScopeService;
	    scope: ng.IScope;

        isLoading: boolean;
        positions: Array<Application.Models.IPosition>;

        appBasePath: string = pageOptions.appBasePath;
        
        statusText: string = "Loading";
        
        modalService: angular.ui.bootstrap.IModalService;
        filter: ng.IFilterService;
        timeOutService: ng.ITimeoutService;
        
        fields: Array<Models.IField>;
        onlyWithExposures:boolean;
        selectedFund: Models.ISummary;
		issuerIds: Array<number>;

		filterFieldName: string = "IsFilterSuccess";
        selectedMatrixSpreadFund: Models.IFund;
        filterCollections: any;
        isFirstTimeLoading :boolean= true;
		windowService: ng.IWindowService;
		selectedView: string;
		positionViews: Array<string>;
		needsRefresh: boolean = false;
	    searchText: string;
        exportUiGridService: any;
        uiGridConstants: any;
        gridApi:any;
        selectedGridRow:Models.IPosition;
        gridOptions: any;
        positionFilter:Models.IPositionFilters;
        fixedFields: Array<Models.IField>;
		funds: Array<Models.IFund>;
		customViews: Array<Models.ICustomView>;
		selectedCustomView: Models.ICustomView;
		customFields: Array<Models.IField>;
        isSuperUser: boolean;
        originalFunds: any;
		pinnedSecurities: any;
		gridHeight: number = 690;
		showMenu = (position: Models.IPosition, hasWatch: boolean, isSellCandidate: boolean, isPrivate: boolean) => {
            var vm = this;
            vm.selectedGridRow = position;
            var menus = [];

            /*
            if (position.hasBuyTrade) {
                menus.push(this.buyMenuOption);
            } else if (position.hasSellTrade) {
                menus.push(this.sellMenuOption);
            } else {
                menus.push(this.buyMenuOption);
                menus.push(this.sellMenuOption);
            }

            menus.push(null);

            if (hasWatch) {
                menus.push(this.editWatchListMenuOption);
                menus.push(this.offWatchListMenuOption);
            } else {
                menus.push(this.onWatchListMenuOption);
            }

	        menus.push(null);

			if (isSellCandidate) {
		        menus.push(this.editSellCandidateMenuOption);
		        menus.push(this.unMarkSellCandidateMenuOption);
	        } else {
		        menus.push(this.onSellCandidateMenuOption);
	        }
            */

            return menus;
        }

		showBbgMenu = (position: Models.IPosition) => {
			var vm = this;
			vm.selectedGridRow = position;
			var menus = [];

			var opt = vm.updateBbgOption;
			opt.ItemDisable = false;
			menus.push(opt);
			return menus;
        }

        onShowImpliedMatrixSpreadChanged = (fund: Models.IFund) => {
            var vm = this;
            var showImsFields = fund.isImsSelected;
            var refreshUiGrid = false;
            var matrixSpreadField = (fund.fundCode.replace('-', '') + 'MatrixImpliedSpread').toLowerCase();
            var diffSpreadField = (fund.fundCode.replace('-', '') + 'DifferentialImpliedSpread').toLowerCase();
            var matrixWarfRecoveryField = (fund.fundCode.replace('-', '') + 'MatrixWarfRecovery').toLowerCase();

            var defs = vm.gridOptions.columnDefs.filter(x => x.field.toLowerCase().startsWith(matrixSpreadField) || x.field.toLowerCase().startsWith(diffSpreadField) || x.field.toLowerCase().startsWith(matrixWarfRecoveryField));

            defs.forEach(def => {
                if (def.visible != showImsFields) {
                    def.visible = showImsFields;
                    refreshUiGrid = true;
                }
            });

            if (refreshUiGrid)
            {
                vm.gridApi.grid.refresh();
            }
        }

		showIsPrivateMenu = (position: Models.IPosition) => {
			var vm = this;
			vm.selectedGridRow = position;
			var menus = [];

			if (position) {
				if (position.isPrivate) {
					var opt = vm.setToNotPrivate;
					opt.ItemDisable = false;
					menus.push(opt);
				} else {
					var opt = vm.setToPrivate;
					opt.ItemDisable = false;
					menus.push(opt);
				}
			}
			return menus;
		}

      updatePositions = (updatedPositions: Array<Models.IPosition>) => {
		  var vm = this;
		  if (updatedPositions == null) {
			  vm.rootScope.$emit('refreshSummaries', null);
			  return;
		  }
            updatedPositions.forEach(up => {
                
                var changedPositions = vm.gridOptions.data.filter(p => p.securityCode === up.securityCode);
                if (changedPositions.length) {
                    changedPositions[0].watchId = up.watchId;
                    changedPositions[0].watchObjectTypeId = up.watchObjectTypeId;
                    changedPositions[0].watchObjectId = up.watchObjectId;
                    changedPositions[0].isOnWatch = up.isOnWatch;
                    changedPositions[0].watchComments = up.watchComments;
                    changedPositions[0].watchUser = up.watchUser;
                    changedPositions[0].watchLastUpdatedOn = up.watchLastUpdatedOn;

	                changedPositions[0].sellCandidateId = up.sellCandidateId;
	                changedPositions[0].sellCandidateObjectTypeId = up.sellCandidateObjectTypeId;
	                changedPositions[0].sellCandidateObjectId = up.sellCandidateObjectId;
					changedPositions[0].isSellCandidate = up.isSellCandidate;
	                changedPositions[0].sellCandidateComments = up.sellCandidateComments;
	                changedPositions[0].sellCandidateUser = up.sellCandidateUser;
	                changedPositions[0].sellCandidateLastUpdatedOn = up.sellCandidateLastUpdatedOn;

	                changedPositions[0].hasBuyTrade = up.hasBuyTrade;
                    changedPositions[0].hasSellTrade = up.hasSellTrade;
                    changedPositions[0].trades = up.trades;
                    changedPositions[0].totalPar = up.totalPar;
                    changedPositions[0].clO1PctExposure = up.clO1PctExposure;
                    changedPositions[0].clO1NumExposure = up.clO1NumExposure;
                    changedPositions[0].clO2PctExposure = up.clO2PctExposure;
                    changedPositions[0].clO2NumExposure = up.clO2NumExposure;
                    changedPositions[0].clO3PctExposure = up.clO3PctExposure;
                    changedPositions[0].clO3NumExposure = up.clO3NumExposure;
                    changedPositions[0].clO4PctExposure = up.clO4PctExposure;
                    changedPositions[0].clO4NumExposure = up.clO4NumExposure;
	                changedPositions[0].clO5PctExposure = up.clO5PctExposure;
	                changedPositions[0].clO5NumExposure = up.clO5NumExposure;
	                changedPositions[0].clO6PctExposure = up.clO6PctExposure;
	                changedPositions[0].clO6NumExposure = up.clO6NumExposure;
	                changedPositions[0].clO7PctExposure = up.clO7PctExposure;
                    changedPositions[0].clO7NumExposure = up.clO7NumExposure;
                    changedPositions[0].clO8PctExposure = up.clO8PctExposure;
                    changedPositions[0].clO8NumExposure = up.clO8NumExposure;
                    changedPositions[0].clO1Exposure = up.clO1Exposure;
                    changedPositions[0].clO2Exposure = up.clO2Exposure;
                    changedPositions[0].clO3Exposure = up.clO3Exposure;
                    changedPositions[0].clO4Exposure = up.clO4Exposure;
	                changedPositions[0].clO5Exposure = up.clO5Exposure;
	                changedPositions[0].clO6Exposure = up.clO6Exposure;
                    changedPositions[0].clO7Exposure = up.clO7Exposure;
                    changedPositions[0].clO8Exposure = up.clO8Exposure;
                    vm.uiService.processTooltip(changedPositions[0]);
                }
            });
            vm.rootScope.$emit('refreshSummaries', null);
        }

		updateBbgOption: any = ['Update Bbg ID', () => {
			var vm = this;
			if (vm.selectedGridRow) {
				vm.uiService.showUpdateSecurityPopup(vm.selectedGridRow, "bbg", vm.modalService, vm.updatePositionBbgIds);
			}
		}, () => this.isSuperUser];

	    setToNotPrivate: any = ['No', () => {
			var vm = this;
			if (vm.selectedGridRow) {
				vm.setPrivates(false);
			}
		}, () => this.isSuperUser];

		setToPrivate: any = ['Yes', () => {
			var vm = this;
			if (vm.selectedGridRow) {
				vm.setPrivates(true);
			}
		}, () => this.isSuperUser];

		setPrivates = (isPrivate: boolean) => {
			var vm = this;
			if (vm.selectedGridRow) {
				var issuer = <Models.IIssuer>{ issuerId: vm.selectedGridRow.issuerId, isPrivate: isPrivate };
				vm.dataService.updateIsPrivate(issuer).then(() => {
					vm.updatePrivates(issuer);
				}).catch(error => {
					var crap = error;
				});
			}
		}

	    updatePrivates = (issuer: Models.IIssuer) => {
		    var vm = this;
			var positionsToUpdate = vm.positions.filter(x => x.issuerId == issuer.issuerId);
		    positionsToUpdate.forEach(up => {
				up.isPrivate = issuer.isPrivate;
		    });
		    vm.gridApi.grid.refresh();
		}

		updatePositionBbgIds = (security: Models.ISecurity) => {
			var vm = this;
			var positionsToUpdate = vm.positions.filter(x => x.securityId == security.securityId);
			positionsToUpdate.forEach(up => {
				up.bbgId = security.bbgId;
			});
			vm.rootScope.$emit('refreshSummaries', null);
			vm.gridApi.grid.refresh();
		}

        buyMenuOption: any = ['Buy', () => {
            var vm = this;
            if (vm.selectedGridRow) {
                vm.uiService.showBuySellModal(vm.selectedFund, vm.selectedGridRow, vm.modalService, true, vm.updatePositions);
            }
		}, () => this.isSuperUser];


        showHideClo = (fund: Models.IFund) => {
            var vm = this;
            fund.isNotSelected = !fund.isNotSelected;
            vm.setCloVisibility(fund);
            vm.gridApi.grid.refresh();
        }

        setCloVisibility = (fund: Models.IFund) => {
            var vm = this;
			var defs = vm.gridOptions.columnDefs.filter(x => x.displayName.startsWith(fund.fundCode));
	        defs.forEach(def => {
		        def.visible = !fund.isNotSelected;
			});
	        if (!defs.length)
		        fund.isNotSelected = true;
        }

        sellMenuOption = ['Sell', () => {
            var vm = this;
            if (vm.selectedGridRow) {
                vm.uiService.showBuySellModal(vm.selectedFund, vm.selectedGridRow, vm.modalService, false, vm.updatePositions);
            }
        }, () => this.isSuperUser];

        addToLoanCompariosnMenuOption = ['Add To Loan Comparison', () => {
            var vm = this;
            if (vm.selectedGridRow) {
                vm.selectedGridRow.forCompare = true;
            }
        }];


        removeFromLoanCompariosnMenuOption = ['Remove From Loan Comparison', () => {
            var vm = this;
            if (vm.selectedGridRow) {
                vm.selectedGridRow.forCompare = false;
            }
        }];

		onSellCandidateMenuOption = [
			'Sell Candidate', () => {
				var vm = this;
				if (vm.selectedGridRow) {
					vm.uiService.showWatchModal(vm.uiService.createSellCandidate(vm.selectedGridRow), vm.modalService, false, 2, vm.updatePositions);
				}
			}, () => true];

		unMarkSellCandidateMenuOption = [
			'Unmark As Sell Candidate', () => {
				var vm = this;
				if (vm.selectedGridRow) {
					vm.uiService.showWatchModal(vm.uiService.createSellCandidate(vm.selectedGridRow), vm.modalService, true, 2, vm.updatePositions);
				}
			}, () => true];

		editSellCandidateMenuOption = [
			'Edit Sell Candidate', () => {
				var vm = this;
				if (vm.selectedGridRow) {
					vm.uiService.showWatchModal(vm.uiService.createSellCandidate(vm.selectedGridRow), vm.modalService, false, 2, vm.updatePositions);
				}
			}, () => true];

        onWatchListMenuOption = [
            'On Watch List', () => {
                var vm = this;
                if (vm.selectedGridRow) {
					vm.uiService.showWatchModal(vm.uiService.createWatch(vm.selectedGridRow), vm.modalService, false, 1, vm.updatePositions);
                }
	    }, () => this.isSuperUser];

        offWatchListMenuOption = ['Off Watch List', () => {
            var vm = this;
            if (vm.selectedGridRow) {
				vm.uiService.showWatchModal(vm.uiService.createWatch(vm.selectedGridRow), vm.modalService, true, 1, vm.updatePositions);
            }
        }, () => this.isSuperUser];

        editWatchListMenuOption = ['Edit Watch List', () => {
            var vm = this;
            if (vm.selectedGridRow) {
				vm.uiService.showWatchModal(vm.uiService.createWatch(vm.selectedGridRow), vm.modalService, false, 1, vm.updatePositions);
            }
        }, () => this.isSuperUser];

        static $inject = ["application.services.uiService", "application.services.dataService", "$scope", "$rootScope", '$uibModal', '$filter', '$timeout', '$window', 'uiGridConstants','exportUiGridService'];
		constructor(uiService: Application.Services.Contracts.IUIService, dataService: Application.Services.Contracts.IDataService, $scope: ng.IScope
            , $rootScope: ng.IRootScopeService, modalService: angular.ui.bootstrap.IModalService, $filter: ng.IFilterService, timeOutService: ng.ITimeoutService, $window: ng.IWindowService, uiGridConstants: any, exportUiGridService:any) {
            var vm = this;

            $scope.$on("$destroy", () => vm.persistState());
            
            this.searchText = null;

            vm.exportUiGridService = exportUiGridService;
            this.gridOptions = {
                rowHeight: 20,
                enableFiltering: true,
                enablePinning: true,
                enableSorting: true,
                onRegisterApi(gridApi) {
                    vm.gridApi = gridApi;

					gridApi.selection.on.rowSelectionChanged($scope, function (row) {
						pageOptions.PositionSettings.selectedRows = gridApi.selection.getSelectedRows();
					});

					gridApi.selection.on.rowSelectionChangedBatch($scope, function (row) {
						pageOptions.PositionSettings.selectedRows = gridApi.selection.getSelectedRows();
					});

                   gridApi.core.on.renderingComplete($scope, () => vm.windowService.setTimeout(() => vm.applyState()));

                    gridApi.core.on.rowsRendered($scope, () => vm.windowService.setTimeout(() =>  vm.applyState()));              

                },
                appScopeProvider: this,
                columnDefs: [],
                exporterCsvFilename: 'Positions.csv',
                exporterExcelFilename:'Positions'
			}
            vm.dataService = dataService;
            vm.uiGridConstants = uiGridConstants;
            vm.uiService = uiService;
			vm.rootScope = $rootScope;
			vm.scope = $scope;
			vm.timeOutService = timeOutService;
            vm.rootScope.$emit('onActivated', 'positions');
            vm.modalService = modalService;
			vm.rootScope.$on('onTestResultsVisibilityChanged', (event, open: boolean) => {
				vm.onTestResultsVisibilityChanged(open);
			});


			vm.positionViews = ["My View 1", "My View 2", "My View 3"];
	        vm.selectedView = vm.positionViews[0];

            vm.filter = $filter;
            vm.windowService = $window;
            vm.isLoading = true;
	        vm.getAnalystResearchIssuerIds();
            vm.dataService.getFunds().then(funds => {
                
                funds = funds.filter(f => f.canFilter);
                vm.funds = funds;
                vm.originalFunds = JSON.parse(JSON.stringify(funds));
				vm.positionFilter = <Models.IPositionFilters>{ funds: JSON.parse(JSON.stringify(funds)) };
                vm.loadData();
            });
            //alert("1");
        }

		getTableHeight: any = () => {
			var vm = this;
		    var rowHeight = 30; // your row height
		    var headerHeight = 30; // your header heightk
			return {
				width: "100%",
				height: vm.gridHeight + "px"
		    };
	    }

		rowSelectionChanged = (row: any) => {
			var vm = this;
			var positionsForComparison = vm.gridApi.selection.getSelectedRows();
			pageOptions.PositionSettings.selectedRows = positionsForComparison;
		}

        researchExists = (issuerId: number) => {
			var vm = this;
			return vm.issuerIds.indexOf(issuerId) > -1;
		}

        showTradeHistory = (securitycode: string, issuer: string) => {
            var vm = this;
            vm.uiService.showTradeHistoryPopup(securitycode, issuer, vm.modalService, vm.resetScreen);
        }

	    getAnalystResearchIssuerIds = () => {
		    var vm = this;
		    vm.dataService.getAnalystResearchIssuerIds().then(ids => {
			    vm.issuerIds = ids;
		    })
	    }

	    export = () => {
			var vm = this;
            var myElement = angular.element($(".custom-csv-link-location")[0]);
            //vm.exportUiGridService.exportToExcel('sheet 1', vm.gridApi, 'visible', 'all', vm.positionFilter);
            vm.gridApi.exporter.csvExport('visible', 'visible', myElement);
        }

        loadData = () => {
 
            var vm = this;
            vm.statusText = "Loading";
			vm.isLoading = true;
            vm.dataService.getCustomViews().then(views => {
                
		        vm.populateViewList(views, null);
            }).then(() => {
                
		        vm.dataService.userIsASuperUser().then(isSuperUser => {
					vm.isSuperUser = isSuperUser;
                }).then(() => {
                    
                    vm.dataService.getFieldsForCustomView(vm.selectedCustomView.viewId).then((d) => {
                        
                        console.log(d);
				        vm.fields = _.sortBy(d, 'sortOrder');
				        if (vm.rootScope['selectedFund']) {
					        vm.selectedFund = vm.rootScope['selectedFund'];
                        }

                        for (var i = 0; i < vm.funds.length; i++) {
                            vm.funds[i].isImsSelected = false;
                        }

				        vm.onFieldGroupChanged(false);
				        vm.rootScope.$on('onFundSelectionChanged',
					        (event, data: Models.ISummary) => {
						        if (!vm.selectedFund) {
							        vm.onFundChanged(data);
						        } else {
							        vm.selectedFund = data;
						        }
					        });

				        vm.rootScope.$on('onAutoRefresh',
					        (event, data: Models.ISummary) => {
						        vm.onFundChanged(data);
					        });
			        });
		        });
	        });
        }

		insertViewSeparator = (separatorName: string, views: Array<Models.ICustomView>) => {
			var vm = this;
			var pubs = views.filter(x => x.isPublic);
			var privates = views.filter(x => !x.isPublic);
			if (pubs.length && privates.length) {
				//var sorted = _.sortBy(views, 'isPublic');
				var firstPubIndex = (<any>views).findIndex(x => x.isPublic);
				var separator = <Models.ICustomView>{ displayName: separatorName, viewName: separatorName, isDisabled: true, isDefault: false };
				views.splice(firstPubIndex, 0, separator);
			}
		}

		populateViewList = (views: Array<Models.ICustomView>, selectedView: Models.ICustomView) => {
			var vm = this;
			var separatorName = "---------------";
			(<any>views).forEach(x => x.isDisabled = false);
			var sorted = views.sort(function (a, b) {
				return ((a.isPublic === b.isPublic) ? 0 : b.isPublic ? -1 : 1) || a.viewName.toLowerCase().localeCompare(b.viewName.toLowerCase());
			});
			vm.customViews = sorted;
			vm.insertViewSeparator(separatorName, vm.customViews);
            if (views.length) {             
				var actualViews = views.filter(x => x.viewName != separatorName);
				var id = selectedView == null ? -1 : selectedView.viewId;
				//var oldSelection = actualViews.filter(x => x.viewId == id);
				var defaultView = actualViews.filter(x => x.isDefault);
				actualViews.forEach(x => x.displayName = x.viewName);
				if (defaultView.length == 0)
                    defaultView= actualViews.filter(x => x.viewName== "Global Default");

					defaultView[0].displayName = "*DEFAULT | " + defaultView[0].viewName;
                    vm.selectedCustomView = defaultView[0];
			}
		}

		setCustomViewFields = () => {
			var vm = this;
			vm.uiService.createColumnDefs(vm, 'gridOptions', 'filterCollections', 'highlightFilteredHeader', vm.fields);
			vm.uiService.processSearchText(vm.positions, vm.fields);
		}

        clearLoanComparisons = (positions: Array<Models.IPosition>) => {
            var vm = this;
            vm.gridApi.selection.clearSelectedRows();
        }

        comapreLoans = () => {
            var vm = this;
			var positionsForComparison = vm.gridApi.selection.getSelectedRows();
            if (positionsForComparison.length) {
	            let selectedViewId = vm.selectedView;
	            vm.uiService.showLoanComparisonModal(vm.rootScope['selectedFund'], vm.selectedCustomView.viewId,positionsForComparison, vm.customViews, vm.modalService, vm.clearLoanComparisons);
	        }
        }

		filterBySearchText = (columnName: string) => {
			var vm = this;
			try {
				if (columnName == null)
					columnName = "SearchText";
				var column = vm.gridApi.grid.getColumn(columnName);
				if (column == null)//use Issuer
					column = vm.gridApi.grid.getColumn("Issuer");
				column.filters[0].term = vm.searchText;
				vm.gridApi.grid.refresh();
			} catch (e) {

			} 
		}

		filterByName = (columnName: string) => {
			var vm = this;
			var column = vm.gridApi.grid.getColumn(columnName);
			if (!column.filters[0].term) {
				column.filters[0].term = true;
			} else {
				column.filters[0].term = null;
			}
			vm.gridApi.grid.refresh();
		}

        filterBy = (columnIndex:number) => {
            var vm = this;
	        var crap = vm.gridApi.grid.columns;
			var crappier = crap[5];
	        columnIndex = 5;
			var crappiest = vm.gridApi.grid.getColumn('IsOnWatch');
            if (!vm.gridApi.grid.columns[columnIndex].filters[0].term) {
                vm.gridApi.grid.columns[columnIndex].filters[0].term = true;
            } else {
                vm.gridApi.grid.columns[columnIndex].filters[0].term = null;
            }
            vm.gridApi.grid.refresh();
        }

        refreshGrid = () => {
			var vm = this;
            
			if (vm.searchText || vm.searchText == "") {
				var column = vm.getSearchTextColumn("SearchText", "Issuer")
				column.filters[0].term = vm.searchText;
			}

            vm.gridApi.grid.refresh();
        }

		getSearchTextColumn = (searchTextColumnName: string, secondarySearchColumnName: string) => {
			var vm = this;
			var column = vm.gridApi.grid.getColumn("SearchText");
			if (column == null)//use Issuer
				column = vm.gridApi.grid.getColumn("Issuer");
			return column;
		}


        showFilter = () => {
            var vm = this;
	        if (!vm.positions)
		        return;
            vm.funds.forEach(fund => {
                vm.positionFilter.funds.filter(f => f.fundCode === fund.fundCode)[0].isNotSelected = fund.isNotSelected;
            });

            var modalInstance = vm.modalService.open({
                templateUrl: pageOptions.appBasePath + 'app/views/filterpositions.html?v=' + pageOptions.appVersion,
                controller: 'application.controllers.filterPositionsController',
                controllerAs: 'filter',
                size: 'x-x-lg',
                resolve: {
                    positionData: () => {
	                    var data: any = {};
						data.positionfilter = vm.positionFilter;
                        data.positions = vm.positions;
                        data.filterCollections = vm.filterCollections;
                        data.uiFields = vm.fields;
	                    return data;
                    }
                }
            });

            modalInstance.result.then((performfilter) => {
                vm.filterPositions(performfilter);
            }, () => { });
        }

		getFilterCount = (positionFilter: Models.IPositionFilters, filterField: string) => {
            var count = 0;
            if (positionFilter[filterField]) {
                positionFilter[filterField].forEach((posfilter: Models.IFilter) => {
                    if ((posfilter.lowerBound.operator.operatorVal && typeof (posfilter.lowerBound.value) != 'undefined'
                        && (posfilter.lowerBound.value || posfilter.lowerBound.value.toString().length)) ||
                        (posfilter.upperBound.operator.operatorVal && typeof (posfilter.upperBound.value) != 'undefined'
                            && (posfilter.upperBound.value || posfilter.upperBound.value.toString().length))) {
                        count++;
                    }
                });
            }
			return count;
		}

        updatePositionFilterCount = () => {
            var vm = this;
            var count = vm.getFilterCount(vm.positionFilter, "fixedfields");
			count += vm.getFilterCount(vm.positionFilter, "security");
            count += vm.getFilterCount(vm.positionFilter, "ratings");
            
			count += vm.getFilterCount(vm.positionFilter, "analyst");
            vm.positionFilter.count = count;
	        vm.uiService.updateFilterStatistics(vm.positions, vm.positionFilter, "issuer");

        }

		changeView = (selectedView) => {
            var vm = this;
            vm.isLoading = true;
            vm.statusText = "Loading";
            vm.dataService.getFieldsForCustomView(vm.selectedCustomView.viewId).then(flds => {
                vm.fields = vm.sortFieldArraySortOrderAsc(flds);
                var funds = JSON.parse(JSON.stringify(vm.originalFunds));
                vm.positionFilter = <Models.IPositionFilters>{ funds: funds };
                var crap = vm.gridApi.grid.columns;
                vm.gridOptions.columnDefs = [];
                vm.needsRefresh = true;
                vm.positionFilter.count = 0;
                for (var i = 0; i < vm.funds.length; i++) {
                    vm.funds[i].isImsSelected = false;
                }
                vm.onFieldGroupChanged(false);
            });

		}

        filterPositions = (performfilter) => {           
            var vm = this;
            if (performfilter) {
                vm.isLoading = true;
                vm.statusText = 'Filtering';
                vm.positionFilter.funds.forEach(fund => {
                    var posFund = vm.funds.filter(posfund => posfund.fundCode === fund.fundCode)[0];
                    if (posFund.isNotSelected != fund.isNotSelected) {
                        posFund.isNotSelected = fund.isNotSelected;
                        vm.setCloVisibility(posFund);
                    }
                });

				vm.uiService.filterPositions(vm.positions, vm.positionFilter);
                vm.windowService.setTimeout(() => {
                    var filterColumn = vm.gridApi.grid.getColumn(vm.filterFieldName);
                    
					if (filterColumn) {
						filterColumn.filters[0].term = true;
						vm.gridApi.grid.refresh();
					}
                    window.setTimeout(() => {
                        vm.isLoading = false;
                    });
                });
            } else {
	            var filterColumn = vm.gridApi.grid.getColumn(vm.filterFieldName);
	            if (filterColumn) {
		            filterColumn.filters[0].term = null;
	            }
                vm.funds.forEach(fund => {
                    fund.isNotSelected = false;
                    vm.setCloVisibility(fund);
                });
                vm.gridApi.grid.refresh();
            }

            vm.updatePositionFilterCount();
        }

		compare = (a: string, b: string) => {
		    if (a < b)
			    return -1;
		    if (a > b)
			    return 1;
		    return 0;
	    }

		refresh = () => {
			var vm = this;
		    vm.scope.refresh = true;
		    vm.timeOutService(() => {
			    vm.scope.refresh = false;
		    }, 0);
		};

        showAnalystResearch = (issuerId: number) => {
		        var vm = this;
				vm.uiService.showAnalystResearchPopup(issuerId, vm.modalService, vm.resetScreen);
		}

	    showViewEditor = (viewId: number) => {
		    var vm = this;
			vm.uiService.showViewEditorPopup(viewId, vm.modalService, vm.resetScreen);
	    }


	    resetScreen = () => {
			var vm = this;
		    var selected = vm.selectedCustomView;
		    vm.dataService.getCustomViews().then(views => {
				vm.populateViewList(views, selected);
			    vm.changeView(selected);
		    });

	    }

		sortFieldArraySortOrderAsc = (arrayToSort: Array<Models.IField>) => {
			return arrayToSort.sort(function (a, b) {
				return a.sortOrder - b.sortOrder;
			});
		}

		unPin = () => {
			var vm = this;
			try {
				var unPinned = vm.fields.filter(f => !f.pinnedLeft);
				if (unPinned.length) {
					unPinned.forEach(f => {
						try {
							var crap = vm.gridApi.grid.getColumn(f.fieldName);
							if(crap)
								vm.gridApi.pinning.pinColumn(crap, "");
						} catch (e) {
							console.log(e);
						} 
					});
				}
			} catch (e) {
				console.log(e);
			} 
		}

        onFieldGroupChanged = (reload: boolean) => {
            var vm = this;
            if (vm.selectedFund) {
                vm.isLoading = true;
				vm.statusText = 'Loading';
				if (vm.positions && vm.positions.length && !reload) {
					vm.constructGrid(vm.positions);
                    window.setTimeout(() => {
                        vm.isLoading = false;
                    });
					vm.unPin();
				} else {
                    vm.dataService.loadPositions(vm.selectedFund, !vm.onlyWithExposures).then((positions) => {

                        console.log(positions);

						vm.constructGrid(positions);
                        var crap = positions.filter(x => x.isSellCandidate);
                        window.setTimeout(() => {
                            vm.isLoading = false;
                        });
							
							vm.unPin();
						}
					).catch(() => {
                        window.setTimeout(() => {
                            vm.isLoading = false;
                        });
						vm.isFirstTimeLoading = false;
					});
				}
            } else {
                window.setTimeout(() => {
                    vm.isLoading = false;
                });
            }


        }

        constructGrid = (positions: Array<Models.IPosition>) => {

            console.log(positions);
            var vm = this;                     
			if (vm.positions) {
				vm.pinnedSecurities = _.map(vm.gridApi.selection.getSelectedRows(), "securityId");
			}
			vm.positions = positions;
			vm.gridOptions.data = vm.positions;

			vm.windowService.setTimeout(() => {
				positions.forEach(p => {
					if (vm.pinnedSecurities && vm.pinnedSecurities.indexOf(p.securityId) >= 0) {
						vm.gridApi.selection.selectRow(p);
					}
				});
			});

			vm.filterCollections = {};
			var fields = vm.fields;
			vm.uiService.createCollectionFilters(vm.positions, vm, 'filterObj', 'filterCollections', fields);
			vm.uiService.processSearchText(vm.positions, vm.fields);
            console.log(vm.fields);
            vm.uiService.createColumnDefs(vm, 'gridOptions', 'filterCollections', 'highlightFilteredHeader', vm.fields);

			var sortByIssuer = vm.gridOptions.columnDefs.filter(x => x.field == 'issuer');
			var sortBySecurityCode = vm.gridOptions.columnDefs.filter(x => x.field == 'securityCode');
			if (sortByIssuer.length && sortBySecurityCode.length) {
				vm.gridOptions.data = vm.positions.sort((a, b) => {
					return vm.compare(a.issuer, b.issuer) || vm.compare(a.securityCode, b.securityCode);
				});
			}
			if (sortByIssuer.length && !sortBySecurityCode.length) {
				vm.gridOptions.data = vm.positions.sort((a, b) => {
					return vm.compare(a.issuer, b.issuer);
				});
			}
			if (sortBySecurityCode.length && !sortByIssuer.length) {
				vm.gridOptions.data = vm.positions.sort((a, b) => {
					return vm.compare(a.securityCode, b.securityCode);
				});
			}

			//var sortBy = vm.gridOptions.columnDefs.filter(x => x.field == 'issuer');
			if (sortByIssuer.length)
				sortByIssuer[0].sort = { direction: 'asc', priority: 0 };
			//sortBy = vm.gridOptions.columnDefs.filter(x => x.field == 'securityCode');
			if (sortBySecurityCode.length)
				sortBySecurityCode[0].sort = { direction: 'asc', priority: 1 };

			vm.funds.forEach(f => {
				var defs = vm.gridOptions.columnDefs.filter(x => x.displayName.startsWith(f.fundCode));
				f.isNotSelected = !defs.length;
			});

			vm.needsRefresh = false;
			if (pageOptions.PositionSettings.selectedRows) {
				var gridData = vm.gridOptions.data;
				vm.gridApi.grid.modifyRows(gridData);
				pageOptions.PositionSettings.selectedRows.forEach(x => {
					var selectedRows = gridData.filter(y => y.securityId == x.securityId);
					if(selectedRows && selectedRows.length)
						vm.gridApi.selection.selectRow(selectedRows[0]);
				});
			}
			vm.gridApi.grid.refresh();
			vm.isFirstTimeLoading = false;

			vm.windowService.setTimeout(() => {
				var filterColumn = vm.gridApi.grid.getColumn(vm.filterFieldName);
				if (filterColumn && filterColumn.filters[0].term) {
					vm.filterPositions(true);
				}
            });

		}

        highlightFilteredHeader = (row, rowRenderIndex, col, colRenderIndex) => {
            if (col.filters[0].term) {
                return 'header-filtered';
            } else {
                return '';
            }
        };

		booleanSortingAlgorithm = (a, b, rowA, rowB, direction) => {
			var vm = this;

			var nulls = vm.gridApi.core.sortHandleNulls(a, b);
			if (nulls !== null) {
				return nulls;
			} else {
				//return (a === b) ? 0 : a ? -1 : 1;
				// false values first
				 return (a === b)? 0 : a ? 1 : -1;
			}
		}

        sortingAlgorithm = (a, b, rowA, rowB, direction) => {
            var vm = this;

            var nulls = vm.gridApi.core.sortHandleNulls(a, b);
            if (nulls !== null) {
                return nulls;
            } else {
                var l = a ? a.toLowerCase() : '', m = b ? b.toLowerCase() : '';
                return l === m ? 0 : l > m ? 1 : -1;
            }
        }

        sortingNumberAlgorithm = (a, b, rowA, rowB, direction) => {
            var vm = this;
            var nulls = vm.gridApi.core.sortHandleNulls(a, b);
            if (nulls !== null) {
                return nulls;
            } else {
                var s3 = a ? a.toString().split(',').join('') : '';
                var s4 = b ? b.toString().split(',').join('') : '';
                if (!(isNaN(s3) && isNaN(s4))) {
                    return Number(s3) - Number(s4);
                }
                return 0;
            }
        }

        sortingDateAlgorithm = (a, b, rowA, rowB, direction) => {

            var vm = this;
            var nulls = vm.gridApi.core.sortHandleNulls(a, b);
            if (nulls !== null) {
                return nulls;
            } else {
                var c: any = new Date(a);
                var d: any = new Date(b);
                return c - d;
            }
        }

        sortingPercentageAlgorithm = (a, b, rowA, rowB, direction) => {
            var vm = this;
            var nulls = vm.gridApi.core.sortHandleNulls(a, b);
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

		onTestResultsVisibilityChanged = (open: boolean) => {
			var vm = this;
			if (open)
				vm.gridHeight = 690 - (22 * vm.funds.length) - 22;
			else
				vm.gridHeight = 690;
			vm.gridApi.grid.refresh();
		}

        onFundChanged = (data: Models.ISummary) => {
            var vm = this;
            vm.selectedFund = data;
            vm.onFieldGroupChanged(true);
        }        

        persistState() {

            let grid = this.gridApi.grid;

            PositionsController.gridState =
            {
                searchText: this.searchText,
                filterInfos: grid.columns.map(this.getFilterInfo),
                sortInfos: grid.columns.map(this.getSortInfo),
                selectedSecurities: grid.rows.filter(r => r.entity.isSelected),
                positionFilter: this.positionFilter
            };

            
        }

        getFilterInfo(col: any): FilterInfo {
            let filter = col.filters[0];
            return {
                field: col.field,
                term: filter.term,
                listTerm: filter.listTerm,
                condition: filter.condition
            };
        }

        getSortInfo(col: any): SortInfo {

            return {
                field: col.field,
                direction: col.sort.direction,
                priority: col.sort.priority
            };
        }

        applyState() {

            if(!PositionsController.gridState)
                return;

            this.applyFilters();
            this.applySorts();
            this.applySelects();
            this.searchText = this.searchText != null 
                ? this.searchText 
                : PositionsController.gridState.searchText;

            this.positionFilter = PositionsController.gridState.positionFilter;

            this.uiService.filterPositions(this.positions, this.positionFilter);     
        }

        applyFilters() {

            let columnFilters = PositionsController.gridState.filterInfos;     

            this.gridApi.grid.columns
                .forEach(c => c.filters[0] = columnFilters[0].term
                    ? c.filters[0]
                    : columnFilters.filter(f => f.field === c.field)[0]);
        }

        applySorts() {

            let sorts = PositionsController.gridState.sortInfos;

            this.gridApi.grid.columns
                .forEach(c => c.sort = c.sort.direction ? c.sort : sorts.filter(s => s.field == c.field)[0]);
        }

        applySelects() {

            let selectedSecurities = PositionsController.gridState.selectedSecurities;

            this.gridApi.grid.rows
                .filter(r => selectedSecurities.lastIndexOf(r.entity.securityId) != -1)
                .forEach(r => r.isSelected = r.isSelected != undefined ? r.isSelected : true);
        }
        

    }

    type FilterInfo = { field: string, term: string, listTerm: string, condition: string };
    type SortInfo = { field: string, direction: string, priority: number };

    interface GridState
    {
        searchText:string
        filterInfos: FilterInfo[];
        sortInfos: SortInfo[];
        selectedSecurities: number[];
        positionFilter: Models.IPositionFilters;
    }



    angular.module("app").controller("application.controllers.positionsController", PositionsController);

} 