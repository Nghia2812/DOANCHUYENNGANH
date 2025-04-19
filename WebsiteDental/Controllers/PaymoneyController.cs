using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
               // TotalPrice = (c.Product.Price ?? 0) * (c.Quantity ?? 0)  // Tính tổng giá cho từng sản phẩm
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

    private int GetCurrentUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.Session.GetInt32("UserId");
        return userId ?? 0;
    }
    [HttpPost]
    public IActionResult Confirm(PaymoneyModelView model)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", model); // Trả về trang thanh toán nếu dữ liệu không hợp lệ
        }

        // Chuyển sang view xác nhận đơn hàng
        return View("Confirm", model);
    }
    [HttpPost]
    public IActionResult PlaceOrder(PaymoneyModelView model)
    {
        // Giả sử PatientId lấy từ thông tin người dùng đã đăng nhập hoặc thông qua Session
        int? patientId = 123; // Lấy PatientId từ thông tin đăng nhập hoặc session, ví dụ.

        // Tạo hóa đơn mới
        var invoice = new Invoice
        {
            PatientId = patientId,  // Liên kết với bệnh nhân (khách hàng)
            TotalAmount = model.TotalAmount,
            IssueDate = DateOnly.FromDateTime(DateTime.Now),  // Ngày xuất hóa đơn
            IsPaid = false,  // Chưa thanh toán
            IsActive = true,
            UserId = 1,  // Liên kết với User đang đăng nhập (có thể lấy từ session hoặc hệ thống quản lý người dùng)
        };

        // Lưu hóa đơn vào bảng Invoice
        _context.Invoices.Add(invoice);
        _context.SaveChanges();  // Lưu vào database, lấy Id của hóa đơn vừa tạo

        // Lưu chi tiết đơn hàng vào bảng InvoiceDetail
        foreach (var item in model.CartItems)
        {
            var invoiceDetail = new InvoiceDetail
            {
                InvoiceId = invoice.Id,  // Liên kết chi tiết với hóa đơn vừa tạo
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Subtotal = item.Quantity * item.Price,  // Tính tổng tiền cho sản phẩm
                DiscountAmount = 0,  // Nếu có giảm giá thì tính ở đây
                FinalAmount = item.Quantity * item.Price,  // Tổng tiền sau khi áp dụng giảm giá (nếu có)
                CreatedAt = DateTime.Now,
                IsActive = true
            };

            _context.InvoiceDetails.Add(invoiceDetail);
        }
        _context.SaveChanges();  // Lưu chi tiết vào database

        // Chuyển hướng đến trang thông báo đặt hàng thành công
        return RedirectToAction("OrderSuccess");
    }
    public IActionResult OrderSuccess()
    {
        return View();
    }


}
