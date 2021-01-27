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
    public class StoresController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Stores
        public ActionResult Index()
        {
            var stores = db.Stores.Include(s => s.City).Where(s=>s.IsDeleted==false).OrderByDescending(s=>s.CreationDate);
            return View(stores.ToList());
        }

     
        public ActionResult Create()
        {
            ViewBag.CityId = new SelectList(db.Cities.OrderBy(c => c.Title), "Id", "Title");
            return View();
        }

        // POST: Stores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,CityId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] Store store)
        {
            if (ModelState.IsValid)
            {
				store.IsDeleted=false;
				store.CreationDate= DateTime.Now; 
                store.Id = Guid.NewGuid();
                db.Stores.Add(store);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityId = new SelectList(db.Cities.OrderBy(c=>c.Title), "Id", "Title", store.CityId);
            return View(store);
        }

        // GET: Stores/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Store store = db.Stores.Find(id);
            if (store == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(db.Cities.OrderBy(c => c.Title), "Id", "Title", store.CityId);
            return View(store);
        }

        // POST: Stores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,CityId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] Store store)
        {
            if (ModelState.IsValid)
            {
				store.IsDeleted=false;
                db.Entry(store).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(db.Cities.OrderBy(c => c.Title), "Id", "Title", store.CityId);
            return View(store);
        }

        // GET: Stores/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Store store = db.Stores.Find(id);
            if (store == null)
            {
                return HttpNotFound();
            }
            return View(store);
        }

        // POST: Stores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Store store = db.Stores.Find(id);
			store.IsDeleted=true;
			store.DeletionDate=DateTime.Now;
 
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
    }
}
