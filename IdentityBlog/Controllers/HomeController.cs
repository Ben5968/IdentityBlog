using Blog.Data;
using Blog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        BlogDataContext _context;

        public ActionResult Index()
        {
            _context = new BlogDataContext();

            var blogContents = _context.Blogs.ToList();

            //List<BlogViewModel> blogViewModel = blogContents
            //    .Select(
            //        b => new BlogViewModel
            //        {
            //            Content = b.Content,
            //            Title = b.Title,
            //            Id = b.Id,
            //            Photo = b.Photo,
            //            PostDate = b.PostDate,
            //            UserId = b.UserId
            //        }
            //    ).ToList();

            List<BlogViewModel> blogViewModel = new List<BlogViewModel>();

            foreach (var item in blogContents)
            {
                blogViewModel.Add(
                    new BlogViewModel()
                    {
                        Content = item.Content,
                        Title = item.Title,
                        Id = item.Id,
                        Photo = item.Photo,
                        PostDate = item.PostDate,
                        UserId = item.UserId
                    }
                    );
            }

            return View(blogViewModel);

        }

        public ActionResult Blog(int id)
        {
            _context = new BlogDataContext();

            var blog = new BlogViewModel();
            var data = _context.Blogs.FirstOrDefault(i => i.Id == id);
            blog.Id = data.Id;
            blog.Photo = data.Photo;
            blog.PostDate = data.PostDate;
            blog.Title = data.Title;
            blog.UserId = data.UserId;
            blog.Content = data.Content;
            return View(blog);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}