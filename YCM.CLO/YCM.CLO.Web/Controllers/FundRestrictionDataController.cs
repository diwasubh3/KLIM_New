using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using YCM.CLO.DataAccess;
using YCM.CLO.DataAccess.Contracts;
using YCM.CLO.DataAccess.Models;
using YCM.CLO.DTO;
using YCM.CLO.Web.Models;
using static YCM.CLO.DataAccess.Helper;
using static YCM.CLO.DataAccess.Constants;

namespace YCM.CLO.Web.Controllers
{
	public class FundRestrictionDataController : Controller
    {
        readonly IRepository _repository;

		//public FundRestrictionDataController() : this(new Repository())
		//{

		//}


		public FundRestrictionDataController(IRepository repository)
        {
            _repository = repository;
        }


        public JsonNetResult GetFundRestrictionsTypes()
        {
			var data = _repository.GetFundRestrictionTypes().ToList();
			return new JsonNetResult() { Data = data.Select(f => new { f.DisplayColor, f.FundRestrictionToolTip, f.FundRestrictionTypeName, f.FundRestrictionTypeId, f.SortOrder }) };
        }

        public JsonNetResult GetFundRestrictions(int? fundId)
        {
			var restrictions = _repository.GetFundRestrictions(fundId).ToList();
            List<MatrixPoint> matrixPoints = new List<MatrixPoint>();
            List<Field> matrixPointBasedFields = _repository.GetFields("Matrix Point Based Fund Restrictions Fields").ToList();

            if (fundId.HasValue)
            {
                matrixPoints = _repository.GetMatrixPoints(fundId.Value).ToList();
            }
            
            var data = restrictions.OrderBy(f => f.Field.SortOrder).
				Select
				(f => new FundRestrictionResult
				{
					FundId = f.FundId,
					RestrictionValue = f.RestrictionValue,
					OperatorId = f.OperatorId,
					Id = f.Id,
					FieldName = f.Field.FieldName,
					DisplayColor = string.Empty,
					FundRestrictionTypeId = f.FundRestrictionTypeId,
					FundRestrictionTypeName = f.FundRestrictionType.FundRestrictionTypeName,
					FundRestrictionToolTip = f.FundRestrictionType.FundRestrictionToolTip,
					FieldTitle = f.Field.FieldTitle,
					FieldId = f.FieldId,
					OperatorCode = f.Operator.OperatorCode,
					OperatorVal = f.Operator.OperatorVal,
					JsonPropertyName = f.Field.JsonPropertyName,
					SortOrder = f.FundRestrictionType.SortOrder,
					IsPercentage = f.Field.IsPercentage,
                    IsDisabled =  (matrixPointBasedFields.Any(fo => fo.FieldTitle  == f.Field.FieldTitle) && matrixPoints.Count() > 0)
				}).ToList();
			var assetParRestrictions = GetAssetParRestrictions(null);
			data.AddRange(assetParRestrictions);
			return new JsonNetResult()
			{
				Data = data.OrderBy(f => f.SortOrder)
			};
		}

		private List<FundRestrictionResult> GetAssetParRestrictions(int? fundId)
		{
			var fields = _repository.GetFields("Fund Restrictions");
			var field = fields.FirstOrDefault(x => x.FieldName == "AssetPar");
			var funds = _repository.GetFunds();
			var difference = AssetParFundRestrictionTypes
				.FirstOrDefault(x => x.FundRestrictionTypeId == (int)CustomFundRestrictions.CURRENTVSPREVIOUSASSPAR);

			var fieldName = field?.FieldName ?? "AssetPar";
			var fieldTitle = field?.FieldTitle ?? "ASSET PAR";
			var jsonPropName = field?.JsonPropertyName ?? "assetPar";
			var fieldId = field?.FieldId ?? default(short);

			var dateId = GetPrevDayDateId();
			var assetPars = _repository.GetAssetPars(dateId).Where(x => !fundId.HasValue
			|| x.FundId == fundId.Value).ToList();
			dateId = GetPrevToPrevDayDateId();
			var prevAssetPars = _repository.GetAssetPars(dateId);
			var restrictions = new List<FundRestrictionResult>();

			var id = 99;
			foreach(var assetPar in assetPars)
			{
				id++;
				var ap = assetPar.AssetPar + assetPar.PrincipalCash;
				var prevAssetPar = prevAssetPars.FirstOrDefault(x => x.FundId == assetPar.FundId);
				var pp = (prevAssetPar?.AssetPar ?? 0) + (prevAssetPar?.PrincipalCash ?? 0);
				var diff = Math.Abs(ap - pp);
				var fund = funds.FirstOrDefault(x => x.FundId == assetPar.FundId);
				var restVal = fund?.AssetParThreshold ?? 0;
				var isOverThreshold = diff > restVal;
				var leftClickMsg = isOverThreshold
					? @"<br/><br/><p>NOTE:&nbsp;&nbsp;<em>left-click on cell to see an output of differences by loan.</em></p>"
					: string.Empty;
				var msg = $"&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;TODAY: {ap:C0}<br/>&nbsp;YESTERDAY: {pp:C0}<br/>DIFFERENCE: {diff:C0}{leftClickMsg}";
				id++;
				var result = new FundRestrictionResult
				{
					FundId = assetPar.FundId
					, RestrictionValue = restVal
					, OperatorId = 4
					, Id = id
					, FieldName = fieldName
					, IsDifferenceOverThreshold = isOverThreshold
					, DisplayColor = isOverThreshold ? "red" : string.Empty
					, FundRestrictionTypeId = difference?.FundRestrictionTypeId ?? (int)CustomFundRestrictions.CURRENTVSPREVIOUSASSPAR
					, FundRestrictionTypeName = difference?.FundRestrictionTypeName
					, FundRestrictionToolTip = msg
					, FieldTitle = fieldTitle
					, FieldId = fieldId
					, OperatorCode = string.Empty
					, OperatorVal = string.Empty
					, JsonPropertyName = jsonPropName
					, SortOrder = difference?.SortOrder ?? (int)CustomFundRestrictions.CURRENTVSPREVIOUSASSPAR
					, IsPercentage = false
				};

				restrictions.Add(result);
			}
			return restrictions;
		}

		[HttpPost]
        public JsonNetResult SaveFundRestrictions(IEnumerable<FundRestriction> fundRestrictions)
        {
            _repository.SaveFundRestrictions(fundRestrictions);
            return GetFundRestrictions(fundRestrictions.First().FundId);
        }


        protected override void Dispose(bool disposing)
        {
            _repository.Dispose();
            base.Dispose(disposing);
        }
    }
}