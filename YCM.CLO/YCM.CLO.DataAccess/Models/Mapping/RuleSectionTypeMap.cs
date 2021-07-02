using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class RuleSectionTypeMap : EntityTypeConfiguration<RuleSectionType>
    {
        public RuleSectionTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.RuleSectionTypeId);

            // Properties
            this.Property(t => t.RuleSectionName)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("RuleSectionType", "CLO");
            this.Property(t => t.RuleSectionTypeId).HasColumnName("RuleSectionTypeId");
            this.Property(t => t.RuleSectionName).HasColumnName("RuleSectionName");
        }
    }
}
