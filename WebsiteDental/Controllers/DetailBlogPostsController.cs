using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Xml.Linq;
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

            // Lấy 3 bài viết liên quan, ngoại trừ bài hiện tại
            var relatedPosts = _context.BlogPosts
                .Where(b => b.Id != id)
                .OrderByDescending(b => b.CreatedAt)
                .Take(3)
                .ToList();
            // Lấy bình luận cho bài viết
            var comments = _context.BlogComments
             .Where(c => c.PostId == id && c.IsActive == true)
            .OrderByDescending(c => c.CreatedAt)
             .ToList();
            var viewModel = new BlogDetailViewModel
            {
                BlogPost = blogPost,
                RelatedPosts = relatedPosts,
                Comments = comments
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddComment(int BlogId, string CommentText, string UserName)
        {
            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(CommentText))
            {
                return RedirectToAction("DetailBlog", new { id = BlogId });
            }

            var comment = new BlogComment
            {
                PostId = BlogId,
                Name = UserName, // Lưu tên từ form
                Comment = CommentText,
                CreatedAt = DateTime.Now,
                IsActive = true // Hiển thị ngay
            };

            _context.BlogComments.Add(comment);
            _context.SaveChanges();

            return RedirectToAction("DetailBlog", new { id = BlogId });
        }

    }
}
