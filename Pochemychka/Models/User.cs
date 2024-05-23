using Microsoft.AspNetCore.Identity;

namespace Pochemychka.Models
{
    public class User : IdentityUser
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
    }
}
