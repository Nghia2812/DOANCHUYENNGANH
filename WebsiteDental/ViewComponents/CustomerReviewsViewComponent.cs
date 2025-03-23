using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using WebsiteDental.Models; // Import model

namespace WebsiteDental.ViewComponents
{
    [ViewComponent]
    public class CustomerReviews : ViewComponent

    {
        private readonly WebsiteDentalContext _context;

        public CustomerReviews(WebsiteDentalContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var CustomerReviews = await _context.CustomerReviews.ToListAsync(); // Lấy dữ liệu từ SQL

            return View(CustomerReviews);// Trả về View
        }
    }
}