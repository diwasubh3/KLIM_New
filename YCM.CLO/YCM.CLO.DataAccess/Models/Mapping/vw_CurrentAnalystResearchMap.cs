using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
    public class vw_CurrentAnalystResearchMap : EntityTypeConfiguration<vw_CurrentAnalystResearch>
    {
        public vw_CurrentAnalystResearchMap()
        {
			// Primary Key
			HasKey(t => new { t.AnalystResearchId});

			// Properties
			Property(t => t.AnalystResearchId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

			Property(t => t.IssuerId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

			Property(t => t.CLOAnalystUserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

			Property(t => t.CLOAnalyst)
                .HasMaxLength(100);

			Property(t => t.HFAnalyst)
                .HasMaxLength(100);

			Property(t => t.IssuerDesc)
                .IsRequired()
                .HasMaxLength(100);

			// Table & Column Mappings
			ToTable(nameof(vw_CurrentAnalystResearch), "CLO");
			Property(t => t.AnalystResearchId).HasColumnName(nameof(vw_CurrentAnalystResearch.AnalystResearchId));
			Property(t => t.IssuerId).HasColumnName(nameof(vw_CurrentAnalystResearch.IssuerId));
			Property(t => t.CLOAnalystUserId).HasColumnName(nameof(vw_CurrentAnalystResearch.CLOAnalystUserId));
			Property(t => t.HFAnalystUserId).HasColumnName(nameof(vw_CurrentAnalystResearch.HFAnalystUserId));
			Property(t => t.CLOAnalyst).HasColumnName(nameof(vw_CurrentAnalystResearch.CLOAnalyst));
			Property(t => t.HFAnalyst).HasColumnName(nameof(vw_CurrentAnalystResearch.HFAnalyst));
			Property(t => t.AsOfDate).HasColumnName(nameof(vw_CurrentAnalystResearch.AsOfDate));
			Property(t => t.CreditScore).HasColumnName(nameof(vw_CurrentAnalystResearch.CreditScore));
			Property(t => t.OneLLeverage).HasColumnName(nameof(vw_CurrentAnalystResearch.OneLLeverage));
			Property(t => t.TotalLeverage).HasColumnName(nameof(vw_CurrentAnalystResearch.TotalLeverage));
			Property(t => t.EVMultiple).HasColumnName(nameof(vw_CurrentAnalystResearch.EVMultiple));
			Property(t => t.LTMRevenues).HasColumnName(nameof(vw_CurrentAnalystResearch.LTMRevenues));
			Property(t => t.LTMEBITDA).HasColumnName(nameof(vw_CurrentAnalystResearch.LTMEBITDA));
			Property(t => t.FCF).HasColumnName(nameof(vw_CurrentAnalystResearch.FCF));
			Property(t => t.Comments).HasColumnName(nameof(vw_CurrentAnalystResearch.Comments));
			Property(t => t.IssuerDesc).HasColumnName(nameof(vw_CurrentAnalystResearch.IssuerDesc));
        }
    }
}
