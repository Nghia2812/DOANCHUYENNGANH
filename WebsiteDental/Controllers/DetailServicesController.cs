using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebsiteDental.Models;



namespace WebsiteDental.Controllers
{
    public class DetailServicesController : Controller
    {
        private readonly WebsiteDentalContext _context;

        public DetailServicesController(WebsiteDentalContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> DetailServices(int id)
        {
            var service = await _context.Services.FindAsync(id); // Lấy dịch vụ theo ID
            var blogPosts = await _context.BlogPosts
                                          .OrderByDescending(b => b.CreatedAt)
                                          .Take(3)
                                          .ToListAsync(); // Lấy 3 bài viết mới nhất

            if (service == null)
            {
                return NotFound();
            }

            var viewModel = new DetailServicesModelView
            {
                Service = service,
                BlogPosts = blogPosts
            };

            return View(viewModel);
        }
    }
}
