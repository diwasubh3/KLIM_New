using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YCM.CLO.DataAccess.Models
{
    [Table("AssetClass",Schema = "CLO")]
    public class AssetClass
    {
        [Key]
        public int AssetClassId { get; set; }

        public string AssetClassCode { get; set; }
    }
}


