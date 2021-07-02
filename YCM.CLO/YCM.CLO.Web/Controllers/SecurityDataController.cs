using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using log4net;
using YCM.CLO.DataAccess;
using YCM.CLO.DataAccess.Contracts;
using YCM.CLO.DataAccess.Models;
using YCM.CLO.DTO;
using YCM.CLO.Web.Models;
using YCM.CLO.Web.Objects;

namespace YCM.CLO.Web.Controllers
{
	public class SecurityDataController : Controller
    {
	    private readonly ILog _logger;
        readonly IRepository _repository;

        //public SecurityDataController() : this(new Repository())
        //{

        //}

        public SecurityDataController(IRepository repository)
        {
	        _logger = LogManager.GetLogger(typeof(SecurityDataController));
            _repository = repository;
        }

        public JsonNetResult GetSecurityOverrides(LoadTypeEnum? loadType, int? securityId)
        {
            var securtiyOverrides = Mapper.Map<IEnumerable<SecurityOverride>, IEnumerable<SecurityOverrideDto>>(
                _repository.GetSecurityOverrides(loadType, securityId));

            return new JsonNetResult() { Data = securtiyOverrides };
        }

        public JsonNetResult GetGroupedSecurityOverrides(LoadTypeEnum? loadType, int? securityId)
        {
            var securtiyOverrides = Mapper.Map<IEnumerable<SecurityOverride>, IEnumerable<SecurityOverrideDto>>(
                _repository.GetSecurityOverrides(loadType, securityId)).GroupBy(s => new { SecurityCode = s.SecurityCode, IssuerDesc = s.IssuerDesc, FacilityDesc = s.FacilityDesc }).Select(s =>
               new {
                   s.Key.SecurityCode,
                   s.Key.IssuerDesc,
                   s.Key.FacilityDesc,
                   SearchText = String.Join("|", s.ToList().Select(si => si.SearchText)),
                   Securities = s.ToList()
               });

            return new JsonNetResult() { Data = securtiyOverrides };
        }

	    [HttpPost]
	    public JsonNetResult UpdateIsPrivate(IssuerDto dto)
	    {
		    _logger.Info($"Updating security {dto.IssuerDesc}. Private? {dto.IsPrivate}.");
		    var updated = _repository.UpdateIsPrivate(dto.IssuerId, dto.IsPrivate, User.Identity.Name);
		    _logger.Info("Calling cache refresh...");
            CLOCache.Refresh();
            _logger.Info("Refresh done.");
		    return new JsonNetResult { Data = dto };
	    }

	    [HttpPost]
	    public JsonNetResult UpdateBbgId(SecurityDto dto)
	    {
			_logger.Info($"Updating security {dto.SecurityCode} with bbg id {dto.BBGId}.");
		    var updated = _repository.UpdateBbgId(dto.SecurityId, dto.BBGId, User.Identity.Name);
		    _logger.Info("Calling cache refresh...");
            CLOCache.Refresh();
            _logger.Info("Refresh done.");
		    return new JsonNetResult{Data = dto};
	    }

        [HttpPost]
        public JsonNetResult SaveSecurityOverrides(SecurityOverride[] securityOverrides)
        {
            var updatedSecurityOverride = _repository.SaveSecurityOverrides(securityOverrides.Reverse().ToArray(), User.Identity.Name);
            return new JsonNetResult() { Data = updatedSecurityOverride };
        }

        public JsonNetResult GetSecurities()
        {
            return new JsonNetResult()
            {
                Data = Mapper.Map<IEnumerable<vw_Security>, IEnumerable<VwSecurityDto>>(_repository.GetSecurities())
            };
        }

        public JsonNetResult GetCurrentSecurities()
        {
            return new JsonNetResult()
            {
                Data = Mapper.Map<IEnumerable<vw_SecurityMarketCalculation>, IEnumerable<VwSecurityDto>>(_repository.GetCurrentSecurities())
            };
        }

        public JsonNetResult GetSecuritiesForRecon()
        {
            return new JsonNetResult()
            {
                Data = Mapper.Map<IEnumerable<Security>, IEnumerable<SecurityReconDto>>(_repository.GetSecuritiesForRecon()).OrderBy(s => s.Source).ThenBy(s => s.Code)
            };
        }

        public JsonNetResult GetLoanAttributesOverridesForRecon(int? securityId)
        {
            var activeSecurityOverrides = Mapper.Map<IEnumerable<SecurityOverride>, IEnumerable<LoanAttributeOverrideDto>>(_repository.GetConflictingSecurityOverrides(securityId));
            var securities = Mapper.Map<IEnumerable<vw_Security>, IEnumerable<VwSecurityDto>>(_repository.GetSecurities(activeSecurityOverrides.Select(a => a.SecurityId).ToArray())).OrderBy(s => s.SecurityCode).ToList();
            securities.ForEach(s =>
            {
                s.LoanAttributeOverrides = activeSecurityOverrides.Where(a => a.SecurityId == s.SecurityId).ToList();
                s.SetSearchText();
            });
            return new JsonNetResult() { Data = securities };
        }

        public JsonNetResult ReconcileSecurities(SecurityReconDto[] securities)
        {
            _repository.TransferSecurities(securities.First(s => s.Source == "YCM").SecurityId,
                securities.First(s => s.Source == "WSO").SecurityId, User.Identity.Name);
            bool result = _repository.DeleteSecurity(securities.First(s => s.Source == "YCM").SecurityId,
                User.Identity.Name);
            CalculationController calculationController = new CalculationController();
            calculationController.Process(null);
            return new JsonNetResult()
            {
                Data = result
            };
        }


        [HttpPost]
        public JsonNetResult EndSecurityOverride(LoanAttributeOverrideDto loanAttributeOverride)
        {
            DateTime endDate = DateTime.Today.AddDays(-1);
            return new JsonNetResult()
            {
                Data =
                    new
                    {
                        Status = _repository.EndSecurityOverride(loanAttributeOverride.SecurityOverrideId, endDate,
                        User.Identity.Name),
                        EndDate = endDate.ToString("MM/dd/yyyy")
                    }
            };
        }

        [HttpPost]
        public JsonNetResult ResetSecurityOverrideConflict(LoanAttributeOverrideDto loanAttributeOverride)
        {
            DateTime endDate = DateTime.Today.AddDays(-1);
            return new JsonNetResult()
            {
                Data =
                    new
                    {
                        Status = _repository.ResetSecurityOverrideConflict(loanAttributeOverride.SecurityOverrideId, loanAttributeOverride.SecurityId,User.Identity.Name)
                    }
            };
        }
    }
}