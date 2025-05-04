using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebsiteDental.Models;

namespace WebsiteDental.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InvoicesController : Controller
    {
        private readonly WebsiteDentalContext _context;

        public InvoicesController(WebsiteDentalContext context)
        {
            _context = context;
        }

        // GET: Admin/Invoices
        public async Task<IActionResult> Index()
        {
            var websiteDentalContext = _context.Invoices
                .Include(i => i.Patient)
                .Include(i => i.User)
                .Include(i => i.InvoiceDetails) // Bao gồm bảng InvoiceDetails
                    .ThenInclude(id => id.Product); // Bao gồm bảng Product thông qua InvoiceDetails
            return View(await websiteDentalContext.ToListAsync());
        }


        // GET: Admin/Invoices/Details/5      
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Patient)
                .Include(i => i.User)
                .Include(i => i.InvoiceDetails) // Bao gồm bảng InvoiceDetails
                    .ThenInclude(id => id.Product) // Bao gồm bảng Product thông qua InvoiceDetails
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }


      
        // GET: Admin/Invoices/Create
        public IActionResult Create()
        {
            var products = _context.Products.ToList();

            // Truyền thông tin bệnh nhân, người dùng và sản phẩm vào View
            ViewBag.PatientId = new SelectList(_context.Patients, "Id", "Id");
            ViewBag.UserId = new SelectList(_context.Users, "Id", "Id");
            ViewBag.Products = new SelectList(products, "Id", "ProductName");

            // Truyền bảng giá sản phẩm cho View
            ViewBag.ProductPrices = products.ToDictionary(p => p.Id, p => p.Price);

            return View();
        }


        // POST: Admin/Invoices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,PatientId,TotalAmount,IssueDate,IsPaid,IsActive,InvoiceDetails")] Invoice invoice,
            string Username, string Phone, string Email, string Address)
        {
            if (ModelState.IsValid)
            {
                // Tạo người dùng mới
                var newUser = new User
                {
                    Username = Username,
                    Phone = Phone,
                    Email = Email,
                    Address = Address,
                    // Cần thêm Password
                    Password = "123" // hoặc lấy từ form, hoặc sinh ngẫu nhiên
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                // Gán UserId vào hóa đơn
                invoice.UserId = newUser.Id;

                // Lưu hóa đơn
                _context.Invoices.Add(invoice);
                await _context.SaveChangesAsync(); // Lưu hóa đơn trước khi tạo chi tiết hóa đơn

                // Lưu chi tiết hóa đơn nếu có
                // Lưu chi tiết hóa đơn nếu có
                if (invoice.InvoiceDetails != null && invoice.InvoiceDetails.Count > 0)
                {
                    foreach (var detail in invoice.InvoiceDetails)
                    {
                        // Xóa Id nếu có (tránh lỗi insert vào identity column)
                        detail.Id = 0;

                        // Gán InvoiceId cho chi tiết hóa đơn
                        detail.InvoiceId = invoice.Id;

                        // Kiểm tra các thông tin chi tiết hóa đơn
                        if (detail.ProductId == 0 || detail.Quantity <= 0 || detail.FinalAmount <= 0)
                        {
                            ModelState.AddModelError("", "Chi tiết hóa đơn không hợp lệ. Vui lòng kiểm tra lại thông tin.");
                            return View(invoice); // Trả về nếu có lỗi
                        }

                        // Tạo mới chi tiết hóa đơn
                        _context.InvoiceDetails.Add(detail); // Thêm chi tiết hóa đơn vào DbContext
                    }

                    await _context.SaveChangesAsync(); // Lưu các thay đổi vào cơ sở dữ liệu
                }


                return RedirectToAction(nameof(Index)); // Điều hướng đến trang danh sách hóa đơn
            }

            // Nếu model không hợp lệ, trả về view để chỉnh sửa
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", invoice.PatientId);
            ViewData["Products"] = new SelectList(_context.Products, "Id", "ProductName");
            return View(invoice);
        }



        // GET: Admin/Invoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.InvoiceDetails) // Bao gồm bảng InvoiceDetails
                    .ThenInclude(id => id.Product) // Bao gồm bảng Product thông qua InvoiceDetails
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
            {
                return NotFound();
            }

            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", invoice.PatientId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", invoice.UserId);
            ViewData["Products"] = new SelectList(_context.Products, "Id", "ProductName"); // Dropdown cho Products
            return View(invoice);
        }

        // POST: Admin/Invoices/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PatientId,TotalAmount,IssueDate,IsPaid,IsActive,UserId, InvoiceDetails")] Invoice invoice)
        {
            if (id != invoice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();

                    // Cập nhật các chi tiết hóa đơn
                    if (invoice.InvoiceDetails != null && invoice.InvoiceDetails.Count > 0)
                    {
                        foreach (var detail in invoice.InvoiceDetails)
                        {
                            if (detail.Id == 0)
                            {
                                _context.InvoiceDetails.Add(detail);
                            }
                            else
                            {
                                _context.Update(detail);
                            }
                        }
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Id", invoice.PatientId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", invoice.UserId);
            ViewData["Products"] = new SelectList(_context.Products, "Id", "ProductName"); // Cập nhật lại dropdown cho Products
            return View(invoice);
        }


        // GET: Admin/Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Patient)
                .Include(i => i.User)
                .Include(i => i.InvoiceDetails) // Bao gồm bảng InvoiceDetails
                    .ThenInclude(id => id.Product) // Bao gồm bảng Product thông qua InvoiceDetails
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Admin/Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.InvoiceDetails) // Bao gồm bảng InvoiceDetails
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice != null)
            {
                _context.InvoiceDetails.RemoveRange(invoice.InvoiceDetails); // Xóa các chi tiết hóa đơn
                _context.Invoices.Remove(invoice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool InvoiceExists(int id)
        {
            return _context.Invoices.Any(e => e.Id == id);
        }
    }
}
