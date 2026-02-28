using TechClubWebApp.Models;
using TeknolojiKulubu.Models;

namespace TechClubWebApp.ViewModels
{
    public class HomeViewModel
    {
        public List<Banner> Banners { get; set; }
        public List<Announcement> Announcements { get; set; }
        public List<Event> Events { get; set; }
    }
}