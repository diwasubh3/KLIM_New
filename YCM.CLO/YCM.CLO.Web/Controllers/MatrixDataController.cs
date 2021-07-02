using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using YCM.CLO.DataAccess;
using YCM.CLO.DataAccess.Contracts;
using YCM.CLO.DataAccess.Models;
using YCM.CLO.Web.Objects;

namespace YCM.CLO.Web.Controllers
{
    public class MatrixDataController : Controller
    {

        readonly IRepository _repository;

        private const string INSERTMATRIXCOLUMNS = "INSERT INTO [CLO].[MatrixData]([FundId],[Spread],[Diversity],[WARF],[WARFModifier],[DataPointType],[InterpolationType],[FromMajorMatrixDataId],[ToMajorMatrixDataId] )";

        //public MatrixDataController() : this(new Repository())
        //{

        //}

        public MatrixDataController(IRepository repository)
        {
            _repository = repository;
        }

        // GET: MatrixData
        public FileResult GenerateMinors(int fundId)
        {
            var minorMatrixDatas = _repository.GenerateMinorMatrixData(fundId).ToList();

            var minorFile = Server.MapPath("~/Content") + "\\" + Guid.NewGuid().ToString() + ".sql" ;

            var file = System.IO.File.CreateText(minorFile);
            file.WriteLine("DELETE FROM [CLO].[MatrixData] WHERE FundId = " + fundId + " AND DataPointType = 2");
            file.WriteLine("GO");
            file.WriteLine(INSERTMATRIXCOLUMNS);
            file.WriteLine("VALUES");
            for (int i = 0; i < minorMatrixDatas.Count; i++)
            {
                if(i % 500==0 && i != 0)
                {
                    file.WriteLine("GO");
                    file.WriteLine(INSERTMATRIXCOLUMNS);
                    file.WriteLine("VALUES");
                }
                file.WriteLine(minorMatrixDatas[i].ToString() + (i == (minorMatrixDatas.Count-1) || ((i+1)%500==0)?string.Empty:","));
            }
            file.WriteLine("GO");
            file.Close();

            var stream = new MemoryStream(System.IO.File.ReadAllBytes(minorFile));
            System.IO.File.Delete(minorFile);
            return new FileStreamResult(stream, "application/sql") { FileDownloadName="Minor.Sql"};
        }


        public JsonResult Majors(int fundId)
        {

            var matrixDatas = _repository.GetMajorMatrixDatas(fundId).GroupBy(m=> new {m.Spread}).ToList();

            return new JsonResult()
            {
                Data = matrixDatas,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        public JsonResult MatrixPoints(int fundId)
        {

            var matrixPoints = _repository.GetMatrixPoints(fundId).ToList();

            return new JsonResult()
            {
                Data = matrixPoints,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        public JsonResult SpreadMinors(int fundId,decimal fromSpread, decimal toSpread)
        {
            var matrixDatas = _repository.GetSpreadInterpolatedMinorMatrixDatas(fundId,fromSpread,toSpread).GroupBy(m => new { m.Spread }).ToList();
            return new JsonResult()
            {
                Data = matrixDatas,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult DiversityMinors(int fundId, decimal fromDiversity, decimal toDiversity)
        {
            var matrixDatas = _repository.GetDiversityInterpolatedMinorMatrixDatas(fundId, fromDiversity, toDiversity).GroupBy(m => new { m.Spread }).ToList();
            return new JsonResult()
            {
                Data = matrixDatas,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult AddMatrixPoint([FromBody] MatrixPoint matrixData)
        {
            _repository.AddMatrixPoint(matrixData, User.Identity.Name);

            CalculationEngineClient calculationEngineClient = new CalculationEngineClient();
            calculationEngineClient.CalculateFrontier(_repository.GetPrevDayDateId(), matrixData.FundId, User.Identity.Name);
            //AllPositionsCache allPositionsCache = new AllPositionsCache();
            //allPositionsCache.Invalidate();
            CLOCache.Invalidate();
            return MatrixPoints(matrixData.FundId);
        }

        protected override void Dispose(bool disposing)
        {
            _repository.Dispose();
            base.Dispose(disposing);
        }
    }
}