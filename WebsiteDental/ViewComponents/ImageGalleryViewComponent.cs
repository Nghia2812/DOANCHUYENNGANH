using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using WebsiteDental.Models; // Import model

namespace WebsiteDental.ViewComponents
{
    [ViewComponent]
    public class ImageGallery : ViewComponent

    {
        private readonly WebsiteDentalContext _context;

        public ImageGallery(WebsiteDentalContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var ImageGallery = await _context.ImageGalleries.ToListAsync(); // Lấy dữ liệu từ SQL


            return View(ImageGallery);// Trả về View
        }
    }
}
