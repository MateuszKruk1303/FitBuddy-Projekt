using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using FitBuddy.DAL;
using FitBuddy.Models;



namespace FitBuddy.Controllers
{
    public class FitBuddyController : Controller
    {
        public ProductsContext db = new ProductsContext();
        public ProdHistoryContext dbh = new ProdHistoryContext();
        public UsersContext dbu = new UsersContext();
        public UserProgressContext dbuh = new UserProgressContext();

        // GET: FitBuddy
        public static float kcallimits;
        public string existing;
        private static bool status = false;
        public static string username;
        public static string rjs;

        private string downloadcont()
        {
            string url = "https://countn.com/";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            StreamReader sr = new StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd();
             
        }

        public ActionResult Welcome()
        {
            Response.Redirect("Fitbuddy/Index");
            return View();
        }

        public ActionResult EmailChange()
        {
            ViewBag.username = username;

            if (status == false)
            {
                Response.Redirect("Index");
                return View();
            }

            return View();
        }

        public ActionResult BMRcalc()
        {
            ViewBag.username = username;
            ViewBag.existing = " ";

            if (status == false)
            {
                Response.Redirect("Index");
            }
            return View();
        }

        [HttpPost]
        public ActionResult BMRcalc(int? ex, int? time, bool goal)
        {
            ViewBag.username = username;

            if (status == false)
            {
                Response.Redirect("Index");
            }
            if(ex == null)
            {
                ViewBag.existing = "How many trainings do you do per week?";
                return View();

            }
            if(time == null)
            {
                ViewBag.existing = "How much time do you spend on traning?";
                return View();

            }

            User us = dbu.Users.Find(username);
            double? prebm = 0;
            if(us.Sex == "m")
            {
                prebm = (9.99 * us.Weight) + (6.25 * us.Height) - (4.92 * us.Age) + 5;
                prebm = prebm + ((ex * time * 8)/7);
                prebm = prebm + 500;
                if(goal)
                {
                    us.Limit = (int)prebm + 300;
                }
                else
                {
                    us.Limit = (int)prebm - 300;
                }

                dbu.SaveChanges();

            }
            else if(us.Sex == "w")
            {
                prebm = (9.99 * us.Weight) + (6.25 * us.Height) - (4.92 * us.Age) - 161;
                prebm = prebm + (ex * time * 8);
                prebm = prebm + 500;
                if (goal)
                {
                    us.Limit = (int)prebm + 300;
                }
                else
                {
                    us.Limit = (int)prebm - 300;
                }

                dbu.SaveChanges();

            }
            else
            {
               return View();
            }

            ViewBag.existing = us.Limit;
            return View();
        }


        public ActionResult Progress()
        {
            if(status == false)
            {
                Response.Redirect("Index");
            }

            ViewBag.username = username;
            List<UserProgress> Prog = dbuh.Progress.Where(x => x.Nick == username).ToList();

            return View(Prog);
        }

        public ActionResult PersonalData()
        {
            ViewBag.username = username;

            if (status == false)
            {
                Response.Redirect("Index");
                return View();
            }

            return View();
        }

        [HttpPost]

        public ActionResult PersonalData(double wg = 0, int age = 0, string sex = null, int hg = 0, int waist = 0, int arm = 0, int thigh = 0, int calf = 0, int forearm = 0, int chest = 0)
        {
            // We dont need default values etc. better use int? double? when we're using forms

            ViewBag.username = username;

            if (status == false)
            {
                Response.Redirect("Index");
                return View();
            }

            User us = dbu.Users.Find(username);

            sex = us.Sex;
            sex = "m";

            if (wg == 0 || age == 0 || hg == 0 || waist == 0 || arm == 0 || thigh == 0 || calf == 0 || forearm == 0 || chest == 0 || sex == null)
            {
                if (wg == 0)
                {
                    wg = us.Weight;
                }
                if (age == 0)
                {
                    age = us.Age;
                }
                if (hg == 0)
                {
                    hg = us.Height;
                }
                if (waist == 0)
                {
                    waist = us.waist;
                }
                if (arm == 0)
                {
                    arm = us.arm;
                }
                if (thigh == 0)
                {
                    thigh = us.thigh;
                }
                if (calf == 0)
                {
                    calf = us.calf;
                }
                if (forearm == 0)
                {
                    forearm = us.forearm;
                }
                if (chest == 0)
                {
                    chest = us.chest;
                }
                if (sex == "n")
                {
                    sex = us.Sex;
                }
            }

            ViewBag.existing = "Succes!";

            if (!(sex == "m" || sex == "k"))
            {
                sex = "n";
                ViewBag.existing = "Error with gender, write m or k in field!";
            }

            

            User helper = dbu.Users.FirstOrDefault(x => x.Nick == username);
            helper.Weight = wg;
            helper.Age = age;
            helper.Height = hg;
            helper.waist = waist;
            helper.arm = arm;
            helper.thigh = thigh;
            helper.calf = calf;
            helper.forearm = forearm;
            helper.chest = chest;
            helper.Sex = sex;
            helper.BMI = (wg / (hg * hg)) * 10000;
            dbu.SaveChanges();

            User userp = dbu.Users.Find(username);
            //DateTime dat = new DateTime(2019, 11, 3);

            UserProgress progres = new UserProgress()
            {
                Limit = userp.Limit,
                Weight = userp.Weight,
                BMI = userp.BMI,
                Date = DateTime.Today.Date,
                //Date = dat,
            waist = userp.waist,
            arm = userp.arm,
            thigh = userp.thigh,
            calf = userp.calf,
            forearm = userp.forearm,
            chest = userp.chest,
            Nick = username
            };
            List<UserProgress> progressiv = dbuh.Progress.Where(x => x.Nick == username).ToList();
            List<UserProgress> helperek = dbuh.Progress.ToList();
            progres.proID = helperek.Count;
            dbuh.Progress.Add(progres);
            dbuh.SaveChanges();
            

            return View();
        }


        [HttpPost]
        public ActionResult EmailChange(string oldmail, string newmail, string pass)
        {
            ViewBag.username = username;

            if (status == false)
            {
                Response.Redirect("Index");
                return View();
            }

            if (oldmail == null || pass == null || newmail == null)
            {
                ViewBag.existing = "Fill every field!";
                return View();
            }

            User us = dbu.Users.Find(username);

            if (!(oldmail == us.Email))
            {
                ViewBag.existing = "Wrong old mail";
                return View();
            }

            if (!(pass == us.Pass))
            {
                ViewBag.existing = "Wrong Pass";
                return View();
            }

            User any = dbu.Users.FirstOrDefault(x => x.Email == newmail);

            if(any != null)
            {
                ViewBag.existing = "This email is already used by another user!";
                return View();
            }
            else
            {
                User helpnewmail = dbu.Users.FirstOrDefault(x => x.Nick == username);
                helpnewmail.Email = newmail;
                dbu.SaveChanges();
                ViewBag.existing = "Email Changed!";
                return View();
            }


            return View();
        }

        public ActionResult PasswordChange()
        {
            ViewBag.username = username;

            if (status == false)
            {
                Response.Redirect("Index");
                return View();
            }


            return View();
        }

        [HttpPost]
        public ActionResult PasswordChange(string oldpass, string newpass)
        {
            ViewBag.username = username;

            if (status == false)
            {
                Response.Redirect("Index");
                return View();
            }
            if (oldpass == null || newpass == null)
            {
                ViewBag.existing = "Fill every field!";
                return View();
            }
            User us = dbu.Users.Find(username);

            if(oldpass != us.Pass)
            {
                ViewBag.existing = "Wrong old password!";
                return View();
            }
            if(oldpass == newpass)
            {
                ViewBag.existing = "New passwrod and old are same!";
                return View();
            }
            else
            {
                User helpnewpass = dbu.Users.FirstOrDefault(x => x.Nick == username);
                helpnewpass.Pass = newpass;
                ViewBag.existing = "Password Changed!";
                try
                {
                   dbu.SaveChanges();
                }
                catch
                {
                    ViewBag.existing = "Something went wrong";
                }
                return View();
            }


            return View();
        }

        public ActionResult AccountDelete()
        {
            ViewBag.username = username;

            if (status == false)
            {
                Response.Redirect("Index");
                return View();
            }

            return View();
        }

        [HttpPost]

        public ActionResult AccountDelete(string pass)
        {
            if (status == false)
            {
                Response.Redirect("Index");
                return View();
            }

            User us = dbu.Users.Find(username);

            if(pass != us.Pass)
            {
                ViewBag.existing = "Wrong Password!";
                return View();

            }
            else
            {
                User delete = dbu.Users.FirstOrDefault(x => x.Nick == username);
                dbu.Users.Remove(delete);
                dbu.SaveChanges();
                status = false;
                username = null;
                Response.Redirect("Index");
                return View();

            }
            return View();

        }

        public ActionResult Index()
        {
            ViewBag.username = username;
            return View();
        }

        public ActionResult Account()
        {
            if (status == false)
            {
                Response.Redirect("Index");
                return View();
            }

            ViewBag.username = username;
            return View();
        }

        [HttpPost]
        public ActionResult Account(int limit)
        {
            ViewBag.username = username;

            User updatelimit = dbu.Users.FirstOrDefault(x => x.Nick == username);
            updatelimit.Limit = limit;
            dbu.SaveChanges();
            return View();
        }


        public ActionResult History()
        {
            if (status == false)
            {
                Response.Redirect("Index");
                return View();
            }
            ViewBag.username = username;
            List<ProdHistory> productt = dbh.ProdHistoryy.Where(x => x.Nick == username).ToList();
            ViewBag.kcallimits = dbu.Users.FirstOrDefault(x => x.Nick == username).Limit;

            return View(productt);
            
        }

    [HttpPost]
        public ActionResult History(Object obj)
        {
            ViewBag.username = username;

            while (true)
            {
                try
                {
                    ProdHistory item = dbh.ProdHistoryy.FirstOrDefault(x => x.Nick == username);
                    dbh.ProdHistoryy.Remove(item);
                    
                    dbh.SaveChanges();
                }
                catch
                {
                    break;
                }
            }
            
            return View();
        }


        public ActionResult Logout()
        {
            status = false;
            username = null;
            return View();
        }

        public ActionResult Add()
        {
            ViewBag.username = username;

            return View();
        }

        [HttpPost]
        public ActionResult Add(Product product)
        {
            ViewBag.username = username;

            if (!status)
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
            ViewBag.username = username;
            //ViewBag.rjs = downloadcont();

            
            // Product p = db.Products.Find("nullprod");

            return View();
        }

        [HttpPost]
        public ActionResult Scale(double gram=100, string pronam="")
        {
            ViewBag.username = username;
            ViewBag.g = gram;

            if (pronam == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product prod = db.Products.Find(pronam);

            if(prod == null)
            {
                ViewBag.existing = "This product is not in our database!";
                return View();

            }

            if (status)
            {

                ViewBag.Protein = prod.Protein;
                ViewBag.Carbo = prod.Carb;
                ViewBag.Fats = prod.Fats;


                ProdHistory productt = new ProdHistory()
                {
                    Name = prod.Name,
                    Kcal =  prod.Kcal,
                    Protein = prod.Protein,
                    Carb = prod.Carb,
                    Fats = prod.Fats,
                    Nick = username
                };

                List<ProdHistory> help2 = dbh.ProdHistoryy.ToList();  
                productt.HisID = help2.Count;
                dbh.ProdHistoryy.Add(productt);
                dbh.SaveChanges();
            }

            return View(prod);
          
        }

        public ActionResult Login()
        {
            if (status)
            {
                Response.Redirect("Index");
                return View();
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(string Nick, string pass)
        {
            if (status)
            {
                Response.Redirect("Index");
                return View();
            }

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
           
            if (!Crypto.VerifyHashedPassword(us.Pass, pass))
            {
                ViewBag.existing = "Fail";
                return View();
            }


            if(Crypto.VerifyHashedPassword(us.Pass, pass))
            {
                status = true;
                username = Nick;
                ViewBag.username = username;
                ViewBag.existing = "Logged!";
                return View();
            }
           

            return View();
        }

        public ActionResult Signup()
        {
            if (status)
            {
                Response.Redirect("Index");
                return View();
            }
            return View();
        }

        [HttpPost]
        public ActionResult Signup(string Nick, string Email, string Pass ,string passrep)
        {
            if (status)
            {
                Response.Redirect("Index");
                return View();
            }
             

            User user = new User();
            user.Nick = Nick;
            var hash = Crypto.HashPassword(Pass);
            user.Pass = hash;
            user.Email = Email;

            if (!ModelState.IsValid)
            {
                ViewBag.existing = "Fill up every field!";
                return View();
            }
            if (dbu.Users.Find(user.Nick) != null)
            {
                ViewBag.existing = "There is user with that nickname!";
                return View();
            }
            if (dbu.Users.FirstOrDefault(x => x.Email == Email) != null)
            {
                ViewBag.existing = "There is user with that Email!";
                return View();
            }
            if (!(Pass == passrep))
            {
                ViewBag.existing = "Passwords are not matching!";
                return View();

            }
            else
            {
                ViewBag.existing = "Feel free to log in!";
                List<User> help = dbu.Users.ToList();
                user.IDu = help.Count;
                user.Limit = 40000;
                user.Height = 0;
                user.Sex = "z";
                user.Weight = 0;
                user.Age = 0;
                user.BMI = 0;
                var DT = DateTime.Now;
                user.Date = DT.Date;
                user.waist = 0;
                user.arm =  0;
                user.thigh = 0;
                user.calf = 0;
                user.forearm = 0;
                user.chest = 0;
                dbu.Users.Add(user);

                try
                {
                  dbu.SaveChanges();
                }
                catch                               // 420 NOSCOPE MOM GET THE CAMERA
                {
                   
                        ViewBag.existing = "Something went wrong";
                }
                
                return View();

            }

            
                


            return View();
        }

    }
}