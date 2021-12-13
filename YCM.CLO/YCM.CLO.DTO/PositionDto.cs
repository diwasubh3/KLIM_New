using System.Collections.Generic;

namespace YCM.CLO.DTO
{
	public class PositionDto:IProcessortDto,ITradeProcessorDto
    {
	    private decimal? _clo1NumExposure;
	    private decimal? _clo2NumExposure;
	    private decimal? _clo3NumExposure;
	    private decimal? _clo4NumExposure;
	    private decimal? _clo5NumExposure;
	    private decimal? _clo6NumExposure;
	    private decimal? _trsNumExposure;
	    private decimal? _wh1NumExposure;
	    private decimal? _clo7NumExposure;
        private decimal? _clo8NumExposure;
        private decimal? _clo9NumExposure;
        private decimal? _clo10NumExposure;
        private decimal? _clo11NumExposure;
        private decimal? _clo8UpNumExposure;

        #region vw_position 
        public long? PositionId { get; set; }
        public int? PositionDateId { get; set; }
        public string Exposure { get; set; }
        public decimal NumExposure { get; set; }
        public string PctExposure { get; set; }
        public decimal? PxPrice { get; set; }
        public decimal? PctExposureNum { get; set; }
        public string FundCode { get; set; }
        public int SecurityId { get; set; }
        public string SecurityCode { get; set; }
        public string SecurityDesc { get; set; }
        public string BBGId { get; set; }
        public string Issuer { get; set; }
        public string IssuerDesc { get; set; }
        public int IssuerId { get; set; }
        public string Facility { get; set; }
        public string CallDate { get; set; }
        public string CountryDesc { get; set; }
        public string MaturityDate { get; set; }
        public string SnpIndustry { get; set; }
        public string MoodyIndustry { get; set; }
        public string IsCovLite { get; set; }
        public string IsFloating { get; set; }
        public string LienType { get; set; }
        
        public bool? IsOnWatch { get; set; }
        public short? WatchObjectTypeId { get; set; }
        public int? WatchObjectId { get; set; }
        public int? WatchId { get; set; }
        public string WatchComments { get; set; }
        public string WatchLastUpdatedOn { get; set; }
        public string WatchUser { get; set; }
        public string OrigSecurityCode { get; set; }
        public string OrigSecurityDesc { get; set; }
        public string OrigBBGId { get; set; }
        public string OrigIssuer { get; set; }
        public string OrigFacility { get; set; }
        public string OrigCallDate { get; set; }
        public string OrigMaturityDate { get; set; }
        public string OrigSnpIndustry { get; set; }
        public string OrigMoodyIndustry { get; set; }
        public string OrigIsFloating { get; set; }
        public string OrigLienType { get; set; }
        public int? MarketDateId { get; set; }
        public long? MarketDataId { get; set; }
        public int? OverrideMarketDataId { get; set; }
        public string Bid { get; set; }
        public string Offer { get; set; }
        public string PrevDayBid { get; set; }
        public string PrevDayOffer { get; set; }
        public string PctBidDiff { get; set; }
        public string CostPrice { get; set; }
        public decimal? PriceMove { get; set; }
        public decimal? Spread { get; set; }
        public decimal? LiborFloor { get; set; }
        public string MoodyCashFlowRating { get; set; }
        public string MoodyCashFlowRatingAdjusted { get; set; }
        public string MoodyFacilityRating { get; set; }
        public string MoodyFacilityRatingAdjusted { get; set; }
        public decimal? MoodyRecovery { get; set; }
        public string SnPIssuerRating { get; set; }
        public string SnPIssuerRatingAdjusted { get; set; }
        public string SnPFacilityRating { get; set; }
        public string SnPfacilityRatingAdjusted { get; set; }
        public string SnPRecoveryRating { get; set; }
        public string MoodyOutlook { get; set; }
        public string MoodyWatch { get; set; }
        public string SnPWatch { get; set; }
        public string NextReportingDate { get; set; }
        public string FiscalYearEndDate { get; set; }
        public string AgentBank { get; set; }
        public int? CalculationId { get; set; }
        public decimal? YieldBid { get; set; }
        public decimal? YieldOffer { get; set; }
        public decimal? YieldMid { get; set; }
        public decimal? CappedYieldBid { get; set; }
        public decimal? CappedYieldOffer { get; set; }
        public decimal? CappedYieldMid { get; set; }
        public decimal? TargetYieldBid { get; set; }
        public decimal? TargetYieldOffer { get; set; }
        public decimal? TargetYieldMid { get; set; }
        public decimal? BetterWorseBid { get; set; }
        public decimal? BetterWorseOffer { get; set; }
        public decimal? BetterWorseMid { get; set; }
        public decimal? TotalCoupon { get; set; }
        public decimal? WARF { get; set; }
        public decimal? WARFRecovery { get; set; }
        public decimal? Life { get; set; }
        public int? CalculationDateId { get; set; }
        public string TotalPar { get; set; }
        public decimal? TotalParNum { get; set; }
        public int? AnalystResearchId { get; set; }
        public string CLOAnalyst { get; set; }
        public string HFAnalyst { get; set; }
        public string AsOfDate { get; set; }
        public decimal? CreditScore { get; set; }
        public decimal? OneLLeverage { get; set; }
        public string TotalLeverage { get; set; }
        public string EVMultiple { get; set; }
        public string LTMRevenues { get; set; }
        public string LTMEBITDA { get; set; }
        public string FCF { get; set; }
        public string Comments { get; set; }
        public string BusinessDescription { get; set; }
        public bool? IsOnAlert { get; set; }
        public int? SearchText { get; set; }
        public long? POSROWNUM { get; set; }
        public int? MaturityDays { get; set; }
	    public bool IsSellCandidate => SellCandidateId.GetValueOrDefault() > 0;
	    public short? SellCandidateObjectTypeId { get; set; }
	    public int? SellCandidateObjectId { get; set; }
	    public int? SellCandidateId { get; set; }
	    public string SellCandidateComments { get; set; }
	    public string SellCandidateLastUpdatedOn { get; set; }
	    public string SellCandidateUser { get; set; }
	    public string Sponsor { get; set; }
	    public decimal? CLO1NumExposure
	    {
		    get {return _clo1NumExposure.GetValueOrDefault() > 0 ? _clo1NumExposure.Value : 0; }
			set { _clo1NumExposure = value; }
	    }

	    public decimal? CLO2NumExposure
	    {
		    get { return _clo2NumExposure.GetValueOrDefault() > 0 ? _clo2NumExposure.Value : 0; }
		    set { _clo2NumExposure = value; }
	    }

		public decimal? CLO3NumExposure
	    {
			get { return _clo3NumExposure.GetValueOrDefault() > 0 ? _clo3NumExposure.Value : 0; }
			set { _clo3NumExposure = value; }
	    }

		public decimal? CLO4NumExposure
	    {
			get { return _clo4NumExposure.GetValueOrDefault() > 0 ? _clo4NumExposure.Value : 0; }
			set { _clo4NumExposure = value; }
	    }

		public decimal? CLO5NumExposure
	    {
			get { return _clo5NumExposure.GetValueOrDefault() > 0 ? _clo5NumExposure.Value : 0; }
			set { _clo5NumExposure = value; }
	    }

	    public decimal? CLO6NumExposure
	    {
		    get { return _clo6NumExposure.GetValueOrDefault() > 0 ? _clo6NumExposure.Value : 0; }
		    set { _clo6NumExposure = value; }
	    }

	    public decimal? CLO7NumExposure
	    {
		    get => _clo7NumExposure.GetValueOrDefault() > 0 ? _clo7NumExposure.Value : 0;
			set => _clo7NumExposure = value;
	    }

        public decimal? CLO8NumExposure
        {
            get => _clo8NumExposure.GetValueOrDefault() > 0 ? _clo8NumExposure.Value : 0;
            set => _clo8NumExposure = value;
        }

        public decimal? CLO9NumExposure
        {
            get => _clo9NumExposure.GetValueOrDefault() > 0 ? _clo9NumExposure.Value : 0;
            set => _clo9NumExposure = value;
        }

        public decimal? CLO10NumExposure
        {
            get => _clo10NumExposure.GetValueOrDefault() > 0 ? _clo10NumExposure.Value : 0;
            set => _clo10NumExposure = value;
        }

        public decimal? CLO11NumExposure
        {
            get => _clo11NumExposure.GetValueOrDefault() > 0 ? _clo11NumExposure.Value : 0;
            set => _clo11NumExposure = value;
        }
        public decimal? CLO8UpNumExposure
        {
            get => _clo8UpNumExposure.GetValueOrDefault() > 0 ? _clo8UpNumExposure.Value : 0;
            set => _clo8UpNumExposure = value;
        }

        public decimal? TRSNumExposure
	    {
			get { return _trsNumExposure.GetValueOrDefault() > 0 ? _trsNumExposure.Value : 0; }
			set { _trsNumExposure = value; }
	    }

	    public decimal? WH1NumExposure
	    {
		    get { return _wh1NumExposure.GetValueOrDefault() > 0 ? _wh1NumExposure.Value : 0; }
		    set { _wh1NumExposure = value; }
	    }

		public decimal? CLO1TargetPar { get; set; }
	    public decimal? CLO2TargetPar { get; set; }
	    public decimal? CLO3TargetPar { get; set; }
	    public decimal? CLO4TargetPar { get; set; }
	    public decimal? CLO5TargetPar { get; set; }
	    public decimal? CLO6TargetPar { get; set; }
	    public decimal? CLO7TargetPar { get; set; }
        public decimal? CLO8TargetPar { get; set; }
        public decimal? CLO9TargetPar { get; set; }
        public decimal? CLO10TargetPar { get; set; }
        public decimal? CLO11TargetPar { get; set; }
        public decimal? CLO8UpTargetPar { get; set; }
        public decimal? TRSTargetPar { get; set; }
	    public decimal? WH1TargetPar { get; set; }

        public decimal? CLO1NumMatrixImpliedSpread { get; set; }
        public decimal? CLO2NumMatrixImpliedSpread { get; set; }
        public decimal? CLO3NumMatrixImpliedSpread { get; set; }
        public decimal? CLO4NumMatrixImpliedSpread { get; set; }
        public decimal? CLO5NumMatrixImpliedSpread { get; set; }
        public decimal? CLO6NumMatrixImpliedSpread { get; set; }
        public decimal? CLO7NumMatrixImpliedSpread { get; set; }
        public decimal? CLO8NumMatrixImpliedSpread { get; set; }
        public decimal? CLO9NumMatrixImpliedSpread { get; set; }
        public decimal? CLO10NumMatrixImpliedSpread { get; set; }
        public decimal? CLO11NumMatrixImpliedSpread { get; set; }
        public decimal? CLO8UpNumMatrixImpliedSpread { get; set; }

        public decimal? CLO1NumDifferentialImpliedSpread { get; set; }
        public decimal? CLO2NumDifferentialImpliedSpread { get; set; }
        public decimal? CLO3NumDifferentialImpliedSpread { get; set; }
        public decimal? CLO4NumDifferentialImpliedSpread { get; set; }
        public decimal? CLO5NumDifferentialImpliedSpread { get; set; }
        public decimal? CLO6NumDifferentialImpliedSpread { get; set; }
        public decimal? CLO7NumDifferentialImpliedSpread { get; set; }
        public decimal? CLO8NumDifferentialImpliedSpread { get; set; }
        public decimal? CLO9NumDifferentialImpliedSpread { get; set; }
        public decimal? CLO10NumDifferentialImpliedSpread { get; set; }
        public decimal? CLO11NumDifferentialImpliedSpread { get; set; }
        public decimal? CLO8UpNumDifferentialImpliedSpread { get; set; }

        public string CLO1Exposure => $"{CLO1NumExposure:N2}";
        public string CLO2Exposure => $"{CLO2NumExposure:N2}";
		public string CLO3Exposure => $"{CLO3NumExposure:N2}";
		public string CLO4Exposure => $"{CLO4NumExposure:N2}";
		public string CLO5Exposure => $"{CLO5NumExposure:N2}";
	    public string CLO6Exposure => $"{CLO6NumExposure:N2}";
	    public string CLO7Exposure => $"{CLO7NumExposure:N2}";
        public string CLO8Exposure => $"{CLO8NumExposure:N2}";
        public string CLO9Exposure => $"{CLO9NumExposure:N2}";
        public string CLO10Exposure => $"{CLO10NumExposure:N2}";
        public string CLO11Exposure => $"{CLO11NumExposure:N2}";
        public string CLO8UpExposure => $"{CLO8UpNumExposure:N2}";
        public string TRSExposure => $"{TRSNumExposure:N2}";
	    public string WH1Exposure => $"{WH1NumExposure:N2}";


        public decimal? CLO1MatrixImpliedSpread { get; set; }
        public decimal? CLO2MatrixImpliedSpread { get; set; }
        public decimal? CLO3MatrixImpliedSpread { get; set; }
        public decimal? CLO4MatrixImpliedSpread { get; set; }
        public decimal? CLO5MatrixImpliedSpread { get; set; }
        public decimal? CLO6MatrixImpliedSpread { get; set; }
        public decimal? CLO7MatrixImpliedSpread { get; set; }
        public decimal? CLO8MatrixImpliedSpread { get; set; }
        public decimal? CLO9MatrixImpliedSpread { get; set; }
        public decimal? CLO10MatrixImpliedSpread { get; set; }
        public decimal? CLO11MatrixImpliedSpread { get; set; }
        public decimal? CLO8UpMatrixImpliedSpread { get; set; }

        public decimal? CLO1DifferentialImpliedSpread { get; set; }
        public decimal? CLO2DifferentialImpliedSpread { get; set; }
        public decimal? CLO3DifferentialImpliedSpread { get; set; }
        public decimal? CLO4DifferentialImpliedSpread { get; set; }
        public decimal? CLO5DifferentialImpliedSpread { get; set; }
        public decimal? CLO6DifferentialImpliedSpread { get; set; }
        public decimal? CLO7DifferentialImpliedSpread { get; set; }
        public decimal? CLO8DifferentialImpliedSpread { get; set; }
        public decimal? CLO9DifferentialImpliedSpread { get; set; }
        public decimal? CLO10DifferentialImpliedSpread { get; set; }
        public decimal? CLO11DifferentialImpliedSpread { get; set; }
        public decimal? CLO8UpDifferentialImpliedSpread { get; set; }

        public decimal? CLO1MatrixWarfRecovery { get; set; }
        public decimal? CLO2MatrixWarfRecovery { get; set; }
        public decimal? CLO3MatrixWarfRecovery { get; set; }
        public decimal? CLO4MatrixWarfRecovery { get; set; }
        public decimal? CLO5MatrixWarfRecovery { get; set; }
        public decimal? CLO6MatrixWarfRecovery { get; set; }
        public decimal? CLO7MatrixWarfRecovery { get; set; }
        public decimal? CLO8MatrixWarfRecovery { get; set; }
        public decimal? CLO9MatrixWarfRecovery { get; set; }
        public decimal? CLO10MatrixWarfRecovery { get; set; }
        public decimal? CLO11MatrixWarfRecovery { get; set; }
        public decimal? CLO8UpMatrixWarfRecovery { get; set; }

        public string CLO1PctExposure => $"{CLO1NumExposure.GetValueOrDefault() / CLO1TargetPar.ToDecimalOrOne():P}";
        public string CLO2PctExposure => $"{CLO2NumExposure.GetValueOrDefault() / CLO2TargetPar.ToDecimalOrOne():P}";
		public string CLO3PctExposure => $"{CLO3NumExposure.GetValueOrDefault() / CLO3TargetPar.ToDecimalOrOne():P}";
		public string CLO4PctExposure => $"{CLO4NumExposure.GetValueOrDefault() / CLO4TargetPar.ToDecimalOrOne():P}";
		public string CLO5PctExposure => $"{CLO5NumExposure.GetValueOrDefault() / CLO5TargetPar.ToDecimalOrOne():P}";
	    public string CLO6PctExposure => $"{CLO6NumExposure.GetValueOrDefault() / CLO6TargetPar.ToDecimalOrOne():P}";
	    public string CLO7PctExposure => $"{CLO7NumExposure.GetValueOrDefault() / CLO7TargetPar.ToDecimalOrOne():P}";
        public string CLO8PctExposure => $"{CLO8NumExposure.GetValueOrDefault() / CLO8TargetPar.ToDecimalOrOne():P}";
        public string CLO9PctExposure => $"{CLO9NumExposure.GetValueOrDefault() / CLO9TargetPar.ToDecimalOrOne():P}";
        public string CLO10PctExposure => $"{CLO10NumExposure.GetValueOrDefault() / CLO10TargetPar.ToDecimalOrOne():P}";
        public string CLO11PctExposure => $"{CLO11NumExposure.GetValueOrDefault() / CLO11TargetPar.ToDecimalOrOne():P}";
        public string CLO8UpPctExposure => $"{CLO8UpNumExposure.GetValueOrDefault() / CLO8UpTargetPar.ToDecimalOrOne():P}";
        public string TRSPctExposure => $"{TRSNumExposure.GetValueOrDefault() / TRSTargetPar.ToDecimalOrOne():P}";
	    public string WH1PctExposure => $"{WH1NumExposure.GetValueOrDefault() / WH1TargetPar.ToDecimalOrOne():P}";

		public string SeniorLeverage { get; set; }
	    public string EnterpriseValue { get; set; }
	    public string LTMFCF { get; set; }
        #endregion vw_position 

		//public string SecurityName { get; set; }

		public decimal OfferYield { get; set; }
        public decimal BidYield { get; set; }

        public bool? HasBuyTrade { get; set; }
        public bool? HasSellTrade { get; set; }

        public PositionDto()
        {
            Alerts = new List<AlertDto>();
            Trades = new List<TradeInfoDto>();
        }

        public IList<AlertDto> Alerts { get; set; }

        public IList<TradeInfoDto> Trades { get; set; }
	    public string ScoreDescription { get; set; }
	    public decimal? GlobalAmount { get; set; }
		public bool IsPrivate { get; set; }
	    public string GlobalAmountString => GlobalAmount.HasValue ? $"{GlobalAmount.Value:N0}"
	    : "N/A";

        public string zSnPAssetRecoveryRating { get; set; }
        public decimal? SnpWarf { get; set; }
        public string SnpWarfPct => $"{SnpWarf:P}";
        public decimal? SnpLgd { get; set; }
        public string SnpLgdPct => $"{SnpLgd:P}";
        public decimal? MoodysLgd { get; set; }
        public string MoodysLgdPct => $"{MoodysLgd:P}";
        public decimal? YieldAvgLgd { get; set; }
        public string YieldAvgLgdPct => $"{YieldAvgLgd:P}";
        public decimal? SnpAAARecovery { get; set; }
        public string SnpCreditWatch { get; set; }

        public string LiborBaseRate { get; set; }

        public string LiborCategory { get; set; }

        public string LiborTransitionNote { get; set; }

        public override string ToString()
		    => $"{SecurityCode} {SecurityDesc} Sell Candidate: {IsSellCandidate} Id: {SellCandidateId} Watch: {IsOnWatch} Id: {WatchId} {SecurityDesc} {Issuer} {BBGId} {SecurityId} {FundCode} - {TRSNumExposure} {TRSExposure} {TRSPctExposure}";

        private decimal? _clo9wapp;
        private decimal? _clo10wapp;
        public decimal? CLO9WAPP
        {
            get { return _clo9wapp.GetValueOrDefault() > 0 ? _clo9wapp.Value : 0; }
            set { _clo9wapp = value; }
        }
        public decimal? CLO10WAPP
        {
            get { return _clo10wapp.GetValueOrDefault() > 0 ? _clo10wapp.Value : 0; }
            set { _clo10wapp = value; }
        }
    }
}
