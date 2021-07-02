using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class FundRestrictionMap : EntityTypeConfiguration<FundRestriction>
    {
        public FundRestrictionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("FundRestriction", "CLO");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.FundId).HasColumnName("FundId");
            this.Property(t => t.FundRestrictionTypeId).HasColumnName("FundRestrictionTypeId");
            this.Property(t => t.FieldId).HasColumnName("FieldId");
            this.Property(t => t.OperatorId).HasColumnName("OperatorId");
            this.Property(t => t.RestrictionValue).HasColumnName("RestrictionValue");

            // Relationships
            this.HasRequired(t => t.Field)
                .WithMany(t => t.FundRestrictions)
                .HasForeignKey(d => d.FieldId);
            this.HasRequired(t => t.Fund)
                .WithMany(t => t.FundRestrictions)
                .HasForeignKey(d => d.FundId);
            this.HasRequired(t => t.FundRestrictionType)
                .WithMany(t => t.FundRestrictions)
                .HasForeignKey(d => d.FundRestrictionTypeId);
            this.HasRequired(t => t.Operator)
                .WithMany(t => t.FundRestrictions)
                .HasForeignKey(d => d.OperatorId);

        }
    }
}
