using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class AnalystResearchRowLocationMap : EntityTypeConfiguration<AnalystResearchRowLocation>
	{
		public AnalystResearchRowLocationMap()
		{
			// Primary Key
			HasKey(t => t.AnalystResearchRowLocationId);

			// Table & Column Mappings
			ToTable(nameof(AnalystResearchRowLocation), "CLO");
			Property(t => t.AnalystResearchRowLocationId)
				.HasColumnName(nameof(AnalystResearchRowLocation.AnalystResearchRowLocationId));
			Property(t => t.ClassName).HasColumnName(nameof(AnalystResearchRowLocation.ClassName));
			Property(t => t.PropertyName).HasColumnName(nameof(AnalystResearchRowLocation.PropertyName));
			Property(t => t.RowIndex).HasColumnName(nameof(AnalystResearchRowLocation.RowIndex));
			Property(t => t.CreatedOn).HasColumnName(nameof(AnalystResearchRowLocation.CreatedOn));
			Property(t => t.CreatedBy).HasColumnName(nameof(AnalystResearchRowLocation.CreatedBy));
			Property(t => t.LastUpdatedOn).HasColumnName(nameof(AnalystResearchRowLocation.LastUpdatedOn));
			Property(t => t.LastUpdatedBy).HasColumnName(nameof(AnalystResearchRowLocation.LastUpdatedBy));

		}
	}
}
