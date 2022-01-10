using System;
using System.Collections.Generic;

namespace YCM.CLO.DTO
{
    public sealed partial class TradeDto : IProcessortDto,IWatchProcessedDto
    {
        public TradeDto()
        {
            this.TradeAllocations = new List<TradeAllocationDto>();
            this.Alerts = new List<AlertDto>();
        }

        public long TradeId { get; set; }
        public int SecurityId { get; set; }
        public int DateId { get; set; }
        public bool? IsBuy { get; set; }

        public bool IsSell { get { return !(IsBuy.HasValue && IsBuy.Value); } }
        public decimal? TradeAmount { get; set; }
        public decimal? TradePrice { get; set; }

        public decimal? FinalAllocation { get; set; }

        public bool? KeepOnBlotter { get; set; }
        public bool? SellAll { get; set; }
        public decimal? BidOfferPrice { get; set; }
        public string Comments { get; set; }

        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public SecurityDto Security { get; set; }

        public string Bid { get; set; }
        public string Offer { get; set; }

        public ICollection<TradeAllocationDto> TradeAllocations { get; set; }

        public int IssuerId
        {
            get;set;
        }

        public bool? IsOnAlert
        {
            get; set;
        }

        public IList<AlertDto> Alerts
        {
            get; set;
        }

        public string FundCode
        {
            get;set;
        }

        public int? WatchId { get; set; }
        public bool? IsOnWatch { get; set; }
        public short? WatchObjectTypeId { get; set; }
        public int? WatchObjectId { get; set; }
        public string WatchComments { get; set; }
        public string WatchLastUpdatedOn { get; set; }
        public string WatchUser { get; set; }

        public bool? IsCancelled { get; set; }

        public string Audit
        {
            get
            {
                return LastUpdatedOn.HasValue
	                ? "Last Updated On " + LastUpdatedOn.Value.ToString("MM/dd/yyyy h:mm tt")
	                : "Created On " + (CreatedOn.HasValue ? CreatedOn.Value.ToString("MM/dd/yyyy h:mm tt") : "")
	                                + " by " +
	                                (string.IsNullOrEmpty(LastUpdatedBy) ? CreatedBy : LastUpdatedBy);
            }
        }


        public decimal NumExposure { get; set; }
        public string PctExposure { get; set; }
        public decimal? PctExposureNum { get; set; }
        public string MaturityDate { get; set; }
        public string SearchText =>( Security.SecurityCode + "|" + Security.Issuer + "|" + Security.Facility + "|" + TradeAmount + "|" + TradePrice + "|" + FinalAllocation + Comments).ToLower();

    }

    public partial class TradeHistoryDto
    {
        public string TradeDate { get; set; }
        public string TradeType { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
        public string Counterparty { get; set; }
    }
}
