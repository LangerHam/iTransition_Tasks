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

                if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
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

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                    RegistrationTime = DateTime.Now,
                    Status = "Active" 
                };

                try
                {
                    db.Users.Add(user);
                    db.SaveChanges(); 

                    FormsAuthentication.SetAuthCookie(user.Email, false);

                    return RedirectToAction("Index", "UserManagement"); 
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                {
                    var sqlException = ex.InnerException?.InnerException as System.Data.SqlClient.SqlException;
                    if (sqlException != null && (sqlException.Number == 2627 || sqlException.Number == 2601))
                    {
                        ModelState.AddModelError("Email", "An account with this email already exists.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "An unexpected error occurred during registration. Please try again.");
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "An unexpected error occurred.");
                }
            }
            return View(model);
        }
    }
}