using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
    public class vw_AnalystResearchMap : EntityTypeConfiguration<vw_AnalystResearch>
    {
        public vw_AnalystResearchMap()
        {
            // Primary Key
            this.HasKey(t => new { t.AnalystResearchId});

            // Properties
            this.Property(t => t.AnalystResearchId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.IssuerId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CLOAnalystUserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CLOAnalyst)
                .HasMaxLength(100);

            this.Property(t => t.HFAnalyst)
                .HasMaxLength(100);

            this.Property(t => t.IssuerDesc)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("vw_AnalystResearch", "CLO");
            this.Property(t => t.AnalystResearchId).HasColumnName("AnalystResearchId");
            this.Property(t => t.IssuerId).HasColumnName("IssuerId");
            this.Property(t => t.CLOAnalystUserId).HasColumnName("CLOAnalystUserId");
            this.Property(t => t.HFAnalystUserId).HasColumnName("HFAnalystUserId");
            this.Property(t => t.CLOAnalyst).HasColumnName("CLOAnalyst");
            this.Property(t => t.HFAnalyst).HasColumnName("HFAnalyst");
            this.Property(t => t.AsOfDate).HasColumnName("AsOfDate");
            this.Property(t => t.CreditScore).HasColumnName("CreditScore");
            this.Property(t => t.OneLLeverage).HasColumnName("OneLLeverage");
            this.Property(t => t.TotalLeverage).HasColumnName("TotalLeverage");
            this.Property(t => t.EVMultiple).HasColumnName("EVMultiple");
            this.Property(t => t.LTMRevenues).HasColumnName("LTMRevenues");
            this.Property(t => t.LTMEBITDA).HasColumnName("LTMEBITDA");
            this.Property(t => t.FCF).HasColumnName("FCF");
            this.Property(t => t.Comments).HasColumnName("Comments");
            this.Property(t => t.IssuerDesc).HasColumnName("IssuerDesc");
        }
    }
}
