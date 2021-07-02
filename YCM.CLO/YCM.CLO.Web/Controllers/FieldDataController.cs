using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using YCM.CLO.DataAccess;
using YCM.CLO.DataAccess.Contracts;
using YCM.CLO.DataAccess.Models;
using YCM.CLO.DTO;
using YCM.CLO.Web.Models;

namespace YCM.CLO.Web.Controllers
{
	public class FieldDataController : Controller
    {
        readonly IRepository _repository;

        //public FieldDataController() : this(new Repository())
        //{

        //}


        public FieldDataController(IRepository repository)
        {
            _repository = repository;
        }

        
        public JsonNetResult GetAllCustomViewFields()
	    {
		    var groups = GetCustomPositionViewFieldGroupsInternal();
		    var fields = groups.SelectMany(x => x.Fields).Where(x => (!x.Hidden.HasValue
			|| !x.Hidden.Value) && !string.IsNullOrEmpty(x.FieldTitle)).ToList();
		    var viewFields = Mapper.Map<IEnumerable<Field>, IEnumerable<CustomViewFieldDto>>(fields).ToList();
			viewFields.ForEach(x => x.SortOrder = 9999);
			return new JsonNetResult{Data = viewFields};
	    }

	    public JsonNetResult GetFieldsForCustomView(int viewId)
	    {
		    var fields = _repository.GetFieldsForCustomView(viewId);
		    var viewFields = Mapper.Map<IEnumerable<Field>, IEnumerable<FieldDto>>(fields).ToList();
		    var result = new JsonNetResult {Data = viewFields};
		    return result;
	    }

		private List<FieldGroup> GetCustomPositionViewFieldGroupsInternal()
			=> _repository.GetFieldGroups().ToList()
				.Where(x => x.ShowOnPositions.GetValueOrDefault() || x.FieldGroupId == 5).ToList();

		public JsonNetResult GetCustomPositionViewFieldGroups()
	    {
		    try
		    {
			    var groups = Mapper.Map<IEnumerable<FieldGroup>, IEnumerable<FieldGroupDto>>(GetCustomPositionViewFieldGroupsInternal());
			    return new JsonNetResult() { Data = groups };
		    }
			catch (Exception e)
		    {
			    Console.WriteLine(e);
			    return new JsonNetResult() { Data = new List<FieldGroupDto>() };
		    }
		}

		public JsonNetResult GetFieldGroups()
        {
            return new JsonNetResult() { Data = Mapper.Map<IEnumerable<FieldGroup>, IEnumerable<FieldGroupDto>>(_repository.GetFieldGroups()) };
        }

        public JsonNetResult GetFilterFields()
        {
            var fieldGroups = _repository.GetFieldGroups().Where(v => (v.FieldGroupId > 0 && v.FieldGroupId < 4) || v.FieldGroupId == 5).ToList();
            fieldGroups.ForEach(fg=>fg.Fields = fg.Fields.Where(fgf => fgf.ShowInFilter.HasValue && fgf.ShowInFilter.Value).OrderBy(fgf=>fgf.FilterOrder).ThenBy(fgf=>fgf.SortOrder).ToList());
            return new JsonNetResult() { Data = Mapper.Map<IEnumerable<FieldGroup>, IEnumerable<FieldGroupDto>>(fieldGroups) };
        }

        public JsonNetResult GetPositionViewFieldGroups()
        {
            return new JsonNetResult() { Data = Mapper.Map<IEnumerable<FieldGroup>, IEnumerable<FieldGroupDto>>(_repository.GetFieldGroups().Where(v => v.ShowOnPositions.HasValue && v.ShowOnPositions.Value)) };
        }

        public JsonNetResult GetFixedFields()
        {
            var fixedFields = Mapper.Map<IEnumerable<Field>, IEnumerable<FieldDto>>(_repository.GetFields("Fixed Fields"));
            return new JsonNetResult() { Data = fixedFields };
        }

        public JsonNetResult GetFundRestrictionFields()
        {
            return new JsonNetResult() { Data = Mapper.Map<IEnumerable<Field>, IEnumerable<FieldDto>>(_repository.GetFields("Fund Restrictions")) };
        }

        public JsonNetResult GetSecurityOverrideHeaderFields()
        {
            var headerFields = new List<FieldDto>() { new FieldDto()
            {
                JsonPropertyName = "securityCode",
                FieldTitle = "LOAN ID | ISSUER | FACILITY"
            }};

            headerFields.AddRange(Mapper.Map<IEnumerable<Field>, IEnumerable<FieldDto>>(_repository.GetFields(new string[] { "OverrideValue", "EffectiveFrom", "EffectiveTo" })).OrderBy(f => f.FieldGroupId).ThenBy(f => f.SortOrder));

            return new JsonNetResult()
            {
                Data =
                    headerFields
            };
        }

        public JsonNetResult GetSecurityOverrideableFields()
        {
            return new JsonNetResult() { Data = Mapper.Map<IEnumerable<Field>, IEnumerable<FieldDto>>(_repository.GetSecurityOverrideableFields()) };
        }

        public JsonNetResult GetSaveSecurityOverrideHeaderFields()
        {
            return new JsonNetResult()
            {
                Data =
                    Mapper.Map<IEnumerable<Field>, IEnumerable<FieldDto>>(_repository.GetFields(new string[] { "Attribute", "CurrentValue", "OverrideValue", "EffectiveFrom", "EffectiveTo" })).OrderBy(f => f.FieldGroupId).ThenBy(f => f.SortOrder)
            };
        }


        public JsonNetResult GetBidOfferHeaderFields()
        {
            return new JsonNetResult()
            {
                Data =
                    Mapper.Map<IEnumerable<Field>, IEnumerable<FieldDto>>(_repository.GetFields(new string[] { "SecurityCode", "Issuer", "Facility"})).OrderBy(f => f.FieldGroupId * f.SortOrder)
                    .Concat(Mapper.Map<IEnumerable<Field>, IEnumerable<FieldDto>>(_repository.GetFields(new string[] { "Bid", "Offer"})).OrderBy(f => f.FieldGroupId * f.SortOrder))
                    .Concat(Mapper.Map<IEnumerable<Field>, IEnumerable<FieldDto>>(_repository.GetFields(new string[] { "UploadDateTimeUser"})).OrderBy(f => f.FieldGroupId * f.SortOrder))
            };
        }


        public JsonNetResult GetAnalystResearchHeaderFields()
        {
            return new JsonNetResult()
            {
                Data =
                    new
                    {
                        IssuerFields = Mapper.Map<IEnumerable<Field>, IEnumerable<FieldDto>>(_repository.GetFields(new string[] { "Issuer", "LastUpdatedOn", "CLOAnalyst", "HFAnalyst" })).OrderByDescending(i => i.IsSecurityOverride).ThenBy(f => f.SortOrder),
                        AnalystResearchFields = Mapper.Map<IEnumerable<Field>, IEnumerable<FieldDto>>(_repository.GetFieldGroups().First(f => f.FieldGroupName == "Analyst").Fields).OrderBy(f => f.SortOrder)
                    }
            };
        }

        public JsonNetResult GetNewSecurityReconHeaderFields()
        {
            return new JsonNetResult()
            {
                Data =
                    Mapper.Map<IEnumerable<Field>, IEnumerable<FieldDto>>(_repository.GetFields(new string[] { "Issuer", "Source", "Code" })).OrderBy(f=>f.FieldGroupId).ThenBy(f => f.SortOrder)
                    .Concat(Mapper.Map<IEnumerable<Field>, IEnumerable<FieldDto>>(_repository.GetFields(new string[] {   "Facility",  "MaturityDate" })).OrderByDescending(f=>f.FieldGroupId).ThenBy(f => f.SortOrder))
            };
        }

        public JsonNetResult GetLoanAttributeOverrideReconHeaderFields()
        {
            var headerFields = new List<FieldDto>() { new FieldDto()
            {
                JsonPropertyName = "securityName",
                FieldTitle = "LOAN ID | ISSUER | FACILITY",
                DisplayWidth = 300
            }};

            headerFields.AddRange(Mapper.Map<IEnumerable<Field>, IEnumerable<FieldDto>>(_repository.GetFields(new string[] { "Attribute", "Blank", "Conflict", "Date" })).OrderBy(f => f.FieldGroupId).ThenBy(a => a.SortOrder));

            return new JsonNetResult()
            {
                Data =
                    headerFields
            };

        }

        public JsonNetResult GetTradesHeaderFields()
        {
            var headerFields = new List<FieldDto>() { new FieldDto()
            {
                JsonPropertyName = "securityName",
                FieldTitle = "LOAN ID | ISSUER | FACILITY"
            }};

            headerFields.AddRange(Mapper.Map<IEnumerable<Field>, IEnumerable<FieldDto>>(_repository.GetFields(new string[] { "Trade Amount", "Trade Price", "Trade Cash", "Trade New" })).OrderBy(f => f.FieldGroupId).ThenBy(a => a.SortOrder));

            return new JsonNetResult()
            {
                Data =
                    headerFields
            };
        }
        
        protected override void Dispose(bool disposing)
        {
            _repository.Dispose();
            base.Dispose(disposing);
        }
    }
}