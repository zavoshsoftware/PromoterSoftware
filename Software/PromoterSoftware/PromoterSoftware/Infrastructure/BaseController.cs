using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Helpers;
using Models;

namespace PromoterSoftware.Infrastructure
{
    public class BaseController : Controller
    {
        public BaseController()
        {
        }

        protected override System.IAsyncResult BeginExecuteCore(System.AsyncCallback callback, object state)
        {
            System.Globalization.CultureInfo oCultureInfo =
                new System.Globalization.CultureInfo("fa-IR");

            System.Threading.Thread.CurrentThread.CurrentCulture = oCultureInfo;
            System.Threading.Thread.CurrentThread.CurrentUICulture = oCultureInfo;

            User user = GetUserInfo.GetUserFullName();
            if (user != null)
            {
                ViewBag.Name = user.FullName;
                ViewBag.avatar = user.AvatarImageUrl;
                ViewBag.Role = user.Role.Title;

                if (string.IsNullOrEmpty(user.AvatarImageUrl))
                    ViewBag.avatar = "/assets/images/avatars/avatar_default.jpg";
            }
          

            return base.BeginExecuteCore(callback, state);
        }
    }
}