using WebsiteDental.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace WebsiteDental.ViewComponents
{
    public class MenuTopViewComponent : ViewComponent
    {
        private readonly WebsiteDentalContext _context;

        public MenuTopViewComponent(WebsiteDentalContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _context.MenuPages
                .Where(m => m.IsActive == true)  
                .OrderBy(m => m.Position)       
                .ToListAsync();

            return View(items);
        }
    }
}
