using Microsoft.AspNetCore.Mvc;
using WebsiteDental.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace WebsiteDental.Controllers
{
    public class ServicesController : Controller
    {
        private readonly WebsiteDentalContext _context;

        public ServicesController(WebsiteDentalContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? categoryId)
        {
            var categories = _context.ServiceCategories?.ToList() ?? new List<ServiceCategory>();

            var services = categoryId.HasValue
                ? _context.Services.Where(s => s.CategoryId == categoryId.Value).ToList()
                : _context.Services.Take(24).ToList();

            ViewBag.ServicesCategories = categories;
            ViewBag.Services = services;

            return View();
        }


    }
}
