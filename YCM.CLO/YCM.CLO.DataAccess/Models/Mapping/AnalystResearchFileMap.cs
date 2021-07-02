using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class AnalystResearchFileMap : EntityTypeConfiguration<AnalystResearchFile>
	{
		public AnalystResearchFileMap()
		{
			// Primary Key
			HasKey(t => t.AnalystResearchFileId);

			// Table & Column Mappings
			ToTable(nameof(AnalystResearchFile), "CLO");
			Property(t => t.AnalystResearchFileId)
				.HasColumnName(nameof(AnalystResearchFile.AnalystResearchFileId));
			Property(t => t.AnalystResearchFileName).HasColumnName(nameof(AnalystResearchFile.AnalystResearchFileName));
			Property(t => t.LastFileUpdate).HasColumnName(nameof(AnalystResearchFile.LastFileUpdate));
			Property(t => t.CreatedOn).HasColumnName(nameof(AnalystResearchFile.CreatedOn));
			Property(t => t.CreatedBy).HasColumnName(nameof(AnalystResearchFile.CreatedBy));
			Property(t => t.LastUpdatedOn).HasColumnName(nameof(AnalystResearchFile.LastUpdatedOn));
			Property(t => t.LastUpdatedBy).HasColumnName(nameof(AnalystResearchFile.LastUpdatedBy));

		}
	}
}
