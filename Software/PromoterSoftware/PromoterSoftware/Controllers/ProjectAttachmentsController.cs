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
    public class ProjectAttachmentsController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index(Guid id)
        {
            Project project = db.Projects.Find(id);

            ViewBag.Title = "ضمایم پروژه " + project.Title;
            var projectAttachments = db.ProjectAttachments.Include(p => p.Project).Where(p => p.ProjectId == id && p.IsDeleted == false).OrderByDescending(p => p.CreationDate);
            return View(projectAttachments.ToList());
        }


        public ActionResult Create(Guid id)
        {
            ViewBag.ProjectId = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProjectAttachment projectAttachment, HttpPostedFileBase fileupload, Guid id)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/ProjectAttachments/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    fileupload.SaveAs(physicalFilename);
                    projectAttachment.FileUrl = newFilenameUrl;
                }
                #endregion
                projectAttachment.ProjectId = id;
                projectAttachment.IsDeleted = false;
                projectAttachment.CreationDate = DateTime.Now;
                projectAttachment.Id = Guid.NewGuid();
                db.ProjectAttachments.Add(projectAttachment);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = id });
            }

            ViewBag.ProjectId = projectAttachment.ProjectId;
            return View(projectAttachment);
        }

        // GET: ProjectAttachments/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectAttachment projectAttachment = db.ProjectAttachments.Find(id);
            if (projectAttachment == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectId = projectAttachment.ProjectId;
            return View(projectAttachment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProjectAttachment projectAttachment, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/ProjectAttachments/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    fileupload.SaveAs(physicalFilename);
                    projectAttachment.FileUrl = newFilenameUrl;
                }
                #endregion
                projectAttachment.IsDeleted = false;
                db.Entry(projectAttachment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = projectAttachment.ProjectId });
            }
            ViewBag.ProjectId = projectAttachment.ProjectId;
            return View(projectAttachment);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectAttachment projectAttachment = db.ProjectAttachments.Find(id);
            if (projectAttachment == null)
            {
                return HttpNotFound();
            }
            return View(projectAttachment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ProjectAttachment projectAttachment = db.ProjectAttachments.Find(id);
            projectAttachment.IsDeleted = true;
            projectAttachment.DeletionDate = DateTime.Now;

            db.SaveChanges();
            return RedirectToAction("Index", new { id = projectAttachment.ProjectId });
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
