module Application.Models {
    export interface  UserModel {
        firstName: string;
        lastName:string;
    }

    export interface ISummary {
        fundCode: string;
        par: Number;
        spread: Number;
        totalCoupon: Number;
        warf: Number;
        moodyRecovery: Number;
        bid: Number;
		principalCash: Number;
		assetPar: Number;
	    wsoSpread: Number;
	    wsowarf: Number;
	    wsoMoodyRecovery: Number;
	    wsowaLife: Number;
	    wsoDiversity: Number;

        cleanNav: any;
        bodCleanNav: any;

        inActive: boolean;
        fundId: number;

        parBgStyle: any;
        spreadBgStyle: any;
        totalCouponBgStyle: any;
        warfBgStyle: any;
        moodyRecoveryBgStyle: any;
        waBidBgStyle: any;
        bidBgStyle:any;
        principalCashBgStyle: any;
        snpRecovery: any;
        diversityCalcBgStyle: any;
		cleanNavBgStyle: any;
		sortOrder:number;
        
    }

    export interface IRule {
        ruleId: number;
        ruleName: string;
        ruleFields:Array<IRuleField>;
    }

    export interface IPosition
    {
        positionId:number;
        fundCode: string;
        issuerId: number;
        issuerDesc: string;
	    issuer: string;
	    securityCode: string;
		securityId: number;
	    bbgId: string;

		watchId: number;
		sellCandidateId: number;

		sellCandidateObjectTypeId: number;
	    sellCandidateObjectId: number;
	    isSellCandidate: boolean;
	    sellCandidateComments: string;
	    sellCandidateLastUpdatedOn: string;
	    sellCandidateUser: string;

		watchObjectTypeId: number;
        watchObjectId: number;
        isOnWatch: boolean;
        watchComments: string;
		inDeleteMode: boolean;
		watchTypeId: number;
        toolTipText: string;
        watchLastUpdatedOn: string;
        watchUser:string;
        searchText: string;
        isOnAlert: boolean;
        alerts: Array<IAlert>;
        offer: number;
        bid: number;
        exposure: number;
        totalPar:number;
        pctExposure:string;
        hasBuyTrade: boolean;
        hasSellTrade: boolean;
	    isPrivate: boolean;

        clO1Exposure: string;
        clO2Exposure: string;
        clO3Exposure: string;
        clO4Exposure: string;
	    clO5Exposure: string;
	    clO6Exposure: string;
        clO7Exposure: string;
        clO8Exposure: string;
	    trsExposure: string;
	    wH1Exposure: string;
        clO1PctExposure: string;
        clO2PctExposure: string;
        clO3PctExposure: string;
        clO4PctExposure: string;
	    clO5PctExposure: string;
	    clO6PctExposure: string;
        clO7PctExposure: string;
        clO8PctExposure: string;
	    trsPctExposure: string;
	    wH1PctExposure: string;
        clO1NumExposure: number;
        clO2NumExposure: number;
        clO3NumExposure: number;
        clO4NumExposure: number;
	    clO5NumExposure: number;
	    clO6NumExposure: number;
        clO7NumExposure: number;
        clO8NumExposure: number;
	    trsNumExposure: number;
	    wH1NumExposure: number;

        forCompare: boolean;
        trades: Array<ITradeInfo>;
        isFilterSuccess:boolean;
	    enterpriseValue: string;
		ltmfcf: string;
		seniorLeverage: string;
		scoreDescription: string;
		globalAmount: number;
		globalAmountString: string;
		sponsor: string;
        snpWarf: string;
        snpLgd: string;
        moodysLgd: string;
        yieldAvgLgd: string;
        snpWarfPct: string;
        snpLgdPct: string;
        moodysLgdPct: string;
        YieldAvgLgdPct: string;
        securityDesc: string;

        tradeHistoryExist: boolean;
        clO1TradeHistoryExist: boolean;
        clO2TradeHistoryExist: boolean;
        clO3TradeHistoryExist: boolean;
        clO4TradeHistoryExist: boolean;
        clO5TradeHistoryExist: boolean;
        clO6TradeHistoryExist: boolean;
        clO7TradeHistoryExist: boolean;
        clO8TradeHistoryExist: boolean;
        clO9TradeHistoryExist: boolean;
        clO10TradeHistoryExist: boolean;
        clO11TradeHistoryExist: boolean;
    }

    export interface IAlert {
        description:string;
    }

	export interface ICustomView {
		viewId: number;
		viewName: string;
		displayName: string;
		isPublic: boolean;
		isDefault: boolean;
		isDisabled: boolean;
		customViewFields: Array<ICustomViewField>;
	}

	export interface ICustomViewField {
		customViewFieldId: number;
		viewId: number;
		fieldGroupId: number;
		fieldId: number;
		fieldTitle: string;
		field: IField;
		sortOrder: number;
		isHidden: boolean;
	}

    export interface IField
    {
        fieldId: Number;
        fieldGroupId: number;
        fieldName: string;
        jsonPropertyName: string;
        displayWidth:number;
        fieldTitle: string;
        fieldType: number;
        cellTemplate: string;
        pinnedLeft: boolean;
        hidden:boolean;
        headerTemplate: string;
        filter: any;
        headerCellClass: string;
        cellClass: string;
        fieldGroupName: string;
		isSecurityOverride: boolean;
		jsonFormatString: string;
		sortOrder: number;
		filterOrder: number;
    }

    export interface IRuleField  {
        ruleFieldId: number,
        fieldId: number,
        field: IField;
        ruleSectionType:IRuleSectionType;
    }

    export interface IRuleSectionType {
        ruleSectionTypeId: number;
        ruleSectionName:string;
    }

    export interface IFieldGroup
    {
        fields: Array<IField>;
        fieldGroupId: number;
        fieldGroupName: string;
        displayIcon: string;
        cancelled: boolean;
    }

    export interface IFundRestrictionsTypes
    {
        displayColor: string;
        fundRestrictionTypeId: number;
        displayColorStyle: any;
		fundRestrictionTypeName: string;
		fundRestrictionToolTip: string;
    }

    export interface IFundRestriction {
        fundRestrictionTypeId: number;
        jsonPropertyName: string;
        restrictionValue: number;
        operatorCode: string;
        operatorVal: string;
		fundId: number;
		fieldName: string;
		restrictionValueCurrent: number;
		restrictionValuePrevious: number;
		fundRestrictionToolTip: string;
		displayColor: string;
        isDifferenceOverThreshold: boolean;
        isDisabled: boolean;
    }

    export interface IOperator{
        operatorId: number;
        operatorVal: string;
        operatorCode:string;
    }

    export interface ITop10Bottom10Positions {
        topPositions: Array<IPosition>;
        bottomPositions: Array<IPosition>;
    }


    export interface IWatch {
        watchId: number;
        watchObjectTypeId: number;
        watchObjectId:number;
		isOnWatch: boolean;
	    inDeleteMode: boolean;
		watchTypeId: number;
        watchComments: string;
		watchHtmlText: string;
		securityId: number;
		issuerId: number;
	    issuer: string;
	    securityCode: string;
    }


    export interface IParameterValue {
        id: number;
        parameterTypeId: number;
        parameterValueNumber: number;
        parameterValueText: string;
        parameterMinValueNumber: number;
        parameterMaxValueNumber:number;
        parameterType:Models.IParameterType;
    }

    export interface IParameterType {
        parameterTypeId: number;
        parameterTypeName:string;
    }

    export interface IFacility {
        facilityId: number;
        facilityDesc: string;
    }

    export interface IIssuer {
        issuerId: number;
		issuerDesc: string;
		isPrivate: boolean;
    }

    export interface ISecurity {
        securityId: number;
        securityCode: string;
        securityDesc: string;
        bbgId: string;
        issuerId: number;
        facilityId: number;
        callDate: string;
        countryId: number,
        maturityDate: string;
        snPIndustryId: number;
        moodyIndustryId: number;
        isCovLite: boolean;
        isFloating: boolean;
		lienTypeId: number;
       
        facility: IFacility;
        issuer: IIssuer;
        securityName: string;
        attribute:string;
    }

    export interface ISecurityOverride {
        securityOverrideId: number;
        securityId: number;
        fieldId: number;
        overrideValue: string;
        effectiveFrom: string,
        effectiveTo: string;
        isDeleted: boolean;
        field: Models.IField;
        security: Models.ISecurity;
        securtiyName: string;
        securityCode:string;
        searchText: string;
        facilityDesc: string;
        issuerDesc: string;
        isDirty: boolean;
        comments: string;
        isCurrent: boolean;
        isFuture: boolean;
        isHistorical: boolean;
        existingValue: string;
    }

	export interface IAnalystResearchDetail {
		asOfDate: string;
		spacer1: string;
		seniorLeverage: string;
		totalLeverage: string;
		netTotalLeverage: string;
		fcfDebt: string;
		enterpriseValue: string;
		spacer2: string;
		ltmRevenues: string;
		ltmebitda: string;
		ltmfcf: string;
		spacer3: string;
		revenues: string;
		yoYGrowth: string;
		organicGrowth: string;
		spacer4: string;
		cashEBITDA: string;
		margin: string;
		spacer5: string;
		transactionExpenses: string;
		restructuringAndIntegration: string;
		other1: string;
		pfebitda: string;
		spacer6: string;
		ltmpfebitda: string;
		pfCostSaves: string;
		pfAcquisitionAdjustment: string;
		covenantEBITDA: string;
		spacer7: string;
		interest: string;
		cashTaxes: string;
		workingCapital: string;
		restructuringOneTime: string;
		other2: string;
		ocf: string;
		capitalExpenditures: string;
		fcf: string;
		ablrcf: string;
		firstLienDebt: string;
		totalDebt: string;
		equityMarketCap: string;
		cash: string;
		comments: string;
		lastUpdatedOn: Date;
	}

	export interface IAnalystResearchHeader {
		analystResearchHeaderId: number;
		analystId: number;
		issuerId: number;
		issuer: string;
		cloAnalystUserId: number;
		hfAnalystUserId: number;
		cloAnalyst: string;
		hfAnalyst: string;
		creditScore: number;
		agentBank: string;
		businessDescription: string;
		comments: string;
		lastUpdatedOn: Date;
		sponsor: string;
	}

    export interface IAnalystResearch  {
        analystId: number;
        issuerId: number;
        cloAnalystUserId: number;
        hfAnalystUserId: number;
        cloAnalystIdPlaceHolder: string;
        hfAnalystIdPlaceHolder: string;
        cloAnalyst: string;
        hfAnalyst: string;
        asOfDate: string;
        creditScore: number;
        oneLLeverage: number;
        totalLeverage: number;
        evMultiple: number;
        ltmRevenues: number;
        ltmebitda: number;
        fcf: number;
        comments: string;
        businessDescription: string;

        asOfDatePlaceHolder: string;
        creditScorePlaceHolder: number;
        oneLLeveragePlaceHolder: number;
        totalLeveragePlaceHolder: number;
        evMultiplePlaceHolder: number;
        ltmRevenuesPlaceHolder: number;
        ltmebitdaPlaceHolder: number;
        fcfPlaceHolder: number;
        commentsPlaceHolder: string;
        analystResearchId:number;

        minAsOfDate:string;
        issuer: string;
        isVisible: boolean;
		isEditMode: boolean;
    }

    export interface IGroupedAnalystRefresh {
        issuerData: Models.IAnalystResearch;
        analystResearches: Array<IAnalystResearch>;
    } 

    export interface IAnalystResearchHeaderFileds {
        issuerFields: Array<Models.IField>;
        analystResearchFields : Array<Models.IField>;
    }

    export interface IUser {
        userId: number;
        fullName: string;
    }

    export interface IAnalysts  {
        cloAnalysts: Array<Models.IUser>;
        hFnalysts:Array<Models.IUser>;
    }

    export interface IPricing {
        bid: number;
        offer: number;
        createdOn: string;
        createdByFullName: string;
        createdBy: string;
        securityCode: string;
        issuer: string;
        facility:string;
    }

    export interface ISecurityRecon {
        source: string;
        code: string;
        loanDesc: string;
        securityId: number;
        issuer: string;
        maturityDate: string;
       
        searchText: string;
        checked:boolean;
    }

    export interface ILoanAttributeDto  {
        field: Models.IField;
        securityOverrideId: number;
        securityId: number;
        overrideValue: string;
        effectiveFrom: string;
        effectiveTo: string;
        existingValue:string;
    }

    export interface IVwSecurityDto {
        loanAttributeOverrides : Array<Models.ILoanAttributeDto>;
        securityCode: string;
        securityId:number;
        securityDesc: string;
        securityName:string;
        bbgId: string;
        issuer: string;
        facility: string;
        callDate: string;
        maturityDate: string;
        snpIndustry: string;
        moodyIndustry: string;
        isFloating: boolean;
        lienType: string;
        issuerId: number;
        watchId: number;
		isOnWatch: boolean;
		isSellCandidate: boolean;
        watchObjectTypeId: number;
        watchObjectId: number;
        watchComments: string;
        watchLastUpdatedOn: Date;
        watchUser: string;
        origSecurityCode: string;
        origSecurityDesc: string;
        origBBGId: string;
        origIssuer: string;
        origFacility: string;
        origCallDate: string;
        origMaturityDate: string;
        origSnpIndustry: string;
        origMoodyIndustry: string;
        origIsFloating: string;
        origLienType: string;
        hasPositions:boolean;
}

    export interface IMessage {
        header: string;
        body:string;
    }

    export interface IFund {
        fundId :number;
        fundCode: string; 
        isNotSelected: boolean;
		displayText: string;
		targetPar: number;
        assetParPercentageThreshold: number;
        isImsSelected: boolean;
        isWareHouse: boolean;
        canFilter: boolean;
    }

    export interface ITradeAllocation {
        tradeAllocationId :number;
        tradeId :number;
        currentAllocation :number;
        newAllocation: number;
        finalAllocation: number;
        fundId :number;
        fund: IFund;
        bid: string;
        offer:string;
    }

    export interface ITrade {
        tradeId : number;
        securityId :number;
        dateId: number;
        tradeAmount:number;
        tradePrice : number;
        keepOnBlotter: boolean;
        sellAll: boolean;
        bidOfferPrice :number;
        comments: string;
        isBuy: boolean;
        isSell:boolean;
        security: ISecurity;
        issuerId: number;
        isCancelled: boolean;
        totalCurrentAllocation: number;
        tradeAllocations: Array<ITradeAllocation>;
        tradeType: number;

        isOnWatch: boolean;
        watchId:number;
		watchObjectTypeId: number;
        watchObjectId: number;
        watchComments: string;
        watchLastUpdatedOn: string;
        watchUser: string;
        toolTipText: string;

        forCompare: boolean;
        audit:string;
    }

    export interface ILienType
    {
        lienTypeId: number;
        lienTypeDesc: string;
    }

    export interface IIndustry
    {
        industryId: number;
        industryDesc: string;
    }

    export interface IRating
    {
        ratingId: number;
        ratingDesc: string;
        rank:number;
    }

    export interface ITradeSourceData
    {
        securities : Array<IVwSecurityDto>;
        issuers: Array<IIssuer>;
        facilities: Array<IFacility>;
        lienTypes: Array<ILienType>;
        snpIndustries: Array<IIndustry>;
        moodyIndustries: Array<IIndustry>;
        ratings: Array<IRating>;
        funds:Array<IFund>;
    }

    export interface ITempSecurity {
        moodyFacilityRatingId: number;
        moodyCashFlowRatingId: number;
        snPFacilityRatingId: number;
        snPIssuerRatingId: number;
        moodyIndustryId: number;
        snPIndustryId: number;
        gicsIndustry: string;
        moodyRecovery: number;
        spread: number;
        
        adjustedWARF: number;
        callDate: Date;
        maturityDate: Date;
        lienType: ILienType;
        facility: IFacility;
        issuer: IIssuer;
        //securityId: string;
        securityCode: string;
        bBGId:string;
    }

    export interface ITradeInfo {
        action: string;
        quantity: number;
        price: number;
        audit: string;
        comment: string;
    }

    export interface ITradeSwapParamCriteria  {
        selectAll: boolean;
        cash: boolean;
        moodyAdjCfr: boolean;
        //moodyAdjFacility: boolean;
        recovery: boolean;
        spread: boolean;
        fundId:number;
        excludeZeroTotalExposure: boolean;
    }

    export interface ITradeSwapParamConstraints {
        liquidityScoreOperatorId: number;
        liquidityScore: number;
        maturityDateOperatorId: number;
        maturityDate: Date;
        pctPositionOperatorId: number;
        pctPosition: number;
        recoveryOperatorId: number;
        recovery: number;
        yieldOperatorId: number;
        yield: number;
        moodyAdjCfrRank: number;
        moodyAdjCfrRankOperatorId: number;
        creditScoreOperatorId: number;
        creditScore: number;

        spreadOperatorId: number;
        spread: number;

    }

    export interface ITradeSwapParam  {
        criteria: ITradeSwapParamCriteria;
        constraints:ITradeSwapParamConstraints;
    }

    export interface ITradeSwapDefinition{
        tradeSwapId: number;
        status: number;
        createdBy: string;
        error:string;
        createdOn:Date;
    }

    export interface ITradeSwap {
        tradeSwap: ITradeSwapDefinition;
        tradeSwapParam: ITradeSwapParam;
    }

    export interface ITradeSwapSnapshot {
        tradeSwapSnapshotId: number;
        tradeSwapId: number;
        sellSecurityId: number;
        sellFundId: number;
        sellExposure: number;
        sellTotalExposure: number;
        sellSecurityBidPrice: number;
        sellPctPosition: number;
        sellSpread: number;
        sellLiquidityScore: number;
        sellMaturityDate: Date;
        sellIssuer: string;
        sellFacility: string;
        sellMoodyAdjCFR: string;
        sellMoodyAdjFacility:string;

        buySecurityId: number;
        buySecurityOfferPrice: number;
        buyFundId: number;
        buyExposure: number;
        buyTotalExposure: number;
        buyPctPosition: number;
        buySpread: number;
        buyLiquidityScore: number;
        buyMaturityDate: Date;
        buyIssuer: string;
        buyFacility: string;
        buySecurityCode: string;
        buyMoodyAdjCFR: string;
        buyMoodyAdjFacility: string;
        sellSecurityCode: string;
        createdOn: Date;

        sellRecovery: number;
        buyRecovery: number;
        sellYield: number;
        buyYield: number;

        buySecurityBidPrice: number;
        sellSecurityOfferPrice: number;
        buySecurityCreditScore: number;
        sellSecurityCreditScore: number;

    }

    export interface IGroupedTradeSwapSnapshot {
        parent: Models.ITradeSwapSnapshot;
        children: Array<Models.ITradeSwapSnapshot>;
        groupBy:number;
    }

    export interface ITradeSwapSnapshotUi {
        securityInfo: string;
        $$treeLevel: number;
        buySell: string;

        totalPar: number;
        par: number;
        pctPosition: number;
        execPrice: number;
        spread: number;
        liquidityScore: number;
        creditScore: number;
        maturityDate: Date;

        totalParFormatted: string;
        parFormatted: string;
        pctPositionFormatted: string;
        execPriceFormatted: string;
        spreadFormatted: string;
        liquidityScoreFormatted: string;
        creditScoreFormatted: string;
        maturityDateFormatted: string;
        moodyAdjCFR: string;
        moodyAdjFacility:string;
        recovery: number;
        yield:number;
        recoveryFormatted: number;
        yieldFormatted: number;


        searchText:string;
    }

    export interface IFilterValue {
        operator: Models.IOperator;
        value: any;
        date: any;
    }

    export interface IFilter {
        field: Models.IField;
        lowerBound: IFilterValue;
		upperBound: IFilterValue;
        average: number;
        sourceCollection: Array<any>;
        isDisabled: boolean;
    }

    export interface IPositionFilters {
	    fixedfields: Array<IFilter>;
        security: Array<IFilter>;
        analyst: Array<IFilter>;
        ratings: Array<IFilter>;
        funds: Array<IFund>;
        operators: Array<IOperator>;
        ratingsDictionary: any;
		loanCount: number;
		borrowerCount: number;
        count:number;
    }

    export interface IMatrixData {
        Id: number;
        FundCode: string;
        Spread: number;
        Diversity: number;
        Warf: number;
        WarfModifier: number;
        DataPoint: string;
        DataPointType: number;
        Interpolation: string;
        FundId: number;
        IsMatrixPoint: boolean;
        TopMajorSpread: number;
        BottomMajorSpread: number;
        LeftMajorDiversity: number;
        RightMajorDiversity: number;

        TopSpread: number;
        BottomSpread: number;
        LeftDiversity: number;
        RightDiversity: number;
    }

    export interface IMatrixGroup {
        isMinor: boolean;
        showMinors: boolean;
        title: string;
        minorsLoaded: boolean;
        data: number;
    }


    export interface IMatrix {
        colGroups: Array<IMatrixGroup>;
        data: Array<Array<IMatrixData>>;
        rowGroups: Array<IMatrixGroup>;
        rowDictionary: any;
        colDictionary: any;
    }


    export interface IMatrixPoint {
        Id: number;
        FundId: number;
        Spread: number;
        Diversity: number;
        Warf: number;
        showDetails: boolean;
        WarfModifier: number;
        CreatedBy: number;
        CreatedOn: string;
    }


    export interface AssetClass {
        assetClassId: number;
        assetClassCode: string;
    }

    export interface FundAssetClass {
        id: number;
        fundId: number;
        assetClassId: number;
        notional: number;
        spread: number;
        libor: number;
        startDate: Date;
        endDate: Date;
        moodyRatingId: number;
    }


    export interface EquityOverride {
        id: number;
        fundId: number;
        securityCode: string;
        isDeleted: boolean;
    }

    export interface DefaultSecurity {
        id: number;
        fundId: number;
    }

    export interface ReportingData {
        funds: IFund[];
        assetClasses: AssetClass[];
        fundAssetClasses: FundAssetClass[];
        ratings: IRating[];
        equityOverrides: EquityOverride[];
        defaultSecurities: DefaultSecurity[];

    }

    export interface ITradeHistory {
        tradeDate: string;
        tradeType: string;
        quantity: string;
        price: string;
        counterparty: string;
    }



}
    
    
    