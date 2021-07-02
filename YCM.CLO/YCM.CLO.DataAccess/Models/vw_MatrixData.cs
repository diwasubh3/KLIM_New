using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YCM.CLO.DataAccess.Models
{
    public class vw_MatrixData
    {
        public int Id { get; set; }

        public string FundCode { get; set; }

        public decimal? Spread { get; set; }

        public decimal? Diversity { get; set; }

        public decimal? Warf { get; set; }

        public decimal? WarfModifier { get; set; }

        public string DataPoint { get; set; }

        public string Interpolation { get; set; }

        public int FundId { get; set; }

        public Int16? DataPointType { get; set; }

    }
}
