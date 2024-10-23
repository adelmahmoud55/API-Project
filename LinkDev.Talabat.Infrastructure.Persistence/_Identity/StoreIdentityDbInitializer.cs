using LinkDev.Talabat.Core.Domain.Contracts.Persistence.DbInitializer;
using LinkDev.Talabat.Core.Domain.Entities.Identity;
using LinkDev.Talabat.Infrastructure.Persistence.Comman;
using LinkDev.Talabat.Infrastructure.Persistence.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence.Identity
{
    public sealed class StoreIdentityDbInitializer(StoreIdentityDbContext _dbContext,UserManager<ApplicationUser> _userManager) :DbInitializer(_dbContext), IStoreIdentityDbInitializer
    {
        public override async Task SeedAsync()
        {
            // just keep it simple by add one user , no need jason file

            var user = new ApplicationUser()
            { 
                DisplayName = "Adel Mahmoud",       
                UserName = "adel.mahmoud",
                Email = "adel.mahmoud@gmail.com",
                PhoneNumber = "01000000000",
            };

            await _userManager.CreateAsync(user, "P@ssw0rd");

            // we cannot add the user throwgh _dbcontext object like we did in the StoreDbInitializer ,
            // because the user manager is responsible for hashing the password and adding the user to the database; userManager comes from Identity package
        }

    }
}
