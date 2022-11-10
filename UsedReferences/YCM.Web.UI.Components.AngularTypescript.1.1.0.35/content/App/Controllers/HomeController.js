var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var HomeController = (function () {
            function HomeController(homeService, $rootScope, modalService, ngTableParams, $filter, timeOutService) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                this.loadData = function () {
                    var vm = _this;
                    vm.statusText = "Loading";
                    vm.isLoading = true;
                    vm.homeService.loadData().then(function (d) {
                        vm.data = d;
                        vm.isLoading = false;
                        vm.setParamsTable();
                    });
                };
                this.sortTableParams = function (columnName) {
                    var vm = _this;
                    var sortObj = {};
                    sortObj[columnName] = vm.tableParams.isSortBy(columnName, 'asc') ? 'desc' : 'asc';
                    vm.tableParams.sorting(sortObj);
                };
                this.setParamsTable = function () {
                    var vm = _this;
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
                            getData: function ($defer, params) {
                                var orderedData = params.sorting() ? vm.filter('orderBy')(vm.data, params.orderBy()) : vm.data;
                                orderedData = params.filter() ? vm.filter('filter')(orderedData, params.filter()) : orderedData;
                                $defer.resolve(orderedData);
                            }
                        });
                    }
                    else {
                        vm.tableParams.reload();
                    }
                };
                this.homeService = homeService;
                this.rootScope = $rootScope;
                this.rootScope.$emit('onActivated', 'home');
                this.modalService = modalService;
                this.ngTableParams = ngTableParams;
                this.filter = $filter;
                this.loadData();
            }
            return HomeController;
        }());
        HomeController.$inject = ["application.services.homeService", "$rootScope", '$modal', 'ngTableParams', '$filter', '$timeout'];
        Controllers.HomeController = HomeController;
        angular.module("app").controller("application.controllers.homeController", HomeController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=HomeController.js.map