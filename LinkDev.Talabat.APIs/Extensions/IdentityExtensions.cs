using LinkDev.Talabat.Core.Domain.Entities.Identity;
using LinkDev.Talabat.Infrastructure.Persistence.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;

namespace LinkDev.Talabat.APIs.Extensions
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection Services)
        {
            //Services.AddIdentity<ApplicationUser, IdentityRole>(); // THis overload add default identity config for the specified user and role types
            Services.AddIdentity<ApplicationUser, IdentityRole>((identityOptions) =>   //this overload to customize the identity options "change config"
            {

                identityOptions.SignIn.RequireConfirmedAccount = true;
                identityOptions.SignIn.RequireConfirmedEmail = true;
                identityOptions.SignIn.RequireConfirmedPhoneNumber = true;

                identityOptions.Password.RequireDigit = true;
                identityOptions.Password.RequireLowercase = true;
                identityOptions.Password.RequireUppercase = true;
                identityOptions.Password.RequireNonAlphanumeric = true; //$#@%
                identityOptions.Password.RequiredLength = 8;
                identityOptions.Password.RequiredUniqueChars = 2; // at least two numbers not repeated in the password.

                identityOptions.User.RequireUniqueEmail = true;
                //identityOptions.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

                identityOptions.Lockout.AllowedForNewUsers = true;
                identityOptions.Lockout.MaxFailedAccessAttempts = 5;
                identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            })
                //here we add data access mechanism(create,update,delete,....) for the identity tables, whya aslun Entityframework 3shan gwaha code EF wlazm t7ddlha hya htklm which DbContext aknk sh8al f el Generic Repository.
                .AddEntityFrameworkStores<StoreIdentityDbContext>();  //If you don’t call .AddEntityFrameworkStores<StoreIdentityDbContext>(), Identity would not know how to store or retrieve its data. Even though services like UserManager and RoleManager are registered, they won’t be able to access the underlying database, and you would need to provide an alternative storage mechanism, this default implementation is provided by the Entity Framework Core package. we can use AdduserStore or AddRoleStore to use custom store for the user or role tables.

            return Services;
        }
    }
}
