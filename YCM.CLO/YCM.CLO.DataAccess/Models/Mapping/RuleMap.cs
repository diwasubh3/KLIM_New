using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class RuleMap : EntityTypeConfiguration<Rule>
    {
        public RuleMap()
        {
            // Primary Key
            this.HasKey(t => t.RuleId);

            // Properties
            this.Property(t => t.RuleName)
                .HasMaxLength(100);

            this.Property(t => t.ExecutionStoredProcedure)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Rule", "CLO");
            this.Property(t => t.RuleId).HasColumnName("RuleId");
            this.Property(t => t.RuleName).HasColumnName("RuleName");
            this.Property(t => t.ExecutionStoredProcedure).HasColumnName("ExecutionStoredProcedure");
            this.Property(t => t.SortOrder).HasColumnName("SortOrder");
        }
    }
}
