using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Helpers;
using Models;
using ViewModels;

namespace PromoterSoftware.Controllers
{
    public class ProjectDetailsController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index(Guid id)
        {
            var projectDetails = db.ProjectDetails.Include(p => p.Project)
                .Where(p => p.ProjectId == id && p.IsDeleted == false).Include(p => p.Store).Include(p => p.User)
                .OrderByDescending(p => p.CreationDate);

            var project = db.Projects.Where(c => c.Id == id).Select(c => c.Title);

            ViewBag.Title = "جزییات پروژه " + project.FirstOrDefault();
            return View(projectDetails.ToList());
        }

        public ActionResult Create(Guid id)
        {
            ViewBag.ProjectId = id;
            ViewBag.ProvinceId = new SelectList(db.Provinces.OrderBy(c => c.Title), "Id", "Title");
            ViewBag.StoreId = new SelectList(db.Stores, "Id", "Title");
            ViewBag.UserId = new SelectList(db.Users.Where(c => c.Role.Name == "supervisor" && c.IsDeleted == false && c.IsActive), "Id", "FullName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProjectDetail projectDetail, Guid id)
        {
            if (ModelState.IsValid)
            {
                projectDetail.ProjectId = id;
                projectDetail.IsDeleted = false;
                projectDetail.CreationDate = DateTime.Now;
                projectDetail.Id = Guid.NewGuid();
                db.ProjectDetails.Add(projectDetail);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = id });
            }

            ViewBag.ProjectId = id;
            ViewBag.StoreId = new SelectList(db.Stores, "Id", "Title", projectDetail.StoreId);
            ViewBag.UserId = new SelectList(db.Users.Where(c => c.Role.Name == "supervisor" && c.IsDeleted == false && c.IsActive), "Id", "FullName", projectDetail.UserId);
            return View(projectDetail);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectDetail projectDetail = db.ProjectDetails.Find(id);
            if (projectDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectId = projectDetail.ProjectId;
            ViewBag.StoreId = new SelectList(db.Stores, "Id", "Title", projectDetail.StoreId);
            ViewBag.UserId = new SelectList(db.Users.Where(c => c.Role.Name == "supervisor" && c.IsDeleted == false && c.IsActive), "Id", "FullName", projectDetail.UserId);
            return View(projectDetail);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProjectDetail projectDetail)
        {
            if (ModelState.IsValid)
            {
                projectDetail.IsDeleted = false;
                db.Entry(projectDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = projectDetail.ProjectId });
            }
            ViewBag.ProjectId = projectDetail.ProjectId;
            ViewBag.StoreId = new SelectList(db.Stores, "Id", "Title", projectDetail.StoreId);
            ViewBag.UserId = new SelectList(db.Users.Where(c => c.Role.Name == "supervisor" && c.IsDeleted == false && c.IsActive), "Id", "FullName", projectDetail.UserId);
            return View(projectDetail);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectDetail projectDetail = db.ProjectDetails.Find(id);
            if (projectDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectId = projectDetail.ProjectId;

            return View(projectDetail);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ProjectDetail projectDetail = db.ProjectDetails.Find(id);
            projectDetail.IsDeleted = true;
            projectDetail.DeletionDate = DateTime.Now;

            db.SaveChanges();
            return RedirectToAction("Index", new { id = projectDetail.ProjectId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult FillCities(string id)
        {
            Guid provinceId = new Guid(id);
            var cities = db.Cities.Where(c => c.ProvinceId == provinceId).OrderBy(current => current.Title).ToList();
            List<DroupDownItemViewModel> cityItems = new List<DroupDownItemViewModel>();
            foreach (Models.City city in cities)
            {
                cityItems.Add(new DroupDownItemViewModel()
                {
                    Text = city.Title,
                    Value = city.Id.ToString()
                });
            }
            return Json(cityItems, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillStores(string id)
        {
            Guid cityId = new Guid(id);
            var stores = db.Stores.Where(c => c.CityId == cityId).OrderBy(current => current.Title).ToList();
            List<DroupDownItemViewModel> cityItems = new List<DroupDownItemViewModel>();
            foreach (Models.Store store in stores)
            {
                cityItems.Add(new DroupDownItemViewModel()
                {
                    Text = store.Title,
                    Value = store.Id.ToString()
                });
            }
            return Json(cityItems, JsonRequestBehavior.AllowGet);
        }


        #region SupervisorList

        public ActionResult SupervisorIndex(Guid id)
        {
            User user = GetUserInfo.GetUser();

            var projectDetails = db.ProjectDetails.Include(p => p.Project)
                .Where(p => p.ProjectId == id && p.UserId == user.Id && p.IsDeleted == false)
                .Include(p => p.Store).OrderByDescending(p => p.CreationDate);

            var project = db.Projects.Where(c => c.Id == id).Select(c => c.Title);

            ViewBag.Title = "جزییات پروژه " + project.FirstOrDefault();
            return View(projectDetails.ToList());
        }

        #endregion
    }
}
