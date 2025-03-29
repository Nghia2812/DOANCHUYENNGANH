using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public IActionResult Index(int? categoryID) // sửa caterogyID thành categoryID
        {
            var categories = _context.ProductCategories?.ToList() ?? new List<ProductCategory>();

            var products = categoryID.HasValue
                ? _context.Products.Where(s => s.CategoryId == categoryID.Value).ToList()
                : _context.Products.ToList();

            ViewBag.ProductCategories = categories;
            ViewBag.Products = products; // sửa lại viết hoa P

            return View(); // Truyền danh sách sản phẩm sang View
        }
    }
}
