using System.ComponentModel.DataAnnotations;

namespace ContactManagment.DTOs
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
