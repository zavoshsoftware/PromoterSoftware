using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;

namespace PromoterSoftware.Controllers
{
    public class DailyPromoterProductSalesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index()
        {
            var dailyPromoterProductSales = db.DailyPromoterProductSales.Include(d => d.DailyPromoterPlan).Where(d => d.IsDeleted == false).OrderByDescending(d => d.CreationDate).Include(d => d.ProjectProduct).Where(d => d.IsDeleted == false).OrderByDescending(d => d.CreationDate);
            return View(dailyPromoterProductSales.ToList());
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyPromoterProductSale dailyPromoterProductSale = db.DailyPromoterProductSales.Find(id);
            if (dailyPromoterProductSale == null)
            {
                return HttpNotFound();
            }
            return View(dailyPromoterProductSale);
        }

        public ActionResult Create()
        {
            ViewBag.DailyPromoterPlanId = new SelectList(db.DailyPromoterPlans, "Id", "StartLat");
            ViewBag.ProjectProductId = new SelectList(db.ProjectProducts, "Id", "ProductTitle");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DailyPromoterProductSale dailyPromoterProductSale)
        {
            if (ModelState.IsValid)
            {
                dailyPromoterProductSale.IsDeleted = false;
                dailyPromoterProductSale.CreationDate = DateTime.Now;
                dailyPromoterProductSale.Id = Guid.NewGuid();
                db.DailyPromoterProductSales.Add(dailyPromoterProductSale);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DailyPromoterPlanId = new SelectList(db.DailyPromoterPlans, "Id", "StartLat", dailyPromoterProductSale.DailyPromoterPlanId);
            ViewBag.ProjectProductId = new SelectList(db.ProjectProducts, "Id", "ProductTitle", dailyPromoterProductSale.ProjectProductId);
            return View(dailyPromoterProductSale);
        }


        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyPromoterProductSale dailyPromoterProductSale = db.DailyPromoterProductSales.Find(id);
            if (dailyPromoterProductSale == null)
            {
                return HttpNotFound();
            }
            ViewBag.DailyPromoterPlanId = new SelectList(db.DailyPromoterPlans, "Id", "StartLat", dailyPromoterProductSale.DailyPromoterPlanId);
            ViewBag.ProjectProductId = new SelectList(db.ProjectProducts, "Id", "ProductTitle", dailyPromoterProductSale.ProjectProductId);
            return View(dailyPromoterProductSale);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DailyPromoterProductSale dailyPromoterProductSale)
        {
            if (ModelState.IsValid)
            {
                dailyPromoterProductSale.IsDeleted = false;
                db.Entry(dailyPromoterProductSale).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DailyPromoterPlanId = new SelectList(db.DailyPromoterPlans, "Id", "StartLat", dailyPromoterProductSale.DailyPromoterPlanId);
            ViewBag.ProjectProductId = new SelectList(db.ProjectProducts, "Id", "ProductTitle", dailyPromoterProductSale.ProjectProductId);
            return View(dailyPromoterProductSale);
        }


        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyPromoterProductSale dailyPromoterProductSale = db.DailyPromoterProductSales.Find(id);
            if (dailyPromoterProductSale == null)
            {
                return HttpNotFound();
            }
            return View(dailyPromoterProductSale);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            DailyPromoterProductSale dailyPromoterProductSale = db.DailyPromoterProductSales.Find(id);
            dailyPromoterProductSale.IsDeleted = true;
            dailyPromoterProductSale.DeletionDate = DateTime.Now;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult SubmitSale(string productId, string dailyPromoterPlanId, string qty, string desc)
        {
            try
            {
                Guid id = new Guid(dailyPromoterPlanId);

                Guid projectProductId = new Guid(productId);

                DailyPromoterProductSale dailyPromoterProductSale = db.DailyPromoterProductSales.FirstOrDefault(c =>
                    c.DailyPromoterPlanId == id && c.ProjectProductId == projectProductId && c.IsDeleted == false);

                if (dailyPromoterProductSale == null)
                {
                    DailyPromoterProductSale oDailyPromoterProductSale = new DailyPromoterProductSale()
                    {
                        Id = Guid.NewGuid(),
                        ProjectProductId = projectProductId,
                        DailyPromoterPlanId = id,
                        Count = Convert.ToInt32(qty),
                        PromoterDescription = desc,
                        IsActive = true,
                        IsDeleted = false,
                        CreationDate = DateTime.Now
                    };

                    db.DailyPromoterProductSales.Add(oDailyPromoterProductSale);
                }
                else
                {
                    dailyPromoterProductSale.Count = Convert.ToInt32(qty);
                    dailyPromoterProductSale.PromoterDescription = desc;
                    dailyPromoterProductSale.LastModifiedDate = DateTime.Now;
                }

                db.SaveChanges();

                return Json("true", JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }
    }
}
