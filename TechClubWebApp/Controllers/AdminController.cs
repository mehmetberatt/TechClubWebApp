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

        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> Announcements()
        {
            var announcements = await _context.Announcements.OrderByDescending(x => x.CreatedDate).ToListAsync();
            return View(announcements);
        }

        public IActionResult CreateAnnouncement()
        {
            return View();
        }

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

        public async Task<IActionResult> EditAnnouncement(int? id)
        {
            if (id == null) return NotFound();

            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement == null) return NotFound();

            return View(announcement);
        }


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


        public async Task<IActionResult> Events()
        {
            var events = await _context.Events.OrderBy(x => x.EventDate).ToListAsync();
            return View(events);
        }

        public IActionResult CreateEvent()
        {
            return View();
        }

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

        public async Task<IActionResult> EditEvent(int? id)
        {
            if (id == null) return NotFound();

            var ev = await _context.Events.FindAsync(id);
            if (ev == null) return NotFound();

            return View(ev);
        }

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

        public async Task<IActionResult> Banners()
        {
            var banners = await _context.Banners.OrderByDescending(x => x.Id).ToListAsync();
            return View(banners);
        }

        public IActionResult CreateBanner()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBanner(Banner model, IFormFile? imageFile)
        {
            if (string.IsNullOrEmpty(model.Title))
            {
                ModelState.AddModelError("Title", "Lütfen bir başlık giriniz.");
                return View(model);
            }

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
                    model.ImageUrl = "/images/uploads/" + uniqueFileName;
                }

                model.IsActive = true;

                _context.Banners.Add(model);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Yeni Banner eklendi.";
                return RedirectToAction(nameof(Banners));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Kayıt sırasında bir hata oluştu: " + ex.Message);
            }
            
            return View(model);
        }

        public async Task<IActionResult> EditBanner(int? id)
        {
            if (id == null) return NotFound();

            var banner = await _context.Banners.FindAsync(id);
            if (banner == null) return NotFound();

            return View(banner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBanner(int id, Banner model, IFormFile? imageFile)
        {
            if (id != model.Id) return NotFound();

            if (string.IsNullOrEmpty(model.Title))
            {
                ModelState.AddModelError("Title", "Lütfen bir başlık giriniz.");
                return View(model);
            }

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

                model.IsActive = true;

                _context.Update(model);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Banner güncellendi.";
                return RedirectToAction(nameof(Banners));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BannerExists(model.Id)) return NotFound();
                else throw;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Güncelleme sırasında bir hata oluştu: " + ex.Message);
            }

            return View(model);
        }

        [HttpPost, ActionName("DeleteBanner")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBannerConfirmed(int id)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner != null)
            {
                _context.Banners.Remove(banner);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Banner sistemden silindi.";
            }
            return RedirectToAction(nameof(Banners));
        }

        private bool BannerExists(int id)
        {
            return _context.Banners.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Admins()
        {
            var admins = await _context.Admins.OrderByDescending(x => x.CreatedDate).ToListAsync();
            return View(admins);
        }

        public IActionResult CreateAdmin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdmin(TechClubWebApp.Models.AdminUser model, string RawPassword)
        {
            ModelState.Remove("PasswordHash");

            if (ModelState.IsValid && !string.IsNullOrEmpty(RawPassword))
            {
                if (_context.Admins.Any(x => x.Username == model.Username))
                {
                    ModelState.AddModelError("Username", "Bu kullanıcı adı zaten mevcut.");
                    return View(model);
                }

                model.PasswordHash = HashPassword(RawPassword);
                model.CreatedDate = DateTime.Now;
                _context.Admins.Add(model);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = "Yeni yönetici başarıyla eklendi.";
                return RedirectToAction(nameof(Admins));
            }
            if (string.IsNullOrEmpty(RawPassword))
                ModelState.AddModelError("RawPassword", "Şifre alanı boş bırakılamaz.");

            return View(model);
        }

        public async Task<IActionResult> EditAdmin(int? id)
        {
            if (id == null) return NotFound();

            var admin = await _context.Admins.FindAsync(id);
            if (admin == null) return NotFound();

            return View(admin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAdmin(int id, TechClubWebApp.Models.AdminUser model, string? NewRawPassword)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var existingAdmin = await _context.Admins.FindAsync(id);
                if (existingAdmin == null) return NotFound();

                if (existingAdmin.Username != model.Username && _context.Admins.Any(x => x.Username == model.Username))
                {
                    ModelState.AddModelError("Username", "Bu kullanıcı adı zaten mevcut.");
                    return View(model);
                }

                existingAdmin.Username = model.Username;
                existingAdmin.Role = model.Role;

                if (!string.IsNullOrEmpty(NewRawPassword))
                {
                    existingAdmin.PasswordHash = HashPassword(NewRawPassword);
                }

                _context.Update(existingAdmin);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Yönetici başarıyla güncellendi.";
                return RedirectToAction(nameof(Admins));
            }
            return View(model);
        }

        [HttpPost, ActionName("DeleteAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAdminConfirmed(int id)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin != null)
            {
                if (_context.Admins.Count() <= 1)
                {
                    TempData["ErrorMessage"] = "Sistemde sadece 1 yönetici kaldı, onu silemezsiniz!";
                    return RedirectToAction(nameof(Admins));
                }

                _context.Admins.Remove(admin);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Yönetici silindi.";
            }
            return RedirectToAction(nameof(Admins));
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Admin"); 
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMessage = "Lütfen kullanıcı adı ve şifre giriniz.";
                return View();
            }

            var hashedPwd = HashPassword(password);
            var adminUser = await _context.Admins.FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == hashedPwd);

            if (adminUser != null)
            {
                var claims = new List<System.Security.Claims.Claim>
                {
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, adminUser.Username),
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, adminUser.Role ?? "Administrator")
                };

                var claimsIdentity = new System.Security.Claims.ClaimsIdentity(
                    claims, Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new Microsoft.AspNetCore.Authentication.AuthenticationProperties
                {
                    IsPersistent = true 
                };

                await HttpContext.SignInAsync(
                    Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme,
                    new System.Security.Claims.ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("Index", "Admin"); 
            }

            ViewBag.ErrorMessage = "Hatalı kullanıcı adı veya şifre girdiniz.";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Admin");
        }

        private string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) return string.Empty;
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
