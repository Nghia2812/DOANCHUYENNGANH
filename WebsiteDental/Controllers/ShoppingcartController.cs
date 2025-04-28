using Microsoft.AspNetCore.Http; // Cần thêm namespace này
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebsiteDental.Models;
using WebsiteDental.ViewModels;




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
        decimal totalAmount = cartItems.Sum(item => item.TotalPrice);

        // Tính phí vận chuyển
        decimal shippingFee = totalAmount < 500000 ? 40000 : 0;

        // Tổng tạm tính trước khi giảm
        decimal totalWithShipping = totalAmount + shippingFee;

        // ==== MÃ GIẢM GIÁ ====
        string discountCode = HttpContext.Session.GetString("DiscountCode");
        decimal discountPercentage = 0;
        decimal discountAmount = 0;

        if (!string.IsNullOrEmpty(discountCode))
        {
            var discount = _context.Discounts
                .FirstOrDefault(d =>
                    (d.ProductCode == discountCode || d.ServiceCode == discountCode || d.ShippingCode == discountCode)
                    && d.IsActive == true
                    && d.StartDate <= DateOnly.FromDateTime(DateTime.Now)
                    && d.EndDate >= DateOnly.FromDateTime(DateTime.Now));

            if (discount != null)
            {
                discountPercentage = discount.DiscountPercentage ?? 0;
                discountAmount = totalAmount * (discountPercentage / 100);
            }
        }

        // Tổng thanh toán cuối cùng sau khi giảm giá
        decimal finalTotal = totalWithShipping - discountAmount;

        // ==== Truyền dữ liệu ra View ====
        ViewData["TotalAmount"] = totalAmount;
        ViewData["ShippingFee"] = shippingFee;
        ViewData["DiscountPercentage"] = discountPercentage;
        ViewData["DiscountAmount"] = discountAmount;
        ViewData["TotalWithShipping"] = finalTotal;

        ViewBag.RecommendedProducts = _context.Products
            .OrderByDescending(p => p.Id)
            .Take(8)
            .ToList();

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

    [HttpPost]
    public IActionResult ApplyDiscount(string discountCode)
    {
        // Kiểm tra mã giảm giá từ bảng Discounts
        var discount = _context.Discounts
            .FirstOrDefault(d => (d.ProductCode == discountCode || d.ServiceCode == discountCode || d.ShippingCode == discountCode)
                                 && d.IsActive == true
                                 && d.StartDate.HasValue
                                 && d.StartDate.Value <= DateOnly.FromDateTime(DateTime.Now)
                                 && d.EndDate.HasValue
                                 && d.EndDate.Value >= DateOnly.FromDateTime(DateTime.Now));

        // Nếu tìm thấy mã giảm giá hợp lệ
        if (discount != null)
        {
            // Kiểm tra xem FinalPrice có phải là null và chuyển sang decimal
            decimal finalPrice = 0;
            if (decimal.TryParse(discount.FinalPrice, out var parsedPrice))
            {
                finalPrice = parsedPrice;
            }

            // Lưu mã giảm giá và giá trị finalPrice vào session
            HttpContext.Session.SetString("DiscountCode", discountCode);
            HttpContext.Session.SetString("FinalPrice", finalPrice.ToString());

            // Cập nhật lại số lượng mã giảm giá trong cơ sở dữ liệu
            if (discount.Quantity > 0)
            {
                discount.Quantity -= 1; // Giảm số lượng
                _context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu

                // Sử dụng TempData để truyền mã giảm giá và thông báo thành công
                TempData["DiscountCode"] = discountCode;
                TempData["DiscountSuccess"] = "Áp dụng mã giảm giá thành công!";
            }
            else
            {
                TempData["DiscountError"] = "Mã giảm giá đã hết!";
                return RedirectToAction("Index", "Shoppingcart");
            }
        }
        else
        {
            TempData["DiscountError"] = "Mã giảm giá không hợp lệ hoặc đã hết hạn.";
        }

        // Quay lại trang giỏ hàng
        return RedirectToAction("Index", "Shoppingcart");
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



    //lưu giỏ hàng vào session 
    [HttpPost]
    public IActionResult ProceedToCheckout()
    {
        var userId = GetCurrentUserId();

        if (userId == 0)
        {
            return RedirectToAction("Register", "Account");
        }

        // Lấy giỏ hàng của người dùng từ database
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

        // Lưu giỏ hàng vào Session
        HttpContext.Session.SetString("CartItems", System.Text.Json.JsonSerializer.Serialize(cartItems));

        // Điều hướng sang trang thanh toán
        return RedirectToAction("Index", "Paymoney");
    }


}