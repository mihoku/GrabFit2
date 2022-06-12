using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using GrabFit2.Models;
using Microsoft.AspNet.Identity;

namespace GrabFit2.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var orders = db.OrderTagged.Where(y=>y.FoodOrdered.tagged && y.FoodOrdered.orderDate == DateTime.Today);
            CultureInfo glob = CultureInfo.CreateSpecificCulture("en-US");
            if (orders.Count() == 0)
            {
                ViewBag.cal = 0;
                ViewBag.carb = 0;
                ViewBag.fat = 0;
                ViewBag.prot = 0;
                return View();
            }
            ViewBag.cal = string.Format(glob, "{0:0.00}", Math.Round(orders.Sum(y => y.FoodOrdered.itemOrdered*y.FoodReferences.Calorie), 2));
            ViewBag.carb = string.Format(glob, "{0:0.00}", Math.Round(orders.Sum(y => y.FoodOrdered.itemOrdered * y.FoodReferences.Carbohydrate), 2));
            ViewBag.fat = string.Format(glob, "{0:0.00}", Math.Round(orders.Sum(y => y.FoodOrdered.itemOrdered * y.FoodReferences.Fat), 2));
            ViewBag.prot = string.Format(glob, "{0:0.00}", Math.Round(orders.Sum(y => y.FoodOrdered.itemOrdered * y.FoodReferences.Protein),2));
            return View();
        }

        public ActionResult Calorie(DateTime selectedDate)
        {
            var orders = db.OrderTagged.Where(y => y.FoodOrdered.tagged && y.FoodOrdered.orderDate == selectedDate);
            CultureInfo glob = CultureInfo.CreateSpecificCulture("en-US");
            if (orders.Count() == 0)
            {
                var carb = 0.00;
                return Content(string.Format(glob, "{0:0.00}", carb));
            }
            var carb2 = Math.Round(orders.Sum(y => y.FoodOrdered.itemOrdered * y.FoodReferences.Calorie), 2);
            return Content(string.Format(glob, "{0:0.00}", carb2));
        }

        public ActionResult Carb(DateTime selectedDate)
        {
            var orders = db.OrderTagged.Where(y => y.FoodOrdered.tagged && y.FoodOrdered.orderDate == selectedDate);
            CultureInfo glob = CultureInfo.CreateSpecificCulture("en-US");
            if (orders.Count() == 0)
            {
                var carb = 0.00;
                return Content(string.Format(glob, "{0:0.00}", carb));
            }
            var carb2 = Math.Round(orders.Sum(y => y.FoodOrdered.itemOrdered * y.FoodReferences.Carbohydrate), 2);
            return Content(string.Format(glob, "{0:0.00}", carb2));
        }

        public ActionResult Fat(DateTime selectedDate)
        {
            var orders = db.OrderTagged.Where(y => y.FoodOrdered.tagged && y.FoodOrdered.orderDate == selectedDate);
            CultureInfo glob = CultureInfo.CreateSpecificCulture("en-US");
            if (orders.Count() == 0)
            {
                var carb = 0.00;
                return Content(string.Format(glob, "{0:0.00}", carb));
            }
            var carb2 = Math.Round(orders.Sum(y => y.FoodOrdered.itemOrdered * y.FoodReferences.Fat), 2);
            return Content(string.Format(glob, "{0:0.00}", carb2));
        }

        public ActionResult Protein(DateTime selectedDate)
        {
            var orders = db.OrderTagged.Where(y => y.FoodOrdered.tagged && y.FoodOrdered.orderDate == selectedDate);
            CultureInfo glob = CultureInfo.CreateSpecificCulture("en-US");
            if (orders.Count() == 0)
            {
                var carb = 0.00;
                return Content(string.Format(glob, "{0:0.00}", carb));
            }
            var carb2 = Math.Round(orders.Sum(y => y.FoodOrdered.itemOrdered * y.FoodReferences.Protein), 2);
            return Content(string.Format(glob, "{0:0.00}", carb2));
        }

        public ActionResult History(int y, int m, int d)
        {
            DateTime selectedDate = new DateTime(y, m, d);
            ViewBag.selectedDate = selectedDate;
            var orders = db.OrderTagged.Where(x => x.FoodOrdered.tagged && x.FoodOrdered.orderDate == selectedDate);
            if (orders.Count() == 0)
            {
                ViewBag.cal = 0;
                ViewBag.carb = 0;
                ViewBag.fat = 0;
                ViewBag.prot = 0;   
                return View();
            }
            ViewBag.cal = Math.Round(orders.Sum(x => x.FoodOrdered.itemOrdered * x.FoodReferences.Calorie),2);
            ViewBag.carb = Math.Round(orders.Sum(x => x.FoodOrdered.itemOrdered * x.FoodReferences.Carbohydrate),2);
            ViewBag.fat = Math.Round(orders.Sum(x => x.FoodOrdered.itemOrdered * x.FoodReferences.Fat),2);
            ViewBag.prot = Math.Round(orders.Sum(x => x.FoodOrdered.itemOrdered * x.FoodReferences.Protein),2);
            return View();
        }

        public ActionResult Name()
        {
            var user = System.Web.HttpContext.Current.User.Identity.GetUserName();
            var completename = db.Users.Where(y => y.UserName == user).First().CompleteName;
            return Content(completename);
        }

        public ActionResult ImageUser()
        {
            var user = System.Web.HttpContext.Current.User.Identity.GetUserName();
            var gender = db.Users.Where(y => y.UserName == user).First().Gender;
            if (gender == "M")
            {
                var pic2 = "/assets-poco/images/dashboard/user2.png";
                return Content(pic2);
            }
            var pic = "/assets-poco/images/dashboard/user.png";
            return Content(pic);
        }

        public ActionResult meal(DateTime dateSelected)
        {
            DateTime dateRecreated = new DateTime(dateSelected.Year, dateSelected.Month, dateSelected.Day);
            var result = db.OrderTagged.Where(y => y.FoodOrdered.orderDate == dateRecreated).ToList();
            return PartialView(result);
        }

        public ActionResult MealDetails(int id)
        {
            OrderTagged item = db.OrderTagged.Find(id);
            ViewBag.FatCal = Math.Round(item.FoodReferences.Fat * 9 / item.FoodReferences.Calorie * 100);
            ViewBag.CarbCal = Math.Round(item.FoodReferences.Carbohydrate * 4 / item.FoodReferences.Calorie * 100);
            ViewBag.ProtCal = Math.Round(item.FoodReferences.Protein * 4 / item.FoodReferences.Calorie * 100);
            if(item.FoodReferences.Fat==0&& item.FoodReferences.Protein == 0&& item.FoodReferences.Carbohydrate == 0)
            {
                ViewBag.CarbCal = 100;
                return View(item);
            }
            return View(item);
        }



    }
}