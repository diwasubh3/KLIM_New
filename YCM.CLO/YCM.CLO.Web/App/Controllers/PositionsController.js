var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var PositionsController = (function () {
            function PositionsController(uiService, dataService, $scope, $rootScope, modalService, $filter, timeOutService, $window, uiGridConstants, exportUiGridService) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                this.statusText = "Loading";
                this.filterFieldName = "IsFilterSuccess";
                this.isFirstTimeLoading = true;
                this.needsRefresh = false;
                this.gridHeight = 690;
                /*showMenu = (position: Models.IPosition, hasWatch: boolean, isSellCandidate: boolean, isPrivate: boolean) => { */
                this.showMenu = function (position, hasWatch, hasPaydown) {
                    var vm = _this;
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
                    */
                    if (hasWatch) {
                        menus.push(_this.editWatchListMenuOption);
                        menus.push(_this.offWatchListMenuOption);
                    }
                    else {
                        menus.push(_this.onWatchListMenuOption);
                    }
                    //menus.push(null);
                    if (position.isOnPaydown) {
                        menus.push(_this.editPaydownMenuOption);
                        menus.push(_this.offPaydownMenuOption);
                    }
                    else {
                        menus.push(_this.onPaydownMenuOption);
                    }
                    //menus.push(null);
                    /*
                    if (isSellCandidate) {
                        menus.push(this.editSellCandidateMenuOption);
                        menus.push(this.unMarkSellCandidateMenuOption);
                    } else {
                        menus.push(this.onSellCandidateMenuOption);
                    }
                    */
                    return menus;
                };
                this.showBbgMenu = function (position) {
                    var vm = _this;
                    vm.selectedGridRow = position;
                    var menus = [];
                    var opt = vm.updateBbgOption;
                    opt.ItemDisable = false;
                    menus.push(opt);
                    return menus;
                };
                this.onShowImpliedMatrixSpreadChanged = function (fund) {
                    var vm = _this;
                    var showImsFields = fund.isImsSelected;
                    var refreshUiGrid = false;
                    var matrixSpreadField = (fund.fundCode.replace('-', '') + 'MatrixImpliedSpread').toLowerCase();
                    var diffSpreadField = (fund.fundCode.replace('-', '') + 'DifferentialImpliedSpread').toLowerCase();
                    var matrixWarfRecoveryField = (fund.fundCode.replace('-', '') + 'MatrixWarfRecovery').toLowerCase();
                    var defs = vm.gridOptions.columnDefs.filter(function (x) { return x.field.toLowerCase().startsWith(matrixSpreadField) || x.field.toLowerCase().startsWith(diffSpreadField) || x.field.toLowerCase().startsWith(matrixWarfRecoveryField); });
                    defs.forEach(function (def) {
                        if (def.visible != showImsFields) {
                            def.visible = showImsFields;
                            refreshUiGrid = true;
                        }
                    });
                    if (refreshUiGrid) {
                        vm.gridApi.grid.refresh();
                    }
                };
                this.showIsPrivateMenu = function (position) {
                    var vm = _this;
                    vm.selectedGridRow = position;
                    var menus = [];
                    if (position) {
                        if (position.isPrivate) {
                            var opt = vm.setToNotPrivate;
                            opt.ItemDisable = false;
                            menus.push(opt);
                        }
                        else {
                            var opt = vm.setToPrivate;
                            opt.ItemDisable = false;
                            menus.push(opt);
                        }
                    }
                    return menus;
                };
                this.updatePositions = function (updatedPositions) {
                    var vm = _this;
                    if (updatedPositions == null) {
                        vm.rootScope.$emit('refreshSummaries', null);
                        return;
                    }
                    updatedPositions.forEach(function (up) {
                        var changedPositions = vm.gridOptions.data.filter(function (p) { return p.securityCode === up.securityCode; });
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
                            changedPositions[0].paydownId = up.paydownId;
                            changedPositions[0].paydownObjectTypeId = up.paydownObjectTypeId;
                            changedPositions[0].paydownObjectId = up.paydownObjectId;
                            changedPositions[0].isOnPaydown = up.isOnPaydown;
                            changedPositions[0].paydownComments = up.paydownComments;
                            changedPositions[0].paydownUser = up.paydownUser;
                            changedPositions[0].paydownLastUpdatedOn = up.paydownLastUpdatedOn;
                            vm.uiService.processTooltip(changedPositions[0]);
                        }
                    });
                    vm.rootScope.$emit('refreshSummaries', null);
                };
                this.updateBbgOption = ['Update Bbg ID', function () {
                        var vm = _this;
                        if (vm.selectedGridRow) {
                            vm.uiService.showUpdateSecurityPopup(vm.selectedGridRow, "bbg", vm.modalService, vm.updatePositionBbgIds);
                        }
                    }, function () { return _this.isSuperUser; }];
                this.setToNotPrivate = ['No', function () {
                        var vm = _this;
                        if (vm.selectedGridRow) {
                            vm.setPrivates(false);
                        }
                    }, function () { return _this.isSuperUser; }];
                this.setToPrivate = ['Yes', function () {
                        var vm = _this;
                        if (vm.selectedGridRow) {
                            vm.setPrivates(true);
                        }
                    }, function () { return _this.isSuperUser; }];
                this.setPrivates = function (isPrivate) {
                    var vm = _this;
                    if (vm.selectedGridRow) {
                        var issuer = { issuerId: vm.selectedGridRow.issuerId, isPrivate: isPrivate };
                        vm.dataService.updateIsPrivate(issuer).then(function () {
                            vm.updatePrivates(issuer);
                        }).catch(function (error) {
                            var crap = error;
                        });
                    }
                };
                this.updatePrivates = function (issuer) {
                    var vm = _this;
                    var positionsToUpdate = vm.positions.filter(function (x) { return x.issuerId == issuer.issuerId; });
                    positionsToUpdate.forEach(function (up) {
                        up.isPrivate = issuer.isPrivate;
                    });
                    vm.gridApi.grid.refresh();
                };
                this.updatePositionBbgIds = function (security) {
                    var vm = _this;
                    var positionsToUpdate = vm.positions.filter(function (x) { return x.securityId == security.securityId; });
                    positionsToUpdate.forEach(function (up) {
                        up.bbgId = security.bbgId;
                    });
                    vm.rootScope.$emit('refreshSummaries', null);
                    vm.gridApi.grid.refresh();
                };
                this.buyMenuOption = ['Buy', function () {
                        var vm = _this;
                        if (vm.selectedGridRow) {
                            vm.uiService.showBuySellModal(vm.selectedFund, vm.selectedGridRow, vm.modalService, true, vm.updatePositions);
                        }
                    }, function () { return _this.isSuperUser; }];
                this.showHideClo = function (fund) {
                    var vm = _this;
                    fund.isNotSelected = !fund.isNotSelected;
                    vm.setCloVisibility(fund);
                    vm.gridApi.grid.refresh();
                };
                this.setCloVisibility = function (fund) {
                    var vm = _this;
                    var defs = vm.gridOptions.columnDefs.filter(function (x) { return x.displayName.startsWith(fund.fundCode); });
                    defs.forEach(function (def) {
                        def.visible = !fund.isNotSelected;
                    });
                    if (!defs.length)
                        fund.isNotSelected = true;
                };
                this.sellMenuOption = ['Sell', function () {
                        var vm = _this;
                        if (vm.selectedGridRow) {
                            vm.uiService.showBuySellModal(vm.selectedFund, vm.selectedGridRow, vm.modalService, false, vm.updatePositions);
                        }
                    }, function () { return _this.isSuperUser; }];
                this.addToLoanCompariosnMenuOption = ['Add To Loan Comparison', function () {
                        var vm = _this;
                        if (vm.selectedGridRow) {
                            vm.selectedGridRow.forCompare = true;
                        }
                    }];
                this.removeFromLoanCompariosnMenuOption = ['Remove From Loan Comparison', function () {
                        var vm = _this;
                        if (vm.selectedGridRow) {
                            vm.selectedGridRow.forCompare = false;
                        }
                    }];
                this.onSellCandidateMenuOption = [
                    'Sell Candidate', function () {
                        var vm = _this;
                        if (vm.selectedGridRow) {
                            vm.uiService.showWatchModal(vm.uiService.createSellCandidate(vm.selectedGridRow), vm.modalService, false, 2, vm.updatePositions);
                        }
                    }, function () { return true; }
                ];
                this.unMarkSellCandidateMenuOption = [
                    'Unmark As Sell Candidate', function () {
                        var vm = _this;
                        if (vm.selectedGridRow) {
                            vm.uiService.showWatchModal(vm.uiService.createSellCandidate(vm.selectedGridRow), vm.modalService, true, 2, vm.updatePositions);
                        }
                    }, function () { return true; }
                ];
                this.editSellCandidateMenuOption = [
                    'Edit Sell Candidate', function () {
                        var vm = _this;
                        if (vm.selectedGridRow) {
                            vm.uiService.showWatchModal(vm.uiService.createSellCandidate(vm.selectedGridRow), vm.modalService, false, 2, vm.updatePositions);
                        }
                    }, function () { return true; }
                ];
                this.onWatchListMenuOption = [
                    'On Watch List', function () {
                        var vm = _this;
                        if (vm.selectedGridRow) {
                            vm.uiService.showWatchModal(vm.uiService.createWatch(vm.selectedGridRow), vm.modalService, false, 1, vm.updatePositions);
                        }
                    }, function () { return _this.isSuperUser; }
                ];
                this.offWatchListMenuOption = ['Off Watch List', function () {
                        var vm = _this;
                        if (vm.selectedGridRow) {
                            vm.uiService.showWatchModal(vm.uiService.createWatch(vm.selectedGridRow), vm.modalService, true, 1, vm.updatePositions);
                        }
                    }, function () { return _this.isSuperUser; }];
                this.editWatchListMenuOption = ['Edit Watch List', function () {
                        var vm = _this;
                        if (vm.selectedGridRow) {
                            vm.uiService.showWatchModal(vm.uiService.createWatch(vm.selectedGridRow), vm.modalService, false, 1, vm.updatePositions);
                        }
                    }, function () { return _this.isSuperUser; }];
                this.getTableHeight = function () {
                    var vm = _this;
                    var rowHeight = 30; // your row height
                    var headerHeight = 30; // your header heightk
                    return {
                        width: "100%",
                        height: vm.gridHeight + "px"
                    };
                };
                this.rowSelectionChanged = function (row) {
                    var vm = _this;
                    var positionsForComparison = vm.gridApi.selection.getSelectedRows();
                    pageOptions.PositionSettings.selectedRows = positionsForComparison;
                };
                this.researchExists = function (issuerId) {
                    var vm = _this;
                    return vm.issuerIds.indexOf(issuerId) > -1;
                };
                this.showTradeHistory = function (securitycode, issuer) {
                    var vm = _this;
                    vm.uiService.showTradeHistoryPopup(securitycode, issuer, vm.modalService, vm.resetScreen);
                };
                this.getAnalystResearchIssuerIds = function () {
                    var vm = _this;
                    vm.dataService.getAnalystResearchIssuerIds().then(function (ids) {
                        vm.issuerIds = ids;
                    });
                };
                this.export = function () {
                    var vm = _this;
                    var myElement = angular.element($(".custom-csv-link-location")[0]);
                    //vm.exportUiGridService.exportToExcel('sheet 1', vm.gridApi, 'visible', 'all', vm.positionFilter);
                    vm.gridApi.exporter.csvExport('visible', 'visible', myElement);
                };
                this.loadData = function () {
                    var vm = _this;
                    vm.statusText = "Loading";
                    vm.isLoading = true;
                    vm.dataService.getCustomViews().then(function (views) {
                        vm.populateViewList(views, null);
                    }).then(function () {
                        vm.dataService.userIsASuperUser().then(function (isSuperUser) {
                            vm.isSuperUser = isSuperUser;
                        }).then(function () {
                            vm.dataService.getFieldsForCustomView(vm.selectedCustomView.viewId).then(function (d) {
                                vm.fields = _.sortBy(d, 'sortOrder');
                                if (vm.rootScope['selectedFund']) {
                                    vm.selectedFund = vm.rootScope['selectedFund'];
                                }
                                for (var i = 0; i < vm.funds.length; i++) {
                                    vm.funds[i].isImsSelected = false;
                                }
                                vm.onFieldGroupChanged(false);
                                vm.rootScope.$on('onFundSelectionChanged', function (event, data) {
                                    if (!vm.selectedFund) {
                                        vm.onFundChanged(data);
                                    }
                                    else {
                                        vm.selectedFund = data;
                                    }
                                });
                                vm.rootScope.$on('onAutoRefresh', function (event, data) {
                                    vm.onFundChanged(data);
                                });
                            });
                        });
                    });
                };
                this.insertViewSeparator = function (separatorName, views) {
                    var vm = _this;
                    var pubs = views.filter(function (x) { return x.isPublic; });
                    var privates = views.filter(function (x) { return !x.isPublic; });
                    if (pubs.length && privates.length) {
                        //var sorted = _.sortBy(views, 'isPublic');
                        var firstPubIndex = views.findIndex(function (x) { return x.isPublic; });
                        var separator = { displayName: separatorName, viewName: separatorName, isDisabled: true, isDefault: false };
                        views.splice(firstPubIndex, 0, separator);
                    }
                };
                this.populateViewList = function (views, selectedView) {
                    var vm = _this;
                    var separatorName = "---------------";
                    views.forEach(function (x) { return x.isDisabled = false; });
                    var sorted = views.sort(function (a, b) {
                        return ((a.isPublic === b.isPublic) ? 0 : b.isPublic ? -1 : 1) || a.viewName.toLowerCase().localeCompare(b.viewName.toLowerCase());
                    });
                    vm.customViews = sorted;
                    vm.insertViewSeparator(separatorName, vm.customViews);
                    if (views.length) {
                        var actualViews = views.filter(function (x) { return x.viewName != separatorName; });
                        var id = selectedView == null ? -1 : selectedView.viewId;
                        //var oldSelection = actualViews.filter(x => x.viewId == id);
                        var defaultView = actualViews.filter(function (x) { return x.isDefault; });
                        actualViews.forEach(function (x) { return x.displayName = x.viewName; });
                        if (defaultView.length == 0)
                            defaultView = actualViews.filter(function (x) { return x.viewName == "Global Default"; });
                        defaultView[0].displayName = "*DEFAULT | " + defaultView[0].viewName;
                        vm.selectedCustomView = defaultView[0];
                    }
                };
                this.setCustomViewFields = function () {
                    var vm = _this;
                    vm.uiService.createColumnDefs(vm, 'gridOptions', 'filterCollections', 'highlightFilteredHeader', vm.fields);
                    vm.uiService.processSearchText(vm.positions, vm.fields);
                };
                this.clearLoanComparisons = function (positions) {
                    var vm = _this;
                    vm.gridApi.selection.clearSelectedRows();
                };
                this.comapreLoans = function () {
                    var vm = _this;
                    var positionsForComparison = vm.gridApi.selection.getSelectedRows();
                    if (positionsForComparison.length) {
                        var selectedViewId = vm.selectedView;
                        vm.uiService.showLoanComparisonModal(vm.rootScope['selectedFund'], vm.selectedCustomView.viewId, positionsForComparison, vm.customViews, vm.modalService, vm.clearLoanComparisons);
                    }
                };
                this.filterBySearchText = function (columnName) {
                    var vm = _this;
                    try {
                        if (columnName == null)
                            columnName = "SearchText";
                        var column = vm.gridApi.grid.getColumn(columnName);
                        if (column == null)
                            column = vm.gridApi.grid.getColumn("Issuer");
                        column.filters[0].term = vm.searchText;
                        vm.gridApi.grid.refresh();
                    }
                    catch (e) {
                    }
                };
                this.filterByName = function (columnName) {
                    var vm = _this;
                    var column = vm.gridApi.grid.getColumn(columnName);
                    if (!column.filters[0].term) {
                        column.filters[0].term = true;
                    }
                    else {
                        column.filters[0].term = null;
                    }
                    vm.gridApi.grid.refresh();
                };
                this.filterBy = function (columnIndex) {
                    var vm = _this;
                    var crap = vm.gridApi.grid.columns;
                    var crappier = crap[5];
                    columnIndex = 5;
                    var crappiest = vm.gridApi.grid.getColumn('IsOnWatch');
                    if (!vm.gridApi.grid.columns[columnIndex].filters[0].term) {
                        vm.gridApi.grid.columns[columnIndex].filters[0].term = true;
                    }
                    else {
                        vm.gridApi.grid.columns[columnIndex].filters[0].term = null;
                    }
                    vm.gridApi.grid.refresh();
                };
                this.refreshGrid = function () {
                    var vm = _this;
                    if (vm.searchText || vm.searchText == "") {
                        var column = vm.getSearchTextColumn("SearchText", "Issuer");
                        column.filters[0].term = vm.searchText;
                    }
                    vm.gridApi.grid.refresh();
                };
                this.getSearchTextColumn = function (searchTextColumnName, secondarySearchColumnName) {
                    var vm = _this;
                    var column = vm.gridApi.grid.getColumn("SearchText");
                    if (column == null)
                        column = vm.gridApi.grid.getColumn("Issuer");
                    return column;
                };
                this.showFilter = function () {
                    var vm = _this;
                    if (!vm.positions)
                        return;
                    vm.funds.forEach(function (fund) {
                        vm.positionFilter.funds.filter(function (f) { return f.fundCode === fund.fundCode; })[0].isNotSelected = fund.isNotSelected;
                    });
                    var modalInstance = vm.modalService.open({
                        templateUrl: pageOptions.appBasePath + 'app/views/filterpositions.html?v=' + pageOptions.appVersion,
                        controller: 'application.controllers.filterPositionsController',
                        controllerAs: 'filter',
                        size: 'x-x-lg',
                        resolve: {
                            positionData: function () {
                                var data = {};
                                data.positionfilter = vm.positionFilter;
                                data.positions = vm.positions;
                                data.filterCollections = vm.filterCollections;
                                data.uiFields = vm.fields;
                                return data;
                            }
                        }
                    });
                    modalInstance.result.then(function (performfilter) {
                        vm.filterPositions(performfilter);
                    }, function () { });
                };
                this.getFilterCount = function (positionFilter, filterField) {
                    var count = 0;
                    if (positionFilter[filterField]) {
                        positionFilter[filterField].forEach(function (posfilter) {
                            if ((posfilter.lowerBound.operator.operatorVal && typeof (posfilter.lowerBound.value) != 'undefined'
                                && (posfilter.lowerBound.value || posfilter.lowerBound.value.toString().length)) ||
                                (posfilter.upperBound.operator.operatorVal && typeof (posfilter.upperBound.value) != 'undefined'
                                    && (posfilter.upperBound.value || posfilter.upperBound.value.toString().length))) {
                                count++;
                            }
                        });
                    }
                    return count;
                };
                this.updatePositionFilterCount = function () {
                    var vm = _this;
                    var count = vm.getFilterCount(vm.positionFilter, "fixedfields");
                    count += vm.getFilterCount(vm.positionFilter, "security");
                    count += vm.getFilterCount(vm.positionFilter, "ratings");
                    count += vm.getFilterCount(vm.positionFilter, "analyst");
                    vm.positionFilter.count = count;
                    vm.uiService.updateFilterStatistics(vm.positions, vm.positionFilter, "issuer");
                };
                this.changeView = function (selectedView) {
                    var vm = _this;
                    vm.isLoading = true;
                    vm.statusText = "Loading";
                    vm.dataService.getFieldsForCustomView(vm.selectedCustomView.viewId).then(function (flds) {
                        vm.fields = vm.sortFieldArraySortOrderAsc(flds);
                        var funds = JSON.parse(JSON.stringify(vm.originalFunds));
                        vm.positionFilter = { funds: funds };
                        var crap = vm.gridApi.grid.columns;
                        vm.gridOptions.columnDefs = [];
                        vm.needsRefresh = true;
                        vm.positionFilter.count = 0;
                        for (var i = 0; i < vm.funds.length; i++) {
                            vm.funds[i].isImsSelected = false;
                        }
                        vm.onFieldGroupChanged(false);
                    });
                };
                this.filterPositions = function (performfilter) {
                    var vm = _this;
                    if (performfilter) {
                        vm.isLoading = true;
                        vm.statusText = 'Filtering';
                        vm.positionFilter.funds.forEach(function (fund) {
                            var posFund = vm.funds.filter(function (posfund) { return posfund.fundCode === fund.fundCode; })[0];
                            if (posFund.isNotSelected != fund.isNotSelected) {
                                posFund.isNotSelected = fund.isNotSelected;
                                vm.setCloVisibility(posFund);
                            }
                        });
                        vm.uiService.filterPositions(vm.positions, vm.positionFilter);
                        vm.windowService.setTimeout(function () {
                            var filterColumn = vm.gridApi.grid.getColumn(vm.filterFieldName);
                            if (filterColumn) {
                                filterColumn.filters[0].term = true;
                                vm.gridApi.grid.refresh();
                            }
                            window.setTimeout(function () {
                                vm.isLoading = false;
                            });
                        });
                    }
                    else {
                        var filterColumn = vm.gridApi.grid.getColumn(vm.filterFieldName);
                        if (filterColumn) {
                            filterColumn.filters[0].term = null;
                        }
                        vm.funds.forEach(function (fund) {
                            fund.isNotSelected = false;
                            vm.setCloVisibility(fund);
                        });
                        vm.gridApi.grid.refresh();
                    }
                    vm.updatePositionFilterCount();
                };
                this.compare = function (a, b) {
                    if (a < b)
                        return -1;
                    if (a > b)
                        return 1;
                    return 0;
                };
                this.refresh = function () {
                    var vm = _this;
                    vm.scope.refresh = true;
                    vm.timeOutService(function () {
                        vm.scope.refresh = false;
                    }, 0);
                };
                this.showAnalystResearch = function (issuerId) {
                    var vm = _this;
                    vm.uiService.showAnalystResearchPopup(issuerId, vm.modalService, vm.resetScreen);
                };
                this.showViewEditor = function (viewId) {
                    var vm = _this;
                    vm.uiService.showViewEditorPopup(viewId, vm.modalService, vm.resetScreen);
                };
                this.resetScreen = function () {
                    var vm = _this;
                    var selected = vm.selectedCustomView;
                    vm.dataService.getCustomViews().then(function (views) {
                        vm.populateViewList(views, selected);
                        vm.changeView(selected);
                    });
                };
                this.sortFieldArraySortOrderAsc = function (arrayToSort) {
                    return arrayToSort.sort(function (a, b) {
                        return a.sortOrder - b.sortOrder;
                    });
                };
                this.unPin = function () {
                    var vm = _this;
                    try {
                        var unPinned = vm.fields.filter(function (f) { return !f.pinnedLeft; });
                        if (unPinned.length) {
                            unPinned.forEach(function (f) {
                                try {
                                    var crap = vm.gridApi.grid.getColumn(f.fieldName);
                                    if (crap)
                                        vm.gridApi.pinning.pinColumn(crap, "");
                                }
                                catch (e) {
                                    console.log(e);
                                }
                            });
                        }
                    }
                    catch (e) {
                        console.log(e);
                    }
                };
                this.onFieldGroupChanged = function (reload) {
                    var vm = _this;
                    if (vm.selectedFund) {
                        vm.isLoading = true;
                        vm.statusText = 'Loading';
                        if (vm.positions && vm.positions.length && !reload) {
                            vm.constructGrid(vm.positions);
                            window.setTimeout(function () {
                                vm.isLoading = false;
                            });
                            vm.unPin();
                        }
                        else {
                            vm.dataService.loadPositions(vm.selectedFund, !vm.onlyWithExposures).then(function (positions) {
                                vm.constructGrid(positions);
                                var crap = positions.filter(function (x) { return x.isSellCandidate; });
                                window.setTimeout(function () {
                                    vm.isLoading = false;
                                });
                                vm.unPin();
                            }).catch(function () {
                                window.setTimeout(function () {
                                    vm.isLoading = false;
                                });
                                vm.isFirstTimeLoading = false;
                            });
                        }
                    }
                    else {
                        window.setTimeout(function () {
                            vm.isLoading = false;
                        });
                    }
                };
                this.constructGrid = function (positions) {
                    var vm = _this;
                    if (vm.positions) {
                        vm.pinnedSecurities = _.map(vm.gridApi.selection.getSelectedRows(), "securityId");
                    }
                    vm.positions = positions;
                    vm.gridOptions.data = vm.positions;
                    vm.windowService.setTimeout(function () {
                        positions.forEach(function (p) {
                            if (vm.pinnedSecurities && vm.pinnedSecurities.indexOf(p.securityId) >= 0) {
                                vm.gridApi.selection.selectRow(p);
                            }
                        });
                    });
                    vm.filterCollections = {};
                    var fields = vm.fields;
                    vm.uiService.createCollectionFilters(vm.positions, vm, 'filterObj', 'filterCollections', fields);
                    vm.uiService.processSearchText(vm.positions, vm.fields);
                    vm.uiService.createColumnDefs(vm, 'gridOptions', 'filterCollections', 'highlightFilteredHeader', vm.fields);
                    var sortByIssuer = vm.gridOptions.columnDefs.filter(function (x) { return x.field == 'issuer'; });
                    var sortBySecurityCode = vm.gridOptions.columnDefs.filter(function (x) { return x.field == 'securityCode'; });
                    if (sortByIssuer.length && sortBySecurityCode.length) {
                        vm.gridOptions.data = vm.positions.sort(function (a, b) {
                            return vm.compare(a.issuer, b.issuer) || vm.compare(a.securityCode, b.securityCode);
                        });
                    }
                    if (sortByIssuer.length && !sortBySecurityCode.length) {
                        vm.gridOptions.data = vm.positions.sort(function (a, b) {
                            return vm.compare(a.issuer, b.issuer);
                        });
                    }
                    if (sortBySecurityCode.length && !sortByIssuer.length) {
                        vm.gridOptions.data = vm.positions.sort(function (a, b) {
                            return vm.compare(a.securityCode, b.securityCode);
                        });
                    }
                    //var sortBy = vm.gridOptions.columnDefs.filter(x => x.field == 'issuer');
                    if (sortByIssuer.length)
                        sortByIssuer[0].sort = { direction: 'asc', priority: 0 };
                    //sortBy = vm.gridOptions.columnDefs.filter(x => x.field == 'securityCode');
                    if (sortBySecurityCode.length)
                        sortBySecurityCode[0].sort = { direction: 'asc', priority: 1 };
                    vm.funds.forEach(function (f) {
                        var defs = vm.gridOptions.columnDefs.filter(function (x) { return x.displayName.startsWith(f.fundCode); });
                        f.isNotSelected = !defs.length;
                    });
                    vm.needsRefresh = false;
                    if (pageOptions.PositionSettings.selectedRows) {
                        var gridData = vm.gridOptions.data;
                        vm.gridApi.grid.modifyRows(gridData);
                        pageOptions.PositionSettings.selectedRows.forEach(function (x) {
                            var selectedRows = gridData.filter(function (y) { return y.securityId == x.securityId; });
                            if (selectedRows && selectedRows.length)
                                vm.gridApi.selection.selectRow(selectedRows[0]);
                        });
                    }
                    vm.gridApi.grid.refresh();
                    vm.isFirstTimeLoading = false;
                    vm.windowService.setTimeout(function () {
                        var filterColumn = vm.gridApi.grid.getColumn(vm.filterFieldName);
                        if (filterColumn && filterColumn.filters[0].term) {
                            vm.filterPositions(true);
                        }
                    });
                };
                this.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                    if (col.filters[0].term) {
                        return 'header-filtered';
                    }
                    else {
                        return '';
                    }
                };
                this.booleanSortingAlgorithm = function (a, b, rowA, rowB, direction) {
                    var vm = _this;
                    var nulls = vm.gridApi.core.sortHandleNulls(a, b);
                    if (nulls !== null) {
                        return nulls;
                    }
                    else {
                        //return (a === b) ? 0 : a ? -1 : 1;
                        // false values first
                        return (a === b) ? 0 : a ? 1 : -1;
                    }
                };
                this.sortingAlgorithm = function (a, b, rowA, rowB, direction) {
                    var vm = _this;
                    var nulls = vm.gridApi.core.sortHandleNulls(a, b);
                    if (nulls !== null) {
                        return nulls;
                    }
                    else {
                        var l = a ? a.toLowerCase() : '', m = b ? b.toLowerCase() : '';
                        return l === m ? 0 : l > m ? 1 : -1;
                    }
                };
                this.sortingNumberAlgorithm = function (a, b, rowA, rowB, direction) {
                    var vm = _this;
                    var nulls = vm.gridApi.core.sortHandleNulls(a, b);
                    if (nulls !== null) {
                        return nulls;
                    }
                    else {
                        var s3 = a ? a.toString().split(',').join('') : '';
                        var s4 = b ? b.toString().split(',').join('') : '';
                        if (!(isNaN(s3) && isNaN(s4))) {
                            return Number(s3) - Number(s4);
                        }
                        return 0;
                    }
                };
                this.sortingDateAlgorithm = function (a, b, rowA, rowB, direction) {
                    var vm = _this;
                    var nulls = vm.gridApi.core.sortHandleNulls(a, b);
                    if (nulls !== null) {
                        return nulls;
                    }
                    else {
                        var c = new Date(a);
                        var d = new Date(b);
                        return c - d;
                    }
                };
                this.sortingPercentageAlgorithm = function (a, b, rowA, rowB, direction) {
                    var vm = _this;
                    var nulls = vm.gridApi.core.sortHandleNulls(a, b);
                    if (nulls !== null) {
                        return nulls;
                    }
                    else {
                        var s3 = a ? a.toString().split([' %']).join('') : '';
                        var s4 = b ? b.toString().split(' %').join('') : '';
                        if (!(isNaN(s3) && isNaN(s4))) {
                            return Number(s3) - Number(s4);
                        }
                        return 0;
                    }
                };
                this.onTestResultsVisibilityChanged = function (open) {
                    var vm = _this;
                    if (open)
                        vm.gridHeight = 690 - (22 * vm.funds.length) - 22;
                    else
                        vm.gridHeight = 690;
                    vm.gridApi.grid.refresh();
                };
                this.onFundChanged = function (data) {
                    var vm = _this;
                    vm.selectedFund = data;
                    vm.onFieldGroupChanged(true);
                };
                this.onPaydownMenuOption = [
                    'On Paydown List', function () {
                        var vm = _this;
                        if (vm.selectedGridRow) {
                            vm.uiService.showPaydownModal(vm.uiService.createPaydown(vm.selectedGridRow), vm.modalService, false, 1, vm.updatePositions);
                        }
                    }, function () { return _this.isSuperUser; }
                ];
                this.offPaydownMenuOption = ['Off Paydown List', function () {
                        var vm = _this;
                        if (vm.selectedGridRow) {
                            vm.uiService.showPaydownModal(vm.uiService.createPaydown(vm.selectedGridRow), vm.modalService, true, 1, vm.updatePositions);
                        }
                    }, function () { return _this.isSuperUser; }];
                this.editPaydownMenuOption = ['Edit Paydown List', function () {
                        var vm = _this;
                        if (vm.selectedGridRow) {
                            vm.uiService.showPaydownModal(vm.uiService.createPaydown(vm.selectedGridRow), vm.modalService, false, 1, vm.updatePositions);
                        }
                    }, function () { return _this.isSuperUser; }];
                var vm = this;
                $scope.$on("$destroy", function () { return vm.persistState(); });
                this.searchText = null;
                vm.exportUiGridService = exportUiGridService;
                this.gridOptions = {
                    rowHeight: 20,
                    enableFiltering: true,
                    enablePinning: true,
                    enableSorting: true,
                    onRegisterApi: function (gridApi) {
                        vm.gridApi = gridApi;
                        gridApi.selection.on.rowSelectionChanged($scope, function (row) {
                            pageOptions.PositionSettings.selectedRows = gridApi.selection.getSelectedRows();
                        });
                        gridApi.selection.on.rowSelectionChangedBatch($scope, function (row) {
                            pageOptions.PositionSettings.selectedRows = gridApi.selection.getSelectedRows();
                        });
                        gridApi.core.on.renderingComplete($scope, function () { return vm.windowService.setTimeout(function () { return vm.applyState(); }); });
                        gridApi.core.on.rowsRendered($scope, function () { return vm.windowService.setTimeout(function () { return vm.applyState(); }); });
                    },
                    appScopeProvider: this,
                    columnDefs: [],
                    exporterCsvFilename: 'Positions.csv',
                    exporterExcelFilename: 'Positions'
                };
                vm.dataService = dataService;
                vm.uiGridConstants = uiGridConstants;
                vm.uiService = uiService;
                vm.rootScope = $rootScope;
                vm.scope = $scope;
                vm.timeOutService = timeOutService;
                vm.rootScope.$emit('onActivated', 'positions');
                vm.modalService = modalService;
                vm.rootScope.$on('onTestResultsVisibilityChanged', function (event, open) {
                    vm.onTestResultsVisibilityChanged(open);
                });
                vm.positionViews = ["My View 1", "My View 2", "My View 3"];
                vm.selectedView = vm.positionViews[0];
                vm.filter = $filter;
                vm.windowService = $window;
                vm.isLoading = true;
                vm.getAnalystResearchIssuerIds();
                vm.dataService.getFunds().then(function (funds) {
                    funds = funds.filter(function (f) { return f.canFilter; });
                    vm.funds = funds;
                    vm.originalFunds = JSON.parse(JSON.stringify(funds));
                    vm.positionFilter = { funds: JSON.parse(JSON.stringify(funds)) };
                    vm.loadData();
                });
                //alert("1");
            }
            PositionsController.prototype.persistState = function () {
                var grid = this.gridApi.grid;
                PositionsController.gridState =
                    {
                        searchText: this.searchText,
                        filterInfos: grid.columns.map(this.getFilterInfo),
                        sortInfos: grid.columns.map(this.getSortInfo),
                        selectedSecurities: grid.rows.filter(function (r) { return r.entity.isSelected; }),
                        positionFilter: this.positionFilter
                    };
            };
            PositionsController.prototype.getFilterInfo = function (col) {
                var filter = col.filters[0];
                return {
                    field: col.field,
                    term: filter.term,
                    listTerm: filter.listTerm,
                    condition: filter.condition
                };
            };
            PositionsController.prototype.getSortInfo = function (col) {
                return {
                    field: col.field,
                    direction: col.sort.direction,
                    priority: col.sort.priority
                };
            };
            PositionsController.prototype.applyState = function () {
                if (!PositionsController.gridState)
                    return;
                this.applyFilters();
                this.applySorts();
                this.applySelects();
                this.searchText = this.searchText != null
                    ? this.searchText
                    : PositionsController.gridState.searchText;
                this.positionFilter = PositionsController.gridState.positionFilter;
                this.uiService.filterPositions(this.positions, this.positionFilter);
            };
            PositionsController.prototype.applyFilters = function () {
                var columnFilters = PositionsController.gridState.filterInfos;
                this.gridApi.grid.columns
                    .forEach(function (c) { return c.filters[0] = columnFilters[0].term
                    ? c.filters[0]
                    : columnFilters.filter(function (f) { return f.field === c.field; })[0]; });
            };
            PositionsController.prototype.applySorts = function () {
                var sorts = PositionsController.gridState.sortInfos;
                this.gridApi.grid.columns
                    .forEach(function (c) { return c.sort = c.sort.direction ? c.sort : sorts.filter(function (s) { return s.field == c.field; })[0]; });
            };
            PositionsController.prototype.applySelects = function () {
                var selectedSecurities = PositionsController.gridState.selectedSecurities;
                this.gridApi.grid.rows
                    .filter(function (r) { return selectedSecurities.lastIndexOf(r.entity.securityId) != -1; })
                    .forEach(function (r) { return r.isSelected = r.isSelected != undefined ? r.isSelected : true; });
            };
            return PositionsController;
        }());
        PositionsController.$inject = ["application.services.uiService", "application.services.dataService", "$scope", "$rootScope", '$uibModal', '$filter', '$timeout', '$window', 'uiGridConstants', 'exportUiGridService'];
        Controllers.PositionsController = PositionsController;
        angular.module("app").controller("application.controllers.positionsController", PositionsController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=PositionsController.js.map