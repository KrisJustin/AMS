using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMS_v1._3.Models;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace AMS_v1._3.Controllers
{
    public class HomeController : Controller
    {
        AMSEntities db = new AMSEntities();
        // GET: Home
        public ActionResult Index()
        {
            if (Session["username"] != null)
            {
                string pid = Session["username"].ToString();
                var student = db.student.Where(x => x.parentid == pid );
                return View(student.ToList());
            }
            else {
                return RedirectToAction("Index", "Account");
            }
        }
        public ActionResult Add()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Add(int id)
        {
            var data = db.parent.Where(x => x.id == id).FirstOrDefault();
            return View(data);
            
        }

        [HttpPost]
        public ActionResult Add(int id, parent Parent)
        {
            string pid = Session["username"].ToString();
            var edit = db.parent.Find(pid);

            edit.balance = edit.balance + Parent.balance;
            Session["balance"] = edit.balance;
            db.SaveChanges();

            return RedirectToAction("index");
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(student Student)
        {
            if (db.student.Any(x => x.username == Student.username))
            {
                ViewBag.Notification = "Username Already Exists!";
                return View();
            }
            else
            {
                string pid = Session["username"].ToString();
                Student.parentid = pid;
                db.student.Add(Student);
                db.SaveChanges();
                //Session["username"] = Student.username.ToString();
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Edit()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            using (AMSEntities dbs = new AMSEntities())
            {
                return View(dbs.student.Where(x => x.id == id).FirstOrDefault());
            }
        }

        [HttpPost]
        public ActionResult Edit(int id,student Student)
        {
            string pid = Session["username"].ToString();
            var students = db.student.Find(id);
            var parents = db.parent.Find(pid);
            if (students.id == id)
            {
                int parentBal = Convert.ToInt32(Session["balance"]);
                
                students.balance = students.balance + Student.balance;
                parents.balance = parents.balance - Student.balance;
                Session["balance"] = parents.balance;
                db.SaveChanges();

                return RedirectToAction("index");
            }
            else {
                return RedirectToAction("index");
            }
            
        }

        public ActionResult Detail(int id)
        {
            var data = db.student.Where(x => x.id == id).FirstOrDefault();
            return View(data);

        }
        
        public ActionResult Delete(int id)
        {
            var data = db.student.Where(x => x.id == id).FirstOrDefault();
            db.student.Remove(data);
            db.SaveChanges();
            ViewBag.Message = "Record Deleted!";
            return RedirectToAction("index");
        }
        
    }
}