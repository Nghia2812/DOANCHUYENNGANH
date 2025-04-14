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
    public class DoctorsCategoriesController : Controller
    {
        private readonly WebsiteDentalContext _context;

        public DoctorsCategoriesController(WebsiteDentalContext context)
        {
            _context = context;
        }

        // GET: Admin/DoctorsCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.DoctorsCategories.ToListAsync());
        }

        // GET: Admin/DoctorsCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctorsCategory = await _context.DoctorsCategories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (doctorsCategory == null)
            {
                return NotFound();
            }

            return View(doctorsCategory);
        }

        // GET: Admin/DoctorsCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/DoctorsCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName,Icon,Position")] DoctorsCategory doctorsCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(doctorsCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(doctorsCategory);
        }

        // GET: Admin/DoctorsCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctorsCategory = await _context.DoctorsCategories.FindAsync(id);
            if (doctorsCategory == null)
            {
                return NotFound();
            }
            return View(doctorsCategory);
        }

        // POST: Admin/DoctorsCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName,Icon,Position")] DoctorsCategory doctorsCategory)
        {
            if (id != doctorsCategory.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doctorsCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorsCategoryExists(doctorsCategory.CategoryId))
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
            return View(doctorsCategory);
        }

        // GET: Admin/DoctorsCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctorsCategory = await _context.DoctorsCategories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (doctorsCategory == null)
            {
                return NotFound();
            }

            return View(doctorsCategory);
        }

        // POST: Admin/DoctorsCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctorsCategory = await _context.DoctorsCategories.FindAsync(id);
            if (doctorsCategory != null)
            {
                _context.DoctorsCategories.Remove(doctorsCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorsCategoryExists(int id)
        {
            return _context.DoctorsCategories.Any(e => e.CategoryId == id);
        }
    }
}
