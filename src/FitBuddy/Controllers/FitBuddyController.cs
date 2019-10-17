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

        Product produkt1 = new Product()
        {
            Id=1,
            Name="God'sss Bread",
            Kcal=1337,
            Carbo=400,
            Whey=100,
            Fat=10


        };

        public ActionResult Index()
        {
            return View(produkt1);
        }


    }
}