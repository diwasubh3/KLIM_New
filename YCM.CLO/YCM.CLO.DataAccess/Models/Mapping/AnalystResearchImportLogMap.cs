using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class AnalystResearchImportLogMap : EntityTypeConfiguration<AnalystResearchImportLog>
	{
		public AnalystResearchImportLogMap()
		{
			// Primary Key
			HasKey(t => t.AnalystResearchImportLogId);

			// Table & Column Mappings
			ToTable(nameof(AnalystResearchImportLog), "CLO");
			Property(t => t.AnalystResearchImportLogId)
				.HasColumnName(nameof(AnalystResearchImportLog.AnalystResearchImportLogId));
			Property(t => t.AnalystResearchFileName).HasColumnName(nameof(AnalystResearchImportLog.AnalystResearchFileName));
			Property(t => t.ImportError).HasColumnName(nameof(AnalystResearchImportLog.ImportError));
			Property(t => t.IssuerName).HasColumnName(nameof(AnalystResearchImportLog.IssuerName));
			Property(t => t.Processed).HasColumnName(nameof(AnalystResearchImportLog.Processed));
			Property(t => t.SheetName).HasColumnName(nameof(AnalystResearchImportLog.SheetName));
			Property(t => t.CreatedOn).HasColumnName(nameof(AnalystResearchImportLog.CreatedOn));
			Property(t => t.CreatedBy).HasColumnName(nameof(AnalystResearchImportLog.CreatedBy));

		}
	}
}
