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


        [HttpPost]
        public IActionResult BookAppointment(int DoctorId, string CustomerName, DateTime AppointmentDate, string Phone, string Email, string Sex, string Notes)
        {
            if (string.IsNullOrWhiteSpace(CustomerName) || string.IsNullOrWhiteSpace(Phone))
            {
                return RedirectToAction("AppointmentsDoctors", new { doctorId = DoctorId });
            }

            var appointment = new Appointment
            {
                DoctorId = DoctorId,
                CustomerName = CustomerName,
                AppointmentDate = AppointmentDate,
                Phone = Phone,
                Email = Email,
                Sex = Sex,
                Notes = Notes,
                Status = "Chờ xác nhận",
                IsActive = true
            };

            _context.Appointments.Add(appointment);
            _context.SaveChanges();

            //return RedirectToAction("AppointmentsDoctors", new { doctorId = DoctorId });
            // Trả về trang Success.cshtml sau khi đặt lịch thành công
            return View("~/Views/DetailDoctors/Success.cshtml");
        }
    }
}

