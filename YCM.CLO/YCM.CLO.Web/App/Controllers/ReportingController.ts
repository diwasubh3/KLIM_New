module Application.Controllers {
    export class ReportingController {
        dataService: Application.Services.Contracts.IDataService;
        rootScope: ng.IRootScopeService;

        isLoading: boolean;
        selectedSummary: Models.ISummary;
        newParameter:Models.IParameterValue;
        appBasePath: string = pageOptions.appBasePath;
        ngTableParams: any;
        statusText: string = 'Loading';
        selectedFund: Models.IFund;
        reportDisplay: string = 'block';
        capitalStructureDisplay: string = 'none';
        cloStatsDisplay: string = 'none';
        env: string = pageOptions.env;
        defaultedLoansDisplay: string = 'none';
        equityDisplay: string = 'none';
        canEdit: boolean = pageOptions.canEditReportData;
        dateOptions: any = {
            formatYear: 'yy',
            startingDay: 1
        };

        data: Application.Models.ReportingData;

        tableParams: any;
        modalService: angular.ui.bootstrap.IModalService;
        filter: ng.IFilterService;
        timeOutService: ng.ITimeoutService;
		static $inject = ["application.services.dataService", "$rootScope", '$uibModal', 'NgTableParams', '$filter', '$timeout'];

        constructor(dataService: Application.Services.Contracts.IDataService,
            $rootScope: ng.IRootScopeService,
            modalService: angular.ui.bootstrap.IModalService,
            ngTableParams: NgTableParams,
            $filter: ng.IFilterService,
            timeOutService: ng.ITimeoutService) {

            var vm = this;
            vm.dataService = dataService;
            vm.rootScope = $rootScope;
            vm.rootScope.$emit('onActivated', 'reporting');
            vm.modalService = modalService;
            vm.ngTableParams = ngTableParams;
            vm.filter = $filter;
            vm.rootScope.$on('onFundChanged', (event, data: Models.ISummary) => {
                if (vm.data.funds) {
                    vm.onSummaryChanged(data);
                }
            });

            vm.rootScope.$on('onFundSelectionChanged', (event, data: Models.ISummary) => {
                vm.onSummaryChanged(data);
            });
            vm.loadData();
        }

        loadData = () => {
            var vm = this;
            vm.statusText = "Loading";
            vm.isLoading = true;
            vm.dataService.getReportingData().then(d => {
                vm.data = d;
                vm.data.fundAssetClasses.forEach(fa => {
                    fa.startDate = new Date(fa.startDate.toString());
                    fa.endDate = new Date(fa.endDate.toString());

                    if (!vm.selectedFund) {
                        vm.onSummaryChanged(vm.rootScope['selectedFund']);
                    }
                });
                vm.isLoading = false;
            });
        }

        onSummaryChanged = (data: Models.ISummary) => {
            var vm = this;

            if (data) {
                vm.selectedSummary = data;
                vm.selectedFund = vm.data.funds.filter(f => { return f.fundId === vm.selectedSummary.fundId })[0];
            }
            else {
                vm.selectedFund = vm.data.funds[0];
            }
        }


        addNewSecurityOverride = () =>{
            var vm = this;
            vm.data.equityOverrides.push(<Models.EquityOverride>{ fundId: vm.selectedFund.fundId , isDeleted:false});
        }

        canSave = () =>{
            var vm = this;            
            var hasInvalidData = false;
            for (var i = 0; i < vm.data.equityOverrides.length; i++) {
                if (!vm.data.equityOverrides[i].securityCode) {
                    hasInvalidData = true;
                    break;
                }
            }
            
            return !hasInvalidData;
        }

        getFilteredAssetClasses = () => {
            var vm = this;
            return vm.data.fundAssetClasses.filter(f => f.fundId == vm.selectedFund.fundId);
        }

        save = () => {
            var vm = this;
            vm.isLoading = true;
            vm.dataService.saveReportingData(vm.data).then(data => {
                vm.data = data;
                vm.data.fundAssetClasses.forEach(fa => {
                    fa.startDate = new Date(fa.startDate.toString());
                    fa.endDate = new Date(fa.endDate.toString());
                });
                vm.selectedFund = vm.data.funds.filter(f => { return f.fundId === vm.selectedFund.fundId })[0];
                vm.isLoading = false;
            });
        }

        deleteSecurityOverride = (eo: Models.EquityOverride) => {
            var vm = this;
            if (eo.id) {
                eo.isDeleted = true;
            }
            else {
                vm.data.equityOverrides.splice(vm.data.equityOverrides.indexOf(eo), 1);
            }
        }

        openDate = ($event: any, src :any, prop: any) => {
            var vm = this;
            $event.preventDefault();
            src[prop] = true;
        }

        openTab = (tabName) => {
            var vm = this;
            vm.reportDisplay = 'none';
            vm.capitalStructureDisplay = 'none';
            vm.cloStatsDisplay = 'none';
            vm.defaultedLoansDisplay = 'none';
            vm.equityDisplay = 'none';

            switch (tabName) {
                case 'Report':
                    vm.reportDisplay = 'block';
                    break;
                case 'Capital Structure':
                    vm.capitalStructureDisplay = 'block';
                    break;
                case 'CLO Stats':
                    vm.cloStatsDisplay = 'block';
                    break;
                case 'Defaulted Loans':
                    vm.defaultedLoansDisplay = 'block';
                    break;
                case 'Equity':
                    vm.equityDisplay = 'block';
                    break;
                default:
            }
        
    }

       
    }

    angular.module("app").controller("application.controllers.reportingController", ReportingController);
} 