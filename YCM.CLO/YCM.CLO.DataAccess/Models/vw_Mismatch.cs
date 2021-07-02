using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YCM.CLO.DataAccess.Models
{
    public class vw_Mismatch
    {
        [Key]
        public string SecurityCode { get; set; }

        
        public string CLO1 { get; set; }

        
        public string CLO2 { get; set; }

        
        public string CLO3 { get; set; }

       
        public string CLO4 { get; set; }

       
        public string CLO5 { get; set; }

       
        public string CLO6 { get; set; }

       
        public string CLO7 { get; set; }

       
        public string CLO8 { get; set; }

       
        public string CLO9 { get; set; }

    }
}
