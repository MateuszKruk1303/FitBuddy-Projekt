using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace FitBuddy.Models
{
    public class ProdHistory
    {
        [Key]
        public int HisID { get; set; }
        public string Name { get; set; }
        public double Kcal { get; set; }
        public double Protein { get; set; }
        public double Carb { get; set; }
        public double Fats { get; set; }
        public string Nick { get; set; }
    }
}