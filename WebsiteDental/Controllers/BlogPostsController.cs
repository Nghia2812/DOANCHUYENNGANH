using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteDental.Models;
using WebsiteDental.ViewComponents;

namespace WebsiteDental.Controllers
{
    public class BlogPostsController : Controller
    {
        private readonly WebsiteDentalContext _context;

        public BlogPostsController(WebsiteDentalContext context)
        {
            _context = context;
        }


        public IActionResult Index(int? categoryId)
        {
            var categories = _context.BlogCategories?.ToList() ?? new List<BlogCategory>();

            var blogPosts = categoryId.HasValue
                ? _context.BlogPosts.Where(p => p.CategoryId == categoryId.Value).ToList()
                : _context.BlogPosts.ToList();

            ViewBag.BlogCategories = categories;
            ViewBag.BlogPosts = blogPosts;

            return View();
        }

    }
}