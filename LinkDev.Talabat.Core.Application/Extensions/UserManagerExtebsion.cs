using LinkDev.Talabat.Core.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Extensions
{
    internal static class UserManagerExtebsion
    {

        // this to eagrly loading the address instead of using FindByEmailAsync which is working lazy loading , means htrg3 el data f el request w el related data htrg3ha f another request (two quers)
        // lakn hena query wa7d , hy3ml left join lel user m3 table el address
        public static async Task<ApplicationUser?> FindUserWithAddress(this UserManager<ApplicationUser> userManager, ClaimsPrincipal claimsPrincipal)
        {
            var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);

            var user =  await userManager.Users
                .Where(uuser=> uuser.Email == email)
                .Include(user => user.Address)
                .FirstOrDefaultAsync();

            return user;
        }
    }
}
