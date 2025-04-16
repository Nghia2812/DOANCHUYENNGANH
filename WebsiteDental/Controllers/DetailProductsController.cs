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


            // Lấy danh mục sản phẩm
            var categoryList = _context.ProductCategories.ToList();

            // Lấy chi tiết sản phẩm (nếu có bảng riêng)
            var productDetail = _context.ProductDetails
                .FirstOrDefault(d => d.ProductId == id);

            // Tạo ViewModel để truyền ra View
            var viewModel = new ProductDetailModelView
            {
                Product = product,
                CategoryList = categoryList, // Truyền danh sách danh mục
                Detail = productDetail,
               
            };
            return View(viewModel);
        }
        //Bình luận trong trang
        [HttpPost]
        public IActionResult AddComment(int ProductId, string CommentText, string CreatedBy, string Gmail)
        {
            if (string.IsNullOrWhiteSpace(CreatedBy) || string.IsNullOrWhiteSpace(CommentText))
            {
                return RedirectToAction("Index", new { id = ProductId });
            }

            var comment = new ProductComment
            {
                ProductId = ProductId,
                CommentText = CommentText,
                CreatedBy = CreatedBy,
                Gmail = Gmail,  // Lưu Gmail từ form
                CreatedAt = DateTime.Now,
                IsActive = true
            };

            _context.ProductComments.Add(comment);  // Thêm bình luận vào bảng
            _context.SaveChanges();  // Lưu thay đổi vào cơ sở dữ liệu

            // Lấy lại danh sách bình luận của sản phẩm này để hiển thị
            var comments = _context.ProductComments
                                    .Where(c => c.ProductId == ProductId && c.IsActive)
                                    .OrderByDescending(c => c.CreatedAt)
                                    .ToList();

            // Lấy thông tin sản phẩm
            var product = _context.Products.FirstOrDefault(p => p.Id == ProductId);
            if (product == null)
            {
                return NotFound();
            }

            // Lấy danh mục sản phẩm và chi tiết sản phẩm
            var categoryList = _context.ProductCategories.ToList();
            var productDetail = _context.ProductDetails
                                        .FirstOrDefault(d => d.ProductId == ProductId);

            // Trả lại view với dữ liệu mới
            var viewModel = new ProductDetailModelView
            {
                Product = product,
                CategoryList = categoryList,  // Truyền danh sách danh mục
                Detail = productDetail,       // Truyền chi tiết sản phẩm
                Comments = comments           // Truyền danh sách bình luận
            };

            return View("Index", viewModel);  // Trả về lại view Index với danh sách bình luận mới
        }




    }
}
