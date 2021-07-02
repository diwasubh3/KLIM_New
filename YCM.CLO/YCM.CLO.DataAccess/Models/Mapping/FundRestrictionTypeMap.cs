using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class FundRestrictionTypeMap : EntityTypeConfiguration<FundRestrictionType>
    {
        public FundRestrictionTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.FundRestrictionTypeId);

            // Properties
            this.Property(t => t.FundRestrictionTypeName)
                .HasMaxLength(100);

            this.Property(t => t.DisplayColor)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("FundRestrictionType", "CLO");
            this.Property(t => t.FundRestrictionTypeId).HasColumnName("FundRestrictionTypeId");
            this.Property(t => t.FundRestrictionTypeName).HasColumnName("FundRestrictionTypeName");
            this.Property(t => t.DisplayColor).HasColumnName("DisplayColor");
            this.Property(t => t.SortOrder).HasColumnName("SortOrder");
        }
    }
}
