using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
    public class TradeBookingMap : EntityTypeConfiguration<TradeBooking>
    {
        public TradeBookingMap()
        {
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.TradeComment)
                .HasMaxLength(500);            

            // Table & Column Mappings
            this.ToTable("TradeBooking", "CLO");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.TradeId).HasColumnName("TradeId");
            this.Property(t => t.TradeGroupId).HasColumnName("TradeGroupId");
            this.Property(t => t.TradeDate).HasColumnName("TradeDate");
            this.Property(t => t.TradeTypeId).HasColumnName("TradeTypeId");
            this.Property(t => t.TraderId).HasColumnName("TraderId");
            this.Property(t => t.LoanXId).HasColumnName("LoanXId");
            this.Property(t => t.IssuerId).HasColumnName("IssuerId");
            this.Property(t => t.FacilityId).HasColumnName("FacilityId");
            this.Property(t => t.CounterPartyId).HasColumnName("CounterPartyId");
            this.Property(t => t.SettleMethodId).HasColumnName("SettleMethodId");
            this.Property(t => t.InterestTreatmentId).HasColumnName("InterestTreatmentId");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.TotalQty).HasColumnName("TotalQty");
            this.Property(t => t.RuleId).HasColumnName("RuleId");
            this.Property(t => t.TradeComment).HasColumnName("TradeComment");
            this.Property(t => t.Price).HasColumnName("Price");            
        }
    }
}
