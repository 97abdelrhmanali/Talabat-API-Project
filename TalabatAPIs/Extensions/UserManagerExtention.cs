﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Entities.Identity;

namespace TalabatAPIs.Extensions
{
    public static class UserManagerExtention
    {
        public static async Task<AppUser> FindUserWithAddressAsync(this UserManager<AppUser> userManager ,ClaimsPrincipal User)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.Users.Include(u => u.Address).SingleOrDefaultAsync(u => u.Email == email);
            return user;
        }
    }
}
