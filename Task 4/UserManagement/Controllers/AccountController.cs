using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UserManagement.Context;
using UserManagement.ViewModel;

namespace UserManagement.Controllers
{
    public class AccountController : Controller
    {
        private UserManagementDBEntities db;

        public AccountController()
        {
            db = new UserManagementDBEntities();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string rurl)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var user = db.Users.FirstOrDefault(u => u.Email == model.Email);

                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid email or password.");
                    return View(model);
                }

                if(user.Status == "Blocked")
                {
                    ModelState.AddModelError("", "Your account is blocked. Please contact support.");
                    return View(model);
                }

                user.LastLoginTime = DateTime.Now;
                db.SaveChanges();

                FormsAuthentication.SetAuthCookie(model.Email, model.RememberMe);

                if(Url.IsLocalUrl(rurl))
                {
                    return Redirect(rurl);
                }
                else
                {
                    return RedirectToAction("Index", "UserManagement");
                }
            }
            catch(Exception e)
            {
                ModelState.AddModelError("", "An error occurred while processing your request. Please try again later.");
                return View(model);
            }
        }
    }
}