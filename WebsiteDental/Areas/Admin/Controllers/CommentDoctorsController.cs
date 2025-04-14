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
    public class CommentDoctorsController : Controller
    {
        private readonly WebsiteDentalContext _context;

        public CommentDoctorsController(WebsiteDentalContext context)
        {
            _context = context;
        }

        // GET: Admin/CommentDoctors
        public async Task<IActionResult> Index()
        {
            var websiteDentalContext = _context.CommentDoctors.Include(c => c.Doctor);
            return View(await websiteDentalContext.ToListAsync());
        }

        // GET: Admin/CommentDoctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentDoctor = await _context.CommentDoctors
                .Include(c => c.Doctor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commentDoctor == null)
            {
                return NotFound();
            }

            return View(commentDoctor);
        }

        // GET: Admin/CommentDoctors/Create
        public IActionResult Create()
        {
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Id");
            return View();
        }

        // POST: Admin/CommentDoctors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DoctorId,Username,CommentText,CreatedAt")] CommentDoctor commentDoctor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(commentDoctor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Id", commentDoctor.DoctorId);
            return View(commentDoctor);
        }

        // GET: Admin/CommentDoctors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentDoctor = await _context.CommentDoctors.FindAsync(id);
            if (commentDoctor == null)
            {
                return NotFound();
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Id", commentDoctor.DoctorId);
            return View(commentDoctor);
        }

        // POST: Admin/CommentDoctors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DoctorId,Username,CommentText,CreatedAt")] CommentDoctor commentDoctor)
        {
            if (id != commentDoctor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(commentDoctor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentDoctorExists(commentDoctor.Id))
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
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "Id", "Id", commentDoctor.DoctorId);
            return View(commentDoctor);
        }

        // GET: Admin/CommentDoctors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentDoctor = await _context.CommentDoctors
                .Include(c => c.Doctor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commentDoctor == null)
            {
                return NotFound();
            }

            return View(commentDoctor);
        }

        // POST: Admin/CommentDoctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var commentDoctor = await _context.CommentDoctors.FindAsync(id);
            if (commentDoctor != null)
            {
                _context.CommentDoctors.Remove(commentDoctor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentDoctorExists(int id)
        {
            return _context.CommentDoctors.Any(e => e.Id == id);
        }
    }
}
