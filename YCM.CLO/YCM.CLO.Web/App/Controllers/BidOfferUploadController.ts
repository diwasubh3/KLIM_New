module Application.Controllers {
    export class BidOfferUploadController {
        dataService: Application.Services.Contracts.IDataService;
        rootScope: ng.IRootScopeService;

        isLoading: boolean;
        data: Array<Application.Models.IPricing>;
        appBasePath: string = pageOptions.appBasePath;
        ngTableParams: any;
        statusText: string = "Loading";
        tableParams: any;
        modalService: angular.ui.bootstrap.IModalService;
        filter: ng.IFilterService;
        timeOutService: ng.ITimeoutService;
        headerFields: Array<Models.IField>;
        alerts: Array<string>;
        infos: Array<string>;
        static $inject = ["application.services.dataService", "$rootScope", '$modal', 'NgTableParams', '$filter', '$timeout'];

        constructor(dataService: Application.Services.Contracts.IDataService, $rootScope: ng.IRootScopeService, modalService: angular.ui.bootstrap.IModalService, ngTableParams: NgTableParams, $filter: ng.IFilterService, timeOutService: ng.ITimeoutService) {
            var vm = this;
            vm.dataService = dataService;
            vm.rootScope = $rootScope;
            vm.rootScope.$emit('onActivated', 'maintenance');
            vm.modalService = modalService;
            vm.ngTableParams = ngTableParams;
            vm.filter = $filter;
            vm.isLoading = true;
            vm.dataService.getBidOfferHeaderFields().then((fields) => {
                vm.headerFields = fields;
                vm.isLoading = false;
            });
        }

        getStyle = (column: Models.IField, data: Models.ISecurityOverride) => {
            var style = {};
            if (column.displayWidth) {
                style['width'] = column.displayWidth  + 40;
            }

            if (column.jsonPropertyName === 'issuer') {
                style['width'] = column.displayWidth + 170;
            }

            if (column.jsonPropertyName === 'facility') {
                style['width'] = column.displayWidth + 80;
            }
            
            if (data) {
                style['border-width'] = 0;
            }

            if (column.jsonPropertyName === 'bid' || column.jsonPropertyName === 'offer') {
                style['text-align'] = 'right';
            }
            
            if (data && data.isHistorical) {
                style['color'] = 'lightgray';
            }
            return style;
        }

        complete = (content: any) => {
            var vm = this;
            vm.isLoading = false;
            vm.data = [];
            vm.alerts = [];
            vm.infos = [];

            if (content.fileUploaded) {
                vm.data = content.data;
                if (content.message) {
                    content.message = content.message.replace("_COUNT_", "<b><u>" + vm.data.length + "</u></b>");
                    vm.infos.push(content.message);
                }
                vm.setParamsTable();
            } else {
                vm.alerts.push(content.error);
            }
        }

        clearUpload = () => {
            var vm = this;
            vm.data = [];
            vm.alerts = [];
            vm.infos = [];
            vm.setParamsTable();
       }


        close = (list,index) => {
            list.splice(index, 1);
        }

        startUploading = () => {
            var vm = this;
            vm.isLoading = true;
            vm.statusText = "Uploading";
            
        }

        save = () => {
            var vm = this;
            vm.isLoading = true;
            vm.statusText = "Saving";
            vm.alerts = [];
            vm.infos = [];
            vm.dataService.savePricings(vm.data).then(d => {
                vm.isLoading = false;
                vm.rootScope.$emit('refreshSummaries', null);
                if (d.status) {
                    vm.data = [];
                    vm.setParamsTable();
                    if (d.message) {
                        vm.infos.push(d.message);
                    }
                } else {
                    if (d.message) {
                        vm.alerts.push(d.message);    
                    }
                }
            });
        }

        setParamsTable = () => {
            var vm = this;
            vm.tableParams = new vm.ngTableParams({
                page: 1,
                noPager: true,
                count: 10000,
                filtering: {
                    searchText: ''
                },
                sorting: {
                    'securityCode': 'asc',
                    'issuer': 'asc',
                    'facility': 'asc'
                }
            }, {
                    total: 1,
                    counts: [],
                    dataset: vm.data
                });
        }


    }

    angular.module("app").controller("application.controllers.bidOfferUploadController", BidOfferUploadController);
} 