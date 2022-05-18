using System;
using System.Collections.Generic;
using System.Linq;
using YCM.CLO.DataAccess.Models;

namespace YCM.CLO.DataAccess.Contracts
{
	public interface IRepository : IDisposable
	{
		bool UserIsAnAdmin(int userId);
	    int GetPrevDayDateId();
	    int GetPrevToPrevDayDateId();
	    bool ViewNameIsTaken(int userId, string viewName);
		List<CacheSetting> GetCacheSettings();
		List<UserDefaultCustomView> GetUserDefaultCustomViews();
	    List<CustomView> DeleteCustomView(CustomView view);
	    List<CustomView> AddOrUpdateCustomView(CustomView view, string userName);
	    List<CustomView> GetCustomViews();
	    List<CustomViewField> GetCustomViewFields();
	    List<ApplicationRole> GetApplicationRoles();
	    List<UserApplicationRole> GetUserApplicationRoles();
	    List<Field> GetFieldsForCustomView(int viewId);
		List<CreditScore> GetCreditScores();
	    List<AnalystResearchDetail> GetAnalystResearchDetails(int headerId);
	    AnalystResearchHeader GetAnalystResearchHeader(int issuerId);
		IEnumerable<LoanPosition> GetLoanPositions(int dateId, int priorDateId, int fundId);
	    IEnumerable<AssetExposure> GetAssetPars(int dateId);
        IEnumerable<vw_CLOSummary> GetSummaries(int dateId);

        IEnumerable<Rule> GetRules();

        IEnumerable<vw_ReinvestDetails> GetReinvestDetails();

        Rule GetRule(short ruleId);

        IEnumerable<vw_Position> GetTop10Positions(string fundCode, int dateId);

        IEnumerable<vw_Position> GetBottom10Positions(string fundCode, int dateId);

        IEnumerable<vw_Position> GetPositions(string fundCode, bool? onlyWithExposures);

        IEnumerable<vw_Position> GetPositions(string securityCode, string fundCode);

        IEnumerable<vw_Position> GetPositions(int securityId);

        IEnumerable<vw_Position> GetPositions(int[] securityIds, string fundCode);

        IQueryable<FieldGroup> GetFieldGroups();

        IEnumerable<Field> GetFields(string fieldGroupName);

        IEnumerable<Operator> GetOperators();

        IEnumerable<FundRestrictionType> GetFundRestrictionTypes();

        IEnumerable<FundRestriction> GetFundRestrictions(int? fundId);

        bool SaveFundRestrictions(IEnumerable<FundRestriction> fundRestrictions);

        IEnumerable<vw_Position> AddOrUpdateWatch(Watch watch, int dateId);

        ParameterValue AddOrUpdateParameterValue(ParameterValue parameterValue);

        IEnumerable<vw_Position> DeleteWatch(int watchId, int dateId);

        IEnumerable<ParameterValue> GetParameterValues(string parameterTypeName);

        IEnumerable<ParameterValue> GetParameterValues();

        IEnumerable<ParameterType> GetParameterTypes();

        IEnumerable<AlertProcessor> GetAlertProcessors();

        RuleResult ExecuteRule(Rule rule, string fundCode, int dateId);

        IEnumerable<int> GetCreditScoreAlertIssuers(string fundCode, int dateId);

        IEnumerable<SecurityOverride> GetSecurityOverrides(LoadTypeEnum? loadTypeEnum, int? securityId);

        IEnumerable<SecurityOverride> GetConflictingSecurityOverrides(int? securityId);

        IEnumerable<Field> GetFields(string[] fieldNames);

        IEnumerable<Field> GetSecurityOverrideableFields();

        bool SaveSecurityOverrides(SecurityOverride[] securityOverrides, string user);

        IEnumerable<vw_Security> GetSecurities();

        IEnumerable<vw_SecurityMarketCalculation> GetCurrentSecurities();

        IEnumerable<vw_Security> GetSecurities(int[] securityIds);

        IEnumerable<Security> GetSecurities(string[] securities);

        IEnumerable<AnalystResearch> GetAnalystResearches();

		IEnumerable<AnalystResearchHeader> GetAnalystResearchHeaders();
		IEnumerable<AnalystResearchDetail> GetAnalystResearchDetails();
		IEnumerable<AnalystResearchRowLocation> GetAnalystResearchRowLocations();
		IEnumerable<AnalystResearchFile> GetAnalystResearchFiles();
		IEnumerable<vw_YorkCoreGenevaAnalyst> GetAnalysts();

		IQueryable<vw_AnalystResearch> GetActiveAnalystResearches();

		DatabaseEntityOperationResult SaveAnalystResearchHeader(AnalystResearchHeader existing
			, AnalystResearchHeader header);
		DatabaseEntityOperationResult SaveAnalystResearchDetail(AnalystResearchDetail detail);
		DatabaseEntityOperationResult SaveAnalystResearchDetails(List<AnalystResearchDetail> existingDetails
			, List<AnalystResearchDetail> details);
		List<DatabaseEntityOperationResult> SaveEntities<T>(List<T> entities) where T : Entity;
	    DatabaseEntityOperationResult SaveEntity<T>(T entity) where T : Entity;

		bool SaveAnalystResearches(AnalystResearch[] analystResearches);

        IQueryable<Issuer> GetIssuers();

        IEnumerable<User> GetUsers();

        bool SavePrices(Pricing[] prices);

        User GetPerson(string userId);

        bool ClearExistingPrices(int dateId, int[] securities);

        IEnumerable<Security> GetSecuritiesForRecon();

        bool DeleteSecurity(int securityId, string user);

        bool TransferSecurities(int sourceSecurityId, int destSecurityId, string user);

        bool EndSecurityOverride(int securityOverrideId, DateTime endDate, string user);

        bool ResetSecurityOverrideConflict(int securityOverrideId, int securityId, string user);

        Trade SaveTrade(Trade trade, string user);

        IEnumerable<Trade> GetTrades(bool includeCancelled, int dateId);

        IEnumerable<Facility> GetFacilities();
        IEnumerable<AssetType> GetAssetTypes();

        IEnumerable<LienType> GetLienTypes();

        IEnumerable<Industry> GetSnPIndustries();

        IEnumerable<Industry> GetMoodyIndustries();

        IEnumerable<Rating> GetRatings();

        Issuer AddIfMissingIssuer(string issuer,string user);

        Facility AddIfMissingFacility(string facility, string user);

        LienType GetLienType(string lienType);

        IEnumerable<Fund> GetFunds();
        IEnumerable<TotalParChange> GetTotalParChange(int startDateId, int endDateId);
        IEnumerable<RatingChange> GetRatingChanges(int startDateId, int endDateId);

        Security AddUpdateSecurity(Security security,string user);
	    Security UpdateBbgId(int securityId, string bbgId, string user);
		Issuer UpdateIsPrivate(int issuerId, bool isPrivate, string user);

		IEnumerable<string> GetSecurityCodesForBidOfferDownload();

        Fund SaveFund(Fund fund);

        TradeSwapParam SaveTradeSwap(TradeSwapParam tradeSwapParam, TradeSwap tradeSwap);

        TradeSwap GetLastTradeSwap();

        TradeSwapParam GetTradeSwapParam(TradeSwap tradeSwap);

        IEnumerable<vw_TradeSwap> GetTradeSwaps(int fundId, int tradeSwapId);

        List<vw_AggregatePosition> GetAllPositions(bool? onlyWithExposures);

        IEnumerable<vw_AggregatePosition> GetAllPositions(string securityCode);

        IEnumerable<vw_AggregatePosition> GetAllPositions(int securityId);

        IEnumerable<vw_AggregatePosition> GetAllPositions(int[] securityIds);

        IEnumerable<vw_Position_Exposure> GetPositionExposures(int securityId);

        IEnumerable<vw_Security_Watch> GetSecurityWatch(int[] securityIds);

        IEnumerable<vw_PriceMove> GetPriceMove(string section, int fromDateId, int toDateId);

        bool CreateStalePositions(int fundId);

        IEnumerable<Rating> GetMoodyRatings();

        bool CaptureDailySnapshot(int fundId, int dateId);

        bool CleanPositionsBasedOnPrincipalCash(int fundId, int dateId);

        bool RefillPositionsBasedOnPrincipalCash(int fundId, int dateId);

        IEnumerable<MatrixData> GenerateMinorMatrixData(int fundId);

        IEnumerable<vw_MatrixData> GetMajorMatrixDatas(int fundId);

        vw_MatrixData GetMatrixData(int fundId,decimal spread,decimal diversity);

        IEnumerable<vw_MatrixData> GetSpreadInterpolatedMinorMatrixDatas(int fundId,decimal fromSpread, decimal toSpread);

        IEnumerable<vw_MatrixData> GetDiversityInterpolatedMinorMatrixDatas(int fundId, decimal fromDiversity, decimal toDiversity);

        bool AddMatrixPoint(MatrixPoint matrixData,string user);

        IEnumerable<MatrixPoint> GetMatrixPoints(int fundId);

        bool CalculateSummaries();

        bool GenerateAggregatedPositions();

        bool UpdateFundTriggersForMatrixPoint(int fundId);

        IEnumerable<AssetClass> GetAssetClasses();

        IEnumerable<FundAssetClass> GetFundAssetClasses();

        IEnumerable<DefaultSecurity> GetDefaultSecurities(int dateId);

        IEnumerable<EquityOverride> GetEquityOverrides();

        EquityOverride SaveEquityOverride(EquityOverride equityOverride, string user);

        FundAssetClass SaveFundAssetClass(FundAssetClass fundAssetClass, string user);

        IEnumerable<vw_Mismatch> GetMismatchData(int fieldId);

        IEnumerable<string> GetPermission(string UserName);

        IEnumerable<TradeHistory> GetTradeHistory(string securityCode);

        IEnumerable<vw_Position> AddOrUpdatePaydown(Paydown watch, int dateId);

        IEnumerable<vw_Position> DeletePaydown(int paydownId, int dateId);

        IEnumerable<vw_Security_Paydown> GetSecurityPaydown(int[] securityIds);

        IEnumerable<Trader> GetTraders();
        IEnumerable<TradeType> GetTradeType();
        IEnumerable<CounterParty> GetCounterParty();
        IEnumerable<SettleMethods> GetSettleMethods();
        IEnumerable<InterestTreatment> GetInterestTreatment();
        IEnumerable<TradeComment> GetTradeComment();
        IEnumerable<TradeReason> GetTradeReasons();
        IEnumerable<AllocationRule> GetAllocationRule(int tradeTypeId);

        IEnumerable<TradeBooking> GetTradeBookingXML(int Id);
        IEnumerable<TradeGroup> GetTradeGroupXML(int Id);
        IEnumerable<TradeBookingDetail> GetTradeBookingDetailXML(int Id); 
        IEnumerable<vw_IssuerSecurity> SearchIssuerSecurities();
        IEnumerable<vw_PositionIssuers> GetIssuerList();
        IEnumerable<Fund> GetFundAllocation();
        IEnumerable<TradeBooking> GetTradeBookings();
        IEnumerable<TradeBookingDetail> GetTradeFundAllocation(string ruleName, int issuerId, string LoanXId, string tradeType);
        int SaveTradeBooking(TradeBooking tradebook, string user);
        bool SaveTradeBookingDetails(IEnumerable<TradeBookingDetail> tradebookdetail, long TradeId);
        bool UpdateSubmitDetails(long TradeId);
        TradeBooking RefreshTradeBooking(long TradeId);
        IEnumerable<TradeBookingDetail> RefreshTradeBookingDetail(long TradeId);
    }
}