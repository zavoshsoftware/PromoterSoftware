using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;

namespace PromoterSoftware.Controllers
{
    public class ProjectProductsController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index(Guid id)
        {
            var projectProducts = db.ProjectProducts.Include(p => p.Project).Where(p => p.ProjectId == id && p.IsDeleted == false).OrderByDescending(p => p.CreationDate);

            var project = db.Projects.Where(c => c.Id == id).Select(c => c.Title);

            ViewBag.Title = "محصولات پروژه " + project.FirstOrDefault();
            return View(projectProducts.ToList());
        }

        public ActionResult List(Guid id)
        {
            var projectProducts = db.ProjectProducts.Include(p => p.Project).Where(p => p.ProjectId == id && p.IsDeleted == false).OrderByDescending(p => p.CreationDate);

            var project = db.Projects.Where(c => c.Id == id).Select(c => c.Title);

            ViewBag.Title = "محصولات پروژه " + project.FirstOrDefault();
            return View(projectProducts.ToList());
        }

        public ActionResult Create(Guid id)
        {
            ViewBag.ProjectId = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProjectProduct projectProduct, Guid id, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                string newFilenameUrl = string.Empty;
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    newFilenameUrl = "/Uploads/ProjectProduct/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    fileupload.SaveAs(physicalFilename);
                    projectProduct.ImageUrl = newFilenameUrl;
                }
                #endregion

                projectProduct.ProjectId = id;
                projectProduct.IsDeleted = false;
                projectProduct.CreationDate = DateTime.Now;
                projectProduct.Id = Guid.NewGuid();
                db.ProjectProducts.Add(projectProduct);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = id });
            }

            ViewBag.ProjectId = id;
            return View(projectProduct);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectProduct projectProduct = db.ProjectProducts.Find(id);
            if (projectProduct == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectId = projectProduct.ProjectId;
            return View(projectProduct);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProjectProduct projectProduct, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                string newFilenameUrl = string.Empty;
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    newFilenameUrl = "/Uploads/ProjectProduct/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    fileupload.SaveAs(physicalFilename);
                    projectProduct.ImageUrl = newFilenameUrl;
                }
                #endregion
                projectProduct.IsDeleted = false;
                db.Entry(projectProduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = projectProduct.ProjectId });
            }
            ViewBag.ProjectId = projectProduct.ProjectId;
            return View(projectProduct);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectProduct projectProduct = db.ProjectProducts.Find(id);
            if (projectProduct == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectId = projectProduct.ProjectId;

            return View(projectProduct);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ProjectProduct projectProduct = db.ProjectProducts.Find(id);
            projectProduct.IsDeleted = true;
            projectProduct.DeletionDate = DateTime.Now;

            db.SaveChanges();
            return RedirectToAction("Index", new { id = projectProduct.ProjectId });
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
