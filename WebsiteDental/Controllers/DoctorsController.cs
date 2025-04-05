using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebsiteDental.Models;
using WebsiteDental.ViewComponents;

namespace WebsiteDental.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly WebsiteDentalContext _context; // DbContext

        public DoctorsController(WebsiteDentalContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? categoryID)
        {

            var categories = _context.DoctorsCategories?.ToList() ?? new List<DoctorsCategory>();
            var Doctors = categoryID.HasValue
                ? _context.Doctors.Where(n => n.CategoryId == categoryID.Value).ToList()
                : _context.Doctors.ToList();

            ViewBag.DoctorsCategories = categories;
            ViewBag.Doctors = Doctors;


            return View();
        }
    }
}
