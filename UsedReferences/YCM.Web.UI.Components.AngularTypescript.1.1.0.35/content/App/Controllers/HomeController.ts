module Application.Controllers {
    export class HomeController {
        homeService: Application.Services.Contracts.IHomeService;
        rootScope: ng.IRootScopeService;
        
        isLoading: boolean;
        data:Array<Application.Models.UserModel>;
        appBasePath: string = pageOptions.appBasePath;
        ngTableParams:any;
        statusText: string;
        tableParams: any;
        modalService: angular.ui.bootstrap.IModalService;
        filter: ng.IFilterService;
        timeOutService: ng.ITimeoutService;
        static $inject = ["application.services.homeService", "$rootScope", '$modal', 'ngTableParams','$filter','$timeout'];   

        
        constructor(homeService: Application.Services.Contracts.IHomeService, $rootScope: ng.IRootScopeService, modalService: angular.ui.bootstrap.IModalService, ngTableParams: ngTableParams, $filter: ng.IFilterService, timeOutService: ng.ITimeoutService) {
            this.homeService = homeService;
            this.rootScope = $rootScope;
            this.rootScope.$emit('onActivated', 'home');
            this.modalService = modalService;
            this.ngTableParams = ngTableParams;
            this.filter = $filter;
            this.loadData();
        }

        loadData = () => {
            var vm = this;
            vm.statusText = "Loading";
            vm.isLoading = true;
            
            vm.homeService.loadData().then((d) => {
                vm.data = d;
                vm.isLoading = false;
                vm.setParamsTable();
            });
        }

        sortTableParams = (columnName: string) => {
            var vm = this;
            var sortObj = {};
            sortObj[columnName] = vm.tableParams.isSortBy(columnName, 'asc') ? 'desc' : 'asc';
            vm.tableParams.sorting(sortObj);
        }

        setParamsTable = () => {
            var vm = this;
            if (!vm.tableParams) {
                vm.tableParams = new vm.ngTableParams({
                    page: 1,
                    noPager: true,
                    count: 10000,
                    filter: {
                        SearchText: ''
                    },
                    sorting: {
                        'FirstName': 'asc'
                    }
                }, {
                        total: 1,
                        
                        counts: [],
                        getData: ($defer, params) => {
                            var orderedData = params.sorting() ? vm.filter('orderBy')(vm.data, params.orderBy()) : vm.data;
                            orderedData = params.filter() ? vm.filter('filter')(orderedData, params.filter()) : orderedData;
                            $defer.resolve(orderedData);
                            
                        }
                    });
            } else {
                vm.tableParams.reload();
            }
        }

    }

    angular.module("app").controller("application.controllers.homeController", HomeController);
} 