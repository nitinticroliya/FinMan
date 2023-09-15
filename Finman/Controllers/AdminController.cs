using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Finman.Models;
using System.Linq;

namespace fin.Controllers
{
    public class AdminController : Controller
    {
        FinManEntities entity = new FinManEntities();
        // GET: Admin
        public ActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminLogin(AdminLogin credentials)
        {
            bool userExists = entity.AdminLogins.Any(model => model.AdminEmail == credentials.AdminEmail && model.AdminPassword == credentials.AdminPassword);
            AdminLogin admin = entity.AdminLogins.FirstOrDefault(model => model.AdminEmail == credentials.AdminEmail && model.AdminPassword == credentials.AdminPassword);
            if (userExists)
            {
                FormsAuthentication.SetAuthCookie(admin.AdminEmail, false);
                return RedirectToAction("Index", "Home");

            }

            ModelState.AddModelError("", "Email or Password is wrong");

            return View();
        }

        public ActionResult AdminSignout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}