module Application.Controllers {
    export class LoanAttributeOverrideReconController {
        uiService: Application.Services.Contracts.IUIService;
        dataService: Application.Services.Contracts.IDataService;
        rootScope: ng.IRootScopeService;
        isLoading: boolean;
        data: Array<Application.Models.IVwSecurityDto>;
        appBasePath: string = pageOptions.appBasePath;
        ngTableParams: any;
        statusText: string;
        tableParams: any;
        modalService: angular.ui.bootstrap.IModalService;
        filter: ng.IFilterService;
        timeOutService: ng.ITimeoutService;
        
        headerFields :Array<Models.IField>;
        
        static $inject = ["application.services.uiService","application.services.dataService", "$rootScope", '$modal', 'NgTableParams', '$filter', '$timeout'];

        constructor(uiService: Application.Services.Contracts.IUIService, dataService: Application.Services.Contracts.IDataService, $rootScope: ng.IRootScopeService, modalService: angular.ui.bootstrap.IModalService, ngTableParams: NgTableParams, $filter: ng.IFilterService, timeOutService: ng.ITimeoutService) {
            var vm = this;
            vm.uiService = uiService;
            vm.dataService = dataService;
            vm.rootScope = $rootScope;
            vm.rootScope.$emit('onActivated', 'wso');
            vm.modalService = modalService;
            vm.ngTableParams = ngTableParams;
            vm.filter = $filter;
            
            vm.isLoading = true;
            vm.dataService.getLoanAttributeOverrideReconHeaderFields().then((sf) => {
                vm.loadData(sf);
            });

            vm.rootScope.$on('onAutoRefresh', (event, data: Models.ISummary) => {
                vm.loadLoanAttributeOverrides(null);
            });

            vm.rootScope.$emit('refreshSummaries', null);
        }

        loadData = (headerFields:Array<Models.IField>) => {
            var vm = this;
            if (!vm.headerFields) {
                vm.headerFields = headerFields;
            }
            vm.loadLoanAttributeOverrides(null);
            vm.statusText = "Loading";
        }

        loadLoanAttributeOverrides = (securityId) => {
            var vm = this;
            vm.isLoading = true;
            vm.dataService.getLoanAttributeOverrides(securityId).then((d) => {
                if (!securityId) {
                    vm.data = d;    
                } else {
                    var security = vm.data.filter(d => { return d.securityId === securityId })[0];
                    if (d && d.length) {
                        security.loanAttributeOverrides = d[0].loanAttributeOverrides;
                    } else {
                        vm.data.splice(vm.data.indexOf(security), 1);
                    }
                }
                
                vm.isLoading = false;
                vm.setParamsTable();
            });
        }

        editSecurityOverride = (securityOverride: Models.ISecurityOverride) => {
            if (!securityOverride) {
                securityOverride = <Models.ISecurityOverride>{};
            }

            var vm = this;
            var modalInstance = vm.modalService.open({
                templateUrl: pageOptions.appBasePath + 'app/views/addupdatesecurityoverride.html?v=' + pageOptions.version,
                controller: 'application.controllers.addUpdateSecurityOverrideController',
                controllerAs: 'so',
                size: 'x-lg',
                resolve: {
                    sourcedata: () => {
                        var modelSourceData = securityOverride.securityId;
                        return modelSourceData;
                    }
                }
            });

            modalInstance.result.then((result: Array<Models.IParameterValue>) => {
                if (result) {
                    vm.loadData(vm.headerFields);    
                }
            }, () => { });
        }

        endSecurityOverride = (loanAttributeOverride: Models.ILoanAttributeDto) => {
            var vm = this;
            vm.isLoading = true;
            vm.dataService.endSecurityOverride(loanAttributeOverride).then((d) => {
                if (d.status) {
                    vm.loadLoanAttributeOverrides(loanAttributeOverride.securityId);
                    var message: Models.IMessage = {
                        header: "Please note:", body: "<p><b>You decided to accept the WSO Value.</b></p><br/><br/>" +
                        "<p><b>The system has therefore gone ahead and ended the York override value <br/>effective: " + d.endDate +"</b></p>"
                    };
                    vm.uiService.showMessage(message);
                } else {
                    vm.isLoading = false;
                }
            });
        }

        resetConflict = (loanAttributeOverride: Models.ILoanAttributeDto) => {
            var vm = this;
            vm.isLoading = true;
            vm.dataService.resetSecurityOverrideConflict(loanAttributeOverride).then((d) => {
                if (d.status) {
                    vm.loadLoanAttributeOverrides(loanAttributeOverride.securityId);
                } else {
                    vm.isLoading = false;
                }
            });
        }

        capitalizeFirstLetter = (match) => {
            return angular.uppercase(match.charAt(0)) + match.slice(1);
        }

        getStyle = (column: Models.IField,data:Models.ISecurityOverride) => {
            var style = {};
            style['width'] = column.displayWidth;

            if (column.fieldTitle === 'ATTRIBUTE') {
                style['width'] = 150;
            }
            return style;
        }

        setParamsTable = () => {
            var vm = this;

            var filterSearchText = (data, filterValues) => {
                filterValues.searchText = filterValues.searchText.toLowerCase();
                return data.filter(item => {
                    item.loanAttributeOverrides.forEach(s => {
                        s['isVisible'] = !filterValues.searchText || s.searchText.indexOf(filterValues.searchText) >= 0;
                    });
                    return item.loanAttributeOverrides.filter(s => { return s['isVisible'] }).length;
                });
            }
                vm.tableParams = new vm.ngTableParams({
                    page: 1,
                    noPager: true,
                    count: 10000,
                    filtering: {
                       searchText:''
                    },
                    sorting: {
                        'securityCode': 'asc'
                    }
                }, {
                        total: 1,
                        counts: [],
                        dataset: vm.data,
                        filterOptions: { filterFn: filterSearchText }
                });
            
        }
    }

    angular.module("app").controller("application.controllers.loanAttributeOverrideReconController", LoanAttributeOverrideReconController);
} 