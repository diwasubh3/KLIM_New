using System.Collections.Generic;
using System.Linq;
using YCM.CLO.DataAccess.Contracts;
using YCM.CLO.DTO;
using YCM.CLO.Web.Objects.Contract;

namespace YCM.CLO.Web.Objects
{
    public class  CreditScoreAlertProcessor : IProcessor
    {
        public IEnumerable<IProcessortDto> Process(IEnumerable<IProcessortDto> positions, IRepository repository, string fundCode,int dateId)
        {
            var alertIssuers = (repository as IRepository).GetCreditScoreAlertIssuers(fundCode,dateId).ToList();

            positions.Where(p=>alertIssuers.Contains(p.IssuerId)).ToList().ForEach(p =>
            {
                p.Alerts.Add(new AlertDto() {Description = "Position sizing is outside of range for this credit score"});
                p.IsOnAlert = true;
            });


            return positions;
        }
    }
}
