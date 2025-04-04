using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebsiteDental.Models;
using WebsiteDental.ViewComponents;

namespace WebsiteDental.Controllers
{
    public class DiscountsController : Controller
    {
        private readonly WebsiteDentalContext _context;

        public DiscountsController(WebsiteDentalContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            var discounts = await _context.Discounts.ToListAsync();

            // Trả về View cùng với danh sách Discounts
            return View(discounts);
        }
    }
}
