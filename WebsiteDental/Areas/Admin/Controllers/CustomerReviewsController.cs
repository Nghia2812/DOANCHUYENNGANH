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
    public class CustomerReviewsController : Controller
    {
        private readonly WebsiteDentalContext _context;

        public CustomerReviewsController(WebsiteDentalContext context)
        {
            _context = context;
        }

        // GET: Admin/CustomerReviews
        public async Task<IActionResult> Index()
        {
            var websiteDentalContext = _context.CustomerReviews.Include(c => c.User);
            return View(await websiteDentalContext.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Vui lòng chọn ảnh hợp lệ!");
            }

            // Tạo thư mục nếu chưa tồn tại
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/img/CustomerReviews");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Tạo tên file duy nhất
            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Lưu file vào thư mục
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            // Trả về đường dẫn ảnh đúng định dạng cho web
            string imagePath = $"/assets/img/CustomerReviews/{uniqueFileName}";
            // Trả về đường dẫn ảnh để lưu vào database

            return Ok(imagePath);
        }
        // GET: Admin/CustomerReviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerReview = await _context.CustomerReviews
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customerReview == null)
            {
                return NotFound();
            }

            return View(customerReview);
        }

        // GET: Admin/CustomerReviews/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Admin/CustomerReviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Customer,Email,Rating,Review,CreatedAt,IsActive,ImageData")] CustomerReview customerReview)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customerReview);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", customerReview.UserId);
            return View(customerReview);
        }

        // GET: Admin/CustomerReviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerReview = await _context.CustomerReviews.FindAsync(id);
            if (customerReview == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", customerReview.UserId);
            return View(customerReview);
        }

        // POST: Admin/CustomerReviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Customer,Email,Rating,Review,CreatedAt,IsActive,ImageData")] CustomerReview customerReview)
        {
            if (id != customerReview.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customerReview);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerReviewExists(customerReview.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", customerReview.UserId);
            return View(customerReview);
        }

        // GET: Admin/CustomerReviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerReview = await _context.CustomerReviews
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customerReview == null)
            {
                return NotFound();
            }

            return View(customerReview);
        }

        // POST: Admin/CustomerReviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customerReview = await _context.CustomerReviews.FindAsync(id);
            if (customerReview != null)
            {
                _context.CustomerReviews.Remove(customerReview);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerReviewExists(int id)
        {
            return _context.CustomerReviews.Any(e => e.Id == id);
        }
    }
}
