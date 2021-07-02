module Application.Controllers {
    export class AddUpdateAnalystResearchController {
        modalInstance: angular.ui.bootstrap.IModalServiceInstance;
        selectedIssuerId:number;
        appBasePath: string = pageOptions.appBasePath;
        isLoading: boolean = false;
        statusText: string = "Loading";
        selectedIssuer:Models.IIssuer;
        issuers : Array<Models.IIssuer>;
        dataService: Application.Services.Contracts.IDataService;
        
        ngTableParams: any;
        data:Array<Models.IAnalystResearch>;
        tableParams: any;
        scope: ng.IScope;
        businessDescription:string;
        fields: Array<Models.IField>;
        warning: string;
        analysts:Models.IAnalysts;
        dateFormat: string = 'MM/dd/yyyy';
        dateOptions:any = {
            formatYear: 'yy',
            startingDay: 1
        };

        
        static $inject = ["application.services.dataService", "$window", "$scope", "$uibModalInstance", 'NgTableParams', 'sourcedata'];
        constructor(dataService: Application.Services.Contracts.IDataService, $window: ng.IWindowService, $scope: angular.IScope, $modalInstance: angular.ui.bootstrap.IModalServiceInstance,
            ngTableParams: NgTableParams,
            sourcedata: number) {
            var vm = this;
            
            vm.modalInstance = $modalInstance;
            vm.dataService = dataService;
            vm.scope = $scope;
            
            vm.ngTableParams = ngTableParams;
            vm.selectedIssuerId = sourcedata;
            vm.scope.$watch(() => {
                return vm.selectedIssuer;
            }, (newValue: Models.IIssuer, oldValue: Models.IIssuer) => {
                if (newValue != oldValue && newValue.issuerId && !vm.isLoading && !vm.selectedIssuerId) {
                    vm.businessDescription = null;
                    vm.loadAnalystResearches(true);
                }
            });
            vm.loadData();
        }

        openDate = ($event: any, so: Models.ISecurityOverride, field: Models.IField) => {
            $event.preventDefault();
            so['is'+field.jsonPropertyName+'opened'] = true;
        };

        loadAnalystResearches = (addnewanalystresearch) => {
            var vm = this;
            vm.isLoading = true;
            if (vm.selectedIssuer && vm.selectedIssuer.issuerId) {
                vm.dataService.getAnalystResearches(null, vm.selectedIssuer.issuerId).then(analystresearches => {
                    if (analystresearches.length) {
                        vm.data = analystresearches[0].analystResearches;
                        vm.businessDescription = vm.data[0].businessDescription;
                    } else {
                        vm.data = [];
                    }
                    if (addnewanalystresearch) {
                        vm.addNewAnalystResearch();
                    }
                    vm.setEditMode();
                    vm.isLoading = false;
               });
            } else {
                vm.isLoading = false;
            }
        }

        setValuesFomPlaceHolders = () => {
            var vm = this;
            if (vm.data && vm.data.length && !vm.data[0].analystResearchId) {
                for (var i = 0; i < vm.fields.length; i++) {
                    var d = vm.data[0];
                    var field = vm.fields[i];
                    if ((typeof (d[field.jsonPropertyName + 'PlaceHolder']) != 'undefined' && d[field.jsonPropertyName + 'PlaceHolder'] != null && d[field.jsonPropertyName + 'PlaceHolder'].toString().length) &&
                        (typeof(d[field.jsonPropertyName]) == 'undefined'   || !d[field.jsonPropertyName].toString().length)
                    ) {
                        d[field.jsonPropertyName] = d[field.jsonPropertyName + 'PlaceHolder'];
                    }
                } 
            }

            if (vm.data && vm.data.length) {
                if (vm.data[0].cloAnalystUserId) {
                    vm.data[0].cloAnalyst = vm.analysts.cloAnalysts.filter(a => { return a.userId == vm.data[0].cloAnalystUserId })[0].fullName;
                } else {
                    vm.data[0].cloAnalyst = null;
                }

                if (vm.data[0].hfAnalystUserId) {
                    vm.data[0].hfAnalyst = vm.analysts.hFnalysts.filter(a => { return a.userId == vm.data[0].hfAnalystUserId })[0].fullName;
                } else {
                    vm.data[0].hfAnalyst = null;
                }

                if (vm.data[0].asOfDate) {
                    vm.data[0].asOfDate = moment(new Date(vm.data[0].asOfDate.toString())).format("MM/DD/YYYY");
                }
            }

        }

        setEditMode = () => {
            var vm = this;

            if (vm.data && vm.data.length) {
                vm.data[0].isEditMode = true;
                for (var i = 1; i < vm.data.length; i++) {
                    vm.data[i].isEditMode = false;
                }
            }
        }

        getStyle = (field: Models.IField) => {
            var style:any = {};
            if (field.fieldName === 'AsOfDate' || field.fieldName === 'CLOAnalyst' || field.fieldName === 'HFAnalyst' ) {
                style.width = 120;
            } else {
                style.width = 87;
            }
            return style;
        } 

        addNewAnalystResearch = () => {
            var vm = this;
            if (vm.selectedIssuer && vm.selectedIssuer.issuerId) {
                var newAnalystResearch: Models.IAnalystResearch= <Models.IAnalystResearch>{
                    issuerId: vm.selectedIssuer.issuerId
                };

                if (vm.data.length && vm.data[0]) {
                    newAnalystResearch.businessDescription = vm.data[0].businessDescription;
                }

                vm.setValuesFomPlaceHolders();

                if (vm.data.length) {
                    vm.fields.forEach(field => {
                        newAnalystResearch[field.jsonPropertyName + 'PlaceHolder'] = vm.data[0][field.jsonPropertyName];
                    });
                    if (newAnalystResearch['asOfDate' + 'PlaceHolder']) {
                        newAnalystResearch['asOfDate' + 'PlaceHolder'] = moment(new Date(newAnalystResearch['asOfDate' + 'PlaceHolder'].toString())).format("MM/DD/YYYY");
                    }
                    newAnalystResearch.cloAnalystUserId = vm.data[0].cloAnalystUserId;
                    newAnalystResearch.hfAnalystUserId = vm.data[0].hfAnalystUserId;

                    if (vm.data[0].asOfDate) {
                        newAnalystResearch.minAsOfDate = moment(new Date(vm.data[0].asOfDate.toString())).add('days', 1).format("MM/DD/YYYY");    
                    }
                    
                }
                
                vm.data.splice(0, 0, newAnalystResearch);
                vm.setEditMode();
            }
        }

        loadData = () => {
            var vm = this;
            vm.isLoading = true;
            
            vm.dataService.getAnalystResearchHeaderFields().then(fields => {
                vm.fields = fields.analystResearchFields;
                vm.dataService.getIssuers().then(issuers => {
                    vm.issuers = issuers;
                    if (vm.selectedIssuerId) {
                        vm.selectedIssuer = vm.issuers.filter(s => s.issuerId === vm.selectedIssuerId)[0];
                    }

                    vm.dataService.getAnalysts().then(analysts => {
                        vm.analysts = analysts;
                        vm.loadAnalystResearches(false);
                    });
                });
            });
        }


        cancel = () => {
            var vm = this;
            vm.statusText = "Closing";
            vm.modalInstance.dismiss('cancel');
        }

      
        save = () => {
            var vm = this;
            vm.isLoading = true;
            vm.statusText = "Updating";
            vm.setValuesFomPlaceHolders();
            var analystResearches: Array<Models.IAnalystResearch> = JSON.parse(JSON.stringify(vm.data));

            analystResearches.forEach(ar => {
                ar.businessDescription = vm.businessDescription;
            });

            if (analystResearches.length) {

                vm.dataService.saveAnalystResearches(analystResearches).then((result) => {
                    vm.isLoading = false;
                    vm.modalInstance.close(result);
                });
            } else {
                vm.modalInstance.dismiss('cancel');
            }
        }
    }

    angular.module("app").controller("application.controllers.addUpdateAnalystResearchController", AddUpdateAnalystResearchController);
}