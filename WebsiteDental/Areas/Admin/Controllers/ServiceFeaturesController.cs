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
    public class ServiceFeaturesController : Controller
    {
        private readonly WebsiteDentalContext _context;

        public ServiceFeaturesController(WebsiteDentalContext context)
        {
            _context = context;
        }

        // GET: Admin/ServiceFeatures
        public async Task<IActionResult> Index()
        {
            return View(await _context.ServiceFeatures.ToListAsync());
        }

        // GET: Admin/ServiceFeatures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceFeature = await _context.ServiceFeatures
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serviceFeature == null)
            {
                return NotFound();
            }

            return View(serviceFeature);
        }

        // GET: Admin/ServiceFeatures/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ServiceFeatures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Icon,Order,IsActive")] ServiceFeature serviceFeature)
        {
            if (ModelState.IsValid)
            {
                _context.Add(serviceFeature);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(serviceFeature);
        }

        // GET: Admin/ServiceFeatures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceFeature = await _context.ServiceFeatures.FindAsync(id);
            if (serviceFeature == null)
            {
                return NotFound();
            }
            return View(serviceFeature);
        }

        // POST: Admin/ServiceFeatures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Icon,Order,IsActive")] ServiceFeature serviceFeature)
        {
            if (id != serviceFeature.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceFeature);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceFeatureExists(serviceFeature.Id))
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
            return View(serviceFeature);
        }

        // GET: Admin/ServiceFeatures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceFeature = await _context.ServiceFeatures
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serviceFeature == null)
            {
                return NotFound();
            }

            return View(serviceFeature);
        }

        // POST: Admin/ServiceFeatures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceFeature = await _context.ServiceFeatures.FindAsync(id);
            if (serviceFeature != null)
            {
                _context.ServiceFeatures.Remove(serviceFeature);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceFeatureExists(int id)
        {
            return _context.ServiceFeatures.Any(e => e.Id == id);
        }
    }
}
