using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace YCM.CLO.DataAccess.Models.Mapping
{
    public  class vw_CLOTestResultsMap : EntityTypeConfiguration<vw_CLOTestResults>
    {
        public vw_CLOTestResultsMap()
        {
            HasKey(t => new { t.TestDisplayName });
            ToTable(nameof(vw_CLOTestResults), "CLO");
        }
    }
}
