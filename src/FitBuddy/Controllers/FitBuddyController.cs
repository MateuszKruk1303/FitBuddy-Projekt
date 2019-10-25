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


        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Product product)
        {
            if(!ModelState.IsValid)
            {
                return View("Index", product);
            }
            else
            {
                
                db.Products.Add(product);
                db.SaveChanges();
                return View("Index");
            }

         

        }

        public ActionResult Zwrot()
        {
           
            return View();
        }

        [HttpPost]
        public ActionResult Zwrot(string pronam="")
        {
            if (pronam == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product prod = db.Products.Find(pronam);

            if(prod == null)
            {
                return HttpNotFound();
                
            }

            return View(prod);
          
            
            //Product product = new Product()
            //{
            //    pName = name,
            //    Kcal = 0,
            //    Protein = 0,
            //    Carb = 0,
            //    Fats = 0
            //};
            //return View(product);
            //ProductsContext db = new ProductsContext();
            //return View(product);
            //product = db.Products.FirstOrDefault(x => x.pName.Contains(name));

            

        }

    }
}