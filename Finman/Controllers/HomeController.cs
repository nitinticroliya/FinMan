using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Finman.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Plans()
        {
            ViewBag.Message = "Your Plans page.";
            return View();
        }

        public ActionResult InvestmentForm()
        {
            ViewBag.Message = "Your Suggeseted Plan.";
            return View();
        }


       /* [AllowAnonymous]*/
        public ActionResult Index()
        {
            return View();
        }

        
        /*[AllowAnonymous]*/
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        
    }
}