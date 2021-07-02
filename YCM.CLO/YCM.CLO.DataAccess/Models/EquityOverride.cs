using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YCM.CLO.DataAccess.Models
{
    [Table("EquityOverride", Schema = "CLO")]
    public class EquityOverride
    {
        [Key]
        public int Id { get; set; }

        public int FundId { get; set; }

        public string SecurityCode { get; set; }

        public decimal? Notional { get; set; }

        public decimal? Bid { get; set; }

        public bool? IsDeleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModifiedOn { get; set; }

    }
}
