var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var Top10Bottom10Controller = (function () {
            function Top10Bottom10Controller(uiService, dataService, $rootScope, modalService, $filter, timeOutService, uiGridConstants, windowService) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                this.statusText = "Loading";
                this.showMenu = function (position, hasWatch) {
                    var vm = _this;
                    vm.selectedGridRow = position;
                    var menus = [];
                    if (position.hasBuyTrade) {
                        menus.push(_this.buyMenuOption);
                    }
                    else if (position.hasSellTrade) {
                        menus.push(_this.sellMenuOption);
                    }
                    else {
                        menus.push(_this.buyMenuOption);
                        menus.push(_this.sellMenuOption);
                    }
                    menus.push(null);
                    //if (position.forCompare) {
                    //    menus.push(this.removeFromLoanCompariosnMenuOption);
                    //}
                    //else {
                    //    menus.push(this.addToLoanCompariosnMenuOption);
                    //}    
                    if (hasWatch) {
                        menus.push(_this.editWatchListMenuOption);
                        menus.push(_this.offWatchListMenuOption);
                    }
                    else {
                        menus.push(_this.onWatchListMenuOption);
                    }
                    return menus;
                };
                this.updatePositions = function (updatedPositions) {
                    var vm = _this;
                    updatedPositions.forEach(function (up) {
                        var changedPositions = vm.topGridOptions.data.filter(function (p) { return p.securityCode === up.securityCode; });
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
                        changedPositions = vm.bottomGridOptions.data.filter(function (p) { return p.securityCode === up.securityCode; });
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
                };
                this.clearLoanComparisons = function (positions) {
                    var vm = _this;
                    vm.bottomGridApi.selection.clearSelectedRows();
                    vm.topGridApi.selection.clearSelectedRows();
                };
                this.comapreLoans = function () {
                    var vm = _this;
                    var positionsForComparison = vm.bottomGridApi.selection.getSelectedRows().concat(vm.topGridApi.selection.getSelectedRows());
                    if (positionsForComparison.length) {
                        var views = vm.uiService.showLoanComparisonModal(vm.rootScope['selectedFund'], -1, positionsForComparison, vm.customViews, vm.modalService, vm.clearLoanComparisons);
                    }
                };
                this.getSortedCustomViews = function (views) {
                    var vm = _this;
                    var separatorName = "---------------";
                    views.forEach(function (x) { return x.isDisabled = false; });
                    var sorted = views.sort(function (a, b) {
                        return ((a.isPublic === b.isPublic) ? 0 : b.isPublic ? -1 : 1) || a.viewName.toLowerCase().localeCompare(b.viewName.toLowerCase());
                    });
                    vm.insertViewSeparator(separatorName, sorted);
                    vm.customViews = sorted;
                };
                this.insertViewSeparator = function (separatorName, views) {
                    var vm = _this;
                    var pubs = views.filter(function (x) { return x.isPublic; });
                    var privates = views.filter(function (x) { return !x.isPublic; });
                    if (pubs.length && privates.length) {
                        //var sorted = _.sortBy(views, 'isPublic');
                        var firstPubIndex = views.findIndex(function (x) { return x.isPublic; });
                        var separator = { viewName: separatorName, isDisabled: true, isDefault: false };
                        views.splice(firstPubIndex, 0, separator);
                    }
                };
                this.buyMenuOption = ['Buy', function () {
                        var vm = _this;
                        if (vm.selectedGridRow) {
                            vm.uiService.showBuySellModal(vm.selectedFund, vm.selectedGridRow, vm.modalService, true, vm.updatePositions);
                        }
                    }];
                this.sellMenuOption = ['Sell', function () {
                        var vm = _this;
                        if (vm.selectedGridRow) {
                            vm.uiService.showBuySellModal(vm.selectedFund, vm.selectedGridRow, vm.modalService, false, vm.updatePositions);
                        }
                    }];
                this.onWatchListMenuOption = [
                    'On Watch List', function () {
                        var vm = _this;
                        if (vm.selectedGridRow) {
                            vm.uiService.showWatchModal(vm.uiService.createWatch(vm.selectedGridRow), vm.modalService, false, 1, vm.updatePositions);
                        }
                    }
                ];
                this.offWatchListMenuOption = ['Off Watch List', function () {
                        var vm = _this;
                        if (vm.selectedGridRow) {
                            vm.uiService.showWatchModal(vm.uiService.createWatch(vm.selectedGridRow), vm.modalService, true, 1, vm.updatePositions);
                        }
                    }];
                this.editWatchListMenuOption = ['Edit Watch List', function () {
                        var vm = _this;
                        if (vm.selectedGridRow) {
                            vm.uiService.showWatchModal(vm.uiService.createWatch(vm.selectedGridRow), vm.modalService, false, 1, vm.updatePositions);
                        }
                    }];
                this.addToLoanCompariosnMenuOption = ['Add To Loan Comparison', function () {
                        var vm = _this;
                        if (vm.selectedGridRow) {
                            vm.selectedGridRow.forCompare = true;
                        }
                    }];
                this.removeFromLoanCompariosnMenuOption = ['Remove From Loan Comparison', function () {
                        var vm = _this;
                        vm.bottomGridApi.selection.clearSelectedRows();
                        vm.topGridApi.selection.clearSelectedRows();
                    }];
                this.filterBy = function (columnIndex) {
                    var vm = _this;
                    if (!vm.topGridApi.grid.columns[columnIndex].filters[0].term) {
                        vm.topGridApi.grid.columns[columnIndex].filters[0].term = true;
                        vm.bottomGridApi.grid.columns[columnIndex].filters[0].term = true;
                    }
                    else {
                        vm.topGridApi.grid.columns[columnIndex].filters[0].term = null;
                        vm.bottomGridApi.grid.columns[columnIndex].filters[0].term = null;
                    }
                    vm.topGridApi.grid.refresh();
                    vm.bottomGridApi.grid.refresh();
                };
                this.loadData = function () {
                    var vm = _this;
                    vm.statusText = "Loading";
                    vm.isLoading = true;
                    vm.dataService.loadRules().then(function (d) {
                        vm.rules = d;
                        if (d.length) {
                            vm.selectedRule = vm.rules[0];
                            if (vm.rootScope['selectedFund']) {
                                vm.selectedFund = vm.rootScope['selectedFund'];
                            }
                            vm.onRuleChanged();
                        }
                    }).then(function () {
                        vm.dataService.getCustomViews().then(function (views) {
                            vm.getSortedCustomViews(views);
                        });
                    });
                };
                this.onRuleChanged = function () {
                    var vm = _this;
                    if (vm.selectedRule) {
                        vm.isLoading = true;
                        vm.dataService.loadTop10Bottom10(vm.selectedFund, vm.selectedRule.ruleId).then(function (d) {
                            if (vm.topGridOptions.data) {
                                vm.topPinnedSecurities = _.map(vm.topGridApi.selection.getSelectedRows(), "securityId");
                            }
                            if (vm.bottomGridOptions.data) {
                                vm.bottomPinnedSecurities = _.map(vm.bottomGridApi.selection.getSelectedRows(), "securityId");
                            }
                            vm.topGridOptions.data = d.topPositions;
                            vm.bottomGridOptions.data = d.bottomPositions;
                            vm.windowService.setTimeout(function () {
                                d.topPositions.forEach(function (p) {
                                    if (vm.topPinnedSecurities.indexOf(p.securityId) >= 0) {
                                        vm.topGridApi.selection.selectRow(p);
                                    }
                                });
                                d.bottomPositions.forEach(function (p) {
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
                            vm.selectedRule.ruleFields.filter(function (r) { return r.ruleSectionType.ruleSectionName === "Top"; }).forEach(function (rf) { return vm.topFields.push(rf.field); });
                            vm.selectedRule.ruleFields.filter(function (r) { return r.ruleSectionType.ruleSectionName === "Bottom"; }).forEach(function (rf) { return vm.bottomFields.push(rf.field); });
                            vm.uiService.createCollectionFilters(vm.topGridOptions.data, vm, 'topFilterObj', 'topFilterCollections', vm.topFields);
                            vm.uiService.processSearchText(vm.topGridOptions.data, vm.topFields);
                            vm.uiService.createCollectionFilters(vm.bottomGridOptions.data, vm, 'bottomFilterObj', 'bottomFilterCollections', vm.bottomFields);
                            vm.uiService.processSearchText(vm.bottomGridOptions.data, vm.bottomFields);
                            vm.uiService.createColumnDefs(vm, 'topGridOptions', 'topFilterCollections', 'highlightFilteredHeader', vm.topFields);
                            vm.uiService.createColumnDefs(vm, 'bottomGridOptions', 'bottomFilterCollections', 'highlightFilteredHeader', vm.bottomFields);
                            vm.isLoading = false;
                        });
                    }
                };
                this.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                    if (col.filters[0].term) {
                        return 'header-filtered';
                    }
                    else {
                        return '';
                    }
                };
                this.sortingAlgorithm = function (a, b, rowA, rowB, direction) {
                    var vm = _this;
                    var nulls = vm.topGridApi.core.sortHandleNulls(a, b);
                    if (nulls !== null) {
                        return nulls;
                    }
                    else {
                        var l = a ? a.toLowerCase() : '', m = b ? b.toLowerCase() : '';
                        return l === m ? 0 : l > m ? 1 : -1;
                    }
                };
                this.refreshGrids = function () {
                    var vm = _this;
                    vm.bottomGridApi.grid.columns[3].filters[0].term = vm.topGridApi.grid.columns[3].filters[0].term;
                    vm.topGridApi.grid.refresh();
                    vm.bottomGridApi.grid.refresh();
                };
                this.sortingNumberAlgorithm = function (a, b, rowA, rowB, direction) {
                    var vm = _this;
                    var nulls = vm.topGridApi.core.sortHandleNulls(a, b);
                    if (nulls !== null) {
                        return nulls;
                    }
                    else {
                        var s3 = a ? a.toString().split([',']).join('') : '';
                        var s4 = b ? b.toString().split(',').join('') : '';
                        if (!(isNaN(s3) && isNaN(s4))) {
                            return Number(s3) - Number(s4);
                        }
                        return 0;
                    }
                };
                this.sortingPercentageAlgorithm = function (a, b, rowA, rowB, direction) {
                    var vm = _this;
                    var nulls = vm.topGridApi.core.sortHandleNulls(a, b);
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
                this.sortingDateAlgorithm = function (a, b, rowA, rowB, direction) {
                    var vm = _this;
                    var nulls = vm.topGridApi.core.sortHandleNulls(a, b);
                    if (nulls !== null) {
                        return nulls;
                    }
                    else {
                        var c = new Date(a);
                        var d = new Date(b);
                        return c - d;
                    }
                };
                this.onFundChanged = function (data) {
                    var vm = _this;
                    vm.selectedFund = data;
                    if (vm.selectedRule.ruleId != 2 || !vm.topGridOptions.data) {
                        vm.onRuleChanged();
                    }
                };
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
                    onRegisterApi: function (gridApi) {
                        vm.topGridApi = gridApi;
                    },
                    appScopeProvider: this,
                    columnDefs: []
                };
                vm.bottomGridOptions = {
                    rowHeight: 20,
                    enableFiltering: true,
                    enablePinning: true,
                    onRegisterApi: function (gridApi) {
                        vm.bottomGridApi = gridApi;
                    },
                    appScopeProvider: this,
                    columnDefs: []
                };
                vm.filter = $filter;
                vm.isLoading = true;
                vm.dataService.loadFixedFields().then(function (fixedfields) {
                    vm.fixedFields = fixedfields;
                    vm.loadData();
                });
                vm.uiGridConstants = uiGridConstants;
                vm.rootScope.$on('onFundSelectionChanged', function (event, data) {
                    vm.onFundChanged(data);
                });
                vm.rootScope.$on('onAutoRefresh', function (event, data) {
                    vm.onFundChanged(data);
                });
            }
            return Top10Bottom10Controller;
        }());
        Top10Bottom10Controller.$inject = ["application.services.uiService", "application.services.dataService", "$rootScope", '$uibModal', '$filter', '$timeout', 'uiGridConstants', '$window'];
        Controllers.Top10Bottom10Controller = Top10Bottom10Controller;
        angular.module("app").controller("application.controllers.top10Bottom10Controller", Top10Bottom10Controller);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=Top10Bottom10Controller.js.map