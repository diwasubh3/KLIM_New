using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class RuleFieldMap : EntityTypeConfiguration<RuleField>
    {
        public RuleFieldMap()
        {
            // Primary Key
            this.HasKey(t => t.RuleFieldId);

            // Properties
            // Table & Column Mappings
            this.ToTable("RuleField", "CLO");
            this.Property(t => t.RuleFieldId).HasColumnName("RuleFieldId");
            this.Property(t => t.RuleId).HasColumnName("RuleId");
            this.Property(t => t.RuleSectionTypeId).HasColumnName("RuleSectionTypeId");
            this.Property(t => t.FieldId).HasColumnName("FieldId");
            this.Property(t => t.SortOrder).HasColumnName("SortOrder");

            // Relationships
            this.HasRequired(t => t.Field)
                .WithMany(t => t.RuleFields)
                .HasForeignKey(d => d.FieldId);
            this.HasRequired(t => t.Rule)
                .WithMany(t => t.RuleFields)
                .HasForeignKey(d => d.RuleId);
            this.HasOptional(t => t.RuleSectionType)
                .WithMany(t => t.RuleFields)
                .HasForeignKey(d => d.RuleSectionTypeId);

        }
    }
}
