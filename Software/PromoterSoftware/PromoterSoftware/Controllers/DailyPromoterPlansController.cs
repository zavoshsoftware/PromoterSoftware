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
    public class DailyPromoterPlansController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: DailyPromoterPlans
        public ActionResult Index(Guid id)
        {
            var dailyPromoterPlans = db.DailyPromoterPlans.Include(d => d.ProjectDetailPromoter).Where(d => d.ProjectDetailPromoterId == id && d.IsDeleted == false).OrderByDescending(d => d.ShiftDate);

            ProjectDetailPromoter projectDetailPromoter = db.ProjectDetailPromoters.Find(id);

            ViewBag.Title = "برنامه روزانه " + projectDetailPromoter.User.FullName + " در فروشگاه " +
                            projectDetailPromoter.ProjectDetail.Store.Title;

            return View(dailyPromoterPlans.ToList());
        }
         
        public ActionResult Create(Guid id)
        {
            ProjectDetailPromoter projectDetailPromoter = db.ProjectDetailPromoters.Find(id);

            ViewBag.Title = "افزودن به برنامه روزانه " + projectDetailPromoter.User.FullName + " در فروشگاه " +
                            projectDetailPromoter.ProjectDetail.Store.Title;


            ViewBag.ProjectDetailPromoterId = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DailyPromoterPlan dailyPromoterPlan,Guid id)
        {
            if (ModelState.IsValid)
            {
                dailyPromoterPlan.IsActive = true;
                dailyPromoterPlan.ProjectDetailPromoterId = id;
                dailyPromoterPlan.IsDeleted = false;
                dailyPromoterPlan.CreationDate = DateTime.Now;
                dailyPromoterPlan.Id = Guid.NewGuid();
                db.DailyPromoterPlans.Add(dailyPromoterPlan);
                db.SaveChanges();
                return RedirectToAction("Index",new{id=id});
            }

            ViewBag.ProjectDetailPromoterId =id;
            return View(dailyPromoterPlan);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyPromoterPlan dailyPromoterPlan = db.DailyPromoterPlans.Find(id);
            if (dailyPromoterPlan == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectDetailPromoterId =   dailyPromoterPlan.ProjectDetailPromoterId ;
            return View(dailyPromoterPlan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DailyPromoterPlan dailyPromoterPlan)
        {
            if (ModelState.IsValid)
            {
                if (dailyPromoterPlan.StartHour == 0 && dailyPromoterPlan.StartMin == 0)
                {
                    dailyPromoterPlan.StartHour = null;
                    dailyPromoterPlan.StartMin = null;
                }
               
                if (dailyPromoterPlan.FinishHour == 0 && dailyPromoterPlan.FinishMin == 0)
                {
                    dailyPromoterPlan.FinishHour = null;
                    dailyPromoterPlan.FinishMin = null;
                }
                dailyPromoterPlan.IsDeleted = false;
                db.Entry(dailyPromoterPlan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index",new{id= dailyPromoterPlan.ProjectDetailPromoterId });
            }
            ViewBag.ProjectDetailPromoterId = dailyPromoterPlan.ProjectDetailPromoterId;
            return View(dailyPromoterPlan);
        }

        // GET: DailyPromoterPlans/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyPromoterPlan dailyPromoterPlan = db.DailyPromoterPlans.Find(id);
            if (dailyPromoterPlan == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectDetailPromoterId = dailyPromoterPlan.ProjectDetailPromoterId;

            return View(dailyPromoterPlan);
        }

        // POST: DailyPromoterPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            DailyPromoterPlan dailyPromoterPlan = db.DailyPromoterPlans.Find(id);
            dailyPromoterPlan.IsDeleted = true;
            dailyPromoterPlan.DeletionDate = DateTime.Now;

            db.SaveChanges();
            return RedirectToAction("Index", new { id = dailyPromoterPlan.ProjectDetailPromoterId });
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
