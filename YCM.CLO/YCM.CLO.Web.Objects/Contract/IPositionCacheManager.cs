using System.Collections.Generic;
using YCM.CLO.DataAccess.Models;

namespace YCM.CLO.Web.Objects.Contract
{
	public interface IPositionCacheManager
    {
        bool Update(IEnumerable<vw_AggregatePosition> positions);
        bool Invalidate();
        bool Check();

    }
}
