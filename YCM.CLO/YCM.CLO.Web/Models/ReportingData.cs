using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YCM.CLO.DataAccess.Models;
using YCM.CLO.DTO;

namespace YCM.CLO.Web.Models
{
    public class ReportingData
    {
        public IEnumerable<AssetClass> AssetClasses { get; set; }

        public IEnumerable<FundAssetClass> FundAssetClasses { get; set; }

        public IEnumerable<FundDto> Funds { get; set; }

        public IEnumerable<RatingDto> Ratings { get; set; }


        public IEnumerable<EquityOverride> EquityOverrides { get; set; }

        public IEnumerable<DefaultSecurity> DefaultSecurities { get; set; }


    }
}