using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteDental.Models;

namespace WebsiteDental.Controllers
{
    public class DetailBlogPostsController : Controller
    {
        private readonly WebsiteDentalContext _context;

        public DetailBlogPostsController(WebsiteDentalContext context)
        {
            _context = context;
        }

        public IActionResult DetailBlog(int id)
        {
            var blogPost = _context.BlogPosts.FirstOrDefault(b => b.Id == id);
            if (blogPost == null)
            {
                return NotFound();
            }

            // Lấy 5 bài viết liên quan, ngoại trừ bài hiện tại
            var relatedPosts = _context.BlogPosts
                .Where(b => b.Id != id)
                .OrderByDescending(b => b.CreatedAt)
                .Take(3)
                .ToList();

            var viewModel = new BlogDetailViewModel
            {
                BlogPost = blogPost,
                RelatedPosts = relatedPosts
            };

            return View(viewModel);
        }
    }
}
