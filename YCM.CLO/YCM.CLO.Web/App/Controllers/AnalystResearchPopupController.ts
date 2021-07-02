module Application.Controllers {
	export class AnalystResearchPopupController {
		modalInstance: angular.ui.bootstrap.IModalServiceInstance;
		appBasePath: string = pageOptions.appBasePath;
		isLoading: boolean = false;
		statusText: string = "Loading";
		analystResearchHeader: Models.IAnalystResearchHeader;
		analystResearchHeaderId: number;
		columns: Array<Date>;
		comments: string;
		analystResearchDetails: Array<Models.IAnalystResearchDetail>;
		dataService: Application.Services.Contracts.IDataService;
		ngTableParams: any;
		tableParams: any;
		lastUpdatedOn: Date;
		scope: ng.IScope;
		issuerId: number
		labelMap: Map<string, string> = { "asOfDate": "Quarter Ended", "spacer1" : "", "seniorLeverage": "Senior Leverage", "totalLeverage": "Total Leverage", "netTotalLeverage": "Net Total Leverage", "fcfDebt": "FCF Debt", "enterpriseValue": "Enterprise Value", "spacer2": "", "ltmRevenues": "LTM Revenues", "ltmebitda": "LTM EBITDA", "ltmfcf": "LTM FCF", "revenues": "Revenues", "yoYGrowth": "YoY Growth", "organicGrowth": "Organic Growth", "cashEBITDA": "Cash EBITDA", "margin": "Margin", "transactionExpenses": "Transaction Expenses", "restructuringAndIntegration": "Restructuring & Integration", "other1": "Other", "pfebitda": "PF EBITDA", "ltmpfebitda": "LTM EBITDA", "pfCostSaves": "PF Cost Saves", "pfAcquisitionAdjustment": "PF Acquisition Adjustment", "covenantEBITDA": "Covenant EBITDA", "interest": "Interest", "cashTaxes": "Cash Taxes", "workingCapital": "Working Capital", "restructuringOneTime": "Restructuring/One-Time", "other2": "Other", "ocf": "OCF", "capitalExpenditures": "Capital Expenditures", "fcf": "FCF", "ablrcf": "ABL/RCF", "firstLienDebt": "First Lien Debt", "totalDebt": "Total Debt", "equityMarketCap": "Equity Market Cap", "cash": "Cash", "comments": "Comments" } ;
		windowService: ng.IWindowService;

		static $inject = ["application.services.dataService", "$window", "$scope", "$uibModalInstance", 'NgTableParams', 'sourcedata'];

		constructor(dataService: Application.Services.Contracts.IDataService, $window: ng.IWindowService, $scope: angular.IScope, $modalInstance: angular.ui.bootstrap.IModalServiceInstance,
			ngTableParams: NgTableParams, sourcedata: any) {
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

		getDisplayText = (key: string) => {
			var vm = this;
			var displayText = vm.labelMap[key];
			return displayText;
		}

		setComments = (detail: Models.IAnalystResearchDetail) => {
			var vm = this;
			vm.comments = detail.comments;
		}

		getAnalystResearchHeader = (issuerId: number) => {
			var vm = this;
			vm.isLoading = true;
			vm.dataService.getAnalystResearchHeader(issuerId).then( header => {
				vm.analystResearchHeader = header;
				vm.analystResearchHeaderId = header.analystResearchHeaderId;
			}, crap => {
				alert(crap);
			}).then(() => {
				vm.dataService.getAnalystResearchDetails(vm.analystResearchHeaderId).then(details => {
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
		}

		cancel = () => {
			var vm = this;
			vm.statusText = "Closing";
			vm.modalInstance.dismiss('cancel');
		}

	}

	angular.module("app").controller("application.controllers.analystResearchPopupController", AnalystResearchPopupController);
}