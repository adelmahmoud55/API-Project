using LinkDev.Talabat.Core.Application.Abstaction.Models.Auth;
using LinkDev.Talabat.Core.Application.Abstaction.Services.Auth;
using LinkDev.Talabat.Core.Application.Services.Auth;
using LinkDev.Talabat.Core.Domain.Entities.Identity;
using LinkDev.Talabat.Infrastructure.Persistence.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using System.Text;

namespace LinkDev.Talabat.APIs.Extensions
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection Services,IConfiguration configuration)
        {

            Services.Configure<JwtSettings>(configuration.GetSection("jwtSettings"));

            //Services.AddIdentity<ApplicationUser, IdentityRole>(); // THis overload add default identity config for the specified user and role types
            Services.AddIdentity<ApplicationUser, IdentityRole>((identityOptions) =>   //this overload to customize the identity options "change config"
            {

                identityOptions.SignIn.RequireConfirmedAccount = true;
                identityOptions.SignIn.RequireConfirmedEmail = true;
                identityOptions.SignIn.RequireConfirmedPhoneNumber = true;


                // best practice to use regular expression to validate the password, in the Registeration page regiterDto
                //identityOptions.Password.RequireDigit = true;
                //identityOptions.Password.RequireLowercase = true;
                //identityOptions.Password.RequireUppercase = true;
                //identityOptions.Password.RequireNonAlphanumeric = true; //$#@%
                //identityOptions.Password.RequiredLength = 8;
                //identityOptions.Password.RequiredUniqueChars = 2; // at least two numbers not repeated in the password.

                identityOptions.User.RequireUniqueEmail = true;
                //identityOptions.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

                identityOptions.Lockout.AllowedForNewUsers = true;
                identityOptions.Lockout.MaxFailedAccessAttempts = 5;
                identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            })
                //here we add data access mechanism(create,update,delete,....) for the identity tables, whya aslun Entityframework 3shan gwaha code EF wlazm t7ddlha hya htklm which DbContext aknk sh8al f el Generic Repository.
                .AddEntityFrameworkStores<StoreIdentityDbContext>();  //If you don’t call .AddEntityFrameworkStores<StoreIdentityDbContext>(), Identity would not know how to store or retrieve its data. Even though services like UserManager and RoleManager are registered, they won’t be able to access the underlying database, and you would need to provide an alternative storage mechanism, this default implementation is provided by the Entity Framework Core package. we can use AdduserStore or AddRoleStore to use custom store for the user or role tables.


            // Factory method 
            Services.AddScoped(typeof(IAuthService),typeof(AuthService)); //register the AuthService in the DI container, so that it can be injected in the controllers
            Services.AddScoped(typeof(Func<IAuthService>),(ServiceProvider) =>
            { 
             return () => ServiceProvider.GetService<IAuthService>();

            });

            Services.AddAuthentication((authenticationOptions) =>
            {
                authenticationOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; //default authentication scheme, which is JwtBearer by default.
                authenticationOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; //default challenge scheme, which is JwtBearer by default.


            })
                .AddJwtBearer(configureOptions =>
                { //handler for authrization scheme, which is authrization and its the default scheme i ve made it for authentication in the application.
                    configureOptions.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        

                        ClockSkew = TimeSpan.Zero, // to disable the default 5 minutes clock skew for the token expiration time, so that the token will expire exactly at the specified time.
                        ValidIssuer = configuration["jwtSettings:Issuer"],
                        ValidAudience = configuration["jwtSettings:Audiance"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwtSettings:Key"]!))
                    };
                });

            return Services;
        }
    }
}
