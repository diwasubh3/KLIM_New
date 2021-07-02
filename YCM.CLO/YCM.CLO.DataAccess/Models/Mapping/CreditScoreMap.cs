using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class CreditScoreMap : EntityTypeConfiguration<CreditScore>
	{
		public CreditScoreMap()
		{
			// Primary Key
			HasKey(t => t.Id);

			// Table & Column Mappings
			ToTable(nameof(CreditScore), "CLO");
			Property(t => t.Id).HasColumnName(nameof(CreditScore.Id));
			Property(t => t.ScoreDescription).HasColumnName(nameof(CreditScore.ScoreDescription));

		}
	}
}
