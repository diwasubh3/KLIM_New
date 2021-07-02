module Application.Controllers {
    export class AddUpdateSecurityOverrideController {
        modalInstance: angular.ui.bootstrap.IModalServiceInstance;
        securityOverride: Models.IPosition;
        selectedSecurityId:number;
        appBasePath: string = pageOptions.appBasePath;
        isLoading: boolean = false;
        statusText: string = "Loading";
        securities : Array<Models.IVwSecurityDto>;
        dataService: Application.Services.Contracts.IDataService;
        selectedSecurity: Models.IVwSecurityDto;
        ngTableParams: any;
        data:Array<Models.ISecurityOverride>;
        tableParams: any;
        scope:ng.IScope;
        fields: Array<Models.IField>;
        warning:string;
        dateFormat: string = 'MM/dd/yyyy';
        dateOptions:any = {
            formatYear: 'yy',
            startingDay: 1
        };
        confirmDelete:boolean=false;
        overrideFields: Array<Models.IField>;
        static $inject = ["application.services.dataService", "$window", "$scope", "$modalInstance", 'NgTableParams', 'sourcedata'];
        constructor(dataService: Application.Services.Contracts.IDataService, $window: ng.IWindowService, $scope: angular.IScope, $modalInstance: angular.ui.bootstrap.IModalServiceInstance,
            ngTableParams: NgTableParams,
            sourcedata: number) {
            var vm = this;
            
            vm.modalInstance = $modalInstance;
            vm.dataService = dataService;
            vm.scope = $scope;
            
            vm.ngTableParams = ngTableParams;
            vm.selectedSecurityId = sourcedata;
            vm.scope.$watch(() => {
                return vm.selectedSecurity;
            }, (newValue: Models.ISecurity, oldValue: Models.ISecurity) => {
                if (newValue != oldValue && newValue.securityId && !vm.isLoading && !vm.selectedSecurityId) {
                    vm.loadSecurityOverrides(true);
                }
            });
            vm.loadData();
        }

        openDate = ($event: any, so: Models.ISecurityOverride, field: Models.IField) => {
            $event.preventDefault();
            so['iseffectiveFromopened'] = false;
            so['iseffectiveToopened'] = false;
            so['is'+field.jsonPropertyName+'opened'] = true;
        };

        setSelectedField = (securityOverride: Models.ISecurityOverride) => {
            var vm = this;
            securityOverride.isDirty = true;
            securityOverride.field = vm.overrideFields.filter(f => f.fieldId === securityOverride.fieldId)[0];
            securityOverride.existingValue = vm.selectedSecurity['orig' + vm.capitalizeFirstLetter(securityOverride.field.jsonPropertyName)].toString();
        }

        loadSecurityOverrides = (addnewsecurity: boolean) => {
            var vm = this;
            vm.isLoading = true;
            vm.confirmDelete = false;
            if (vm.selectedSecurity && vm.selectedSecurity.securityId) {
                vm.dataService.getSecurityOverrides(null, vm.selectedSecurity.securityId).then(securityoverrides => {
                    vm.data = securityoverrides;
                    if (addnewsecurity) {
                        vm.addNewSecurityOverride();
                    }
                    vm.isLoading = false;
               });
            } else {
                vm.isLoading = false;
            }
        }

        addNewSecurityOverride = () => {
            var vm = this;
            vm.confirmDelete = false;
            if (vm.selectedSecurity && vm.selectedSecurity.securityId) {
                var newSecurityOverride: Models.ISecurityOverride = <Models.ISecurityOverride>{
                    securityId : vm.selectedSecurity.securityId
            };
                vm.data.splice(0, 0, newSecurityOverride);
            }
            vm.validateIsRequired();
        }

        capitalizeFirstLetter = (match) => {
            if (match) {
                return angular.uppercase(match.charAt(0)) + match.slice(1);
            } else {
                return match;
            }
        }

        loadData = () => {
            var vm = this;
            vm.isLoading = true;
            vm.confirmDelete = false;
            vm.dataService.getSaveSecurityOverrideHeaderFields().then(fields => {
                vm.fields = fields;
                vm.dataService.getSecurityOverrideableFields().then(overridefields => {
                    vm.overrideFields = overridefields;
                    vm.dataService.getCurrentSecurities().then(securities => {
                        vm.securities = securities;
                        if (vm.selectedSecurityId) {
                            vm.selectedSecurity = vm.securities.filter(s => s.securityId == vm.selectedSecurityId)[0];
                        }
                        vm.loadSecurityOverrides(false);
                    });
                });
            });
        }

        getStyle = (column: Models.IField) => {
            var style:any = {};
            if (column.displayWidth) {
                style.width = column.displayWidth  ;
            }
            if (column.fieldTitle === 'ATTRIBUTE') {
                style.width = null;
            }
            return style;
        }

        cancel = () => {
            var vm = this;
            vm.statusText = "Closing";
            vm.modalInstance.dismiss('cancel');
        }

        validateIsRequired = () => {
            var vm = this;
            for (var j = 0; j < vm.data.length; j++) {
                if (vm.data[j].field && vm.data[j].field.fieldTitle) {
                    vm.data[j]['fieldTitle'] = vm.data[j].field.fieldTitle;
                } else {
                    vm.data[j]['fieldTitle'] = '';
                }
            }
            var groupByField = _.chain(vm.data).groupBy('fieldTitle').map((v, i) => {
              return {
                  field: i,
                  items:v
              }  
            }).value();
            
            groupByField.forEach(g => {
                if (g.items.length) {
                    g.items[0]['iseffectiveTorequired'] = false;
                    g.items[0]['iseffectiveToinvalid'] = false;
                    for (var i = 1; i < g.items.length; i++) {
                        g.items[i]['iseffectiveTorequired'] = true;
                        if (g.items[i - 1]['effectiveFrom'] && g.items[i]['effectiveTo']) {
                            var fromDate = new Date(g.items[i - 1]['effectiveFrom'].toString());
                            var toDate = new Date(g.items[i]['effectiveTo'].toString());
                            if (toDate >= fromDate) {
                                g.items[i]['iseffectiveToinvalid'] = true;
                            } else {
                                g.items[i]['iseffectiveToinvalid'] = false;
                            }
                        }
                    }
                }
            });
        }

        save = (confirmedDelete:boolean) => {
            var vm = this;
            vm.confirmDelete = false;
            if (!confirmedDelete && vm.data.length && vm.data.filter(d => { return d.isDeleted; }).length === vm.data.length) {
                vm.warning = 'You have deleted all of the attribute overrides for this loan. Are you sure you want to proceed ?';
                vm.confirmDelete = true;
            } else {
                vm.isLoading = true;
                vm.statusText = "Updating";
                var dirtySecurityOverrides: Array<Models.ISecurityOverride> = JSON.parse(JSON.stringify(vm.data));
                if (dirtySecurityOverrides.length) {
                    dirtySecurityOverrides.forEach(d => d.field = null);

                    vm.dataService.saveSecurityOverides(dirtySecurityOverrides).then((result) => {
                        vm.isLoading = false;
                        vm.modalInstance.close(result);
                    });
                } else {
                    vm.modalInstance.dismiss('cancel');
                }
            }
        }
    }

    angular.module("app").controller("application.controllers.addUpdateSecurityOverrideController", AddUpdateSecurityOverrideController);
}