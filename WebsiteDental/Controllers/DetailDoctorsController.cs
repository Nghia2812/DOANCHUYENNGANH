using Microsoft.AspNetCore.Mvc;
using WebsiteDental.Models;
using System.Linq;

namespace WebsiteDental.Controllers
{
    public class DetailDoctorsController : Controller
    {
        private readonly WebsiteDentalContext _context;

        public DetailDoctorsController(WebsiteDentalContext context)
        {
            _context = context;
        }

        public IActionResult Detail(int id)
        {
            var doctor = _context.Doctors.FirstOrDefault(d => d.Id == id);
            var certificates = _context.DoctorCertificates.Where(c => c.DoctorId == id).ToList();

            if (doctor == null)
            {
                return NotFound();
            }

            var model = new DoctorsDetailViewModel
            {
                Doctor = doctor,
                Certificates = certificates
            };

            return View("DetailDoctors", model); // Chỉ định View đúng với file DetailDoctors.cshtml
        }

    }
}
