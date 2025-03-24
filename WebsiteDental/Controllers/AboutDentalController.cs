using Microsoft.AspNetCore.Mvc;

namespace WebsiteDental.Controllers
{
    public class AboutDentalController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
