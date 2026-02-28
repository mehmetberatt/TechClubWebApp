using Microsoft.AspNetCore.Mvc;
using TechClubWebApp.Data;
using TechClubWebApp.ViewModels;

namespace TechClubWebApp.Controllers
{
    public class HomeController : Controller
    {
       
        private readonly AppDbContext _context;

      
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        
        public IActionResult Index()
        {
         
            var activeBanners = _context.Banners.Where(b => b.IsActive).OrderBy(b => b.DisplayOrder).ToList();
            var activeAnnouncements = _context.Announcements.Where(a => a.IsActive).OrderByDescending(a => a.CreatedDate).ToList();
            var upcomingEvents = _context.Events.Where(e => e.EventDate >= DateTime.Now).OrderBy(e => e.EventDate).ToList();

          
            var viewModel = new HomeViewModel
            {
                Banners = activeBanners,
                Announcements = activeAnnouncements,
                Events = upcomingEvents
            };

            
            return View(viewModel);
        }
    }
}