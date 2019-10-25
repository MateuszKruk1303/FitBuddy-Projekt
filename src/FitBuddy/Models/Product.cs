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
        [Required(ErrorMessage = "enter the Name of product")]
        public string pName { get; set; }
        [Required(ErrorMessage = "enter the Kcal of product")]
        public double Kcal { get; set; }
        [Required(ErrorMessage = "enter the Protein of product")]
        public double Protein { get; set; }
        [Required(ErrorMessage = "enter the Carbo of product")]
        public double Carb { get; set; }
        [Required(ErrorMessage = "enter the Fats of product")]
        public double Fats { get; set; }
    }
}