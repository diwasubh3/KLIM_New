using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YCM.CLO.DataAccess.Models
{
    public class vw_IssuerSecurity
    {
        public vw_IssuerSecurity()
        {
        }
        public long Id { get; set; }
        public string SecurityCode { get; set; }
        public string SecurityDesc { get; set; }
        public int IssuerId { get; set; }
        public string Issuer { get; set; }
        public string Facility { get; set; }
        public decimal? Exposure { get; set; }
        public string MaturityDate { get; set; }
        public string PrimarySpreadType { get; set; }
        public string LiborBaseRate { get; set; }
        public string SecurityFilter { get; set; }
    }
}
