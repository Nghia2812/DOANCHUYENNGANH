using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using WebsiteDental.Models;
using WebsiteDental.ViewModels;

public class PaymoneyController : Controller
{
    private readonly WebsiteDentalContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PaymoneyController(WebsiteDentalContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    // GET: Trang thanh toán (index)
    public IActionResult Index()
    {
        var userId = GetCurrentUserId();

        if (userId == 0)
        {
            return RedirectToAction("Register", "Account");
        }

        // Lấy thông tin người dùng
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);
        if (user != null)
        {
            ViewBag.FullName = user.Username;
            ViewBag.Email = user.Email;
            ViewBag.Phone = user.Phone;
        }

        // Lấy giỏ hàng của người dùng
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
                Quantity = c.Quantity ?? 0,
               // TotalPrice = (c.Product.Price ?? 0) * (c.Quantity ?? 0) // Tính tổng giá cho từng sản phẩm
            })
            .ToList();

        // Kiểm tra nếu không có sản phẩm trong giỏ hàng
        if (cartItems == null || !cartItems.Any())
        {
            return RedirectToAction("Index", "ShoppingCart"); // Nếu không có sản phẩm trong giỏ hàng
        }

        // Tính tổng tiền và phí vận chuyển
        decimal totalAmount = cartItems.Sum(item => item.TotalPrice);
        decimal shippingFee = totalAmount < 500000 ? 40000 : 0; // Tính phí vận chuyển
        decimal totalWithShipping = totalAmount + shippingFee;

        // Truyền thông tin giỏ hàng và thanh toán sang ViewModel
        var paymentModelView = new PaymoneyModelView
        {
            CartItems = cartItems,
            TotalAmount = totalAmount,
            ShippingFee = shippingFee,
            TotalWithShipping = totalWithShipping
        };

        // Trả về view với model
        return View(paymentModelView);
    }

    // Lấy ID người dùng hiện tại từ session
    private int GetCurrentUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.Session.GetInt32("UserId");
        return userId ?? 0;
    }

    // POST: Xác nhận đơn hàng
    [HttpPost]
    public IActionResult Confirm(PaymoneyModelView model)
    {
        // Kiểm tra tính hợp lệ của model
        if (!ModelState.IsValid)
        {
            return View("Confirm", model); // Nếu không hợp lệ, quay lại trang xác nhận
        }

        // Lưu giỏ hàng vào Session
        var cartJson = JsonConvert.SerializeObject(model.CartItems);
        HttpContext.Session.SetString("CartItems", cartJson);

        // Chuyển đến trang xác nhận sau khi lưu giỏ hàng
        return View("Confirm", model); // Confirm.cshtml
    }

    // POST: Đặt hàng
    [HttpPost]
    public IActionResult PlaceOrder()
    {
        // Lấy giỏ hàng từ session
        var cartJson = HttpContext.Session.GetString("CartItems");
        var cartItems = string.IsNullOrEmpty(cartJson)
            ? new List<CartItemModelView>()
            : JsonConvert.DeserializeObject<List<CartItemModelView>>(cartJson);

        // Kiểm tra giỏ hàng
        if (cartItems == null || !cartItems.Any())
        {
            TempData["ErrorMessage"] = "Giỏ hàng của bạn trống!";
            return RedirectToAction("Index", "ShoppingCart");
        }

        // Lấy ID người dùng
        var userId = GetCurrentUserId();
        if (userId == 0)
        {
            TempData["ErrorMessage"] = "Bạn cần phải đăng nhập trước khi đặt hàng!";
            return RedirectToAction("Login", "Account");
        }

        // Tính tổng tiền của đơn hàng
        var totalAmount = cartItems.Sum(item => item.TotalPrice);

        // Tạo hóa đơn
        var invoice = new Invoice
        {
            UserId = userId,
            TotalAmount = totalAmount,
            IssueDate = DateOnly.FromDateTime(DateTime.Now),
            IsPaid = false,
            IsActive = true
        };

        _context.Invoices.Add(invoice);
        _context.SaveChanges();

        // Thêm chi tiết hóa đơn
        foreach (var item in cartItems)
        {
            var invoiceDetail = new InvoiceDetail
            {
                InvoiceId = invoice.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Subtotal = item.TotalPrice,
                DiscountAmount = 0,
                FinalAmount = item.TotalPrice,
                CreatedAt = DateTime.Now,
                IsActive = true
            };

            _context.InvoiceDetails.Add(invoiceDetail);
        }

        _context.SaveChanges();

        // Xóa giỏ hàng
        HttpContext.Session.Remove("CartItems");

        TempData["SuccessMessage"] = "Đặt hàng thành công!";
        return RedirectToAction("OrderSuccess"); // Chuyển hướng đến trang OrderSuccess
    }

    // Trang thành công khi đặt hàng
    public IActionResult OrderSuccess()
    {
        return View("OrderSuccess"); // Trả về view OrderSuccess.cshtml
    }
}
