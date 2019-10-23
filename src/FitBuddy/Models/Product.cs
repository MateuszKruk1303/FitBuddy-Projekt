using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace FitBuddy.Models
{
    public class Product
    {
        public string pName { get; set; }
        public double Kcal { get; set; }
        public double Protein { get; set; }
        public double Carb { get; set; }
        public double Fats { get; set; }

        public string Display()
        {
            return pName + ": " + Kcal + " Carbs: " + Carb + " Proteins: " + Protein + " Fats: " + Fats; 
        }
    }
}