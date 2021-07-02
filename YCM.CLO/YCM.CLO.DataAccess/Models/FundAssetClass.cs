using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YCM.CLO.DataAccess.Models
{

    public class FundAssetClass
    {
        [Key]
        public int Id { get; set; }

        public int FundId { get; set; }

 
        public int AssetClassId { get; set; }


        public decimal? Notional { get; set; }


        public decimal? Spread { get; set; }


        public decimal? Libor { get; set; }

        public DateTime? StartDate { get; set; }


        public DateTime? EndDate { get; set; }

        public Int16? MoodyRatingId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModifiedOn { get; set; }

        
        public decimal? OverrideCalcSpread { get; set; }
    }
}
