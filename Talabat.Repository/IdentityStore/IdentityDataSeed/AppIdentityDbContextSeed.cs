using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.IdentityStore.IdentityDataSeed
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> _userManager)
        {
            if(_userManager.Users.Count() == 0)
            {
                var User = new AppUser
                {
                    DisplayName = "Ahmed Nasr",
                    Email = "ahmed.nasr@linkdev.com",
                    UserName = "Ahmed.Nasr",
                    PhoneNumber= "0123456789"
                };

                await _userManager.CreateAsync(User, "Pa$$w0rd");
            }
        }
    }
}
