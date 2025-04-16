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
                                          .Take(12)
                                          .ToListAsync();
            // ✅ Đổi tên biến này để tránh trùng với property trong ViewModel
            var serviceFeatures = await _context.ServiceFeatures
                .Where(s => s.Id != id && s.IsActive == true)
                .OrderBy(s => s.Order)
                .Take(4)
                .ToListAsync();

            if (service == null)
            {
                return NotFound();
            }

            var viewModel = new DetailServicesModelView
            {
                ServiceFeatures = serviceFeatures, // Gán đúng biến tên khác
                Service = service,
                BlogPosts = blogPosts
            };

            return View(viewModel);
        }
    }
}
