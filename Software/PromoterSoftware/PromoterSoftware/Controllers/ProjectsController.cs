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
    public class ProjectsController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index()
        {
            var projects = db.Projects.Include(p => p.Customer).Where(p=>p.IsDeleted==false).OrderByDescending(p=>p.CreationDate);
            return View(projects.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Project project, List<HttpPostedFileBase> attachments)
        {
            if (ModelState.IsValid)
            {

				project.IsDeleted=false;
				project.CreationDate= DateTime.Now; 
                project.Id = Guid.NewGuid();
                db.Projects.Add(project);

                #region Upload and resize image if needed

                if (attachments != null)
                {
                    foreach (HttpPostedFileBase t in attachments)
                    {
                        if (t != null)
                        {
                            string filename = Path.GetFileName(t.FileName);
                            string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                                 + Path.GetExtension(filename);

                            string newFilenameUrl = "/Uploads/ProjectAttachments/" + newFilename;
                            string physicalFilename = Server.MapPath(newFilenameUrl);

                            t.SaveAs(physicalFilename);

                            ProjectAttachment projectAttachment = new ProjectAttachment()
                            {
                                Id = Guid.NewGuid(),
                                FileUrl = newFilenameUrl,
                                CreationDate = DateTime.Now,
                                ProjectId = project.Id,
                                IsActive = true,
                                IsDeleted = false,
                            };
                            db.ProjectAttachments.Add(projectAttachment);
                        }
                    }
                }

                #endregion


                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Title", project.CustomerId);
            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Title", project.CustomerId);
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,StartDate,EndDate,CustomerId,Body,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] Project project)
        {
            if (ModelState.IsValid)
            {
				project.IsDeleted=false;
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Title", project.CustomerId);
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Project project = db.Projects.Find(id);
			project.IsDeleted=true;
			project.DeletionDate=DateTime.Now;
 
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
