using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YCM.CLO.DataAccess.Models
{
    [Table("vw_DefaultSecurities", Schema ="CLO")]
    public class DefaultSecurity
    {

        public decimal? Exposure { get; set; }

        [Key]
        [Column(Order =1)]
        public int FundId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int SecurityId { get; set; }

        public string SecurityCode { get; set; }

        public int DateId { get; set; }

        public DateTime? DefaultDate { get; set; }

        public decimal? Bid { get; set; }

    }
}
