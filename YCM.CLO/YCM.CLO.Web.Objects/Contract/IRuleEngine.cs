using YCM.CLO.DTO;

namespace YCM.CLO.Web.Objects.Contract
{
    public interface IRuleEngine
    {
        RuleRestultDto Process(short ruleId,string fundCode,int dateId);
    }
}
