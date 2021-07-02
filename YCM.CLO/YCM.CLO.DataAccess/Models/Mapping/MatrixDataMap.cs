using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YCM.CLO.DataAccess.Models.Mapping
{
    public class MatrixDataMap : EntityTypeConfiguration<MatrixData>
    {
        public MatrixDataMap()
        {

            // Primary Key
            this.HasKey(t => t.Id);
            this.Property(t => t.Spread).HasPrecision(28, 10);
            this.ToTable("MatrixData", "CLO");
        }

    }
}
