using Blog.Core;
using Blog.Data;
using Blog.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class AdminController : Controller
    {
        // Introducing field
        BlogDataContext _context;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public AdminController()
        {

        }

        public AdminController(ApplicationUserManager userManager, ApplicationSignInManager signInManager,
            ApplicationRoleManager roleManager )
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }




        // GET: Admin
        public ActionResult Index()
        {
            // creating or initializing object instance
            _context = new BlogDataContext();

            List<Blogs> getBlogs = _context.Blogs.ToList();

            List<BlogViewModel> model = new List<BlogViewModel>();

            foreach (var blog in getBlogs)
            {
                model.Add(new BlogViewModel
                {
                    Id = blog.Id,
                    Content = blog.Content,
                    Photo = blog.Photo,
                    PostDate = blog.PostDate,
                    Title = blog.Title,
                    UserId = blog.UserId
                });
            }

            return View(model);
        }

        public ActionResult EditBlog(int id)
        {
            _context = new BlogDataContext();

            var getBlog = _context.Blogs.SingleOrDefault(b => b.Id == id);

            BlogViewModel blogModel = new BlogViewModel
            {
                Id = getBlog.Id,
                Photo = getBlog.Photo,
                PostDate = getBlog.PostDate,
                Title = getBlog.Title,
                UserId = getBlog.UserId,
                Content = getBlog.Content
            };

            if (getBlog != null)
            {

                return View(blogModel);
            }
            return View();
        }

        [HttpPost]
        public ActionResult EditBlog(BlogViewModel blogView, HttpPostedFileBase photoFile)
        {
            _context = new BlogDataContext();
            var blog = _context.Blogs.SingleOrDefault(b => b.Id == blogView.Id);

            if (blog != null)
            {
                blog.Content = blogView.Content;
                blog.Title = blogView.Title;
                if (photoFile != null)
                {
                    blog.Photo = $@"{DateTime.UtcNow.Ticks}_{photoFile.FileName}";
                    //_blog.Photo = $@"{Guid.NewGuid()}{photoFile.FileName}";

                    var path = Path.Combine(Server.MapPath("~/Content/Upload/Photos"), blog.Photo);
                    photoFile.SaveAs(path);
                }

                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("EditBlog", new { id = blogView.Id });

        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(BlogViewModel model, HttpPostedFileBase photoFile)
        {

            var _blog = new Blogs();
            _blog.Title = model.Title;
            _blog.Content = model.Content;
            _blog.PostDate = DateTime.Now;

            if (photoFile != null)
            {
                _blog.Photo = $@"{DateTime.UtcNow.Ticks}_{photoFile.FileName}";
                //_blog.Photo = $@"{Guid.NewGuid()}{photoFile.FileName}";

                var path = Path.Combine(Server.MapPath("~/Content/Upload/Photos"), _blog.Photo);
                photoFile.SaveAs(path);
            }

            BlogDataContext _context = new BlogDataContext();
            _context.Blogs.Add(_blog);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteBlog (int id)
        {
            _context = new BlogDataContext();
            var blog = _context.Blogs.SingleOrDefault(b => b.Id == id);
                
                if (blog != null)
                {
                _context.Blogs.Remove(blog);
                _context.SaveChanges();
                }
            
            return RedirectToAction("Index");
        }
        

        public ActionResult ListUsers()
        {           
            var Users = UserManager.Users.Include(u => u.Roles).ToList();
            List<Users_in_Role_ViewModel> UserWithRoles = new List<Users_in_Role_ViewModel>();

            var _userRoles = GetRoles();

            //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());
            //var roles = roleManager.Roles.ToList();

            var rolesInRolemanager = RoleManager.Roles.ToList();

            foreach (var user in Users)
            {
                //var rolelist = string.Join(",", user.Roles);
                UserWithRoles.Add(new Users_in_Role_ViewModel
                {
                UserId = user.Id,
                Email = user.Email,
                UserRoles = _userRoles
                    //UserRoles = new SelectList(RoleManager.Roles, "Value", "Text")
                });
        }

            //var Users = UserManager.Users.ToList();
            //ViewBag.Name = new SelectList(RoleManager.Roles.Where(u => !u.Name.Contains("Admin"))
            //                                .ToList(), "Name", "Name");
            
            //working viewbag
            //ViewBag.Name = new SelectList(RoleManager.Roles.ToList(), "Name", "Name");

            return View(UserWithRoles);
        }
        //private IEnumerable<SelectListItem> GetRoles()
        private SelectList GetRoles()
        {
            var roles = RoleManager.Roles
                        .Select(x =>
                                new SelectListItem
                                {
                                    Value = x.Id.ToString(),
                                    Text = x.Name
                                });
            return new SelectList(roles, "Value", "Text");
        }

        public JsonResult AddUserToRole(string id)
        {
            return Json(id, JsonRequestBehavior.AllowGet );
        }
    }
}