using Microsoft.AspNetCore.Identity;

namespace ContactManagment.Models
{
    public class User: IdentityUser<int>
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual List<Contact> Contacts { get; set; } = new List<Contact>();
    }
}
