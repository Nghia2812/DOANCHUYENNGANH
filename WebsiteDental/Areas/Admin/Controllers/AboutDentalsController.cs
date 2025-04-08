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
    public class AboutDentalsController : Controller
    {
        private readonly WebsiteDentalContext _context;

        public AboutDentalsController(WebsiteDentalContext context)
        {
            _context = context;
        }

        // GET: Admin/AboutDentals
        public async Task<IActionResult> Index()
        {
            return View(await _context.AboutDentals.ToListAsync());
        }

        // GET: Admin/AboutDentals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aboutDental = await _context.AboutDentals
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aboutDental == null)
            {
                return NotFound();
            }

            return View(aboutDental);
        }

        // GET: Admin/AboutDentals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AboutDentals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content,Description,IconPath,ImagePath,StatValue1,StatLabel1,CreatedDate,UpdatedDate,IsActive")] AboutDental aboutDental)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aboutDental);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aboutDental);
        }

        // GET: Admin/AboutDentals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aboutDental = await _context.AboutDentals.FindAsync(id);
            if (aboutDental == null)
            {
                return NotFound();
            }
            return View(aboutDental);
        }

        // POST: Admin/AboutDentals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,Description,IconPath,ImagePath,StatValue1,StatLabel1,CreatedDate,UpdatedDate,IsActive")] AboutDental aboutDental)
        {
            if (id != aboutDental.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aboutDental);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutDentalExists(aboutDental.Id))
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
            return View(aboutDental);
        }

        // GET: Admin/AboutDentals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aboutDental = await _context.AboutDentals
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aboutDental == null)
            {
                return NotFound();
            }

            return View(aboutDental);
        }

        // POST: Admin/AboutDentals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aboutDental = await _context.AboutDentals.FindAsync(id);
            if (aboutDental != null)
            {
                _context.AboutDentals.Remove(aboutDental);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AboutDentalExists(int id)
        {
            return _context.AboutDentals.Any(e => e.Id == id);
        }
    }
}
