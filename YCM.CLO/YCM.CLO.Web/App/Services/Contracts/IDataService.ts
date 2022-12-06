
module Application.Services.Contracts {
    export interface IDataService {
        userIsAnAdmin: () => ng.IPromise<boolean>;
        userIsASuperUser: () => ng.IPromise<boolean>;
        getFieldsForCustomView: (viewId: number) => ng.IPromise<Array<Models.IField>>;
        getCustomViews: () => ng.IPromise<Array<Models.ICustomView>>;
        getPerson: () => ng.IPromise<Array<Models.ICustomView>>;
        getCustomView: (viewId: number) => ng.IPromise<Models.ICustomView>;
        viewNameIsTaken: (viewName: string) => ng.IPromise<boolean>;
        deleteCustomView: (view: Models.ICustomView) => ng.IPromise<Array<Models.ICustomView>>;
        saveCustomView: (view: Models.ICustomView) => ng.IPromise<Array<Models.ICustomView>>;
        updateSecurity: (security: Models.ISecurity) => ng.IPromise<Application.Models.ISecurity>;
        updateIsPrivate: (issuer: Models.IIssuer) => ng.IPromise<Application.Models.IIssuer>;
        getAnalystResearchDetails: (headerId: number) => ng.IPromise<Array<Application.Models.IAnalystResearchDetail>>;
        getAnalystResearchIssuerIds: () => ng.IPromise<Array<number>>;
        getAnalystResearchHeader: (issuerId: number) => ng.IPromise<Application.Models.IAnalystResearchHeader>;
        downloadSummaries: () => void;
        downloadLoanPositions: (fundId: number) => void;
        downloadReInvestCash: (filePath: string) => void;
        loadData: () => ng.IPromise<Array<Application.Models.UserModel>>;
        updateData: (userModel: Application.Models.UserModel) => ng.IPromise<boolean>;
        loadSummaryData: () => ng.IPromise<Array<Application.Models.ISummary>>;
        loadTestResults: () => ng.IPromise<Array<Application.Models.ITestResults>>;
        loadTrends: (startDate: any, endDate: any, trendTypeId: number, periodId) => ng.IPromise<Array<Application.Models.ITrends>>;
        loadRules: () => ng.IPromise<Array<Application.Models.IRule>>;
        loadTop10Bottom10: (fund: Models.ISummary, ruleId: number) => ng.IPromise<Models.ITop10Bottom10Positions>;
        loadPositionViewFieldGroups: () => ng.IPromise<Array<Application.Models.IFieldGroup>>;
        loadFixedFields: () => ng.IPromise<Array<Application.Models.IField>>;
        loadParameterValues: () => ng.IPromise<Array<Application.Models.IParameterValue>>;
        loadParameterValuesForParameterType: (parameterType: string) => ng.IPromise<Array<Application.Models.IParameterValue>>;
        loadParameterTypes: () => ng.IPromise<Array<Application.Models.IParameterType>>;
        getCustomPositionViewFieldGroups: () => ng.IPromise<Array<Application.Models.IFieldGroup>>;
        getAllFieldGroups: () => ng.IPromise<Array<Application.Models.IFieldGroup>>;
        getAllCustomViewFields: () => ng.IPromise<Array<Application.Models.ICustomViewField>>;

        loadPositions: (fund: Models.ISummary, onlyWithExposures: boolean) => ng.IPromise<Array<Models.IPosition>>;
        getPositions: (securityCode: string, fundCode: string) => ng.IPromise<Array<Models.IPosition>>;
        getPositionsForSecurities: (securityIds: Array<number>, fundCode: string) => ng.IPromise<Array<Models.IPosition>>;

        loadFundRestrictionsTypes: () => ng.IPromise<Array<Models.IFundRestrictionsTypes>>;
        loadFundRestrictions: (fundId: number) => ng.IPromise<Array<Models.IFundRestriction>>;
        loadFundRestrictionFields: () => ng.IPromise<Array<Models.IField>>;
        loadOperators: () => ng.IPromise<Array<Models.IOperator>>;
        saveFundRestrictions: (fundRestrictions: Array<Models.IFundRestriction>) => ng.IPromise<Array<Models.IFundRestriction>>;

        updateWatch: (watch: Application.Models.IWatch, fundCode: string) => ng.IPromise<Models.IWatch>;
        deleteWatch: (watch: Application.Models.IWatch) => ng.IPromise<boolean>;


        updateParameterValue: (parametrWatch: Application.Models.IParameterValue) => ng.IPromise<Models.IParameterValue>;

        getSecurityOverrides: (securityOverrideType: number, securityId: number) => ng.IPromise<Array<Models.ISecurityOverride>>;
        getGroupedSecurityOverrides: (securityOverrideType: number, securityId: number) => ng.IPromise<Array<Models.ISecurityOverride>>;
        saveSecurityOverides: (securityOverrides: Array<Models.ISecurityOverride>) => ng.IPromise<Array<Models.ISecurityOverride>>;

        getSecurities: () => ng.IPromise<Array<Models.IVwSecurityDto>>;


        getCurrentSecurities: () => ng.IPromise<Array<Models.IVwSecurityDto>>;
        getSecurityOverrideHeaderFields: () => ng.IPromise<Array<Models.IField>>;
        getSaveSecurityOverrideHeaderFields: () => ng.IPromise<Array<Models.IField>>;

        getSecurityOverrideableFields: () => ng.IPromise<Array<Models.IField>>;

        getAnalystResearches: (loadType: number, issuerId: number) => ng.IPromise<Array<Models.IGroupedAnalystRefresh>>;
        getAnalystResearchHeaderFields: () => ng.IPromise<Models.IAnalystResearchHeaderFileds>;
        getIssuers: () => ng.IPromise<Array<Models.IIssuer>>;

        getAnalysts: () => ng.IPromise<Models.IAnalysts>;
        saveAnalystResearches: (analystResearches: Array<Models.IAnalystResearch>) => ng.IPromise<boolean>;

        getBidOfferHeaderFields: () => ng.IPromise<Array<Models.IField>>;
        savePricings: (pricings: Array<Models.IPricing>) => ng.IPromise<any>;

        getSecurityReconHeaderFields: () => ng.IPromise<Array<Models.IField>>;
        getSecuritiesForRecon: () => ng.IPromise<Array<Models.ISecurityRecon>>;

        reconcileSecurities: (securities: Array<Models.ISecurityRecon>) => ng.IPromise<boolean>;

        getLoanAttributeOverrideReconHeaderFields: () => ng.IPromise<Array<Models.IField>>;
        getLoanAttributeOverrides: (securityId: number) => ng.IPromise<Array<Models.IVwSecurityDto>>;

        endSecurityOverride: (loanAttributeOverride: Models.ILoanAttributeDto) => ng.IPromise<any>;
        resetSecurityOverrideConflict: (loanAttributeOverride: Models.ILoanAttributeDto) => ng.IPromise<any>;

        getTradeAllocations: (securityId: number) => ng.IPromise<Array<Models.ITradeAllocation>>;
        saveTrades: (trade: Models.ITrade, processSaveTrade: boolean) => ng.IPromise<boolean>;
        getTradesHeaderFields: () => ng.IPromise<Array<Models.IField>>;
        getTrades: (includeCancelled: boolean, fundCode: string) => ng.IPromise<Array<Models.ITrade>>;
        getTradeSourceData: () => ng.IPromise<Models.ITradeSourceData>;
        getTradeBookingData: () => ng.IPromise<Models.ITradeBookingData>;
        generateTradeXML: (data: Models.ITradeBooking) => ng.IPromise<Models.ITradeBooking>;
        cancelTrade: (data: Models.ITradeBooking) => ng.IPromise<Models.ITradeBooking>;
        getTradeFundAllocation: (data: Models.ITradeBooking) => ng.IPromise<Array<Models.ITradeBookingDetail>>;
        getCalculatedData: (data: Array<Models.ITradeBookingDetail>) => ng.IPromise<Array<Models.ITradeBookingDetail>>;//, totalQty: number, ruleName: string
        refreshTradeBooking: (tradeId: number) => ng.IPromise<Models.ITradeBooking>;
        getIssuerSecurities: () => ng.IPromise<Array<Models.IIssuerSecurity>>;
        getIssuerList: () => ng.IPromise<Array<Models.IIssuerSecurity>>;
        getTradeBooking: () => ng.IPromise<Array<Models.ITradeBooking>>;
        getTradeBookingHistory: () => ng.IPromise<Array<Models.ITradeBooking>>;
        getAllocationRule: (tradeTypeId: number) => ng.IPromise<Array<Models.IAllocationRule>>;
        getBloombergData: (securityCode: string) => ng.IPromise<Models.ITempSecurity>;
        saveTempSecurity: (tempsecurity: Models.ITempSecurity) => ng.IPromise<Models.ISecurity>;

        getFunds: () => ng.IPromise<Array<Models.IFund>>;
        saveFund: (fund: Models.IFund) => ng.IPromise<Models.IFund>;

        getLastTradeSwap: () => ng.IPromise<Models.ITradeSwap>;
        startTradeSwap: (tradeSwapParam: Models.ITradeSwapParam) => ng.IPromise<Models.ITradeSwap>;

        getTradeSnapshots: (fundid: number, tradeswapid: number, groupby: number) => ng.IPromise<Array<Models.IGroupedTradeSwapSnapshot>>;

        process: (url: string) => ng.IPromise<boolean>;

        getMoodyRatings: () => ng.IPromise<Array<Models.IRating>>;

        getRatings: () => ng.IPromise<Array<Models.IRating>>;

        getFilterFieldGroups: () => ng.IPromise<Array<Application.Models.IFieldGroup>>;

        getMajors: (fundId: number) => ng.IPromise<Array<Array<Application.Models.IMatrixData>>>;

        getMinorsByDiversity: (fundId: number, fromDiversity: number, toDiversity: number) => ng.IPromise<Array<Array<Application.Models.IMatrixData>>>;
        getMinorsBySpread: (fundId: number, fromSpread: number, toSpread: number) => ng.IPromise<Array<Array<Application.Models.IMatrixData>>>;


        getMatrixPoints: (fundId: number) => ng.IPromise<Array<Application.Models.IMatrixPoint>>;
        addMatrixPoint: (matrixData: Models.IMatrixData) => ng.IPromise<Array<Application.Models.IMatrixPoint>>;

        getReportingData: () => ng.IPromise<Models.ReportingData>;

        saveReportingData: (data: Models.ReportingData) => ng.IPromise<Models.ReportingData>;
        getTradeHistory: (securityCode: string) => ng.IPromise<Array<Application.Models.ITradeHistory>>;

        updatePaydown: (watch: Application.Models.IPaydown, fundCode: string) => ng.IPromise<Models.IPaydown>;
        deletePaydown: (watch: Application.Models.IPaydown) => ng.IPromise<boolean>;
        getFilteredTrades: (startDate: any, endDate: any) => ng.IPromise<Array<Models.ITradeBooking>>;
        loadTrendTypes: () => ng.IPromise<Array<Models.ITrendType>>;
        loadPeriod: () => ng.IPromise<Array<Models.ITrendPeriod>>;
        getRatingchanges: () => ng.IPromise<Array<Models.IRatingChange>>;
        getTotalParDifferenceforUI: () => ng.IPromise<Array<Models.ITotalParChanges>>;
        getMoodyRecoveryDateChanges: () => ng.IPromise<Array<Models.IMoodyRecoveryChange>>;
        getTopBottomPriceMoversChanges: () => ng.IPromise<Models.ITopBottonPriceMovers>;

    }
}