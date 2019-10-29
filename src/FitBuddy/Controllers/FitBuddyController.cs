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
        public static Int32 helper = 1;
        private static bool status = false;
        private static int register = 0;


        public ActionResult Index()
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
           if(!status)
            {
                ViewBag.existing = "You have to be looged in to add product!";
                return View();
            }
           else
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.existing = "Fill up every field!";
                    return View(); ;
                }
                if (db.Products.Find(product.Name) != null)
                {
                    ViewBag.existing = "This item is already in database!";
                    return View();
                }
                else
                {
                    ViewBag.existing = "Item succesfully added!";
                    db.Products.Add(product);
                    db.SaveChanges();
                    return View();
                }
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
                ViewBag.existing = "This product is not in our database!";

                //Product notf = new Product()
                //{

                //Name = "    ",
                //Kcal = 0,
                //Protein=0,
                //Carb=0,
                //Fats=0

                
                //};

                return View();

            }

            return View(prod);
          
        }

        public ActionResult Login()
        {
            return View();
        }

        public UsersContext dbu = new UsersContext();

        [HttpPost]
        public ActionResult Login(string Nick, string pass)
        {

            if (Nick == null || pass==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User us = dbu.Users.Find(Nick);

            if(dbu.Users.Find(Nick) == null)
            {
                ViewBag.existing = "Fail";
                return View();
            }

            if (!(pass == us.Pass))
            {
                ViewBag.existing = "Fail";
                return View();
            }

            if(status)
            {
                ViewBag.existing = "You are already logged in!";
                return View();
            }

            if (pass == us.Pass)
            {
                status = true;
                ViewBag.existing = "Logged!";
                return View();
            }
           

            return View();
        }

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(User user)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.existing = "Fill up every field!";
                return View(); ;
            }
            if (dbu.Users.Find(user.Nick) != null)
            {
                ViewBag.existing = "There is user with that nickname!";
                return View();
            }
            else
            {
                ViewBag.existing = "Feel free to log in!";
                List<User> help = dbu.Users.ToList();
                user.IDu = help.Count;
                dbu.Users.Add(user);
                dbu.SaveChanges();
                return View();

            }

            
                


            return View();
        }

    }
}