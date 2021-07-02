using System;

namespace YCM.CLO.DataAccess.Models
{
	public class TradeSwapParamConstraints
    {
        public int? LiquidityScoreOperatorId { get; set; }
        public decimal? LiquidityScore { get; set; }

        public int? MaturityDateOperatorId { get; set; }
        public DateTime? MaturityDate { get; set; }

        public int? PctPositionOperatorId { get; set; }
        public decimal? PctPosition { get; set; }

        public int? RecoveryOperatorId { get; set; }
        public decimal? Recovery { get; set; }

        public int? YieldOperatorId { get; set; }
        public decimal? Yield { get; set; }

        public int? CreditScoreOperatorId { get; set; }
        public decimal? CreditScore { get; set; }

        public int? MoodyAdjCfrRankOperatorId { get; set; }
        public decimal? MoodyAdjCfrRank { get; set; }


        public int? SpreadOperatorId { get; set; }
        public decimal? Spread { get; set; }


    }
}
