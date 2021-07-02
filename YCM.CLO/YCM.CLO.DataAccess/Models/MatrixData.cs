using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YCM.CLO.DataAccess.Models
{
    public class MatrixData
    {
        public int Id { get; set; }

        public int FundId { get; set; }

        public decimal? Spread { get; set; }

        public decimal? Diversity { get; set; }

        public decimal? Warf { get; set; }

        public decimal? WarfModifier { get; set; }

        public Int16? DataPointType { get; set; }

        public Int16? InterpolationType { get; set; }

        public int? FromMajorMatrixDataId { get; set; }

        public int? ToMajorMatrixDataId { get; set; }

        public override string ToString()
        {
            return "(" + FundId + "," + Spread + "," + Diversity + "," + Warf + "," + WarfModifier + "," + DataPointType + "," + (InterpolationType??0) + "," + (FromMajorMatrixDataId ?? 0) + "," + (ToMajorMatrixDataId ?? 0) + ")";
        }
    }
}
