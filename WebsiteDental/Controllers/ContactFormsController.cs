using Microsoft.AspNetCore.Mvc;
using WebsiteDental.Models;

namespace WebsiteDental.Controllers
{
    public class ContactFormsController : Controller
    {
        private readonly WebsiteDentalContext _context;

        public ContactFormsController(WebsiteDentalContext context)
        {
            _context = context;
        }

        // Hiển thị form liên hệ
        public IActionResult Index()
        {
            return View();
        }

        // Xử lý khi người dùng gửi form
        [HttpPost]
        public IActionResult Create(ContactForm contactForm)
        {
            if (ModelState.IsValid)
            {
                contactForm.SubmittedAt = DateTime.Now; // Ghi lại thời gian gửi form
                contactForm.IsActive = true; // Mặc định là đang hoạt động

                _context.ContactForms.Add(contactForm);
                _context.SaveChanges();

                return RedirectToAction("Success"); // Chuyển hướng đến trang thông báo
            }
            return View("Index", contactForm); // Nếu có lỗi, quay lại trang Index
        }

        // Trang thông báo gửi thành công
        public IActionResult Success()
        {
            return View();
        }
    }
}
