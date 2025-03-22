using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using WebsiteDental.Models; // Import model

namespace WebsiteDental.ViewComponents
{
    [ViewComponent]
    public class ServiceFeatures : ViewComponent

    {
        private readonly WebsiteDentalContext _context;

        public ServiceFeatures(WebsiteDentalContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var services = await _context.ServiceFeatures.ToListAsync(); // Lấy dữ liệu từ SQL


            return View(services);// Trả về View
        }
    }
}
