var Application;
(function (Application) {
    var Services;
    (function (Services) {
        var UIService = (function () {
            function UIService(httpWrapperFactory, uiGridConstants, $sce, modalService, $filter) {
                var _this = this;
                this.currentHeight = screen.availHeight - (282 + (24 + (22 * 4))) + (yorkCore.isMsIe ? 35 : 0);
                this.getHeaderTemplate = function () {
                    return '<div class="ngHeaderSortColumn {{col.headerClass}}" ng-style="{cursor: col.cursor}" ng-class="{ ngSorted: !noSortVisible }">' +
                        '<div ng-click="col.sort($event)" ng-class="\'colt\' + col.index" class="ngHeaderText">{{col.displayName}}</div>' +
                        '<div class="ngSortButtonDown" ng-show="col.showSortButtonDown()"></div>' +
                        '<div class="ngSortButtonUp" ng-show="col.showSortButtonUp()"></div>' +
                        '<div class="ngSortPriority">{{col.sortPriority}}</div>' +
                        '</div>' +
                        '<div ng-show="col.resizable" class="ngHeaderGrip" ng-click="col.gripClick($event)" ng-mousedown="col.gripOnMouseDown($event)"></div>';
                };
                this.capitalizeFirstLetter = function (match) {
                    var result = match;
                    if (match) {
                        result = (angular.uppercase(match.charAt(0)) + match.slice(1));
                    }
                    return result;
                };
                this.filterPositions = function (positions, positionFilter) {
                    var vm = _this;
                    positions.forEach(function (pos) {
                        pos.isFilterSuccess = true;
                        if (positionFilter.fixedfields) {
                            positionFilter.fixedfields.forEach(function (posfilter) {
                                if (posfilter.lowerBound.operator.operatorVal && typeof (posfilter.lowerBound.value) != 'undefined' && (posfilter.lowerBound.value || posfilter.lowerBound.value.toString().length)) {
                                    vm.filterPosition(pos, posfilter.lowerBound, posfilter.field, positionFilter);
                                }
                                if (posfilter.upperBound.operator.operatorVal && typeof (posfilter.upperBound.value) != 'undefined' && (posfilter.upperBound.value || posfilter.upperBound.value.toString().length)) {
                                    vm.filterPosition(pos, posfilter.upperBound, posfilter.field, positionFilter);
                                }
                            });
                        }
                        if (positionFilter.security) {
                            positionFilter.security.forEach(function (posfilter) {
                                if (posfilter.lowerBound.operator.operatorVal && typeof (posfilter.lowerBound.value) != 'undefined' && (posfilter.lowerBound.value || posfilter.lowerBound.value.toString().length)) {
                                    vm.filterPosition(pos, posfilter.lowerBound, posfilter.field, positionFilter);
                                }
                                if (posfilter.upperBound.operator.operatorVal && typeof (posfilter.upperBound.value) != 'undefined' && (posfilter.upperBound.value || posfilter.upperBound.value.toString().length)) {
                                    vm.filterPosition(pos, posfilter.upperBound, posfilter.field, positionFilter);
                                }
                            });
                        }
                        if (positionFilter.ratings) {
                            positionFilter.ratings.forEach(function (posfilter) {
                                if (posfilter.lowerBound.operator.operatorVal && typeof (posfilter.lowerBound.value) != 'undefined' && (posfilter.lowerBound.value || posfilter.lowerBound.value.toString().length)) {
                                    vm.filterPosition(pos, posfilter.lowerBound, posfilter.field, positionFilter);
                                }
                                if (posfilter.upperBound.operator.operatorVal && typeof (posfilter.upperBound.value) != 'undefined' && (posfilter.upperBound.value || posfilter.upperBound.value.toString().length)) {
                                    vm.filterPosition(pos, posfilter.upperBound, posfilter.field, positionFilter);
                                }
                            });
                        }
                        if (positionFilter.analyst) {
                            positionFilter.analyst.forEach(function (posfilter) {
                                if (posfilter.lowerBound.operator.operatorVal && typeof (posfilter.lowerBound.value) != 'undefined' && (posfilter.lowerBound.value || posfilter.lowerBound.value.toString().length)) {
                                    vm.filterPosition(pos, posfilter.lowerBound, posfilter.field, positionFilter);
                                }
                                if (posfilter.upperBound.operator.operatorVal && typeof (posfilter.upperBound.value) != 'undefined' && (posfilter.upperBound.value || posfilter.upperBound.value.toString().length)) {
                                    vm.filterPosition(pos, posfilter.upperBound, posfilter.field, positionFilter);
                                }
                            });
                        }
                    });
                };
                this.sortArrayBySortOrderAsc = function (arrayToSort, sortField) {
                    return arrayToSort.sort(function (a, b) {
                        return a[sortField] - b[sortField];
                    });
                };
                this.filterPosition = function (pos, posfilter, field, positionFilter) {
                    if (pos.isFilterSuccess && posfilter.operator && posfilter.operator.operatorVal && (posfilter.value || posfilter.value.toString().length)) {
                        pos.isFilterSuccess = pos.isFilterSuccess && (pos[field.jsonPropertyName] != null && pos[field.jsonPropertyName].toString().length > 0);
                        if (pos.isFilterSuccess && field.fieldType == 3) {
                            var filterdate = new Date(pos[field.jsonPropertyName]);
                            var dateexpression = 'new Date( "' + pos[field.jsonPropertyName].toString() + '") ' + ' ' +
                                posfilter.operator.operatorVal + ' new Date("' + posfilter.value + '")';
                            pos.isFilterSuccess = pos.isFilterSuccess && eval(dateexpression);
                        }
                        else if (pos.isFilterSuccess && field.fieldType == 4) {
                            var value = pos[field.jsonPropertyName].toString().replace(/ %/g, '');
                            var expression = parseFloat(value).toString() + ' ' +
                                posfilter.operator.operatorVal + ' ' + posfilter.value;
                            pos.isFilterSuccess = pos.isFilterSuccess && eval(expression);
                        }
                        else if (pos.isFilterSuccess && field.fieldType == 2) {
                            debugger;
                            var value = pos[field.jsonPropertyName].toString().replace(/,/g, '');
                            var expression = parseFloat(value).toString() + ' ' +
                                posfilter.operator.operatorVal + ' ' + posfilter.value;
                            pos.isFilterSuccess = pos.isFilterSuccess && eval(expression);
                        }
                        else if (pos.isFilterSuccess && field.fieldType == 1 && field.fieldGroupId == 2) {
                            var ratingFiledValue = positionFilter.ratingsDictionary[pos[field.jsonPropertyName].toString().replace(/,/g, '')];
                            var ratingFilterValue = posfilter.value.toString();
                            if (ratingFilterValue == undefined) {
                                ratingFilterValue = -2;
                            }
                            if (ratingFiledValue == undefined) {
                                ratingFiledValue = -2;
                            }
                            var expression = parseFloat(ratingFiledValue).toString() + ' ' +
                                posfilter.operator.operatorVal + ' ' + ratingFilterValue;
                            pos.isFilterSuccess = pos.isFilterSuccess && eval(expression);
                        }
                    }
                };
                this.updateFilterStatistics = function (positions, positionFilter, field) {
                    var vm = _this;
                    vm.filterPositions(positions, positionFilter);
                    var filtered = positions.filter(function (x) { return x.isFilterSuccess; });
                    if (filtered && filtered.length) {
                        var distincts = vm.getDistinctStrings(field, filtered);
                        positionFilter.borrowerCount = distincts.length;
                        positionFilter.loanCount = filtered.length;
                    }
                    else {
                        positionFilter.borrowerCount = null;
                        positionFilter.loanCount = null;
                    }
                };
                this.showWatchModal = function (watch, modalService, inDeleteMode, watchTypeId, codeToExcute) {
                    var modalInstance = modalService.open({
                        templateUrl: pageOptions.appBasePath + 'app/views/watch.html?v=' + pageOptions.appVersion,
                        controller: 'application.controllers.watchController',
                        controllerAs: 'watch',
                        size: 'md',
                        resolve: {
                            sourcedata: function () {
                                var modelSourceData = watch;
                                modelSourceData.inDeleteMode = inDeleteMode;
                                return modelSourceData;
                            }
                        }
                    });
                    modalInstance.result.then(function (updatedPositions) {
                        if (updatedPositions && codeToExcute) {
                            codeToExcute(updatedPositions);
                        }
                    }, function () { });
                };
                this.createWatch = function (position) {
                    var watch = {
                        watchId: position.watchId, watchTypeId: 1, watchObjectTypeId: position.watchObjectTypeId,
                        watchObjectId: position.watchObjectId, watchComments: position.watchComments,
                        watchHtmlText: null, isOnWatch: position.isOnWatch, securityId: position.securityId, issuerId: position.issuerId,
                        issuer: position.issuer, securityCode: position.securityCode
                    };
                    return watch;
                };
                this.createSellCandidate = function (position) {
                    var watch = {
                        watchId: position.sellCandidateId, watchTypeId: 2, watchObjectTypeId: position.sellCandidateObjectTypeId,
                        watchObjectId: position.sellCandidateObjectTypeId, watchComments: position.sellCandidateComments,
                        watchHtmlText: null, isOnWatch: null, securityId: position.securityId, issuerId: position.issuerId,
                        issuer: position.issuer, securityCode: position.securityCode
                    };
                    return watch;
                };
                this.showBuySellModal = function (fund, position, modalService, isBuy, codeToExcute) {
                    var modalInstance = modalService.open({
                        templateUrl: pageOptions.appBasePath + 'app/views/buyselltrade.html?v=' + pageOptions.appVersion,
                        controller: 'application.controllers.buySellTradeController',
                        controllerAs: 'buyselltrade',
                        size: 'x-lg',
                        resolve: {
                            sourcedata: function () {
                                var data = {};
                                data.position = JSON.parse(JSON.stringify(position));
                                data.isBuy = isBuy;
                                data.fund = fund;
                                return data;
                            }
                        }
                    });
                    modalInstance.result.then(function (updatedPositions) {
                        if (updatedPositions && codeToExcute) {
                            codeToExcute(updatedPositions);
                        }
                    }, function () { });
                };
                this.showUpdateSecurityPopup = function (position, updateType, modalService, codeToExcute) {
                    var modalInstance = modalService.open({
                        templateUrl: pageOptions.appBasePath + 'app/views/bbgPopup.html?v=' + pageOptions.appVersion,
                        controller: 'application.controllers.bbgPopupController',
                        controllerAs: 'bbgpopup',
                        size: 'md',
                        resolve: {
                            sourcedata: function () {
                                var data = {};
                                data.position = JSON.parse(JSON.stringify(position));
                                return data;
                            }
                        }
                    });
                    modalInstance.result.then(function (updatedPositions) {
                        if (updatedPositions && codeToExcute) {
                            codeToExcute(updatedPositions);
                        }
                    }, function () { });
                };
                this.showLoanComparisonModal = function (fund, selectedViewId, positions, customViews, modalService, codeToExcuteOnRemoveAll) {
                    var modalInstance = modalService.open({
                        templateUrl: pageOptions.appBasePath + 'app/views/loancomparison.html?v=' + pageOptions.appVersion,
                        controller: 'application.controllers.loanComparisonController',
                        controllerAs: 'loancomparison',
                        size: 'x-lg',
                        resolve: {
                            sourcedata: function () {
                                var data = {};
                                data.fund = JSON.parse(JSON.stringify(fund));
                                data.positions = JSON.parse(JSON.stringify(positions));
                                data.customViews = JSON.parse(JSON.stringify(customViews));
                                data.selectedViewId = selectedViewId;
                                return data;
                            }
                        }
                    });
                    modalInstance.result.then(function (removeall) {
                        if (removeall) {
                            codeToExcuteOnRemoveAll(positions);
                        }
                    }, function () { });
                };
                this.showAnalystResearchPopup = function (issuerId, modalService, codeToExcute) {
                    var modalInstance = modalService.open({
                        templateUrl: pageOptions.appBasePath + 'app/views/analystresearchpopup.html?v=' + pageOptions.appVersion,
                        controller: 'application.controllers.analystResearchPopupController',
                        controllerAs: 'researchpopup',
                        size: 'x-lg',
                        resolve: {
                            sourcedata: function () {
                                return issuerId;
                            }
                        }
                    });
                    modalInstance.result.then(function (updatedPositions) {
                        if (updatedPositions && codeToExcute) {
                            codeToExcute(updatedPositions);
                        }
                    }, function () { });
                };
                this.showViewEditorPopup = function (viewId, modalService, codeToExcute) {
                    var modalInstance = modalService.open({
                        templateUrl: pageOptions.appBasePath + 'app/views/vieweditorpopup.html?v=' + pageOptions.appVersion,
                        controller: 'application.controllers.viewEditorPopupController',
                        controllerAs: 'vieweditorpopup',
                        size: 'lg',
                        resolve: {
                            viewId: function () {
                                return viewId;
                            }
                        }
                    }).result.finally(codeToExcute);
                    //modalInstance.result.then(() => {
                    //	if (codeToExcute) {
                    //		codeToExcute();
                    //	}
                    //}, () => { });
                };
                this.showTradeModel = function (trade, modalService, codeToExcute) {
                    var modalInstance = modalService.open({
                        templateUrl: pageOptions.appBasePath + 'app/views/buyselltrade.html?v=' + pageOptions.appVersion,
                        controller: 'application.controllers.buySellTradeController',
                        controllerAs: 'buyselltrade',
                        size: 'x-lg',
                        resolve: {
                            sourcedata: function () {
                                var data = {};
                                data.trade = JSON.parse(JSON.stringify(trade));
                                return data;
                            }
                        }
                    });
                    modalInstance.result.then(function (updatedPositions) {
                        if (updatedPositions && codeToExcute) {
                            codeToExcute(updatedPositions);
                        }
                    }, function () { });
                };
                this.createCollectionFilters = function (positions, sourceObject, filterObjName, filterCollectionName, fields) {
                    if (positions.length) {
                        if (!sourceObject[filterObjName]) {
                            sourceObject[filterObjName] = {};
                        }
                        fields.forEach(function (field) {
                            sourceObject[filterCollectionName][field.jsonPropertyName + 's'] = _.chain(positions).map(field.jsonPropertyName).uniq().value();
                            var sortFn = function (s1, s2) {
                                var l = s1 ? s1.toString().toLowerCase() : '', m = s2 ? s2.toString().toLowerCase() : '';
                                return l === m ? 0 : l > m ? 1 : -1;
                            };
                            if (field.fieldType === 2) {
                                sortFn = function (s1, s2) {
                                    var s3 = s1 ? s1.toString().split(',').join('') : '';
                                    var s4 = s2 ? s2.toString().split(',').join('') : '';
                                    if (!(isNaN(s3) && isNaN(s4))) {
                                        return Number(s3) - Number(s4);
                                    }
                                    return 0;
                                };
                            }
                            else if (field.fieldType === 3) {
                                sortFn = function (a, b) {
                                    var c = new Date(a);
                                    var d = new Date(b);
                                    return c - d;
                                };
                            }
                            sourceObject[filterCollectionName][field.jsonPropertyName + 's'] = sourceObject[filterCollectionName][field.jsonPropertyName + 's'].sort(sortFn);
                            sourceObject[filterCollectionName][field.jsonPropertyName + 's'] = _.reject(sourceObject[filterCollectionName][field.jsonPropertyName + 's'], function (item) { return (item == null || item === "null"); });
                            var collection = sourceObject[filterCollectionName][field.jsonPropertyName + 's'];
                            sourceObject[filterCollectionName][field.jsonPropertyName + 's'] = [];
                            collection.forEach(function (s) {
                                sourceObject[filterCollectionName][field.jsonPropertyName + 's'].push({ value: s, label: s });
                            });
                        });
                    }
                };
                this.showMessage = function (message) {
                    var vm = _this;
                    var modalInstance = vm.modalService.open({
                        templateUrl: pageOptions.appBasePath + 'app/views/message.html?v=' + pageOptions.version,
                        controller: 'application.controllers.showMessageController',
                        controllerAs: 'message',
                        size: 'md',
                        resolve: {
                            sourcedata: function () {
                                var modelSourceData = message;
                                return modelSourceData;
                            }
                        }
                    });
                    modalInstance.result.then(function (result) {
                        if (result) {
                        }
                    }, function () { });
                };
                this.processSearchText = function (positions, fields) {
                    positions.forEach(function (p) {
                        p.searchText = p.issuerDesc ? p.issuerDesc : '';
                        p.searchText = p.securityDesc ? (p.searchText + ' ' + p.securityDesc) : p.searchText;
                        fields.forEach(function (f) {
                            if (f.fieldType == 2) {
                                p.searchText += ((p[f.jsonPropertyName] ? p[f.jsonPropertyName].toString().split(',').join('') : '') + '|');
                            }
                            else {
                                p.searchText += ((p[f.jsonPropertyName] ? p[f.jsonPropertyName] : '') + '|');
                            }
                        });
                        _this.processTooltip(p);
                    });
                };
                this.createColumnDefs = function (sourceObject, gridOptionsName, filtercollectionsName, highlightFilteredHeaderName, fields) {
                    var columnDefs = sourceObject[gridOptionsName]['columnDefs'];
                    for (var i = 0; i < fields.length; i++) {
                        var field = fields[i];
                        var checkIfColumnDefExist = columnDefs.filter(function (c) { return c.name === field.fieldName; });
                        try {
                            if (!checkIfColumnDefExist.length) {
                                var columnDef = {
                                    field: field.jsonPropertyName,
                                    width: field.displayWidth,
                                    name: field.fieldName,
                                    visible: !field.hidden,
                                    displayName: field.fieldTitle,
                                    pinnedLeft: field.pinnedLeft ? true : null,
                                    enablePinning: field.pinnedLeft,
                                    headerCellClass: sourceObject[highlightFilteredHeaderName],
                                    filterHeaderTemplate: '<div class="ui-grid-filter-container" ng-repeat="colFilter in col.filters"><div custom-modal-filter></div></div>',
                                    filter: {
                                        selectOptions: sourceObject[filtercollectionsName][field.jsonPropertyName + 's']
                                    }
                                };
                                if (field.cellTemplate) {
                                    columnDef['cellTemplate'] = field.cellTemplate;
                                }
                                if (field.cellTemplate) {
                                    columnDef['cellTemplate'] = field.cellTemplate;
                                }
                                if (field.cellClass) {
                                    columnDef['cellClass'] = field.cellClass;
                                }
                                if (field.headerCellClass) {
                                    columnDef['header' + 'CellClass'] = field.headerCellClass;
                                }
                                switch (field.fieldType) {
                                    case 1:
                                        columnDef['sortingAlgorithm'] = sourceObject['sortingAlgorithm'];
                                        break;
                                    case 2:
                                        columnDef['sortingAlgorithm'] = sourceObject['sortingNumberAlgorithm'];
                                        break;
                                    case 3:
                                        columnDef['sortingAlgorithm'] = sourceObject['sortingDateAlgorithm'];
                                        break;
                                    case 4:
                                        columnDef['sortingAlgorithm'] = sourceObject['sortingPercentageAlgorithm'];
                                        break;
                                    case 99:
                                        columnDef['sortingAlgorithm'] = sourceObject['booleanSortingAlgorithm'];
                                        break;
                                    default:
                                }
                                if (columnDefs.length <= i) {
                                    columnDefs.push(columnDef);
                                }
                                else {
                                    columnDefs[i] = columnDef;
                                }
                            }
                        }
                        catch (e) {
                            //alert(e);
                        }
                    }
                    for (var j = columnDefs.length - 1; j >= 0; j--) {
                        var checkIfFieldExist = fields.filter(function (f) { return f.fieldName === columnDefs[j].name; });
                        if (!checkIfFieldExist.length) {
                            columnDefs.splice(j, 1);
                        }
                    }
                };
                this.processTooltip = function (p) {
                    var vm = _this;
                    var watchHtmlText = p.isOnWatch ? ('ON WATCH LIST: ' + p.watchLastUpdatedOn + ' by ' + p.watchUser + (p.watchComments ? '<br/>COMMENT:&nbsp;&nbsp;' + p.watchComments : '')) : '';
                    var alertHtmlText = '';
                    if (p.alerts && p.alerts.length) {
                        p.alerts.forEach(function (a) {
                            alertHtmlText += a.description + '<br/>';
                        });
                    }
                    p.toolTipText = alertHtmlText ? 'ALERT:&nbsp;&nbsp;' + (watchHtmlText.length ? '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' : '') + alertHtmlText : '';
                    if (p.toolTipText && watchHtmlText.length) {
                        p.toolTipText += '<hr>';
                    }
                    p.toolTipText += watchHtmlText;
                    if (p.toolTipText && p.toolTipText.length && ((p.trades && p.trades.length) || p.tradeId)) {
                        p.toolTipText += '<hr>';
                    }
                    if (p.trades) {
                        for (var i = 0; i < p.trades.length; i++) {
                            var trade = p.trades[i];
                            var tradeInfo = trade.action + ": " + trade.quantity + " at " + trade.price + "<br/>";
                            tradeInfo += "AUDIT: " + trade.audit + "<br/>";
                            tradeInfo += (trade.comment ? "COMMENT: " + trade.comment + "<br/>" : '');
                            if (i < (p.trades.length - 1)) {
                                tradeInfo += '<hr>';
                            }
                            p.toolTipText += tradeInfo;
                        }
                    }
                    else if (p.tradeId) {
                        var t = p;
                        var tInfo = (t.isBuy ? "BUY " : "SELL ") + ": " + vm.filterService('currency')(t.tradeAmount, '', '') + " at " + vm.filterService('currency')(t.tradePrice, '', '') + "<br/>";
                        tInfo += "AUDIT: " + t.audit + "<br/>";
                        tInfo += (t.comments ? "COMMENT: " + t.comments + "<br/>" : '');
                        p.toolTipText += tInfo;
                    }
                };
                this.httpWrapperFactory = httpWrapperFactory;
                this.uiGridConstants = uiGridConstants;
                this.sce = $sce;
                this.filterService = $filter;
                this.modalService = modalService;
            }
            UIService.prototype.getDistinctStrings = function (field, data) {
                var codes = [];
                var vm = this;
                var flags = [];
                var l = data.length, i;
                for (i = 0; i < l; i++) {
                    var code = data[i][field];
                    if (!code || code == "")
                        continue;
                    var ucode = code.toUpperCase();
                    data[i][field] = code;
                    if (flags[ucode])
                        continue;
                    flags[ucode] = true;
                    codes.push(code);
                }
                codes = codes.sort(function (a, b) { return 0 - (a < b ? 1 : -1); });
                return codes;
            };
            return UIService;
        }());
        UIService.$inject = ["application.factories.httpWrapperFactory", "uiGridConstants", "$sce", "$uibModal", "$filter"];
        Services.UIService = UIService;
        angular.module("app").service("application.services.uiService", UIService);
    })(Services = Application.Services || (Application.Services = {}));
})(Application || (Application = {}));
//# sourceMappingURL=UIService.js.map