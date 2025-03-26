using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebsiteDental.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WebsiteDental.Controllers
{
    [Route("AppointmentsDoctors")] // Định nghĩa route cho controller
    public class AppointmentsDoctorsController : Controller
    {
        private readonly WebsiteDentalContext _context;

        public AppointmentsDoctorsController(WebsiteDentalContext context)
        {
            _context = context;
        }



        [HttpGet]
        public async Task<IActionResult> AppointmentsDoctors(int doctorId)
        {
            // Tìm bác sĩ theo ID
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == doctorId);

            if (doctor == null)
            {
                return NotFound("Không tìm thấy bác sĩ!");
            }

            // Tạo Model chứa thông tin bác sĩ
            var viewModel = new AppointmentsDoctorsModelView
            {
                DoctorId = doctor.Id,
                DoctorName = doctor.FullName,
                ImageUrl = doctor.Image
            };

            return View("~/Views/DetailDoctors/AppointmentsDoctors.cshtml", viewModel);
        }


        [HttpPost("BookAppointment")]
        public async Task<IActionResult> BookAppointment(AppointmentsDoctorsModelView model)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/DetailDoctors/AppointmentsDoctors.cshtml", model);
            }

            var doctor = await _context.Doctors.FindAsync(model.DoctorId);
            if (doctor == null)
            {
                ModelState.AddModelError("", "Không tìm thấy bác sĩ.");
                return View("~/Views/DetailDoctors/AppointmentsDoctors.cshtml", model);
            }

            var appointment = new Appointment
            {
                DoctorId = model.DoctorId,
                CustomerName = model.CustomerName,
                AppointmentDate = model.AppointmentDate,
                Phone = model.Phone,
                Email = model.Email,
                Sex = model.Sex,
                Notes = model.Notes,
                Status = "Chờ xác nhận",
                IsActive = true
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đặt lịch thành công! Chúng tôi sẽ liên hệ sớm.";
            return RedirectToAction("AppointmentsDoctors", "DetailDoctors", new { id = model.DoctorId });
        }



    }
}

