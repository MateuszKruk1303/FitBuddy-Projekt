using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FitBuddy.Models
{
    public class User
    {
        public int IDu{ get; set; }
        [Key]
        [Required(ErrorMessage = " ")]
        public string Nick{ get; set; }
        [Required(ErrorMessage = " ")]
        public string Pass{ get; set; }
       
    }
}