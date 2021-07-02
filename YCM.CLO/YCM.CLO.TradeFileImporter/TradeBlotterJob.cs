using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace YCM.CLO.TradeFileImporter
{
    [Table("TradeBlotterJob",Schema = "CLO")]
    public class TradeBlotterJob
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime? StartedOn { get; set; }

        public DateTime? CompletedOn { get; set; }

        public string Error { get; set; }

        public string FileName { get; set; }

        public int DateId { get; set; }

    }
}
