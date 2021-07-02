using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class TradeSwapMap : EntityTypeConfiguration<TradeSwap>
    {
        public TradeSwapMap()
        {
            // Primary Key
            this.HasKey(t => t.TradeSwapId);

            // Properties
            this.Property(t => t.CreatedBy)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TradeSwap", "CLO");
            this.Property(t => t.TradeSwapId).HasColumnName("TradeSwapId");
            this.Property(t => t.Parameters).HasColumnName("Parameters");
            
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.Error).HasColumnName("Error");

            this.Property(t => t.ProcessStartedOn).HasColumnName("ProcessStartedOn");
            this.Property(t => t.ProcessCompletedOn).HasColumnName("ProcessCompletedOn");

        }
    }
}
