using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GrabFit2.Models;

namespace GrabFit2.Controllers
{
    [Authorize]
    public class FoodTrackController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: FoodTrack
        public ActionResult Index()
        {
            return View(db.FoodOrdered.Where(y=>!y.tagged).ToList());
        }

        // GET: FoodTrack/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: FoodTrack/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ID,orderDate,merchantID,merchantName,itemID,itemName,itemDescription,imageURL,itemOrdered,tagged")] FoodOrdered foodOrdered)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.FoodOrdered.Add(foodOrdered);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(foodOrdered);
        //}

        // GET: FoodTrack/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodOrdered foodOrdered = db.FoodOrdered.Find(id);
            if (foodOrdered == null)
            {
                return HttpNotFound();
            }
            return View(foodOrdered);
        }

        // POST: FoodTrack/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,orderDate,merchantID,merchantName,itemID,itemName,itemDescription,imageURL,itemOrdered,tagged")] FoodOrdered foodOrdered)
        {
            if (ModelState.IsValid)
            {
                FoodOrdered order = db.FoodOrdered.Find(foodOrdered.ID);
                var tag = db.OrderTagged.Where(y => y.OrderID == foodOrdered.ID).First();
                order.itemOrdered = foodOrdered.itemOrdered;
                db.SaveChanges();
                return RedirectToAction("MealDetails","Home",new { id = tag.ID });
            }
            return View(foodOrdered);
        }

        public ActionResult Include(int id)
        {
            var order = db.FoodOrdered.Find(id);
            var q = order.merchantName + " " + order.itemName;
            List<string> bow = q?.Split(' ').ToList();
            List<string> bowPairs = q.Split(' ').Select((s, i) => new { s, i }).GroupBy(n => n.i / 2)
                .Select(g => string.Join(" ", g.Select(p => p.s))).ToList();
            List<string> merchantBow = order.merchantName.Split(' ').ToList();
            List<string> foodBow = order.itemName.Split(' ').ToList();
            var foodbase = db.FoodReferences.ToList();
            var result = new List<ComparisonResultModel>();
            ViewBag.searched = q;
            var score = 0;
            foreach (var item in foodbase)
            {
                score = 0;
                var foodtext = item.FoodName + " " + item.Brand;

                //if every single word from the order merchant name is contained in foodbase brand name
                foreach (var merchantSubs in merchantBow)
                {
                    if (item.Brand != "" && item.Brand != null)
                    {
                        if (item.Brand.Contains(merchantSubs))
                        {
                            score += 1;
                        }
                        else
                        {
                            score += 0;
                        }
                    }
                }
                //if every single word from the order food name is contained in foodbase food name
                foreach (var foodSubs in foodBow)
                {
                    if (item.FoodName.Contains(foodSubs))
                    {
                        score += 1;
                    }
                    else
                    {
                        score += 0;
                    }
                }
                //if every single word is contained n foodtext
                foreach (var subs in bow)
                {
                    if (foodtext.Contains(subs))
                    {
                        score += 1;
                    }
                    else
                    {
                        score += 0;
                    }
                }
                //if every pair of words is contained in foodtext
                foreach (var subs in bowPairs)
                {
                    if (foodtext.Contains(subs))
                    {
                        score += 1;
                    }
                    else
                    {
                        score += 0;
                    }
                }
                //if every single word match exactly with the food name
                foreach (var subs in bow)
                {
                    if (subs == item.FoodName)
                    {
                        score += 1;
                    }
                    else
                    {
                        score += 0;
                    }
                }
                //if pair of words match exactly with the food name
                foreach (var subs in bowPairs)
                {
                    if (subs==item.FoodName)
                    {
                        score += 1;
                    }
                    else
                    {
                        score += 0;
                    }
                }
                result.Add(new ComparisonResultModel { OrderID = item.ID, Score = score });
            }
            var final = result.OrderByDescending(y => y.Score).ToList().First();
            OrderTagged tag = new OrderTagged()
            {
                OrderID = order.ID,
                FoodID = final.OrderID
            };
            db.OrderTagged.Add(tag);
            order.tagged = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult IncludeMenu(int id)
        {
            var order = db.Menu.Find(id);
            FoodOrdered foodOrdered = new FoodOrdered()
            {
                orderDate = DateTime.Today,
                merchantID = 99999,
                merchantName = order.merchantName,
                itemID = 99999,
                itemName = order.itemName,
                itemDescription = order.itemName,
                imageURL = order.imageURL,
                itemOrdered = 1,
                tagged = true
            };
            var q = order.merchantName + " " + order.itemName;
            List<string> bow = q?.Split(' ').ToList();
            List<string> bowPairs = q.Split(' ').Select((s, i) => new { s, i }).GroupBy(n => n.i / 2)
                .Select(g => string.Join(" ", g.Select(p => p.s))).ToList();
            List<string> merchantBow = order.merchantName.Split(' ').ToList();
            List<string> foodBow = order.itemName.Split(' ').ToList();
            var foodbase = db.FoodReferences.ToList();
            var result = new List<ComparisonResultModel>();
            ViewBag.searched = q;
            var score = 0;
            foreach (var item in foodbase)
            {
                score = 0;
                var foodtext = item.FoodName + " " + item.Brand;

                //if every single word from the order merchant name is contained in foodbase brand name
                foreach (var merchantSubs in merchantBow)
                {
                    if (item.Brand != "" && item.Brand != null)
                    {
                        if (item.Brand.Contains(merchantSubs))
                        {
                            score += 1;
                        }
                        else
                        {
                            score += 0;
                        }
                    }
                }
                //if every single word from the order food name is contained in foodbase food name
                foreach (var foodSubs in foodBow)
                {
                    if (item.FoodName.Contains(foodSubs))
                    {
                        score += 1;
                    }
                    else
                    {
                        score += 0;
                    }
                }
                //if every single word is contained n foodtext
                foreach (var subs in bow)
                {
                    if (foodtext.Contains(subs))
                    {
                        score += 1;
                    }
                    else
                    {
                        score += 0;
                    }
                }
                //if every pair of words is contained in foodtext
                foreach (var subs in bowPairs)
                {
                    if (foodtext.Contains(subs))
                    {
                        score += 1;
                    }
                    else
                    {
                        score += 0;
                    }
                }
                //if every single word match exactly with the food name
                foreach (var subs in bow)
                {
                    if (subs == item.FoodName)
                    {
                        score += 1;
                    }
                    else
                    {
                        score += 0;
                    }
                }
                //if pair of words match exactly with the food name
                foreach (var subs in bowPairs)
                {
                    if (subs == item.FoodName)
                    {
                        score += 1;
                    }
                    else
                    {
                        score += 0;
                    }
                }
                result.Add(new ComparisonResultModel { OrderID = item.ID, Score = score });
            }
            var final = result.OrderByDescending(y => y.Score).ToList().First();
            var foodReference = db.FoodReferences.Find(final.OrderID);
            OrderTagged tag = new OrderTagged{FoodReferences = foodReference, FoodOrdered = foodOrdered};
            db.FoodOrdered.Add(foodOrdered);
            db.OrderTagged.Add(tag);
            db.SaveChanges();
            return RedirectToAction("Index","Home",null);
        }

        public ActionResult Merchants()
        {
            var merchants = db.Merchant.ToList();
            return View(merchants);
        }

        public ActionResult Menus(string merchant)
        {
            var menus = db.Menu.Where(y=>y.merchantName==merchant).ToList();
            ViewBag.resto = merchant;
            return View(menus);
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
