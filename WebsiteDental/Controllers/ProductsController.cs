using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebsiteDental.Models; // Import namespace chứa DbContext

namespace WebsiteDental.Controllers
{
    public class ProductsController : Controller
    {
        private readonly WebsiteDentalContext _context; // DbContext

        public ProductsController(WebsiteDentalContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.ToList(); // Lấy danh sách sản phẩm
            return View(); // Truyền danh sách sản phẩm sang View
        }
    }
}
