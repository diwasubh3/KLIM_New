var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var BidOfferUploadController = (function () {
            function BidOfferUploadController(dataService, $rootScope, modalService, ngTableParams, $filter, timeOutService) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                this.statusText = "Loading";
                this.getStyle = function (column, data) {
                    var style = {};
                    if (column.displayWidth) {
                        style['width'] = column.displayWidth + 40;
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
                };
                this.complete = function (content) {
                    var vm = _this;
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
                    }
                    else {
                        vm.alerts.push(content.error);
                    }
                };
                this.clearUpload = function () {
                    var vm = _this;
                    vm.data = [];
                    vm.alerts = [];
                    vm.infos = [];
                    vm.setParamsTable();
                };
                this.close = function (list, index) {
                    list.splice(index, 1);
                };
                this.startUploading = function () {
                    var vm = _this;
                    vm.isLoading = true;
                    vm.statusText = "Uploading";
                };
                this.save = function () {
                    var vm = _this;
                    vm.isLoading = true;
                    vm.statusText = "Saving";
                    vm.alerts = [];
                    vm.infos = [];
                    vm.dataService.savePricings(vm.data).then(function (d) {
                        vm.isLoading = false;
                        vm.rootScope.$emit('refreshSummaries', null);
                        if (d.status) {
                            vm.data = [];
                            vm.setParamsTable();
                            if (d.message) {
                                vm.infos.push(d.message);
                            }
                        }
                        else {
                            if (d.message) {
                                vm.alerts.push(d.message);
                            }
                        }
                    });
                };
                this.setParamsTable = function () {
                    var vm = _this;
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
                };
                var vm = this;
                vm.dataService = dataService;
                vm.rootScope = $rootScope;
                vm.rootScope.$emit('onActivated', 'maintenance');
                vm.modalService = modalService;
                vm.ngTableParams = ngTableParams;
                vm.filter = $filter;
                vm.isLoading = true;
                vm.dataService.getBidOfferHeaderFields().then(function (fields) {
                    vm.headerFields = fields;
                    vm.isLoading = false;
                });
            }
            return BidOfferUploadController;
        }());
        BidOfferUploadController.$inject = ["application.services.dataService", "$rootScope", '$modal', 'NgTableParams', '$filter', '$timeout'];
        Controllers.BidOfferUploadController = BidOfferUploadController;
        angular.module("app").controller("application.controllers.bidOfferUploadController", BidOfferUploadController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=BidOfferUploadController.js.map