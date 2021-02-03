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
    public class DailyPromoterPlanAttachmentsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: DailyPromoterPlanAttachments
        public ActionResult Index()
        {
            var dailyPromoterPlanAttachments = db.DailyPromoterPlanAttachments.Include(d => d.DailyPromoterPlan).Where(d=>d.IsDeleted==false).OrderByDescending(d=>d.CreationDate);
            return View(dailyPromoterPlanAttachments.ToList());
        }

        // GET: DailyPromoterPlanAttachments/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyPromoterPlanAttachment dailyPromoterPlanAttachment = db.DailyPromoterPlanAttachments.Find(id);
            if (dailyPromoterPlanAttachment == null)
            {
                return HttpNotFound();
            }
            return View(dailyPromoterPlanAttachment);
        }

        // GET: DailyPromoterPlanAttachments/Create
        public ActionResult Create()
        {
            ViewBag.DailyPromoterPlanId = new SelectList(db.DailyPromoterPlans, "Id", "Description");
            return View();
        }

        // POST: DailyPromoterPlanAttachments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FileUrl,DailyPromoterPlanId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] DailyPromoterPlanAttachment dailyPromoterPlanAttachment)
        {
            if (ModelState.IsValid)
            {
				dailyPromoterPlanAttachment.IsDeleted=false;
				dailyPromoterPlanAttachment.CreationDate= DateTime.Now; 
                dailyPromoterPlanAttachment.Id = Guid.NewGuid();
                db.DailyPromoterPlanAttachments.Add(dailyPromoterPlanAttachment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DailyPromoterPlanId = new SelectList(db.DailyPromoterPlans, "Id", "Description", dailyPromoterPlanAttachment.DailyPromoterPlanId);
            return View(dailyPromoterPlanAttachment);
        }

        // GET: DailyPromoterPlanAttachments/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyPromoterPlanAttachment dailyPromoterPlanAttachment = db.DailyPromoterPlanAttachments.Find(id);
            if (dailyPromoterPlanAttachment == null)
            {
                return HttpNotFound();
            }
            ViewBag.DailyPromoterPlanId = new SelectList(db.DailyPromoterPlans, "Id", "Description", dailyPromoterPlanAttachment.DailyPromoterPlanId);
            return View(dailyPromoterPlanAttachment);
        }

        // POST: DailyPromoterPlanAttachments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FileUrl,DailyPromoterPlanId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] DailyPromoterPlanAttachment dailyPromoterPlanAttachment)
        {
            if (ModelState.IsValid)
            {
				dailyPromoterPlanAttachment.IsDeleted=false;
                db.Entry(dailyPromoterPlanAttachment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DailyPromoterPlanId = new SelectList(db.DailyPromoterPlans, "Id", "Description", dailyPromoterPlanAttachment.DailyPromoterPlanId);
            return View(dailyPromoterPlanAttachment);
        }

        // GET: DailyPromoterPlanAttachments/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyPromoterPlanAttachment dailyPromoterPlanAttachment = db.DailyPromoterPlanAttachments.Find(id);
            if (dailyPromoterPlanAttachment == null)
            {
                return HttpNotFound();
            }
            return View(dailyPromoterPlanAttachment);
        }

        // POST: DailyPromoterPlanAttachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            DailyPromoterPlanAttachment dailyPromoterPlanAttachment = db.DailyPromoterPlanAttachments.Find(id);
			dailyPromoterPlanAttachment.IsDeleted=true;
			dailyPromoterPlanAttachment.DeletionDate=DateTime.Now;
 
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteByPromoter(Guid id)
        {
            DailyPromoterPlanAttachment dailyPromoterPlanAttachment = db.DailyPromoterPlanAttachments.Find(id);
			dailyPromoterPlanAttachment.IsDeleted=true;
			dailyPromoterPlanAttachment.DeletionDate=DateTime.Now;
 
            db.SaveChanges();
            return RedirectToAction("PromoterDailyPlanAction","DailyPromoterPlans");
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
