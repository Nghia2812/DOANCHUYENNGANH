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
    public class BlogCommentsController : Controller
    {
        private readonly WebsiteDentalContext _context;

        public BlogCommentsController(WebsiteDentalContext context)
        {
            _context = context;
        }

        // GET: Admin/BlogComments
        public async Task<IActionResult> Index()
        {
            var websiteDentalContext = _context.BlogComments.Include(b => b.Post).Include(b => b.User);
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
        // GET: Admin/BlogComments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogComment = await _context.BlogComments
                .Include(b => b.Post)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogComment == null)
            {
                return NotFound();
            }

            return View(blogComment);
        }

        // GET: Admin/BlogComments/Create
        public IActionResult Create()
        {
            ViewData["PostId"] = new SelectList(_context.BlogPosts, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Admin/BlogComments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PostId,UserId,Comment,CreatedAt,IsActive,Email,Img,Name")] BlogComment blogComment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(blogComment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PostId"] = new SelectList(_context.BlogPosts, "Id", "Id", blogComment.PostId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", blogComment.UserId);
            return View(blogComment);
        }

        // GET: Admin/BlogComments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogComment = await _context.BlogComments.FindAsync(id);
            if (blogComment == null)
            {
                return NotFound();
            }
            ViewData["PostId"] = new SelectList(_context.BlogPosts, "Id", "Id", blogComment.PostId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", blogComment.UserId);
            return View(blogComment);
        }

        // POST: Admin/BlogComments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PostId,UserId,Comment,CreatedAt,IsActive,Email,Img,Name")] BlogComment blogComment)
        {
            if (id != blogComment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blogComment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogCommentExists(blogComment.Id))
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
            ViewData["PostId"] = new SelectList(_context.BlogPosts, "Id", "Id", blogComment.PostId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", blogComment.UserId);
            return View(blogComment);
        }

        // GET: Admin/BlogComments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogComment = await _context.BlogComments
                .Include(b => b.Post)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogComment == null)
            {
                return NotFound();
            }

            return View(blogComment);
        }

        // POST: Admin/BlogComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogComment = await _context.BlogComments.FindAsync(id);
            if (blogComment != null)
            {
                _context.BlogComments.Remove(blogComment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogCommentExists(int id)
        {
            return _context.BlogComments.Any(e => e.Id == id);
        }
    }
}
