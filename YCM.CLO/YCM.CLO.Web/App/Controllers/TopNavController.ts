
module Application.Controllers {
    export class TopNavController {
        rootScope: ng.IRootScopeService;
        summaryTableParams: any;
        ngTableParams: any;
        uiService: Application.Services.Contracts.IUIService;
        statusText: string;
        isLoading: boolean;
        summaryData: Array<Application.Models.ISummary>;
        dataService: Application.Services.Contracts.IDataService;
        window: ng.IWindowService;
        activeView: string;
        filter: ng.IFilterService;
        selectedFund: Models.ISummary;
        fundRestrictionTypes: Array<Models.IFundRestrictionsTypes>;
        fundRestrictions: Array<Models.IFundRestriction>;
        appBasePath: string = pageOptions.appBasePath;
        isAutoRefresh:boolean;
        hideNotActive: boolean = false;
        fundRestrictionFields: Array<Models.IField>;
		interval: ng.IIntervalService;
		testResultIsOpen: boolean;
        tooltipTextTemplate:string = '<div>__CONTENT__</div>';
		static $inject = ["application.services.uiService", "application.services.dataService", "$rootScope", 'NgTableParams', '$filter', '$window', '$interval'];
		constructor(uiService: Application.Services.Contracts.IUIService, dataService: Application.Services.Contracts.IDataService, $rootScope: ng.IRootScopeService, ngTableParams: NgTableParams, $filter: ng.IFilterService, $window: ng.IWindowService, $interval:ng.IIntervalService) {
            var vm = this;
            vm.uiService = uiService;
            vm.rootScope = $rootScope;
            vm.ngTableParams  = ngTableParams;
            vm.dataService    = dataService;
            vm.activeView     = 'home';
            vm.window         = $window;
            vm.filter = $filter;
            vm.interval = $interval;

            vm.rootScope.$on('onActivated', (event, data) => {
                vm.activeView = data;
                vm.rootScope['activeView'] = data;
            });

            vm.rootScope.$on('refreshSummaries', (event, data: Models.ISummary) => {
				vm.refreshSummariesAndFundRestrictions();
            });

            vm.rootScope.$on('refreshFundRestrictions', (event, data: Models.ISummary) => {
                vm.refreshFundRestrictions();
            });

            vm.loadData();

            vm.interval(vm.checkForAutoRefresh, 300000);
        }

        checkForAutoRefresh = () => {
            var vm = this;
            if (vm.isAutoRefresh) {
				vm.refreshSummariesAndFundRestrictions();
                vm.rootScope.$emit('onAutoRefresh', vm.selectedFund);
            }
        }

		refreshSummariesAndFundRestrictions = () => {
			var vm = this;
			vm.refreshSummaries();
			vm.refreshFundRestrictions();
		}

        refreshSummaries = () => {
            var vm = this;
            vm.dataService.loadSummaryData().then((summaries) => {
                var selectedFundCode = vm.selectedFund.fundCode;
                vm.summaryData = summaries;
                vm.setParamsTable();
                vm.window.setTimeout(() => {
                    if (summaries.length) {
                        vm.selectFund(summaries.filter(f => { return f.fundCode == selectedFundCode })[0]);
                        vm.setBackgroundStyleBasedOnFundRestrictions();
                    }
                }, 100);
            });
        }

        refreshFundRestrictions = () => {
            var vm = this;
            vm.dataService.loadFundRestrictions(null).then(fundrestrictions => {
                vm.fundRestrictions = fundrestrictions;
                vm.setBackgroundStyleBasedOnFundRestrictions();
            });
        }

        setBackgroundStyleBasedOnFundRestrictions = () => {
            var vm = this;
            vm.dataService.loadFundRestrictionFields().then(fields => {
                vm.summaryData.forEach(summary => {
                    vm.fundRestrictionFields.forEach((fundRestrictionField: Models.IField) => {
                        var rowStyles = vm.getBackgroundColorStyle(<string>fundRestrictionField.jsonPropertyName, summary);
                        var jsonPropName = fundRestrictionField.jsonPropertyName;
                        var bodJsonPropName = 'bod' + vm.uiService.capitalizeFirstLetter(fundRestrictionField.jsonPropertyName);
                        summary[jsonPropName + 'BgStyle'] = rowStyles.topRow;
                        summary[bodJsonPropName + 'BgStyle'] = rowStyles.bodRow;

                        summary[jsonPropName + 'Tooltip'] = rowStyles.toolTipText;
                        summary[bodJsonPropName + 'Tooltip'] = rowStyles.toolTipText;
                    });
                });
            });
        }

        hideUnhideNotActive = () => {
            var vm = this;
            vm.hideNotActive = !vm.hideNotActive;
            vm.summaryData.forEach(summary => {
                summary.inActive = vm.hideNotActive;
            });
            vm.rootScope.$emit('onFundVisibleRowsChanged', vm.summaryData.filter(fundRow => {
                return (vm.selectedFund == fundRow && fundRow.inActive);
            }).length);
        }

        loadData = () => {
            var vm = this;
            vm.statusText = "Loading";
            vm.isLoading = true;
            vm.window.setTimeout(() => {
                vm.dataService.loadFundRestrictionsTypes().then(fundrestrictionTypes => {
                    vm.fundRestrictionTypes = fundrestrictionTypes;
                    vm.dataService.loadFundRestrictionFields().then(fundRestrictionFields => {
                        vm.fundRestrictionFields = fundRestrictionFields;
                        vm.dataService.loadFundRestrictions(null).then(fundrestrictions => {
							vm.fundRestrictions = fundrestrictions;
							if(!pageOptions.TestResults)
								vm.dataService.loadSummaryData().then((summaries) => {
									pageOptions.TestResults = summaries;
									vm.setSummaryData(summaries);
								});
							else
								vm.setSummaryData(pageOptions.TestResults);
                        });
                    });
                });
            });
		}

		setSummaryData = (summaries: Models.ISummary[]) => {
			var vm = this;
			vm.summaryData = summaries;
			vm.setBackgroundStyleBasedOnFundRestrictions();

			vm.window.setTimeout(() => {
				vm.isLoading = false;
				vm.setParamsTable();
				if (summaries.length) {
					vm.selectFund(summaries[0]);
				}
			});
		}

        getBackgroundColorStyle = (fieldName: string, summaryRow: Models.ISummary) => {
            var vm = this;
            var color: string = '';
            var bodColor: string = '';
            var fieldFundRestrictions = vm.fundRestrictions.filter(fr => {
                return fr.jsonPropertyName == fieldName;
            });
            var rowStyle: any = { toolTipText:''};

			if (fieldFundRestrictions.length) {
				//TODO - unify logic, maybe move to c# controller
				//var assetParFundRestrictions = fieldFundRestrictions.filter(fieldFundRestriction => { return fieldFundRestriction.fieldName == 'AssetPar' && fieldFundRestriction.fundId == summaryRow.fundId });
				if (fieldName == 'assetPar') {
					//var assetParFundRestrictions = vm.getAssetParRestrictions('AssetPar', summaryRow.fundId);
					//if (assetParFundRestrictions.length) {
					//	color = assetParFundRestrictions[0].displayColor;
					//	if (!rowStyle.toolTipText)
					//		rowStyle.toolTipText = assetParFundRestrictions[0].fundRestrictionToolTip;
					//}
				}
				vm.fundRestrictionTypes.forEach(frt => {
					var fundRestrictions = fieldFundRestrictions.filter(fieldFundRestriction => { return fieldFundRestriction.fundRestrictionTypeId == frt.fundRestrictionTypeId && fieldFundRestriction.fundId == summaryRow.fundId });
					if (fundRestrictions.length) {
						if (eval(summaryRow[fieldName] + fundRestrictions[0].operatorVal + fundRestrictions[0].restrictionValue)) {
							color = frt.displayColor;
						}
						if (eval(summaryRow['bod' + vm.uiService.capitalizeFirstLetter(fieldName)] + fundRestrictions[0].operatorVal + fundRestrictions[0].restrictionValue)) {
							bodColor = frt.displayColor;
						}
						var tooltipText = vm.tooltipTextTemplate.replace('__CONTENT__', (frt.fundRestrictionTypeName.replace('IN ', '').replace(' ', '&nbsp;') + '&nbsp;' + fundRestrictions[0].operatorCode.toString() + '&nbsp;' + vm.filter('currency')(fundRestrictions[0].restrictionValue, '', 2)));
						rowStyle.toolTipText += tooltipText;
                    }
              });
            }

            

            if (color) {
                rowStyle.topRow = { 'background-color': color, 'color': '#333333' }
            } else {
                rowStyle.topRow = null;
            }

            if (bodColor) {
                rowStyle.bodRow = { 'background-color': bodColor, 'color': '#333333' }
            } else {
                rowStyle.bodRow = null;
            }

            return rowStyle;
        }

        selectFund = (fundRow: Models.ISummary) => {
            var vm = this;
            if (!vm.selectedFund || (vm.selectedFund && vm.selectedFund.fundId !== fundRow.fundId)) {
                vm.rootScope.$emit('onFundSelectionChanged', fundRow);        
            }
            if (vm.selectedFund != fundRow) {
                vm.selectedFund = fundRow;
                vm.rootScope['selectedFund'] = fundRow;
                vm.rootScope.$emit('onFundChanged', fundRow);
            }
        }

	    onTestResultsVisibilityChanged(open: boolean): void {
			var vm = this;
			vm.rootScope.$emit('onTestResultsVisibilityChanged', open);
		}

	    getAssetParRestrictions(fieldName: string, fundId: number): Models.IFundRestriction[] {
		    var vm = this;
		    var fieldFundRestrictions = vm.fundRestrictions.filter(fr => {
			    return fr.fieldName == fieldName && fr.fundId == fundId;
			});
		    return fieldFundRestrictions;
	    }

		downloadLoanPositions = (fundId: number) => {
			var vm = this;
			var fieldFundRestrictions = vm.getAssetParRestrictions('AssetPar', fundId);
			//var fieldFundRestrictions = vm.fundRestrictions.filter(fr => {
			//	return fr.fieldName == 'AssetPar' && fr.fundId == fundId;
			//});
			var rest = fieldFundRestrictions[0];
			if(rest.isDifferenceOverThreshold)
				vm.dataService.downloadLoanPositions(fundId);
        }

        downloadReInvestCash = (Url: string) => {
            var vm = this;
            vm.dataService.downloadReInvestCash(Url);
        }

        setParamsTable = () => {
            var vm = this;
            //if (!vm.summaryTableParams) {
                vm.summaryTableParams = new vm.ngTableParams({
                    page: 1,
                    noPager: true,
                    count: 10000
                    //sorting: {
                    //    'fundCode': 'asc'
                    //}
                }, {
                        total: 1,
                        counts: [],
                        dataset: vm.summaryData
                    });
            //} else {
            //    vm.summaryTableParams.reload();
            //}
        }
    }

    angular.module("app").controller("topNavController", TopNavController);
}