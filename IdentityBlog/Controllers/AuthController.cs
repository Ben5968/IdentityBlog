using Blog.Data;
using Blog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Blog.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        BlogDataContext _context; 

        // GET: Auth
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserViewModel userModel)
        {
            _context = new BlogDataContext();
            var users = _context.Users.Where(u => u.Email == userModel.Email && u.Password == userModel.Password).ToList();

            if (users.Count > 0)
            {
                FormsAuthentication.SetAuthCookie(userModel.Email, false);
                return RedirectToAction("Index", "Admin");
            }
            
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}