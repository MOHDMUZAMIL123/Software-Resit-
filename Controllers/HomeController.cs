using HappyTech.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HappyTech.Controllers
{
    public class HomeController : Controller
    {
        private HappyTechDBEntities db = new HappyTechDBEntities();
        public ActionResult Index()
        {

            ViewBag.TemplateList = db.Templates.ToList();
            if (Session["UserExist"] == null || Session["UserExist"].ToString() == "false")
            {
                return RedirectToAction("/login");
            }
            if(Session["Username"].ToString().ToLower() == "admin")
            {
                return View(db.Applications.ToList());
            }
            else
            {
                string username = Session["Username"].ToString();
                return View(db.Applications.Where(m=>m.SubmittedBy == username).ToList());
            }
        }

        public ActionResult Login()
        {
            return View();
        }
        
        public ActionResult Logout()
        {
            Session["UserExist"] = "";
            Session["Username"] = "";
            return RedirectToAction("/login");
        }

        public ActionResult LoginUser([Bind(Include = "Username,Password")] string Username, string password)
        {
            if(!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(password))
            {
                var userInfo = db.Users.Where(user => user.Username == Username && user.Password == password);
                if (userInfo.Any())
                {
                    Session["UserExist"] = "true";
                    Session["Username"] = userInfo.FirstOrDefault().Username;
                    return RedirectToAction("/");
                }
                TempData["ErrorMessage"] = "Invalid Username and password";
                return RedirectToAction("/login");
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid Username and password";
                Session["UserExist"] = "false";
                return RedirectToAction("/login");
            }
        }


        public ActionResult Submit(int? id,string user)
        {
            Template template = db.Templates.Find(id);
            Application uInfo = db.Applications.Where(app => app.SubmittedBy == user).FirstOrDefault();
            uInfo.FeedbackSubmittedBy = Session["Username"].ToString();
            uInfo.FeedbackId = template.ID;
            uInfo.FeedbackSubmitted = true;
            db.Entry(uInfo).State = EntityState.Modified;
            db.SaveChanges();
            TempData["EmailMessage"] = "Email has been sent to "+uInfo.SubmittedBy+" for there feedback successfully.";
            return RedirectToAction("/");
        }

        public ActionResult ViewFeedback(int? id)
        {
            ViewBag.TemplateList = db.Templates.ToList();
            Application uInfo = db.Applications.Where(app => app.ID == id).FirstOrDefault();
            return View(uInfo);
        }
    }
}