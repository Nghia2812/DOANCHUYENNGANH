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

        public async Task<IActionResult> Index(int? categoryID)
        {
            var categories = _context.DiscountCategories?.ToList() ?? new List<DiscountCategory>();

            var Discounts = categoryID.HasValue
                ? _context.Discounts.Where(s => s.CategoryId == categoryID.Value).ToList()
                : _context.Discounts.ToList();

            ViewBag.DiscountsCategories = categories;
            ViewBag.Discounts = Discounts; // sửa lại viết hoa P

            return View();
        }
    }
}
