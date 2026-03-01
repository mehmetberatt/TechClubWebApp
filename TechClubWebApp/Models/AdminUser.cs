using System;
using System.ComponentModel.DataAnnotations;

namespace TechClubWebApp.Models
{
    public class AdminUser
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Kullanıcı adı 3 ila 50 karakter arasında olmalıdır.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        public string PasswordHash { get; set; }

        public string Role { get; set; } = "Administrator";

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
