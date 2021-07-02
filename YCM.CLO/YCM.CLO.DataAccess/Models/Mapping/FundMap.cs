using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class FundMap : EntityTypeConfiguration<Fund>
    {
        public FundMap()
        {
            // Primary Key
            this.HasKey(t => t.FundId);

            // Properties
            this.Property(t => t.FundCode)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.FundDesc)
                .HasMaxLength(500);

            this.Property(t => t.CreatedBy)
                .HasMaxLength(100);

            this.Property(t => t.LastUpdatedBy)
                .HasMaxLength(100);

            this.Property(t => t.CLOFileName)
                .HasMaxLength(1000);

            // Table & Column Mappings
            this.ToTable(nameof(Fund), "CLO");
            this.Property(t => t.FundId).HasColumnName(nameof(Fund.FundId));
            this.Property(t => t.FundCode).HasColumnName(nameof(Fund.FundCode));
            this.Property(t => t.FundDesc).HasColumnName(nameof(Fund.FundDesc));
            this.Property(t => t.WSOLastUpdatedOn).HasColumnName(nameof(Fund.WSOLastUpdatedOn));
			this.Property(t => t.AssetParPercentageThreshold).HasColumnName(nameof(Fund.AssetParPercentageThreshold));

			this.Property(t => t.PrincipalCash).HasColumnName(nameof(Fund.PrincipalCash));
            this.Property(t => t.LiabilityPar).HasColumnName(nameof(Fund.LiabilityPar));
            this.Property(t => t.EquityPar).HasColumnName(nameof(Fund.EquityPar));
            this.Property(t => t.TargetPar).HasColumnName(nameof(Fund.TargetPar));
            this.Property(t => t.RecoveryMultiplier).HasColumnName(nameof(Fund.RecoveryMultiplier));

            this.Property(t => t.CreatedOn).HasColumnName(nameof(Fund.CreatedOn));
            this.Property(t => t.CreatedBy).HasColumnName(nameof(Fund.CreatedBy));
            this.Property(t => t.LastUpdatedOn).HasColumnName(nameof(Fund.LastUpdatedOn));
            this.Property(t => t.LastUpdatedBy).HasColumnName(nameof(Fund.LastUpdatedBy));
        }
    }
}
