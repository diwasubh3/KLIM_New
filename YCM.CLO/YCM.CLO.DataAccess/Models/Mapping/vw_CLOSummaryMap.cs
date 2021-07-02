using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
    public class vw_CLOSummaryMap : EntityTypeConfiguration<vw_CLOSummary>
    {
        public vw_CLOSummaryMap()
        {
			// Primary Key
			HasKey(t => new { t.FundCode, t.DateId, t.FundId });

			// Properties
			Property(t => t.FundCode)
                .IsRequired()
                .HasMaxLength(100);

			Property(t => t.DateId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

			Property(t => t.FundId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

			// Table & Column Mappings
			ToTable(nameof(vw_CLOSummary), "CLO");

			Ignore(p => p.PositionDate);

            Ignore(p => p.SpreadDiff);
        }
    }
}
