using Microsoft.AspNetCore.Mvc;
using WebsiteDental.Models;  // Thay thế bằng namespace của model User
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace WebsiteDental.Controllers
{
    public class ProfileController : Controller
    {
        private readonly WebsiteDentalContext _context;

        public ProfileController(WebsiteDentalContext context)
        {
            _context = context;
        }

        // Trang hiển thị thông tin người dùng
        public IActionResult Index()
        {
            try
            {
                // 🔑 Lấy username từ session (chú ý: phân biệt chữ hoa chữ thường)
                string username = HttpContext.Session.GetString("Username");

                // 🛑 Kiểm tra nếu chưa đăng nhập
                if (string.IsNullOrEmpty(username))
                {
                    ViewBag.ErrorMessage = "Bạn chưa đăng nhập!";
                    return View();
                }

                // ✅ In ra kiểm tra xem đã lấy được session chưa
                Console.WriteLine("Session Username: " + username);

                // 🌐 Lấy thông tin người dùng từ database
                var user = _context.Users
                    .Where(u => u.Username == username)
                    .FirstOrDefault();

                // 🛑 Nếu không tìm thấy người dùng
                if (user == null)
                {
                    ViewBag.ErrorMessage = "Không tìm thấy thông tin người dùng!";
                    return View();
                }

                // ✅ Trả về thông tin người dùng
                return View(user);
            }
            catch (Exception ex)
            {
                // ⚠️ Xử lý lỗi và hiển thị thông báo
                ViewBag.ErrorMessage = "Đã xảy ra lỗi khi lấy thông tin người dùng: " + ex.Message;
                return View();
            }
        }
    }
}
