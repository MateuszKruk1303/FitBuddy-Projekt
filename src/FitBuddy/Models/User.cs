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
        [StringLength(100,ErrorMessage = "At Least 6 characters", MinimumLength = 6)]
        public string Pass{ get; set; }
        [Required(ErrorMessage = " ")]
        [EmailAddress]
        public string Email{ get; set; }
        public int Limit { get; set; }
        public double Weight { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
        public int Height { get; set; }
        public double BMI { get; set; }
        public DateTime Date { get; set; }
        public int waist { get; set; }
        public int arm { get; set; }
        public int thigh { get; set; }
        public int calf { get; set; }
        public int forearm { get; set; }
        public int chest { get; set; }
        public string regID { get; set; }
        public bool Active { get; set; }



    }
}