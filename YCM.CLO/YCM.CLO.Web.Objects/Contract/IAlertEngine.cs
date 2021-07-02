using System.Collections.Generic;
using YCM.CLO.DTO;

namespace YCM.CLO.Web.Objects.Contract
{
	public interface IAlertEngine
    {
        IEnumerable<IProcessortDto> ProcessAlerts(IEnumerable<IProcessortDto> position,int dateId,string fundCode);
    }
}
