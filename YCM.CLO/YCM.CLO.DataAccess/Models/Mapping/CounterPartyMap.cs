using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
    public class CounterPartyMap: EntityTypeConfiguration<CounterParty>
    {
        public CounterPartyMap()
        {
            // Primary Key
            this.HasKey(t => t.PartyId);

            // Properties
            this.Property(t => t.PartyName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.MEI)
                .HasMaxLength(100);

            this.Property(t => t.CreatedBy)
                .HasMaxLength(100);

            this.Property(t => t.LastUpdatedBy)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("CounterParty", "CLO");
            this.Property(t => t.PartyId).HasColumnName("PartyId");
            this.Property(t => t.PartyName).HasColumnName("PartyName");
            this.Property(t => t.WSOId).HasColumnName("WSOId");
            this.Property(t => t.MEI).HasColumnName("MEI");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.LastUpdatedBy).HasColumnName("LastUpdatedBy");
        }
    }
}
