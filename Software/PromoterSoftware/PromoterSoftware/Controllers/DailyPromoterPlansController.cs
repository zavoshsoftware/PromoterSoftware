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
using ViewModels;

namespace PromoterSoftware.Controllers
{
    public class DailyPromoterPlansController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

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
        public ActionResult Create(DailyPromoterPlan dailyPromoterPlan, Guid id)
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
                return RedirectToAction("Index", new { id = id });
            }

            ViewBag.ProjectDetailPromoterId = id;
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
            ViewBag.ProjectDetailPromoterId = dailyPromoterPlan.ProjectDetailPromoterId;
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
                return RedirectToAction("Index", new { id = dailyPromoterPlan.ProjectDetailPromoterId });
            }
            ViewBag.ProjectDetailPromoterId = dailyPromoterPlan.ProjectDetailPromoterId;
            return View(dailyPromoterPlan);
        }


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

        #region Promoter


        [Authorize(Roles = "promoter")]
        public ActionResult PromoterIndex()
        {
            User user = GetUserInfo.GetUser();

            var dailyPromoterPlans = db.DailyPromoterPlans.Include(d => d.ProjectDetailPromoter)
                .Where(d => d.ProjectDetailPromoter.UserId == user.Id && d.IsDeleted == false)
                .OrderByDescending(d => d.ShiftDate);


       

            ViewBag.Title = "برنامه روزانه ";

            return View(dailyPromoterPlans.ToList());
        }


        [Authorize(Roles = "promoter")]
        public ActionResult PromoterDailyPlanAction()
        {
            PromoterDailyPlanActionViewModel result = new PromoterDailyPlanActionViewModel() { IsStart = false };
            User user = GetUserInfo.GetUser();

            DailyPromoterPlan dailyPromoterPlan = db.DailyPromoterPlans.FirstOrDefault(c =>
                c.ProjectDetailPromoter.UserId == user.Id && c.ShiftDate == DateTime.Today);

            if (dailyPromoterPlan != null)
            {
                ViewBag.dailyPromoterPlanId = dailyPromoterPlan.Id;

                if (dailyPromoterPlan.StartHour != null)
                    result.IsStart = true;

                if (dailyPromoterPlan.FinishHour != null)
                    result.IsFinish = true;

                result.StartTime = dailyPromoterPlan.StartHourStr;
                result.FinishTime = dailyPromoterPlan.FinishHourStr;
                result.ShiftDate = dailyPromoterPlan.ShiftDateStr;

                Project project = dailyPromoterPlan.ProjectDetailPromoter.ProjectDetail.Project;
                result.ProjectTitle = project.Title;
                result.BrandTitle = project.Customer.Title;
                result.Description = project.Body;
                result.StoreTitle = dailyPromoterPlan.ProjectDetailPromoter.ProjectDetail.Store.Title;
                result.ProjectProducts = GetProjectProduct(project.Id, dailyPromoterPlan.Id);
                result.DailyPromoterPlanAttachments = db.DailyPromoterPlanAttachments.Where(c =>
                        c.DailyPromoterPlanId == dailyPromoterPlan.Id && c.IsDeleted == false && c.IsActive == true)
                    .ToList();

                result.SupervisorFullname = dailyPromoterPlan.ProjectDetailPromoter.User.FullName;
                result.ProjectAttachments = GetProjectAttachment(project.Id);
            }

            else
            {
                return RedirectToAction("NoPlan");
            }
            return View(result);
        }

        [Authorize(Roles = "promoter")]
        public ActionResult NoPlan()
        {
            return View();
        }
        [Authorize(Roles = "promoter")]
        public ActionResult PromoterDailyPlanActionById(Guid id)
        {
            PromoterDailyPlanActionViewModel result = new PromoterDailyPlanActionViewModel() { IsStart = false };
            User user = GetUserInfo.GetUser();

            DailyPromoterPlan dailyPromoterPlan = db.DailyPromoterPlans.Find(id);

            if (dailyPromoterPlan != null)
            {

                if (dailyPromoterPlan.ShiftDate == DateTime.Today)
                    return RedirectToAction("PromoterDailyPlanAction");

                ViewBag.dailyPromoterPlanId = dailyPromoterPlan.Id;

                if (dailyPromoterPlan.StartHour != null)
                    result.IsStart = true;

                if (dailyPromoterPlan.FinishHour != null)
                    result.IsFinish = true;

                result.StartTime = dailyPromoterPlan.StartHourStr;
                result.FinishTime = dailyPromoterPlan.FinishHourStr;
                result.ShiftDate = dailyPromoterPlan.ShiftDateStr;

                Project project = dailyPromoterPlan.ProjectDetailPromoter.ProjectDetail.Project;
                result.ProjectTitle = project.Title;
                result.BrandTitle = project.Customer.Title;
                result.Description = project.Body;
                result.StoreTitle = dailyPromoterPlan.ProjectDetailPromoter.ProjectDetail.Store.Title;
                result.ProjectProducts = GetProjectProduct(project.Id, dailyPromoterPlan.Id);
                result.DailyPromoterPlanAttachments = db.DailyPromoterPlanAttachments.Where(c =>
                        c.DailyPromoterPlanId == dailyPromoterPlan.Id && c.IsDeleted == false && c.IsActive == true)
                    .ToList();

                result.SupervisorFullname = dailyPromoterPlan.ProjectDetailPromoter.User.FullName;
                result.ProjectAttachments = GetProjectAttachment(project.Id);
            }

            return View(result);
        }

        public List<ProjectProductItem> GetProjectProduct(Guid projectId, Guid dailyPromoterPlanId)
        {
            List<ProjectProductItem> result = new List<ProjectProductItem>();

            var projectProducts = db.ProjectProducts
                 .Where(c => c.ProjectId == projectId && c.IsDeleted == false && c.IsActive).Select(c => new
                 {
                     c.Id,
                     c.ProductTitle,
                     c.ImageUrl,
                     c.ProductDescription
                 });

            foreach (var projectProduct in projectProducts)
            {
                int count = 0;
                string promoterDescription = null;

                DailyPromoterProductSale dailyPromoterProductSale = db.DailyPromoterProductSales.FirstOrDefault(c =>
                    c.DailyPromoterPlanId == dailyPromoterPlanId && c.ProjectProductId == projectProduct.Id &&
                    c.IsDeleted == false);

                if (dailyPromoterProductSale != null)
                {
                    count = dailyPromoterProductSale.Count;
                    promoterDescription = dailyPromoterProductSale.PromoterDescription;
                }
                result.Add(new ProjectProductItem()
                {
                    ProductId = projectProduct.Id,
                    ImageUrl = projectProduct.ImageUrl,
                    ProductDescription = projectProduct.ProductDescription,
                    ProductTitle = projectProduct.ProductTitle,
                    Count = count,
                    PromoterDesc = promoterDescription
                });
            }

            return result;
        }

        public List<string> GetProjectAttachment(Guid projectId)
        {
            List<string> result = new List<string>();
            var projectAttachments =
                 db.ProjectAttachments.Where(c => c.ProjectId == projectId && c.IsDeleted == false && c.IsActive).Select(c => c.FileUrl);

            foreach (var attachment in projectAttachments)
            {
                result.Add(attachment);
            }

            return result;
        }


        [HttpPost, ActionName("PromoterDailyPlanAction")]
        [ValidateAntiForgeryToken]
        public ActionResult PromoterDailyPlanAction(List<HttpPostedFileBase> attachments)
        {
            #region Upload and resize image if needed

            if (attachments != null)
            {
                foreach (HttpPostedFileBase t in attachments)
                {
                    if (t != null)
                    {
                        User user = GetUserInfo.GetUser();

                        DailyPromoterPlan dailyPromoterPlan = db.DailyPromoterPlans.FirstOrDefault(c =>
                            c.ProjectDetailPromoter.UserId == user.Id && c.ShiftDate == DateTime.Today);

                        if (dailyPromoterPlan != null)
                        {
                            string filename = Path.GetFileName(t.FileName);
                            string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                                 + Path.GetExtension(filename);

                            string newFilenameUrl = "/Uploads/PromoterAttachment/" + newFilename;
                            string physicalFilename = Server.MapPath(newFilenameUrl);

                            t.SaveAs(physicalFilename);

                            DailyPromoterPlanAttachment dailyPromoterPlanAttachment = new DailyPromoterPlanAttachment()
                            {
                                Id = Guid.NewGuid(),
                                FileUrl = newFilenameUrl,
                                CreationDate = DateTime.Now,
                                DailyPromoterPlanId = dailyPromoterPlan.Id,
                                IsActive = true,
                                IsDeleted = false,
                            };
                            db.DailyPromoterPlanAttachments.Add(dailyPromoterPlanAttachment);
                        }
                    }
                }

                db.SaveChanges();
            }

            #endregion



            return RedirectToAction("PromoterDailyPlanAction");
        }




        [AllowAnonymous]
        [HttpPost]
        public ActionResult SubmitLocation(string latitude, string longitude, string requestType, string id)
        {
            try
            {
                Guid dailyPromoterPlanId = new Guid(id);

                DailyPromoterPlan dailyPromoterPlan = db.DailyPromoterPlans.Find(dailyPromoterPlanId);

                if (requestType == "start")
                {
                    dailyPromoterPlan.StartLat = latitude;
                    dailyPromoterPlan.StartLong = longitude;
                    dailyPromoterPlan.StartHour = DateTime.Now.Hour;
                    dailyPromoterPlan.StartMin = DateTime.Now.Minute;
                }
                else
                {
                    dailyPromoterPlan.FinishLat = latitude;
                    dailyPromoterPlan.FinishLong = longitude;
                    dailyPromoterPlan.FinishHour = DateTime.Now.Hour;
                    dailyPromoterPlan.FinishMin = DateTime.Now.Minute;
                }

                db.SaveChanges();

                return Json("true", JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult ShowMap(string mapLat, string mapLong)
        {
            ViewBag.mapLat = mapLat;
            ViewBag.mapLong = mapLong;
            return View();
        }
        #endregion
    }
}
