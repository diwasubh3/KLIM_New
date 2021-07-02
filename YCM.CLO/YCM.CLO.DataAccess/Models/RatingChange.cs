using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YCM.CLO.DataAccess.Models
{
    public class RatingChange
    {
        public string SecurityCode { get; set; }
        public string Issuer { get; set; }
        public string TotalPar { get; set; }
        public string MoodyCFJAdjNew { get; set; }
        public string MoodyCFRAdjOld { get; set; }
        public string MoodyFacitityRatingNew { get; set; }
        public string MoodyFacitityRatingOld { get; set; }
        public string SnPIssuerRatingNew { get; set; }
        public string SnPIssuerRatingOld { get; set; }
    }
}
