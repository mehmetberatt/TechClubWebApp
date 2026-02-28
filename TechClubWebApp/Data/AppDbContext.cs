using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TechClubWebApp.Models;
using TeknolojiKulubu.Models;

namespace TechClubWebApp.Data
{
    public class AppDbContext : DbContext // inheritance miras aldık
    {
        // Kurulum ayarlarını içeri alan yapıcı metot (Constructor)
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSet komutları, yazdığımız C# sınıflarını SQL veritabanındaki fiziksel tablolara dönüştürür.
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Event> Events { get; set; }
    }
}