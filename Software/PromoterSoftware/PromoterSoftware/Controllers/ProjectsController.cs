using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Helpers;
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
         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Project project)
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


        #region Supervisor

        public ActionResult SupervisorIndex()
        {
            User user = GetUserInfo.GetUser();

            List<Project> projects=new List<Project>();

            if (user != null)
            {
                var projectDetails = db.ProjectDetails.Where(c => c.UserId == user.Id).Select(c => new
                {
                    c.ProjectId,
                    c.UserId
                });

                foreach (var projectDetail in projectDetails)
                {
                    if (projects.All(c => c.Id != projectDetail.ProjectId))
                    {
                        projects.Add(db.Projects.Find(projectDetail.ProjectId));
                    }
                }

            }

            return View(projects);
        }


        #endregion
    }
}
