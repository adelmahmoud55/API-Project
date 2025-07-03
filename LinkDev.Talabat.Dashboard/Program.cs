using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using LinkDev.Talabat.Core.Domain.Entities.Identity;
using LinkDev.Talabat.Infrastructure.Persistence.Data;
using LinkDev.Talabat.Infrastructure.Persistence.Identity;
using LinkDev.Talabat.Infrastructure.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.DashBoard.Helpers;

namespace LinkDev.Talabat.Dashboard
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<StoreDbContext>((serviceProvider, optionsBuilder) =>
            {
                optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(builder.Configuration.GetConnectionString("StoreContext"));
               
            }/*,contextLifetime: ServiceLifetime.Scoped,optionsLifetime: ServiceLifetime.Scoped */);

            //builder.Services.AddSingleton(typeof(IConnectionMultiplexer), (serviceProvider) =>
            //{
            //    var connectionString = builder.Configuration.GetConnectionString("Redis");
            //    var connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString!);
            //    return connectionMultiplexer;
            //});

            builder.Services.AddDbContext<StoreIdentityDbContext>(optionsBuilder =>
            {
                optionsBuilder.
                UseSqlServer(builder.Configuration.GetConnectionString("IdentityContext"));
            }/*,contextLifetime: ServiceLifetime.Scoped,optionsLifetime: ServiceLifetime.Scoped */);
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>((identityOptions) =>   //this overload to customize the identity options "change config"
            {

                //identityOptions.SignIn.RequireConfirmedAccount = true;
                //identityOptions.SignIn.RequireConfirmedEmail = true;
                //identityOptions.SignIn.RequireConfirmedPhoneNumber = true;


                // best practice to use regular expression to validate the password, in the Registeration page regiterDto
                identityOptions.Password.RequireDigit = true;
                identityOptions.Password.RequireLowercase = true;
                identityOptions.Password.RequireUppercase = true;
                identityOptions.Password.RequireNonAlphanumeric = true; //$#@%
                identityOptions.Password.RequiredLength = 8;
                //identityOptions.Password.RequiredUniqueChars = 2; // at least two numbers not repeated in the password.

                //identityOptions.User.RequireUniqueEmail = true;
                ////identityOptions.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

                //identityOptions.Lockout.AllowedForNewUsers = true;
                //identityOptions.Lockout.MaxFailedAccessAttempts = 5;
                //identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            })
                //here we add data access mechanism(create,update,delete,....) for the identity tables, whya aslun Entityframework 3shan gwaha code EF wlazm t7ddlha hya htklm which DbContext aknk sh8al f el Generic Repository.
                .AddEntityFrameworkStores<StoreIdentityDbContext>();
            builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            builder.Services.AddAutoMapper(typeof(MapsProfile));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Admin}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
