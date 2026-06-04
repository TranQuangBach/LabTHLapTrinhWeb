using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace webbangiay.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Vui lòng nhập Họ và Tên")]
        [MaxLength(100)]
        public string FullName { get; set; }

        [MaxLength(255)]
        public string Address { get; set; }
    }
}