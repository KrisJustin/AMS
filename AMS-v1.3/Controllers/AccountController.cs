using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS_v1._3.Models;
using System.Diagnostics;

namespace AMS_v1._3.Controllers
{
    public class AccountController : Controller
    {
        AMSEntities db = new AMSEntities();
        // GET: Account

        public ActionResult Home()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(parent Parent)
        {
            var checkLogin = db.parent.Where(x => x.username.Equals(Parent.username) && x.passwd.Equals(Parent.passwd)).FirstOrDefault();
            if (checkLogin != null)
            {
                var bal = checkLogin.balance;
                var id = checkLogin.id;
                Session["username"] = Parent.username.ToString();
                Session["firstname"] = checkLogin.firstname;
                Session["balance"] = bal;
                Session["id"] = id;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Notification = "Wrong Username or Password!";
                return View();
            }
        }

        public ActionResult Register() {
            return View();
        }

        [HttpPost]
        public ActionResult Register(parent Parent) {

            if (db.parent.Any(x => x.username == Parent.username))
            {
                ViewBag.Notification = "Username Already Exists!";
                return View();
            }
            else {
                db.parent.Add(Parent);
                db.SaveChanges();
                var bal = Parent.balance;
                var id = Parent.id;
                Session["username"] = Parent.username.ToString();
                Session["firstname"] = Parent.firstname.ToString();
                Session["balance"] = bal;
                Session["id"] = id;
                return RedirectToAction("Index","Home");
            }
            
        }

        public ActionResult parent() {
            return View();
        }


        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Account");
        }

        
    }
}