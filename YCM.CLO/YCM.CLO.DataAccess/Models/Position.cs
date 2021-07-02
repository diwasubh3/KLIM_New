using System;

namespace YCM.CLO.DataAccess.Models
{
	public partial class Position
    {
        public long PositionId { get; set; }
        public int FundId { get; set; }
        public int SecurityId { get; set; }
        public int DateId { get; set; }
        public decimal? Exposure { get; set; }
        public decimal? PctExposure { get; set; }
        public decimal? PxPrice { get; set; }
        public bool IsCovLite { get; set; }
        public short? CountryId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }

        public bool? IsStale { get; set; }

        public decimal? CapitalizedInterestOrig { get; set; }

        public string SnPAssetRecoveryRating { get; set; }

        public virtual Country Country { get; set; }
        public virtual Fund Fund { get; set; }
        public virtual Security Security { get; set; }

	    public override string ToString()
		    => $"Id: {PositionId} FundId: {FundId} SecurityId: {SecurityId} DateId: {DateId} Exposure:{Exposure:N}";
    }
}
