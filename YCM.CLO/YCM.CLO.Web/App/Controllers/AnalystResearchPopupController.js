var Application;
(function (Application) {
    var Controllers;
    (function (Controllers) {
        var AnalystResearchPopupController = (function () {
            function AnalystResearchPopupController(dataService, $window, $scope, $modalInstance, ngTableParams, sourcedata) {
                var _this = this;
                this.appBasePath = pageOptions.appBasePath;
                this.isLoading = false;
                this.statusText = "Loading";
                this.labelMap = { "asOfDate": "Quarter Ended", "spacer1": "", "seniorLeverage": "Senior Leverage", "totalLeverage": "Total Leverage", "netTotalLeverage": "Net Total Leverage", "fcfDebt": "FCF Debt", "enterpriseValue": "Enterprise Value", "spacer2": "", "ltmRevenues": "LTM Revenues", "ltmebitda": "LTM EBITDA", "ltmfcf": "LTM FCF", "revenues": "Revenues", "yoYGrowth": "YoY Growth", "organicGrowth": "Organic Growth", "cashEBITDA": "Cash EBITDA", "margin": "Margin", "transactionExpenses": "Transaction Expenses", "restructuringAndIntegration": "Restructuring & Integration", "other1": "Other", "pfebitda": "PF EBITDA", "ltmpfebitda": "LTM EBITDA", "pfCostSaves": "PF Cost Saves", "pfAcquisitionAdjustment": "PF Acquisition Adjustment", "covenantEBITDA": "Covenant EBITDA", "interest": "Interest", "cashTaxes": "Cash Taxes", "workingCapital": "Working Capital", "restructuringOneTime": "Restructuring/One-Time", "other2": "Other", "ocf": "OCF", "capitalExpenditures": "Capital Expenditures", "fcf": "FCF", "ablrcf": "ABL/RCF", "firstLienDebt": "First Lien Debt", "totalDebt": "Total Debt", "equityMarketCap": "Equity Market Cap", "cash": "Cash", "comments": "Comments" };
                this.getDisplayText = function (key) {
                    var vm = _this;
                    var displayText = vm.labelMap[key];
                    return displayText;
                };
                this.setComments = function (detail) {
                    var vm = _this;
                    vm.comments = detail.comments;
                };
                this.getAnalystResearchHeader = function (issuerId) {
                    var vm = _this;
                    vm.isLoading = true;
                    vm.dataService.getAnalystResearchHeader(issuerId).then(function (header) {
                        vm.analystResearchHeader = header;
                        vm.analystResearchHeaderId = header.analystResearchHeaderId;
                    }, function (crap) {
                        alert(crap);
                    }).then(function () {
                        vm.dataService.getAnalystResearchDetails(vm.analystResearchHeaderId).then(function (details) {
                            vm.analystResearchDetails = details;
                            var otherc = Object.keys(details[0]);
                            if (details.length > 0) {
                                vm.lastUpdatedOn = new Date(Math.max.apply(null, details.map(function (e) {
                                    return new Date(e.lastUpdatedOn);
                                })));
                                vm.comments = details[0].comments;
                            }
                            vm.isLoading = false;
                        });
                    });
                };
                this.cancel = function () {
                    var vm = _this;
                    vm.statusText = "Closing";
                    vm.modalInstance.dismiss('cancel');
                };
                var vm = this;
                vm.issuerId = sourcedata;
                vm.modalInstance = $modalInstance;
                vm.dataService = dataService;
                vm.scope = $scope;
                vm.windowService = $window;
                vm.ngTableParams = ngTableParams;
                vm.getAnalystResearchHeader(sourcedata);
                vm.lastUpdatedOn = new Date();
            }
            return AnalystResearchPopupController;
        }());
        AnalystResearchPopupController.$inject = ["application.services.dataService", "$window", "$scope", "$uibModalInstance", 'NgTableParams', 'sourcedata'];
        Controllers.AnalystResearchPopupController = AnalystResearchPopupController;
        angular.module("app").controller("application.controllers.analystResearchPopupController", AnalystResearchPopupController);
    })(Controllers = Application.Controllers || (Application.Controllers = {}));
})(Application || (Application = {}));
//# sourceMappingURL=AnalystResearchPopupController.js.map