using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SCUTClubManager.Models;
using SCUTClubManager.DAL;

namespace SCUTClubManager.Controllers
{
    public class HomeController : Controller
    {
        private UnitOfWork db = new UnitOfWork();

        //
        // GET: /Home/
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LogInModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);

                    HttpCookie cookie = new HttpCookie("LoginUserFullName");
                    cookie.Expires = DateTime.Now.AddHours(24);
                    cookie.Values.Add("Name", Server.UrlEncode(db.Users.Find(model.UserName).Name));
                    Response.Cookies.Add(cookie);

                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "用户名或者密码错误。");
                }
            }

            return View(model);
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Login");
        }

        [Authorize]
        public ActionResult ChangePassword(string return_url = "")
        {
            ViewBag.ReturnUrl = return_url;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model, string return_url)
        {
            if (ModelState.IsValid)
            {
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return Json(new { success = true, msg = "修改密码成功", url = return_url });
                }
            }

            return Json(new { success = true, msg = "修改密码成功, 新密码不符合要求或者原密码输入错误" });
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
