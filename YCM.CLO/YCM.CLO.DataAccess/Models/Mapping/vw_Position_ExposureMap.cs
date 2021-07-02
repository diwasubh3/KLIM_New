using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
    public class vw_Position_ExposureMap : EntityTypeConfiguration<vw_Position_Exposure>
    {
        public vw_Position_ExposureMap()
        {
            // Primary Key
            this.HasKey(t => new { t.SecurityId, t.FundId});

            // Properties
            this.Property(t => t.PositionId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);



            this.Property(t => t.Exposure)
                .HasMaxLength(4000);


            this.Property(t => t.FundCode)
                .HasMaxLength(100);

            this.Property(t => t.FundDesc)
                .HasMaxLength(500);

            this.Property(t => t.SecurityCode)
                .HasMaxLength(100);



  

            this.Property(t => t.Bid)
                .HasMaxLength(30);

            this.Property(t => t.Offer)
                .HasMaxLength(30);


            this.Property(t => t.TotalPar)
                .HasMaxLength(4000);

           

            // Table & Column Mappings
            this.ToTable("vw_Position_Exposure", "CLO");
            this.Property(t => t.PositionId).HasColumnName("PositionId");
            
            this.Property(t => t.Exposure).HasColumnName("Exposure");
            this.Property(t => t.NumExposure).HasColumnName("NumExposure");
            
            this.Property(t => t.FundId).HasColumnName("FundId");
            this.Property(t => t.FundCode).HasColumnName("FundCode");
            this.Property(t => t.FundDesc).HasColumnName("FundDesc");
            this.Property(t => t.SecurityId).HasColumnName("SecurityId");
            this.Property(t => t.SecurityCode).HasColumnName("SecurityCode");
            this.Property(t => t.Bid).HasColumnName("Bid");
            this.Property(t => t.Offer).HasColumnName("Offer");
            this.Property(t => t.BidNum).HasColumnName("BidNum");
            this.Property(t => t.OfferNum).HasColumnName("OfferNum");
            this.Property(t => t.TotalPar).HasColumnName("TotalPar");
            this.Property(t => t.TotalParNum).HasColumnName("TotalParNum");

        }
    }
}
