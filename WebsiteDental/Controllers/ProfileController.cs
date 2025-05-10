using Microsoft.AspNetCore.Mvc;

namespace WebsiteDental.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
