using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace YCM.CLO.DataAccess.Models
{
    public partial class Security
    {
        public Security()
        {
            this.Calculations = new List<Calculation>();
            this.MarketDatas = new List<MarketData>();
            this.Positions = new List<Position>();
            this.Pricings = new List<Pricing>();
            this.SecurityOverrides = new List<SecurityOverride>();
            this.Trades = new List<Trade>();
            this.SellTradeSwappingSnapshots = new List<TradeSwapSnapshot>();
            this.BuyTradeSwappingSnapshots = new List<TradeSwapSnapshot>();
        }

        public int SecurityId { get; set; }
        public string SecurityCode { get; set; }
        public string SecurityDesc { get; set; }
        public string BBGId { get; set; }
        public int IssuerId { get; set; }
        public short FacilityId { get; set; }
        public DateTime? CallDate { get; set; }
        public DateTime? MaturityDate { get; set; }
        public string GICSIndustry { get; set; }
        public short SnPIndustryId { get; set; }
        public short MoodyIndustryId { get; set; }
        public bool IsFloating { get; set; }
        public short LienTypeId { get; set; }
        public string ISIN { get; set; }
        public short? SourceId { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public virtual ICollection<Calculation> Calculations { get; set; }
        public virtual Facility Facility { get; set; }
        public virtual Industry Industry { get; set; }
        public virtual Industry Industry1 { get; set; }
        public virtual Issuer Issuer { get; set; }
        public virtual LienType LienType { get; set; }
        public virtual ICollection<MarketData> MarketDatas { get; set; }
        public virtual ICollection<Position> Positions { get; set; }
        public virtual ICollection<Pricing> Pricings { get; set; }
        public virtual ICollection<SecurityOverride> SecurityOverrides { get; set; }
        public virtual ICollection<Trade> Trades { get; set; }
        public virtual ICollection<TradeSwapSnapshot> SellTradeSwappingSnapshots { get; set; }
        public virtual ICollection<TradeSwapSnapshot> BuyTradeSwappingSnapshots { get; set; }
    }
}
