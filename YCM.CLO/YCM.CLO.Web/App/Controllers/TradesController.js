var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var TradesController = (function () {
            function TradesController(uiService, dataService, $rootScope, modalService, ngTableParams, $filter, timeOutService) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                this.statusText = "Loading";
                this.includeCancelled = false;
                this.getPositionFromTrade = function (trade) {
                    var security = trade['security'];
                    security.watchId = trade.watchId;
                    security.isOnWatch = trade.isOnWatch;
                    security.watchObjectTypeId = trade.watchObjectTypeId;
                    security.watchObjectId = trade.watchObjectId;
                    security.watchComments = trade.watchComments;
                    security.watchLastUpdatedOn = trade.watchLastUpdatedOn;
                    security.watchUser = trade.watchUser;
                    return security;
                };
                this.showMenu = function (trade) {
                    var vm = _this;
                    vm.selectedRow = trade;
                    var menus = [];
                    if (trade.isBuy) {
                        menus.push(_this.buyMenuOption);
                    }
                    else if (trade.isSell) {
                        menus.push(_this.sellMenuOption);
                    }
                    menus.push(null);
                    //if (trade.forCompare) {
                    //    menus.push(this.removeFromLoanCompariosnMenuOption);
                    //}
                    //else {
                    //    menus.push(this.addToLoanCompariosnMenuOption);
                    //}    
                    if (trade.isOnWatch) {
                        menus.push(_this.editWatchListMenuOption);
                        menus.push(_this.offWatchListMenuOption);
                    }
                    else {
                        menus.push(_this.onWatchListMenuOption);
                    }
                    return menus;
                };
                this.buyMenuOption = ['Buy', function () {
                        var vm = _this;
                        if (vm.selectedRow) {
                            vm.uiService.showBuySellModal(vm.selectedFund, vm.getPositionFromTrade(vm.selectedRow), vm.modalService, true, vm.loadDataWithSummaryRefresh);
                        }
                    }];
                this.sellMenuOption = ['Sell', function () {
                        var vm = _this;
                        if (vm.selectedRow) {
                            vm.uiService.showBuySellModal(vm.selectedFund, vm.getPositionFromTrade(vm.selectedRow), vm.modalService, false, vm.loadDataWithSummaryRefresh);
                        }
                    }];
                this.addToLoanCompariosnMenuOption = ['Add To Loan Comparison', function () {
                        var vm = _this;
                        if (vm.selectedRow) {
                            vm.selectedRow.forCompare = true;
                        }
                    }];
                this.select = function (trade) {
                    trade.forCompare = !trade.forCompare;
                };
                this.removeFromLoanCompariosnMenuOption = ['Remove From Loan Comparison', function () {
                        var vm = _this;
                        if (vm.selectedRow) {
                            vm.selectedRow.forCompare = false;
                        }
                    }];
                this.onWatchListMenuOption = [
                    'On Watch List', function () {
                        var vm = _this;
                        if (vm.selectedRow) {
                            vm.uiService.showWatchModal(vm.getPositionFromTrade(vm.selectedRow), vm.modalService, false, 1, vm.updateTradeFromPosition);
                        }
                    }
                ];
                this.offWatchListMenuOption = ['Off Watch List', function () {
                        var vm = _this;
                        if (vm.selectedRow) {
                            vm.uiService.showWatchModal(vm.getPositionFromTrade(vm.selectedRow), vm.modalService, true, 1, vm.updateTradeFromPosition);
                        }
                    }];
                this.editWatchListMenuOption = ['Edit Watch List', function () {
                        var vm = _this;
                        if (vm.selectedRow) {
                            vm.uiService.showWatchModal(vm.getPositionFromTrade(vm.selectedRow), vm.modalService, false, 1, vm.updateTradeFromPosition);
                        }
                    }];
                this.updateTradeFromPosition = function (updatedPositions) {
                    var vm = _this;
                    if (updatedPositions.length) {
                        updatedPositions.forEach(function (position) {
                            vm.data.forEach(function (trade) {
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
                };
                this.clearLoanComparisons = function (positions) {
                    var vm = _this;
                    vm.data.filter(function (f) { return f.forCompare; }).forEach(function (p) { p.forCompare = false; });
                };
                this.comapreLoans = function () {
                    var vm = _this;
                    var positionsForComparison = vm.data.filter(function (f) { return f.forCompare; });
                    if (positionsForComparison.length) {
                        vm.uiService.showLoanComparisonModal(vm.rootScope['selectedFund'], -1, positionsForComparison, vm.customViews, vm.modalService, vm.clearLoanComparisons);
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
                this.loadHeaderFields = function () {
                    var vm = _this;
                    vm.isLoading = true;
                    vm.statusText = "Loading";
                    vm.dataService.getTradesHeaderFields().then(function (headers) {
                        vm.headerFields = headers;
                        vm.loadData();
                    });
                };
                this.editTrade = function (trade) {
                    var vm = _this;
                    var tr = trade ? trade : { tradeAllocations: [] };
                    vm.uiService.showTradeModel(tr, vm.modalService, vm.resetTrade);
                };
                this.resetTrade = function (trade) {
                    var vm = _this;
                    var trades = vm.data.filter(function (tr) { return tr.tradeId == trade.tradeId; });
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
                };
                this.loadData = function () {
                    var vm = _this;
                    vm.statusText = "Loading";
                    vm.isLoading = true;
                    if (vm.selectedFund) {
                        vm.dataService.getTrades(vm.includeCancelled, vm.selectedFund.fundCode).then(function (d) {
                            if (vm.data) {
                                var pinnedSecurities = _.map(vm.data.filter(function (p) { return p.forCompare; }), "securityId");
                                d.forEach(function (p) {
                                    if (pinnedSecurities.indexOf(p.securityId) >= 0) {
                                        p.forCompare = true;
                                    }
                                });
                            }
                            vm.data = d;
                            vm.data.forEach(function (t) {
                                vm.uiService.processTooltip(t);
                            });
                            vm.isLoading = false;
                            vm.setParamsTable();
                        }).then(function () {
                            vm.dataService.getCustomViews().then(function (views) {
                                vm.getSortedCustomViews(views);
                            });
                        });
                    }
                    else {
                        vm.isLoading = false;
                    }
                };
                this.loadDataWithSummaryRefresh = function () {
                    var vm = _this;
                    vm.rootScope.$emit('refreshSummaries', null);
                    vm.loadData();
                };
                this.sortTableParams = function (columnName) {
                    var vm = _this;
                    var sortObj = {};
                    sortObj[columnName] = vm.tableParams.isSortBy(columnName, 'asc') ? 'desc' : 'asc';
                    vm.tableParams.sorting(sortObj);
                };
                this.getStyle = function (column, data, index) {
                    var style = {};
                    style['width'] = column.displayWidth;
                    if (data) {
                        if (index == 0) {
                            style['border-width'] = '0 0 0 0';
                        }
                        else {
                            style['border-width'] = '1px 0 0px 0';
                        }
                    }
                    if (data && data.isHistorical) {
                        style['color'] = 'lightgray';
                    }
                    if (column.jsonPropertyName != 'securityName') {
                        style['text-align'] = 'right';
                    }
                    return style;
                };
                this.resetFilter = function (prop) {
                    var vm = _this;
                    if (vm.tableParams.filter()[prop]) {
                        vm.tableParams.filter()[prop] = undefined;
                    }
                    else {
                        vm.tableParams.filter()[prop] = true;
                    }
                };
                this.setParamsTable = function () {
                    var vm = _this;
                    var filterSearchText = function (data, filterValues) {
                        var filtered = data;
                        if (filterValues.searchText) {
                            filterValues.searchText = filterValues.searchText.toLowerCase();
                            filtered = filtered.filter(function (item) {
                                item.tradeAllocations.forEach(function (s) {
                                    s['isVisible'] = !filterValues.searchText || s.searchText.indexOf(filterValues.searchText) >= 0;
                                });
                                return item.tradeAllocations.filter(function (s) { return s['isVisible']; }).length;
                            });
                        }
                        if (filterValues.isOnWatch) {
                            filtered = filtered.filter(function (item) {
                                return item.isOnWatch == true;
                            });
                        }
                        if (filterValues.isOnAlert) {
                            filtered = filtered.filter(function (item) {
                                return item.isOnAlert == true;
                            });
                        }
                        if (filterValues.isBuy) {
                            filtered = filtered.filter(function (item) {
                                return item.isBuy == true;
                            });
                        }
                        if (filterValues.isSell) {
                            filtered = filtered.filter(function (item) {
                                return item.isSell == true;
                            });
                        }
                        return filtered;
                    };
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
                };
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
                vm.rootScope.$on('onFundSelectionChanged', function (event, data) {
                    vm.selectedFund = data;
                    if (!vm.selectedFund) {
                        vm.loadData();
                    }
                });
                vm.rootScope.$on('onAutoRefresh', function (event, data) {
                    vm.selectedFund = data;
                    vm.loadData();
                });
                vm.isLoading = true;
                vm.loadHeaderFields();
            }
            return TradesController;
        }());
        TradesController.$inject = ["application.services.uiService", "application.services.dataService", "$rootScope", '$uibModal', 'NgTableParams', '$filter', '$timeout'];
        Controllers.TradesController = TradesController;
        angular.module("app").controller("application.controllers.tradesController", TradesController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=TradesController.js.map