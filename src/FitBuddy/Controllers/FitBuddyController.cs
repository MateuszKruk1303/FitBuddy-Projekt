using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FitBuddy.Models;

namespace FitBuddy.Controllers
{
    public class FitBuddyController : Controller
    {
        // GET: FitBuddy


        public ActionResult Index()
        {

            FitBuddyEntities db = new FitBuddyEntities();
            List<Products> prod = db.Products.ToList();

            var produ = new Product()
            {
                pName = prod[0].pName,
                Kcal = prod[0].Kcal,
                Protein = prod[0].Protein,
                Fats = prod[0].Fats,
                Carb = prod[0].Carb


            };
   
            return View(produ);
        }


    }
}