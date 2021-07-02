using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
    public class FundCalculationMap : EntityTypeConfiguration<FundCalculation>
    {
        public FundCalculationMap()
        {
            // Primary Key
            this.HasKey(t => new { t.DateId, t.FundId });

            // Properties
            this.Property(t => t.FundCalculationId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.DateId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.FundId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CreatedBy)
                .HasMaxLength(100);

            this.Property(t => t.LastUpdatedBy)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("FundCalculation", "CLO");
            

            // Relationships
            this.HasRequired(t => t.Fund)
                .WithMany(t => t.FundCalculations)
                .HasForeignKey(d => d.FundId);
            
        }
    }
}
