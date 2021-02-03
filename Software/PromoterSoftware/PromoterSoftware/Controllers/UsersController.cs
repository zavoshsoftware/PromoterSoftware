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
    public class UsersController : Infrastructure.BaseController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Users
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.City).Where(u => u.IsDeleted == false).OrderByDescending(u => u.CreationDate).Include(u => u.Role).Where(u => u.IsDeleted == false).OrderByDescending(u => u.CreationDate);
            return View(users.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Title");
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Title");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user, HttpPostedFileBase fileupload, HttpPostedFileBase fileUploadAvatar)
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

                    newFilenameUrl = "/Uploads/User/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    fileupload.SaveAs(physicalFilename);
                    user.NationalCardFileUrl = newFilenameUrl;
                }

                if (fileUploadAvatar != null)
                {
                    string filename = Path.GetFileName(fileUploadAvatar.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    newFilenameUrl = "/Uploads/User/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    fileUploadAvatar.SaveAs(physicalFilename);
                    user.AvatarImageUrl = newFilenameUrl;
                }

                #endregion
                CodeGenerator codeGenerator = new CodeGenerator();

                user.Code = codeGenerator.ReturnUserCode();
                user.IsDeleted = false;
                user.CreationDate = DateTime.Now;
                user.Id = Guid.NewGuid();
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityId = new SelectList(db.Cities, "Id", "Title", user.CityId);
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Title", user.RoleId);
            return View(user);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Title", user.CityId);
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Title", user.RoleId);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user, HttpPostedFileBase fileupload, HttpPostedFileBase fileUploadAvatar)
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

                    newFilenameUrl = "/Uploads/User/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    fileupload.SaveAs(physicalFilename);
                    user.NationalCardFileUrl = newFilenameUrl;
                }

                if (fileUploadAvatar != null)
                {
                    string filename = Path.GetFileName(fileUploadAvatar.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    newFilenameUrl = "/Uploads/User/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    fileUploadAvatar.SaveAs(physicalFilename);
                    user.AvatarImageUrl = newFilenameUrl;
                }


                #endregion
                user.IsDeleted = false;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Title", user.CityId);
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Title", user.RoleId);
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            User user = db.Users.Find(id);
            user.IsDeleted = true;
            user.DeletionDate = DateTime.Now;

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


        public ActionResult EditByUser()
        {
            User user = GetUserInfo.GetUser();

            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Title", user.CityId);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditByUser(User user, HttpPostedFileBase fileupload, HttpPostedFileBase fileUploadAvatar)
        {
            try
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

                        newFilenameUrl = "/Uploads/User/" + newFilename;
                        string physicalFilename = Server.MapPath(newFilenameUrl);
                        fileupload.SaveAs(physicalFilename);
                        user.NationalCardFileUrl = newFilenameUrl;
                    }

                    if (fileUploadAvatar != null)
                    {
                        string filename = Path.GetFileName(fileUploadAvatar.FileName);
                        string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                             + Path.GetExtension(filename);

                        newFilenameUrl = "/Uploads/User/" + newFilename;
                        string physicalFilename = Server.MapPath(newFilenameUrl);
                        fileUploadAvatar.SaveAs(physicalFilename);
                        user.AvatarImageUrl = newFilenameUrl;
                    }


                    #endregion

                    user.IsDeleted = false;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["suucess"] = "تغییرات با موفقیت انجام شد";
                    ViewBag.CityId = new SelectList(db.Cities, "Id", "Title", user.CityId);
                    return View(user);
                }
                ViewBag.CityId = new SelectList(db.Cities, "Id", "Title", user.CityId);
                TempData["error"] = "خطایی رخ داده است";

                return View(user);

            }
            catch (Exception e)
            {
                TempData["error"] = "خطایی رخ داده است";

                return View(user);
            }
        }

        public ActionResult ChangePassword()
        {
            User user = GetUserInfo.GetUser();

            if (user == null)
            {
                return HttpNotFound();
            }
            ChangePasswordViewModel result = new ChangePasswordViewModel()
            {
                Id = user.Id
            };
            return View(result);
        }

        [HttpPost, ActionName("ChangePassword")]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePasswordConfirmed(ChangePasswordViewModel input)
        {
            if (ModelState.IsValid)
            {
                if (input.NewPassword != input.RepeatPassword)
                {
                    TempData["error"] = "تکرار کلمه عبور صحیح نمی باشد.";
                    return View(input);
                }

                User user = GetUserInfo.GetUser();
                if (user.Password != input.OldPassword)
                {
                    TempData["error"] = "کلمه عبور پیشین صحیح نمی باشد.";
                    return View(input);
                }

                user.Password = input.NewPassword;
                user.LastModifiedDate = DateTime.Now;
                TempData["suucess"] = "تغییرات با موفقیت انجام شد";

                db.SaveChanges();

            }
            return View(input);

        }

    }
}
