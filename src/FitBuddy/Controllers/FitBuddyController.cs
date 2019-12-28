using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
        public float kcallimits;
        public string existing;
        public string rjs;
        public string ip;
        public int Id = 2;
        HttpCookie NickCookie = new HttpCookie("username", null);
        HttpCookie WeightIP = new HttpCookie("ip", null);

        private string weightresult(string ip)
        {                          
            if(ip != null)
            {
                string url = $@"http://" + ip + "/";
               
                try
                {
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                    HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                    StreamReader sr = new StreamReader(resp.GetResponseStream());
                    return sr.ReadToEnd();

                }
                catch
                {
                    ViewBag.existing = "Problem with weight";
                    return "0,0";
                }
               

            }
                
                    
                return "0,0";
            
                   
        }

        public ActionResult Welcome()
        {
            Response.Redirect("Fitbuddy/Index");
            return View();
        }

        public ActionResult EmailChange()
        {
            try
            {
                ViewBag.username = Request.Cookies["username"].Value;
            }
            catch
            {

            }

            if (Request.Cookies["username"] == null)
            {
                Response.Redirect("Index");
                return View();
            }

            return View();
        }

        public ActionResult Confirm(string regId)
        {
            try
            {
                User con = dbu.Users.FirstOrDefault(x => x.regID == regId);
                if(con.Active == true)
                {
                    ViewBag.existing = "Your account is already active!";
                    return View();
                }
                con.Active = true;
                dbu.SaveChanges();
                ViewBag.existing = "Your accound has been activated!";
                return View();

            }
            catch
            {
                ViewBag.existing = "fail";
                return View();
            }
            
                
        }

        public ActionResult BMRcalc()
        {
            try
            {
                ViewBag.username = Request.Cookies["username"].Value;
            }
            catch
            {
            }
            ViewBag.existing = null;

            if (Request.Cookies["username"] == null)
            {
                Response.Redirect("Index");
            }
            return View();
        }

        [HttpPost]
        public ActionResult BMRcalc(int? ex, int? time, bool goal)
        {
            try
            {
                ViewBag.username = Request.Cookies["username"].Value;
            }
            catch
            {
            }

            if (Request.Cookies["username"] == null)
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

            string str = Request.Cookies["username"].Value;

            User us = dbu.Users.Find(str);
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
          

            try
            {

                if (Request.Cookies["username"].Value == null)
                {
                    Response.Redirect("Index");
                }
                ViewBag.username = Request.Cookies["username"].Value;

                string stri = Request.Cookies["username"].Value;


            }
            catch
            {

                Response.Redirect("Index");
                return View();
            }

     
            string str = Request.Cookies["username"].Value;


            List<UserProgress> Prog = dbuh.Progress.Where(x => x.Nick == str).ToList();

            return View(Prog);
        }

        public ActionResult PersonalData()
        {
            try
            {
                ViewBag.username = Request.Cookies["username"].Value;
            }
            catch
            {
            }

            if (Request.Cookies["username"] == null)
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

            try
            {
                ViewBag.username = Request.Cookies["username"].Value;
            }
            catch
            {
            }
            for (int i=0;i<=7;i++)
            {
               DateTime today = DateTime.Today;
               DateTime nju = today.AddDays(-i);
               if(dbuh.Progress.FirstOrDefault(x => x.Date == nju) !=null)
                {
                    ViewBag.existing = $"you have to wait {8-i} days to do next measure";
                    return View();
                }
                else
                {
                    continue;
                }
               

                
            }



            if (Request.Cookies["username"] == null)
            {
                Response.Redirect("Index");
                return View();
            }

            string str = Request.Cookies["username"].Value;

            User us = dbu.Users.Find(str);

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

            string stri = Request.Cookies["username"].Value;

            User helper = dbu.Users.FirstOrDefault(x => x.Nick == stri);
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

            User userp = dbu.Users.Find(stri);
            //DateTime dat = new DateTime(2019, 11, 3);

            UserProgress progres = new UserProgress()
            {
                Limit = userp.Limit,
                Weight = userp.Weight,
                BMI = userp.BMI,
                Date = DateTime.Today,
                //Date = dat,
                waist = userp.waist,
                arm = userp.arm,
                thigh = userp.thigh,
                calf = userp.calf,
                forearm = userp.forearm,
                chest = userp.chest,
                Nick = Request.Cookies["username"].Value
            };
            List<UserProgress> progressiv = dbuh.Progress.Where(x => x.Nick == stri).ToList();
            List<UserProgress> helperek = dbuh.Progress.ToList();
            progres.proID = helperek.Count;
            dbuh.Progress.Add(progres);
            dbuh.SaveChanges();
            

            return View();
        }


        [HttpPost]
        public ActionResult EmailChange(string oldmail, string newmail, string pass)
        {
            try
            {
                ViewBag.username = Request.Cookies["username"].Value;
            }
            catch
            {
            }

            if (Request.Cookies["username"] == null)
            {
                Response.Redirect("Index");
                return View();
            }

            if (oldmail == null || pass == null || newmail == null)
            {
                ViewBag.existing = "Fill every field!";
                return View();
            }

            string str = Request.Cookies["username"].Value;

            User us = dbu.Users.Find(str);

            if (!(oldmail == us.Email))
            {
                ViewBag.existing = "Wrong old mail";
                return View();
            }
            
            if (!Crypto.VerifyHashedPassword(us.Pass, pass))
            {
                ViewBag.existing = "Wrong Pass";
                return View();
            }

            bool IsValidEmail(string email)
            {
                try
                {
                    var addr = new MailAddress(email);
                    return addr.Address == email;
                }
                catch
                {
                    return false;
                }
            }

            if (!IsValidEmail(newmail))
            {
                ViewBag.existing = "new mail is not valid!";
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
                User helpnewmail = dbu.Users.FirstOrDefault(x => x.Nick == str);
                Random r = new Random();
                int rand = r.Next(1000, 214748364);
                string randh = rand.ToString();
                randh = Crypto.HashPassword(randh);
                while (dbu.Users.FirstOrDefault(x => x.regID == randh) != null)
                {
                    rand = r.Next(1000, 214748364);
                    randh = rand.ToString();
                    randh = Crypto.HashPassword(randh);
                }
                randh = randh.Replace("+", "X");
                randh = randh.Replace("/", "U");
                randh = randh.Replace("=", "I");
                randh = randh.Replace("%", "Z");

                helpnewmail.regID = randh;
                helpnewmail.Active = false;

                try
                {
                    dbu.SaveChanges();
                }
                catch                               
                {

                    ViewBag.existing = "Fail";
                }

                using (MailMessage msg = new MailMessage())
                {
                    msg.From = new MailAddress("fitbuddy13@gmail.com");
                    msg.To.Add(oldmail);
                    msg.Subject = "Your E-mail has been changed!";
                    msg.Body = "Your Email has been changed to: " + newmail;


                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("fitbuddy13@gmail.com", "Fitbuddy1303");
                        smtp.EnableSsl = true;
                        smtp.Send(msg);

                    }
                }

                using (MailMessage msg = new MailMessage())
                {
                    msg.From = new MailAddress("fitbuddy13@gmail.com");
                    msg.To.Add(newmail);
                    msg.Subject = "Confirm your new Email";
                    var url = "http://localhost:50647/FitBuddy/Confirm?regID=" + helpnewmail.regID;
                    using (StreamReader reader = new StreamReader(Server.MapPath("~/MailStyle/ActivMail.html")))
                    {
                        msg.Body = reader.ReadToEnd();
                    }

                    msg.Body = msg.Body.Replace("{url}", url);
                    msg.Body = msg.Body.Replace("{Nick}", helpnewmail.Nick);
                    // msg.Body = "Confirm you account clicking there: " + url;
                    msg.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("fitbuddy13@gmail.com", "Fitbuddy1303");
                        smtp.EnableSsl = true;
                        smtp.Send(msg);

                    }
                }

                helpnewmail.Email = newmail;
                dbu.SaveChanges();
                ViewBag.username = null;
                ViewBag.existing = "Check mailbox and confirm your new email!";
                return View();
            }

        }

        public ActionResult PasswordChange()
        {
            try
            {
                ViewBag.username = Request.Cookies["username"].Value;
            }
            catch
            {
            }

            if (Request.Cookies["username"] == null)
            {
                Response.Redirect("Index");
                return View();
            }


            return View();
        }

        [HttpPost]
        public ActionResult PasswordChange(string oldpass, string newpass)
        {
            try
            {
                ViewBag.username = Request.Cookies["username"].Value;
            }
            catch
            {
            }

            if (Request.Cookies["username"] == null)
            {
                Response.Redirect("Index");
                return View();
            }
            if (oldpass == null || newpass == null)
            {
                ViewBag.existing = "Fill every field!";
                return View();
            }

            string str = Request.Cookies["username"].Value;
            User us = dbu.Users.Find(str);
            

            if (!Crypto.VerifyHashedPassword(us.Pass, oldpass))
            {
                ViewBag.existing = "Wrong old password!";
                return View();
            }
            if(oldpass == newpass)
            {
                ViewBag.existing = "New passwrod and old are same!";
                return View();
            }
            if(newpass.Length < 6 )
            {
                ViewBag.existing = "At least 6 characters!";
                return View();
            }
            else
            {
                newpass = Crypto.HashPassword(newpass);
                User helpnewpass = dbu.Users.FirstOrDefault(x => x.Nick == str);
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
            try
            {
                ViewBag.username = Request.Cookies["username"].Value;
            }
            catch
            {
            }

            ViewBag.existing = ":(";

            if (Request.Cookies["username"] == null)
            {
                Response.Redirect("Index");
                return View();
            }

            return View();
        }

        [HttpPost]

        public ActionResult AccountDelete(string pass)
        {
            if (Request.Cookies["username"] == null)
            {
                Response.Redirect("Index");
                return View();
            }

            string str = Request.Cookies["username"].Value;

            User us = dbu.Users.Find(str);

            if(!Crypto.VerifyHashedPassword(us.Pass, pass))
            {
                try
                {
                    ViewBag.username = Request.Cookies["username"].Value;
                }
                catch
                {
                }

                ViewBag.existing = "Wrong Password (Unknow power says - Dont do this!)";
                return View();

            }
            else
            {
                User delete = dbu.Users.FirstOrDefault(x => x.Nick == str);
                dbu.Users.Remove(delete);
                dbu.SaveChanges();
                Session.Abandon();
                NickCookie.Value = null;
                //username = null;
                Response.Redirect("Index");
                return View();

            }
            return View();

        }

        public ActionResult Index()
        {
            try
            {
                ViewBag.username = Request.Cookies["username"].Value;
            }
            catch
            {
            }
            
            return View();
        }

        public string getIP ()
        {
            string NazwaHosta = Dns.GetHostName();
            IPHostEntry AdresyIP = Dns.GetHostEntry(NazwaHosta);
            int licznik = 0;
            string ip = "";
            foreach (IPAddress AdresIP in AdresyIP.AddressList)
            {

                if (licznik == 0)
                {
                    licznik++;
                    continue;

                }
                else
                {
                    ip = AdresIP.ToString();
                }

            }
               
            return ip;

        }
    

    [HttpPost]
        public ActionResult Index(string Name, string Subject, string text)
        {
            try
            {
                ViewBag.username = Request.Cookies["username"].Value;
            }
            catch
            {
            }

            if (Request.Cookies["username"] != null)
            {
                try
                {
                    using (MailMessage msg = new MailMessage())
                    {
                        msg.From = new MailAddress("fitbuddy13@gmail.com");
                        msg.To.Add("fitbuddy13@gmail.com");
                        msg.Subject = Subject;
                        msg.Body = text + "    " + "From:" + Name + "   IP:" + getIP();
                        //msg.IsBodyHtml = true;

                        using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                        {
                            smtp.Credentials = new System.Net.NetworkCredential("fitbuddy13@gmail.com", "Fitbuddy1303");
                            smtp.EnableSsl = true;
                            smtp.Send(msg);

                        }
                    }

                    ViewBag.existing = "Email sent!";

                }
                catch (Exception e)
                {
                    ViewBag.existing = e;
                }

                return View();
            }
            else
            {
                ViewBag.existing = "You have be logged in to send message";
                return View();
            }
            
        }


        public ActionResult Account()
        {
            if (Request.Cookies["username"] == null)
            {
                Response.Redirect("Index");
                return View();
            }

            try
            {
                ViewBag.username = Request.Cookies["username"].Value;
            }
            catch
            {
            }

            return View();
        }

        [HttpPost]
        public ActionResult Account(string IP)
        {
            try
            {
                ViewBag.username = Request.Cookies["username"].Value;
            }
            catch
            {
            }

            
            Response.Cookies.Add(WeightIP);
            Request.Cookies["ip"].Value = IP;
            return View();
        }


        public ActionResult History()
        {
            if (Request.Cookies["username"] == null)
            {
                Response.Redirect("Index");
                return View();
            }
            try
            {
                ViewBag.username = Request.Cookies["username"].Value;
            }
            catch
            {
            }
            string str = Request.Cookies["username"].Value;
            List<ProdHistory> productt = dbh.ProdHistoryy.Where(x => x.Nick == str).ToList();
            ViewBag.kcallimits = dbu.Users.FirstOrDefault(x => x.Nick == str).Limit;

            return View(productt);
            
        }

    [HttpPost]
        public ActionResult History(Object obj)
        {
            try
            {
                ViewBag.username = Request.Cookies["username"].Value;
            }
            catch
            {
            }

            while (true)
            {
                try
                {
                    string str = Request.Cookies["username"].Value;
                    ProdHistory item = dbh.ProdHistoryy.FirstOrDefault(x => x.Nick == str);
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
            Session.Abandon();
            NickCookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(NickCookie);
            WeightIP.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(WeightIP);

            return View();
        }

        public ActionResult Add()
        {
            try
            {
                ViewBag.username = Request.Cookies["username"].Value;
            }
            catch
            {
            }

            return View();
        }

        [HttpPost]
        public ActionResult Add(Product product)
        {
            try
            {
                ViewBag.username = Request.Cookies["username"].Value;
            }
            catch
            {
            }

            if (Request.Cookies["username"] == null)
            {
                ViewBag.existing = "You have to be looged in to add product!";
                return View();
            }
           else
            {
                

                if (!ModelState.IsValid)
                {
                    ViewBag.existing = "Fill up every field!";
                    return View();
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
            try
            {
                ViewBag.username = Request.Cookies["username"].Value;
                ViewBag.ip = Request.Cookies["ip"].Value;
            }
            catch
            {
            }
            
            //ViewBag.rjs = downloadcont();

            
            // Product p = db.Products.Find("nullprod");

            return View();
        }

        [HttpPost]
        public ActionResult Scale(double gram = 100, bool type = false, string pronam="")
        {
            try
            {
                if(Request.Cookies["username"] != null)
                {
                    ViewBag.username = Request.Cookies["username"].Value;
                }
                if(Request.Cookies["ip"] != null)
                {

                    ViewBag.ip = Request.Cookies["ip"].Value;
                    string ip = Request.Cookies["ip"].Value;

                }
                
               
            }
            catch(Exception e)
            {

                
                
            }

            //string ipw = Request.Cookies["ip"].Value;
            
            
            

            if (type == false)
            {
                ViewBag.g = gram;
            }
            else
            {
                string toConv = Request.Cookies["gramy"].Value;
                ViewBag.g = Convert.ToDouble(toConv);
            }

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

            if (Request.Cookies["username"] != null)
            {

                ViewBag.Protein = prod.Protein;
                ViewBag.Carbo = prod.Carb;
                ViewBag.Fats = prod.Fats;


                ProdHistory productt = new ProdHistory()
                {
                    Name = prod.Name,
                    Kcal = prod.Kcal * (ViewBag.g / 100),
                    Protein = prod.Protein * (ViewBag.g / 100),
                    Carb = prod.Carb * (ViewBag.g / 100),
                    Fats = prod.Fats * (ViewBag.g / 100),
                    Nick = Request.Cookies["username"].Value
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
            if (Request.Cookies["username"] != null)
            {
                Response.Redirect("Index");
                return View();
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(string Nick, string pass)
        {
            if (Request.Cookies["username"] != null)
            {
                Response.Redirect("Index");
                return View();
            }

            

            if (Nick == null || pass==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //..

            User us = dbu.Users.Find(Nick);

            if(dbu.Users.Find(Nick) == null)
            {
                ViewBag.nickinv = "There is no user with that nickname";
                ViewBag.passinv = null;
                return View();
            }


            if (!Crypto.VerifyHashedPassword(us.Pass, pass))
            {
                ViewBag.passinv = "Wrong Password";
                return View();
            }

            if (us.Active == false)
            {
                ViewBag.existing = "you have to activate your account!";
                return View();
            }


            if (Crypto.VerifyHashedPassword(us.Pass, pass))
            {
                //HttpCookie NickCookie = new HttpCookie("username", us.Nick);
                NickCookie.Value = us.Nick;
                Response.Cookies.Add(NickCookie);
                //Session["username"] = us.Nick;
                //Session["status"] = true;
                //ViewBag.username = (string)Session["username"];
                ViewBag.passinv = null;
                ViewBag.nickinv = null;
                Response.Redirect("Index");
                return View();
            }
           

            return View();
        }

        public ActionResult Signup()
        {
            if (Request.Cookies["username"] != null)
            {
                Response.Redirect("Index");
                return View();
            }
            return View();
        }

        [HttpPost]
        public ActionResult Signup(string Nick, string Email, string Pass ,string passrep)
        {
            if (Request.Cookies["username"] != null)
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
                ViewBag.existing = "Confirm your mail!";
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
                Random r = new Random();
                int rand = r.Next(1000, 214748364);
                string randh = rand.ToString();
                randh = Crypto.HashPassword(randh);
                while (dbu.Users.FirstOrDefault(x => x.regID == randh) != null)
                {
                    rand = r.Next(1000, 214748364);
                    randh = rand.ToString();
                    randh = Crypto.HashPassword(randh);
                }
                randh = randh.Replace("+", "X");
                randh = randh.Replace("/", "U");
                randh = randh.Replace("=", "I");
                randh = randh.Replace("%", "Z");

                user.regID = randh;
                user.Active = false;
                dbu.Users.Add(user);

               

                using (MailMessage msg = new MailMessage())
                {
                    msg.From = new MailAddress("fitbuddy13@gmail.com");
                    try
                    {
                        msg.To.Add(user.Email);
                    }
                    catch
                    {
                        ViewBag.existing = "Wrong E-mail adress!";
                        return View();
                    }
                    
                    msg.Subject = "Confirm your registeration";
                    var url = "http://fitbuddy.com.pl/FitBuddy/Confirm?regID=" + user.regID;
                    using (StreamReader reader = new StreamReader(Server.MapPath("~/MailStyle/ActivMail.html")))
                    {
                        msg.Body = reader.ReadToEnd();
                    }

                    msg.Body = msg.Body.Replace("{url}", url);
                    msg.Body = msg.Body.Replace("{Nick}", user.Nick);
                       // msg.Body = "Confirm you account clicking there: " + url;
                    msg.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new System.Net.NetworkCredential("fitbuddy13@gmail.com", "Fitbuddy1303");
                        smtp.EnableSsl = true;
                        smtp.Send(msg);

                    }
                }

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