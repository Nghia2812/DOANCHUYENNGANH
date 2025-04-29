using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteDental.Models;
using WebsiteDental.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace WebsiteDental.Controllers
{
    public class InvoiceHistoryController : Controller
    {
        private readonly WebsiteDentalContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public InvoiceHistoryController(WebsiteDentalContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Register", "Account");
            }

            var invoices = await _context.Invoices
                .Where(invoice => invoice.UserId == userId) // Lọc hóa đơn của người dùng
                .Include(invoice => invoice.InvoiceDetails) // Nạp chi tiết hóa đơn
                .ThenInclude(detail => detail.Product) // Nạp thông tin sản phẩm
                .Select(invoice => new InvoiceViewModel
                {
                    InvoiceId = invoice.Id,
                    OrderDate = invoice.IssueDate,
                    TotalAmount = invoice.TotalAmount,
                    Details = invoice.InvoiceDetails.Select(detail => new InvoiceDetailWithProduct
                    {
                        ProductId = detail.Id,
                        ProductName = detail.Product != null ? detail.Product.ProductName : "Không có tên sản phẩm", // Kiểm tra null
                        Quantity = detail.Quantity,
                        FinalAmount = detail.FinalAmount
                    }).ToList()
                })
                .ToListAsync(); // Chuyển thành asynchronous query

            return View(invoices);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.InvoiceDetails)
                    .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
            {
                return NotFound();
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = await _context.Users.FindAsync(userId.Value);

            var viewModel = new InvoiceViewModel
            {
                InvoiceId = invoice.Id,
                OrderDate = invoice.IssueDate,
                TotalAmount = invoice.TotalAmount,
                User = user,
                Details = invoice.InvoiceDetails.Select(d => new InvoiceDetailWithProduct
                {
                    ProductId = d.Id,
                    ProductName = d.Product?.ProductName,
                    Quantity = d.Quantity,
                    FinalAmount = d.FinalAmount
                }).ToList()
            };

            return View(viewModel);
        }



    }
}
