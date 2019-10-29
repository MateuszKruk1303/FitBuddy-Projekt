using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace FitBuddy.Models
{
    public class Product
    {
      
        [Key]
        [Required(ErrorMessage = " ")]
        public string Name { get; set; }
        [Required(ErrorMessage = " ")]
        public double Kcal { get; set; }
        [Required(ErrorMessage = " ")]
        public double Protein { get; set; }
        [Required(ErrorMessage = " ")]
        public double Carb { get; set; }
        [Required(ErrorMessage = " ")]
        public double Fats { get; set; }
    }
}