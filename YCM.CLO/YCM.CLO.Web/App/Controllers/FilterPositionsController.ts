module Application.Controllers {
    export class FilterPositionsController {
        modalInstance: angular.ui.bootstrap.IModalServiceInstance;
        selectedIssuerId: number;
        appBasePath: string = pageOptions.appBasePath;
        isLoading: boolean = false;
        statusText: string = "Loading";
        sourcedata: any;
        securityIds: Array<number>;
        fund:  Models.IFund;
        funds: Array<Models.IFund>;
        positions: Array<Models.IPosition>;
	    uiService: Application.Services.Contracts.IUIService;
        alwaysCalculateFields: string[] = [ 'Bid','Offer','Spread','MoodyRecovery','SeniorLeverage','WARF','WARF Recovery','Spread','WARF'];
        dataService: Application.Services.Contracts.IDataService;
        fieldGroups: Array<Models.IFieldGroup>;
        selectedFieldGroup: Models.IFieldGroup;
        ngTableParams: any;
        
        tableParams: any;
		scope: ng.IScope;
		average: number;

        positionfilter:Models.IPositionFilters;
        operators:Array<Models.IOperator>;
        filterCollections: any;
        uiFields: Models.IField[];

        warning: string;
        dateOptions: any = {
            formatYear: 'yy',
            startingDay: 1
        };
        
        windowService: ng.IWindowService;

		static $inject = ["application.services.uiService", "application.services.dataService", "$window", "$scope", "$uibModalInstance", 'NgTableParams', 'positionData'];

		constructor(uiService: Application.Services.Contracts.IUIService, dataService: Application.Services.Contracts.IDataService, $window: ng.IWindowService, $scope: angular.IScope, $modalInstance: angular.ui.bootstrap.IModalServiceInstance,
			ngTableParams: NgTableParams, positionData: any) {
            var vm = this;
            
            vm.modalInstance = $modalInstance;
            vm.dataService = dataService;
            vm.scope = $scope;
			vm.windowService = $window;
	        vm.positions = positionData.positions;
            vm.positionfilter = positionData.positionfilter;
            vm.filterCollections = positionData.filterCollections;
            vm.uiFields = positionData.uiFields;
            vm.ngTableParams = ngTableParams;
			vm.uiService = uiService;
            
            vm.loadData();
        }

        loadData = () => {
            var vm = this;
            vm.isLoading = true;
            if (!vm.positionfilter.operators) {
                vm.dataService.loadOperators().then(operators => {
                    vm.positionfilter.operators = operators;
                    vm.positionfilter.operators.splice(0, 0, <Models.IOperator>{ operatorCode:'', operatorId:null, operatorVal:null });
                    vm.operators = vm.positionfilter.operators;

                    if (!vm.positionfilter.ratingsDictionary) {
                        vm.dataService.getRatings().then(ratings => {
                            vm.positionfilter.ratingsDictionary = {};
                            for (var i = 0; i < ratings.length; i++) {
                                vm.positionfilter.ratingsDictionary[ratings[i].ratingDesc] = ratings[i].rank;
                            }

                            vm.loadAllFieldGroups();
                        });
                    }
                });
            } else {
                vm.operators = vm.positionfilter.operators;
                vm.loadAllFieldGroups();
            }

            

        }

        loadAllFieldGroups = () => {
            var vm = this;
            if (!vm.positionfilter.security) {
                vm.dataService.getFilterFieldGroups().then((fieldgroups) => {
	                vm.positionfilter.fixedfields = [];
                    vm.positionfilter.security = [];
                    vm.positionfilter.analyst = [];
                    vm.positionfilter.ratings = [];

					vm.loadFieldGroup(fieldgroups, 5, "fixedfields");
					vm.loadFieldGroup(fieldgroups, 1, "security");
					vm.loadFieldGroup(fieldgroups, 2, "ratings");
					vm.loadFieldGroup(fieldgroups, 3, "analyst");
                    vm.isLoading = false;
                });
            } else {
                vm.isLoading = false;
                vm.setFields("fixedfields");
                vm.setFields("security");
                vm.setFields("ratings");
                vm.setFields("analyst");
            }
        }

		loadFieldGroup = (fieldgroups: Array<Models.IFieldGroup>, fieldGroupId: number, filterField: string) => {
			var vm = this;

			var fields = fieldgroups.filter(fg => fg.fieldGroupId == fieldGroupId)[0].fields;
			if (fields && fields.length) {
				fields = vm.uiService.sortArrayBySortOrderAsc(fields, "filterOrder");
				fields.forEach(field => {
                    vm.positionfilter[filterField].push(<Models.IFilter>{
                        field: field,
                        lowerBound: { operator: vm.operators[0] },
                        upperBound: { operator: vm.operators[0] }
                    });
				});
            }
            vm.setFields(filterField);
        }

        setFields = (filterField: string) => {
            var vm = this;
            for (var i = 0; i < vm.positionfilter[filterField].length; i++) {
                vm.positionfilter[filterField][i].isDisabled = vm.uiFields.filter((f: Models.IField) => { return f.fieldName == vm.positionfilter[filterField][i].field.fieldName; }).length == 0;
                if (!vm.positionfilter[filterField][i].isDisabled && vm.positionfilter[filterField][i].field.fieldType == 1)
                {
                    if (filterField == 'ratings')
                    {
                        var collection = vm.filterCollections[vm.positionfilter[filterField][i].field.jsonPropertyName + 's'];
                        for (var j = 0; j < collection.length; j++) {
                            var val = vm.positionfilter.ratingsDictionary[collection[j].label];
                            if (val == undefined)
                            {
                                val = -2;
                            }
                            collection[j].value = val;
                        }
                        vm.positionfilter[filterField][i].sourceCollection = JSON.parse(JSON.stringify(collection));
                    }
                    else
                    {
                        vm.positionfilter[filterField][i].sourceCollection = vm.filterCollections[vm.positionfilter[filterField][i].field.jsonPropertyName + 's'];
                    }
                    
                }
            }
        }

        openDate = ($event: any, filter: Models.IFilterValue) => {
            $event.preventDefault();
            if (!filter.date) {
                filter.date = {opened:false};
            }
            filter.date.opened = true;
        }

		calculateAverages = () => {
			var vm = this;
			vm.uiService.updateFilterStatistics(vm.positions, vm.positionfilter, "issuer");
			var filtered = vm.positions.filter(x => x.isFilterSuccess);
			vm.updateAverages("fixedfields", filtered, vm.positionfilter);
			vm.updateAverages("ratings", filtered, vm.positionfilter);
			vm.updateAverages("security", filtered, vm.positionfilter);
			vm.updateAverages("analyst", filtered, vm.positionfilter);
		}

        updateAverages = (field: string, positions: Array<Models.IPosition>, positionFilter: Models.IPositionFilters) => {
            var vm = this;
			positionFilter[field].forEach(y => {
				var sum = 0;
                if (y.field.fieldType != 3 && y.field.fieldType != 1 && ((y.lowerBound && y.lowerBound.value != null && y.lowerBound.value.toString().length > 0) || (y.upperBound && y.upperBound.value != null && y.upperBound.value.toString().length > 0) || vm.alwaysCalculateFields.indexOf(y.field.fieldName) >= 0)) {
                    positions.forEach(x => {
                        sum += parseFloat(x[y.field.jsonPropertyName] != null && x[y.field.jsonPropertyName].toString().length > 0 ? x[y.field.jsonPropertyName].toString().replace(/,/g, ''):'0');
					});
					y.average = sum / positionFilter.loanCount;
				}
			});
		}

        cancel = () => {
            var vm = this;
            vm.statusText = "Closing";
            vm.reset();
            vm.modalInstance.close(false);
        }
        
        filter = () => {
            var vm = this;
            vm.modalInstance.close(true);
        }

		resetFieldGroup = (positionFilter: Models.IPositionFilters, filterField: string) => {
			var vm = this;
			positionFilter[filterField].forEach(filt => {
				filt.lowerBound.value = null;
				filt.upperBound.value = null;
				filt.lowerBound.operator = vm.operators[0];
				filt.upperBound.operator = vm.operators[0];
				filt.average = null;
			});
		}

        reset  = () => {
            var vm = this;
			vm.positionfilter.funds.forEach(f => f.isNotSelected = false);
			vm.resetFieldGroup(vm.positionfilter, "fixedfields");
			vm.resetFieldGroup(vm.positionfilter, "security");
			vm.resetFieldGroup(vm.positionfilter, "ratings");
			vm.resetFieldGroup(vm.positionfilter, "analyst");
        }
    }

    angular.module("app").controller("application.controllers.filterPositionsController", FilterPositionsController);
}