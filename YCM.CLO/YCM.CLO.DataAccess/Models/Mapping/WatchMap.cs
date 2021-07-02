using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
	public class WatchMap : EntityTypeConfiguration<Watch>
    {
        public WatchMap()
        {
            // Primary Key
            this.HasKey(t => t.WatchId);

            // Properties
            this.Property(t => t.WatchComments)
                .HasMaxLength(500);

            this.Property(t => t.WatchUser)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Watch", "CLO");
            this.Property(t => t.WatchId).HasColumnName("WatchId");
            this.Property(t => t.WatchObjectTypeId).HasColumnName("WatchObjectTypeId");
            this.Property(t => t.WatchObjectId).HasColumnName("WatchObjectId");
            this.Property(t => t.WatchComments).HasColumnName("WatchComments");
            this.Property(t => t.WatchUser).HasColumnName("WatchUser");
            this.Property(t => t.WatchLastUpdatedOn).HasColumnName("WatchLastUpdatedOn");
        }
    }
}
