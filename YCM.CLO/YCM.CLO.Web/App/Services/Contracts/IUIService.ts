module Application.Services.Contracts {
    export interface IUIService {
		showWatchModal: (watch: Models.IWatch, modalService: angular.ui.bootstrap.IModalService, inDeleteMode: boolean, watchTypeId: number, codeToExcute: any) => void;
        showBuySellModal: (fund:any, position: any, modalService: angular.ui.bootstrap.IModalService, isBuy: boolean, codeToExcute: any) => void;
        getHeaderTemplate : () => string;
        createCollectionFilters: (positions: Array<Models.IPosition>, sourceObject: any,filterObjName:string,filterCollectionName,  fields: Array<Models.IField>) => void; 
        processSearchText: (positions: Array<Models.IPosition>, fields: Array<Models.IField>) => void;
        processTooltip: (position: any) => void;
        showMessage: (message: Models.IMessage) => void;
        capitalizeFirstLetter : (match:any) => string;
        showTradeModel: (trade: Models.ITrade, modalService: angular.ui.bootstrap.IModalService, codeToExcute: any) => void;
        createColumnDefs: (sourceObject: any, gridOptionsName: string, filtercollectionsName: string, highlightFilteredHeaderName: string, fields: Array<Models.IField>) => void; 
		showLoanComparisonModal: (fund: Models.IFund, selectedViewId: number, positions: any, customViews: Array<Models.ICustomView>, modalService: angular.ui.bootstrap.IModalService, codeToExcuteOnRemoveAll: any) => void;
		showAnalystResearchPopup: (issuerId: number, modalService: angular.ui.bootstrap.IModalService, codeToExcute: any) => void;
		showViewEditorPopup: (viewId: number, modalService: angular.ui.bootstrap.IModalService, codeToExcute: any) => void;
		showUpdateSecurityPopup: (position: any, updateType: string, modalService: angular.ui.bootstrap.IModalService, codeToExcute: any) => void;
	    createWatch: (position: Models.IPosition) => Models.IWatch;
		createSellCandidate: (position: Models.IPosition) => Models.IWatch;
		getDistinctStrings: (field: string, data: Array<Models.IPosition>) => Array<string>;
		filterPositions: (positions: Array<Models.IPosition>, positionFilter: Models.IPositionFilters) => void;
		updateFilterStatistics: (positions: Array<Models.IPosition>, positionFilter: Models.IPositionFilters, field: string) => void;
        sortArrayBySortOrderAsc: (arrayToSort: any, sortField: string) => any;
        showTradeHistoryPopup: (securitycode: string, issuer: string, modalService: angular.ui.bootstrap.IModalService, codeToExcute: any) => void;
        showPaydownModal: (watch: Models.IPaydown, modalService: angular.ui.bootstrap.IModalService, inDeleteMode: boolean, paydownTypeId: number, codeToExcute: any) => void;
        createPaydown: (position: Models.IPosition) => Models.IPaydown;
        showChartsPopup: (modalService: angular.ui.bootstrap.IModalService, trendsData: Array<Application.Models.ITrends>) => void;
    }
} 