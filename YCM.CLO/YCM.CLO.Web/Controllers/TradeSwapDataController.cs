using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using YCM.CLO.DataAccess.Contracts;
using YCM.CLO.DataAccess.Models;
using YCM.CLO.DTO;
using YCM.CLO.Web.Models;
using YCM.CLO.Web.Objects;
using YCM.CLO.Web.Objects.Contract;
using AutoMapper;
using YCM.CLO.DataAccess;

namespace YCM.CLO.Web.Controllers
{
	public class TradeSwapDataController : Controller
    {

        private IRepository _repository;
        private readonly IAlertEngine _alertEngine;

        //public TradeSwapDataController() : this(new Repository(),new AlertEngine())
        //{

        //}

        public TradeSwapDataController(IRepository repository, IAlertEngine alertEngine)
        {
            _repository = repository;
            _alertEngine = alertEngine;
        }


        [HttpPost]
        public JsonNetResult StartTradeSwap(TradeSwapParam tradeSwapParam)
        {
            _repository.SaveTradeSwap(tradeSwapParam, new TradeSwap() {CreatedBy = User.Identity.Name,CreatedOn = DateTime.Now,DateId = Helper.GetPrevDayDateId()});
            var tradeSwap = _repository.GetLastTradeSwap();
            Task.Run(() =>
            {
                CalculationEngineClient calculationEngineClient = new CalculationEngineClient();
                calculationEngineClient.StartTradeSwapping(tradeSwap.TradeSwapId);
            });
            tradeSwap.Status = 1;
            return new JsonNetResult()
            {
                Data = new
                {
                    TradeSwap = tradeSwap,
                    TradeSwapParam = _repository.GetTradeSwapParam(tradeSwap)
                }
            };
        }

        public JsonNetResult ProcessTradeSwap(int tradeSwapId)
        {
            Task.Run(() =>
            {
                CalculationEngineClient calculationEngineClient = new CalculationEngineClient();
                calculationEngineClient.StartTradeSwapping(tradeSwapId);
            });

            return new JsonNetResult()
            {
                Data = new
                {
                    Status="Started"
                }
            };
        }

        public JsonNetResult GetTradeSwaps(int fundId, int tradeSwapId, GroupBy groupBy)
        {
            var tradeSwaps = _repository.GetTradeSwaps(fundId, tradeSwapId);
            if (groupBy == GroupBy.Sell)
            {
                var groupedBySeller = tradeSwaps.GroupBy(t => t.SellSecurityId).Select(t=> new
                {
                    Parent = t.ToList()[0],
                    GroupBy = groupBy,
                    Children = t.ToList()
                }).ToList();
                return new JsonNetResult() {Data = groupedBySeller};
            }
            else
            {
                var groupedByBuyer = tradeSwaps.GroupBy(t => t.BuySecurityId).Select(t => new
                {
                    Parent = t.ToList()[0],
                    GroupBy = groupBy,
                    Children = t.ToList()
                }).ToList();
                groupedByBuyer.ForEach(gb =>
                {
                    gb.Parent.BuyExposure = gb.Children.Sum(c => c.SellExposure);
                });
                return new JsonNetResult() { Data = groupedByBuyer };
            }
        }

        public JsonNetResult GetLastSavedTradeSwap()
        {
            var tradeSwap = _repository.GetLastTradeSwap();
            return new JsonNetResult()
            {
                Data = new
                {
                    TradeSwap = Mapper.Map<TradeSwap, TradeSwapDto>(tradeSwap),
                    TradeSwapParam = _repository.GetTradeSwapParam(tradeSwap)
                }
            };
        }
    }
}