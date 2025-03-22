using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using WebsiteDental.Models; // Import model

namespace WebsiteDental.ViewComponents
{
    [ViewComponent]
    public class FQAs : ViewComponent

    {
        private readonly WebsiteDentalContext _context;

        public FQAs(WebsiteDentalContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var FQAs = await _context.Faqs.ToListAsync(); // Lấy dữ liệu từ SQL

            return View(FQAs);// Trả về View
        }
    }
}
