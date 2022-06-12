using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GrabFit2.Models;

namespace GrabFit2.Controllers
{
    [Authorize]
    public class FoodBaseController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: FoodBase
        public ActionResult Index()
        {
            return View(db.FoodReferences.ToList());
        }

        // GET: FoodBase/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodReferences foodReferences = db.FoodReferences.Find(id);
            if (foodReferences == null)
            {
                return HttpNotFound();
            }
            ViewBag.FatCal = Math.Round(foodReferences.Fat*9/foodReferences.Calorie*100);
            ViewBag.CarbCal = Math.Round(foodReferences.Carbohydrate * 4 / foodReferences.Calorie*100);
            ViewBag.ProtCal = Math.Round(foodReferences.Protein*4 / foodReferences.Calorie*100);
            if (foodReferences.Fat == 0 && foodReferences.Protein == 0 && foodReferences.Carbohydrate == 0)
            {
                ViewBag.CarbCal = 100;
                return View(foodReferences);
            }
            return View(foodReferences);
        }

        // GET: FoodBase/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FoodBase/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FoodName,Brand,Size,Calorie,Fat,Carbohydrate,Protein")] FoodReferences foodReferences)
        {
            if (ModelState.IsValid)
            {
                db.FoodReferences.Add(foodReferences);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(foodReferences);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
