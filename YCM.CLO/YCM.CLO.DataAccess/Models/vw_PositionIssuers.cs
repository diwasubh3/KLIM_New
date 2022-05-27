using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YCM.CLO.DataAccess.Models
{
    public class vw_PositionIssuers
    {
        public vw_PositionIssuers()
        {
        }
        [System.ComponentModel.DataAnnotations.Key]
        public int IssuerId { get; set; }
        public string Issuer { get; set; }
        public Int16 FacilityId { get; set; }
        public string FacilityDesc { get; set; }
        public decimal? Exposure { get; set; }
        public string MaturityDate { get; set; }
        public decimal? Spread { get; set; }
    }
}
