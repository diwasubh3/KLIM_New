using System;
using System.Collections.Generic;

namespace YCM.CLO.DataAccess.Models
{
    public partial class Rating
    {
        public Rating()
        {
            this.MarketDatas = new List<MarketData>();
            this.MarketDatas1 = new List<MarketData>();
            this.Calculations = new List<Calculation>();
            this.MarketDatas3 = new List<MarketData>();
            this.MarketDatas4 = new List<MarketData>();
            this.MarketDatas5 = new List<MarketData>();
            this.MarketDatas6 = new List<MarketData>();
            this.MarketDatas7 = new List<MarketData>();
            this.MarketDatas8 = new List<MarketData>();

        }

        public short RatingId { get; set; }
        public string RatingDesc { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }

        public short? Rank { get; set; }

        public DateTime? LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public bool? IsMoody { get; set; }
        public bool? IsSnP { get; set; }
        public bool? IsFitch { get; set; }
        public virtual ICollection<MarketData> MarketDatas { get; set; }
        public virtual ICollection<MarketData> MarketDatas1 { get; set; }
        public virtual ICollection<Calculation> Calculations { get; set; }
        public virtual ICollection<MarketData> MarketDatas3 { get; set; }
        public virtual ICollection<MarketData> MarketDatas4 { get; set; }
        public virtual ICollection<MarketData> MarketDatas5 { get; set; }
        public virtual ICollection<MarketData> MarketDatas6 { get; set; }
        public virtual ICollection<MarketData> MarketDatas7 { get; set; }
        public virtual ICollection<MarketData> MarketDatas8 { get; set; }

    }
}
