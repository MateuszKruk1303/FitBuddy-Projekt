using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FitBuddy.DAL;
using FitBuddy.Models;


namespace FitBuddy.Controllers
{
    public class FitBuddyController : Controller
    {
       public ProductsContext db = new ProductsContext();
        // GET: FitBuddy

        public string existing;


        public ActionResult Indexx()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Product product)
        {
            
            if (!ModelState.IsValid)
            {
                ViewBag.existing = "Fill up every field!";
                return View(); ;
            }
            if(db.Products.Find(product.Name)!=null)
            {
                ViewBag.existing = "This item is already in database!";
                return View();
            }
            else
            {
                ViewBag.existing = "Item succesfully added!";
                db.Products.Add(product);
                db.SaveChanges();
                return View("Add");
            }

         

        }

        public ActionResult Scale()
        {
           
            return View();
        }

        [HttpPost]
        public ActionResult Scale(string pronam="")
        {
            if (pronam == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product prod = db.Products.Find(pronam);

            if(prod == null)
            {
                Product notf = new Product()
                {
                Name = "Not Found",
                Kcal = 0,
                Protein=0,
                Carb=0,
                Fats=0

                
                };

                return View(notf);

            }

            return View(prod);
          
            
            

        }

    }
}