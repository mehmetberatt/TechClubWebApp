using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

using Microsoft.EntityFrameworkCore;
using TechClubWebApp.Data;
using TeknolojiKulubu.Models;

namespace TechClubWebApp.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Admin/Index
        public IActionResult Index()
        {
            return View();
        }

        // ==========================================
        // ANNOUNCEMENTS CRUD
        // ==========================================

        // GET: /Admin/Announcements
        public async Task<IActionResult> Announcements()
        {
            var announcements = await _context.Announcements.OrderByDescending(x => x.CreatedDate).ToListAsync();
            return View(announcements);
        }

        // GET: /Admin/CreateAnnouncement
        public IActionResult CreateAnnouncement()
        {
            return View();
        }

        // POST: /Admin/CreateAnnouncement
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAnnouncement(Announcement model, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "uploads");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }
                    model.ImageUrl = "/images/uploads/" + uniqueFileName;
                }

                model.CreatedDate = DateTime.Now;
                _context.Announcements.Add(model);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = "Yeni duyuru başarıyla eklendi.";
                return RedirectToAction(nameof(Announcements));
            }
            return View(model);
        }

        // GET: /Admin/EditAnnouncement/5
        public async Task<IActionResult> EditAnnouncement(int? id)
        {
            if (id == null) return NotFound();

            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement == null) return NotFound();

            return View(announcement);
        }

        // POST: /Admin/EditAnnouncement/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAnnouncement(int id, Announcement model, IFormFile? imageFile)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "uploads");
                        if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fileStream);
                        }

                        // Silme işlemi (isteğe bağlı, disk alanından tasarruf için eski resmi sil)
                        if (!string.IsNullOrEmpty(model.ImageUrl))
                        {
                            var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", model.ImageUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldPath)) System.IO.File.Exists(oldPath);
                        }

                        model.ImageUrl = "/images/uploads/" + uniqueFileName;
                    }

                    _context.Update(model);
                    await _context.SaveChangesAsync();
                    
                    TempData["SuccessMessage"] = "Duyuru başarıyla güncellendi.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnnouncementExists(model.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Announcements));
            }
            return View(model);
        }

        // POST: /Admin/DeleteAnnouncement/5
        [HttpPost, ActionName("DeleteAnnouncement")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement != null)
            {
                _context.Announcements.Remove(announcement);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Duyuru sistemden tamamen silindi.";
            }
            return RedirectToAction(nameof(Announcements));
        }

        private bool AnnouncementExists(int id)
        {
            return _context.Announcements.Any(e => e.Id == id);
        }

        // ==========================================
        // EVENTS CRUD
        // ==========================================

        // GET: /Admin/Events
        public async Task<IActionResult> Events()
        {
            var events = await _context.Events.OrderBy(x => x.EventDate).ToListAsync();
            return View(events);
        }

        // GET: /Admin/CreateEvent
        public IActionResult CreateEvent()
        {
            return View();
        }

        // POST: /Admin/CreateEvent
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEvent(Event model, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "uploads");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }
                    model.ImageUrl = "/images/uploads/" + uniqueFileName;
                }

                _context.Events.Add(model);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = "Yeni etkinlik başarıyla sisteme eklendi.";
                return RedirectToAction(nameof(Events));
            }
            return View(model);
        }

        // GET: /Admin/EditEvent/5
        public async Task<IActionResult> EditEvent(int? id)
        {
            if (id == null) return NotFound();

            var ev = await _context.Events.FindAsync(id);
            if (ev == null) return NotFound();

            return View(ev);
        }

        // POST: /Admin/EditEvent/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEvent(int id, Event model, IFormFile? imageFile)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "uploads");
                        if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fileStream);
                        }

                        if (!string.IsNullOrEmpty(model.ImageUrl))
                        {
                            var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", model.ImageUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldPath)) System.IO.File.Exists(oldPath);
                        }

                        model.ImageUrl = "/images/uploads/" + uniqueFileName;
                    }

                    _context.Update(model);
                    await _context.SaveChangesAsync();
                    
                    TempData["SuccessMessage"] = "Etkinlik detayları başarıyla güncellendi.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(model.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Events));
            }
            return View(model);
        }

        // POST: /Admin/DeleteEvent/5
        [HttpPost, ActionName("DeleteEvent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEventConfirmed(int id)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev != null)
            {
                _context.Events.Remove(ev);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Etkinlik sistemden silindi.";
            }
            return RedirectToAction(nameof(Events));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }

        // ==========================================
        // AUTHENTICATION
        // ==========================================

        // GET: /Admin/Login
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            // If already logged in, redirect to Dashboard
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Admin"); 
            }
            return View();
        }

        // POST: /Admin/Login
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Hardcoded credentials for ninja door setup
            if (username == "admin" && password == "123")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "Administrator")
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true // Remember me basically
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                // Redirect to a secure area after successful login.
                return RedirectToAction("Index", "Admin"); 
            }

            // If login fails
            ViewBag.ErrorMessage = "Hatalı kullanıcı adı veya şifre girdiniz.";
            return View();
        }

        // GET: /Admin/Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Admin");
        }
    }
}
