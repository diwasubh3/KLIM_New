using System;

namespace YCM.CLO.DataAccess.Models
{
	public partial class vw_Security
    {
        public int SecurityId { get; set; }
        public string SecurityCode { get; set; }
        public string SecurityDesc { get; set; }
        public string BBGId { get; set; }
        public string Issuer { get; set; }
        public string IssuerDesc { get; set; }
        public string Facility { get; set; }
        public DateTime? CallDate { get; set; }
        public DateTime? MaturityDate { get; set; }
        public string SnpIndustry { get; set; }
        public string MoodyIndustry { get; set; }
        public bool IsFloating { get; set; }
        public string LienType { get; set; }
        public int IssuerId { get; set; }
        public int? WatchId { get; set; }

        public bool? IsOnWatch { get; set; }
        public short? WatchObjectTypeId { get; set; }
        public int? WatchObjectId { get; set; }
        public string WatchComments { get; set; }
        public string WatchLastUpdatedOn { get; set; }
        public string WatchUser { get; set; }
        public string OrigSecurityCode { get; set; }
        public string OrigSecurityDesc { get; set; }
        public string OrigBBGId { get; set; }
        public string OrigIssuer { get; set; }
        public string GICSIndustry { get; set; }
        public string OrigFacility { get; set; }
        public string OrigCallDate { get; set; }
        public string OrigMaturityDate { get; set; }
        public string OrigSnpIndustry { get; set; }
        public string OrigMoodyIndustry { get; set; }
        public string OrigIsFloating { get; set; }
        public string OrigLienType { get; set; }
        public string OrigMoodyFacilityRatingAdjusted { get; set; }
        public string OrigMoodyCashFlowRatingAdjusted { get; set; }
        public string MoodyFacilityRatingAdjusted { get; set; }
        public string MoodyCashFlowRatingAdjusted { get; set; }
        public DateTime? SecurityLastUpdatedOn { get; set; }
        public string SecurityLastUpdatedBy { get; set; }
        public short? SourceId { get; set; }
        public bool? HasPositions { get; set; }
    }
}
