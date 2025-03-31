using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using WebsiteDental.Models; // Import model

namespace WebsiteDental.ViewComponents
{
    [ViewComponent]
    public class BlogPosts : ViewComponent

    {
        private readonly WebsiteDentalContext _context;

        public BlogPosts(WebsiteDentalContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var BlogPosts = await _context.BlogPosts 
            .OrderByDescending(b => b.CreatedAt) // Sắp xếp theo ngày mới nhất
            .Take(4) // Lấy 3 bài viết đầu tiên
            // Chuyển thành danh sách
            .ToListAsync();

            return View(BlogPosts);// Trả về View
        }
    }
}