using Microsoft.AspNetCore.Mvc;
using WebsiteDental.Models;

namespace WebsiteDental.Controllers
{
    public class DetailProductsController : Controller
    {
        private readonly WebsiteDentalContext _context;

        public DetailProductsController(WebsiteDentalContext context)
        {
            _context = context;
        }
        public IActionResult Index(int id)
        {
            // Lấy chi tiết sản phẩm theo id và truyền ra View
            return View();
        }
    }
}
