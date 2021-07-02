using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
    public class vw_PriceMap : EntityTypeConfiguration<vw_Price>
    {
        public vw_PriceMap()
        {
            // Primary Key
            this.HasKey(t => new { t.DateId, t.SecurityId, t.Bid, t.Offer });

            // Properties

            this.Property(t => t.DateId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.SecurityId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Bid)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Offer)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);


            // Table & Column Mappings
            this.ToTable("vw_Price", "CLO");
            
            this.Property(t => t.DateId).HasColumnName("DateId");
            this.Property(t => t.SecurityId).HasColumnName("SecurityId");
            this.Property(t => t.Bid).HasColumnName("Bid");
            this.Property(t => t.Offer).HasColumnName("Offer");
        }
    }
}
