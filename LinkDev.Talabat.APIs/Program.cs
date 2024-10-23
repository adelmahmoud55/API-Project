using LinkDev.Talabat.Core.Application;
using LinkDev.Talabat.APIs.Extensions;
using LinkDev.Talabat.APIs.Services;
using LinkDev.Talabat.Core.Application.Abstaction;
using LinkDev.Talabat.Core.Domain.Contracts;
using LinkDev.Talabat.Infrastructure.Persistence;
using LinkDev.Talabat.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using LinkDev.Talabat.APIs.Controllers.Errors;
using LinkDev.Talabat.APIs.Middlewares;
using LinkDev.Talabat.Infrastructure;
using LinkDev.Talabat.Core.Application.Models.Products;
using LinkDev.Talabat.Core.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using LinkDev.Talabat.Infrastructure.Persistence.Identity;
namespace LinkDev.Talabat.APIs
{
    public class Program
    {
        // Entry point for the application.
        public static async Task Main(string[] args)
        {
            var webApplicationBuilder = WebApplication.CreateBuilder(args);


            #region Configure Services

            // Add services to the container.

            //AddApplicationPart is used to add the controllers to the DI container. and to tell that the controllers are in the same assembly as the AssemblyInformation class.
            webApplicationBuilder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(options => // this handling the validation errors in the model binding across the application.
                {
                    // e7na bnsh8l el model state 3shan msh f kol end point hro7 check 3la if(!ModelState.IsValid) w nrg3 response 400 bad request
                    options.SuppressModelStateInvalidFilter = false; // to disable the default behavior of returning 400 bad request when the model is invalid.
                    options.InvalidModelStateResponseFactory = actioncontext =>
                    {
                        var errors = actioncontext.ModelState //modelState is a dictionary of key value pairs , key is the name of the property and value is the error message, contains all the properties for the endpoint that has been called.
                            .Where(e => e.Value!.Errors.Count > 0) // get all the properties that have errors.
                           .Select(P => new ApiValidationErrorResponse.ValidationError()
                           {
                               Filed = P.Key,
                               Errors = P.Value!.Errors.Select(E => E.ErrorMessage)

                           });

                        var errorResponse = new ApiValidationErrorResponse("Validation Error")
                        {
                            Errors = errors
                        };

                        return new BadRequestObjectResult(errorResponse); //we cannot use hellepr method like BadRequest() or NotFound() because we are not in the controller. there is no [ApiController] attribute.
                    };
                })
                .AddApplicationPart(typeof(LinkDev.Talabat.APIs.Controllers.AssemblyInformation).Assembly); // Register Controllers To DI Container.
                                                                                                            // Register Required Services By ASP.NET Core Web APIs To Di Container.



            // we dont need it as we use ConfigureApiBehaviorOption
            ///webApplicationBuilder.Services.Configure<ApiBehaviorOptions>(options =>
            ///{
            ///    options.SuppressModelStateInvalidFilter = false;
            ///    options.InvalidModelStateResponseFactory = actionContext =>
            ///    {
            ///        var errors = actionContext.ModelState
            ///            .Where(e => e.Value.Errors.Count > 0)
            ///            .SelectMany(x => x.Value.Errors)
            ///            .Select(x => x.ErrorMessage).ToArray();
            ///
            ///
            ///
            ///        var errorResponse = new ApiValidationErrorResponse("Validation Error") 
            ///        {
            ///            Errors = errors
            ///        };
            ///        return new BadRequestObjectResult(errorResponse);
            ///    };
            ///
            ///});    


            /// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            webApplicationBuilder.Services.AddEndpointsApiExplorer();
            webApplicationBuilder.Services.AddSwaggerGen();


            webApplicationBuilder.Services.AddPersistenceServices(webApplicationBuilder.Configuration);
            //DependencyInjection.AddPersistenceServices(webApplicationBuilder.Services, webApplicationBuilder.Configuration); // Register Persistence Services To DI Container.

            webApplicationBuilder.Services.AddApplicationServices(); // Register Application Services To DI Container.

            //webApplicationBuilder.Services.AddScoped(typeof(IHttpContextAccessor), typeof(HttpContextAccessor));
            webApplicationBuilder.Services.AddHttpContextAccessor(); // IHttpContextAccessor depend on another service , so u have to use this method , not only register the IHttpContextAccessor as above 

            webApplicationBuilder.Services.AddScoped(typeof(ILoggedInUserService), typeof(LoggedInUserService)); // Register LoggedInUserService To DI Container.

            webApplicationBuilder.Services.AddInfrastructureServices(webApplicationBuilder.Configuration);


            //webApplicationBuilder.Services.AddIdentity<ApplicationUser, IdentityRole>(); // THis overload add default identity config for the specified user and role types
            webApplicationBuilder.Services.AddIdentity<ApplicationUser, IdentityRole>((identityOptions) =>   //this overload to customize the identity options "change config"
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
                identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                identityOptions.Lockout.MaxFailedAccessAttempts = 5;
            })
                //here we add data access mechanism.
                .AddEntityFrameworkStores<StoreIdentityDbContext>();  //If you don’t call .AddEntityFrameworkStores<StoreIdentityDbContext>(), Identity would not know how to store or retrieve its data. Even though services like UserManager and RoleManager are registered, they won’t be able to access the underlying database, and you would need to provide an alternative storage mechanism.




            #endregion

            var app = webApplicationBuilder.Build();



            #region Update DataBase and Data Seeding

            await app.InitializeDbAsync();

            #endregion



            #region Configure Kestrel Middlewares

            // first middleware the request will go through ,  and the last middleware the response will go through.
            app.UseMiddleware<ExceptionHandlerMiddleware>(); // to handle the exceptions that are thrown from the application.

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                //app.UseDeveloperExceptionPage(); this used internally from .net 6 , so no need to add it. means return the exception as a response.
            }

            app.UseHttpsRedirection();

            //to handle not found requests
            app.UseStatusCodePagesWithReExecute("/Error/{0}"); // to redirect the request to the errors controller when the status code is not 200.

            app.UseAuthorization();


            app.UseStaticFiles(); // to allow kestrel to serve the requests that ask for any static file like from wwwroot.
                                  // enable static file serving for the current request path {current : wwwroot path}

            app.MapControllers();

            #endregion




            app.Run();





        }
    }
}
