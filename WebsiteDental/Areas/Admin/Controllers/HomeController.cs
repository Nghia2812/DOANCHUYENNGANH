using Microsoft.AspNetCore.Mvc;

namespace WebsiteDental.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("Username");
            ViewBag.Username = username; // Truyền username vào ViewBag
            return View();
        }

    }
}
