using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YCM.CLO.DataAccess.Models.Mapping
{
    public class MatrixPointMap : EntityTypeConfiguration<MatrixPoint>
    {
        public MatrixPointMap()
        {

            // Primary Key
            this.HasKey(t => t.Id);

            this.Property(t => t.Spread).HasPrecision(28, 10);
            this.Property(t => t.Diversity).HasPrecision(28, 10);
            this.Property(t => t.TopMajorSpread).HasPrecision(28, 10);
            this.Property(t => t.BottomMajorSpread).HasPrecision(28, 10);
            this.Property(t => t.LeftMajorDiversity).HasPrecision(28, 10);
            this.Property(t => t.RightMajorDiversity).HasPrecision(28, 10);
            this.Property(t => t.TopSpread).HasPrecision(28, 10);
            this.Property(t => t.BottomSpread).HasPrecision(28, 10);
            this.Property(t => t.LeftDiversity).HasPrecision(28, 10);
            this.Property(t => t.RightDiversity).HasPrecision(28, 10);

            this.Ignore(t => t.CreatedOnString);

            this.ToTable("MatrixPoint", "CLO");
        }

    }
}
