using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http; // Thêm thư viện này

namespace WebsiteDental.Controllers
{
    public class AccountController : Controller
    {
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContextAccessor; // Khai báo biến
   

       

        // Constructor để lấy chuỗi kết nối từ appsettings.json và khởi tạo IHttpContextAccessor
        public AccountController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _httpContextAccessor = httpContextAccessor;
        }
       
        public IActionResult TestConnection()
        {
            return Content($"Chuỗi kết nối: {_connectionString}");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Kiểm tra email có tồn tại hay không
        private bool IsEmailExists(string email)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Users WHERE email = @Email";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        // Xử lý Đăng Ký
        [HttpPost]
        public IActionResult Register(string username, string email, string phone, string password, string confirmPassword, string address)
        {
            if (password != confirmPassword)
            {
                ViewData["Error"] = "Mật khẩu xác nhận không khớp!";
                return View();
            }

            if (IsEmailExists(email))
            {
                ViewData["Error"] = "Email đã tồn tại! Vui lòng chọn email khác.";
                return View();
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Users (username, email, phone, password, address, created_at) VALUES (@Username, @Email, @Phone, @Password, @Address, GETDATE())";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Phone", phone);
                    cmd.Parameters.AddWithValue("@Password", hashedPassword);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.ExecuteNonQuery();
                }
            }

            ViewData["Success"] = "Đăng ký thành công! Vui lòng đăng nhập.";
            return View();
        }



        // Xử lý Đăng Nhập
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT id, username, password, phone, address FROM Users WHERE email = @Email";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedUsername = reader["username"].ToString();
                            string storedHashedPassword = reader["password"].ToString();
                            int userId = Convert.ToInt32(reader["id"]);
                            string storedPhone = reader["phone"].ToString();
                            string storedAddress = reader["address"].ToString();

                            // Kiểm tra mật khẩu
                            if (BCrypt.Net.BCrypt.Verify(password, storedHashedPassword))
                            {
                                // Lưu thông tin vào session
                                _httpContextAccessor.HttpContext.Session.SetString("Username", storedUsername);
                                _httpContextAccessor.HttpContext.Session.SetInt32("UserId", userId);
                                _httpContextAccessor.HttpContext.Session.SetString("UserPhone", storedPhone);
                                _httpContextAccessor.HttpContext.Session.SetString("UserAddress", storedAddress);

                                // Kiểm tra session đã lưu đúng thông tin
                                Console.WriteLine("Session Username: " + _httpContextAccessor.HttpContext.Session.GetString("Username"));
                                Console.WriteLine("Session UserId: " + _httpContextAccessor.HttpContext.Session.GetInt32("UserId"));

                                // Chuyển hướng về trang chủ
                                return RedirectToAction("Index", "Home");
                            }
                        }
                    }
                }
            }

            ViewData["Error"] = "Email hoặc mật khẩu không đúng!";
            return View("Login"); // Chuyển về trang đăng nhập nếu không đúng
        }



        // Xử lý Đăng Xuất
        public IActionResult Logout()
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                _httpContextAccessor.HttpContext.Session.Clear(); // Xóa toàn bộ Session
            }
            return RedirectToAction("Index", "Home"); // Quay về trang chủ
        }
    }
}
