using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YCM.CLO.DataAccess.Models
{
    public class TrendType
    {
        public TrendType()
        {

        }
        [Key]
        public int TypeID { get; set; }
        public string TrendTypeName { get; set; }
        public bool IsActive { get; set; }
    }
}
