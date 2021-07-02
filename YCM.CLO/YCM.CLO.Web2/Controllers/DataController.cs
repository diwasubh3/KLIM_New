using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

using YCM.CLO.Data.Models;
using YCM.CLO.Data.Repositories.Contracts;
using YCM.CLO.Web.Models;

namespace YCM.CLO.Web.Controllers
{
    public class DataController : ApiController
    {
        readonly IRepository _repository;
        public DataController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IEnumerable<vw_CLOSummary> GetSummaries()
        {
            return _repository.GetSummaries();
        }

        protected override void Dispose(bool disposing)
        {
            _repository.Dispose();
            base.Dispose(disposing);
        }
    }
}