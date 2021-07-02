using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class vw_YorkCoreGenevaAnalystMap : EntityTypeConfiguration<vw_YorkCoreGenevaAnalyst>
	{
		public vw_YorkCoreGenevaAnalystMap()
		{
			// Primary Key
			HasKey(t => t.AnalystId);

			// Table & Column Mappings
			ToTable(nameof(vw_YorkCoreGenevaAnalyst), "CLO");
			Property(t => t.AnalystId)
				.HasColumnName(nameof(vw_YorkCoreGenevaAnalyst.AnalystId));
			Property(t => t.AnalystCode).HasColumnName(nameof(vw_YorkCoreGenevaAnalyst.AnalystCode));
			Property(t => t.AnalystDesc).HasColumnName(nameof(vw_YorkCoreGenevaAnalyst.AnalystDesc));
			Property(t => t.AnalystTypeId).HasColumnName(nameof(vw_YorkCoreGenevaAnalyst.AnalystTypeId));
			Property(t => t.AppUserId).HasColumnName(nameof(vw_YorkCoreGenevaAnalyst.AppUserId));
		}
	}
}
