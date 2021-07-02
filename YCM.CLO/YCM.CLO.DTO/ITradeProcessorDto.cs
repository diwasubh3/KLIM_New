using System.Collections.Generic;

namespace YCM.CLO.DTO
{
	public interface ITradeProcessorDto
    {
        int SecurityId { get; set; }

        string FundCode { get; set; }

        bool? HasBuyTrade { get; set; }

        bool? HasSellTrade { get; set; }

        IList<TradeInfoDto> Trades { get; set; }
    }

}
