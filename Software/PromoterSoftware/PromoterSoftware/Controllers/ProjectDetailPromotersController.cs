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
    public class ProjectDetailPromotersController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index(Guid id)
        {
            ProjectDetailPromoterViewModel result = new ProjectDetailPromoterViewModel();
            List<ProjectDetailPromoter> projectDetailPromoters = db.ProjectDetailPromoters
                .Where(p => p.ProjectDetailId == id && p.IsDeleted == false).OrderByDescending(p => p.CreationDate)
                .ToList();


            var projectDetail = db.ProjectDetails.Find(id);

            if (projectDetail != null)
            {
                ViewBag.UserId = new SelectList(db.Users.Where(c => c.Role.Name == "promoter" && c.CityId == projectDetail.Store.CityId), "Id",
                    "FullName");

                ViewBag.Title = "فهرست پروموترهای پروژه " + projectDetail.Project.Title + " فروشگاه " +
                                projectDetail.Store.Title;

                result.ProjectDetailPromoters = projectDetailPromoters;
                result.StartDate = projectDetail.Project.StartDateStr;
                result.EndDate = projectDetail.Project.EndDateStr;
                result.SalaryPerHour = projectDetail.SalaryPerHourStr;
                result.TransportationAmount = projectDetail.TransportationAmountStr;
                result.StartHour = projectDetail.StartHourStr;
                result.FinishHour = projectDetail.FinishHourStr;
                result.ProjectDetailId = id;
            }

            return View(result);
        }

        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePromoter(ProjectDetailPromoterViewModel input)
        {
            if (db.ProjectDetailPromoters.Any(c =>
                c.UserId == input.UserId && c.ProjectDetailId == input.ProjectDetailId && c.IsDeleted == false))
            {
                TempData["invalidPromoter"] = "پروموتر انتخابی قبلا برای این پروژه انتخاب شده است. پروموتر دیگری را انتخاب کنید.";
                return RedirectToAction("index", new { id = input.ProjectDetailId });

            }

            ProjectDetailPromoter projectDetailPromoter = new ProjectDetailPromoter()
            {
                Id = Guid.NewGuid(),
                IsActive = true,
                IsDeleted = false,
                IsFullTime = true,
                ProjectDetailId = input.ProjectDetailId,
                UserId = input.UserId.Value,
                CreationDate = DateTime.Now,
            };
            db.ProjectDetailPromoters.Add(projectDetailPromoter);

            var projectDetail = db.ProjectDetails.Find(input.ProjectDetailId);

            string totalTimeSpan = (projectDetail.Project.EndDate.Value - projectDetail.Project.StartDate).ToString();

            int totalDays = Convert.ToInt32(totalTimeSpan.Split('.')[0]);
            for (int i = 0; i <= totalDays; i++)
            {
                DailyPromoterPlan dailyPromoterPlan = new DailyPromoterPlan()
                {
                    Id = Guid.NewGuid(),
                    ProjectDetailPromoterId = projectDetailPromoter.Id,
                    ShiftDate = projectDetail.Project.StartDate.AddDays(i),
                    CreationDate = DateTime.Now,
                    IsDeleted = false,
                    IsActive = true,

                };

                db.DailyPromoterPlans.Add(dailyPromoterPlan);
            }

            db.SaveChanges();
            return RedirectToAction("index", new { id = input.ProjectDetailId });
        }

        public ActionResult Create()
        {
            ViewBag.ProjectDetailId = new SelectList(db.ProjectDetails, "Id", "Description");
            ViewBag.UserId = new SelectList(db.Users, "Id", "FullName");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProjectDetailPromoter projectDetailPromoter)
        {
            if (ModelState.IsValid)
            {
                projectDetailPromoter.IsDeleted = false;
                projectDetailPromoter.CreationDate = DateTime.Now;
                projectDetailPromoter.Id = Guid.NewGuid();
                db.ProjectDetailPromoters.Add(projectDetailPromoter);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectDetailId = new SelectList(db.ProjectDetails, "Id", "Description", projectDetailPromoter.ProjectDetailId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FullName", projectDetailPromoter.UserId);
            return View(projectDetailPromoter);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectDetailPromoter projectDetailPromoter = db.ProjectDetailPromoters.Find(id);
            if (projectDetailPromoter == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectDetailId = new SelectList(db.ProjectDetails, "Id", "Description", projectDetailPromoter.ProjectDetailId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FullName", projectDetailPromoter.UserId);
            return View(projectDetailPromoter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProjectDetailPromoter projectDetailPromoter)
        {
            if (ModelState.IsValid)
            {
                projectDetailPromoter.IsDeleted = false;
                db.Entry(projectDetailPromoter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectDetailId = new SelectList(db.ProjectDetails, "Id", "Description", projectDetailPromoter.ProjectDetailId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FullName", projectDetailPromoter.UserId);
            return View(projectDetailPromoter);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectDetailPromoter projectDetailPromoter = db.ProjectDetailPromoters.Find(id);
            if (projectDetailPromoter == null)
            {
                return HttpNotFound();
            }
            return View(projectDetailPromoter);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ProjectDetailPromoter projectDetailPromoter = db.ProjectDetailPromoters.Find(id);
            projectDetailPromoter.IsDeleted = true;
            projectDetailPromoter.DeletionDate = DateTime.Now;

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
