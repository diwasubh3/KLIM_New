using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
    public class PositionMap : EntityTypeConfiguration<Position>
    {
        public PositionMap()
        {
            // Primary Key
            this.HasKey(t => new { t.FundId, t.SecurityId, t.DateId });

            // Properties
            this.Property(t => t.PositionId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.FundId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.SecurityId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.DateId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CreatedBy)
                .HasMaxLength(100);

            this.Property(t => t.LastUpdatedBy)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Position", "CLO");
            this.Property(t => t.PositionId).HasColumnName("PositionId");
            this.Property(t => t.FundId).HasColumnName("FundId");
            this.Property(t => t.SecurityId).HasColumnName("SecurityId");
            this.Property(t => t.DateId).HasColumnName("DateId");
            this.Property(t => t.Exposure).HasColumnName("Exposure");
            this.Property(t => t.PctExposure).HasColumnName("PctExposure");
            this.Property(t => t.PxPrice).HasColumnName("PxPrice");
            this.Property(t => t.IsCovLite).HasColumnName("IsCovLite");
            this.Property(t => t.CountryId).HasColumnName("CountryId");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.LastUpdatedBy).HasColumnName("LastUpdatedBy");

            // Relationships
            this.HasOptional(t => t.Country)
                .WithMany(t => t.Positions)
                .HasForeignKey(d => d.CountryId);
            this.HasRequired(t => t.Fund)
                .WithMany(t => t.Positions)
                .HasForeignKey(d => d.FundId);
            this.HasRequired(t => t.Security)
                .WithMany(t => t.Positions)
                .HasForeignKey(d => d.SecurityId);

        }
    }
}
