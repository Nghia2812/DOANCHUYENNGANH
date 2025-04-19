using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteDental.Models;
using WebsiteDental.ViewModels;
using Microsoft.AspNetCore.Http; // Cần thêm namespace này



public class ShoppingcartController : Controller
{
    private readonly WebsiteDentalContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ShoppingcartController(WebsiteDentalContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public IActionResult Index()
    {
        var userId = GetCurrentUserId();

        if (userId == 0)
        {
            return RedirectToAction("Register", "Account");
        }
     
        var cartItems = _context.Carts
            .Where(c => c.UserId == userId)
            .Include(c => c.Product)
            .Select(c => new CartItemModelView
            {
                ProductId = c.ProductId ?? 0,
                ProductName = c.Product.ProductName,
                Image = c.Product.Image,
                Rating = c.Product.Rating,
                Price = c.Product.Price,
                Quantity = c.Quantity ?? 0
                
            })
            .ToList();

        // Tính tổng giá trị giỏ hàng
        decimal totalAmount = cartItems.Sum(item => item.TotalPrice);  // Sử dụng TotalPrice trong CartItemModelView

        // Tính phí vận chuyển
        decimal shippingFee = totalAmount < 500000 ? 40000 : 0;

        // Tính tổng thanh toán (giỏ hàng + phí vận chuyển)
        decimal totalWithShipping = totalAmount + shippingFee;

        // Lấy 8 sản phẩm gợi ý
        var recommendedProducts = _context.Products
            .OrderByDescending(p => p.Id)
            .Take(8)
            .ToList();
        // Gửi dữ liệu ra View
        ViewData["TotalAmount"] = totalAmount;  
        ViewData["ShippingFee"] = shippingFee; 
        ViewData["TotalWithShipping"] = totalWithShipping;  
        ViewBag.RecommendedProducts = recommendedProducts; // Truyền sang View
        return View(cartItems);
    }
    [HttpPost]
    public IActionResult AddToCart(int productId, int quantity)
    {
        if (quantity <= 0)
        {
            // Kiểm tra số lượng hợp lệ
            ViewData["Error"] = "Số lượng không hợp lệ!";
            return RedirectToAction("Detail", new { id = productId }); // Quay lại trang chi tiết sản phẩm
        }

        var userId = GetCurrentUserId();  // Lấy ID người dùng từ session

        if (userId == 0)
        {
            // Nếu người dùng chưa đăng nhập, yêu cầu đăng nhập
            return RedirectToAction("Register", "Account");
        }

        // Kiểm tra xem sản phẩm đã có trong giỏ của người dùng chưa
        var cartItem = _context.Carts.FirstOrDefault(c => c.ProductId == productId && c.UserId == userId);

        if (cartItem != null)
        {
            // Nếu sản phẩm đã có trong giỏ hàng, cập nhật số lượng
            cartItem.Quantity += quantity;
        }
        else
        {
            // Nếu sản phẩm chưa có, thêm sản phẩm mới vào giỏ
            cartItem = new Cart
            {
                ProductId = productId,
                UserId = userId,
                Quantity = quantity
            };
            _context.Carts.Add(cartItem);
        }

        // Lưu thay đổi vào cơ sở dữ liệu
        _context.SaveChanges();

        // Quay về trang giỏ hàng
        return RedirectToAction("Index", "ShoppingCart");
    }


    private int GetCurrentUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.Session.GetInt32("UserId");

        if (userId.HasValue)
        {
            return userId.Value;
        }

        return 0;
    }
    //Xoá sản phẩm trong giỏ hàng ạ
    [HttpPost]
    public IActionResult RemoveFromCart(int productId)
    {
        var userId = GetCurrentUserId(); // Giả sử bạn có phương thức lấy ID người dùng

        if (userId == 0)
        {
            return RedirectToAction("Register", "Account");
        }

        // Tìm sản phẩm trong giỏ hàng của người dùng
        var cartItem = _context.Carts.FirstOrDefault(c => c.ProductId == productId && c.UserId == userId);

        if (cartItem != null)
        {
            // Xóa sản phẩm khỏi giỏ hàng
            _context.Carts.Remove(cartItem);
            _context.SaveChanges();
        }

        // Quay lại giỏ hàng
        return RedirectToAction("Index", "ShoppingCart");
    }
    //xoá tất cả
    [HttpPost]
    public IActionResult ClearCart()
    {
        var userId = GetCurrentUserId(); // Lấy ID người dùng hiện tại

        if (userId == 0)
        {
            return RedirectToAction("Register", "Account");
        }

        // Tìm tất cả các sản phẩm trong giỏ hàng của người dùng
        var cartItems = _context.Carts.Where(c => c.UserId == userId).ToList();

        // Xóa tất cả sản phẩm trong giỏ hàng
        _context.Carts.RemoveRange(cartItems);
        _context.SaveChanges();

        // Quay lại giỏ hàng
        return RedirectToAction("Index", "ShoppingCart");
    }
    //cập nhật số lượng thêm trong giỏ hàng
    [HttpPost]
    public IActionResult UpdateQuantity(int productId, int quantity, string action)
    {
        // Lấy thông tin giỏ hàng của người dùng
        var userId = GetCurrentUserId(); // Thay đổi hàm này tùy thuộc vào cách bạn lấy userId hiện tại
        if (userId == 0)
        {
            return RedirectToAction("Register", "Account"); // Nếu người dùng chưa đăng nhập
        }

        // Tìm sản phẩm trong giỏ hàng
        var cartItem = _context.Carts.FirstOrDefault(c => c.ProductId == productId && c.UserId == userId);
        if (cartItem != null)
        {
            // Thay đổi số lượng tùy thuộc vào action (decrease/increase)
            if (action == "decrease" && cartItem.Quantity > 1)
            {
                cartItem.Quantity--; // Giảm số lượng nếu > 1
            }
            else if (action == "increase")
            {
                cartItem.Quantity++; // Tăng số lượng
            }

            // Lưu thay đổi
            _context.SaveChanges();
        }

        // Quay lại trang giỏ hàng sau khi cập nhật
        return RedirectToAction("Index", "Shoppingcart");
    }

    [HttpPost]
    public IActionResult RemoveCart(int productId)
    {
        var userId = GetCurrentUserId();

        if (userId == 0)
        {
            return RedirectToAction("Register", "Account");
        }

        var cartItem = _context.Carts.FirstOrDefault(c => c.ProductId == productId && c.UserId == userId);

        if (cartItem != null)
        {
            _context.Carts.Remove(cartItem);  // Xóa sản phẩm khỏi giỏ
            _context.SaveChanges();
        }

        // Gửi thông báo thành công qua TempData
        TempData["SuccessMessage"] = "Giỏ hàng đã được cập nhật thành công!";

        // Quay lại trang giỏ hàng
        return RedirectToAction("Index", "Shoppingcart");
    }


}