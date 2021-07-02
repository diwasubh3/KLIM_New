using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using YCM.CLO.DataAccess.Models;
using YCM.CLO.Web.Objects.Contract;

namespace YCM.CLO.Web.Objects
{
	public class PositionCacheManager : IPositionCacheManager
    {

        public bool Update(IEnumerable<vw_AggregatePosition> positions)
        {
            var serializedPositions = JsonConvert.SerializeObject(positions);
	        Task.Run(() =>
	        {
		        AllPositionsCache allPositionsCache = new AllPositionsCache();
		        allPositionsCache.UpdatePositions(JsonConvert.DeserializeObject<IEnumerable<vw_AggregatePosition>>(serializedPositions));

		        Top10Bottom10Cache top10Bottom10Cache = new Top10Bottom10Cache();
		        top10Bottom10Cache.UpdatePositions(JsonConvert.DeserializeObject<IEnumerable<vw_AggregatePosition>>(serializedPositions));
	        });

			return true;
        }

        public bool Invalidate()
        {
            AllPositionsCache allPositionsCache = new AllPositionsCache();
            allPositionsCache.Invalidate();

            Top10Bottom10Cache top10Bottom10Cache = new Top10Bottom10Cache();
            top10Bottom10Cache.Invalidate();

            return true;
        }


        public bool Check()
        {
            AllPositionsCache.Check();
            Top10Bottom10Cache.Check();
            return true;
        }


    }
}
