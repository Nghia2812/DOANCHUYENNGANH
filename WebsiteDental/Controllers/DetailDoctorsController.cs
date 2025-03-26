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
            var comments = _context.CommentDoctors // Lấy bình luận từ bảng CommentDoctors
               .Where(c => c.DoctorId == id)
               .OrderByDescending(c => c.CreatedAt) // Hiển thị bình luận mới nhất trước
               .ToList();

            if (doctor == null)
            {
                return NotFound();
            }

            var model = new DoctorsDetailViewModel
            {

                Doctor = doctor,
                Certificates = certificates,
                Comments = comments
            };

            return View("DetailDoctors", model);
        }

        [HttpPost]
        public IActionResult AddComment(int DoctorId, string AuthorName, string Content)
        {
            if (string.IsNullOrWhiteSpace(AuthorName) || string.IsNullOrWhiteSpace(Content))
            {
                TempData["Error"] = "Tên và bình luận không được để trống.";
                return RedirectToAction("Detail", new { id = DoctorId });
            }

            var newComment = new CommentDoctor // Đảm bảo model này trùng với bảng CommentDoctors
            {
                DoctorId = DoctorId,
                Username = AuthorName,
                CommentText = Content,
                CreatedAt = DateTime.Now
            };

            _context.CommentDoctors.Add(newComment); // Lưu vào bảng CommentDoctors
            _context.SaveChanges();

            return RedirectToAction("Detail", new { id = DoctorId });
        }
    }
}
