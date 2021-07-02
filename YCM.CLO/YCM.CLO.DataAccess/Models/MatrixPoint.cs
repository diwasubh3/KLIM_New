using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YCM.CLO.DataAccess.Models
{
    public class MatrixPoint
    {
        public int Id { get; set; }

        public int FundId { get; set; }

        public decimal? Spread { get; set; }

        public decimal? Diversity { get; set; }

        public decimal? Warf { get; set; }

        public decimal? WarfModifier { get; set; }

        public string CreatedBy { get; set; }

        public DateTime?  CreatedOn { get; set; }

        public Int16? DataPointType { get; set; }

        public decimal? TopMajorSpread { get; set; }

        public decimal? BottomMajorSpread { get; set; }

        public decimal? LeftMajorDiversity { get; set; }

        public decimal? RightMajorDiversity { get; set; }


        public decimal? TopSpread { get; set; }

        public decimal? BottomSpread { get; set; }

        public decimal? LeftDiversity { get; set; }

        public decimal? RightDiversity { get; set; }

        public string CreatedOnString { get {
                return CreatedOn.HasValue ? CreatedOn.Value.ToString("MM/dd/yyyy hh:mm tt") : string.Empty;
            }
        }


    }
}
