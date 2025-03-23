using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using WebsiteDental.Models; // Import model

namespace WebsiteDental.ViewComponents
{
    [ViewComponent]
    public class Discounts : ViewComponent

    {
        private readonly WebsiteDentalContext _context;

        public Discounts(WebsiteDentalContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var Discounts = await _context.Discounts.ToListAsync(); // Lấy dữ liệu từ SQL

            return View(Discounts);// Trả về View
        }
    }
}