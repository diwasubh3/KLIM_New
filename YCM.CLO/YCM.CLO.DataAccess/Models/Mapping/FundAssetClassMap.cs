using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YCM.CLO.DataAccess.Models.Mapping
{
    public class FundAssetClassMap : EntityTypeConfiguration<FundAssetClass>
    {
        public FundAssetClassMap()
        {
            HasKey(t => t.Id);

            
            // Table & Column Mappings
            ToTable("FundAssetClass", "CLO");

            Property(t => t.Spread).HasPrecision(38, 10);
            Property(t => t.OverrideCalcSpread).HasPrecision(38, 10);
        }
    }
}
