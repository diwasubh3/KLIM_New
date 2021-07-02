using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YCM.CLO.DataAccess.Models;

namespace YCM.CLO.Web.Models
{
    public class MismatchData
    {
        public List<vw_Mismatch> Cfrs { get; set; }

        public List<vw_Mismatch> CfrAdjs { get; set; }

        public List<vw_Mismatch> Facilities { get; set; }


        
    }
}