using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using WebsiteDental.Models; // Import model

namespace WebsiteDental.ViewComponents
{
    [ViewComponent]
    public class Services : ViewComponent

    {
        private readonly WebsiteDentalContext _context;

        public Services(WebsiteDentalContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var services = await _context.Services.ToListAsync(); // Lấy dữ liệu từ SQL

            return View(services);// Trả về View
        }
    }
}
