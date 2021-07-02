using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using YCM.CLO.DataAccess.Contracts;
using YCM.CLO.DataAccess.Models;
using YCM.CLO.DTO;
using YCM.CLO.Web.Models;

namespace YCM.CLO.Web.Controllers
{
	public class ParameterDataController : Controller
    {
        readonly IRepository _repository;

        public ParameterDataController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public JsonNetResult SaveParameterValue(ParameterValue parameterValue)
        {
            var updatedParameterValue = _repository.AddOrUpdateParameterValue(parameterValue);
            return new JsonNetResult() { Data = updatedParameterValue };
        }


        public JsonNetResult GetParameterValues()
        {
            return new JsonNetResult()
            {
                Data = Mapper.Map<IEnumerable<ParameterValue>, IEnumerable<ParameterValueDto>>(_repository.GetParameterValues())
            };
        }

        public JsonNetResult GetParameterValuesForParameterType(string parameterTypeName)
        {
            return new JsonNetResult()
            {
                Data = Mapper.Map<IEnumerable<ParameterValue>, IEnumerable<ParameterValueDto>>(_repository.GetParameterValues(parameterTypeName))
            };
        }

        public JsonNetResult GetParameterTypes()
        {
            return new JsonNetResult()
            {
                Data = Mapper.Map<IEnumerable<ParameterType>, IEnumerable<ParameterTypeDto>>(_repository.GetParameterTypes())
            };
        }

        protected override void Dispose(bool disposing)
        {
            _repository.Dispose();
            base.Dispose(disposing);
        }
    }
}