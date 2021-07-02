module Application.Controllers {
    export class LoanComparisonController {
        modalInstance: angular.ui.bootstrap.IModalServiceInstance;
        selectedIssuerId: number;
        appBasePath: string = pageOptions.appBasePath;
        isLoading: boolean = false;
        statusText: string = "Loading";
        sourcedata: any;
        securityIds: Array<number>;
        fund: Models.IFund;
		positions: Array<Models.IPosition>;
		customViews: Array<Models.ICustomView>;
		selectedCustomView: Models.ICustomView;

        dataService: Application.Services.Contracts.IDataService;
        fieldGroups: Array<Models.IFieldGroup>;
		selectedFieldGroup: Models.IFieldGroup;
		selectedViewId: number;
        ngTableParams: any;
        
        tableParams: any;
        scope: ng.IScope;
        
        
        warning: string;
        dateOptions: any = {
            formatYear: 'yy',
            startingDay: 1
        };
        
        windowService: ng.IWindowService;

        static $inject = ["application.services.dataService", "$window", "$scope", "$uibModalInstance", 'NgTableParams', 'sourcedata'];

        constructor(dataService: Application.Services.Contracts.IDataService, $window: ng.IWindowService, $scope: angular.IScope, $modalInstance: angular.ui.bootstrap.IModalServiceInstance,
            ngTableParams: NgTableParams, sourcedata: any) {
            var vm = this;
            
            vm.modalInstance = $modalInstance;
            vm.dataService = dataService;
            vm.scope = $scope;
            vm.windowService = $window;
            vm.sourcedata = sourcedata;
			vm.ngTableParams = ngTableParams;
	        vm.selectedViewId = sourcedata.selectedViewId;

            if (sourcedata.fund) {
                vm.fund = sourcedata.fund;
            }

			if (sourcedata.customViews) {
				vm.customViews = sourcedata.customViews;
				var actuals = vm.customViews.filter(x => !(<any>x.viewName).startsWith("---"));
				if (actuals.length) {
					var selected = actuals.filter(x => x.viewId == vm.selectedViewId);
					if (selected.length)
						vm.selectedCustomView = selected[0];
					else
						vm.selectedCustomView = actuals[0];
				}
			}

            if (sourcedata.positions) {
                vm.securityIds = sourcedata.positions.map(p => p.securityId);
                vm.loadData();
            }
        }

		sortFieldArraySortOrderAsc = (arrayToSort: Array<Models.IField>) => {
			return arrayToSort.sort(function (a, b) {
				return a.sortOrder - b.sortOrder;
			});
		}

		sortCustomViewFieldArraySortOrderAsc = (arrayToSort: Array<Models.ICustomViewField>) => {
			return arrayToSort.sort(function (a, b) {
				return a.sortOrder - b.sortOrder;
			});
		}

        loadData = () => {
            var vm = this;
            vm.isLoading = true;
            vm.dataService.loadFixedFields().then((fixedfields) => {
                vm.dataService.loadPositionViewFieldGroups().then(d => {
                    vm.fieldGroups = d;
                    var selectAll = <Models.IFieldGroup>{
                        fieldGroupId: -1,
                        fieldGroupName: 'Select All',
                        fields: []
                    };
					fixedfields = vm.sortFieldArraySortOrderAsc(fixedfields);
					d.forEach(fg => {
						var fields = vm.sortFieldArraySortOrderAsc(fg.fields);
                        if (fg.fieldGroupName == 'Security')
                        {
							fields = [].concat(fixedfields).concat(fields);
                        }

                        fields.forEach(f => {
                            selectAll.fields.push(f);
                        });
                    });

                    vm.fieldGroups.splice(0, 0, selectAll);
					vm.selectedFieldGroup = vm.fieldGroups[0];
					vm.customViews.forEach(v => {
						if (v.customViewFields) {
							var cvfs = v.customViewFields.filter(x => x.fieldId != 132 && x.fieldId != 133);
							cvfs.forEach(x => {
								var fields = selectAll.fields.filter(f => f.fieldId == x.fieldId);
								if (fields.length) {
									x.field = fields[0];
								}
							});
							v.customViewFields = vm.sortCustomViewFieldArraySortOrderAsc(cvfs);
						}
					});

                    vm.dataService.getPositionsForSecurities(vm.securityIds, vm.fund.fundCode).then((positions) => {
                        vm.positions = positions;
                        vm.isLoading = false;
                    });
                });    
            });
            
        }


        removePosition = (pos: Models.IPosition) => {
            var vm = this;
            var posIndex = vm.positions.indexOf(pos);
            vm.positions.splice(posIndex, 1);
        }


        cancel = () => {
            var vm = this;
            vm.statusText = "Closing";
            vm.modalInstance.dismiss('cancel');
        }

        
        save = () => {
            var vm = this;
            vm.modalInstance.close(true);
        }

       
    }

    angular.module("app").controller("application.controllers.loanComparisonController", LoanComparisonController);
}