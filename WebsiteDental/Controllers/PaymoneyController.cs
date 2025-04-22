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
            ViewBag.Address = user.Address;
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

    [HttpPost]
    public IActionResult PlaceOrder()
    {
        // Lấy giỏ hàng từ session
        var cartJson = HttpContext.Session.GetString("CartItems");
        var cartItems = string.IsNullOrEmpty(cartJson)
            ? new List<CartItemModelView>()
            : JsonConvert.DeserializeObject<List<CartItemModelView>>(cartJson);

        if (cartItems == null || !cartItems.Any())
        {
            TempData["ErrorMessage"] = "Giỏ hàng của bạn trống!";
            return RedirectToAction("Index", "ShoppingCart");
        }

        // Lấy ID người dùng
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            TempData["ErrorMessage"] = "Bạn cần phải đăng nhập trước khi đặt hàng!";
            return RedirectToAction("Login", "Account");
        }

        // Tính tổng tiền
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
        _context.SaveChanges(); // Lưu để lấy Invoice.Id

        // Danh sách chi tiết hóa đơn
        List<InvoiceDetail> invoiceDetails = new List<InvoiceDetail>();

        foreach (var item in cartItems)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);
            if (product == null || product.Quantity < item.Quantity)
            {
                TempData["ErrorMessage"] = $"Sản phẩm '{item.ProductName}' không đủ số lượng trong kho.";
                return RedirectToAction("Index", "ShoppingCart");
            }

            // Trừ số lượng trong kho
            product.Quantity -= item.Quantity;
            _context.Products.Update(product);

            // Thêm chi tiết hóa đơn
            invoiceDetails.Add(new InvoiceDetail
            {
                InvoiceId = invoice.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Subtotal = item.TotalPrice,
                DiscountAmount = 0,
                FinalAmount = item.TotalPrice,
                CreatedAt = DateTime.Now,
                IsActive = true
            });
        }

        // Lưu chi tiết hóa đơn
        _context.InvoiceDetails.AddRange(invoiceDetails);
        _context.SaveChanges();

        // Xoá giỏ hàng sau khi đặt
        HttpContext.Session.Remove("CartItems");

        // Truyền dữ liệu sang trang xác nhận
        ViewBag.Invoice = invoice;
        ViewBag.InvoiceDetails = invoiceDetails;
        ViewBag.CartItems = cartItems;

        return View("~/Views/Paymoney/Confirm.cshtml");
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
        return View("OrderSuccess", model); // Confirm.cshtml
    }



    // Trang thành công khi đặt hàng
    public IActionResult OrderSuccess()
    {
        return View("OrderSuccess"); // Trả về view OrderSuccess.cshtml
    }
}
