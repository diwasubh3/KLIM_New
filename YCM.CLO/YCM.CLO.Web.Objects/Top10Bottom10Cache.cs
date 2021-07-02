using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YCM.CLO.DataAccess;
using YCM.CLO.DataAccess.Contracts;
using YCM.CLO.DataAccess.Models;
using YCM.CLO.Web.Objects.Contract;

namespace YCM.CLO.Web.Objects
{
	public class Top10Bottom10Cache : ICache<vw_AggregatePosition>
    {
        static IRepository _repository;
        static ConcurrentDictionary<int, RuleResult> _top10Bottom10;
        static object _lock;

        static Top10Bottom10Cache()
        {
            _lock = new object();
            _top10Bottom10 = new ConcurrentDictionary<int, RuleResult>();
            _repository = new Repository();
            Fill();
        }

        public static void Fill()
        {
	        Task.Run(() =>
	        {
		        lock (_lock)
		        {
			        AddUpdateRule(2);
		        }
	        });
        }

		private static void AddUpdateRule(short ruleId)
        {
            var rule = _repository.GetRule(ruleId);
            RuleResult ruleResult;
            if (_top10Bottom10.TryGetValue(ruleId, out ruleResult))
            {
                _top10Bottom10.TryUpdate(ruleId, _repository.ExecuteRule(rule, "CLO-1", Helper.GetPrevDayDateId()), ruleResult);
            }
            else
            {
                _top10Bottom10.TryAdd(ruleId, _repository.ExecuteRule(rule, "CLO-1", Helper.GetPrevDayDateId()));
            }
        }


        public bool UpdatePositions(IEnumerable<vw_AggregatePosition> positions)
        {
            lock (_lock)
            {
                RuleResult ruleResult;
                if (_top10Bottom10.TryGetValue(2, out ruleResult))
                {
                    if (ruleResult != null)
                    {
                        positions.ToList().ForEach(p =>
                        {
                            var originalTopPosition = ruleResult.TopPositions.FirstOrDefault(s => s.SecurityId == p.SecurityId);
                            if (originalTopPosition != null)
                            {
                                var originalTopPositionIndex = ruleResult.TopPositions.IndexOf(originalTopPosition);
                                ruleResult.TopPositions.RemoveAt(originalTopPositionIndex);
                                ruleResult.TopPositions.Insert(originalTopPositionIndex, p);
                            }

                            var originalBottomPosition = ruleResult.BottomPositions.FirstOrDefault(s => s.SecurityId == p.SecurityId);
                            if (originalBottomPosition != null)
                            {
                                var originalBottomPositionIndex = ruleResult.BottomPositions.IndexOf(originalBottomPosition);
                                ruleResult.BottomPositions.RemoveAt(originalBottomPositionIndex);
                                ruleResult.BottomPositions.Insert(originalBottomPositionIndex, p);
                            }
                        });
                    }
                }

            }

            return true;
        }

        public RuleResult Get(short ruleId)
        {
            lock (_lock)
            {
                RuleResult ruleResult;
                bool status = _top10Bottom10.TryGetValue(ruleId, out ruleResult);
                if (status && ruleResult != null)
                {
                    return ruleResult;
                }
                else
                {
                    AddUpdateRule(ruleId);
                    return Get(ruleId);
                }
            }

        }

        public static bool Check()
        {
            return true;
        }

        public void Invalidate()
        {
            Top10Bottom10Cache.Fill();
        }

    }
}
