using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    { 
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var User = new AppUser()
                {
                    DisplayName = "Dalia Mahmoud",
                    Email = "dalia.m3544214@gmail.com",
                    UserName = "dalia",
                    PhoneNumber = "01095200308"
                };
                await userManager.CreateAsync(User, "Pa$$w0rd");
            }  
        }

    }
}
