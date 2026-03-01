using System;

namespace TeknolojiKulubu.Models
{
    public class Event
    {
        public int Id { get; set; } // Benzersiz kimlik numarası.
        public string Title { get; set; } // Etkinlik Adı
        public string Description { get; set; } // Etkinlik Detayı
        public DateTime EventDate { get; set; } // Etkinliğin yapılacağı tarih ve saat
        public string Location { get; set; } // Nerede yapılacak?
        public string? ImageUrl { get; set; } // Etkinliğin afişi (Nullable)
    }
}