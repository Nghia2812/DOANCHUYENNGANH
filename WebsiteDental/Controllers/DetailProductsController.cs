using Microsoft.AspNetCore.Mvc;
using WebsiteDental.Models;
using System.Linq;

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
            // Lấy thông tin sản phẩm theo id
            var product = _context.Products
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            // Lấy danh mục sản phẩm nếu cần (nếu có quan hệ khóa ngoại)
            var categoryList = _context.ProductCategories.ToList();

            // Lấy chi tiết sản phẩm (nếu có bảng riêng)
            var productDetail = _context.ProductDetails
                .FirstOrDefault(d => d.ProductId == id);

            // Tạo ViewModel để truyền ra View
            var viewModel = new ProductDetailModelView
            {
                Product = product,
                CategoryList = categoryList, // Truyền danh sách danh mục
                Detail = productDetail
            };
            return View(viewModel);
        }
    }
}
