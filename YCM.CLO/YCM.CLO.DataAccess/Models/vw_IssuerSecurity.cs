using System;
namespace YCM.CLO.DataAccess.Models
{
    public class vw_IssuerSecurity
    {
        public vw_IssuerSecurity()
        {
        }
        [System.ComponentModel.DataAnnotations.Key]
        public int SecurityId { get; set; }
        public string SecurityCode { get; set; }
        public string SecurityDesc { get; set; }
        public int IssuerId { get; set; }
        public string Issuer { get; set; }
        public Int16 FacilityId { get; set; }
        public string FacilityDesc { get; set; }
        public decimal? Exposure { get; set; }
        public string MaturityDate { get; set; }
        public string PrimarySpreadType { get; set; }
        public string LiborBaseRate { get; set; }
        public string SecurityFilter { get; set; }        
    }
}
