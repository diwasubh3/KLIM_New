var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var TradeSwappingController = (function () {
            function TradeSwappingController(uiService, dataService, $rootScope, modalService, $filter, timeOutService, $window, uiGridConstants, uiGridTreeViewConstants, $interval) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                this.statusText = "Loading";
                this.gridHeight = { 'height': '402px' };
                this.collectionFilters = {
                    securityInfo: [],
                    totalPar: [],
                    par: [],
                    pctPosition: [],
                    cash: [],
                    spread: [],
                    liquidityScore: [],
                    maturityDate: [],
                    moodyAdjCFR: [],
                    moodyAdjFacility: [],
                    recovery: [],
                    yield: []
                };
                this.dateOptions = {
                    formatYear: 'yy',
                    startingDay: 1
                };
                this.groupBySell = true;
                this.checkForRunningAnalysis = function () {
                    var vm = _this;
                    if (vm.rootScope['activeView'] == 'tradeswapping') {
                        vm.dataService.getLastTradeSwap().then(function (t) {
                            if (t.tradeSwap && t.tradeSwap.tradeSwapId) {
                                if ((!vm.tradeSwapDefinition) ||
                                    (!vm.tradeSwapDefinition.tradeSwapId) ||
                                    vm.tradeSwapDefinition.tradeSwapId != t.tradeSwap.tradeSwapId ||
                                    vm.tradeSwapDefinition.status != t.tradeSwap.status) {
                                    vm.loadData();
                                }
                            }
                        });
                    }
                };
                this.selectAllCriteria = function () {
                    var vm = _this;
                    vm.tradeSwappingParam.criteria.cash = vm.tradeSwappingParam.criteria.selectAll;
                    vm.tradeSwappingParam.criteria.moodyAdjCfr = vm.tradeSwappingParam.criteria.selectAll;
                    //vm.tradeSwappingParam.criteria.moodyAdjFacility = vm.tradeSwappingParam.criteria.selectAll;
                    vm.tradeSwappingParam.criteria.recovery = vm.tradeSwappingParam.criteria.selectAll;
                    vm.tradeSwappingParam.criteria.spread = vm.tradeSwappingParam.criteria.selectAll;
                };
                this.changeGroupBy = function () {
                    var vm = _this;
                    vm.groupBySell = !vm.groupBySell;
                    vm.getTradeSwapSnapshots();
                };
                this.openDate = function ($event, openProp) {
                    var vm = _this;
                    $event.preventDefault();
                    vm[openProp] = true;
                };
                this.createTradeSwap = function () {
                    var vm = _this;
                    vm.isLoading = true;
                    vm.tradeSwappingParam.criteria.fundId = vm.selectedFund.fundId;
                    vm.dataService.startTradeSwap(vm.tradeSwappingParam).then(function (t) {
                        vm.tradeSwapDefinition = t.tradeSwap;
                        vm.isLoading = false;
                        vm.tradeSwappingParam = t.tradeSwapParam;
                    });
                };
                this.checkForTradeSwap = function () {
                    var vm = _this;
                    if (vm.tradeSwapDefinition && vm.tradeSwapDefinition.status === 2) {
                        vm.confirmTradeSwap();
                    }
                    else {
                        vm.createTradeSwap();
                    }
                };
                this.hideUnhideDefinitions = function () {
                    var vm = _this;
                    vm.hideDefinitions = !vm.hideDefinitions;
                    if (vm.hideDefinitions) {
                        vm.gridHeight.height = '592px';
                    }
                    else {
                        vm.gridHeight.height = '402px';
                    }
                };
                this.export = function () {
                    var vm = _this;
                    var myElement = angular.element($(".custom-csv-link-location")[0]);
                    vm.gridApi.exporter.csvExport('all', 'visible', myElement);
                };
                this.loadData = function () {
                    var vm = _this;
                    vm.statusText = "Loading";
                    vm.isLoading = true;
                    vm.gridOptions.data = vm.tradeSnapshots = [];
                    vm.dataService.loadOperators().then(function (operators) {
                        operators.splice(0, 0, { operatorId: null, operatorCode: '' });
                        vm.operators = operators;
                        vm.dataService.getLastTradeSwap().then(function (t) {
                            vm.isLoading = false;
                            if (t) {
                                vm.tradeSwapDefinition = t.tradeSwap;
                                vm.tradeSwappingParam = t.tradeSwapParam;
                                if (vm.tradeSwappingParam.constraints.maturityDate) {
                                    vm.tradeSwappingParam.constraints.maturityDate = new Date(vm.tradeSwappingParam.constraints.maturityDate.toString());
                                }
                                vm.isLoading = false;
                                vm.getTradeSwapSnapshots();
                            }
                        });
                    });
                };
                this.formatTradeSwapSnapshotUI = function (ts) {
                    var vm = _this;
                    ts.totalParFormatted = vm.filter('currency')(ts.totalPar, '', 2);
                    ts.parFormatted = vm.filter('currency')(ts.par, '', 2);
                    ts.pctPositionFormatted = vm.filter('currency')(ts.pctPosition, '', 2);
                    ts.execPriceFormatted = vm.filter('currency')(ts.execPrice, '', 2);
                    ts.spreadFormatted = vm.filter('currency')(ts.spread, '', 2);
                    ts.liquidityScoreFormatted = vm.filter('currency')(ts.liquidityScore, '', 2);
                    ts.creditScoreFormatted = vm.filter('currency')(ts.creditScore, '', 2);
                    ts.maturityDateFormatted = vm.filter('date')(ts.maturityDate, 'MM/dd/yyyy');
                    ts.recoveryFormatted = vm.filter('currency')(ts.recovery, '', 2);
                    ts.yieldFormatted = vm.filter('currency')(ts.yield, '', 2);
                    ts.searchText = ts.securityInfo + ts.totalPar + ts.totalParFormatted + ts.par + ts.parFormatted +
                        ts.pctPosition + ts.pctPositionFormatted +
                        ts.execPrice + ts.execPriceFormatted +
                        ts.spread + ts.spreadFormatted +
                        ts.liquidityScore + ts.liquidityScoreFormatted +
                        ts.maturityDate + ts.maturityDateFormatted + ts.moodyAdjFacility + ts.moodyAdjCFR + ts.recovery + ts.recoveryFormatted + ts.yield + ts.yieldFormatted;
                };
                this.getTradeSwapSnapshots = function () {
                    var vm = _this;
                    if (vm.tradeSwapDefinition && vm.tradeSwapDefinition.status && vm.tradeSwapDefinition.status === 2 && vm.selectedFund && vm.selectedFund.fundId) {
                        vm.isLoading = true;
                        vm.gridOptions.data = [];
                        vm.windowService.setTimeout(function () {
                            vm.dataService.getTradeSnapshots(vm.selectedFund.fundId, vm.tradeSwapDefinition.tradeSwapId, vm.groupBySell ? 0 : 1)
                                .then(function (tradeswapsnapshots) {
                                vm.tradeSnapshots = [];
                                for (var i = 0; i < tradeswapsnapshots.length; i++) {
                                    var ts = tradeswapsnapshots[i];
                                    var tsParentUi = { $$treeLevel: 0 };
                                    if (ts.groupBy === 0) {
                                        tsParentUi.securityInfo = ts.parent.sellSecurityCode + " | " + ts.parent.sellIssuer + " | " + ts.parent.sellFacility;
                                        tsParentUi.totalPar = ts.parent.sellTotalExposure;
                                        tsParentUi.par = ts.parent.sellExposure;
                                        tsParentUi.pctPosition = ts.parent.sellPctPosition;
                                        tsParentUi.execPrice = ts.parent.sellSecurityBidPrice;
                                        tsParentUi.spread = ts.parent.sellSpread;
                                        tsParentUi.liquidityScore = ts.parent.sellLiquidityScore;
                                        tsParentUi.maturityDate = ts.parent.sellMaturityDate;
                                        tsParentUi.moodyAdjCFR = ts.parent.sellMoodyAdjCFR;
                                        tsParentUi.moodyAdjFacility = ts.parent.sellMoodyAdjFacility;
                                        tsParentUi.recovery = ts.parent.sellRecovery;
                                        tsParentUi.yield = ts.parent.sellYield;
                                        tsParentUi.creditScore = ts.parent.sellSecurityCreditScore;
                                        tsParentUi.buySell = 'S';
                                    }
                                    else {
                                        tsParentUi.securityInfo = ts.parent.buySecurityCode + " | " + ts.parent.buyIssuer + " | " + ts.parent.buyFacility;
                                        tsParentUi.totalPar = ts.parent.buyTotalExposure;
                                        tsParentUi.par = ts.parent.buyExposure;
                                        tsParentUi.pctPosition = ts.parent.buyPctPosition;
                                        tsParentUi.execPrice = ts.parent.buySecurityOfferPrice;
                                        tsParentUi.spread = ts.parent.buySpread;
                                        tsParentUi.liquidityScore = ts.parent.buyLiquidityScore;
                                        tsParentUi.maturityDate = ts.parent.buyMaturityDate;
                                        tsParentUi.moodyAdjCFR = ts.parent.buyMoodyAdjFacility;
                                        tsParentUi.moodyAdjFacility = ts.parent.buyMoodyAdjFacility;
                                        tsParentUi.recovery = ts.parent.buyRecovery;
                                        tsParentUi.yield = ts.parent.buyYield;
                                        tsParentUi.creditScore = ts.parent.buySecurityCreditScore;
                                        tsParentUi.buySell = 'B';
                                    }
                                    vm.formatTradeSwapSnapshotUI(tsParentUi);
                                    vm.tradeSnapshots.push(tsParentUi);
                                    for (var j = 0; j < ts.children.length; j++) {
                                        var tsChildUi = {};
                                        var tschild = ts.children[j];
                                        if (ts.groupBy === 0) {
                                            tsChildUi.securityInfo = tschild.buySecurityCode + " | " + tschild.buyIssuer + " | " + tschild.buyFacility;
                                            tsChildUi.totalPar = tschild.buyTotalExposure;
                                            tsChildUi.par = tschild.buyExposure;
                                            tsChildUi.pctPosition = tschild.buyPctPosition;
                                            tsChildUi.execPrice = tschild.buySecurityOfferPrice;
                                            tsChildUi.spread = tschild.buySpread;
                                            tsChildUi.liquidityScore = tschild.buyLiquidityScore;
                                            tsChildUi.maturityDate = tschild.buyMaturityDate;
                                            tsChildUi.moodyAdjCFR = tschild.buyMoodyAdjCFR;
                                            tsChildUi.moodyAdjFacility = tschild.buyMoodyAdjFacility;
                                            tsChildUi.yield = tschild.buyYield;
                                            tsChildUi.recovery = tschild.buyRecovery;
                                            tsChildUi.creditScore = tschild.buySecurityCreditScore;
                                            tsChildUi.buySell = 'B';
                                        }
                                        else {
                                            tsChildUi.securityInfo = tschild.sellSecurityCode + " | " + tschild.sellIssuer + " | " + tschild.sellFacility;
                                            tsChildUi.totalPar = tschild.sellTotalExposure;
                                            tsChildUi.par = tschild.sellExposure;
                                            tsChildUi.pctPosition = tschild.sellPctPosition;
                                            tsChildUi.execPrice = tschild.sellSecurityBidPrice;
                                            tsChildUi.spread = tschild.sellSpread;
                                            tsChildUi.liquidityScore = tschild.sellLiquidityScore;
                                            tsChildUi.maturityDate = tschild.sellMaturityDate;
                                            tsChildUi.moodyAdjCFR = tschild.sellMoodyAdjCFR;
                                            tsChildUi.moodyAdjFacility = tschild.sellMoodyAdjFacility;
                                            tsChildUi.recovery = tschild.sellRecovery;
                                            tsChildUi.yield = tschild.sellYield;
                                            tsChildUi.creditScore = tschild.sellSecurityCreditScore;
                                            tsChildUi.buySell = 'S';
                                        }
                                        vm.formatTradeSwapSnapshotUI(tsChildUi);
                                        vm.tradeSnapshots.push(tsChildUi);
                                    }
                                }
                                vm.collectionFilters.searchText = _.map(_.sortBy(_.uniq(_.map(_.flatten(vm.tradeSnapshots), "searchText")), function (x) { return x; }), function (x) { return { label: x, value: x }; });
                                vm.collectionFilters.securityInfo = _.map(_.sortBy(_.uniq(_.map(_.flatten(vm.tradeSnapshots), "securityInfo")), function (x) { return x; }), function (x) { return { label: x, value: x }; });
                                vm.collectionFilters.totalPar = _.map(_.sortBy(_.uniq(_.map(_.flatten(vm.tradeSnapshots), "totalPar")), function (x) { return x; }), function (x) { return { label: vm.filter('currency')(x, '', 2), value: x, format: 'currency:2' }; });
                                vm.collectionFilters.par = _.map(_.sortBy(_.uniq(_.map(_.flatten(vm.tradeSnapshots), "par")), function (x) { return x; }), function (x) { return { label: vm.filter('currency')(x, '', 2), value: x, format: 'currency:2' }; });
                                vm.collectionFilters.pctPosition = _.map(_.sortBy(_.uniq(_.map(_.flatten(vm.tradeSnapshots), "pctPosition")), function (x) { return x; }), function (x) { return { label: vm.filter('currency')(x, '', 2), value: x, format: 'currency:2' }; });
                                vm.collectionFilters.execPrice = _.map(_.sortBy(_.uniq(_.map(_.flatten(vm.tradeSnapshots), "execPrice")), function (x) { return x; }), function (x) { return { label: vm.filter('currency')(x, '', 2), value: x, format: 'currency:2' }; });
                                vm.collectionFilters.spread = _.map(_.sortBy(_.uniq(_.map(_.flatten(vm.tradeSnapshots), "spread")), function (x) { return x; }), function (x) { return { label: vm.filter('currency')(x, '', 2), value: x, format: 'currency:2' }; });
                                vm.collectionFilters.liquidityScore = _.map(_.sortBy(_.uniq(_.map(_.flatten(vm.tradeSnapshots), "liquidityScore")), function (x) { return x; }), function (x) { return { label: vm.filter('currency')(x, '', 2), value: x, format: 'currency:2' }; });
                                vm.collectionFilters.creditScore = _.map(_.sortBy(_.uniq(_.map(_.flatten(vm.tradeSnapshots), "creditScore")), function (x) { return x; }), function (x) { return { label: vm.filter('currency')(x, '', 2), value: x, format: 'currency:2' }; });
                                vm.collectionFilters.maturityDate = _.map(_.sortBy(_.uniq(_.map(_.flatten(vm.tradeSnapshots), "maturityDate")), function (x) { return x; }), function (x) { return { label: vm.filter('date')(x, 'MM/dd/yyyy'), value: x, format: 'date' }; });
                                vm.collectionFilters.moodyAdjCFR = _.map(_.sortBy(_.uniq(_.map(_.flatten(vm.tradeSnapshots), "moodyAdjCFR")), function (x) { return x; }), function (x) { return { label: x, value: x }; });
                                vm.collectionFilters.moodyAdjFacility = _.map(_.sortBy(_.uniq(_.map(_.flatten(vm.tradeSnapshots), "moodyAdjFacility")), function (x) { return x; }), function (x) { return { label: x, value: x }; });
                                vm.collectionFilters.recovery = _.map(_.sortBy(_.uniq(_.map(_.flatten(vm.tradeSnapshots), "recovery")), function (x) { return x; }), function (x) { return { label: vm.filter('currency')(x, '', 2), value: x, format: 'currency:2' }; });
                                vm.collectionFilters.yield = _.map(_.sortBy(_.uniq(_.map(_.flatten(vm.tradeSnapshots), "yield")), function (x) { return x; }), function (x) { return { label: vm.filter('currency')(x, '', 2), value: x, format: 'currency:2' }; });
                                vm.gridOptions.columnDefs = [
                                    {
                                        name: 'searchText',
                                        width: '100',
                                        visible: false,
                                        displayName: 'SearchText',
                                        filter: {
                                            type: vm.uiGridConstants.filter.SELECT,
                                            selectOptions: vm.collectionFilters.searchText
                                        },
                                        filterHeaderTemplate: '<div class="ui-grid-filter-container" ng-repeat="colFilter in col.filters"><div custom-modal-filter></div></div>'
                                    },
                                    {
                                        name: 'securityInfo',
                                        width: '*',
                                        pinnedLeft: true,
                                        enablePinning: true,
                                        displayName: 'LOANX ID/CUSIP | ISSUER | FACILITY',
                                        cellTemplate: '<div class="ui-grid-cell-contents" ><b class="buy" ng-if="row.entity.buySell==\'B\'">B</b><b class="sell" ng-if="row.entity.buySell==\'S\'">S</b>&nbsp;&nbsp;&nbsp;&nbsp;{{grid.getCellValue(row, col)}}</div>',
                                        filter: {
                                            type: vm.uiGridConstants.filter.SELECT,
                                            selectOptions: vm.collectionFilters.securityInfo
                                        },
                                        filterHeaderTemplate: '<div class="ui-grid-filter-container" ng-repeat="colFilter in col.filters"><div custom-modal-filter></div></div>',
                                        headerCellClass: vm.highlightFilteredHeader
                                    },
                                    {
                                        name: 'totalParFormatted',
                                        width: '105',
                                        displayName: 'TOTAL EXPOSURE',
                                        cellClass: 'text-right',
                                        filter: {
                                            type: vm.uiGridConstants.filter.SELECT,
                                            selectOptions: vm.collectionFilters.totalPar
                                        },
                                        cellTemplate: '<div class="ui-grid-cell-contents" >{{row.entity.totalParFormatted}}</div>',
                                        headerCellClass: vm.highlightFilteredHeader,
                                        filterHeaderTemplate: '<div class="ui-grid-filter-container" ng-repeat="colFilter in col.filters"><div custom-modal-filter></div></div>'
                                    },
                                    {
                                        name: 'parFormatted',
                                        width: '100',
                                        displayName: vm.selectedFund.fundCode + ' ' + 'EXPOSURE',
                                        cellClass: 'text-right',
                                        filter: {
                                            type: vm.uiGridConstants.filter.SELECT,
                                            selectOptions: vm.collectionFilters.par
                                        },
                                        cellTemplate: '<div class="ui-grid-cell-contents" >{{row.entity.parFormatted}}</div>',
                                        headerCellClass: vm.highlightFilteredHeader,
                                        filterHeaderTemplate: '<div class="ui-grid-filter-container" ng-repeat="colFilter in col.filters"><div custom-modal-filter></div></div>'
                                    },
                                    {
                                        name: 'pctPositionFormatted',
                                        width: '80',
                                        displayName: '% POSITION',
                                        cellClass: 'text-right',
                                        filter: {
                                            type: vm.uiGridConstants.filter.SELECT,
                                            selectOptions: vm.collectionFilters.pctPosition
                                        },
                                        cellTemplate: '<div class="ui-grid-cell-contents" >{{row.entity.pctPositionFormatted}}</div>',
                                        headerCellClass: vm.highlightFilteredHeader,
                                        filterHeaderTemplate: '<div class="ui-grid-filter-container" ng-repeat="colFilter in col.filters"><div custom-modal-filter></div></div>'
                                    }
                                ];
                                if (vm.tradeSwappingParam.criteria.cash) {
                                    vm.gridOptions.columnDefs.push({
                                        name: 'execPriceFormatted',
                                        width: '100',
                                        displayName: 'EXEC PRICE',
                                        cellClass: 'text-right',
                                        filter: {
                                            type: vm.uiGridConstants.filter.SELECT,
                                            selectOptions: vm.collectionFilters.execPrice
                                        },
                                        cellTemplate: '<div class="ui-grid-cell-contents" >{{row.entity.execPriceFormatted}}</div>',
                                        headerCellClass: vm.highlightFilteredHeader,
                                        filterHeaderTemplate: '<div class="ui-grid-filter-container" ng-repeat="colFilter in col.filters"><div custom-modal-filter></div></div>'
                                    });
                                }
                                if (vm.tradeSwappingParam.criteria.moodyAdjCfr || vm.tradeSwappingParam.constraints.moodyAdjCfrRank) {
                                    vm.gridOptions.columnDefs.push({
                                        name: 'moodyAdjCFR',
                                        width: '105',
                                        displayName: 'MOODY\'s ADJ CFR',
                                        filter: {
                                            type: vm.uiGridConstants.filter.SELECT,
                                            selectOptions: vm.collectionFilters.moodyAdjCFR
                                        },
                                        headerCellClass: vm.highlightFilteredHeader,
                                        filterHeaderTemplate: '<div class="ui-grid-filter-container" ng-repeat="colFilter in col.filters"><div custom-modal-filter></div></div>'
                                    });
                                }
                                if (vm.tradeSwappingParam.criteria.spread) {
                                    vm.gridOptions.columnDefs.push({
                                        name: 'spreadFormatted',
                                        width: '60',
                                        displayName: 'SPREAD',
                                        cellClass: 'text-right',
                                        filter: {
                                            type: vm.uiGridConstants.filter.SELECT,
                                            selectOptions: vm.collectionFilters.spread
                                        },
                                        cellTemplate: '<div class="ui-grid-cell-contents" >{{row.entity.spreadFormatted}}</div>',
                                        headerCellClass: vm.highlightFilteredHeader,
                                        filterHeaderTemplate: '<div class="ui-grid-filter-container" ng-repeat="colFilter in col.filters"><div custom-modal-filter></div></div>'
                                    });
                                }
                                if (vm.tradeSwappingParam.constraints.liquidityScoreOperatorId && vm.tradeSwappingParam.constraints.liquidityScore != null) {
                                    vm.gridOptions.columnDefs.push({
                                        name: 'liquidityScoreFormatted',
                                        width: '100',
                                        displayName: 'LIQUIDITY SCORE',
                                        cellClass: 'text-right',
                                        filter: {
                                            type: vm.uiGridConstants.filter.SELECT,
                                            selectOptions: vm.collectionFilters.liquidityScore
                                        },
                                        cellTemplate: '<div class="ui-grid-cell-contents" >{{row.entity.liquidityScoreFormatted}}</div>',
                                        headerCellClass: vm.highlightFilteredHeader,
                                        filterHeaderTemplate: '<div class="ui-grid-filter-container" ng-repeat="colFilter in col.filters"><div custom-modal-filter></div></div>'
                                    });
                                }
                                if (vm.tradeSwappingParam.constraints.creditScoreOperatorId && vm.tradeSwappingParam.constraints.creditScore != null) {
                                    vm.gridOptions.columnDefs.push({
                                        name: 'creditScoreFormatted',
                                        width: '100',
                                        displayName: 'CREDIT SCORE',
                                        cellClass: 'text-right',
                                        filter: {
                                            type: vm.uiGridConstants.filter.SELECT,
                                            selectOptions: vm.collectionFilters.creditScore
                                        },
                                        cellTemplate: '<div class="ui-grid-cell-contents" >{{row.entity.creditScoreFormatted}}</div>',
                                        headerCellClass: vm.highlightFilteredHeader,
                                        filterHeaderTemplate: '<div class="ui-grid-filter-container" ng-repeat="colFilter in col.filters"><div custom-modal-filter></div></div>'
                                    });
                                }
                                if (vm.tradeSwappingParam.constraints.maturityDateOperatorId && vm.tradeSwappingParam.constraints.maturityDate != null) {
                                    vm.gridOptions.columnDefs.push({
                                        name: 'maturityDateFormatted',
                                        width: '100',
                                        cellFilter: 'date:"MM/dd/yyyy"',
                                        displayName: 'MATURITY DATE',
                                        filter: {
                                            type: vm.uiGridConstants.filter.SELECT,
                                            selectOptions: vm.collectionFilters.maturityDate
                                        },
                                        cellTemplate: '<div class="ui-grid-cell-contents" >{{row.entity.maturityDateFormatted}}</div>',
                                        headerCellClass: vm.highlightFilteredHeader,
                                        filterHeaderTemplate: '<div class="ui-grid-filter-container" ng-repeat="colFilter in col.filters"><div custom-modal-filter></div></div>'
                                    });
                                }
                                if (vm.tradeSwappingParam.criteria.recovery ||
                                    (vm.tradeSwappingParam.constraints.recoveryOperatorId && vm.tradeSwappingParam.constraints.recovery != null)) {
                                    vm.gridOptions.columnDefs.push({
                                        name: 'recoveryFormatted',
                                        width: '100',
                                        displayName: 'RECOVERY',
                                        cellClass: 'text-right',
                                        filter: {
                                            type: vm.uiGridConstants.filter.SELECT,
                                            selectOptions: vm.collectionFilters.recovery
                                        },
                                        cellTemplate: '<div class="ui-grid-cell-contents" >{{row.entity.recoveryFormatted}}</div>',
                                        headerCellClass: vm.highlightFilteredHeader,
                                        filterHeaderTemplate: '<div class="ui-grid-filter-container" ng-repeat="colFilter in col.filters"><div custom-modal-filter></div></div>'
                                    });
                                }
                                if (vm.tradeSwappingParam.constraints.yieldOperatorId && vm.tradeSwappingParam.constraints.yield != null) {
                                    vm.gridOptions.columnDefs.push({
                                        name: 'yieldFormatted',
                                        width: '75',
                                        displayName: 'YIELD',
                                        cellClass: 'text-right',
                                        filter: {
                                            type: vm.uiGridConstants.filter.SELECT,
                                            selectOptions: vm.collectionFilters.yield
                                        },
                                        cellTemplate: '<div class="ui-grid-cell-contents" >{{row.entity.yieldFormatted}}</div>',
                                        headerCellClass: vm.highlightFilteredHeader,
                                        filterHeaderTemplate: '<div class="ui-grid-filter-container" ng-repeat="colFilter in col.filters"><div custom-modal-filter></div></div>'
                                    });
                                }
                                vm.gridOptions.data = vm.tradeSnapshots;
                                vm.isLoading = false;
                            });
                        });
                    }
                };
                this.refreshGrid = function () {
                    var vm = _this;
                    vm.gridApi.grid.refresh();
                };
                this.highlightFilteredHeader = function (row, rowRenderIndex, col, colRenderIndex) {
                    if (col.filters[0].term) {
                        return 'header-filtered';
                    }
                    else {
                        return '';
                    }
                };
                this.confirmTradeSwap = function () {
                    var vm = _this;
                    var modalInstance = vm.modalService.open({
                        templateUrl: pageOptions.appBasePath + 'app/views/confirmtradeswap.html?v=' + pageOptions.appVersion,
                        controller: 'application.controllers.confirmTradeSwapRunController',
                        controllerAs: 'confirmtradeswap',
                        size: 'md',
                        resolve: {
                            sourcedata: function () {
                                return vm.tradeSwapDefinition;
                            }
                        }
                    });
                    modalInstance.result.then(function (confirm) {
                        if (confirm) {
                            vm.createTradeSwap();
                        }
                    }, function () { });
                };
                this.onFundChanged = function (data) {
                    var vm = _this;
                    vm.selectedFund = data;
                    vm.windowService.setTimeout(function () {
                        vm.loadData();
                    });
                };
                var vm = this;
                this.gridOptions = {
                    rowHeight: 20,
                    enableFiltering: true,
                    enablePinning: true,
                    enableSorting: true,
                    showTreeExpandNoChildren: true,
                    onRegisterApi: function (gridApi) {
                        vm.gridApi = gridApi;
                        console.log('REGISTERD');
                    },
                    appScopeProvider: this
                };
                vm.dataService = dataService;
                vm.uiGridConstants = uiGridConstants;
                vm.uiService = uiService;
                vm.rootScope = $rootScope;
                vm.rootScope.$emit('onActivated', 'tradeswapping');
                vm.modalService = modalService;
                vm.interval = $interval;
                vm.filter = $filter;
                vm.windowService = $window;
                vm.isLoading = true;
                vm.uiGridTreeViewConstants = uiGridTreeViewConstants;
                vm.dataService.getMoodyRatings().then(function (ratings) {
                    ratings.splice(0, 0, { rank: null, ratingDesc: '' });
                    vm.moodyRatings = ratings;
                    if (vm.rootScope['selectedFund']) {
                        vm.selectedFund = vm.rootScope['selectedFund'];
                        vm.loadData();
                    }
                    vm.rootScope.$on('onFundSelectionChanged', function (event, data) {
                        vm.onFundChanged(data);
                    });
                    vm.rootScope.$on('onAutoRefresh', function (event, data) {
                        vm.onFundChanged(data);
                    });
                    vm.interval(vm.checkForRunningAnalysis, 3000);
                });
            }
            return TradeSwappingController;
        }());
        TradeSwappingController.$inject = ["application.services.uiService", "application.services.dataService", "$rootScope", '$uibModal', '$filter', '$timeout', '$window', 'uiGridConstants', 'uiGridTreeViewConstants', '$interval'];
        Controllers.TradeSwappingController = TradeSwappingController;
        angular.module("app").controller("application.controllers.tradeSwappingController", TradeSwappingController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=TradeSwappingController.js.map