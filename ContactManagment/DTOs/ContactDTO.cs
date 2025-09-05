using System.ComponentModel.DataAnnotations;

namespace ContactManagment.DTOs
{
    public class ContactDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(80)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(80)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(13)]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(120)]
        [EmailAddress]
        public string Email { get; set; }


        public DateTime? Birthdate { get; set; }
    }
}
