using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace WebsiteDental.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly string _connectionString;

        public HomeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("Username");
            ViewBag.Username = username;

            // Gọi hàm lấy doanh thu trong ngày
            decimal dailyRevenue = GetDailyRevenue();
            ViewBag.DailyRevenue = dailyRevenue;

            // Gọi hàm lấy tổng số lượt đặt lịch trong ngày
            int totalAppointmentsToday = GetTotalAppointmentsToday();
            ViewBag.TotalAppointmentsToday = totalAppointmentsToday;

            return View();
        }


        //doanh thu đơn hàng
        private decimal GetDailyRevenue()
        {
            decimal totalRevenue = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT ISNULL(SUM(total_amount), 0) 
                    FROM Invoices 
                    WHERE CAST(issue_date AS DATE) = CAST(GETDATE() AS DATE)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        totalRevenue = Convert.ToDecimal(result);
                    }
                }
            }
            return totalRevenue;
        }



        //doanh thu số liệu đặt lịch hẹn
        private int GetTotalAppointmentsToday()
        {
            int totalAppointments = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT COUNT(*) 
                    FROM Appointments 
                    WHERE CAST(appointment_date AS DATE) = CAST(GETDATE() AS DATE)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    totalAppointments = (int)cmd.ExecuteScalar();
                }
            }
            return totalAppointments;
        }

        //lưu vào bảng doanh thu

    }
}
