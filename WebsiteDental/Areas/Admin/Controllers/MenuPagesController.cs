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
    public class MenuPagesController : Controller
    {
        private readonly WebsiteDentalContext _context;

        public MenuPagesController(WebsiteDentalContext context)
        {
            _context = context;
        }

        // GET: Admin/MenuPages
        public async Task<IActionResult> Index()
        {
            var websiteDentalContext = _context.MenuPages.Include(m => m.Parent);
            return View(await websiteDentalContext.ToListAsync());
        }

        // GET: Admin/MenuPages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuPage = await _context.MenuPages
                .Include(m => m.Parent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menuPage == null)
            {
                return NotFound();
            }

            return View(menuPage);
        }

        // GET: Admin/MenuPages/Create
        public IActionResult Create()
        {
            ViewData["ParentId"] = new SelectList(_context.MenuPages, "Id", "Id");
            return View();
        }

        // POST: Admin/MenuPages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PageName,Url,Position,IsActive,ParentId")] MenuPage menuPage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(menuPage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentId"] = new SelectList(_context.MenuPages, "Id", "Id", menuPage.ParentId);
            return View(menuPage);
        }

        // GET: Admin/MenuPages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuPage = await _context.MenuPages.FindAsync(id);
            if (menuPage == null)
            {
                return NotFound();
            }
            ViewData["ParentId"] = new SelectList(_context.MenuPages, "Id", "Id", menuPage.ParentId);
            return View(menuPage);
        }

        // POST: Admin/MenuPages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PageName,Url,Position,IsActive,ParentId")] MenuPage menuPage)
        {
            if (id != menuPage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(menuPage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuPageExists(menuPage.Id))
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
            ViewData["ParentId"] = new SelectList(_context.MenuPages, "Id", "Id", menuPage.ParentId);
            return View(menuPage);
        }

        // GET: Admin/MenuPages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuPage = await _context.MenuPages
                .Include(m => m.Parent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menuPage == null)
            {
                return NotFound();
            }

            return View(menuPage);
        }

        // POST: Admin/MenuPages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menuPage = await _context.MenuPages.FindAsync(id);
            if (menuPage != null)
            {
                _context.MenuPages.Remove(menuPage);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MenuPageExists(int id)
        {
            return _context.MenuPages.Any(e => e.Id == id);
        }

        [HttpPost]
        public IActionResult Edit(MenuPage menuPage)
        {
            if (ModelState.IsValid)
            {
                // Cập nhật giá trị IsActive nếu có thay đổi
                var isActive = menuPage.IsActive ?? false;
                menuPage.IsActive = isActive;

                // Save the menuPage to the database
                _context.Update(menuPage);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(menuPage);
        }

    }
}
