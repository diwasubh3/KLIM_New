using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using YCM.CLO.DataAccess;
using YCM.CLO.DataAccess.Contracts;
using YCM.CLO.DataAccess.Models;
using YCM.CLO.DTO;
using YCM.CLO.Web.Models;
using YCM.CLO.Web.Objects;

namespace YCM.CLO.Web.Controllers
{
	public class AnalystDataController : Controller
    {
        // GET: BidOfferUpload
        readonly IRepository _repository;

        //public AnalystDataController() : this(new Repository())
        //{

        //}

        public AnalystDataController(IRepository repository)
        {
            _repository = repository;
        }


        public JsonNetResult GetAnalystRefresh(LoadTypeEnum? loadType, int? issuerId)
        {
            IEnumerable<AnalystResearchDto> analystResearchDtos;
            if (loadType.HasValue && loadType == LoadTypeEnum.Active)
            {
                analystResearchDtos =
                    Mapper.Map<IEnumerable<vw_AnalystResearch>, IEnumerable<AnalystResearchDto>>(
                        _repository.GetActiveAnalystResearches());
            }
            else
            {
                analystResearchDtos =
                    Mapper.Map<IEnumerable<AnalystResearch>, IEnumerable<AnalystResearchDto>>(
                        _repository.GetAnalystResearches().Where(i => !issuerId.HasValue || i.IssuerId == issuerId.Value));
            }

            var analystResearchDictionary = analystResearchDtos.GroupBy(a => a.Issuer).Select(a => new { IssuerData = a.ToList()[0], AnalystResearches = a.ToList() }).ToDictionary(a=>a.IssuerData.Issuer,a=>a);

	        var allIssuers = CLOCache.GetPositionsWithExposure()
		        .Select(i => new { i.Issuer, i.IssuerDesc, i.IssuerId }).GroupBy(i => i.Issuer).ToDictionary(i => i.Key, i => i.ToList()[0]);
            //var allIssuers = (new AllPositionsCache()).Position.Select(i => new { i.Issuer, i.IssuerDesc,i.IssuerId }).GroupBy(i=>i.Issuer).ToDictionary(i=>i.Key,i=>i.ToList()[0]);
			allIssuers.Keys.ToList().ForEach(i =>
            {
                if (!analystResearchDictionary.ContainsKey(i))
                {
                    analystResearchDictionary.Add(i, new { IssuerData = new AnalystResearchDto() { IssuerId= allIssuers[i].IssuerId.Value, Issuer=allIssuers[i].Issuer }, AnalystResearches = new List<AnalystResearchDto>() });
                }
            });
            return new JsonNetResult()
            {
                Data = analystResearchDictionary.Values.OrderBy(i=>i.IssuerData.Issuer).ToList()
            };
        }

        public JsonNetResult GetAnalysts()
        {
            var analysts = _repository.GetUsers();

            return new JsonNetResult()
            {
                Data = new
                {
                    CLOAnalysts = Mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(analysts.Where(a => a.IsCLOAnalyst.HasValue && a.IsCLOAnalyst.Value).OrderBy(a => a.FullName)),
                    HFnalysts = Mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(analysts.Where(a => a.IsHFAnalyst.HasValue && a.IsHFAnalyst.Value).OrderBy(a => a.FullName))
                }
            };
        }



        [HttpPost]
        public JsonNetResult SaveAnalystResearches(AnalystResearch[] analystResearches)
        {
            var result = _repository.SaveAnalystResearches(analystResearches.Reverse().ToArray());
            return new JsonNetResult() { Data = result };
        }

        protected override void Dispose(bool disposing)
        {
            _repository.Dispose();
            base.Dispose(disposing);
        }
    }
}