using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitBuddy.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Kcal { get; set; }
        public int Carbo { get; set; }
        public int Whey { get; set; }
        public int Fat { get; set; }

        public string DisplayProduct
        {
            get
            {
                return Id + " " + Name + " " + Kcal + "kcal  " + "Macro:" + "Carb " + Carbo + " Protein " + Whey + " Fats " + Fat;
            }
        }
    }
}