using System.ComponentModel.DataAnnotations;

namespace ContactManagment.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Full Name Is Required")]
        [MaxLength(70)]
        public string FullName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Format")]
        [Required]
        [MaxLength(150)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
