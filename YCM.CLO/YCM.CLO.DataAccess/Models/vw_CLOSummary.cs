using System;
using System.Globalization;

namespace YCM.CLO.DataAccess.Models
{
	public partial class vw_CLOSummary
	{
		public string FundCode { get; set; }

		public bool? IsStale { get; set; }

		public int DateId { get; set; }
		public decimal? Par { get; set; }
		public decimal? BODPar { get; set; }
		public decimal? Spread { get; set; }
		public decimal? BODSpread { get; set; }
		public decimal? TotalCoupon { get; set; }
		public decimal? BODTotalCoupon { get; set; }

		public decimal? WARF { get; set; }
		public decimal? BODWarf { get; set; }
		public decimal? MoodyRecovery { get; set; }
		public decimal? BODMoodyRecovery { get; set; }
		public decimal? Bid { get; set; }
		public decimal? BODBid { get; set; }
		public decimal? PrincipalCash { get; set; }
		public decimal? BODPrincipalCash { get; set; }
		public int FundId { get; set; }
		public int SortOrder { get; set; }
		public decimal? Diversity { get; set; }
		public decimal? BODDiversity { get; set; }
		public decimal? CleanNav { get; set; }
		public decimal? BODCleanNav { get; set; }
		public decimal? WAMaturityDays { get; set; }
		public decimal? BODWAMaturityDays { get; set; }
		public decimal? AssetPar { get; set; }
		public decimal? reInvestCash { get; set; }


		public decimal? B3ToAssetParPct { get; set; }

        public decimal? BMinusToAssetParPct { get; set; }

        public decimal? WSOSpread
		{
            get;set;
		}
		public decimal? WSOWARF
		{
            get;set;
		}

		public decimal? WSOMoodyRecovery
		{
            get;set;
		}

		public decimal? WSOWALife
		{
            get;set;
		}

		public decimal? WSODiversity
		{
            get;set;
		}

		
		
		public decimal? MatrixImpliedSpread { get; set; }

        public decimal? SpreadDiff { get; set; }

        public decimal? BBMVOC { get; set; }

        public decimal? WALCushion { get; set; }

        public decimal? TimeToReinvest { get; set; }

        public string PositionDate => DateId > 0
			? DateTime.ParseExact(DateId.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy")
			: "";

		public override string ToString()
			=> $"{FundCode}: {Par} {Spread} {PrincipalCash} {AssetPar}";
	}


}
