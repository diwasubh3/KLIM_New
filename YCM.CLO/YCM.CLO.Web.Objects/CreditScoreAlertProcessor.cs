using System.Collections.Generic;
using System.Linq;
using YCM.CLO.DataAccess.Contracts;
using YCM.CLO.DataAccess.Extensions;
using YCM.CLO.DTO;
using YCM.CLO.Web.Objects.Contract;

namespace YCM.CLO.Web.Objects
{
    public class  CreditScoreAlertProcessor : IProcessor
    {
        public IEnumerable<IProcessortDto> Process(IEnumerable<IProcessortDto> positions, IRepository repository, string fundCode,int dateId)
        {
            var alertIssuers = (repository as IRepository).GetCreditScoreAlertIssuers(fundCode,dateId).ToList();

            positions.ToList().ForEach(p =>
            {
                if(alertIssuers.Contains(p.IssuerId))
                {
                    p.Alerts.Add(new AlertDto() { Description = "Position sizing is outside of range for this credit score" });
                    p.IsOnAlert = true;
                }

                if (string.IsNullOrEmpty(p.MaturityDate) )
                {
                    p.Alerts.Add(new AlertDto() { Description = "Security is missing maturity date" });
                    p.IsOnAlert = true;
                }

                if (string.IsNullOrEmpty(p.Bid) || string.IsNullOrEmpty(p.Offer) || !p.Bid.IsDecimal() ||
                    !p.Offer.IsDecimal() || p.Bid.ToDecimal() == 0 || p.Offer.ToDecimal() == 0)
                {
                    p.Alerts.Add(new AlertDto() { Description = "Loan is missing either Bid or Offer" });
                    p.IsOnAlert = true;
                }
            }); 

               return positions;
        }
    }
}
