using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Threading.Tasks;
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

        // Kiểm tra nếu người dùng chưa đăng nhập
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
            })
            .ToList();

        // Kiểm tra nếu không có sản phẩm trong giỏ hàng
        if (cartItems == null || !cartItems.Any())
        {
            return RedirectToAction("Index", "ShoppingCart"); // Nếu không có sản phẩm trong giỏ hàng
        }

        // Tính tổng tiền giỏ hàng
        decimal totalAmount = cartItems.Sum(item => item.TotalPrice);

        // Tính phí vận chuyển
        decimal shippingFee = totalAmount < 500000 ? 40000 : 0; // Nếu tổng dưới 500,000 VND thì tính phí vận chuyển

        // Tính tổng thanh toán với phí vận chuyển
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

        // ==== Truyền dữ liệu ra ViewModel ====
        var paymentModelView = new PaymoneyModelView
        {
            CartItems = cartItems,
            TotalAmount = totalAmount,
            DiscountAmount = discountAmount, // Số tiền giảm
            DiscountCode = discountCode, // Mã giảm giá
            ShippingFee = shippingFee, // Phí vận chuyển
            TotalWithShipping = finalTotal // Tổng thanh toán sau khi tính phí vận chuyển và giảm giá
        };

        // Trả về view với dữ liệu của model
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

        // Tính tổng tiền giỏ hàng
        decimal totalAmount = cartItems.Sum(item => item.TotalPrice);

        // Tính phí vận chuyển
        decimal shippingFee = totalAmount < 500000 ? 40000 : 0;

        // Tính tổng tiền với phí vận chuyển
        decimal totalWithShipping = totalAmount + shippingFee;

        // ==== Xử lý mã giảm giá ====
        string discountCode = HttpContext.Session.GetString("DiscountCode");
        decimal discountPercentage = 0;
        decimal discountAmount = 0;

        if (!string.IsNullOrEmpty(discountCode))
        {
            var discount = _context.Discounts.FirstOrDefault(d =>
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

        // Tổng thanh toán cuối cùng
        decimal finalTotal = totalWithShipping - discountAmount;


        // Tạo hóa đơn
        var invoice = new Invoice
        {
            UserId = userId,
            TotalAmount = finalTotal,
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

            // ==== Tính lại giá chi tiết ====
            // Tổng tiền sản phẩm này
            decimal itemSubtotal = item.TotalPrice;

            // Phí ship phân bổ cho sản phẩm này (nếu muốn chia đều phí ship cho từng món)
            decimal itemShippingFee = (totalAmount > 0) ? (itemSubtotal / totalAmount) * shippingFee : 0;

            // Giảm giá phân bổ cho sản phẩm này
            decimal itemDiscountAmount = (totalAmount > 0) ? (itemSubtotal / totalAmount) * discountAmount : 0;

            // Final = Subtotal + phí ship phân bổ - giảm giá phân bổ
            decimal itemFinalAmount = itemSubtotal + itemShippingFee - itemDiscountAmount;

            // Thêm chi tiết hóa đơn
            invoiceDetails.Add(new InvoiceDetail
            {
                InvoiceId = invoice.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Subtotal = itemSubtotal, // Giá gốc
                DiscountAmount = itemDiscountAmount, // Số tiền giảm
                FinalAmount = itemFinalAmount, // Giá cuối cùng sau ship và giảm giá
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



    [HttpPost]
    public async Task<IActionResult> Confirm()
    {
        var userId = _httpContextAccessor.HttpContext?.Session.GetInt32("UserId");

        // Kiểm tra nếu chưa đăng nhập
        if (userId == null)
        {
            TempData["Error"] = "Bạn cần đăng nhập để tiếp tục.";
            return RedirectToAction("Login", "Account");
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user != null)
        {
            ViewBag.UserAddress = user.Address;
            ViewBag.UserPhone = user.Phone;
            ViewBag.UserName = user.Username;
            ViewBag.UserEmail = user.Email;
        }

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

        // Tính tổng tiền giỏ hàng
        decimal totalAmount = cartItems.Sum(item => item.TotalPrice);

        // Tính phí vận chuyển
        decimal shippingFee = totalAmount < 500000 ? 40000 : 0;

        // Tính tổng tiền với phí vận chuyển
        decimal totalWithShipping = totalAmount + shippingFee;

        // ==== Xử lý mã giảm giá ====
        string discountCode = HttpContext.Session.GetString("DiscountCode");
        decimal discountPercentage = 0;
        decimal discountAmount = 0;

        if (!string.IsNullOrEmpty(discountCode))
        {
            var discount = _context.Discounts.FirstOrDefault(d =>
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

        // Tổng thanh toán cuối cùng
        decimal finalTotal = totalWithShipping - discountAmount;

        // Tạo hóa đơn
        var invoice = new Invoice
        {
            UserId = userId.Value,
            TotalAmount = finalTotal,
            IssueDate = DateOnly.FromDateTime(DateTime.Now),
            IsPaid = false,
            IsActive = true
        };

        _context.Invoices.Add(invoice);
        await _context.SaveChangesAsync(); // Lưu để lấy Invoice.Id

        // Danh sách chi tiết hóa đơn
        List<InvoiceDetail> invoiceDetails = new List<InvoiceDetail>();

        foreach (var item in cartItems)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
            if (product == null || product.Quantity < item.Quantity)
            {
                TempData["ErrorMessage"] = $"Sản phẩm '{item.ProductName}' không đủ số lượng trong kho.";
                return RedirectToAction("Index", "ShoppingCart");
            }

            // Trừ số lượng trong kho
            product.Quantity -= item.Quantity;
            _context.Products.Update(product);

            // ==== Tính lại giá chi tiết ====
            decimal itemSubtotal = item.TotalPrice;

            // Phí ship phân bổ cho sản phẩm này
            decimal itemShippingFee = (totalAmount > 0) ? (itemSubtotal / totalAmount) * shippingFee : 0;

            // Giảm giá phân bổ cho sản phẩm này
            decimal itemDiscountAmount = (totalAmount > 0) ? (itemSubtotal / totalAmount) * discountAmount : 0;

            // Final = Subtotal + phí ship phân bổ - giảm giá phân bổ
            decimal itemFinalAmount = itemSubtotal + itemShippingFee - itemDiscountAmount;

            // Thêm chi tiết hóa đơn
            invoiceDetails.Add(new InvoiceDetail
            {
                InvoiceId = invoice.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Subtotal = itemSubtotal,
                DiscountAmount = itemDiscountAmount,
                FinalAmount = itemFinalAmount,
                CreatedAt = DateTime.Now,
                IsActive = true
            });
        }

        // Lưu chi tiết hóa đơn
        await _context.InvoiceDetails.AddRangeAsync(invoiceDetails);
        await _context.SaveChangesAsync();

        // Xoá giỏ hàng sau khi đặt
        HttpContext.Session.Remove("CartItems");

        // Truyền dữ liệu sang trang xác nhận
        ViewBag.TotalAmount = totalAmount;
        ViewBag.ShippingFee = shippingFee;
        ViewBag.DiscountAmount = discountAmount;
        ViewBag.FinalTotal = finalTotal;

        ViewBag.Invoice = invoice;
        ViewBag.InvoiceDetails = invoiceDetails;
        ViewBag.CartItems = cartItems;

        return View("Confirm"); // Trả về view xác nhận
    }







    // Trang thành công khi đặt hàng
    public IActionResult OrderSuccess()
    {
        return View("OrderSuccess"); // Trả về view OrderSuccess.cshtml
    }
}