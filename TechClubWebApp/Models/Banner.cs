using System;

namespace TeknolojiKulubu.Models
{
    public class Banner
    {
        public int Id { get; set; } // Her kaydın benzersiz kimliği (Primary Key)
        public string Title { get; set; } = string.Empty; // Banner üzerindeki büyük yazı
        public string? Description { get; set; } // Banner altındaki açıklama yazısı
        public string? ImageUrl { get; set; } // Görselin dosya yolu
        public string? LinkUrl { get; set; } // Butona tıklanınca gidilecek link 
        public bool IsActive { get; set; } // Sitede görünsün mü, gizlensin mi? (true-false)
        public int DisplayOrder { get; set; } // Hangi sırada gösterileceği 123...
    }
}