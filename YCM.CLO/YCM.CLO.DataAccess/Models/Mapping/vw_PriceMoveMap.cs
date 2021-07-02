using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class vw_PriceMoveMap : EntityTypeConfiguration<vw_PriceMove>
    {
        public vw_PriceMoveMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Issuer, t.Bid,t.PriceMove});

            // Properties
            // Table & Column Mappings
            this.ToTable("vw_PriceMove", "CLO");
        }
    }
}
