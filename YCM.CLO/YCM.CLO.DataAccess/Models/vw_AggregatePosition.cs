using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace YCM.CLO.DataAccess.Models
{
	public partial class vw_AggregatePosition
    {
        public int? PositionDateId { get; set; }
        public decimal? CLO1NumExposure { get; set; }
        public decimal? CLO2NumExposure { get; set; }
        public decimal? CLO3NumExposure { get; set; }
        public decimal? CLO4NumExposure { get; set; }
	    public decimal? CLO5NumExposure { get; set; }
	    public decimal? CLO6NumExposure { get; set; }
	    public decimal? CLO7NumExposure { get; set; }
        public decimal? CLO8NumExposure { get; set; }
        public decimal? CLO9NumExposure { get; set; }


        [NotMapped]
        public decimal? TRSNumExposure { get; set; }
	    [NotMapped]
	    public decimal? WH1NumExposure { get; set; }

		public decimal? CLO1TargetPar { get; set; }
	    public decimal? CLO2TargetPar { get; set; }
	    public decimal? CLO3TargetPar { get; set; }
	    public decimal? CLO4TargetPar { get; set; }
	    public decimal? CLO5TargetPar { get; set; }
	    public decimal? CLO6TargetPar { get; set; }
	    public decimal? CLO7TargetPar { get; set; }
        public decimal? CLO8TargetPar { get; set; }
        public decimal? CLO9TargetPar { get; set; }
        [NotMapped]
	    public decimal? TRSTargetPar { get; set; }
	    [NotMapped]
	    public decimal? WH1TargetPar { get; set; }

		public string CLO1Exposure { get; set; }
        public string CLO2Exposure { get; set; }
        public string CLO3Exposure { get; set; }
        public string CLO4Exposure { get; set; }
	    public string CLO5Exposure { get; set; }
	    public string CLO6Exposure { get; set; }
	    public string CLO7Exposure { get; set; }
        public string CLO8Exposure { get; set; }
        public string CLO9Exposure { get; set; }
        [NotMapped]
        public string TRSExposure { get; set; }
		[NotMapped]
	    public string WH1Exposure { get; set; }
        public string CLO1PctExposure { get; set; }
        public string CLO2PctExposure { get; set; }
        public string CLO3PctExposure { get; set; }
        public string CLO4PctExposure { get; set; }
	    public string CLO5PctExposure { get; set; }
	    public string CLO6PctExposure { get; set; }
	    public string CLO7PctExposure { get; set; }
        public string CLO8PctExposure { get; set; }
        public string CLO9PctExposure { get; set; }
        [NotMapped]
        public string TRSPctExposure { get; set; }
		[NotMapped]
	    public string WH1PctExposure { get; set; }
        public int SecurityId { get; set; }
        public string SecurityCode { get; set; }
        public string SecurityDesc { get; set; }
        public string BBGId { get; set; }
        public string Issuer { get; set; }
        public string IssuerDesc { get; set; }
        public int? IssuerId { get; set; }
		public bool? IsPrivate { get; set; }
        public string Facility { get; set; }
        public string CallDate { get; set; }
        public string CountryDesc { get; set; }
        public string MaturityDate { get; set; }
        public string SnpIndustry { get; set; }
        public string MoodyIndustry { get; set; }
        public int? IsCovLite { get; set; }
        public string IsFloating { get; set; }
        public string LienType { get; set; }
        public bool? IsOnWatch { get; set; }
        public short? WatchObjectTypeId { get; set; }
        public int? WatchObjectId { get; set; }
        public int? WatchId { get; set; }
        public string WatchComments { get; set; }
        public string WatchLastUpdatedOn { get; set; }
        public string WatchUser { get; set; }
		[NotMapped]
	    public bool IsSellCandidate => SellCandidateId.GetValueOrDefault() > 0;
	    public short? SellCandidateObjectTypeId { get; set; }
	    public int? SellCandidateObjectId { get; set; }
	    public int? SellCandidateId { get; set; }
	    public string SellCandidateComments { get; set; }
	    public string SellCandidateLastUpdatedOn { get; set; }
	    public string SellCandidateUser { get; set; }
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
        public string OrigMoodyFacilityRatingAdjusted { get; set; }
        public string OrigMoodyCashFlowRatingAdjusted { get; set; }
        public string Bid { get; set; }
        public string Offer { get; set; }
        public decimal? BidNum { get; set; }
        public decimal? OfferNum { get; set; }
        public decimal? Spread { get; set; }
        public decimal? LiborFloor { get; set; }
        public string MoodyCashFlowRating { get; set; }
        public string MoodyCashFlowRatingAdjusted { get; set; }
        public string MoodyFacilityRating { get; set; }
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
        public string TotalPar { get; set; }
        public decimal? TotalParNum { get; set; }
        public decimal BODTotalPar { get; set; }
        public string MoodyFacilityRatingAdjusted { get; set; }
        public string CLOAnalyst { get; set; }
        public string HFAnalyst { get; set; }
        public string AsOfDate { get; set; }
        public decimal? CreditScore { get; set; }
        public decimal? LiquidityScore { get; set; }
        public decimal? OneLLeverage { get; set; }
        public string TotalLeverage { get; set; }
        public string EVMultiple { get; set; }
        public string LTMRevenues { get; set; }
        public string LTMEBITDA { get; set; }
        public string FCF { get; set; }
        public string Comments { get; set; }
	    public string Sponsor { get; set; }

		public string BusinessDescription { get; set; }
        public DateTime? SecurityMaturityDate { get; set; }
        public bool? IsOnAlert { get; set; }
        public int? SearchText { get; set; }
        public string CostPrice { get; set; }
        public decimal? CostPriceNum { get; set; }
        public decimal? PrevDayBidNum { get; set; }
        public decimal? PrevDayOfferNum { get; set; }
        public string PrevDayBid { get; set; }
        public string PrevDayOffer { get; set; }
        public decimal? PriceMove { get; set; }

	    public decimal? SeniorLeverage { get; set; }
	    public decimal? EnterpriseValue { get; set; }
	    public decimal? LTMFCF { get; set; }

		public short LienTypeId { get; set; }
		public string ScoreDescription { get; set; }

		public decimal? GlobalAmount { get; set; }

        public decimal? CLO1MatrixImpliedSpread { get; set; }
        public decimal? CLO2MatrixImpliedSpread { get; set; }
        public decimal? CLO3MatrixImpliedSpread { get; set; }
        public decimal? CLO4MatrixImpliedSpread { get; set; }
        public decimal? CLO5MatrixImpliedSpread { get; set; }
        public decimal? CLO6MatrixImpliedSpread { get; set; }
        public decimal? CLO7MatrixImpliedSpread { get; set; }
        public decimal? CLO8MatrixImpliedSpread { get; set; }
        public decimal? CLO9MatrixImpliedSpread { get; set; }

        public decimal? CLO1DifferentialImpliedSpread { get; set; }
        public decimal? CLO2DifferentialImpliedSpread { get; set; }
        public decimal? CLO3DifferentialImpliedSpread { get; set; }
        public decimal? CLO4DifferentialImpliedSpread { get; set; }
        public decimal? CLO5DifferentialImpliedSpread { get; set; }
        public decimal? CLO6DifferentialImpliedSpread { get; set; }
        public decimal? CLO7DifferentialImpliedSpread { get; set; }
        public decimal? CLO8DifferentialImpliedSpread { get; set; }
        public decimal? CLO9DifferentialImpliedSpread { get; set; }

        public decimal? CLO1MatrixWarfRecovery { get; set; }
        public decimal? CLO2MatrixWarfRecovery { get; set; }
        public decimal? CLO3MatrixWarfRecovery { get; set; }
        public decimal? CLO4MatrixWarfRecovery { get; set; }
        public decimal? CLO5MatrixWarfRecovery { get; set; }
        public decimal? CLO6MatrixWarfRecovery { get; set; }
        public decimal? CLO7MatrixWarfRecovery { get; set; }
        public decimal? CLO8MatrixWarfRecovery { get; set; }
        public decimal? CLO9MatrixWarfRecovery { get; set; }

        public string zSnPAssetRecoveryRating { get; set; }
        public decimal? SnpWarf { get; set; }
        public decimal? SnpLgd { get; set; }
        public decimal? MoodysLgd { get; set; }
        public decimal? YieldAvgLgd { get; set; }
        public decimal? SnpAAARecovery { get; set; }
        public string SnpCreditWatch { get; set; }

        public string LiborBaseRate { get; set; }

        public string LiborCategory { get; set; }

        public string LiborTransitionNote { get; set; }

        public override string ToString() => $"{SecurityCode}: Sell Candidate: {IsSellCandidate} Id: {SellCandidateId} Watch: {IsOnWatch} Id: {WatchId}";
    }
}
