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

            if (!_userManager.Users.Any()) // we use this condition cuz we use seeding for the first time only
            {
                var user = new ApplicationUser()
                {
                    DisplayName = "Adel Mahmoud",
                    UserName = "adel.mahmoud",
                    Email = "adel.mahmoud@gmail.com",
                    PhoneNumber = "01000000000",
                };

                await _userManager.CreateAsync(user, "P@ssw0rd");

            }
            // we cannot add the user throwgh _dbcontext object like we did in the StoreDbInitializer ,
            // because the user manager is responsible for hashing the password and adding the user to the database; userManager comes from Identity package
            // thre main sevices in Identity package are RoleManager and UserManager, sign in manager, and user claims principal factory in the identity package
            // so we can create the user and add it to the database using the user manager
            // sp we need to inject the user manager in the StoreIdentityDbInitializer constructor
            //  w 5li balk hena msh ht3rf twsl aslun l sign in manager , enta btwslhaf el higher layer zy presentation layer, api layer, or web layer
            // 3shan ana msh m7tag a3ml sign in f el db initializer, f hwa dotnet msh hy5lek twslha aslun r8m enk 3aml install f el identity package
            // 3shan kda by7a22 el separation of concerns , en kol layer bt3ml 7aga mo3yna , hena e7na f lower layer msh h3ml sign in
        }

    }
}
