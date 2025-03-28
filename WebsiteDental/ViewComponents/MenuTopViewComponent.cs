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

            var menuHierarchy = _context.MenuPages
     .Where(m => m.ParentId == null)
     .Select(m => new MenuPage
     {
         Id = m.Id,              // Đổi MenuId = m.Id (đúng với cột id)
         PageName = m.PageName,      // Đổi MenuName = m.PageName
         Url = m.Url,
         Position = m.Position,
         IsActive = m.IsActive,
         InverseParent = _context.MenuPages
                         .Where(sub => sub.ParentId == m.Id)
                         .ToList()   // Lấy danh sách menu con có ParentId = Id của menu cha
     })
     .ToList();

            return View(menuHierarchy);
        }
    }
}
