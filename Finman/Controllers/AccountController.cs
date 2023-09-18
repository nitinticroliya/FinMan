﻿using System;
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

        public ActionResult InvestmentPlansSuggested()
        {

            return View();
        }

        public int GetMonthDifference(DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart);
        }

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
            /*HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            Debug.WriteLine(authCookie);
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);*/
            /*planDetails.userId = Convert.ToInt32(ticket.UserData);*/
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
                planDetails.risk + " " + planDetails.finalAmount + " " + planDetails.investmentTime + " " + planDetails.inflation + " " + planDetails.returnPercentage);


            entity.investmentPlansSuggesteds.Add(planDetails);
            entity.SaveChanges();
            return RedirectToAction("InvestmentPlansSuggested", "Account");
            /*return View(planDetails);*/

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