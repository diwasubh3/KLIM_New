using System.Collections.Generic;
using YCM.CLO.DataAccess.Contracts;
using YCM.CLO.DTO;

namespace YCM.CLO.Web.Objects.Contract
{
    public interface IProcessor
    {
        IEnumerable<IProcessortDto> Process(IEnumerable<IProcessortDto> positions,IRepository repository,string fundCode,int dateId);
    }
}
