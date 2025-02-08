using Microsoft.AspNetCore.Identity;

namespace ProductsAPI.Model
{
    public class AppUser: IdentityUser<int>
    {
        public string FullName { get; set; } = null!;

        public DateTime DateAdded { get; set; } 
    }
}