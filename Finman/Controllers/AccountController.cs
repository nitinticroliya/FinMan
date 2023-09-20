using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Finman.Models;
using System.Linq;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace Finman.Controllers
{
    public class AccountController : Controller
    {
        FinManEntities entity = new FinManEntities();

        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Signup()
        {
            return View();
        }

        public ActionResult Plans()
        {
            return View();
        }

        public ActionResult InvestmentForm()
        {
            return View();
        }

        public ActionResult RetirementForm()
        {
            return View();
        }

        /*  public ActionResult InvestmentPlansSuggested()
          {

              return View();
          }*/

        public int GetMonthDifference(DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart);
        }

        [Authorize]
        [HttpPost]
        public ActionResult InvestmentForm(InvestmentForm investmentForm)
        {
            Debug.WriteLine(investmentForm.Amount);
            Debug.WriteLine(investmentForm.Datetime.ToString().Substring(0, 10));

            int currentAmountt = investmentForm.Amount;


            int startdate = Convert.ToInt32(DateTime.Now.Day.ToString());
            int startMonth = Convert.ToInt32(DateTime.Now.Month.ToString());
            int startYear = Convert.ToInt32(DateTime.Now.Year.ToString());

            string[] datetime = investmentForm.Datetime.ToString().Substring(0, 10).Split('-');
            int endDate = Convert.ToInt32(datetime[0]);
            int endMonth = Convert.ToInt32(datetime[1]);
            int endYear = Convert.ToInt32(datetime[2]);
            Debug.WriteLine(endDate + "  " + endMonth + " " + endYear);

            DateTime startDateTime = new DateTime(startYear, startMonth, startdate);
            DateTime endDateTime = new DateTime(endYear, endMonth, endDate);

            int totalMonth = GetMonthDifference(startDateTime, endDateTime);

            int years = totalMonth / 12;

            /*RiskAmount riskCal = GetRiskAmount(user);*/
            string risk = "lowrisk";

            /* AdminData adminData = GetAdminData();*/

            double inflation = 0.06;
            double lowriskreturn = 0.08;
            double midriskreturn = 0.12;
            double highriskreturn = 0.18;

            /*double inflation = adminData.Inflation / (100 * 1.0);
            double lowriskreturn = adminData.LowRisk / (100 * 1.0);
            double midriskreturn = adminData.MidRisk / (100 * 1.0);
            double highriskreturn = adminData.HighRisk / (100 * 1.0);*/


            Debug.WriteLine(inflation + "  " + lowriskreturn + "  " + midriskreturn + "  " + highriskreturn + "  lll ");

            Debug.WriteLine(inflation + " inflatoin " + lowriskreturn + "  " + midriskreturn + " sd " + highriskreturn);
            /* Console.WriteLine(adminData.Inflation + " inflatoin " + adminData.LowRisk + "  " + adminData.MidRisk + " sd " + adminData.HighRisk);
 */
            double ret = (risk == "LowRisk" ? lowriskreturn : (risk == "MidRisk" ? midriskreturn : highriskreturn));

            double adj = ((1 + ret) / ((1 + inflation)) * 1.0) - 1;

            Debug.WriteLine(adj);
            double FV = currentAmountt * (Math.Pow((1 + adj), years));
            Debug.WriteLine(FV);
            double ypmt = FV / (((Math.Pow((1 + adj), years) - 1) / adj) * (1 + adj));
            Debug.WriteLine(ypmt);
            double mpmt = ypmt / 12;
            Debug.WriteLine(mpmt);

            investmentPlansSuggested planDetails = new investmentPlansSuggested();
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            Debug.WriteLine(authCookie);
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            Debug.WriteLine(ticket + " break");
            Debug.WriteLine(ticket.UserData);
            string userData = ticket.UserData;
            /*planDetails.userId = Convert.ToInt32(userData);*/
            planDetails.currentAmount = investmentForm.Amount;
            planDetails.startTime = startDateTime.ToString();
            planDetails.endTime = endDateTime.ToString();
            planDetails.risk = risk;
            planDetails.finalAmount = (int)FV;
            planDetails.investmentTime = totalMonth;
            planDetails.monthlyExpenses = (int)mpmt;
            planDetails.inflation = (int)(inflation * 100);
            planDetails.returnPercentage = (int)(adj * 100);

            Debug.WriteLine(planDetails.currentAmount + " " + planDetails.startTime + " " + planDetails.endTime + " " +
                planDetails.risk + " " + planDetails.finalAmount + " " + planDetails.investmentTime + " " + planDetails.monthlyExpenses + " " + planDetails.inflation + " " + planDetails.returnPercentage);


            entity.investmentPlansSuggesteds.Add(planDetails);
            entity.SaveChanges();
            return RedirectToAction("InvestmentPlansSuggested", "Account");
            /*return View(planDetails);*/

        }


        [HttpPost]
        public ActionResult RetirementForm(RetirementForm retirementForm)
        {
            Debug.WriteLine(retirementForm.RetirementTime.ToString().Substring(0, 10));
            Debug.WriteLine(retirementForm.MonthlyExpenses);
            Debug.WriteLine(retirementForm.ExpectedPeriod);

            int startdate = Convert.ToInt32(DateTime.Now.Day.ToString());
            int startMonth = Convert.ToInt32(DateTime.Now.Month.ToString());
            int startYear = Convert.ToInt32(DateTime.Now.Year.ToString());
            string mn = startMonth < 10 ? "0" + startMonth : startMonth.ToString();
            string dy = startdate < 10 ? "0" + startdate : startdate.ToString();
            string startDT = startYear + "-" + mn + "-" + dy;

            string[] datetime = retirementForm.RetirementTime.ToString().Substring(0, 10).Split('-');
            int endDate = Convert.ToInt32(datetime[0]);
            int endMonth = Convert.ToInt32(datetime[1]);
            int endYear = Convert.ToInt32(datetime[2]);

            Console.WriteLine("jemllleoeeoe" + retirementForm.MonthlyExpenses);
            DateTime startDateTime = new DateTime(startYear, startMonth, startdate);
            DateTime endDateTime = new DateTime(endYear, endMonth, endDate);

            int totalMonth = GetMonthDifference(startDateTime, endDateTime);
            // int startYear = Convert.ToInt32(DateTime.Now.Year.ToString());

            int retirementYear = endYear - startYear;
            int retirementPeriod = retirementForm.ExpectedPeriod;

            /*RiskAmount riskCal = GetRiskAmount(user);*/
            string risk = "lowrisk";


            double inflation = 0.06;
            double lowriskreturn = 0.08;
            double midriskreturn = 0.12;
            double highriskreturn = 0.18;
            //  double inflation = 0.06;

            //double ret = (risk=="LowRisk" ? 0.08 : (risk=="MidRisk" ? 0.11 : 0.14));

            /*AdminData adminData = GetAdminData();


            double inflation = adminData.Inflation / (100 * 1.0);
            double lowriskreturn = adminData.LowRisk / (100 * 1.0);
            double midriskreturn = adminData.MidRisk / (100 * 1.0);
            double highriskreturn = adminData.HighRisk / (100 * 1.0);*/


            double ret = (risk == "LowRisk" ? lowriskreturn : (risk == "MidRisk" ? midriskreturn : highriskreturn));

            Console.WriteLine(inflation + "  " + lowriskreturn + "  " + midriskreturn + "  " + highriskreturn + "  lll ");

            double adj = ((1 + ret) / (1 + inflation) * 1.0) - 1;
            Debug.WriteLine(adj);

            double FV = retirementForm.MonthlyExpenses * (Math.Pow((1 + inflation), retirementYear));
            Debug.WriteLine(FV);
            double earexp = FV * 12;
            Debug.WriteLine(earexp);

            double retcorp = earexp * ((1 - (1 / ((Math.Pow((1 + adj), (retirementPeriod - 1)))))) / adj) + earexp;
            Debug.WriteLine(retcorp);
            double ypmt = retcorp / (((Math.Pow((1 + adj), retirementYear) - 1) / adj) * (1 + adj));
            Debug.WriteLine(ypmt);
            double mpmt = ypmt / 12;
            Debug.WriteLine(mpmt);

            inflation = inflation * 100;
            Debug.WriteLine(inflation);
            ret = ret * 100;
            Debug.WriteLine(ret);
            // Debug.WriteLine(ypmt + "  " + FV + " " + retcorp + " " + ypmt + " " + inflation + " " + ret);


            retirementPlansSuggested retirementDetails = new retirementPlansSuggested();
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            Debug.WriteLine(authCookie);
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            Debug.WriteLine(ticket + " break");
            Debug.WriteLine(ticket.UserData);
            string userData = ticket.UserData;
            /*planDetails.userId = Convert.ToInt32(userData);*/
            retirementDetails.MonthlyExpenses = retirementForm.MonthlyExpenses;
            retirementDetails.startTime = startDateTime.ToString();
            retirementDetails.endTime = endDateTime.ToString();
            retirementDetails.risk = risk;
            retirementDetails.finalAmount = (int)FV;
            retirementDetails.InvestmentTimeLeft = totalMonth;
            retirementDetails.MonthlyInvestment = (int)mpmt;
            retirementDetails.Inflation = (int)(inflation * 100);
            retirementDetails.ReturnPercentage = (int)(adj * 100);

            Debug.WriteLine(retirementDetails.MonthlyExpenses + " " + retirementDetails.startTime + " " + retirementDetails.endTime + " " +
                retirementDetails.risk + " " + retirementDetails.finalAmount + " " + retirementDetails.InvestmentTimeLeft + " " + retirementDetails.MonthlyInvestment + " " + retirementDetails.Inflation + " " + retirementDetails.ReturnPercentage);


            entity.retirementPlansSuggesteds.Add(retirementDetails);
            entity.SaveChanges();
            return RedirectToAction("InvestmentPlansSuggested", "Account");
        }

        // [HttpGet]
        public ActionResult InvestmentPlansSuggested()
        {
            List<investmentPlansSuggested> investmentData = entity.investmentPlansSuggesteds.ToList();
            ViewData["InvestmentData"] = investmentData;

           List<retirementPlansSuggested> retirementData = entity.retirementPlansSuggesteds.ToList();
            ViewData["RetirementmentData"] = retirementData;

            return View();

        }

        /* [HttpPost]
         public ActionResult InvestmentPlansSuggested(investmentPlansSuggested investdata)
         {
             entity.investmentPlansSuggesteds.Add(investdata);
             *//*Debug.WriteLine(userinfo.FirstName + userinfo.Id + userinfo.LastName + userinfo.Email + userinfo.Password);*//*
             entity.SaveChanges();
             return RedirectToAction("Plans");
         }*/


        [HttpPost]
        public ActionResult Login(LoginViewModel credentials)
        {
            bool userExists = entity.Logins.Any(model => model.Email == credentials.Email && model.Password == credentials.Password);
            Login user = entity.Logins.FirstOrDefault(model => model.Email == credentials.Email && model.Password == credentials.Password);
            if (userExists)
            {
                FormsAuthentication.SetAuthCookie(user.Id.ToString(), false);
                return RedirectToAction("Index", "Home");

            }

            ModelState.AddModelError("", "Email or Password is wrong");

            return View();
        }


        [HttpPost]
        public ActionResult Signup(Login userinfo)
        {
            entity.Logins.Add(userinfo);
            Debug.WriteLine(userinfo.FirstName + userinfo.Id + userinfo.LastName + userinfo.Email + userinfo.Password);
            entity.SaveChanges();
            return RedirectToAction("Login");
        }

        public ActionResult Signout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}