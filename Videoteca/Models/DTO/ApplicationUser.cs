using Microsoft.AspNetCore.Identity;

namespace Videoteca.Models.DTO
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string? ProfilePicture { get; set; }
    }
}
