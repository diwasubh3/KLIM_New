using System.Collections.Generic;
using System.Linq;
using YCM.CLO.DataAccess;
using YCM.CLO.DataAccess.Contracts;
using YCM.CLO.DTO;

namespace YCM.CLO.Web.Objects
{
	public class TradesProcessor
    {
        public void Process(IRepository repository, IEnumerable<ITradeProcessorDto> positions, string fundCode)
        {
            var positionDtos = positions as ITradeProcessorDto[] ?? positions.ToArray();
            var groupedTradesDictionary = repository.GetTrades(false, Helper.GetPrevDayDateId()).OrderBy(t=>t.TradeId).GroupBy(t=>t.SecurityId).ToDictionary(tg=>tg.Key,tg=>tg.ToList());

            positionDtos.ToList().ForEach(pos =>
            {
                if (groupedTradesDictionary.ContainsKey(pos.SecurityId))
                {
                    var trades = groupedTradesDictionary[pos.SecurityId].OrderBy(t => t.TradeId).ToList();

                    int buyCounter = 0;
                    int sellCounter = 0;

                    trades.ForEach(t =>
                    {
                        var tradeAllocation =
                            t.TradeAllocations.FirstOrDefault(
                                ta => ta.NewAllocation.HasValue &&
                                      ta.NewAllocation.Value != 0);

                        if (tradeAllocation != null)
                        {
                            var tradeInfo = new TradeInfoDto
                            {
                                Action = t.IsBuy.HasValue && t.IsBuy.Value
                                    ? "BUY " + (++buyCounter).ToString()
                                    : "SELL " + (++sellCounter).ToString()
                            };

                            if (t.IsBuy.HasValue && t.IsBuy.Value)
                            {
                                pos.HasBuyTrade = true;
                            }
                            else
                            {
                                pos.HasSellTrade = true;
                            }
                            
                            tradeInfo.Quantity = t.TradeAmount.HasValue? t.TradeAmount.Value.ToString("#,##,###") :"";

                            if (t.TradePrice != null) tradeInfo.Price = t.TradePrice.Value.ToString("#,##,###.##");


                            if (t.CreatedOn != null)
                                tradeInfo.Audit = (t.LastUpdatedOn.HasValue
                                    ? "Last Updated On " + t.LastUpdatedOn.Value.ToString("MM/dd/yyyy h:mm tt")
                                    : "Created On " + t.CreatedOn.Value.ToString("MM/dd/yyyy h:mm tt"))
                                                  + " by " +
                                                  (string.IsNullOrEmpty(t.LastUpdatedBy) ? t.CreatedBy : t.LastUpdatedBy);

                            tradeInfo.Comment = t.Comments;

                            pos.Trades.Add(tradeInfo);
                        }
                    });
                }
            }
            );

        }
    }
}
