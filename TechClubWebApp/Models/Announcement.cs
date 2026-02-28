using System;

namespace TeknolojiKulubu.Models
{
    public class Announcement
    {
        public int Id { get; set; } // Benzersiz kimlik yapısı
        public string Title { get; set; } // Duyuru Başlığı
        public string Content { get; set; } // Duyuru İçeriği/Detayı
        public DateTime CreatedDate { get; set; } // yayınlandığı tarih ve saat
        public bool IsActive { get; set; } // true false duyuru yayında mı sorusuna karşılık
    }
}