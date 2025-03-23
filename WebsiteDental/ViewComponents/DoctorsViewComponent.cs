using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using WebsiteDental.Models; // Import model

namespace WebsiteDental.ViewComponents
{
    [ViewComponent]
    public class Doctors: ViewComponent

    {
        private readonly WebsiteDentalContext _context;

        public Doctors(WebsiteDentalContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var Doctors = await _context.Doctors.ToListAsync(); // Lấy dữ liệu từ SQL

            return View(Doctors);// Trả về View
        }
    }
}