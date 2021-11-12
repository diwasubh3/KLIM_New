using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
    public class vw_ReinvestDetailsMap : EntityTypeConfiguration<vw_ReinvestDetails>
    {
        public vw_ReinvestDetailsMap()
        {
            // Table & Column Mappings
            this.ToTable("vw_ReinvestDetails", "CLO");
            this.Property(t => t.id).HasColumnName("ID");
            this.Property(t => t.FundId).HasColumnName("FundID");
            this.Property(t => t.FundCode).HasColumnName("FundCode");
            this.Property(t => t.FilePath).HasColumnName("FilePath");
            this.Property(t => t.FileName).HasColumnName("FileName");
            this.Property(t => t.SheetName).HasColumnName("SheetName");
            this.Property(t => t.FieldLocation).HasColumnName("FieldLocation");
        }
    }
}
