using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Entites.Identity;


namespace Talabat.APIs.Extensions

{
    public static class UserManagerExtension
    {
        public static async Task<AppUser?> FindUserWithAddressAsync(this UserManager<AppUser> userManager , ClaimsPrincipal User)
        {
            var email= User.FindFirstValue(ClaimTypes.Email);
            var user= await userManager.Users.Where(U=>U.Email == email).Include(U=>U.Address).FirstOrDefaultAsync(User=>User.Email==email);
            return user; 
        }
    }
}
