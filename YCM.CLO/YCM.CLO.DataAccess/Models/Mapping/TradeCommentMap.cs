using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
    public class TradeCommentMap : EntityTypeConfiguration<TradeComment>
    {
        public TradeCommentMap()
        {
            // Primary Key
            this.HasKey(t => t.CommentId);

            // Properties
            this.Property(t => t.Comment)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.CreatedBy)
                .HasMaxLength(100);

            this.Property(t => t.LastUpdatedBy)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TradeComment", "CLO");
            this.Property(t => t.CommentId).HasColumnName("CommentId");
            this.Property(t => t.Comment).HasColumnName("Comment");
            this.Property(t => t.IsActive).HasColumnName("IsActive");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.LastUpdatedOn).HasColumnName("LastUpdatedOn");
            this.Property(t => t.LastUpdatedBy).HasColumnName("LastUpdatedBy");
        }
    }
}
