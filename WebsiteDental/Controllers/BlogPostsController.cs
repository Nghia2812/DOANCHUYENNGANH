using Microsoft.AspNetCore.Mvc;
using WebsiteDental.Models;
using Microsoft.EntityFrameworkCore;

public class BlogPostsController : Controller
{
    private readonly WebsiteDentalContext _context;

    public BlogPostsController(WebsiteDentalContext context)
    {
        _context = context;
    }


    public async Task<IActionResult> Index()
    {
        var blogPosts = await _context.BlogPosts.ToListAsync();
        return View();
    }
}
