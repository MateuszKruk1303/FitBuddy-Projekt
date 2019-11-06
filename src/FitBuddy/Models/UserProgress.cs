using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FitBuddy.Models
{
    public class UserProgress
    {
        [Key]
        public int proID { get; set; }
        public int Limit { get; set; }
        public double Weight { get; set; }
        public double BMI { get; set; }
        public DateTime Date { get; set; }
        public int waist { get; set; }
        public int arm { get; set; }
        public int thigh { get; set; }
        public int calf { get; set; }
        public int forearm { get; set; }
        public int chest { get; set; }
        public string Nick { get; set; }



    }
}