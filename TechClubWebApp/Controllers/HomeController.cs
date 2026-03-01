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

        public IActionResult AnnouncementDetail(int id)
        {
            var announcement = _context.Announcements.FirstOrDefault(a => a.Id == id && a.IsActive);
            if (announcement == null)
            {
                return NotFound();
            }
            return View(announcement);
        }

        public IActionResult EventDetail(int id)
        {
            var ev = _context.Events.FirstOrDefault(e => e.Id == id);
            if (ev == null)
            {
                return NotFound();
            }
            return View(ev);
        }

        public IActionResult AllEvents()
        {
            var events = _context.Events.OrderBy(e => e.EventDate).ToList();
            return View(events);
        }

        public IActionResult AllAnnouncements()
        {
            var announcements = _context.Announcements.Where(a => a.IsActive).OrderByDescending(a => a.CreatedDate).ToList();
            return View(announcements);
        }

        public IActionResult Career()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Services()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}