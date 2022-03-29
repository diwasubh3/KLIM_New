declare var yorkCore: any;
module Application.Services {
    export class UIService implements Contracts.IUIService {
        private httpWrapperFactory: Application.Factories.HttpWrapperFactory;
        uiGridConstants: any;
        currentHeight = screen.availHeight - (282 + (24 + (22 * 4))) + (yorkCore.isMsIe ? 35 : 0);
        sce: any;
        modalService: angular.ui.bootstrap.IModalService;
        filterService: angular.IFilterService;
		static $inject = ["application.factories.httpWrapperFactory", "uiGridConstants", "$sce", "$uibModal","$filter"];
        constructor(httpWrapperFactory: Application.Factories.HttpWrapperFactory, uiGridConstants: any,
            $sce: any, modalService: angular.ui.bootstrap.IModalService, $filter: angular.IFilterService) {
            this.httpWrapperFactory = httpWrapperFactory;
            this.uiGridConstants = uiGridConstants;
            this.sce = $sce;
            this.filterService = $filter;
            this.modalService = modalService;
        }

        getHeaderTemplate = () => {
            return '<div class="ngHeaderSortColumn {{col.headerClass}}" ng-style="{cursor: col.cursor}" ng-class="{ ngSorted: !noSortVisible }">' +
                '<div ng-click="col.sort($event)" ng-class="\'colt\' + col.index" class="ngHeaderText">{{col.displayName}}</div>' +
                '<div class="ngSortButtonDown" ng-show="col.showSortButtonDown()"></div>' +
                '<div class="ngSortButtonUp" ng-show="col.showSortButtonUp()"></div>' +
                '<div class="ngSortPriority">{{col.sortPriority}}</div>' +
                '</div>' +
                '<div ng-show="col.resizable" class="ngHeaderGrip" ng-click="col.gripClick($event)" ng-mousedown="col.gripOnMouseDown($event)"></div>';
        }

        capitalizeFirstLetter = (match: any) => {
            var result: string = match;
            if (match) {
                result = (angular.uppercase(match.charAt(0)) + match.slice(1));
            } 

            return result;
        }

		filterPositions = (positions: Array<Models.IPosition>, positionFilter: Models.IPositionFilters) => {
            var vm = this;

			positions.forEach(pos => {

				pos.isFilterSuccess = true;
                if (positionFilter.fixedfields) {
                    positionFilter.fixedfields.forEach((posfilter: Models.IFilter) => {
                        if (posfilter.lowerBound.operator.operatorVal && typeof (posfilter.lowerBound.value) != 'undefined' && (posfilter.lowerBound.value || posfilter.lowerBound.value.toString().length)) {
                            vm.filterPosition(pos, <Models.IFilterValue>posfilter.lowerBound, <Models.IField>posfilter.field, positionFilter);
                        }
                        if (posfilter.upperBound.operator.operatorVal && typeof (posfilter.upperBound.value) != 'undefined' && (posfilter.upperBound.value || posfilter.upperBound.value.toString().length)) {
                            vm.filterPosition(pos, <Models.IFilterValue>posfilter.upperBound, <Models.IField>posfilter.field, positionFilter);
                        }
                    });
                }

                if (positionFilter.security) {
                    positionFilter.security.forEach((posfilter: Models.IFilter) => {
                        if (posfilter.lowerBound.operator.operatorVal && typeof (posfilter.lowerBound.value) != 'undefined' && (posfilter.lowerBound.value || posfilter.lowerBound.value.toString().length)) {
                            vm.filterPosition(pos, <Models.IFilterValue>posfilter.lowerBound, <Models.IField>posfilter.field, positionFilter);
                        }
                        if (posfilter.upperBound.operator.operatorVal && typeof (posfilter.upperBound.value) != 'undefined' && (posfilter.upperBound.value || posfilter.upperBound.value.toString().length)) {
                            vm.filterPosition(pos, <Models.IFilterValue>posfilter.upperBound, <Models.IField>posfilter.field, positionFilter);
                        }
                    });
                }

                if (positionFilter.ratings) {
                    positionFilter.ratings.forEach((posfilter: Models.IFilter) => {
                        if (posfilter.lowerBound.operator.operatorVal && typeof (posfilter.lowerBound.value) != 'undefined' && (posfilter.lowerBound.value || posfilter.lowerBound.value.toString().length)) {
                            vm.filterPosition(pos, <Models.IFilterValue>posfilter.lowerBound, <Models.IField>posfilter.field, positionFilter);
                        }
                        if (posfilter.upperBound.operator.operatorVal && typeof (posfilter.upperBound.value) != 'undefined' && (posfilter.upperBound.value || posfilter.upperBound.value.toString().length)) {
                            vm.filterPosition(pos, <Models.IFilterValue>posfilter.upperBound, <Models.IField>posfilter.field, positionFilter);
                        }
                    });

                }

                if (positionFilter.analyst) {
                    positionFilter.analyst.forEach((posfilter: Models.IFilter) => {
                        if (posfilter.lowerBound.operator.operatorVal && typeof (posfilter.lowerBound.value) != 'undefined' && (posfilter.lowerBound.value || posfilter.lowerBound.value.toString().length)) {
                            vm.filterPosition(pos, <Models.IFilterValue>posfilter.lowerBound, <Models.IField>posfilter.field, positionFilter);
                        }
                        if (posfilter.upperBound.operator.operatorVal && typeof (posfilter.upperBound.value) != 'undefined' && (posfilter.upperBound.value || posfilter.upperBound.value.toString().length)) {
                            vm.filterPosition(pos, <Models.IFilterValue>posfilter.upperBound, <Models.IField>posfilter.field, positionFilter);
                        }
                    });
                }
			});

		}

		sortArrayBySortOrderAsc = (arrayToSort: any, sortField: string) => {
			return arrayToSort.sort(function (a, b) {
				return a[sortField] - b[sortField];
			});
		}

        filterPosition = (pos: Models.IPosition, posfilter: Models.IFilterValue, field: Models.IField, positionFilter: Models.IPositionFilters) => {
			if (pos.isFilterSuccess && posfilter.operator && posfilter.operator.operatorVal && (posfilter.value || posfilter.value.toString().length)) {
				pos.isFilterSuccess = pos.isFilterSuccess && (pos[field.jsonPropertyName] != null && pos[field.jsonPropertyName].toString().length > 0);

				if (pos.isFilterSuccess && field.fieldType == 3) {
					var filterdate = new Date(pos[field.jsonPropertyName]);
					var dateexpression: string = 'new Date( "' + pos[field.jsonPropertyName].toString() + '") ' + ' ' +
						posfilter.operator.operatorVal + ' new Date("' + posfilter.value + '")';
					pos.isFilterSuccess = pos.isFilterSuccess && eval(dateexpression);
                }
                else if (pos.isFilterSuccess && field.fieldType == 4) {
                    var value = pos[field.jsonPropertyName].toString().replace(/ %/g, '');
                    var expression: string = parseFloat(value).toString() + ' ' +
                        posfilter.operator.operatorVal + ' ' + posfilter.value;
                    pos.isFilterSuccess = pos.isFilterSuccess && eval(expression);
                }
                else if (pos.isFilterSuccess && field.fieldType == 2) {
					var value = pos[field.jsonPropertyName].toString().replace(/,/g, '');
					var expression: string = parseFloat(value).toString() + ' ' +
						posfilter.operator.operatorVal + ' ' + posfilter.value;
					pos.isFilterSuccess = pos.isFilterSuccess && eval(expression);
                }
                else if (pos.isFilterSuccess && field.fieldType == 1 && field.fieldGroupId == 2) {
                    var ratingFiledValue =  positionFilter.ratingsDictionary[pos[field.jsonPropertyName].toString().replace(/,/g, '')];
                    var ratingFilterValue = posfilter.value.toString();
                    if (ratingFilterValue == undefined)
                    {
                        ratingFilterValue = -2;
                    }

                    if (ratingFiledValue == undefined)
                    {
                        ratingFiledValue = -2;
                    }

                    var expression: string = parseFloat(ratingFiledValue).toString() + ' ' +
                        posfilter.operator.operatorVal + ' ' + ratingFilterValue;

                    pos.isFilterSuccess = pos.isFilterSuccess && eval(expression);
                }
			}
		}

		getDistinctStrings(field: string, data: Array<Models.IPosition>): Array<string> {
			let codes: string[] = [];
			var vm = this;
			let flags: boolean[] = [];
			var l = data.length, i;
			for (i = 0; i < l; i++) {
				var code = data[i][field];
				if (!code || code == "")
					continue;
				var ucode = code.toUpperCase();
				data[i][field] = code;
				if (flags[ucode]) continue;
				flags[ucode] = true;
				codes.push(code);
			}
			codes = codes.sort((a, b) => 0 - (a < b ? 1 : -1));
			return codes;
		}

		updateFilterStatistics = (positions: Array<Models.IPosition>, positionFilter: Models.IPositionFilters, field: string) => {
			var vm = this;
			vm.filterPositions(positions, positionFilter);
			var filtered = positions.filter(x => x.isFilterSuccess);
			if (filtered && filtered.length) {
				var distincts = vm.getDistinctStrings(field, filtered);
				positionFilter.borrowerCount = distincts.length;
				positionFilter.loanCount = filtered.length;
			} else {
				positionFilter.borrowerCount = null;
				positionFilter.loanCount = null;
			}
		}

		showWatchModal = (watch: Models.IWatch, modalService: angular.ui.bootstrap.IModalService, inDeleteMode: boolean, watchTypeId: number, codeToExcute: any) => {
            var modalInstance = modalService.open({
                templateUrl: pageOptions.appBasePath + 'app/views/watch.html?v=' + pageOptions.appVersion,
                controller: 'application.controllers.watchController',
                controllerAs: 'watch',
                size: 'md',
                resolve: {
                    sourcedata: () => {
						var modelSourceData = watch;
						modelSourceData.inDeleteMode = inDeleteMode;
                        return modelSourceData;
                    }
                }
            });

            modalInstance.result.then((updatedPositions: Array<Models.IPosition>) => {
                if (updatedPositions && codeToExcute) {
                    codeToExcute(updatedPositions);
                }
            }, () => { });
        }

		createWatch = (position: Models.IPosition) => {
			var watch = <Models.IWatch>{
				watchId: position.watchId, watchTypeId: 1, watchObjectTypeId: position.watchObjectTypeId
				, watchObjectId: position.watchObjectId, watchComments: position.watchComments
				, watchHtmlText: null, isOnWatch: position.isOnWatch, securityId: position.securityId, issuerId: position.issuerId
				, issuer: position.issuer, securityCode: position.securityCode
			};
			return watch;
        }

        
		createSellCandidate = (position: Models.IPosition) => {
			var watch = <Models.IWatch>{
				watchId: position.sellCandidateId, watchTypeId: 2, watchObjectTypeId: position.sellCandidateObjectTypeId
				, watchObjectId: position.sellCandidateObjectTypeId, watchComments: position.sellCandidateComments
				, watchHtmlText: null, isOnWatch: null, securityId: position.securityId, issuerId: position.issuerId
				, issuer: position.issuer, securityCode: position.securityCode
			};
			return watch;
		}

        showBuySellModal = (fund: Models.IFund, position: any, modalService: angular.ui.bootstrap.IModalService, isBuy: boolean, codeToExcute: any) => {
            
            var modalInstance = modalService.open({
                templateUrl: pageOptions.appBasePath + 'app/views/buyselltrade.html?v=' + pageOptions.appVersion,
                controller: 'application.controllers.buySellTradeController',
                controllerAs: 'buyselltrade',
                size: 'x-lg',
                resolve: {
                    sourcedata: () => {
                        var data: any = {};
                        data.position = JSON.parse(JSON.stringify(position));
                        data.isBuy = isBuy;
                        data.fund = fund;
                        return data;
                    }
                }
            });

            modalInstance.result.then((updatedPositions: Array<Models.IPosition>) => {
                if (updatedPositions && codeToExcute) {
                    codeToExcute(updatedPositions);
                }
            }, () => { });
        }

		showUpdateSecurityPopup = (position: any, updateType: string, modalService: angular.ui.bootstrap.IModalService, codeToExcute: any) => {

			var modalInstance = modalService.open({
				templateUrl: pageOptions.appBasePath + 'app/views/bbgPopup.html?v=' + pageOptions.appVersion,
				controller: 'application.controllers.bbgPopupController',
				controllerAs: 'bbgpopup',
				size: 'md',
				resolve: {
					sourcedata: () => {
						var data: any = {};
						data.position = JSON.parse(JSON.stringify(position));
						return data;
					}
				}
			});

			modalInstance.result.then((updatedPositions: Array<Models.IPosition>) => {
				if (updatedPositions && codeToExcute) {
					codeToExcute(updatedPositions);
				}
			}, () => { });
		}

		showLoanComparisonModal = (fund: Models.IFund, selectedViewId: number, positions: any, customViews: Array<Models.ICustomView>
			, modalService: angular.ui.bootstrap.IModalService, codeToExcuteOnRemoveAll: any) => {
            var modalInstance = modalService.open({
                templateUrl: pageOptions.appBasePath + 'app/views/loancomparison.html?v=' + pageOptions.appVersion,
                controller: 'application.controllers.loanComparisonController',
                controllerAs: 'loancomparison',
                size: 'x-lg',
                resolve: {
                    sourcedata: () => {
                        var data: any = {};
                        data.fund = JSON.parse(JSON.stringify(fund));
						data.positions = JSON.parse(JSON.stringify(positions));
						data.customViews = JSON.parse(JSON.stringify(customViews));
	                    data.selectedViewId = selectedViewId;
                        return data;
                    }
                }
            });

            modalInstance.result.then((removeall: boolean) => {
                if (removeall) {
                    codeToExcuteOnRemoveAll(positions);
                }
            }, () => { });
        }

	    showAnalystResearchPopup = (issuerId: number, modalService: angular.ui.bootstrap.IModalService, codeToExcute: any) => {
		    var modalInstance = modalService.open({
			    templateUrl: pageOptions.appBasePath + 'app/views/analystresearchpopup.html?v=' + pageOptions.appVersion,
			    controller: 'application.controllers.analystResearchPopupController',
			    controllerAs: 'researchpopup',
			    size: 'x-lg',
			    resolve: {
				    sourcedata: () => {
					    return issuerId;
				    }
			    }
		    });

		    modalInstance.result.then((updatedPositions: Array<Models.IPosition>) => {
			    if (updatedPositions && codeToExcute) {
				    codeToExcute(updatedPositions);
			    }
		    }, () => { });
        }

        showTradeHistoryPopup = (securitycode: string, issuer: string, modalService: angular.ui.bootstrap.IModalService, codeToExcute: any) => {
            var modalInstance = modalService.open({
                templateUrl: pageOptions.appBasePath + 'app/views/tradeHistorypopup.html?v=' + pageOptions.appVersion,
                controller: 'application.controllers.tradeHistoryPopupController',
                controllerAs: 'researchpopup',
                size: 'x-lg',
                resolve: {
                    sourcedata: () => {
                        var data: any = {};
                        data.securitycode = securitycode;
                        data.issuer = issuer;
                        return data;
                    }
                }
            });

            modalInstance.result.then((updatedPositions: Array<Models.IPosition>) => {
                if (updatedPositions && codeToExcute) {
                    codeToExcute(updatedPositions);
                }
            }, () => { });
            //var modalInstance = modalService.open({
            //    templateUrl: pageOptions.appBasePath + 'app/views/tradeHistorypopup.html?v=' + pageOptions.appVersion,
            //    controller: 'application.controllers.tradeHistoryPopupController',
            //    controllerAs: 'tradeHistoryPopup',
            //    size: 'x-lg',
            //    resolve: {
            //        sourcedata: () => {
            //            var data: any = {};
            //            data.securitycode = securitycode;
            //            data.portfolioName = portfolioName;
            //            return data;
            //        }
            //    }
            //}).result.finally(codeToExcute);

   //        modalInstance.result.then(() => {
			//	if (codeToExcute) {
			//		codeToExcute();
			//	}
			//}, () => { });
        }

		showViewEditorPopup = (viewId: number
			, modalService: angular.ui.bootstrap.IModalService, codeToExcute: any) => {
			var modalInstance = modalService.open({
				templateUrl: pageOptions.appBasePath + 'app/views/vieweditorpopup.html?v=' + pageOptions.appVersion,
				controller: 'application.controllers.viewEditorPopupController',
				controllerAs: 'vieweditorpopup',
				size: 'lg',
				resolve: {
					viewId: () => {
						return viewId;
					}
				}
			}).result.finally(codeToExcute);

			//modalInstance.result.then(() => {
			//	if (codeToExcute) {
			//		codeToExcute();
			//	}
			//}, () => { });
		}

        showTradeModel = (trade: Models.ITrade, modalService: angular.ui.bootstrap.IModalService, codeToExcute: any) => {
            var modalInstance = modalService.open({
                templateUrl: pageOptions.appBasePath + 'app/views/buyselltrade.html?v=' + pageOptions.appVersion,
                controller: 'application.controllers.buySellTradeController',
                controllerAs: 'buyselltrade',
                size: 'x-lg',
                resolve: {
                    sourcedata: () => {
                        var data: any = {};
                        data.trade = JSON.parse(JSON.stringify(trade));
                        return data;
                    }
                }
            });

            modalInstance.result.then((updatedPositions: Array<Models.IPosition>) => {
                if (updatedPositions && codeToExcute) {
                    codeToExcute(updatedPositions);
                }
            }, () => { });
        }



        createCollectionFilters = (positions: Array<Models.IPosition>, sourceObject: any, filterObjName: string, filterCollectionName, fields: Array<Models.IField>) => {
            if (positions.length) {
                if (!sourceObject[filterObjName]) {
                    sourceObject[filterObjName] = {};
                }

                fields.forEach(field => {
                    sourceObject[filterCollectionName][field.jsonPropertyName + 's'] = _.chain(positions).map(field.jsonPropertyName).uniq().value();

                    var sortFn: any = (s1, s2) => {
                        var l = s1 ? s1.toString().toLowerCase() : '', m = s2 ? s2.toString().toLowerCase() : '';
                        return l === m ? 0 : l > m ? 1 : -1;
                    };

                    if (field.fieldType === 2) {
                        sortFn = (s1, s2) => {
                            var s3 = s1 ? s1.toString().split(',').join('') : '';
                            var s4 = s2 ? s2.toString().split(',').join('') : '';
                            if (!(isNaN(s3) && isNaN(s4))) {
                                return Number(s3) - Number(s4);
                            }
                            return 0;
                        };
                    } else if (field.fieldType === 3) {
                        sortFn = (a, b) => {
                            var c: any = new Date(a);
                            var d: any = new Date(b);
                            return c - d;
                        };
                    }

                    sourceObject[filterCollectionName][field.jsonPropertyName + 's'] = sourceObject[filterCollectionName][field.jsonPropertyName + 's'].sort(sortFn);
                    sourceObject[filterCollectionName][field.jsonPropertyName + 's'] = _.reject(sourceObject[filterCollectionName][field.jsonPropertyName + 's'], item => { return (item == null || item === "null"); });
                    var collection = sourceObject[filterCollectionName][field.jsonPropertyName + 's'];
                    sourceObject[filterCollectionName][field.jsonPropertyName + 's'] = [];
                    collection.forEach(s => {
                        sourceObject[filterCollectionName][field.jsonPropertyName + 's'].push({ value: s, label: s });
                    });
                });
            }
        }

        showMessage = (message: Models.IMessage) => {
            var vm = this;
            var modalInstance = vm.modalService.open({
                templateUrl: pageOptions.appBasePath + 'app/views/message.html?v=' + pageOptions.version,
                controller: 'application.controllers.showMessageController',
                controllerAs: 'message',
                size: 'md',
                resolve: {
                    sourcedata: () => {
                        var modelSourceData = message;
                        return modelSourceData;
                    }
                }
            });

            modalInstance.result.then((result: Array<Models.IParameterValue>) => {
                if (result) {

                }
            }, () => { });
        };


        processSearchText = (positions: Array<Models.IPosition>, fields: Array<Models.IField>) => {
            positions.forEach(p => {
                p.searchText = p.issuerDesc ? p.issuerDesc:'';
                p.searchText = p.securityDesc ? (p.searchText + ' ' + p.securityDesc) : p.searchText;
                fields.forEach(f => {
                    if (f.fieldType == 2) {
                        p.searchText += ((p[f.jsonPropertyName] ? p[f.jsonPropertyName].toString().split(',').join('') : '') + '|');
                    } else {
                        p.searchText += ((p[f.jsonPropertyName] ? p[f.jsonPropertyName] : '') + '|');
                    }

                });

                this.processTooltip(p);
            });
        }


        createColumnDefs = (sourceObject: any, gridOptionsName: string, filtercollectionsName: string, highlightFilteredHeaderName: string, fields: Array<Models.IField>) => {
            var columnDefs = sourceObject[gridOptionsName]['columnDefs'];
            for (var i = 0; i < fields.length; i++) {
                var field = fields[i];
				var checkIfColumnDefExist = columnDefs.filter(c => c.name === field.fieldName);
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
						}

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
						} else {
							columnDefs[i] = columnDef;
						}
					}
	            } catch (e) {
		            //alert(e);
	            } 
            }

            for (var j = columnDefs.length - 1; j >= 0; j--) {
                var checkIfFieldExist = fields.filter(f => f.fieldName === columnDefs[j].name);
                if (!checkIfFieldExist.length) {
                    columnDefs.splice(j, 1);
                }
            }
        }

        processTooltip = (p: any) => {
            
            var vm = this;
            var watchHtmlText = p.isOnWatch ? ('ON WATCH LIST: ' + p.watchLastUpdatedOn + ' by ' + p.watchUser + (p.watchComments ? '<br/>COMMENT:&nbsp;&nbsp;' + p.watchComments : '')) : '';

            var alertHtmlText = '';
            if (p.alerts && p.alerts.length) {
                p.alerts.forEach(a => {
                    alertHtmlText += a.description + '<br/>';
                });
            }

            p.toolTipText = alertHtmlText ? 'ALERT:&nbsp;&nbsp;' + (watchHtmlText.length ? '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' : '') + alertHtmlText : '';
            if (p.toolTipText && watchHtmlText.length) {
                p.toolTipText += '<hr>';
            }
            p.toolTipText += watchHtmlText;

            if (p.toolTipText && p.toolTipText.length && ((p.trades && p.trades.length) || p.tradeId )) {
                p.toolTipText += '<hr>';
            }

            if (p.trades) {
                for (var i = 0; i < p.trades.length; i++) {
                    var trade: Models.ITradeInfo = p.trades[i];
                    var tradeInfo = trade.action + ": " + trade.quantity + " at " + trade.price + "<br/>";
                    tradeInfo += "AUDIT: " + trade.audit + "<br/>";
                    tradeInfo += (trade.comment ? "COMMENT: " + trade.comment + "<br/>" : '');
                    if (i < (p.trades.length - 1)) {
                        tradeInfo += '<hr>';
                    }
                    p.toolTipText += tradeInfo;
                }
            } else if (p.tradeId) {
                var t: Models.ITrade = p;
                var tInfo = (t.isBuy ? "BUY " : "SELL ") + ": " + vm.filterService('currency')(t.tradeAmount, '', '') + " at " + vm.filterService('currency')(t.tradePrice,'','') + "<br/>";
                tInfo += "AUDIT: " + t.audit + "<br/>";
                tInfo += (t.comments ? "COMMENT: " + t.comments + "<br/>" : '');
                p.toolTipText += tInfo;
            }
        }

        showPaydownModal = (paydown: Models.IPaydown, modalService: angular.ui.bootstrap.IModalService, inDeleteMode: boolean, paydownTypeId: number, codeToExcute: any) => {
            var modalInstance = modalService.open({
                templateUrl: pageOptions.appBasePath + 'app/views/paydown.html?v=' + pageOptions.appVersion,
                controller: 'application.controllers.paydownController',
                controllerAs: 'paydown',
                size: 'md',
                resolve: {
                    sourcedata: () => {
                        var modelSourceData = paydown;
                        modelSourceData.inDeleteMode = inDeleteMode;
                        return modelSourceData;
                    }
                }
            });

            modalInstance.result.then((updatedPositions: Array<Models.IPosition>) => {
                if (updatedPositions && codeToExcute) {
                    codeToExcute(updatedPositions);
                }
            }, () => { });
        }

        createPaydown = (position: Models.IPosition) => {
            var paydown = <Models.IPaydown>{
                paydownId: position.paydownId, paydownTypeId: 1, paydownObjectTypeId: position.paydownObjectTypeId
                , paydownObjectId: position.paydownObjectId, paydownComments: position.paydownComments
                , paydownHtmlText: null, isOnPaydown: position.isOnPaydown, securityId: position.securityId, issuerId: position.issuerId
                , issuer: position.issuer, securityCode: position.securityCode
            };
            return paydown;
        }

    }

    angular.module("app").service("application.services.uiService", UIService);
}
