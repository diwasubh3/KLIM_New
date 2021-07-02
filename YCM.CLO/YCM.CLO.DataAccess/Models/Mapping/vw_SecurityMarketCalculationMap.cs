using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
    public class vw_SecurityMarketCalculationMap : EntityTypeConfiguration<vw_SecurityMarketCalculation>
    {
        public vw_SecurityMarketCalculationMap()
        {
            // Primary Key
            this.HasKey(t => new { t.SecurityId, t.SecurityCode, t.IsFloating, t.IssuerId, t.OrigSecurityCode, t.OrigBBGId, t.OrigIsFloating });

            // Properties
            this.Property(t => t.SecurityId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.SecurityCode)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.SecurityDesc)
                .HasMaxLength(100);

            this.Property(t => t.BBGId)
                .HasMaxLength(100);

            this.Property(t => t.Issuer)
                .HasMaxLength(100);

            this.Property(t => t.Facility)
                .HasMaxLength(100);

            this.Property(t => t.SnpIndustry)
                .HasMaxLength(100);

            this.Property(t => t.MoodyIndustry)
                .HasMaxLength(100);

            this.Property(t => t.LienType)
                .HasMaxLength(100);

            this.Property(t => t.IssuerId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.WatchComments)
                .HasMaxLength(500);

            this.Property(t => t.WatchLastUpdatedOn)
                .HasMaxLength(4000);

            this.Property(t => t.WatchUser)
                .HasMaxLength(100);

            this.Property(t => t.OrigSecurityCode)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.OrigSecurityDesc)
                .HasMaxLength(500);

            this.Property(t => t.OrigBBGId)
                .IsRequired()
                .HasMaxLength(1000);

            this.Property(t => t.OrigIssuer)
                .HasMaxLength(100);

            this.Property(t => t.GICSIndustry)
                .HasMaxLength(500);

            this.Property(t => t.OrigFacility)
                .HasMaxLength(100);

            this.Property(t => t.OrigCallDate)
                .HasMaxLength(10);

            this.Property(t => t.OrigMaturityDate)
                .HasMaxLength(10);

            this.Property(t => t.OrigSnpIndustry)
                .HasMaxLength(100);

            this.Property(t => t.OrigMoodyIndustry)
                .HasMaxLength(100);

            this.Property(t => t.OrigIsFloating)
                .IsRequired()
                .HasMaxLength(8);

            this.Property(t => t.OrigLienType)
                .HasMaxLength(100);

            this.Property(t => t.SecurityLastUpdatedBy)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("vw_SecurityMarketCalculation", "CLO");
            this.Property(t => t.SecurityId).HasColumnName("SecurityId");
            this.Property(t => t.SecurityCode).HasColumnName("SecurityCode");
            this.Property(t => t.SecurityDesc).HasColumnName("SecurityDesc");
            this.Property(t => t.BBGId).HasColumnName("BBGId");
            this.Property(t => t.Issuer).HasColumnName("Issuer");
            this.Property(t => t.Facility).HasColumnName("Facility");
            this.Property(t => t.CallDate).HasColumnName("CallDate");
            this.Property(t => t.MaturityDate).HasColumnName("MaturityDate");
            this.Property(t => t.SnpIndustry).HasColumnName("SnpIndustry");
            this.Property(t => t.MoodyIndustry).HasColumnName("MoodyIndustry");
            this.Property(t => t.IsFloating).HasColumnName("IsFloating");
            this.Property(t => t.LienType).HasColumnName("LienType");
            
            this.Property(t => t.IssuerId).HasColumnName("IssuerId");
            this.Property(t => t.WatchId).HasColumnName("WatchId");
            this.Property(t => t.IsOnWatch).HasColumnName("IsOnWatch");
            this.Property(t => t.WatchObjectTypeId).HasColumnName("WatchObjectTypeId");
            this.Property(t => t.WatchObjectId).HasColumnName("WatchObjectId");
            this.Property(t => t.WatchComments).HasColumnName("WatchComments");
            this.Property(t => t.WatchLastUpdatedOn).HasColumnName("WatchLastUpdatedOn");
            this.Property(t => t.WatchUser).HasColumnName("WatchUser");
            this.Property(t => t.OrigSecurityCode).HasColumnName("OrigSecurityCode");
            this.Property(t => t.OrigSecurityDesc).HasColumnName("OrigSecurityDesc");
            this.Property(t => t.OrigBBGId).HasColumnName("OrigBBGId");
            this.Property(t => t.OrigIssuer).HasColumnName("OrigIssuer");
            this.Property(t => t.GICSIndustry).HasColumnName("GICSIndustry");
            this.Property(t => t.OrigFacility).HasColumnName("OrigFacility");
            this.Property(t => t.OrigCallDate).HasColumnName("OrigCallDate");
            this.Property(t => t.OrigMaturityDate).HasColumnName("OrigMaturityDate");
            this.Property(t => t.OrigSnpIndustry).HasColumnName("OrigSnpIndustry");
            this.Property(t => t.OrigMoodyIndustry).HasColumnName("OrigMoodyIndustry");
            this.Property(t => t.OrigIsFloating).HasColumnName("OrigIsFloating");
            this.Property(t => t.OrigLienType).HasColumnName("OrigLienType");
            
            this.Property(t => t.SecurityLastUpdatedOn).HasColumnName("SecurityLastUpdatedOn");
            this.Property(t => t.SecurityLastUpdatedBy).HasColumnName("SecurityLastUpdatedBy");
            this.Property(t => t.SourceId).HasColumnName("SourceId");
        }
    }
}
