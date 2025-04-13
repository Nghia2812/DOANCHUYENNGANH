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
    public class ContactFormsController : Controller
    {
        private readonly WebsiteDentalContext _context;

        public ContactFormsController(WebsiteDentalContext context)
        {
            _context = context;
        }

        // GET: Admin/ContactForms
        public async Task<IActionResult> Index()
        {
            return View(await _context.ContactForms.ToListAsync());
        }


        // GET: Admin/ContactForms/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactForm = await _context.ContactForms.FindAsync(id);
            if (contactForm == null)
            {
                return NotFound();
            }
            return View(contactForm);
        }

        // POST: Admin/ContactForms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Message,Phone,SubmittedAt,IsActive")] ContactForm contactForm)
        {
            if (id != contactForm.Id)
            {
                return NotFound(); // Nếu ID trong URL không khớp với ID trong model, trả về NotFound
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contactForm); // Cập nhật đối tượng trong CSDL
                    await _context.SaveChangesAsync(); // Lưu thay đổi vào CSDL
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactFormExists(contactForm.Id)) // Nếu đối tượng không tồn tại trong CSDL
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw; // Nếu có lỗi, ném lại ngoại lệ
                    }
                }
                return RedirectToAction(nameof(Index)); // Sau khi thành công, chuyển hướng tới trang Index
            }
            return View(contactForm); // Nếu có lỗi, trả lại view với model
        }


        // GET: Admin/ContactForms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactForm = await _context.ContactForms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactForm == null)
            {
                return NotFound();
            }

            return View(contactForm);
        }

        // POST: Admin/ContactForms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contactForm = await _context.ContactForms.FindAsync(id);
            if (contactForm != null)
            {
                _context.ContactForms.Remove(contactForm);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactFormExists(int id)
        {
            return _context.ContactForms.Any(e => e.Id == id);
        }
    }
}
